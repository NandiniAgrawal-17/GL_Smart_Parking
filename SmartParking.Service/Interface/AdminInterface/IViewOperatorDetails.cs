using SmartParking.Service.Entities.AdminEntities.ViewDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Interface.AdminInterface
{
    public interface IViewOperatorDetails
    {
        public Task<OperatorResponse> GetOperatorDetails();
    }
}
