using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParking.Service.Entities.EmployeeEntities.Authentication;
using SmartParking.Service.Interface.EmployeeInterface;

namespace SmartParking.WebApi.Controllers.EmployeeControllers
{

    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class EmployeeAuthenticationController : ControllerBase
    {
        public readonly IEmployeeAuthentication _authentication;


        public EmployeeAuthenticationController(IEmployeeAuthentication authenticationDataAccess)
        {
            _authentication = authenticationDataAccess;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterEmployee(EmployeeRegisterRequest request)
        {
            EmployeeRegisterResponse response = new EmployeeRegisterResponse();
            try
            {

                response = await _authentication.RegisterEmployee(request);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }



        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginEmployee(EmployeeLoginRequest request)
        {
            EmployeeLoginResponse response = new EmployeeLoginResponse();
            try
            {
                response = await _authentication.LoginEmployee(request);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }
        [HttpPost]

        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken (EmployeeTokens employeetoken)
        {
            EmployeeLoginResponse response = new EmployeeLoginResponse();
            try
            {
                response = await _authentication.RefreshToken(employeetoken);

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
