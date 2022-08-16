using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.Repositories;
using ItAccept.Teste.Domain.Interfaces.Services;
using ItAccept.Teste.Domain.ViewModels.Produtos;

namespace ItAccept.Teste.Services
{
    public class ProdutosService : IProdutosService
    {
        private readonly IProdutosRepository _repository;

        public ProdutosService(IProdutosRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));    
        }

        public async Task<int> AtualizarAsync(Produto entity)
            => await _repository.AtualizarAsync(entity);

        public async Task<IEnumerable<ProdutoParaConsultarVM>> ConsultarPelaEmbarcadoraAsync(int embarcadoraId)
            => await _repository.ConsultarPelaEmbarcadoraAsync(embarcadoraId);

        public async Task<IEnumerable<ProdutoParaConsultarVM>> ConsultarAtivosPelaEmbarcadoraAsync(int embarcadoraId)
            => await _repository.ConsultarAtivosPelaEmbarcadoraAsync(embarcadoraId);

        public async Task<ProdutoParaConsultarVM> ConsultarPeloIdAsync(int id)
            => await _repository.ConsultarPeloIdAsync(id);

        public async Task<int> InativarAsync(Produto entity)
            => await _repository.InativarAsync(entity);

        public async Task<int> InserirAsync(Produto entity)
            => await _repository.InserirAsync(entity);
    }
}
