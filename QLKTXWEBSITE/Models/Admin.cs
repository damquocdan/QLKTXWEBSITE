using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QLKTXWEBSITE.Models
{
    public partial class Admin
    {
        public int AdminId { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? FullName { get; set; }
    }
}
