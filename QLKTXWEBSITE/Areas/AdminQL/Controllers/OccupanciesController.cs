using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLKTXWEBSITE.Models;

namespace QLKTXWEBSITE.Areas.AdminQL.Controllers
{
    //[Area("AdminQL")]
    public class OccupanciesController : BaseController
    {
        private readonly QlktxContext _context;

        public OccupanciesController(QlktxContext context)
        {
            _context = context;
        }

        // GET: AdminQL/Occupancies
        public async Task<IActionResult> Index(string name, string room, string status)
        {
            IQueryable<Occupancy> occupancies = _context.Occupancies.Include(o => o.Student).Include(o => o.Room);

            if (!string.IsNullOrEmpty(name))
            {
                occupancies = occupancies.Where(o => o.Student.FullName.Contains(name));
            }

            if (!string.IsNullOrEmpty(status))
            {
                if (status == "paid")
                {
                    occupancies = occupancies.Where(o => o.Status == true);
                }
                else if (status == "unpaid")
                {
                    occupancies = occupancies.Where(o => o.Status == false);
                }
            }

            return View(await occupancies.ToListAsync());
        }


        // GET: AdminQL/Occupancies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Occupancies == null)
            {
                return NotFound();
            }

            var occupancy = await _context.Occupancies
                .Include(o => o.Room)
                .Include(o => o.Student)
                .FirstOrDefaultAsync(m => m.OccupancyId == id);
            if (occupancy == null)
            {
                return NotFound();
            }

            return View(occupancy);
        }

        // GET: AdminQL/Occupancies/Create
        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "NumberRoom");
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "FullName");
            return View();
        }

        // POST: AdminQL/Occupancies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OccupancyId,StudentId,RoomId,RenewalDate,ExpirationDate,CycleMonths,Status")] Occupancy occupancy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(occupancy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "NumberRoom", occupancy.RoomId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "FullName", occupancy.StudentId);
            return View(occupancy);
        }

        // GET: AdminQL/Occupancies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Occupancies == null)
            {
                return NotFound();
            }

            var occupancy = await _context.Occupancies.FindAsync(id);
            if (occupancy == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "NumberRoom", occupancy.RoomId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "FullName", occupancy.StudentId);
            return View(occupancy);
        }

        // POST: AdminQL/Occupancies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OccupancyId,StudentId,RoomId,RenewalDate,ExpirationDate,CycleMonths,Status")] Occupancy occupancy)
        {
            if (id != occupancy.OccupancyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(occupancy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OccupancyExists(occupancy.OccupancyId))
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
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "NumberRoom", occupancy.RoomId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "FullName", occupancy.StudentId);
            return View(occupancy);
        }

        // GET: AdminQL/Occupancies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Occupancies == null)
            {
                return NotFound();
            }

            var occupancy = await _context.Occupancies
                .Include(o => o.Room)
                .Include(o => o.Student)
                .FirstOrDefaultAsync(m => m.OccupancyId == id);
            if (occupancy == null)
            {
                return NotFound();
            }

            return View(occupancy);
        }

        // POST: AdminQL/Occupancies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Occupancies == null)
            {
                return Problem("Entity set 'QlktxContext.Occupancies'  is null.");
            }
            var occupancy = await _context.Occupancies.FindAsync(id);
            if (occupancy != null)
            {
                _context.Occupancies.Remove(occupancy);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OccupancyExists(int id)
        {
          return (_context.Occupancies?.Any(e => e.OccupancyId == id)).GetValueOrDefault();
        }
        // POST: AdminQL/Occupancies/SendEmailToOccupants
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendEmailToOccupants()
        {
            try
            {
                var occupants = _context.Occupancies
                    .Include(o => o.Student) // Đảm bảo rằng thông tin của sinh viên được bao gồm
                    .Where(o => o.Status == false)
                    .ToList();

                // Gửi email cho từng cư dân trong danh sách
                foreach (var occupant in occupants)
                {
                    string email = occupant.Student.Email;
                    string subject = "Ký túc xá trường đại học tài nguyên và môi trường";
                    string studentCode = occupant.Student.StudentCode;
                    var listName = _context.Students.FirstOrDefault(c => c.StudentCode.Equals(studentCode));
                    string name = !string.IsNullOrEmpty(listName.FullName) ? listName.FullName : studentCode;
                    string htmlBody = "<div>" +
                                        "<h2>Dear " + name + "!</h2>" +
                                        "<p>Mời bạn vào trang web ký túc xá để gia hạn phòng!</p>" +
                                        "<a href=\"#\">Nhấp vào đây để tới trang web</a>" +
                                    "</div>";
                    SendEmail(email, subject, htmlBody);
                }

                ViewBag.Message = "Email đã được gửi thành công đến tất cả cư dân.";
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Đã xảy ra lỗi khi gửi email: " + ex.Message;
            }

            return View();
        }

        // Phương thức gửi email
        private void SendEmail(string email, string subject, string body)
        {
            var fromAddress = new MailAddress("21111064572@hunre.edu.vn");
            var toAddress = new MailAddress(email);
            const string fromPassword = "Danli29.03";
            string host = "smtp.gmail.com";
            int port = 587;

            var smtp = new SmtpClient
            {
                Host = host,
                Port = port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }
        public IActionResult GetRoomsByBuilding(string building)
        {
            var rooms = _context.Rooms.Where(r => r.Building == building)
                                       .Select(r => new { value = r.RoomId, text = r.NumberRoom })
                                       .ToList();
            return Json(rooms);
        }
        public IActionResult GetStudentsByRoom(int roomId)
        {
            var students = _context.Students.Where(s => s.RoomId == roomId)
                                            .Select(s => new { value = s.StudentId, text = s.FullName })
                                            .ToList();
            return Json(students);
        }

    }
}
