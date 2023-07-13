using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartParking.Service.Entities.EmployeeEntities.Authentication;

namespace SmartParking.Service.Entities.EmployeeEntities
{
    public class ManualVehicleNumberRequest
    {

        public string VehicleNumber { get; set; }
    }

    public class ManualVehicleNumberResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public VehicleInformation data { get; set; }

    }
    public class VehicleInformation
    {
        public string VehicleType{ get; set; }

        public string VehicleModel{ get; set; }

        public string VehicleNumber { get; set; }

        public int EmployeeId { get; set; }

    }
}
