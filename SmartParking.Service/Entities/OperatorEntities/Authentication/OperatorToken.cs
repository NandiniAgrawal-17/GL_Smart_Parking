using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Entities.OperatorEntities.Authentication
{
    public class OperatorToken
    {
        public string JWTToken { get; set; }
        public string OperatorRefreshToken { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Expire { get; set; } = DateTime.Now.AddMonths(1);
    }
}
