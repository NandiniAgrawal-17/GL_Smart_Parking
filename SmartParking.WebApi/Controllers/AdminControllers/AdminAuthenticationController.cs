using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParking.Service.Entities.AdminEntities.Authentication;
using SmartParking.Service.Interface.AdminInterface;

namespace SmartParking.WebApi.Controllers.AdminControllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class AdminAuthenticationController : ControllerBase
    {
        public readonly IAdminAuthentication _authentication;


        public AdminAuthenticationController(IAdminAuthentication authentication)
        {
            _authentication = authentication;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAdmin(AdminRegisterRequest request)
        {
            AdminRegisterResponse response = new AdminRegisterResponse();
            try
            {

                response = await _authentication.RegisterAdmin(request);

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
        public async Task<IActionResult> LoginAdmin(AdminLoginRequest request)
        {
            AdminLoginResponse response = new AdminLoginResponse();
            try
            {
                response = await _authentication.LoginAdmin(request);

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
