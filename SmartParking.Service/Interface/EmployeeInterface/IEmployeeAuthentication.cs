using SmartParking.Service.Entities.EmployeeEntities.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Interface.EmployeeInterface
{
    public interface IEmployeeAuthentication
    {
        public Task<EmployeeRegisterResponse> RegisterEmployee(EmployeeRegisterRequest request);

        public Task<EmployeeLoginResponse> LoginEmployee(EmployeeLoginRequest request);

    }
}
