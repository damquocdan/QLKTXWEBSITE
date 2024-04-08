using System.ComponentModel.DataAnnotations;
namespace QLKTXWEBSITE.Models
{
    public class LoginUser
    {
     
            [Required(ErrorMessage = "Mã sinh viên không để trống")]
            public string StudentCode { get; set; }
            [Required(ErrorMessage = "Mật khẩu không để trống")]
            public string Password { get; set; }
            public bool Remember { get; set; }
             public int StudentId { get; internal set; }
    }
}
