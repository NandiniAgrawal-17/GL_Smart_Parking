using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Entities.AdminEntities.Authentication
{
    public class AdminToken
    {
        public string JWTToken { get; set; }
        public string AdminRefreshToken { get; set; }
    }
}
