using ItAccept.Teste.Domain.Interfaces.AppSettings;
using ItAccept.Teste.Domain.Interfaces.Repositories;
using ItAccept.Teste.Domain.Models;
using MySqlConnector;
using System.Data;

namespace ItAccept.Teste.Infrastructure.Data.Repositories
{
    public class PasswordsRepository : IPasswordsRepository
    {
        private readonly IDapperWrapper _dapperWrapper;
        
        public PasswordsRepository(IDapperWrapper dapperWrapper)
        {
            _dapperWrapper = dapperWrapper ?? throw new ArgumentNullException(nameof(dapperWrapper));
        }

        public async Task<byte[]> ConsultarHashPasswordPorUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Inválido", nameof(username));

            var sqlCommand = $@"SELECT password_hash AS PasswordHash
		                        FROM usuarios 
		                        WHERE username = '{MySqlHelper.EscapeString(username)}';";

            var hash = await _dapperWrapper.ExecuteScalarAsync<byte[]>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return hash;
        }

        public async Task<PasswordInfo> ConsultarPasswordInfoPorUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException($"'{nameof(username)}' não pode ser nulo", nameof(username));

            var sqlCommand = $@"SELECT 	password AS Password, 
				                        password_hash AS PasswordHash
		                        FROM usuarios 
		                        WHERE username = '{MySqlHelper.EscapeString(username)}';";

            var passwordInfo = await _dapperWrapper.QueryFirstOrDefaultAsync<PasswordInfo>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return passwordInfo;
        }
    }
}
