using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParking.Service.Entities.AdminEntities.ViewDetails;
using SmartParking.Service.Entities.EmployeeEntities.Authentication;
using SmartParking.Service.Interface.AdminInterface;

namespace SmartParking.WebApi.Controllers.AdminControllers
{

    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class AdminGetDetailsController : ControllerBase
    {
        public readonly IViewOperatorDetails _viewOperatorDetails;
        public readonly IViewEmployeeDetails _viewEmployeeDetails;
        public AdminGetDetailsController(IViewOperatorDetails viewOperatorDetails, IViewEmployeeDetails viewEmployeeDetails)
        {
            _viewOperatorDetails= viewOperatorDetails;
            _viewEmployeeDetails= viewEmployeeDetails;
        }
        [HttpGet]
        public async Task<IActionResult> GetOperatorDetails()
        {
            OperatorResponse response = new OperatorResponse();
            try
            {
                response = await _viewOperatorDetails.GetOperatorDetails();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {

            EmployeeDetailsResponse response = new EmployeeDetailsResponse();
            try
            {
                response = await _viewEmployeeDetails.GetAllEmployees();
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
