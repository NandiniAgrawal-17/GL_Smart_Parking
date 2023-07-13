using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartParking.DataAccess.Services.EmployeeServices;
using SmartParking.Service.Entities.EmployeeEntities.Authentication;
using SmartParking.Service.Entities.OperatorEntities.Authentication;
using SmartParking.Service.Interface.OperatorInterface;
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

namespace SmartParking.DataAccess.Services.OperatorServices
{
    public class OperatorAuthentication:IOperatorAuthentication
    {
        public readonly IConfiguration _configuration;
        public readonly SqlConnection _mySqlConnection;
        public OperatorAuthentication(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlConnection = new SqlConnection(_configuration.GetConnectionString("MyDBConnection").ToString());
        }

        public async Task<OperatorLoginResponse> LoginOperator(OperatorLoginRequest request)
        {
            OperatorLoginResponse response = new OperatorLoginResponse();

            response.IsSuccess = true;
            response.Message = "SuccessFul";

            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string SqlQuery = @"SELECT 1
                                    FROM Operator
                                    WHERE OperatorId=@OperatorId AND Password=@Password";

                using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;

                    sqlCommand.Parameters.AddWithValue("@OperatorId", request.OperatorId);
                    // sqlCommand.Parameters.AddWithValue("@EmployeeName", request.EmployeeName);
                    sqlCommand.Parameters.AddWithValue("@Password", request.Password);

                    using (DbDataReader dataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (dataReader.HasRows)
                        {
                            await dataReader.ReadAsync();
                            response.Message = "Login Sucessful";
                            response.data = new OperatorLoginInformation();
                            response.OperatorToken = new OperatorToken();
                            OperatorTokenGenerate tokenGenerate = new OperatorTokenGenerate(_configuration);
                            response.data.OperatorId = Convert.ToInt32(request.OperatorId);

                            response.OperatorToken.JWTToken = tokenGenerate.GenerateJWTToken(response.data.OperatorId);
                            response.OperatorToken.OperatorRefreshToken = tokenGenerate.GenerateRefresh();


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



        public async Task<OperatorLoginResponse> RefreshToken(OperatorToken operatortokens)
        {
            OperatorLoginResponse response = new OperatorLoginResponse();
            response.IsSuccess = true;
            response.Message = "SuccessFul";

            if (operatortokens is null)
            {
                //return BadRequest("Invalid client request");
                response.IsSuccess = false;
                response.Message = "Invalid client request";
            }

            string? accessToken = operatortokens.JWTToken;
            string? refreshToken = operatortokens.OperatorRefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {

                response.IsSuccess = false;
                response.Message = "Invalid access token or refresh token";
            }


            string OperatorId = principal.Identity.Name;
            OperatorLoginResponse tokenresponse = new OperatorLoginResponse();
            tokenresponse.data = new OperatorLoginInformation();
            tokenresponse.OperatorToken = new OperatorToken();

            //var user = await _userManager.FindByNameAsync(username);
            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string SqlQuery = "SELECT * FROM OperatorRefreshToken WHERE OperatorId =" + OperatorId;
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
                                tokenresponse.data.OperatorId = dataReader["OperatorId"] != DBNull.Value ? Convert.ToInt32(dataReader["OperatorId"]) : 0;
                                tokenresponse.OperatorToken.OperatorRefreshToken = dataReader["RefreshToken"] != DBNull.Value ? (dataReader["RefreshToken"].ToString()) : null;
                                tokenresponse.OperatorToken.Expire = dataReader["Expire"] != DBNull.Value ? DateTime.Parse(dataReader["Expire"].ToString()) : null;
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

            if (OperatorId == null || tokenresponse.OperatorToken.OperatorRefreshToken != refreshToken || tokenresponse.OperatorToken.Expire <= DateTime.Now)
            {

                response.IsSuccess = false;
                response.Message = "Invalid access token or refresh token";
            }
            response.IsSuccess = true;
            response.Message = "SuccessFul";
            response.OperatorToken = new OperatorToken();

            var newAccessToken = CreateToken(principal.Claims.ToList());
            var newRefreshToken = GenerateRefresh();

            response.OperatorToken.JWTToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken);
            response.OperatorToken.OperatorRefreshToken = newRefreshToken;
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


        public async Task RefreshTokenDB(OperatorLoginResponse response)
        {
            try
            {

                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }


                string SqlQuery = @"Update OperatorRefreshToken set
                                   JWTToken=@JWTToken, RefreshToken=@RefreshToken,Expire=@Expire WHERE OperatorId=" + response.data.OperatorId;


                using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    //sqlCommand.CommandTimeout = 180;
                    sqlCommand.Parameters.AddWithValue("@JWTToken", response.OperatorToken.JWTToken);
                    sqlCommand.Parameters.AddWithValue("@RefreshToken", response.OperatorToken.OperatorRefreshToken);

                    sqlCommand.Parameters.AddWithValue("@Expire", response.OperatorToken.Expire);


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
