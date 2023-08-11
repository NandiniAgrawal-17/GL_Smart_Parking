
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SmartParking.Service.Entities.EmployeeEntities.Authentication;
using SmartParking.Service.Entities.SlotEntities;
using SmartParking.Service.Interface.AdminInterface;

namespace SmartParking.DataAccess.Services.AdminServices
{
    public class AddSlotsRequired : IAddSlot
    {
        public readonly IConfiguration _configuration;
        public readonly SqlConnection _mySqlConnection;

        public AddSlotsRequired(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlConnection = new SqlConnection(_configuration.GetConnectionString("MyDBConnection").ToString());

        }
        public async Task<AddSlotsResposne> BookSlotOnRequest(AddSlotsRequest request)
        {
            AddSlotsResposne response = new AddSlotsResposne();
            response.IsSuccess = true;
            response.Message = "SuccessFul";

            try
            {

                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

               

                string SqlQuery = @"INSERT INTO MasterTable
                                    (SId,SlotNumber,Type) Values 
                                    (@SId,@SlotNumber, @Type);";

                using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                
                    sqlCommand.Parameters.AddWithValue("@SId", request.SId);
                    sqlCommand.Parameters.AddWithValue("@SlotNumber", request.SlotNumber);
                    sqlCommand.Parameters.AddWithValue("@Type", request.Type);
                   
                    int Status = await sqlCommand.ExecuteNonQueryAsync();

                    if (Status <= 0)
                    {
                        response.IsSuccess = false;
                        response.Message = "Register Query Not Executed";
                        return response;
                    }
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
