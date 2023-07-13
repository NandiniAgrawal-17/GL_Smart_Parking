using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Entities.AdminEntities.ViewDetails
{
    public class EmployeeDetails
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "EmployeeName Is Mandetory")]
        public string EmployeeName { get; set; }
        [Required(ErrorMessage = "Email Is Mandetory")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contact Number Is Mandetory")]

        public string ContactNo { get; set; }
        //public string RefreshToken { get; set; }

        public int? VehicleId { get; set; }
        public string? VehicleNumber { get; set; }
        public string? VehicleType { get; set; }
        public string? VehicleModel { get; set; }

    }
    public class EmployeeDetailsResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public List<EmployeeDetails> GetEmployeeDetails { get; set; }
    }
}
