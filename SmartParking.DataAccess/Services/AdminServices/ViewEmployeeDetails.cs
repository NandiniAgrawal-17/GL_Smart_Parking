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
    public class ViewEmployeeDetails : IViewEmployeeDetails
    {
        public readonly IConfiguration _configuration;
        public readonly SqlConnection _mySqlConnection;

        public ViewEmployeeDetails(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlConnection = new SqlConnection(_configuration.GetConnectionString("MyDBConnection").ToString());

        }
        public async Task<EmployeeDetailsResponse> GetAllEmployees()
        {
            EmployeeDetailsResponse response = new EmployeeDetailsResponse();
            response.IsSuccess = true;
            response.Message = "SuccessFul";
            /*EmployeeVehicleResponse response1 = new EmployeeVehicleResponse();*/
            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string SqlQuery = "SELECT * FROM ViewEmployeeDetails";
                using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;
                    using (DbDataReader dataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (dataReader.HasRows)
                        {
                            response.GetEmployeeDetails = new List<EmployeeDetails>();
                            /* response1.Vehicles = new List<EmployeeVehicleResponse>();*/
                            while (await dataReader.ReadAsync())
                            {
                                EmployeeDetails data = new EmployeeDetails();
                                /*EmployeeVehicle vehicle = new EmployeeVehicle();*/
                                data.EmployeeId = dataReader["EmployeeId"] != DBNull.Value ? Convert.ToInt32(dataReader["EmployeeId"]) : 0;
                                data.EmployeeName = dataReader["EmployeeName"] != DBNull.Value ? Convert.ToString(dataReader["EmployeeName"]) : string.Empty;
                                data.Email = dataReader["Email"] != DBNull.Value ? Convert.ToString(dataReader["Email"]) : string.Empty;
                                data.ContactNo = dataReader["ContactNo"] != DBNull.Value ? Convert.ToString(dataReader["ContactNo"]) : string.Empty;
                                data.VehicleNumber = dataReader["VehicleNumber"] != DBNull.Value ? Convert.ToString(dataReader["VehicleNumber"]) : string.Empty;
                                data.VehicleId = dataReader["VehicleId"] != DBNull.Value ? Convert.ToInt32(dataReader["VehicleId"]) : 0;
                                data.VehicleType = dataReader["VehicleNumber"] != DBNull.Value ? Convert.ToString(dataReader["VehicleType"]) : string.Empty;
                                data.VehicleModel = dataReader["VehicleNumber"] != DBNull.Value ? Convert.ToString(dataReader["VehicleModel"]) : string.Empty;

                                response.GetEmployeeDetails.Add(data);

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
        /* public async Task<EmployeeRegisterRequest> GetAllEmployees1()
         {
             List<EmployeeRegisterRequest> response = new List<EmployeeRegisterRequest>();


             try
             {
                 if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                 {
                     await _mySqlConnection.OpenAsync();
                 }

                 string SqlQuery = "SELECT * FROM ViewEmployeeDetails";
                 using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _mySqlConnection))
                 {
                     sqlCommand.CommandType = System.Data.CommandType.Text;
                     sqlCommand.CommandTimeout = 180;
                     using (DbDataReader dataReader = await sqlCommand.ExecuteReaderAsync())
                     {
                         if (dataReader.HasRows)
                         {


                             while (await dataReader.ReadAsync())
                             {
                                 EmployeeRegisterRequest data = new EmployeeRegisterRequest();

                                 data.EmployeeId = dataReader["EmployeeId"] != DBNull.Value ? Convert.ToInt32(dataReader["EmployeeId"]) : 0;
                                 data.EmployeeName = dataReader["EmployeeName"] != DBNull.Value ? Convert.ToString(dataReader["EmployeeName"]) : string.Empty;
                                 data.Email = dataReader["Email"] != DBNull.Value ? Convert.ToString(dataReader["Email"]) : string.Empty;
                                 data.ContactNo = dataReader["ContactNo"] != DBNull.Value ? Convert.ToString(dataReader["ContactNo"]) : string.Empty;
                                 data.VehicleNumber = dataReader["VehicleNumber"] != DBNull.Value ? Convert.ToString(dataReader["VehicleNumber"]) : string.Empty;


                                 response.Add(data);
                             }
                         }
                     }
                 }
             }

             catch (Exception ex)
             {

             }
             finally
             {
                 await _mySqlConnection.CloseAsync();
                 await _mySqlConnection.DisposeAsync();
             }

             return response;
         }*/
    }
}
