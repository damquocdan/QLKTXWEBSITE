using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QLKTXWEBSITE.Models
{
    public partial class Student
    {
        public int StudentId { get; set; }

        public string? FullName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ParentPhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? StudentCode { get; set; }

        public int? Dhid { get; set; }

        public int? DepartmentId { get; set; }

        public string? Class { get; set; }

        public string? AdmissionConfirmation { get; set; }

        public int? RoomId { get; set; }

        public int? BedId { get; set; }

        public virtual BedOfRoom? Bed { get; set; }

        public virtual Department? Department { get; set; }

        public virtual Dh? Dh { get; set; }

        public virtual ICollection<Occupancy> Occupancies { get; set; } = new List<Occupancy>();

        public virtual Room? Room { get; set; }

        public virtual ICollection<Service> Services { get; set; } = new List<Service>();

        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
