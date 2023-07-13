using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParking.Service.Entities.EmployeeEntities.Authentication;
using SmartParking.Service.Entities.OperatorEntities.Authentication;
using SmartParking.Service.Interface.OperatorInterface;
using System.Net.NetworkInformation;

namespace SmartParking.WebApi.Controllers.OperatorContrrollers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperatorAuthenticationController : ControllerBase
    {
        public readonly IOperatorAuthentication _authentication;
        public OperatorAuthenticationController(IOperatorAuthentication authentication)
        {
            _authentication = authentication;
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> LoginOperator(OperatorLoginRequest request)
        {
            OperatorLoginResponse response = new OperatorLoginResponse();
            try
            {
                response = await _authentication.LoginOperator(request);

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
        public async Task<IActionResult> RefreshToken(OperatorToken operatortokens)
        {
            OperatorLoginResponse response = new OperatorLoginResponse();
            try
            {
                response = await _authentication.RefreshToken(operatortokens);

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
