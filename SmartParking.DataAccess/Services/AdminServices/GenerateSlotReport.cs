
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SmartParking.Service.Entities.SlotEntities;
using SmartParking.Service.Interface.AdminInterface;

namespace SmartParking.DataAccess.Services.AdminServices
{
    public class GenerateSlotReport : ViewSlotHistory
    {
        public readonly IConfiguration _configuration;
        public readonly SqlConnection _mySqlConnection;
        public GenerateSlotReport(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlConnection = new SqlConnection(_configuration.GetConnectionString("MyDBConnection").ToString());
        }
        public async Task<SlotHistoryResponse> SlotReportGenerate(SlotHistoryRequest request)
        {
            SlotHistoryResponse response = new SlotHistoryResponse();
            response.IsSuccess = true;
            response.Message = "SuccessFul";

            try
            {

                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }


                string SqlQuery = @"Select * from Booked where cast(@InTime as Date) = @InTime";

                using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                     sqlCommand.Parameters.AddWithValue("@InTime", request.InTime);
                    using (DbDataReader dataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (dataReader.HasRows)
                        {
                            response.getInformation = new List<ReadSlotHistory>();
                            while(await dataReader.ReadAsync())
                            {
                                ReadSlotHistory Data = new ReadSlotHistory();

                              Data.SId = dataReader["SId"] != DBNull.Value ? Convert.ToInt32(dataReader["SId"]) : 0;
                              Data.VehicleNumber = dataReader["VehicleNumber"] != DBNull.Value ? Convert.ToString(dataReader["VehicleNumber"]) : null;        
                              Data.InTime = dataReader["InTime"] != DBNull.Value ? Convert.ToDateTime(dataReader["InTime"]) : DateTime.MinValue;                         
                              Data.OutTime = dataReader["OutTime"] != DBNull.Value ? Convert.ToDateTime(dataReader["OutTime"]) : DateTime.MinValue;

                              response.getInformation.Add(Data);
                            }
                     
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = "Record Not Found";
                            return response;
                        }

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
