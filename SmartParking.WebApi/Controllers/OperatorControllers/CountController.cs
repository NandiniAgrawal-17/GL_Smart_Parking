using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParking.Service.Entities.OperatorEntities.Authentication;
using SmartParking.Service.Interface.OperatorInterface;

namespace SmartParking.WebApi.Controllers.OperatorControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountController : ControllerBase
    {
        public readonly ICount _slot;
        public CountController(ICount slotData)
        {
            _slot = slotData;
        }



        [HttpGet]
        public async Task<IActionResult> TotalSlot()
        {

            SlotAssignResponse response = new SlotAssignResponse();
            try
            {
                response = await _slot.SlotNumber();
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
