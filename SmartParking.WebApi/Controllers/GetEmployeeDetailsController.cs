using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParking.Service.Entities;
using SmartParking.Service.Interface;

namespace SmartParking.WebApi.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class GetEmployeeDetailsController : ControllerBase
    {
        public readonly IGetEmployeeInformation _getEmployeeInformation;


        public GetEmployeeDetailsController(IGetEmployeeInformation acessdata)
        {
            _getEmployeeInformation = acessdata;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetEmployeeInfo()
        {
            GetEmployeeInformationResponse response = new GetEmployeeInformationResponse();
            try
            {
                response = await _getEmployeeInformation.GetEmployeeInfo();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }
    }
}
