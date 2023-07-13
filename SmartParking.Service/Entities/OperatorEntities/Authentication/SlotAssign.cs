using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Entities.OperatorEntities.Authentication
{
    public class SlotAssignRequest
    {

        public string EmployeeId { get; set; }

        [Required(ErrorMessage = "Password Is Mandetory")]
        public string VehicleNumber { get; set; }
        [Required(ErrorMessage = "Slot is Required")]
        public int Slot { get; set; }
        public string Occupancy { get; set; }
    }
    public class SlotAssignResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public int totalSlot { get; set; }
        //public int available { get; set; }
        //public int Occupied { get; set; }
    }
}
