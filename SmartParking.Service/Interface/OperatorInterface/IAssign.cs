using SmartParking.Service.Entities.OperatorEntities.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Interface.OperatorInterface
{
    public interface IAssign
    {
        public Task<BookingModelResponse> BookStatus(BookingModelRequest request);
        public Task<BookingModelResponse> UpdateMasterType();
    }
}
