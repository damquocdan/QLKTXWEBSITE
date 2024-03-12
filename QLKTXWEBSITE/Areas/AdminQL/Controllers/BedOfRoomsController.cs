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
    [Area("AdminQL")]
    public class BedOfRoomsController : Controller
    {
        private readonly QlktxContext _context;

        public BedOfRoomsController(QlktxContext context)
        {
            _context = context;
        }

        // GET: AdminQL/BedOfRooms
        public async Task<IActionResult> Index()
        {
            var qlktxContext = _context.BedOfRooms.Include(b => b.Room);
            return View(await qlktxContext.ToListAsync());
        }

        // GET: AdminQL/BedOfRooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.BedOfRooms == null)
            {
                return NotFound();
            }

            var bedOfRoom = await _context.BedOfRooms
                .Include(b => b.Room)
                .FirstOrDefaultAsync(m => m.BedId == id);
            if (bedOfRoom == null)
            {
                return NotFound();
            }

            return View(bedOfRoom);
        }

        // GET: AdminQL/BedOfRooms/Create
        public IActionResult Create()
        {
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId");
            return View();
        }

        // POST: AdminQL/BedOfRooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BedId,RoomId,NumberBed,Status")] BedOfRoom bedOfRoom)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bedOfRoom);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", bedOfRoom.RoomId);
            return View(bedOfRoom);
        }

        // GET: AdminQL/BedOfRooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.BedOfRooms == null)
            {
                return NotFound();
            }

            var bedOfRoom = await _context.BedOfRooms.FindAsync(id);
            if (bedOfRoom == null)
            {
                return NotFound();
            }
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", bedOfRoom.RoomId);
            return View(bedOfRoom);
        }

        // POST: AdminQL/BedOfRooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BedId,RoomId,NumberBed,Status")] BedOfRoom bedOfRoom)
        {
            if (id != bedOfRoom.BedId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bedOfRoom);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BedOfRoomExists(bedOfRoom.BedId))
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
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", bedOfRoom.RoomId);
            return View(bedOfRoom);
        }

        // GET: AdminQL/BedOfRooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.BedOfRooms == null)
            {
                return NotFound();
            }

            var bedOfRoom = await _context.BedOfRooms
                .Include(b => b.Room)
                .FirstOrDefaultAsync(m => m.BedId == id);
            if (bedOfRoom == null)
            {
                return NotFound();
            }

            return View(bedOfRoom);
        }

        // POST: AdminQL/BedOfRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.BedOfRooms == null)
            {
                return Problem("Entity set 'QlktxContext.BedOfRooms'  is null.");
            }
            var bedOfRoom = await _context.BedOfRooms.FindAsync(id);
            if (bedOfRoom != null)
            {
                _context.BedOfRooms.Remove(bedOfRoom);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BedOfRoomExists(int id)
        {
          return (_context.BedOfRooms?.Any(e => e.BedId == id)).GetValueOrDefault();
        }
    }
}
