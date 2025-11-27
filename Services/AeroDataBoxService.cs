using Hackathon_API.Models;
using Microsoft.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Net.Http.Headers;
using System.Text.Json;
using Npgsql;

namespace Hackathon_API.Services
{
    public class AeroDataBoxService
    {
        private readonly HttpClient _http;
        private readonly string _connectionString;


        public AeroDataBoxService(HttpClient http, IConfiguration config)
        {
            _http = http;
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<AirportDelay> GetAirportDelays(string country, string date)
        {
            string iataCode =await GetIATACode(country);
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"https://aerodatabox.p.rapidapi.com/airports/iata/{iataCode.Trim()}/delays/{date}"
            );

            // Add RapidAPI headers              --4bc0a93f56msh1fc78c95f2515adp17bc1ajsn7794c93b8e8f
            request.Headers.Add("x-rapidapi-key", "cd2ebcd6b6mshb39e3c6f7fde03bp14ce6fjsnae006f6b92e5");
            request.Headers.Add("x-rapidapi-host", "aerodatabox.p.rapidapi.com");

            var response = await _http.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            //return JsonSerializer.Deserialize<object>(json);
            using JsonDocument jsonD = JsonDocument.Parse(json);

            var dDelay = jsonD.RootElement.GetProperty("departuresDelayInformation");
            var aDelay = jsonD.RootElement.GetProperty("arrivalsDelayInformation");
            string depDelay = null;
            string arrDelay = null;
            // var forecast = json.RootElement.GetProperty("forecast").GetProperty("forecastday")[0].GetProperty("day");
            //foreach (var delay in dDelay.EnumerateArray())
            //{
            try
            {
                depDelay = dDelay.GetProperty("delayIndex").GetDecimal().ToString();
            }
            catch
            {
                depDelay = "0";
            }
            try
            {
                arrDelay = aDelay.GetProperty("delayIndex").GetDecimal().ToString();
            }
            catch
            {
                arrDelay = "0";
            }
            //}
            //foreach (var dely in aDelay.EnumerateArray())
            //{
            
            //}

            return new AirportDelay
            {
                ArrivalDelay = arrDelay,
                DepartureDelay = depDelay
            };
        }

        public async Task<string> GetIATACode(string country)
        {
            string iata = "DC";
            //using (SqlConnection conn = new SqlConnection(_connectionString))
            //{
            //    conn.Close();
            //    conn.Open();

            //    string query = @"
            //         select IATACode from IATA_Mst
            //        WHERE Upper(Country) = @Country
            //    ";

            //    using (SqlCommand cmd = new SqlCommand(query, conn))
            //    {
            //        cmd.Parameters.AddWithValue("@Country", country.ToUpper());

            //        using (SqlDataReader reader = cmd.ExecuteReader())
            //        {
            //            if (reader.Read())
            //            {
            //                try
            //                {
            //                    iata = reader["IATACode"]?.ToString();
            //                }
            //                catch (Exception ex)
            //                {
            //                }

            //            }
            //        }
            //    }
            //}
            try 
            { 
                using var connection = new NpgsqlConnection(_connectionString);
                connection.OpenAsync();

                Console.WriteLine("Connected to Supabase Database!");

                string sql = "select IATACode from IATA_Mst WHERE Upper(Country) = @Country;";

                using var command = new NpgsqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Country", country.ToUpper());
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    iata = reader["IATACode"]?.ToString();
                    Console.WriteLine($"ID: {reader["id"]}, Name: {reader["name"]}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return iata;
        }
    }
}
