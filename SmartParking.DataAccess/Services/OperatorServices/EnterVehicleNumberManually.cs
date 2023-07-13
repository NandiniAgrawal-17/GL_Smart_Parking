using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SmartParking.DataAccess.Services.EmployeeServices;
using SmartParking.Service.Entities.EmployeeEntities;
using SmartParking.Service.Entities.EmployeeEntities.Authentication;
using SmartParking.Service.Interface.VehicleInterface;

namespace SmartParking.DataAccess.Services.OperatorServices
{
    public class EnterVehicleNumberManually : IManualVehicleNumber
    {
        public readonly IConfiguration _configuration;
        public readonly SqlConnection _mySqlConnection;
        public readonly string VehicleNumberRegex = @"^[A-Z]{2}[ -][0-9]{1,2}(?: [A-Z])?(?: [A-Z]{0,2})? [0-9]{4}$";
        public readonly string BharatVehicleNumberRegex = @"^[0-9]{2}BH[0-9]{4}[A-HJ-NP-Z]{1,2}$";


        public EnterVehicleNumberManually(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlConnection = new SqlConnection(_configuration.GetConnectionString("MyDBConnection").ToString());

        }


        public async Task<ManualVehicleNumberResponse> EnterVehicleNumber(ManualVehicleNumberRequest request)
        {
            ManualVehicleNumberResponse response = new ManualVehicleNumberResponse();
            response.IsSuccess = true;
            response.Message = "SuccessFul";

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

                string SqlQuery = @"Select VehicleNumber from EmployeeVehicle where
                                    VehicleNumber = @VehicleNumber";

                using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.Parameters.AddWithValue("@VehicleNumber", request.VehicleNumber);

                    using (DbDataReader dataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (dataReader.HasRows)
                        {
                            await dataReader.ReadAsync();
                            response.Message = "Vehicle Number Exist";

                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = "Vehicle Number Dosn't Exist,Please Register";
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
