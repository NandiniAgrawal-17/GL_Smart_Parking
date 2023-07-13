using Microsoft.Extensions.Configuration;
using SmartParking.Service.Entities.OperatorEntities.Authentication;
using SmartParking.Service.Interface.OperatorInterface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartParking.DataAccess.Services.OperatorServices
{
    public class AssignService:IAssign
    {
        public readonly IConfiguration _configuration;
        public readonly SqlConnection _mySqlConnection;
        public AssignService(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlConnection = new SqlConnection(_configuration.GetConnectionString("MyDBConnection").ToString());
        }
        public async Task<BookingModelResponse> BookStatus(BookingModelRequest request)

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
                string query = @"INSERT INTO Booked ( SId, VehicleNumber,InTime)
                             VALUES ( @SId, @VehicleNumber,@InTime)";
                using (SqlCommand command = new SqlCommand(query, _mySqlConnection))
                {
                    command.CommandType = System.Data.CommandType.Text;

                    command.Parameters.AddWithValue("@SId", request.SId);
                    command.Parameters.AddWithValue("@VehicleNumber", request.VehicleNumber);
                    command.Parameters.AddWithValue("@InTime", request.InTime);
                    //command.Parameters.AddWithValue("@OutTime", request.OutTime);

                    int Status = await command.ExecuteNonQueryAsync();
                    if (Status <= 0)
                    {
                        response.IsSuccess = false;
                        response.Message = "SlotAssign Query Not Executed";
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
        public async Task<BookingModelResponse> UpdateMasterType()
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
                string query = @"UPDATE MasterTable
                       SET Type = 'O'
                       WHERE Id IN (
                           SELECT SId
                           FROM Booked
                           WHERE OutTime IS NULL
                       )";
                using (SqlCommand sqlCommand = new SqlCommand(query, _mySqlConnection))
                {
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
