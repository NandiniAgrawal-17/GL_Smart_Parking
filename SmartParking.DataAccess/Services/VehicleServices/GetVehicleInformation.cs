using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SmartParking.Service.Entities.EmployeeEntities;
using SmartParking.Service.Entities.EmployeeEntities.Authentication;
using SmartParking.Service.Interface.VehicleInterface;

namespace SmartParking.DataAccess.Services.VehicleServices
{
    public class GetVehicleInformation:IGetVehicleInformation
    {
        public readonly IConfiguration _configuration;
        public readonly SqlConnection _mySqlConnection;
        public readonly string VehicleNumberRegex = @"^[A-Z]{2}[ -][0-9]{1,2}(?: [A-Z])?(?: [A-Z]{0,2})? [0-9]{4}$";
        public readonly string BharatVehicleNumberRegex = @"^[0-9]{2}BH[0-9]{4}[A-HJ-NP-Z]{1,2}$";


        public GetVehicleInformation(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlConnection = new SqlConnection(_configuration.GetConnectionString("MyDBConnection").ToString());

        }


        public async Task<ManualVehicleNumberResponse> VehicleInformation(ManualVehicleNumberRequest request)
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
                    response.Message = "Vehicle Number Exist";
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

                string SqlQuery = @"Select VehicleType,VehicleModel,VehicleNumber,EmployeeId from EmployeeVehicle where
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

                            response.data = new VehicleInformation();
                         
                            response.data.VehicleType= dataReader[1].ToString();
                            response.data.VehicleModel = dataReader[1].ToString();
                             response.data.VehicleNumber = dataReader[1].ToString();
                            /*response.data.EmployeeId = Convert.ToInt32(EmployeeId);*/

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
