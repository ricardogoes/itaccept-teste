using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.AppSettings;
using ItAccept.Teste.Domain.Interfaces.Repositories;
using ItAccept.Teste.Domain.ViewModels.Ofertas;
using MySqlConnector;
using System.Data;

namespace ItAccept.Teste.Infrastructure.Data.Repositories
{
    public class OfertasRepository : IOfertasRepository
    {
        private readonly IDapperWrapper _dapperWrapper;
        public OfertasRepository(IDapperWrapper dapperWrapper)
        {
            _dapperWrapper = dapperWrapper ?? throw new ArgumentNullException(nameof(dapperWrapper));
        }

        public async Task<int> AtualizarAsync(Oferta oferta)
        {
            if (oferta is null)
                throw new ArgumentNullException(nameof(oferta));

            var sqlCommand = $@"UPDATE ofertas
			                        SET embarcadora_id = {oferta.EmbarcadoraId},
                                        produto_id = {oferta.ProdutoId},
                                        quantidade = {oferta.Quantidade},
                                        endereco_origem = '{MySqlHelper.EscapeString(oferta.EnderecoOrigem)}',
                                        endereco_destino = '{MySqlHelper.EscapeString(oferta.EnderecoDestino)}'
		                        WHERE oferta_id = {oferta.OfertaId};";

            await _dapperWrapper.ExecuteAsync(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return oferta.OfertaId;
        }

        public async Task<IEnumerable<OfertaParaConsultarVM>> ConsultarPelaEmbarcadoraAsync(int embarcadoraId)
        {
            if (embarcadoraId <= 0)
                throw new ArgumentException("Inválido", nameof(embarcadoraId));

            var sqlCommand = $@"SELECT 	O.oferta_id AS OfertaId,
		                                O.embarcadora_id AS EmbarcadoraId,
		                                E.nome_empresa AS NomeEmbarcadora,
		                                O.produto_id AS ProdutoId,
		                                P.nome_produto AS NomeProduto,
		                                O.quantidade AS Quantidade,
                                        (O.quantidade - SUM(IFNULL(L.volume, 0))) AS QuantidadeDisponivel,
                                        CASE 
			                                WHEN (O.quantidade - SUM(IFNULL(L.volume, 0))) = 0 
                                            THEN 'Finalizada'
			                                ELSe 'Em Aberto'
		                                END AS StatusOferta,
		                                O.endereco_origem AS EnderecoOrigem,
		                                O.endereco_destino AS EnderecoDestino,                        
		                                O.status AS Status
                                FROM ofertas O
                                INNER JOIN empresas E
	                                ON O.embarcadora_id = E.empresa_id
                                INNER JOIN produtos P
	                                ON O.produto_id = P.produto_id
                                LEFT JOIN lances L
	                                ON O.oferta_id = L.oferta_id
                                    AND L.lance_vencedor = 1
                                WHERE O.embarcadora_id = {embarcadoraId}
                                GROUP BY O.oferta_id,
		                                O.embarcadora_id,
		                                E.nome_empresa,
		                                O.produto_id,
		                                P.nome_produto,
		                                O.quantidade,
                                        O.endereco_origem,
		                                O.endereco_destino,
		                                O.status;";

            var ofertas = await _dapperWrapper.QueryAsync<OfertaParaConsultarVM>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return ofertas;
        }

        public async Task<IEnumerable<OfertaParaConsultarVM>> ConsultarAtivosPelaEmbarcadoraAsync(int embarcadoraId)
        {
            if (embarcadoraId <= 0)
                throw new ArgumentException("Inválido", nameof(embarcadoraId));

            var sqlCommand = $@"SELECT 	O.oferta_id AS OfertaId,
		                                O.embarcadora_id AS EmbarcadoraId,
		                                E.nome_empresa AS NomeEmbarcadora,
		                                O.produto_id AS ProdutoId,
		                                P.nome_produto AS NomeProduto,
		                                O.quantidade AS Quantidade,
                                        (O.quantidade - SUM(IFNULL(L.volume, 0))) AS QuantidadeDisponivel,
                                        CASE 
			                                WHEN (O.quantidade - SUM(IFNULL(L.volume, 0))) = 0 
                                            THEN 'Finalizada'
			                                ELSe 'Em Aberto'
		                                END AS StatusOferta,
		                                O.endereco_origem AS EnderecoOrigem,
		                                O.endereco_destino AS EnderecoDestino,                        
		                                O.status AS Status
                                FROM ofertas O
                                INNER JOIN empresas E
	                                ON O.embarcadora_id = E.empresa_id
                                INNER JOIN produtos P
	                                ON O.produto_id = P.produto_id
                                LEFT JOIN lances L
	                                ON O.oferta_id = L.oferta_id
                                    AND L.lance_vencedor = 1
                                WHERE O.embarcadora_id = {embarcadoraId}
                                  AND O.status = 1
                                GROUP BY O.oferta_id,
		                                O.embarcadora_id,
		                                E.nome_empresa,
		                                O.produto_id,
		                                P.nome_produto,
		                                O.quantidade,
                                        O.endereco_origem,
		                                O.endereco_destino,
		                                O.status;";

            var ofertas = await _dapperWrapper.QueryAsync<OfertaParaConsultarVM>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return ofertas;
        }

        public async Task<IEnumerable<OfertaParaConsultarVM>> ConsultarPeloProdutoAsync(int produtoId)
        {
            if (produtoId <= 0)
                throw new ArgumentException("Inválido", nameof(produtoId));

            var sqlCommand = $@"SELECT 	O.oferta_id AS OfertaId,
		                                O.embarcadora_id AS EmbarcadoraId,
		                                E.nome_empresa AS NomeEmbarcadora,
		                                O.produto_id AS ProdutoId,
		                                P.nome_produto AS NomeProduto,
		                                O.quantidade AS Quantidade,
                                        (O.quantidade - SUM(IFNULL(L.volume, 0))) AS QuantidadeDisponivel,
                                        CASE 
			                                WHEN (O.quantidade - SUM(IFNULL(L.volume, 0))) = 0 
                                            THEN 'Finalizada'
			                                ELSe 'Em Aberto'
		                                END AS StatusOferta,
		                                O.endereco_origem AS EnderecoOrigem,
		                                O.endereco_destino AS EnderecoDestino,                        
		                                O.status AS Status
                                FROM ofertas O
                                INNER JOIN empresas E
	                                ON O.embarcadora_id = E.empresa_id
                                INNER JOIN produtos P
	                                ON O.produto_id = P.produto_id
                                LEFT JOIN lances L
	                                ON O.oferta_id = L.oferta_id
                                    AND L.lance_vencedor = 1
                                WHERE O.produto_id = {produtoId}
                                GROUP BY O.oferta_id,
		                                O.embarcadora_id,
		                                E.nome_empresa,
		                                O.produto_id,
		                                P.nome_produto,
		                                O.quantidade,
                                        O.endereco_origem,
		                                O.endereco_destino,
		                                O.status;";

            var ofertas = await _dapperWrapper.QueryAsync<OfertaParaConsultarVM>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return ofertas;
        }

        public async Task<IEnumerable<OfertaParaConsultarVM>> ConsultarAtivosPeloProdutoAsync(int produtoId)
        {
            if (produtoId <= 0)
                throw new ArgumentException("Inválido", nameof(produtoId));

            var sqlCommand = $@"SELECT 	O.oferta_id AS OfertaId,
		                                O.embarcadora_id AS EmbarcadoraId,
		                                E.nome_empresa AS NomeEmbarcadora,
		                                O.produto_id AS ProdutoId,
		                                P.nome_produto AS NomeProduto,
		                                O.quantidade AS Quantidade,
                                        (O.quantidade - SUM(IFNULL(L.volume, 0))) AS QuantidadeDisponivel,
                                        CASE 
			                                WHEN (O.quantidade - SUM(IFNULL(L.volume, 0))) = 0 
                                            THEN 'Finalizada'
			                                ELSe 'Em Aberto'
		                                END AS StatusOferta,
		                                O.endereco_origem AS EnderecoOrigem,
		                                O.endereco_destino AS EnderecoDestino,                        
		                                O.status AS Status
                                FROM ofertas O
                                INNER JOIN empresas E
	                                ON O.embarcadora_id = E.empresa_id
                                INNER JOIN produtos P
	                                ON O.produto_id = P.produto_id
                                LEFT JOIN lances L
	                                ON O.oferta_id = L.oferta_id
                                    AND L.lance_vencedor = 1
                                WHERE O.produto_id = {produtoId}
                                  AND O.status = 1
                                GROUP BY O.oferta_id,
		                                O.embarcadora_id,
		                                E.nome_empresa,
		                                O.produto_id,
		                                P.nome_produto,
		                                O.quantidade,
                                        O.endereco_origem,
		                                O.endereco_destino,
		                                O.status;";

            var ofertas = await _dapperWrapper.QueryAsync<OfertaParaConsultarVM>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return ofertas;
        }

        public async Task<IEnumerable<OfertaParaConsultarVM>> ConsultarAtivosPelaTransportadoraAsync(int transportadoraId)
        {
            if (transportadoraId <= 0)
                throw new ArgumentException("Inválido", nameof(transportadoraId));

            var sqlCommand = $@"SELECT 	O.oferta_id AS OfertaId,
			                            O.embarcadora_id AS EmbarcadoraId,
			                            E.nome_empresa AS NomeEmbarcadora,
			                            O.produto_id AS ProdutoId,
			                            P.nome_produto AS NomeProduto,
			                            O.quantidade AS Quantidade,
                                        (O.quantidade - SUM(IFNULL(L.volume, 0))) AS QuantidadeDisponivel,
                                        CASE 
			                                WHEN (O.quantidade - SUM(IFNULL(L.volume, 0))) = 0 
                                            THEN 'Finalizada'
			                                ELSe 'Em Aberto'
		                                END AS StatusOferta,
			                            O.endereco_origem AS EnderecoOrigem,
			                            O.endereco_destino AS EnderecoDestino,                
			                            O.status AS Status
	                            FROM ofertas O
	                            INNER JOIN empresas E
		                            ON O.embarcadora_id = E.empresa_id
	                            INNER JOIN produtos P
		                            ON O.produto_id = P.produto_id
	                            INNER JOIN embarcadoras_transportadoras ET
		                            ON E.empresa_id = ET.embarcadora_id
                                LEFT JOIN lances L
	                                ON O.oferta_id = L.oferta_id
                                    AND L.lance_vencedor = 1
	                            WHERE ET.transportadora_id = 5
                                  AND O.status = 1
                                  AND (O.quantidade - SUM(IFNULL(L.volume, 0))) > 0
                                GROUP BY O.oferta_id,
		                                O.embarcadora_id,
		                                E.nome_empresa,
		                                O.produto_id,
		                                P.nome_produto,
		                                O.quantidade,
                                        O.endereco_origem,
		                                O.endereco_destino,
		                                O.status;";

            var ofertas = await _dapperWrapper.QueryAsync<OfertaParaConsultarVM>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return ofertas;
        }

        public async Task<OfertaParaConsultarVM> ConsultarPeloIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Inválido", nameof(id));

            var sqlCommand = $@"SELECT 	O.oferta_id AS OfertaId,
		                                O.embarcadora_id AS EmbarcadoraId,
		                                E.nome_empresa AS NomeEmbarcadora,
		                                O.produto_id AS ProdutoId,
		                                P.nome_produto AS NomeProduto,
		                                O.quantidade AS Quantidade,
                                        (O.quantidade - SUM(IFNULL(L.volume, 0))) AS QuantidadeDisponivel,
                                        CASE 
			                                WHEN (O.quantidade - SUM(IFNULL(L.volume, 0))) = 0 
                                            THEN 'Finalizada'
			                                ELSe 'Em Aberto'
		                                END AS StatusOferta,
		                                O.endereco_origem AS EnderecoOrigem,
		                                O.endereco_destino AS EnderecoDestino,                        
		                                O.status AS Status
                                FROM ofertas O
                                INNER JOIN empresas E
	                                ON O.embarcadora_id = E.empresa_id
                                INNER JOIN produtos P
	                                ON O.produto_id = P.produto_id
                                LEFT JOIN lances L
	                                ON O.oferta_id = L.oferta_id
                                    AND L.lance_vencedor = 1
                                WHERE O.oferta_id = {id};
                                GROUP BY O.oferta_id,
		                                O.embarcadora_id,
		                                E.nome_empresa,
		                                O.produto_id,
		                                P.nome_produto,
		                                O.quantidade,
                                        O.endereco_origem,
		                                O.endereco_destino,
		                                O.status;";

            var oferta = await _dapperWrapper.QueryFirstOrDefaultAsync<OfertaParaConsultarVM>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return oferta;
        }

        public async Task<int> InativarAsync(Oferta oferta)
        {
            if (oferta is null)
                throw new ArgumentNullException(nameof(oferta));

            var sqlCommand = $@"UPDATE ofertas
			                        SET status = {oferta.Status}
		                        WHERE oferta_id = {oferta.OfertaId};";

            await _dapperWrapper.ExecuteAsync(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return oferta.OfertaId;
        }

        public async Task<int> InserirAsync(Oferta oferta)
        {
            if (oferta is null)
                throw new ArgumentNullException(nameof(oferta));

            var sqlCommand = $@"INSERT INTO ofertas (embarcadora_id, produto_id, quantidade, endereco_origem, endereco_destino, status)
			                        VALUES ({oferta.EmbarcadoraId}, {oferta.ProdutoId}, {oferta.Quantidade}, '{MySqlHelper.EscapeString(oferta.EnderecoOrigem)}', '{MySqlHelper.EscapeString(oferta.EnderecoDestino)}', {oferta.Status});
        
                                SELECT LAST_INSERT_ID();";

            var idInserido = await _dapperWrapper.QueryFirstOrDefaultAsync<int>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return idInserido;
        }
    }
}
