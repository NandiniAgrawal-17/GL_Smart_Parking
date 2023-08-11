using System;
using System.ComponentModel.DataAnnotations;

namespace SmartParking.Service.Entities.SlotEntities
{
    public class AddSlotsRequest
    {

        [Required(ErrorMessage = "AdminId Is Mandetory")]

        public int SId { get; set; }

        [Required(ErrorMessage = "SlotNumber Is Mandetory")]

        public string SlotNumber { get; set; }


        [Required(ErrorMessage = "Occupancy Is Mandetory")]

        public string Type { get; set; }

    }

    public class AddSlotsResposne
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
