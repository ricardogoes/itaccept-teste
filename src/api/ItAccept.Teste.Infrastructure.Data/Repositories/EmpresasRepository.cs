using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.AppSettings;
using ItAccept.Teste.Domain.Interfaces.Repositories;
using MySqlConnector;
using System.Data;

namespace ItAccept.Teste.Infrastructure.Data.Repositories
{
    public class EmpresasRepository : IEmpresasRepository
    {
        private readonly IDapperWrapper _dapperWrapper;
        
        public EmpresasRepository(IDapperWrapper dapperWrapper)
        {
            _dapperWrapper = dapperWrapper ?? throw new ArgumentNullException(nameof(dapperWrapper));
        }

        public async Task<int> AtualizarAsync(Empresa empresa)
        {
            if (empresa is null)
                throw new ArgumentNullException(nameof(empresa));

            var sqlCommand = $@"UPDATE empresas
			                        SET nome_empresa = '{MySqlHelper.EscapeString(empresa.NomeEmpresa)}',
                                        tipo_empresa = '{empresa.TipoEmpresa}'
		                        WHERE empresa_id = {empresa.EmpresaId};";

            await _dapperWrapper.ExecuteAsync(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return empresa.EmpresaId;
        }

        public async Task<IEnumerable<Empresa>> ConsultarEmbarcadorasAsync()
        {
            var sqlCommand = @$"SELECT 	empresa_id AS EmpresaId,
                                        nome_empresa AS NomeEmpresa,
                                        status AS Status,
                                        tipo_empresa AS TipoEmpresa
                                FROM empresas
                                WHERE tipo_empresa = 'Embarcadora'; ";

            var empresas = await _dapperWrapper.QueryAsync<Empresa>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return empresas;
        }

        public async Task<IEnumerable<Empresa>> ConsultarEmbarcadorasAtivasAsync()
        {
            var sqlCommand = @$"SELECT 	empresa_id AS EmpresaId,
                                        nome_empresa AS NomeEmpresa,
                                        status AS Status,
                                        tipo_empresa AS TipoEmpresa
                                FROM empresas
                                WHERE tipo_empresa = 'Embarcadora'
                                  AND status = 1; ";

            var empresas = await _dapperWrapper.QueryAsync<Empresa>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return empresas;
        }

        public async Task<Empresa> ConsultarPeloIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Inválido", nameof(id));

            var sqlCommand = @$"SELECT 	empresa_id AS EmpresaId,
                                        nome_empresa AS NomeEmpresa,
                                        status AS Status,
                                        tipo_empresa AS TipoEmpresa
                                FROM empresas
                                WHERE empresa_id = {id};";

            var empresa = await _dapperWrapper.QueryFirstOrDefaultAsync<Empresa>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return empresa;
        }

        public async Task<IEnumerable<Empresa>> ConsultarTransportadorasAsync()
        {
            var sqlCommand = @$"SELECT 	empresa_id AS EmpresaId,
                                        nome_empresa AS NomeEmpresa,
                                        status AS Status,
                                        tipo_empresa AS TipoEmpresa
                                FROM empresas
                                WHERE tipo_empresa = 'Transportadora'; ";

            var empresas = await _dapperWrapper.QueryAsync<Empresa>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return empresas;
        }

        public async Task<IEnumerable<Empresa>> ConsultarTransportadorasAtivasAsync()
        {
            var sqlCommand = @$"SELECT 	empresa_id AS EmpresaId,
                                        nome_empresa AS NomeEmpresa,
                                        status AS Status,
                                        tipo_empresa AS TipoEmpresa
                                FROM empresas
                                WHERE tipo_empresa = 'Transportadora'
                                  AND status = 1; ";

            var empresas = await _dapperWrapper.QueryAsync<Empresa>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return empresas;
        }

        public async Task<int> InativarAsync(Empresa empresa)
        {
            if (empresa is null)
                throw new ArgumentNullException(nameof(empresa));

            var sqlCommand = $@"UPDATE empresas
			                        SET status = {empresa.Status}
		                        WHERE empresa_id = {empresa.EmpresaId};";

            await _dapperWrapper.ExecuteAsync(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return empresa.EmpresaId;
        }

        public async Task<int> InserirAsync(Empresa empresa)
        {
            if (empresa is null)
                throw new ArgumentNullException(nameof(empresa));

            var sqlCommand = $@"INSERT INTO empresas (nome_empresa, status, tipo_empresa)
			                        VALUES ('{MySqlHelper.EscapeString(empresa.NomeEmpresa)}', {empresa.Status}, '{empresa.TipoEmpresa}');
        
                                SELECT LAST_INSERT_ID();";

            var idInserido = await _dapperWrapper.QueryFirstOrDefaultAsync<int>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return idInserido;
        }
    }
}
