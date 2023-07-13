using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SmartParking.Service.Entities.EmployeeEntities;
using SmartParking.Service.Entities.EmployeeEntities.Authentication;
using SmartParking.Service.Interface.VehicleInterface;

namespace SmartParking.DataAccess.Services.OperatorServices
{
    public class RegisterNewVehicle : IRegisterVehicles
    {
        public readonly IConfiguration _configuration;
        public readonly SqlConnection _mySqlConnection;
        public readonly string VehicleNumberRegex = @"^[A-Z]{2}[ -][0-9]{1,2}(?: [A-Z])?(?: [A-Z]{0,2})? [0-9]{4}$";
        public readonly string BharatVehicleNumberRegex = @"^[0-9]{2}BH[0-9]{4}[A-HJ-NP-Z]{1,2}$";
     
        public RegisterNewVehicle(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlConnection = new SqlConnection(_configuration.GetConnectionString("MyDBConnection").ToString());

        }


        public async Task<EmployeeVehicleResponse> RegisterVehicle(EmployeeVehicleRequest request)
        {
            EmployeeVehicleResponse response = new EmployeeVehicleResponse();
            response.IsSuccess = true;
            response.Message = "Vehicle Registred";

            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                if ((Regex.IsMatch(request.VehicleNumber, BharatVehicleNumberRegex)))
                {
                    response.IsSuccess = true;
                    response.Message = "Vehicle Registred";
                    return response;
                }
                else
                {
                    if (!(Regex.IsMatch(request.VehicleNumber, VehicleNumberRegex)))
                    {
                        response.IsSuccess = false;
                        response.Message = "Vehicle Number not in Valid Format";
                        return response;
                    }
                }
              
                string SqlQuery = @"INSERT INTO EmployeeVehicle
                                    (VehicleType,VehicleModel,VehicleNumber,EmployeeId) Values 
                                    (@VehicleType,@VehicleModel,@VehicleNumber,@EmployeeId);";

                using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    // sqlCommand.CommandTimeout = 180;
                    sqlCommand.Parameters.AddWithValue("@VehicleType", request.VehicleType);
                    sqlCommand.Parameters.AddWithValue("@VehicleModel", request.VehicleModel);
                    sqlCommand.Parameters.AddWithValue("@VehicleNumber", request.VehicleNumber);
                    sqlCommand.Parameters.AddWithValue("@EmployeeId", request.EmployeeId);
               


                    int Status = await sqlCommand.ExecuteNonQueryAsync();

                    if (Status <= 0)
                    {
                        response.IsSuccess = false;
                        response.Message = "Vehicle Registred Sucessfully!";
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
