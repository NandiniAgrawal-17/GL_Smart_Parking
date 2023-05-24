using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.Service.Entities
{
    public class GetInformation
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Email { get; set; }
        public string ContactNo { get; set; }

    }
    public class GetEmployeeInformationResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public List<GetInformation> getInformation { get; set; }
    }
}
