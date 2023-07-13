using SmartParking.Service.Entities.EmployeeEntities.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Entities.OperatorEntities.Authentication
{
    public class OperatorLoginRequest
    {
        [Required(ErrorMessage = "OperatorName Is Mandetory")]
        public string OperatorId { get; set; }

        [Required(ErrorMessage = "Password Is Mandetory")]
        public string Password { get; set; }

    }


    public class OperatorLoginResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public OperatorLoginInformation data { get; set; }
        public OperatorToken OperatorToken { get; set; }
    }

    public class OperatorLoginInformation
    {
        public int OperatorId { get; set; }
    }
}
