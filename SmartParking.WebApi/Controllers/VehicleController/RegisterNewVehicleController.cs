using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParking.Service.Entities.EmployeeEntities;
using SmartParking.Service.Entities.EmployeeEntities.Authentication;
using SmartParking.Service.Interface.EmployeeInterface;
using SmartParking.Service.Interface.VehicleInterface;

namespace SmartParking.WebApi.Controllers.VehicleController
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterNewVehicleController : ControllerBase
    {
        public readonly IRegisterVehicles _registerVehicle;


        public RegisterNewVehicleController(IRegisterVehicles authenticationDataAccess)
        {
            _registerVehicle = authenticationDataAccess;
        }




        [HttpPost]

        public async Task<IActionResult> RegisterVehicle(EmployeeVehicleRequest request)
        {
            EmployeeVehicleResponse response = new EmployeeVehicleResponse();
            try
            {

                response = await _registerVehicle.RegisterVehicle(request);

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
