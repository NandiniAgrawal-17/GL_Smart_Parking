using Microsoft.Extensions.Configuration;
using SmartParking.Service.Entities.AdminEntities.ViewDetails;
using SmartParking.Service.Interface.AdminInterface;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.DataAccess.Services.AdminServices
{

    public class ViewOperatorDetails:IViewOperatorDetails
    {
        public readonly IConfiguration _configuration;
        public readonly SqlConnection _mySqlConnection;
        public ViewOperatorDetails(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlConnection = new SqlConnection(_configuration.GetConnectionString("MyDBConnection").ToString());
        }
        public async Task<OperatorResponse> GetOperatorDetails()
        {
            OperatorResponse response = new OperatorResponse();
            response.IsSuccess = true;
            response.Message = "SuccessFul";

            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string SqlQuery = @"SELECT * FROM Operator";

                using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;
                    using (DbDataReader dataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (dataReader.HasRows)
                        {
                            response.OperatorList = new List<Operator>();

                            while (await dataReader.ReadAsync())
                            {
                                Operator data = new Operator();

                                data.OperatorId = dataReader["OperatorId"] != DBNull.Value ? Convert.ToInt32(dataReader["OperatorId"]) : 0;
                                data.OperatorName = dataReader["OperatorName"] != DBNull.Value ? Convert.ToString(dataReader["OperatorName"]) : string.Empty;

                                data.ContactNo = dataReader["ContactNo"] != DBNull.Value ? Convert.ToString(dataReader["ContactNo"]) : string.Empty;


                                response.OperatorList.Add(data);
                            }
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
