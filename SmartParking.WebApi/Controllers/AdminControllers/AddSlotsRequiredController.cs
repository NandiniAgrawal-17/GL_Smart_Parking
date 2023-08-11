using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParking.Service.Entities.EmployeeEntities.Authentication;
using SmartParking.Service.Entities.SlotEntities;
using SmartParking.Service.Interface.AdminInterface;


namespace SmartParking.WebApi.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddSlotsRequiredController : ControllerBase
    {
        public readonly IAddSlot _authentication;


        public AddSlotsRequiredController(IAddSlot authenticationDataAccess)
        {
            _authentication = authenticationDataAccess;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> BookSlotOnRequest(AddSlotsRequest request)
        {
            AddSlotsResposne response = new AddSlotsResposne();
            try
            {

                response = await _authentication.BookSlotOnRequest(request);

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
