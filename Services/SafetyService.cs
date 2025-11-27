using Hackathon_API.Models;
using Microsoft.Data;
using Microsoft.Data.SqlClient;
using Npgsql;
using System.Text.Json;

namespace Hackathon_API.Services
{
    public class SafetyService
    {
        private readonly string _connectionString;

        public SafetyService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<SafetyIndexInfo> GetSafetyAsync(string country)
        {
            string level = null;
            string remarks = null;

            //using (SqlConnection conn = new SqlConnection(_connectionString))
            //{
            //    await conn.OpenAsync();

            //    string query = @"
            //        select [Risk Index],Remarks from SafetyIndex_Mst
            //        WHERE Destination = @Country

            //    ";

            //    using (SqlCommand cmd = new SqlCommand(query, conn))
            //    {
            //        cmd.Parameters.AddWithValue("@Country", country.ToUpper());

            //        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            //        {
            //            if (await reader.ReadAsync())
            //            {
            //                // JSON lists from DB (Vaccinations & Diseases)
            //                level = reader["Risk Index"]?.ToString();
            //                remarks = reader["Remarks"]?.ToString();

            //                //healthInfo.MalariaRisk = reader["MalariaRisk"]?.ToString();
            //                //healthInfo.Precautions = reader["Precautions"]?.ToString();
            //            }
            //        }
            //    }
            //}

            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                connection.OpenAsync();

                Console.WriteLine("Connected to Supabase Database!");

                string sql = "select [Risk Index],Remarks from SafetyIndex_Mst WHERE Destination = @Country;";

                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Country", country.ToUpper());
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    // JSON lists from DB (Vaccinations & Diseases)
                    level = reader["Risk Index"]?.ToString();
                    remarks = reader["Remarks"]?.ToString();
                    Console.WriteLine($"ID: {reader["id"]}, Name: {reader["name"]}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return new SafetyIndexInfo
            {
                Level = level,
                Remarks = remarks
            };
        }
    }
}
