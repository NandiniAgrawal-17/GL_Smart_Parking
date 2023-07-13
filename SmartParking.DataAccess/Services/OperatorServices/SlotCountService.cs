using Microsoft.Extensions.Configuration;
using SmartParking.Service.Entities.OperatorEntities.Authentication;
using SmartParking.Service.Interface.OperatorInterface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.DataAccess.Services.OperatorServices
{
    public class SlotCountService:ICount
    {
        public readonly IConfiguration _configuration;
        public readonly SqlConnection _mySqlConnection;
        public SlotCountService(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlConnection = new SqlConnection(_configuration.GetConnectionString("MyDBConnection").ToString());
        }

        public async Task<SlotAssignResponse> SlotNumber()
        {
            SlotAssignResponse response = new SlotAssignResponse();
            response.IsSuccess = true;
            response.Message = "SuccessFul";
            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string SqlQuery = @"SELECT COUNT(*)FROM MasterTable;";
                using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;

                    int rowCount = (int)sqlCommand.ExecuteScalar();

                    response.totalSlot = rowCount;


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
