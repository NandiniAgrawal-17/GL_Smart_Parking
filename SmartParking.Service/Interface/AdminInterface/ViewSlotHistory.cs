using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartParking.Service.Entities.AdminEntities.Authentication;
using SmartParking.Service.Entities.SlotEntities;

namespace SmartParking.Service.Interface.AdminInterface
{
    public interface ViewSlotHistory
    {
        public Task<SlotHistoryResponse> SlotReportGenerate(SlotHistoryRequest request);
    }
}
