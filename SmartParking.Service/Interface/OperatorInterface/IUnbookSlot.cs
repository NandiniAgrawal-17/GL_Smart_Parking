using SmartParking.Service.Entities.OperatorEntities.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Interface.OperatorInterface
{
    public interface IUnbookSlot
    {
        public Task<BookingModelResponse> BookStatus(BookingModel request);
        public Task<BookingModelResponse> UpdateStatus(BookingModel request);
    }
}
