using SmartParking.Service.Entities.EmployeeEntities.Authentication;
using SmartParking.Service.Entities.OperatorEntities.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Interface.OperatorInterface
{
    public interface IOperatorToken
    {
        public string GenerateJWTToken(int OperatorId);
        public Task RefreshTokenDB(OperatorLoginResponse response);
        public string GenerateRefresh();
    }
}
