using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartParking.Service.Entities.AdminEntities.ViewDetails;
using SmartParking.Service.Entities.EmployeeEntities.Authentication;
using SmartParking.Service.Interface.EmployeeInterface;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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




       
        public async Task<EmployeeLoginResponse> RefreshToken(EmployeeTokens employeetoken)
        {
            EmployeeLoginResponse response = new EmployeeLoginResponse();
            response.IsSuccess = true;
            response.Message = "SuccessFul";
            
            if (employeetoken is null)
            {
                //return BadRequest("Invalid client request");
                response.IsSuccess = false;
                response.Message = "Invalid client request";
            }

            string? accessToken = employeetoken.JWTToken;
            string? refreshToken = employeetoken.EmployeeRefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
               
                response.IsSuccess = false;
                response.Message = "Invalid access token or refresh token";
            }


            string EmployeeId = principal.Identity.Name;
            EmployeeLoginResponse tokenresponse = new EmployeeLoginResponse();
            tokenresponse.data = new LoginInformation();
            tokenresponse.EmployeeToken = new EmployeeTokens();

            //var user = await _userManager.FindByNameAsync(username);
            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string SqlQuery = "SELECT * FROM EmployeeRefreshToken WHERE EmployeeId ="+ EmployeeId;
                using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    //sqlCommand.CommandTimeout = 180;
                    using (DbDataReader dataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        
                        if (dataReader.HasRows)
                        {
                            
                            /* response1.Vehicles = new List<EmployeeVehicleResponse>();*/
                            while (await dataReader.ReadAsync())
                            {

                                /*EmployeeVehicle vehicle = new EmployeeVehicle();*/
                                tokenresponse.data.EmployeeId = dataReader["EmployeeId"] != DBNull.Value ? Convert.ToInt32(dataReader["EmployeeId"]) : 0;
                                tokenresponse.EmployeeToken.EmployeeRefreshToken = dataReader["RefreshToken"] != DBNull.Value ? (dataReader["RefreshToken"].ToString()) : null;
                                tokenresponse.EmployeeToken.Expire = dataReader["Expire"] != DBNull.Value ? DateTime.Parse(dataReader["Expire"].ToString()):null;
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

            if (EmployeeId == null || tokenresponse.EmployeeToken.EmployeeRefreshToken != refreshToken || tokenresponse.EmployeeToken.Expire <= DateTime.Now)
            {
                
                response.IsSuccess = false;
                response.Message = "Invalid access token or refresh token";
            }
            response.IsSuccess = true;
            response.Message = "SuccessFul";
            response.EmployeeToken = new EmployeeTokens();

            var newAccessToken = CreateToken(principal.Claims.ToList());
            var newRefreshToken = GenerateRefresh();
           
            response.EmployeeToken.JWTToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken);
                response.EmployeeToken.EmployeeRefreshToken = newRefreshToken;
            RefreshTokenDB(response);
            return response;

        }



        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {

                ValidateAudience = false,
                 ValidateIssuer = false,
                 ValidateIssuerSigningKey = true,
                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"])),
                 
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }

        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }



        public string GenerateRefresh()
        {
            var randomNumber = new byte[32];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }


        public async Task RefreshTokenDB(EmployeeLoginResponse response)
        {
            try
            {

                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }


                string SqlQuery = @"Update RefreshToken set
                                   JWTToken=@JWTToken, RefreshToken=@RefreshToken,Expire=@Expire WHERE EmployeeId=" + response.data.EmployeeId;
                                    

                using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    //sqlCommand.CommandTimeout = 180;
                    sqlCommand.Parameters.AddWithValue("@JWTToken", response.EmployeeToken.JWTToken);
                    sqlCommand.Parameters.AddWithValue("@RefreshToken", response.EmployeeToken.EmployeeRefreshToken);
                   
                    sqlCommand.Parameters.AddWithValue("@Expire", response.EmployeeToken.Expire);


                    int Status = await sqlCommand.ExecuteNonQueryAsync();
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

        }

    }
}
