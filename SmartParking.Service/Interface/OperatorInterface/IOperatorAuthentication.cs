using SmartParking.Service.Entities.EmployeeEntities.Authentication;
using SmartParking.Service.Entities.OperatorEntities.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Interface.OperatorInterface
{
    public interface IOperatorAuthentication
    {
        public Task<OperatorLoginResponse> LoginOperator(OperatorLoginRequest request);
        public Task<OperatorLoginResponse> RefreshToken(OperatorToken operatortokens);
    }
}
