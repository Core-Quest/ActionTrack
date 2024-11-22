using System;
using System.IO;
using System.Data;
using Npgsql;
using System.Threading.Tasks;
using System.Collections.Generic;
using static ActionTrack.Core.DeviceSettings;
using Syncfusion.Windows.Tools;
using System.Diagnostics.Metrics;

namespace ActionTrack.Core
{
    internal class AuthenticationService
    {
        private readonly string _loginFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ActionTrack", "userLoginData.txt");
        private readonly string _connectionString = "Host=localhost;Port=5432;Username=postgres;Password=!LoveLilyO23;Database=postgres";
        private bool _isLoggedIn = false;
        private string _currentToken = null; // Temporary storage for the current token

        public AuthenticationService()
        {
            // Ensure the directory exists
            var directory = Path.GetDirectoryName(_loginFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public bool IsUserLoggedIn()
        {
            if (!_isLoggedIn)
            {
                // Block and wait for the asynchronous method to complete
                var (isLoggedIn, _) = IsUserLoggedInAsync().GetAwaiter().GetResult();
                _isLoggedIn = isLoggedIn;
            }
            return _isLoggedIn;
        }


        public async Task<(bool IsLoggedIn, string UserToken)> IsUserLoggedInAsync()
        {
            string userToken;
            bool isLoggedIn;

            // Check if the login file exists
            if (!File.Exists(_loginFilePath))
                return (false, null);

            // Read the token from the file
            userToken = File.ReadAllText(_loginFilePath);

            // If the token from the file is invalid, try using a temporary token
            if (string.IsNullOrWhiteSpace(userToken))
                userToken = _currentToken; // Use the temporary token if available

            // If no valid token is found, return false and null
            if (string.IsNullOrWhiteSpace(userToken))
                return (false, null);

            // Verify the token asynchronously
            isLoggedIn = await VerifyUserTokenAsync(userToken);

            // Update the global logged-in state
            _isLoggedIn = isLoggedIn;

            // Return the result as a tuple
            return (isLoggedIn, userToken);
        }


        private async Task<bool> VerifyUserTokenAsync(string userToken)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT COUNT(1) FROM UserData WHERE userToken = @userToken";
                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@userToken", userToken);
                    int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                    _isLoggedIn = count > 0;
                    return _isLoggedIn;
                }
            }
        }

        public async Task<User> GetUserDataAsync(string token)
        {
            string effectiveToken;
            // Use provided token or fallback to the current token
            if (token == null)
            {
                effectiveToken = _currentToken;
            }
            else { effectiveToken = token; }

            _currentToken = effectiveToken;
            Console.WriteLine($"Token: {token}");
            Console.WriteLine($"Effective Token: {effectiveToken}");
            Console.WriteLine($"Set Current Token: {_currentToken}");
            // Validate the token
            if (string.IsNullOrWhiteSpace(effectiveToken))
                return null;

            // Define the query
            string query = "SELECT username, password, userToken, department FROM UserData WHERE userToken = @token LIMIT 1";

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@token", effectiveToken);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                // Create and return the User object
                                return new User
                                {
                                    Username = reader["username"].ToString(),
                                    Password = reader["password"].ToString(),
                                    UserToken = reader["usertoken"].ToString(),
                                    Department = reader["department"] == DBNull.Value ? "N/A" : reader["department"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                Console.WriteLine($"Database error: {ex.Message}");
            }

            return null; // Return null if no data is found or in case of an error
        }


        public void SaveUserDataAsync(string token, bool rememberMe = false)
        {
            _currentToken = token; // Store the token temporarily
            if (rememberMe)
            {
                File.WriteAllText(_loginFilePath, token);
            }
            _isLoggedIn = true;
        }

        public void ClearUserData()
        {
            _currentToken = null; // Clear the temporary token
            if (File.Exists(_loginFilePath))
            {
                File.Delete(_loginFilePath);
            }
            _isLoggedIn = false;
        }
        public async Task<List<string>> GetMemberInfo(string membername)
        {
            string query = "SELECT * FROM MemberData WHERE name = @membername"; // Filter by membername

            var memberInfoList = new List<string>();

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@membername", membername); // Use parameterized queries to avoid SQL injection

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        // Read each row and add the necessary information to the list
                        while (await reader.ReadAsync())
                        {
                            // You can select which columns you want to retrieve
                            string memberData = $"Name: {reader["Name"]}, " +
                            $"Department: {reader["Department"]}, " +
                            $"Rank: {reader["Rank"]}, " +
                            $"BloodType: {reader["BloodType"]}, " +
                            $"DOB: {reader["DOB"]}, " +
                            $"EmergencyContact: {reader["EmergencyContact"]}, " +
                            $"Status: {reader["Status"]}";
                            memberInfoList.Add(memberData);
                        }
                    }
                }
            }

            return memberInfoList;
        }
        public async Task<MemberInfo> GetMemberInfoFromNum(string memberid)
        {
            string query = "SELECT MemberId, Name, Department, Rank, BloodType, DOB, EmergencyContact, Status FROM MemberData WHERE memberid = @memberid";

            MemberInfo member = null;

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@memberid", memberid);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            member = new MemberInfo
                            {
                                ID = reader["MemberId"].ToString(),
                                Name = reader["Name"].ToString(),
                                Department = reader["Department"].ToString(),
                                Rank = reader["Rank"].ToString(),
                                BloodType = reader["BloodType"].ToString(),
                                DOB = reader["DOB"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DOB"]),
                                EmergencyContact = reader["EmergencyContact"] == DBNull.Value ? string.Empty : reader["EmergencyContact"].ToString(),
                                Status = reader["Status"] == DBNull.Value ? string.Empty : reader["Status"].ToString()
                            };
                        }
                    }
                }
            }

            return member;
        }


        public async Task<List<string>> GetMemberNamesAsync()
        {
            List<string> memberNames = new List<string>();
            string query = "SELECT Name FROM MemberData"; // Query to get all member names

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync(); // Open the connection

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    // Execute the query and read the results
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            // Add each name to the list
                            memberNames.Add(reader["Name"].ToString());
                        }
                    }
                }
            }

            return memberNames; // Return the list of member names
        }

        public async Task<List<MemberInfo>> GetAllMemberInfo()
        {
            var memberInfoList = new List<MemberInfo>();
            string query = "SELECT * FROM MemberData";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var member = new MemberInfo
                            {
                                ID = reader["MemberId"].ToString(),
                                Name = reader["Name"].ToString(),
                                Department = reader["Department"].ToString(),
                                Rank = reader["Rank"].ToString(),
                                BloodType = reader["BloodType"].ToString(),
                                DOB = reader["DOB"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["DOB"]),
                                EmergencyContact = reader["EmergencyContact"].ToString(),
                                Status = reader["Status"].ToString()
                            };
                            memberInfoList.Add(member);
                        }
                    }
                }
            }
            return memberInfoList;
        }

        public async Task AddMemberAsync(string num, string name, string department, string rank, string bloodType, DateTime dob, string emergencyContact, string status)
        {
            
            string query = @"
        INSERT INTO MemberData (MemberID, Name, Department, Rank, BloodType, DOB, EmergencyContact, Status)
        VALUES (@memberid, @name, @department, @rank, @bloodType, @dob, @emergencyContact, @status)";

            using (var connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var cmd = new NpgsqlCommand(query, connection))
                {
                    // Add parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@memberid", num);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@department", department);
                    cmd.Parameters.AddWithValue("@rank", rank);
                    cmd.Parameters.AddWithValue("@bloodType", bloodType);
                    cmd.Parameters.AddWithValue("@dob", dob);
                    cmd.Parameters.AddWithValue("@emergencyContact", emergencyContact);
                    cmd.Parameters.AddWithValue("@status", status);

                    // Execute the query
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateMemberAsync(string oldNum, string num, string name, string department, string rank, string bloodType, DateTime dob, string emergencyContact, string status)
        {
            // Debug log to check input
            Console.WriteLine($"Updating MemberID: {num}, Name: {name}, Department: {department}, Rank: {rank}, BloodType: {bloodType}, DOB: {dob}, EmergencyContact: {emergencyContact}, Status: {status}");

            string query = @"
            UPDATE MemberData
            SET
                MemberID = @newMemberid,
                Name = @name,
                Department = @department,
                Rank = @rank,
                BloodType = @bloodType,
                DOB = @dob,
                EmergencyContact = @emergencyContact,
                Status = @status
            WHERE MemberID = @oldMemberid";  // Update the record where the MemberID matches the provided 'num'

            try
            {
                using (var connection = new NpgsqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var cmd = new NpgsqlCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        cmd.Parameters.AddWithValue("@oldMemberid", oldNum); // MemberID is a Text type
                        cmd.Parameters.AddWithValue("@newMemberid", num);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@department", department);
                        cmd.Parameters.AddWithValue("@rank", rank);
                        cmd.Parameters.AddWithValue("@bloodType", bloodType);
                        cmd.Parameters.AddWithValue("@dob", dob); // Ensure DOB is a valid DateTime
                        cmd.Parameters.AddWithValue("@emergencyContact", emergencyContact);
                        cmd.Parameters.AddWithValue("@status", status);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public async Task RemoveMemberData(string memberID)
        {
            string query = @"DELETE FROM MemberData WHERE MemberID = @memberid";

            try
            {
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        // Add parameter for the member ID to prevent SQL injection
                        cmd.Parameters.AddWithValue("@memberid", memberID);

                        // Execute the query to delete the member from the database
                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        // Check if any rows were affected
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"Member with ID {memberID} has been deleted.");
                        }
                        else
                        {
                            Console.WriteLine($"No member found with ID {memberID}.");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Catch any exceptions and print the error message
                Console.WriteLine($"Error: {e.Message}");
            }
        }

    }
}
