using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Entities.OperatorEntities.Authentication
{
    
        public class BookingModelRequest
        {
            public int SId { get; set; }
            public string VehicleNumber { get; set; }
            public DateTime InTime { get; set; }
            public DateTime OutTime { get; set; }
        }
        public class BookingModel
        {
            public int SId { get; set; }
            public DateTime OutTime { get; set; }
    }
        public class BookingModelResponse
        {
            public bool IsSuccess { get; set; }
            public string Message { get; set; }

        }
}

