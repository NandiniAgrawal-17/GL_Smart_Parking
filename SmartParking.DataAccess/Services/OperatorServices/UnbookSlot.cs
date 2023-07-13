using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SmartParking.Service.Entities.OperatorEntities.Authentication;
using SmartParking.Service.Interface.OperatorInterface;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.DataAccess.Services.OperatorServices
{
    public class UnbookSlot:IUnbookSlot
    {
        public readonly IConfiguration _configuration;
        public readonly SqlConnection _mySqlConnection;
        public UnbookSlot(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlConnection = new SqlConnection(_configuration.GetConnectionString("MyDBConnection").ToString());
        }
        public async Task<BookingModelResponse> BookStatus(BookingModel request)

        {
            BookingModelResponse response = new BookingModelResponse();
            response.IsSuccess = true;
            response.Message = "SuccessFul";
            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }
                //string SqlQuery = "insert into Booked (OutTime) values (@OutTime)";
                string SqlQuery = "UPDATE Booked SET OutTime = @OutTime WHERE SId = @SId";

                using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;
                    sqlCommand.Parameters.AddWithValue("@SId", request.SId);
                    sqlCommand.Parameters.AddWithValue("@OutTime", DateTime.Now);
                    await sqlCommand.ExecuteNonQueryAsync();
                    
                    
                        string SqlQuery1 = @"UPDATE MasterTable
                                   SET Type = 'A'
                                   FROM MasterTable
                                   INNER JOIN Booked ON MasterTable.Id = Booked.SId
                                   WHERE Booked.outtime IS NOT NULL";
                        using (SqlCommand sqlCommand1 = new SqlCommand(SqlQuery1, _mySqlConnection))
                        {
                            sqlCommand1.CommandType = System.Data.CommandType.Text;
                            sqlCommand1.CommandTimeout = 180;

                            await sqlCommand1.ExecuteNonQueryAsync();
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

        public async Task<BookingModelResponse> UpdateStatus(BookingModel request)

        {
            BookingModelResponse response = new BookingModelResponse();
            response.IsSuccess = true;
            response.Message = "SuccessFul";
            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string SqlQuery = @"UPDATE MasterTable
                                   SET Type = 'A'
                                   FROM MasterTable
                                   INNER JOIN Booked ON MasterTable.Id = Booked.SId
                                   WHERE Booked.outtime IS NOT NULL";
                using (SqlCommand sqlCommand = new SqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;
                    
                    await sqlCommand.ExecuteNonQueryAsync();
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
