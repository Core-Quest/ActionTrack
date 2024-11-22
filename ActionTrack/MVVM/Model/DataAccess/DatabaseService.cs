using Npgsql;
using System;
using System.Data;
using System.Threading.Tasks;

namespace ActionTrack.MVVM.Model.DataAccess
{
    internal class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Method to open a connection
        private NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        // Example method to execute a non-query (e.g., INSERT, UPDATE, DELETE)
        public async Task<int> ExecuteNonQueryAsync(string query, NpgsqlParameter[] parameters = null)
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                await conn.OpenAsync();
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    return await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        // Example method to execute a query and return a DataTable (e.g., SELECT)
        public async Task<DataTable> ExecuteQueryAsync(string query, NpgsqlParameter[] parameters = null)
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                await conn.OpenAsync();
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    using (NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        // Example method to retrieve a single value (e.g., COUNT, SUM)
        public async Task<object> ExecuteScalarAsync(string query, NpgsqlParameter[] parameters = null)
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                await conn.OpenAsync();
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    return await cmd.ExecuteScalarAsync();
                }
            }
        }
    }
}
