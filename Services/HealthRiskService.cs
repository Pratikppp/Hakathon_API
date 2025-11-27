using Microsoft.Data;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using Supabase;
using Npgsql;
using Hackathon_API.Models;

namespace Hackathon_API.Services
{
    public class HealthRiskService
    {
        private readonly string _connectionString;

        public HealthRiskService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<HealthInfo> GetHealthAsync(string country)
        {
            var healthInfo = new HealthInfo
            {
                Health_Risk = new List<string>(),
                Description = new List<string>(),
                //MalariaRisk = "Unknown",
                //Precautions = "No data available"
            };

            //using (NpgsqlConnection conn = new NpgsqlConnection(_connectionString))
            //{
            //    try
            //    {
            //        await conn.OpenAsync();
            //    }
            //    catch
            //    {

            //    }

            //    string query = @"
            //        select Health_Risks,Precautions from Health_Mst
            //        WHERE Country = @Country OR Country_Code = @Country
            //    ";

            //    using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
            //    {
            //        CC

            //        using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
            //        {
            //            if (await reader.ReadAsync())
            //            {
            //                // JSON lists from DB (Vaccinations & Diseases)
            //                string healthRisksJson = reader["Health_Risks"]?.ToString();
            //                string precautionsJson = reader["Precautions"]?.ToString();
            //                List<string> list = healthRisksJson.Split(',', StringSplitOptions.RemoveEmptyEntries)
            //                .Select(s => s.Trim())
            //                .ToList();
            //                healthInfo.Health_Risk = string.IsNullOrWhiteSpace(healthRisksJson)
            //                    ? new List<string>()
            //                    : healthRisksJson.Split(',', StringSplitOptions.RemoveEmptyEntries)
            //                .Select(s => s.Trim())
            //                .ToList();

            //                healthInfo.Description = string.IsNullOrWhiteSpace(precautionsJson)
            //                    ? new List<string>()
            //                    : precautionsJson.Split(',', StringSplitOptions.RemoveEmptyEntries)
            //                .Select(s => s.Trim())
            //                .ToList();

            //                // Normal text fields
            //                //healthInfo.MalariaRisk = reader["MalariaRisk"]?.ToString();
            //                //healthInfo.Precautions = reader["Precautions"]?.ToString();
            //            }
            //        }
            //    }
            //}

            try
            {
                using var connection = new NpgsqlConnection(_connectionString);
                await connection.OpenAsync();

                Console.WriteLine("Connected to Supabase Database!");

                string sql = "select Health_Risks,Precautions from Health_Mst WHERE Country = @Country OR Country_Code = @Country;";

                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Country", country.ToUpper());
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    //JSON lists from DB (Vaccinations & Diseases)
                    string healthRisksJson = reader["Health_Risks"]?.ToString();
                    string precautionsJson = reader["Precautions"]?.ToString();
                    List<string> list = healthRisksJson.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList();
                    healthInfo.Health_Risk = string.IsNullOrWhiteSpace(healthRisksJson)
                        ? new List<string>()
                        : healthRisksJson.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList();

                    healthInfo.Description = string.IsNullOrWhiteSpace(precautionsJson)
                        ? new List<string>()
                        : precautionsJson.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return healthInfo;
        }
    }
}
