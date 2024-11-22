using Npgsql;
using System;
using System.Threading.Tasks;

namespace ActionTrack.Core
{
    public class DeviceSettings
    {
        private static readonly string _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=!LoveLilyO23;Database=postgres";

        /// <summary>
        /// Represents the settings data retrieved from the database.
        /// </summary>
        public class SettingsData
        {
            public string DeviceName { get; set; }
            public string ApparatusName { get; set; }
            public string ApparatusType { get; set; }
        }

        /// <summary>
        /// Checks the database for device-specific settings.
        /// </summary>
        public static async Task<SettingsData> GetDeviceSettingsAsync()
        {
            string machineName = Environment.MachineName;
            string query = "SELECT devicename, apperatusname, apperatustype FROM Settings WHERE devicename = @machinename";

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("machinename", machineName);

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return new SettingsData
                                {
                                    DeviceName = reader["devicename"].ToString(),
                                    ApparatusName = reader["apperatusname"].ToString(),
                                    ApparatusType = reader["apperatustype"].ToString()
                                };
                            }
                            else
                            {
                                Console.WriteLine($"No settings found for the device: {machineName}");
                                return null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                throw;
            }
        }
    }
}
