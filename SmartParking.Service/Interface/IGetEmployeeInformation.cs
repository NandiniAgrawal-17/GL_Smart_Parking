using SmartParking.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Interface
{
    public interface IGetEmployeeInformation
    {
        public Task<GetEmployeeInformationResponse> GetEmployeeInfo();
    }
}
