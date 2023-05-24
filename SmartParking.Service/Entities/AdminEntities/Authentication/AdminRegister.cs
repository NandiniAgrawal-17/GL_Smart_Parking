using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Entities.AdminEntities.Authentication
{
    public class AdminRegisterRequest
    {
        public int AdminId { get; set; }

        [Required(ErrorMessage = "AdminName Is Mandetory")]
        public string AdminName { get; set; }

        [Required(ErrorMessage = "Password Is Mandetory")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password Is Mandetory")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email Is Mandetory")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact Number Is Mandetory")]
        public string ContactNo { get; set; }
    }
    public class AdminRegisterResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
