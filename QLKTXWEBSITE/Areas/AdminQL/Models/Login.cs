using System.ComponentModel.DataAnnotations;

namespace QLKTXWEBSITE.Areas.AdminQL.Models
{
    public class Login
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Remember { get; set; }
    }
}
