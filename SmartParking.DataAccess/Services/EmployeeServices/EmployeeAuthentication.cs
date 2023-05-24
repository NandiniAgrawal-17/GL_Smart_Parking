﻿using Microsoft.Extensions.Configuration;
using SmartParking.Service.Entities.EmployeeEntities.Authentication;
using SmartParking.Service.Interface.EmployeeInterface;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.DataAccess.Services.EmployeeServices
{
    public class EmployeeAuthentication : IEmployeeAuthentication
    {

        public readonly IConfiguration _configuration;
        public readonly SqlConnection _mySqlConnection;

        public EmployeeAuthentication(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlConnection = new SqlConnection(_configuration.GetConnectionString("MyDBConnection").ToString());

        }


        public async Task<EmployeeRegisterResponse> RegisterEmployee(EmployeeRegisterRequest request)
        {
            EmployeeRegisterResponse response = new EmployeeRegisterResponse();
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

                string SqlQuery = @"INSERT INTO Employee
                                    (EmployeeId,EmployeeName,Password,Email,ContactNo) Values 
                                    (@EmployeeId,@EmployeeName, @Password,@Email,@ContactNo);";

                using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    // sqlCommand.CommandTimeout = 180;
                    sqlCommand.Parameters.AddWithValue("@EmployeeId", request.EmployeeId);
                    sqlCommand.Parameters.AddWithValue("@EmployeeName", request.EmployeeName);
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
        }



        public async Task<EmployeeLoginResponse> LoginEmployee(EmployeeLoginRequest request)
        {
            EmployeeLoginResponse response = new EmployeeLoginResponse();

            response.IsSuccess = true;
            response.Message = "SuccessFul";

            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string SqlQuery = @"SELECT EmployeeId,Email,Password
                                    FROM Employee
                                    WHERE EmployeeId=@EmployeeId AND Password=@Password";

                using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;

                    sqlCommand.Parameters.AddWithValue("@EmployeeId", request.EmployeeId);
                    // sqlCommand.Parameters.AddWithValue("@EmployeeName", request.EmployeeName);
                    sqlCommand.Parameters.AddWithValue("@Password", request.Password);
                    using (DbDataReader dataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (dataReader.HasRows)
                        {
                            await dataReader.ReadAsync();
                            response.Message = "Login Sucessful";
                            response.data = new LoginInformation();
                            response.EmployeeToken = new EmployeeTokens();
                            EmployeeTokenGenerate tokenGenerate = new EmployeeTokenGenerate(_configuration);
                            response.data.EmployeeId = Convert.ToInt32(request.EmployeeId);
                            response.data.Email = dataReader[1].ToString();

                            response.EmployeeToken.JWTToken = tokenGenerate.GenerateJWTToken(response.data.EmployeeId);
                            response.EmployeeToken.EmployeeRefreshToken = tokenGenerate.GenerateRefresh();


                            tokenGenerate.RefreshTokenDB(response);
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
        }

    }
}