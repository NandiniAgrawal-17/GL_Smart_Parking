using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Entities.EmployeeEntities
{
    public class EmployeeVehicleRequest
    {
       
        public string VehicleType { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleNumber { get; set; }
        public int EmployeeId { get; set; }

    }
    public class EmployeeVehicleResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
       

    }
}
