using SmartParking.Service.Entities.OperatorEntities.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Interface.OperatorInterface
{
    public interface ICount
    {
        public Task<SlotAssignResponse> SlotNumber();
    }
}
