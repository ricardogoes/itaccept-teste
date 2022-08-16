using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.AppSettings;
using ItAccept.Teste.Domain.Interfaces.Repositories;
using ItAccept.Teste.Domain.ViewModels.Lances;
using System.Data;

namespace ItAccept.Teste.Infrastructure.Data.Repositories
{
    public class LancesRepository : ILancesRepository
    {
        private readonly IDapperWrapper _dapperWrapper;
        
        public LancesRepository(IDapperWrapper dapperWrapper)
        {
            _dapperWrapper = dapperWrapper ?? throw new ArgumentNullException(nameof(dapperWrapper));
        }

        public async Task<int> AtualizarAsync(Lance lance)
        {
            if (lance is null)
                throw new ArgumentNullException(nameof(lance));

            var sqlCommand = $@"UPDATE lances
			                        SET oferta_id = {lance.OfertaId},
                                        transportadora_id = {lance.TransportadoraId},
                                        volume = {lance.Volume},
                                        preco = {lance.Preco},
                                        lance_vencedor = {lance.LanceVencedor}
		                        WHERE lance_id = {lance.LanceId};"; 

            await _dapperWrapper.ExecuteAsync(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return lance.LanceId;
        }

        public async Task<IEnumerable<LanceParaConsultarVM>> ConsultarPelaOfertaAsync(int ofertaId)
        {
            if (ofertaId <= 0)
                throw new ArgumentException("Inválido", nameof(ofertaId));

            var sqlCommand = $@"SELECT 	L.lance_id AS LanceId,
				                        L.oferta_id AS OfertaId,
                                        L.transportadora_id AS TransportadoraId,
                                        E.nome_empresa AS NomeTransportadora,
                                        L.volume AS Volume,
                                        L.preco AS Preco,
                                        L.lance_vencedor AS LanceVencedor,
                                        L.status AS Status
		                        FROM lances L
                                INNER JOIN empresas E
			                        ON L.transportadora_id = E.empresa_id
                                WHERE L.oferta_id = {ofertaId};";

            var lances = await _dapperWrapper.QueryAsync<LanceParaConsultarVM>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return lances;
        }

        public async Task<IEnumerable<LanceParaConsultarVM>> ConsultarAtivosPelaOfertaAsync(int ofertaId)
        {
            if (ofertaId <= 0)
                throw new ArgumentException("Inválido", nameof(ofertaId));

            var sqlCommand = $@"SELECT 	L.lance_id AS LanceId,
				                        L.oferta_id AS OfertaId,
                                        L.transportadora_id AS TransportadoraId,
                                        E.nome_empresa AS NomeTransportadora,
                                        L.volume AS Volume,
                                        L.preco AS Preco,
                                        L.lance_vencedor AS LanceVencedor,
                                        L.status AS Status
		                        FROM lances L
                                INNER JOIN empresas E
			                        ON L.transportadora_id = E.empresa_id
                                WHERE L.oferta_id = {ofertaId}
                                  AND L.status = 1;";

            var lances = await _dapperWrapper.QueryAsync<LanceParaConsultarVM>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return lances;
        }

        public async Task<IEnumerable<LanceParaConsultarVM>> ConsultarPelaTransportadoraAsync(int transportadoraId)
        {
            if (transportadoraId <= 0)
                throw new ArgumentException("Inválido", nameof(transportadoraId));

            var sqlCommand = $@"SELECT 	L.lance_id AS LanceId,
				                        L.oferta_id AS OfertaId,
                                        L.transportadora_id AS TransportadoraId,
                                        E.nome_empresa AS NomeTransportadora,
                                        L.volume AS Volume,
                                        L.preco AS Preco,
                                        L.lance_vencedor AS LanceVencedor,
                                        L.status AS Status
		                        FROM lances L
                                INNER JOIN empresas E
			                        ON L.transportadora_id = E.empresa_id
                                WHERE L.transportadora_id = {transportadoraId};";

            var lances = await _dapperWrapper.QueryAsync<LanceParaConsultarVM>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return lances;
        }

        public async Task<IEnumerable<LanceParaConsultarVM>> ConsultarAtivosPelaTransportadoraAsync(int transportadoraId)
        {
            if (transportadoraId <= 0)
                throw new ArgumentException("Inválido", nameof(transportadoraId));

            var sqlCommand = $@"SELECT 	L.lance_id AS LanceId,
				                        L.oferta_id AS OfertaId,
                                        L.transportadora_id AS TransportadoraId,
                                        E.nome_empresa AS NomeTransportadora,
                                        L.volume AS Volume,
                                        L.preco AS Preco,
                                        L.lance_vencedor AS LanceVencedor,
                                        L.status AS Status
		                        FROM lances L
                                INNER JOIN empresas E
			                        ON L.transportadora_id = E.empresa_id
                                WHERE L.transportadora_id = {transportadoraId}
                                  AND L.status = 1;";

            var lances = await _dapperWrapper.QueryAsync<LanceParaConsultarVM>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return lances;
        }

        public async Task<LanceParaConsultarVM> ConsultarPeloIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Inválido", nameof(id));

            var sqlCommand = $@"SELECT 	L.lance_id AS LanceId,
				                        L.oferta_id AS OfertaId,
                                        L.transportadora_id AS TransportadoraId,
                                        E.nome_empresa AS NomeTransportadora,
                                        L.volume AS Volume,
                                        L.preco AS Preco,
                                        L.lance_vencedor AS LanceVencedor,
                                        L.status AS Status
		                        FROM lances L
                                INNER JOIN empresas E
			                        ON L.transportadora_id = E.empresa_id
                                WHERE L.lance_id = {id};";

            var lance = await _dapperWrapper.QueryFirstOrDefaultAsync<LanceParaConsultarVM>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return lance;
        }

        public async Task<int> InativarAsync(Lance lance)
        {
            if (lance is null)
                throw new ArgumentNullException(nameof(lance));

            var sqlCommand = $@"UPDATE lances
			                        SET status = {lance.Status},
		                        WHERE lanceId = {lance.LanceId};";

            await _dapperWrapper.ExecuteAsync(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return lance.LanceId;
        }

        public async Task<int> InserirAsync(Lance lance)
        {
            if (lance is null)
                throw new ArgumentNullException(nameof(lance));

            var sqlCommand = $@"INSERT INTO lances (oferta_id, transportadora_id, volume, preco, lance_vencedor, status)
			                        VALUES ({lance.OfertaId}, {lance.TransportadoraId}, {lance.Volume}, {lance.Preco}, {lance.LanceVencedor}, {lance.Status});
        
                                SELECT LAST_INSERT_ID();";

            var idInserido = await _dapperWrapper.QueryFirstOrDefaultAsync<int>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return idInserido;
        }
    }
}
