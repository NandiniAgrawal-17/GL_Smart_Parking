using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartParking.Service.Entities.EmployeeEntities;

namespace SmartParking.Service.Interface.VehicleInterface
{
    public interface IRegisterVehicles
    {
        public Task<EmployeeVehicleResponse> RegisterVehicle(EmployeeVehicleRequest request);

    }
}
