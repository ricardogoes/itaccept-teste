using MySqlConnector;
using ItAccept.Teste.Domain.Interfaces.AppSettings;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System.Data;

namespace ItAccept.Teste.Infrastructure.Data.Configuration
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly IConnectionSettings _connectionConfig;

        public ConnectionFactory(IConnectionSettings connectionConfig)
        {
            _connectionConfig = connectionConfig ?? throw new ArgumentNullException(nameof(connectionConfig));
        }

        public IDbConnection CreateConnection()
        {
            var connection = new MySqlConnection(_connectionConfig.DBConnection);
            return new ProfiledDbConnection(connection, MiniProfiler.Current);
        }
    }
}
