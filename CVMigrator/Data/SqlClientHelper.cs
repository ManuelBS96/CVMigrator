
using Microsoft.Data.SqlClient;
using System.Data;

namespace CVMigrator.Data
{
    public class SqlClientHelper
    {
        private readonly String _connectionString;

        public SqlClientHelper(String connectionString)
        {
            _connectionString = connectionString;
        }

        #region Connection

        public async Task<SqlConnection> CreateConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        #endregion

        #region Execute Non Query

        public async Task<int> ExecuteNonQueryAsync(
            string sql,
            IEnumerable<SqlParameter>? parameters = null,
            SqlTransaction? transaction = null)
        {
            SqlConnection connection;
            bool disposeConnection = false;

            if (transaction != null)
            {
                connection = transaction.Connection!;
            }
            else
            {
                connection = await CreateConnectionAsync();
                disposeConnection = true;
            }

            try
            {
                using var command = new SqlCommand(sql, connection);

                if (transaction != null)
                {
                    command.Transaction = transaction;
                }

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }

                return await command.ExecuteNonQueryAsync();
            }
            finally
            {
                if (disposeConnection)
                {
                    await connection.DisposeAsync();
                }
            }
        }

        #endregion

        #region Execute Scalar

        public async Task<object?> ExecuteScalarAsync(
            string sql,
            IEnumerable<SqlParameter>? parameters = null)
        {
            await using var connection = await CreateConnectionAsync();

            using var command = new SqlCommand(sql, connection);

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters.ToArray());
            }

            return await command.ExecuteScalarAsync();
        }

        #endregion

        #region Execute Query

        public async Task<DataTable> ExecuteQueryAsync(
            string sql,
            IEnumerable<SqlParameter>? parameters = null)
        {
            await using var connection = await CreateConnectionAsync();

            using var command = new SqlCommand(sql, connection);

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters.ToArray());
            }

            using var reader = await command.ExecuteReaderAsync();

            var table = new DataTable();

            table.Load(reader);

            return table;
        }

        #endregion

        #region Transactions

        public async Task<SqlTransaction> BeginTransactionAsync(
            SqlConnection connection)
        {
            return (SqlTransaction)await connection.BeginTransactionAsync();
        }

        #endregion
    }
}

