using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Entities.SlotEntities
{
    public class BookSlotsRequest
    {
        public string SlotNumber { get; set; }
     }


    public class BookSlotsResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
