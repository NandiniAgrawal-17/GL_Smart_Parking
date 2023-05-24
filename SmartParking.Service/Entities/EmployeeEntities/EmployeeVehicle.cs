using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Entities.EmployeeEntities
{
    public class EmployeeVehicle
    {
        public int VechileId { get; set; }
        public string VechileType { get; set; }
        public string VechileModel { get; set; }
        public string VechileNumber { get; set; }
    }
    public class EmployeeVehicleResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<EmployeeVehicle> Vehicles { get; set; }

    }
}
