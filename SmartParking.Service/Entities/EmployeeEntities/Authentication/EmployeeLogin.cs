using SmartParking.Service.Entities.OperatorEntities.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Entities.EmployeeEntities.Authentication
{
    public class EmployeeLoginRequest
    {
        [Required(ErrorMessage = "EmployeeId Is Mandetory")]
        public string EmployeeId { get; set; }

        [Required(ErrorMessage = "Password Is Mandetory")]
        public string Password { get; set; }
        
    }


    public class EmployeeLoginResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public LoginInformation data { get; set; }
        public EmployeeTokens EmployeeToken { get; set; }
    }

    public class LoginInformation
    {
        public int EmployeeId { get; set; }

        public string Email { get; set; }
       
    }
}
