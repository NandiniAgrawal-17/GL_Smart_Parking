using System.ComponentModel.DataAnnotations;

namespace SmartParking.WebApp.Models.AdminGetDetails
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

    public class EmployeeRegisterResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<EmployeeDetails> GetEmployeeDetails { get; set; }
    }
}
