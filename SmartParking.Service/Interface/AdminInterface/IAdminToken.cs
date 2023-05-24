using SmartParking.Service.Entities.AdminEntities.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Interface.AdminInterface
{
    public interface IAdminToken
    {
        public string GenerateJWTToken(int Id);

        public Task AdminTokenDB(AdminLoginResponse response);
        public string GenerateRefresh();
    }
}
