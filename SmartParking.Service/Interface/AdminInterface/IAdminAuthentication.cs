using SmartParking.Service.Entities.AdminEntities.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Interface.AdminInterface
{
    public interface IAdminAuthentication
    {
        public Task<AdminRegisterResponse> RegisterAdmin(AdminRegisterRequest request);
        public Task<AdminLoginResponse> LoginAdmin(AdminLoginRequest request);
    }
}
