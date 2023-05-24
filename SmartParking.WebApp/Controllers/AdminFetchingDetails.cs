using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartParking.WebApp.Models.AdminGetDetails;
using System.Text.Json.Serialization;

namespace SmartParking.WebApp.Controllers
{
    public class AdminFetchingDetails : Controller
    {
        public async Task<IActionResult> GetEmployeeDetails()
        {
            EmployeeRegisterResponse employeeRegisterResponse;
            IEnumerable<EmployeeDetails> employeedetails;
            using (var httpClient = new HttpClient())//handler
            {
                using (var response = await httpClient.GetAsync("https://localhost:7258/api/AdminGetDetails/GetAllEmployees"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    employeeRegisterResponse = JsonConvert.DeserializeObject<EmployeeRegisterResponse>(apiResponse);
                    employeedetails = employeeRegisterResponse.GetEmployeeDetails;
                }
            }
            return View(employeedetails);
        }

        public async Task<IActionResult> GetOperatorDetails()
        {
            OperatorResponse operatorResponse;
            IEnumerable<Operator> operators;
            using (var httpClient = new HttpClient())//handler
            {
                using (var response = await httpClient.GetAsync("https://localhost:7258/api/AdminGetDetails/GetOperatorDetails"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    operatorResponse = JsonConvert.DeserializeObject<OperatorResponse>(apiResponse);
                    operators = operatorResponse.OperatorList;
                }
            }
            return View(operators);
        }
    }
}
