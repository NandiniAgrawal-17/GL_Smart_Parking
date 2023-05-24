using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Entities.EmployeeEntities.Authentication
{
    public class EmployeeRefreshToken
    {
        public int EmployeeId { get; set; }
        public string TokenEmployee { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expire { get; set; }
    }
}
