using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Entities.EmployeeEntities.Authentication
{
    public class EmployeeTokens
    {
        public string JWTToken { get; set; }
        public string EmployeeRefreshToken { get; set; }

    }
}
