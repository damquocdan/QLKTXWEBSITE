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
        public async Task<IActionResult> Index()
        {
            var qlktxContext = _context.Occupancies.Include(o => o.Room).Include(o => o.Student);
            return View(await qlktxContext.ToListAsync());
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
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId");
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId");
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
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", occupancy.RoomId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", occupancy.StudentId);
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
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", occupancy.RoomId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", occupancy.StudentId);
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
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "RoomId", occupancy.RoomId);
            ViewData["StudentId"] = new SelectList(_context.Students, "StudentId", "StudentId", occupancy.StudentId);
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
        public async Task<IActionResult> SendEmailToOccupants()
        {
            try
            {
                var occupants = await _context.Occupancies
                    .Include(o => o.Student)
                    .ToListAsync();

                foreach (var occupancy in occupants)
                {
                    // Gửi email tới địa chỉ email của cư dân
                    string toEmail = occupancy.Student.Email;
                    string subject = "Thông báo từ hệ thống quản lý KTX";
                    string body = "Xin chào " + occupancy.Student.FullName + ",\n\n"
                                + "Nội dung email: Bạn đang ở trong phòng số " + occupancy.Room.NumberRoom + ".\n\n"
                                + "Xin vui lòng kiểm tra thông tin đăng nhập tại đây: https://yourwebsite.com/login\n\n"
                                + "Trân trọng,\n"
                                + "Hệ thống quản lý KTX";

                    // Thiết lập thông tin của email người gửi
                    string fromEmail = "21111064572@gmail.com";
                    string fromPassword = "Danli29.03"; // Nhập mật khẩu ứng với tài khoản Gmail
                    string host = "smtp.gmail.com";
                    int port = 587;

                    // Gửi email
                    var smtpClient = new SmtpClient(host, port)
                    {
                        Credentials = new NetworkCredential(fromEmail, fromPassword),
                        EnableSsl = true
                    };

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromEmail),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true 
                    };

                    mailMessage.To.Add(toEmail);

                    await smtpClient.SendMailAsync(mailMessage);
                }

                TempData["SuccessMessage"] = "Email đã được gửi thành công cho tất cả cư dân.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Đã xảy ra lỗi khi gửi email: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
