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
    //[Area("AdminQL")]
    public class TransactionsController : BaseController
    {
        private readonly QlktxContext _context;

        public TransactionsController(QlktxContext context)
        {
            _context = context;
        }

        // GET: AdminQL/Transactions
        public async Task<IActionResult> Index()
        {
            var qlktxContext = _context.Transactions.Include(t => t.Student);
            return View(await qlktxContext.ToListAsync());
        }

        // GET: AdminQL/Transactions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Student)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // GET: AdminQL/Transactions/Create
        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "NumberRoom");
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "FullName");
            return View();
        }

        // POST: AdminQL/Transactions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransactionId,StudentId,Amount,Description,TransactionCode,TransactionDate")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "NumberRoom", transaction.Student.Room);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "FullName", transaction.StudentId);
            return View(transaction);
        }

        // GET: AdminQL/Transactions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "NumberRoom", transaction.Student.Room);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "FullName", transaction.StudentId);
            return View(transaction);
        }

        // POST: AdminQL/Transactions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransactionId,StudentId,Amount,Description,TransactionCode,TransactionDate")] Transaction transaction)
        {
            if (id != transaction.TransactionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transaction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransactionExists(transaction.TransactionId))
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
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "NumberRoom", transaction.Student.Room);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "FullName", transaction.StudentId);
            return View(transaction);
        }

        // GET: AdminQL/Transactions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Transactions == null)
            {
                return NotFound();
            }

            var transaction = await _context.Transactions
                .Include(t => t.Student)
                .FirstOrDefaultAsync(m => m.TransactionId == id);
            if (transaction == null)
            {
                return NotFound();
            }

            return View(transaction);
        }

        // POST: AdminQL/Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Transactions == null)
            {
                return Problem("Entity set 'QlktxContext.Transactions'  is null.");
            }
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransactionExists(int id)
        {
          return (_context.Transactions?.Any(e => e.TransactionId == id)).GetValueOrDefault();
        }
        [HttpGet]
        public IActionResult GetRoomsByBuilding(string building)
        {
            var rooms = _context.Rooms.Where(r => r.Building == building)
                                      .Select(r => new { value = r.RoomId, text = r.RoomId })
                                      .ToList();
            return Json(rooms);
        }

        // Action để lấy danh sách sinh viên theo phòng
        [HttpGet]
        public IActionResult GetStudentsByRoom(int roomId)
        {
            var students = _context.Students.Where(s => s.RoomId == roomId)
                                            .Select(s => new { value = s.StudentId, text = s.FullName })
                                            .ToList();
            return Json(students);
        }
    }
}
