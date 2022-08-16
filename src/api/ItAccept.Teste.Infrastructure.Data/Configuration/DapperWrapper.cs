using Dapper;
using ItAccept.Teste.Domain.Interfaces.AppSettings;
using System.Data;

namespace ItAccept.Teste.Infrastructure.Data.Configuration
{
    public class DapperWrapper : IDapperWrapper
    {
        private readonly IConnectionFactory _connectionFactory;

        public DapperWrapper(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public IDbConnection DbConnection
        {
            get
            {
                return _connectionFactory.CreateConnection();
            }
        }

        public Task ExecuteAsync(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return DbConnection.ExecuteAsync(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<T> ExecuteScalarAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return DbConnection.ExecuteScalarAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return DbConnection.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }

        public Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return DbConnection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
        }
    }
}
