using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParking.Service.Entities.EmployeeEntities;
using SmartParking.Service.Interface.VehicleInterface;

namespace SmartParking.WebApi.Controllers.VehicleController
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnterVehicleNumberManuallyController : ControllerBase
    {

        public readonly IManualVehicleNumber _enterVehicleNumber;


        public EnterVehicleNumberManuallyController(IManualVehicleNumber authenticationDataAccess)
        {
            _enterVehicleNumber = authenticationDataAccess;
        }




        [HttpPost]
        public async Task<IActionResult> EnterVehicleNumber(ManualVehicleNumberRequest request)
        {
            ManualVehicleNumberResponse response = new ManualVehicleNumberResponse();
            try
            {

                response = await _enterVehicleNumber.EnterVehicleNumber(request);

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
