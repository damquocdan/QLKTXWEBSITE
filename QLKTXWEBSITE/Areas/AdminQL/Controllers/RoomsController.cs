using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLKTXWEBSITE.Models;

namespace QLKTXWEBSITE.Areas.AdminQL.Controllers
{
    public class RoomsController : BaseController
    {
        private readonly QlktxContext _context;

        public RoomsController(QlktxContext context)
        {
            _context = context;
        }

        // GET: AdminQL/Rooms
        public async Task<IActionResult> Index(string mowroom, string building, string numberRoom, string status)
        {
            var rooms = _context.Rooms.AsQueryable();

            // Kiểm tra và áp dụng các điều kiện tìm kiếm
            if (!string.IsNullOrEmpty(mowroom))
            {
                rooms = rooms.Where(r => r.Mowroom == mowroom);
            }

            if (!string.IsNullOrEmpty(building))
            {
                rooms = rooms.Where(r => r.Building == building);
            }

            if (!string.IsNullOrEmpty(numberRoom))
            {
                int roomNumber;
                if (int.TryParse(numberRoom, out roomNumber))
                {
                    rooms = rooms.Where(r => r.NumberRoom == roomNumber);
                }
            }

            if (!string.IsNullOrEmpty(status))
            {
                bool isFull = status == "True"; // Kiểm tra nếu status là "True" (đầy) thì isFull là true, ngược lại là false

                if (isFull)
                {
                    rooms = rooms.Where(r => r.NumberOfStudents == r.BedNumber);
                }
                else
                {
                    rooms = rooms.Where(r => r.NumberOfStudents < r.BedNumber);
                }
            }


            // Thực hiện truy vấn để lấy dữ liệu phòng
            var roomList = rooms.Select(room => new Room
            {
                Mowroom = room.Mowroom,
                Building = room.Building,
                Floor = room.Floor,
                BedNumber = room.BedNumber,
                RoomId = room.RoomId,
                NumberRoom = room.NumberRoom,
                Status = room.Status,
                NumberOfStudents = _context.Students.Count(student => student.RoomId == room.RoomId)
            }).ToList();

            return View(roomList);
        }


        public async Task<IActionResult> BedList(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .Include(r => r.BedOfRooms)
                .FirstOrDefaultAsync(r => r.RoomId == id);

            if (room == null)
            {
                return NotFound();
            }

            return View(room.BedOfRooms);
        }
        // GET: AdminQL/Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            // Lấy số lượng sinh viên trong phòng
            var numberOfStudents = _context.Students.Count(student => student.RoomId == room.RoomId);
            if (numberOfStudents == room.BedNumber)
            {
                ViewData["Status"] = "Đầy";
            }
            else
            {
                ViewData["Status"] = "Trống";
            }
            // Thêm số lượng sinh viên vào ViewData để truyền vào view
            ViewData["NumberOfStudents"] = numberOfStudents;

            return View(room);
        }


        // GET: AdminQL/Rooms/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminQL/Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,Mowroom,Building,Floor,NumberRoom,BedNumber,NumberOfStudents,Status")] Room room)
        {
            // Kiểm tra xem phòng đã tồn tại hay không
            var existingRoom = await _context.Rooms.FirstOrDefaultAsync(r => r.Building == room.Building && r.NumberRoom == room.NumberRoom);
            if (existingRoom != null)
            {
                // Nếu phòng đã tồn tại, thì hiển thị thông báo lỗi
                ModelState.AddModelError(string.Empty, "Phòng đã tồn tại.");
                return View(room);
            }

            // Tiếp tục quá trình tạo phòng mới
            if (ModelState.IsValid)
            {
                _context.Add(room);
                await _context.SaveChangesAsync();

                // Tạo các giường tương ứng với số lượng BedNumber
                for (int i = 0; i < room.BedNumber; i++)
                {
                    var bed = new BedOfRoom
                    {
                        RoomId = room.RoomId,
                        NumberBed = i + 1, // Số giường bắt đầu từ 1
                        Status = false // Mặc định giường chưa được sử dụng
                    };
                    _context.Add(bed);
                }
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }


        // GET: AdminQL/Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            // Lấy số lượng sinh viên trong phòng
            var numberOfStudents = _context.Students.Count(student => student.RoomId == room.RoomId);
            
            // Thêm số lượng sinh viên vào ViewData để truyền vào view
            ViewData["NumberOfStudents"] = numberOfStudents;

            return View(room);
        }


        // POST: AdminQL/Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RoomId,Mowroom,Building,Floor,NumberRoom,BedNumber,NumberOfStudents,Status")] Room room)
        {
            if (id != room.RoomId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(room);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoomExists(room.RoomId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(room);
        }

        // GET: AdminQL/Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }
            var numberOfStudents = _context.Students.Count(student => student.RoomId == room.RoomId);
            if (numberOfStudents == room.BedNumber)
            {
                ViewData["Status"] = true;

            }
            else
            {
                ViewData["Status"] = false;
            }
            // Thêm số lượng sinh viên vào ViewData để truyền vào view
            ViewData["NumberOfStudents"] = numberOfStudents;
            return View(room);
        }

        // POST: AdminQL/Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Rooms == null)
            {
                return Problem("Entity set 'QlktxContext.Rooms' is null.");
            }

            var room = await _context.Rooms.FindAsync(id);
            if (room == null)
            {
                return NotFound();
            }

            // Tìm và xoá các giường của phòng
            var bedsToDelete = await _context.BedOfRooms.Where(bed => bed.RoomId == id).ToListAsync();
            _context.BedOfRooms.RemoveRange(bedsToDelete);

            // Tiến hành xoá phòng
            _context.Rooms.Remove(room);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool RoomExists(int id)
        {
          return (_context.Rooms?.Any(e => e.RoomId == id)).GetValueOrDefault();
        }
    }
}
