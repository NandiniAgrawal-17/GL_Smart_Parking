using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParking.Service.Entities.OperatorEntities.Authentication;
using SmartParking.Service.Interface.OperatorInterface;
using System.Data.SqlClient;

namespace SmartParking.WebApi.Controllers.OperatorControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnbookSlotStatusController : ControllerBase
    {
        private readonly IUnbookSlot _status;
        public UnbookSlotStatusController(IUnbookSlot status)
        {
            _status = status;
        }
        [HttpPost]
        public async Task<IActionResult> BookedStatus(BookingModel request)
        {
            BookingModelResponse response = new BookingModelResponse();
            try
            {

                response = await _status.BookStatus(request);

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;

            }
            return Ok(response);
        }
        [HttpPut]
        [AllowAnonymous]


        public async Task<IActionResult> UpdateStatus(BookingModel request)
        {
            BookingModelResponse response = new BookingModelResponse();
            try
            {

                response = await _status.UpdateStatus(request);

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
