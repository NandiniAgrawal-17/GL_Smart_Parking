using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Entities.AdminEntities.Authentication
{
    public class AdminLoginRequest
    {
        [Required(ErrorMessage = "AdminId Is Mandetory")]
        public string AdminId { get; set; }

        [Required(ErrorMessage = "Password Is Mandetory")]
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class AdminLoginResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public AdminLoginInformation data { get; set; }
        public AdminToken AdminToken { get; set; }
    }

    public class AdminLoginInformation
    {
        public int AdminId { get; set; }
        /*        public string Password { get; set; }*/
        public string Email { get; set; }
        public string Role { get; set; }

    }
}
