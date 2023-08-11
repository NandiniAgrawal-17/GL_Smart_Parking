using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParking.Service.Entities.SlotEntities;
using SmartParking.Service.Interface.AdminInterface;

namespace SmartParking.WebApi.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookSlotOnSpecialRequestController : ControllerBase
    {
       
            public readonly IBookSlotOnPriority _bookSlot;


            public BookSlotOnSpecialRequestController(IBookSlotOnPriority authenticationDataAccess)
            {
            _bookSlot = authenticationDataAccess;
            }

            [HttpPut]
            [AllowAnonymous]

            public async Task<IActionResult> BookSlotOnRequest(BookSlotsRequest request)
            {
            BookSlotsResponse response = new BookSlotsResponse();
                try
                {

                    response = await _bookSlot.BookSlotOnRequest(request);

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
