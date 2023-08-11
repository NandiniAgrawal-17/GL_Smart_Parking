using System.ComponentModel.DataAnnotations;

namespace SmartParking.Service.Entities.SlotEntities
{
    public class SlotHistoryRequest
    {
        [Required(ErrorMessage = "DateTime Is Mandetory")]
        public DateTime InTime {get;set;}
    }

    public class SlotHistoryResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<ReadSlotHistory> getInformation { get; set; }

    }

    public class ReadSlotHistory
    {
    public int SId { get; set; }
    public string VehicleNumber { get; set; }
    public DateTime InTime { get; set; }
    public DateTime OutTime { get; set; }

      
    }
}

