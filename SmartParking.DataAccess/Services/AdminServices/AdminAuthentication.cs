using Microsoft.Extensions.Configuration;
using SmartParking.Service.Entities.AdminEntities.Authentication;
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
    public class AdminAuthentication : IAdminAuthentication
    {
        public readonly IConfiguration _configuration;
        public readonly SqlConnection _mySqlConnection;
        public AdminAuthentication(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlConnection = new SqlConnection(_configuration.GetConnectionString("MyDBConnection").ToString());
        }
        public async Task<AdminRegisterResponse> RegisterAdmin(AdminRegisterRequest request)
        {
            AdminRegisterResponse response = new AdminRegisterResponse();
            response.IsSuccess = true;
            response.Message = "SuccessFul";

            try
            {

                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                if (request.Password != request.ConfirmPassword)
                {
                    response.IsSuccess = false;
                    response.Message = "Password Not Match";
                    return response;
                }

                string SqlQuery = @"INSERT INTO Admin 
                                    (AdminId,AdminName,Password,Email,ContactNo) Values 
                                    (@AdminId,@AdminName, @Password,@Email,@ContactNo);";

                using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    // sqlCommand.CommandTimeout = 180;
                    sqlCommand.Parameters.AddWithValue("@AdminId", request.AdminId);
                    sqlCommand.Parameters.AddWithValue("@AdminName", request.AdminName);
                    sqlCommand.Parameters.AddWithValue("@Password", request.Password);
                    sqlCommand.Parameters.AddWithValue("@Email", request.Email);
                    sqlCommand.Parameters.AddWithValue("@ContactNo", request.ContactNo);


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
            throw new NotImplementedException();
        }

        public async Task<AdminLoginResponse> LoginAdmin(AdminLoginRequest request)
        {
            AdminLoginResponse response = new AdminLoginResponse();

            response.IsSuccess = true;
            response.Message = "SuccessFul";

            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string SqlQuery = @"SELECT AdminId,Password
                                    FROM Admin
                                    WHERE AdminId=@AdminId AND Password=@Password";

                using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;

                    sqlCommand.Parameters.AddWithValue("@AdminId", request.AdminId);
                    sqlCommand.Parameters.AddWithValue("@Password", request.Password);
                    using (DbDataReader dataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (dataReader.HasRows)
                        {
                            await dataReader.ReadAsync();
                            response.Message = "Login Sucessful";

                            response.data = new AdminLoginInformation();
                            response.AdminToken = new AdminToken();

                            AdminTokenGenerate tokenGenerate = new AdminTokenGenerate(_configuration);

                            response.data.AdminId = Convert.ToInt32(request.AdminId);
                            response.data.Email = dataReader[1].ToString();


                            response.AdminToken.JWTToken = tokenGenerate.GenerateJWTToken(response.data.AdminId);
                            response.AdminToken.AdminRefreshToken = tokenGenerate.GenerateRefresh();

                            tokenGenerate.AdminTokenDB(response);
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = "Login Unsuccesful";
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
            throw new NotImplementedException();
        }
    }
}
