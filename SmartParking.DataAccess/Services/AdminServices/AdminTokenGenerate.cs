﻿using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SmartParking.Service.Entities.AdminEntities.Authentication;
using SmartParking.Service.Interface.AdminInterface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.DataAccess.Services.AdminServices
{
    public class AdminTokenGenerate : IAdminToken
    {
        private readonly IConfiguration _configuration;
        public readonly SqlConnection _mySqlConnection;
        private readonly byte[] key;
        public AdminTokenGenerate(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlConnection = new SqlConnection(_configuration.GetConnectionString("MyDBConnection").ToString());

        }

        public string GenerateJWTToken(int Id,string Role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            var claims = new[] {
         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
         new Claim(JwtRegisteredClaimNames.Sid,Convert.ToString(Id)),
         new Claim(ClaimTypes.Role, Role),
         //  new Claim(JwtRegisteredClaimNames.Email, Email),
         
         new Claim("Date", DateTime.Now.ToString()),
         };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
           _configuration["Jwt:Audiance"],
           claims,    //null original value
           expires: DateTime.Now.AddMinutes(120),


           //notBefore:
           signingCredentials: credentials);

            string Data = new JwtSecurityTokenHandler().WriteToken(token); //return access token 
            return Data;

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


        public async Task AdminTokenDB(AdminLoginResponse response)
        {
            try
            {

                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }


                string SqlQuery = "insert into AdminToken (AdminId,JWTToken,RefreshToken,Created,Expire) Values (@AdminId,@JWTToken,@RefreshToken, @Created,@Expire);";

                using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    //sqlCommand.CommandTimeout = 180;
                    sqlCommand.Parameters.AddWithValue("@AdminId", response.data.AdminId);
                    sqlCommand.Parameters.AddWithValue("@JWTToken", response.AdminToken.JWTToken);
                    sqlCommand.Parameters.AddWithValue("@RefreshToken", response.AdminToken.AdminRefreshToken);
                    sqlCommand.Parameters.AddWithValue("@Created", DateTime.Now);
                    sqlCommand.Parameters.AddWithValue("@Expire", DateTime.Now.AddMonths(1));

                    try
                    {
                        int Status =await sqlCommand.ExecuteNonQueryAsync();
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }

        }
    }
}
