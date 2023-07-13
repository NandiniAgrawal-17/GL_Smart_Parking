using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParking.Service.Entities.EmployeeEntities;
using SmartParking.Service.Interface.VehicleInterface;

namespace SmartParking.WebApi.Controllers.VehicleController
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetVehicleInformationController : ControllerBase
    {

        public readonly IGetVehicleInformation _getVehicleDetails;


        public GetVehicleInformationController(IGetVehicleInformation authenticationDataAccess)
        {
            _getVehicleDetails = authenticationDataAccess;
        }


        [HttpPost]
        
        public async Task<IActionResult> VehicleInformation(ManualVehicleNumberRequest request)
        {
            ManualVehicleNumberResponse response = new ManualVehicleNumberResponse();
            try
            {
                response = await _getVehicleDetails.VehicleInformation(request);

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
