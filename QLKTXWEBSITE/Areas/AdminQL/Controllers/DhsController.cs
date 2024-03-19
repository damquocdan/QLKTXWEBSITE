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
    public class DhsController : BaseController
    {
        private readonly QlktxContext _context;

        public DhsController(QlktxContext context)
        {
            _context = context;
        }

        // GET: AdminQL/Dhs
        public async Task<IActionResult> Index()
        {
              return _context.Dhs != null ? 
                          View(await _context.Dhs.ToListAsync()) :
                          Problem("Entity set 'QlktxContext.Dhs'  is null.");
        }

        // GET: AdminQL/Dhs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Dhs == null)
            {
                return NotFound();
            }

            var dh = await _context.Dhs
                .FirstOrDefaultAsync(m => m.Dhid == id);
            if (dh == null)
            {
                return NotFound();
            }

            return View(dh);
        }

        // GET: AdminQL/Dhs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AdminQL/Dhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Dhid,Dhcode")] Dh dh)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dh);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(dh);
        }

        // GET: AdminQL/Dhs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Dhs == null)
            {
                return NotFound();
            }

            var dh = await _context.Dhs.FindAsync(id);
            if (dh == null)
            {
                return NotFound();
            }
            return View(dh);
        }

        // POST: AdminQL/Dhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Dhid,Dhcode")] Dh dh)
        {
            if (id != dh.Dhid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DhExists(dh.Dhid))
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
            return View(dh);
        }

        // GET: AdminQL/Dhs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Dhs == null)
            {
                return NotFound();
            }

            var dh = await _context.Dhs
                .FirstOrDefaultAsync(m => m.Dhid == id);
            if (dh == null)
            {
                return NotFound();
            }

            return View(dh);
        }

        // POST: AdminQL/Dhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Dhs == null)
            {
                return Problem("Entity set 'QlktxContext.Dhs'  is null.");
            }
            var dh = await _context.Dhs.FindAsync(id);
            if (dh != null)
            {
                _context.Dhs.Remove(dh);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DhExists(int id)
        {
          return (_context.Dhs?.Any(e => e.Dhid == id)).GetValueOrDefault();
        }
    }
}
