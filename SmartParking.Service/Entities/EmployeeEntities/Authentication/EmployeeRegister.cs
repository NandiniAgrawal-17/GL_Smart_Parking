using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Entities.EmployeeEntities.Authentication
{
    public class EmployeeRegisterRequest
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "EmployeeName Is Mandetory")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Password Is Mandetory")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password Is Mandetory")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email Is Mandetory")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact Number Is Mandetory")]
        public string ContactNo { get; set; }
        //public string RefreshToken { get; set; }
        public string? VehicleNumber { get; set; }

    }


    public class EmployeeRegisterResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<EmployeeRegisterRequest> getEmployeeInformation { get; set; }

    }
}
