using SmartParking.Service.Entities.EmployeeEntities.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Interface.EmployeeInterface
{
    public interface IEmployeeToken
    {
        public string GenerateJWTToken(int EmployeeId);
        public Task RefreshTokenDB(EmployeeLoginResponse response);
        public string GenerateRefresh();
    }
}
