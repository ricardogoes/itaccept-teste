using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.AppSettings;
using ItAccept.Teste.Domain.Interfaces.Repositories;
using ItAccept.Teste.Domain.ViewModels.EmbarcadorasTransportadoras;
using System.Data;

namespace ItAccept.Teste.Infrastructure.Data.Repositories
{
    public class EmbarcadorasTransportadorasRepository : IEmbarcadorasTransportadorasRepository
    {
        private readonly IDapperWrapper _dapperWrapper;
        
        public EmbarcadorasTransportadorasRepository(IDapperWrapper dapperWrapper)
        {
            _dapperWrapper = dapperWrapper ?? throw new ArgumentNullException(nameof(dapperWrapper));
        }

        public async Task ApagarAsync(EmbarcadoraTransportadora entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            var sqlCommand = @$"DELETE FROM embarcadoras_transportadoras WHERE embarcadora_transportadora_id = {entity.EmbarcadoraTransportadoraId};";

            await _dapperWrapper.ExecuteAsync(
                sql: sqlCommand,
                commandType: CommandType.Text);
        }

        public async Task<int> AtualizarAsync(EmbarcadoraTransportadora empresa)
        {
            if (empresa is null)
                throw new ArgumentNullException(nameof(empresa));

            var sqlCommand = @$"UPDATE embarcadoras_transprotadoras
			                        SET embarcadora_id = {empresa.EmbarcadoraId},
				                    transportadora_id = {empresa.TransportadoraId}
		                        WHERE embarcadora_transportadora_id = {empresa.EmbarcadoraTransportadoraId};";

            await _dapperWrapper.ExecuteAsync(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return empresa.EmbarcadoraTransportadoraId;
        }

        public async Task<IEnumerable<Empresa>> ConsultarAssociadosPelaEmbarcadoraAsync(int embarcadoraId)
        {
            if (embarcadoraId <= 0)
                throw new ArgumentException("Inválido", nameof(embarcadoraId));

            var sqlCommand = @$"SELECT 	empresa_id AS EmpresaId,
                                        nome_empresa AS NomeEmpresa,
                                        status AS Status,
                                        tipo_empresa AS TipoEmpresa
                                FROM empresas
                                WHERE tipo_empresa = 'Transportadora'
                                  AND empresa_id IN(SELECT transportadora_id FROM embarcadoras_transportadoras WHERE embarcadora_id = {embarcadoraId});";

            var empresas = await _dapperWrapper.QueryAsync<Empresa>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return empresas;
        }

        public async Task<IEnumerable<Empresa>> ConsultarNaoAssociadosPelaEmbarcadoraAsync(int embarcadoraId)
        {
            if (embarcadoraId <= 0)
                throw new ArgumentException("Inválido", nameof(embarcadoraId));

            var sqlCommand = @$"SELECT 	empresa_id AS EmpresaId,
                                        nome_empresa AS NomeEmpresa,
                                        status AS Status,
                                        tipo_empresa AS TipoEmpresa
                                FROM empresas
                                WHERE tipo_empresa = 'Transportadora'
                                  AND empresa_id NOT IN(SELECT transportadora_id FROM embarcadoras_transportadoras WHERE embarcadora_id = {embarcadoraId});";

            var empresas = await _dapperWrapper.QueryAsync<Empresa>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return empresas;
        }

        public async Task<IEnumerable<Empresa>> ConsultarAssociadosPelaTransportadoraAsync(int transportadoraId)
        {
            if (transportadoraId <= 0)
                throw new ArgumentException("Inválido", nameof(transportadoraId));

            var sqlCommand = @$"SELECT 	empresa_id AS EmpresaId,
                                        nome_empresa AS NomeEmpresa,
                                        status AS Status,
                                        tipo_empresa AS TipoEmpresa
                                FROM empresas
                                WHERE tipo_empresa = 'Embarcadora'
                                  AND empresa_id IN(SELECT embarcadora_id FROM embarcadoras_transportadoras WHERE transportadora_id = {transportadoraId});";

            var empresas = await _dapperWrapper.QueryAsync<Empresa>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return empresas;
        }

        public async Task<IEnumerable<Empresa>> ConsultarNaoAssociadosPelaTransportadoraAsync(int transportadoraId)
        {
            if (transportadoraId <= 0)
                throw new ArgumentException("Inválido", nameof(transportadoraId));

            var sqlCommand = @$"SELECT 	empresa_id AS EmpresaId,
                                        nome_empresa AS NomeEmpresa,
                                        status AS Status,
                                        tipo_empresa AS TipoEmpresa
                                FROM empresas
                                WHERE tipo_empresa = 'Embarcadora'
                                  AND empresa_id NOT IN(SELECT embarcadora_id FROM embarcadoras_transportadoras WHERE transportadora_id = {transportadoraId});";

            var empresas = await _dapperWrapper.QueryAsync<Empresa>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return empresas;
        }

        public async Task<EmbarcadoraTransportadoraParaConsultarVM> ConsultarPeloIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Inválido", nameof(id));

            var sqlCommand = @$"SELECT 	ET.embarcadora_transportadora_id AS EmbarcadoraTransportadoraId,
				                        ET.embarcadora_id AS EmbarcadoraId,
				                        E.nome_empresa AS NomeEmbarcadora,
                                        ET.transportadora_id AS TransportadoraId,
                                        T.nome_empresa AS NomeTransportadora
		                        FROM embarcadoras_transportadoras ET
                                INNER JOIN empresas E
			                        ON ET.embarcadora_id = E.empresa_id
		                        INNER JOIN empresas T
			                        ON ET.transportadora_id = T.empresa_id
		                        WHERE ET.embarcadora_transportadora_id = {id}";

            var empresa = await _dapperWrapper.QueryFirstOrDefaultAsync<EmbarcadoraTransportadoraParaConsultarVM>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return empresa;
        }

        public async Task<int> InserirAsync(EmbarcadoraTransportadora empresa)
        {
            if (empresa is null)
                throw new ArgumentNullException(nameof(empresa));

            var sqlCommand = @$"INSERT INTO embarcadoras_transportadoras (embarcadora_id, transportadora_id)
			                        VALUES ({empresa.EmbarcadoraId}, {empresa.TransportadoraId});
        
                               SELECT LAST_INSERT_ID();";

            var idInserido = await _dapperWrapper.QueryFirstOrDefaultAsync<int>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return idInserido;
        }
    }


}
