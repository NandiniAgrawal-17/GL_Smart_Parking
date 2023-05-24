using Microsoft.Extensions.Configuration;
using SmartParking.Service.Entities;
using SmartParking.Service.Interface;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.DataAccess.Services
{
    public class GetEmployeeDetails : IGetEmployeeInformation
    {
        public readonly IConfiguration _configuration;
        public readonly SqlConnection _mySqlConnection;

        public GetEmployeeDetails(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlConnection = new SqlConnection(_configuration.GetConnectionString("MyDBConnection").ToString());

        }

        public async Task<GetEmployeeInformationResponse> GetEmployeeInfo()
        {

            GetEmployeeInformationResponse response = new GetEmployeeInformationResponse();
            response.IsSuccess = true;
            response.Message = "SuccessFul";

            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string SqlQuery = @"SELECT * FROM Employee";

                using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;
                    using (DbDataReader dataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (dataReader.HasRows)
                        {
                            response.getInformation = new List<GetInformation>();

                            while (await dataReader.ReadAsync())
                            {
                                GetInformation data = new GetInformation();

                                data.EmployeeId = dataReader["EmployeeId"] != DBNull.Value ? Convert.ToInt32(dataReader["EmployeeId"]) : 0;
                                data.EmployeeName = dataReader["EmployeeName"] != DBNull.Value ? Convert.ToString(dataReader["EmployeeName"]) : string.Empty;
                                data.Email = dataReader["Email"] != DBNull.Value ? Convert.ToString(dataReader["Email"]) : string.Empty;
                                data.ContactNo = dataReader["ContactNo"] != DBNull.Value ? Convert.ToString(dataReader["ContactNo"]) : string.Empty;


                                response.getInformation.Add(data);
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
