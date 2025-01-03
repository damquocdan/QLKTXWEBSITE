﻿using Microsoft.AspNetCore.Mvc;
using QLKTXWEBSITE.Models;
using System.Linq;

namespace QLKTXWEBSITE.Areas.AdminQL.Controllers
{
    public class DashboardController : BaseController
    {
        private readonly QlktxContext _context;

        public DashboardController(QlktxContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var totalStudents = _context.Students.Count();
            var studentsWithoutBed = _context.Students.Count(s => s.BedId == null);
            var totalBeds = _context.BedOfRooms.Count();
            var bedsWithoutStudent = _context.BedOfRooms.Count(b => b.Status == false);
            var totalFalseServices = _context.Services.Count(s => s.Status == false);
            var totalServices = _context.Services.Count();
            var notPayMoney = _context.Occupancies.Count(s=>s.Status == false);
            var totalPayMoney = _context.Occupancies.Count();

            var viewModel = new DashboardViewModel
            {
                TotalStudents = _context.Students.Count(),
                TotalRooms = _context.Rooms.Count(),
                TotalBeds = _context.BedOfRooms.Count(),
                StudentsWithoutBed = _context.Students.Count(s => s.BedId == null),

                BedsWithoutStudent = _context.BedOfRooms.Count(b => b.Status ==false),
                StudentWithoutBedPercentage = (double)studentsWithoutBed / totalStudents * 100 ,
                BedWithoutStudentPercentage = (double)bedsWithoutStudent / totalBeds * 100,
                TotalFalseServices = totalFalseServices,
                TotalServices = totalServices,
                TotalFalseServicesPercentage =(double)totalFalseServices / totalServices * 100,
                NotPaymoney = notPayMoney,
                TotalPaymoney = totalPayMoney,
                NotPaymoneyPercentage = (double)notPayMoney /totalPayMoney * 100,

            };

            return View(viewModel);
        }
        [HttpGet]
        public IActionResult GetIncomeData()
        {
            var transactions = _context.Transactions.OrderBy(t => t.TransactionDate).ToList();

            var labels = new List<string>();
            var totalWithStudent = new List<decimal>();
            var totalWithoutStudent = new List<decimal>();

            decimal accumulatedTotalWithStudent = 0;
            decimal accumulatedTotalWithoutStudent = 0;

            foreach (var transaction in transactions)
            {
                // Kiểm tra xem TransactionDate có giá trị không
                if (transaction.TransactionDate.HasValue)
                {
                    labels.Add(transaction.TransactionDate.Value.ToString("dd/MM/yyyy"));

                    if (transaction.StudentId != null)
                    {
                        accumulatedTotalWithoutStudent += 0;
                        accumulatedTotalWithStudent += transaction.Amount.Value;
                        
                    }
                    else
                    {
                        accumulatedTotalWithStudent += 0;
                        accumulatedTotalWithoutStudent += transaction.Amount.Value;
                    }

                    totalWithStudent.Add(accumulatedTotalWithStudent);
                    totalWithoutStudent.Add(accumulatedTotalWithoutStudent);
                }
            }

            return Json(new { labels, totalWithStudent, totalWithoutStudent });
        }

    }
}
