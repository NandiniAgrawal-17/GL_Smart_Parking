using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParking.Service.Entities.EmployeeEntities.Authentication;
using SmartParking.Service.Entities.SlotEntities;
using SmartParking.Service.Interface.AdminInterface;
using SmartParking.Service.Interface.EmployeeInterface;

namespace SmartParking.WebApi.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateSlotReportController : ControllerBase
    {
        public readonly ViewSlotHistory _authentication;


        public GenerateSlotReportController(ViewSlotHistory authenticationDataAccess)
        {
            _authentication = authenticationDataAccess;
        }

        [HttpPost]
        [AllowAnonymous]
    
        public async Task<IActionResult> SlotReportGenerate(SlotHistoryRequest request)
        {
            SlotHistoryResponse response = new SlotHistoryResponse();
            try
            {

                response = await _authentication.SlotReportGenerate(request);

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
