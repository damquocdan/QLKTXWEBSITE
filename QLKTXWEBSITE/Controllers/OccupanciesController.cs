using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLKTXWEBSITE.Models;

namespace QLKTXWEBSITE.Controllers
{
    public class OccupanciesController : Controller
    {
        private readonly QlktxContext _context;

        public OccupanciesController(QlktxContext context)
        {
            _context = context;
        }

        // GET: Occupancies
        public async Task<IActionResult> Index()
        {
            var qlktxContext = _context.Occupancies.Include(o => o.Room).Include(o => o.Student);
            return View(await qlktxContext.ToListAsync());
        }

        // GET: Occupancies/Details/5
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

        // GET: Occupancies/Create
        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId");
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId");
            return View();
        }

        // POST: Occupancies/Create
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
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", occupancy.RoomId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", occupancy.StudentId);
            return View(occupancy);
        }

        // GET: Occupancies/Edit/5
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
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", occupancy.RoomId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", occupancy.StudentId);
            return View(occupancy);
        }

        // POST: Occupancies/Edit/5
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
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", occupancy.RoomId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", occupancy.StudentId);
            return View(occupancy);
        }

        // GET: Occupancies/Delete/5
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

        // POST: Occupancies/Delete/5
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
    }
}
