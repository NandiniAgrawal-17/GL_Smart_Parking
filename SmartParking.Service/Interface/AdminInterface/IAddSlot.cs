using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartParking.Service.Entities.SlotEntities;

namespace SmartParking.Service.Interface.AdminInterface
{
    public interface IAddSlot
    {
        public Task<AddSlotsResposne> BookSlotOnRequest(AddSlotsRequest request);
    }
}
