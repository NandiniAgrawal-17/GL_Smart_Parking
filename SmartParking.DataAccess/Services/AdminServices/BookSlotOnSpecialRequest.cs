using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SmartParking.Service.Entities.OperatorEntities.Authentication;
using SmartParking.Service.Entities.SlotEntities;
using SmartParking.Service.Interface.AdminInterface;

namespace SmartParking.DataAccess.Services.AdminServices
{
    public class BookSlotOnSpecialRequest : IBookSlotOnPriority
    {

        public readonly IConfiguration _configuration;
        public readonly SqlConnection _mySqlConnection;
        public BookSlotOnSpecialRequest(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlConnection = new SqlConnection(_configuration.GetConnectionString("MyDBConnection").ToString());
        }




        public async Task<BookSlotsResponse> BookSlotOnRequest(BookSlotsRequest request)

        {
            BookSlotsResponse response = new BookSlotsResponse();
            response.IsSuccess = true;
            response.Message = "SuccessFul";
            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string SqlQuery = @"UPDATE MasterTable
                                   SET Type = 'R'
                                   FROM MasterTable where SlotNumber=@SlotNumber";


                using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;

                    sqlCommand.Parameters.AddWithValue("@SlotNumber", request.SlotNumber);
                    await sqlCommand.ExecuteNonQueryAsync();
                }
            }

            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }


            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }
            return response;
        }

    }

}
