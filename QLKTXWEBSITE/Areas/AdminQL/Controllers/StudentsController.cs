using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QLKTXWEBSITE.Models;
using OfficeOpenXml;
using System.IO;
using System.Data;
using OfficeOpenXml.Table;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Security.Cryptography;
using System.Globalization;
using System.Net.Mail;
using System.Net;
namespace QLKTXWEBSITE.Areas.AdminQL.Controllers
{
    //[Area("AdminQL")]
    public class StudentsController : BaseController
    {
        private readonly QlktxContext _context;

        public StudentsController(QlktxContext context)
        {
            _context = context;
        }

        // GET: AdminQL/Students
        public async Task<IActionResult> Index(string name,string bed)
        {
            IQueryable<Student> students = _context.Students.Include(s => s.Bed).Include(s => s.Department).Include(s => s.Dh).Include(s => s.Room);

            if (!string.IsNullOrEmpty(name))
            {
                students = students.Where(s => s.FullName.Contains(name));
            }
            if (!string.IsNullOrEmpty(bed))
            {
                if (bed == "null")
                {
                    students = students.Where(s => s.BedId == null);
                }
                else if (bed == "notnull")
                {
                    students = students.Where(s => s.BedId != null);
                }
            }
            return View(await students.ToListAsync());

        }
        [HttpGet]
        public IActionResult GetBedsByRoomId(int roomId)
        {
            // Lấy danh sách giường của phòng theo roomId và có trạng thái trống từ cơ sở dữ liệu
            var emptyBedsInRoom = _context.BedOfRooms.Where(b => b.RoomId == roomId && b.Status==false).ToList();

            // Tạo danh sách giường để trả về
            var bedList = emptyBedsInRoom.Select(b => new SelectListItem
            {
                Value = b.BedId.ToString(),
                Text = b.NumberBed.ToString() // Thay bằng tên trường chứa tên giường trong model
            }).ToList();

            // Trả về danh sách giường dưới dạng JSON
            return Json(bedList);
        }
        public async Task<IActionResult> Search(string name)
        {
            var students = await _context.Students
                .Where(s => s.FullName.Contains(name))
                .Include(s => s.Bed)
                .Include(s => s.Department)
                .Include(s => s.Dh)
                .Include(s => s.Room)
                .ToListAsync();

            return View("Index", students);
        }
        public async Task<IActionResult> ExportToExcel()
            {
                var students = await _context.Students
                    .Include(s => s.Bed)
                    .Include(s => s.Department)
                    .Include(s => s.Dh)
                    .Include(s => s.Room)
                    .ToListAsync();

                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Students");

                // Header row
                    worksheet.Cells[1, 1].Value = "STT";
                    worksheet.Cells[1, 2].Value = "Mã sinh viên";
                    worksheet.Cells[1, 3].Value = "Họ và tên";
                    worksheet.Cells[1, 4].Value = "NTNS";
                    worksheet.Cells[1, 5].Value = "Giới tính";
                    worksheet.Cells[1, 6].Value = "Điện thoại";
                    worksheet.Cells[1, 7].Value = "Parent Phone Number";
                    worksheet.Cells[1, 8].Value = "Email";
                    worksheet.Cells[1, 9].Value = "Khoa";
                    worksheet.Cells[1, 10].Value = "Lớp";
                    worksheet.Cells[1, 11].Value = "Admission Confirmation";


                // Style for header row
                using (var range = worksheet.Cells["A1:K1"])
                    {
                            range.Style.Font.Bold = true;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            range.Style.WrapText = true; // Enable text wrapping
                            range.Style.Font.Size = 12; // Set font size
                            range.Style.Font.Color.SetColor(Color.White); // Set font color
                            range.Style.Font.Name = "Calibri"; // Set font name

                            // Custom width for each column
                            worksheet.Row(1).Height = 35;
                            worksheet.Column(1).Width = 10;
                            worksheet.Column(2).Width = 30;
                            worksheet.Column(3).Width = 40; 
                            worksheet.Column(4).Width = 15;
                            worksheet.Column(5).Width = 15;
                            worksheet.Column(6).Width = 20;
                            worksheet.Column(7).Width = 32;
                            worksheet.Column(8).Width = 30;
                            worksheet.Column(9).Width = 35;
                            worksheet.Column(10).Width = 15;
                            worksheet.Column(11).Width = 30; 

                            // Custom height for header row
                            range.Style.Font.Bold = false;
                            range.Style.Font.Size = 13;
                            range.Style.Font.Color.SetColor(Color.White);
                            range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            range.Style.Fill.BackgroundColor.SetColor(Color.LightGreen);
                            range.Style.WrapText = false;
                  


                        }

                        // Data rows
                        int row = 2;
                        int stt = 1;
                foreach (var student in students)
                    {   
                        worksheet.Row(row).Height = 20;
                        worksheet.Cells[row, 1].Value = stt;
                        worksheet.Cells[row, 2].Value = student.StudentCode;
                        worksheet.Cells[row, 3].Value = student.FullName;
                        worksheet.Cells[row, 4].Value = Convert.ToDateTime(student.DateOfBirth).ToString("dd/MM/yyyy");
                        worksheet.Cells[row, 5].Value = student.Gender;
                        worksheet.Cells[row, 6].Value = student.PhoneNumber;
                        worksheet.Cells[row, 7].Value = student.ParentPhoneNumber;
                        worksheet.Cells[row, 8].Value = student.Email;
                        worksheet.Cells[row, 9].Value = student.Department?.DepartmentName; // Assuming DepartmentName property exists
                        worksheet.Cells[row, 10].Value = student.Class;
                        worksheet.Cells[row, 11].Value = student.Dhid;
                    
                    row++;
                    stt++;
                    }

                    // Auto-fit columns
                    //worksheet.Cells.AutoFitColumns();

                    // Add borders for all cells
                    using (var range = worksheet.Cells[1, 1, row - 1, 11])
                    {
                        range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }

                    // Convert package to a byte array
                    byte[] excelBytes = package.GetAsByteArray();

                    // Return Excel file
                    return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Students.xlsx");
                }
            }

        // POST: AdminQL/Students/ImportFromExcel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportFromExcel(IFormFile file)
        {
            var departments = await _context.Departments.ToListAsync();
            Dictionary<string, int> departmentMappings = departments.ToDictionary(d => d.DepartmentName, d => d.DepartmentId);

            if (file == null || file.Length <= 0)
            {
                ModelState.AddModelError("", "Vui lòng chọn một tệp Excel.");
                return RedirectToAction(nameof(Index));
            }

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                stream.Position = 0;

                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                    if (worksheet == null)
                    {
                        ModelState.AddModelError("", "Không tìm thấy bảng dữ liệu trong tệp Excel.");
                        return RedirectToAction(nameof(Index));
                    }

                    var rowCount = worksheet.Dimension.Rows;
                    var importedStudents = new List<Student>();

                    // Bắt đầu từ dòng thứ 2 để bỏ qua dòng tiêu đề
                    for (int row = 2; row <= rowCount; row++)
                    {
                        string departmentName = worksheet.Cells[row, 9].Value?.ToString();
                        if (departmentMappings.ContainsKey(departmentName))
                        {
                            DateTime dateOfBirth;
                            string dateOfBirthString = worksheet.Cells[row, 4].Value?.ToString();
                            if (!DateTime.TryParseExact(dateOfBirthString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateOfBirth))
                            {
                                ModelState.AddModelError("", $"Không thể chuyển đổi ngày sinh '{dateOfBirthString}' thành DateTime.");
                                return RedirectToAction(nameof(Index));
                            }

                            var newStudent = new Student
                            {
                                StudentCode = worksheet.Cells[row, 2].Value?.ToString(),
                                FullName = worksheet.Cells[row, 3].Value?.ToString(),
                                DateOfBirth = dateOfBirth,
                                Gender = worksheet.Cells[row, 5].Value?.ToString(),
                                PhoneNumber = worksheet.Cells[row, 6].Value?.ToString(),
                                ParentPhoneNumber = worksheet.Cells[row, 7].Value?.ToString(),
                                Email = worksheet.Cells[row, 8].Value?.ToString(),
                                DepartmentId = departmentMappings[departmentName],
                                Class = worksheet.Cells[row, 10].Value?.ToString(),
                                AdmissionConfirmation = worksheet.Cells[row, 11].Value?.ToString(),
                                // Khởi tạo các giá trị còn lại tùy theo cấu trúc của model Student
                                // Ví dụ: Dhid, RoomId, BedId, ...
                            };

                            importedStudents.Add(newStudent);
                        }
                        else
                        {
                            // Xử lý khi không tìm thấy phòng ban trong từ điển
                        }
                    }

                    // Thêm danh sách sinh viên từ Excel vào cơ sở dữ liệu
                    _context.Students.AddRange(importedStudents);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
        }



        // GET: AdminQL/Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Bed)
                .Include(s => s.Department)
                .Include(s => s.Dh)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: AdminQL/Students/Create
        public IActionResult Create()
        {
            // Lấy danh sách các phòng còn trống từ cơ sở dữ liệu
            var emptyRooms = _context.Rooms.Where(r => r.Status == false).ToList();

            // Chuyển danh sách các phòng còn trống thành SelectList
            SelectList roomList = new SelectList(emptyRooms, "RoomId", "NumberRoom");

            // Đặt danh sách các phòng vào ViewBag để sử dụng trong view
            ViewBag.RoomId = roomList;

            // Tạo SelectList cho các thông tin khác cần thiết
            ViewData["BedId"] = new SelectList(_context.BedOfRooms, "BedId", "NumberBed");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName");
            ViewData["Dhid"] = new SelectList(_context.Dhs, "Dhid", "Dhcode");

            return View();
        }

        // POST: AdminQL/Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,FullName,DateOfBirth,Gender,PhoneNumber,ParentPhoneNumber,Email,Password,StudentCode,Dhid,DepartmentId,Class,AdmissionConfirmation,RoomId,BedId")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BedId"] = new SelectList(_context.BedOfRooms, "BedId", "NumberBed", student.BedId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", student.DepartmentId);
            ViewData["Dhid"] = new SelectList(_context.Dhs, "Dhid", "Dhcode", student.Dhid);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "NumberRoom", student.RoomId);
            return View(student);
        }

        // GET: AdminQL/Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["BedId"] = new SelectList(_context.BedOfRooms, "BedId", "NumberBed", student.BedId);
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", student.DepartmentId);
            ViewData["Dhid"] = new SelectList(_context.Dhs, "Dhid", "Dhcode", student.Dhid);
            ViewData["RoomId"] = new SelectList(_context.Rooms, "RoomId", "NumberRoom", student.RoomId);
            return View(student);
        }

        // POST: AdminQL/Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,FullName,DateOfBirth,Gender,PhoneNumber,ParentPhoneNumber,Email,Password,StudentCode,Dhid,DepartmentId,Class,AdmissionConfirmation,RoomId,BedId")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
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
            ViewData["BedNumbers"] = new SelectList(_context.BedOfRooms, "BedId", "NumberBed", student.BedId);
            ViewData["DepartmentNames"] = new SelectList(_context.Departments, "DepartmentId", "DepartmentName", student.DepartmentId);
            ViewData["DhCodes"] = new SelectList(_context.Dhs, "Dhid", "Dhcode", student.Dhid);
            ViewData["RoomNumbers"] = new SelectList(_context.Rooms, "RoomId", "NumberRoom", student.RoomId);
            return View(student);
        }

        // GET: AdminQL/Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Bed)
                .Include(s => s.Department)
                .Include(s => s.Dh)
                .Include(s => s.Room)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: AdminQL/Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Students == null)
            {
                return Problem("Entity set 'QlktxContext.Students'  is null.");
            }
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
          return (_context.Students?.Any(e => e.StudentId == id)).GetValueOrDefault();
        }
        [HttpPost]
        public IActionResult SendEmailToSelected(List<int> ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    var student = _context.Students.FirstOrDefault(s => s.StudentId == id);
                    if (student != null && !string.IsNullOrEmpty(student.Email))
                    {
                        // Gửi email cho sinh viên ở đây
                        SendEmail(student.Email, "Tiêu đề email", "Nội dung email");
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        public IActionResult SendEmailToAllStudents()
        {
            // Lấy danh sách sinh viên từ cơ sở dữ liệu
            var students = _context.Students.ToList();

            // Gửi email cho từng sinh viên trong danh sách
            foreach (var student in students)
            {
                SendEmail(student.Email, "Tiêu đề email", "Nội dung email");
            }

            return RedirectToAction("Index"); // Chuyển hướng sau khi gửi email
        }

        private void SendEmail(string toEmail, string subject, string body)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("21111064572@hunre.edu.vn"); // Địa chỉ email người gửi
                mail.To.Add(toEmail); // Địa chỉ email người nhận
                mail.Subject = subject; // Tiêu đề email
                mail.Body = body; // Nội dung email
                mail.IsBodyHtml = true; // Thiết lập email có dạng HTML (tuỳ chọn)

                // Cấu hình thông tin SMTP
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("21111064572@hunre.edu.vn", "Danli29.03");
                    smtp.UseDefaultCredentials = false;
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }


        [HttpPost]
        public IActionResult DeleteSelected(List<int> ids)
        {
            // Xử lý logic xoá các sinh viên với ids được truyền vào
            // Ví dụ: 
            foreach (var id in ids)
            {
                var student = _context.Students.Find(id);
                if (student != null)
                {
                    _context.Students.Remove(student);
                }
            }
            _context.SaveChanges();

            return Ok(); // Trả về mã HTTP 200 OK nếu xoá thành công
        }
    }
}
