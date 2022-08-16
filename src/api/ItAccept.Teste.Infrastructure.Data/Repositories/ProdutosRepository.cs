using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.AppSettings;
using ItAccept.Teste.Domain.Interfaces.Repositories;
using ItAccept.Teste.Domain.ViewModels.Produtos;
using MySqlConnector;
using System.Data;

namespace ItAccept.Teste.Infrastructure.Data.Repositories
{
    public class ProdutosRepository : IProdutosRepository
    {
        private readonly IDapperWrapper _dapperWrapper;
        
        public ProdutosRepository(IDapperWrapper dapperWrapper)
        {
            _dapperWrapper = dapperWrapper ?? throw new ArgumentNullException(nameof(dapperWrapper));
        }

        public async Task<int> AtualizarAsync(Produto produto)
        {
            if (produto is null)
                throw new ArgumentNullException(nameof(produto));

            var sqlCommand = $@"UPDATE produtos
			                        SET nome_produto = '{MySqlHelper.EscapeString(produto.NomeProduto)}',
                                        embarcadora_id = {produto.EmbarcadoraId}
		                        WHERE produto_id = {produto.ProdutoId};";

            await _dapperWrapper.ExecuteAsync(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return produto.ProdutoId;
        }

        public async Task<IEnumerable<ProdutoParaConsultarVM>> ConsultarPelaEmbarcadoraAsync(int embarcadoraId)
        {
            if (embarcadoraId <= 0)
                throw new ArgumentException("Inválido", nameof(embarcadoraId));

            var sqlCommand = $@"SELECT 	produto_id AS ProdutoId,
				                        nome_produto AS NomeProduto,
                                        embarcadora_id AS EmbarcadoraId,
                                        status AS Status
		                        FROM produtos 
                                WHERE embarcadora_id = {embarcadoraId};";

            var produtos = await _dapperWrapper.QueryAsync<ProdutoParaConsultarVM>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return produtos;
        }

        public async Task<IEnumerable<ProdutoParaConsultarVM>> ConsultarAtivosPelaEmbarcadoraAsync(int embarcadoraId)
        {
            if (embarcadoraId <= 0)
                throw new ArgumentException("Inválido", nameof(embarcadoraId));

            var sqlCommand = $@"SELECT 	produto_id AS ProdutoId,
				                        nome_produto AS NomeProduto,
                                        embarcadora_id AS EmbarcadoraId,
                                        status AS Status
		                        FROM produtos 
                                WHERE embarcadora_id = {embarcadoraId}
                                  AND status = 1;";

            var produtos = await _dapperWrapper.QueryAsync<ProdutoParaConsultarVM>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return produtos;
        }

        public async Task<ProdutoParaConsultarVM> ConsultarPeloIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Inválido", nameof(id));

            var sqlCommand = $@"SELECT 	produto_id AS ProdutoId,
				                        nome_produto AS NomeProduto,
                                        embarcadora_id AS EmbarcadoraId,
                                        status AS Status
		                        FROM produtos 
                                WHERE produto_id = {id};";

            var produto = await _dapperWrapper.QueryFirstOrDefaultAsync<ProdutoParaConsultarVM>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return produto;
        }

        public async Task<int> InativarAsync(Produto produto)
        {
            if (produto is null)
                throw new ArgumentNullException(nameof(produto));

            var sqlCommand = $@"UPDATE produtos
			                        SET status = {produto.Status}
		                        WHERE produto_id = {produto.ProdutoId};";

            await _dapperWrapper.ExecuteAsync(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return produto.ProdutoId;
        }

        public async Task<int> InserirAsync(Produto produto)
        {
            if (produto is null)
                throw new ArgumentNullException(nameof(produto));

            var sqlCommand = $@"INSERT INTO produtos (nome_produto, embarcadora_id, status)
			                        VALUES ('{MySqlHelper.EscapeString(produto.NomeProduto)}', {produto.EmbarcadoraId}, {produto.Status});
        
                                SELECT LAST_INSERT_ID();";

            var idInserido = await _dapperWrapper.QueryFirstOrDefaultAsync<int>(
                sql: sqlCommand,
                commandType: CommandType.Text);

            return idInserido;
        }
    }
}
