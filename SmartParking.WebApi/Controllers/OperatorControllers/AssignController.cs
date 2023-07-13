using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartParking.Service.Entities.OperatorEntities.Authentication;
using SmartParking.Service.Interface.OperatorInterface;

namespace SmartParking.WebApi.Controllers.OperatorControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignController : ControllerBase
    {
        public readonly IAssign _status;
        public AssignController(IAssign slotData)
        {
            _status = slotData;
        }
        [HttpPost]

        public async Task<IActionResult> Status(BookingModelRequest request)
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
        [HttpGet]

        public async Task<IActionResult> UpdateMasterType()
        {
            BookingModelResponse response = new BookingModelResponse();
            try
            {

                response = await _status.UpdateMasterType();

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
