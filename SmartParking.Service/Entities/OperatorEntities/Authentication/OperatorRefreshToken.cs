using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Entities.OperatorEntities.Authentication
{
    public class OperatorRefreshToken
    {
        public int OperatorId { get; set; }
        public string TokenOperator { get; set; }
        public DateTime Created { get; set; }
        public DateTime Expire { get; set; }
    }
}
