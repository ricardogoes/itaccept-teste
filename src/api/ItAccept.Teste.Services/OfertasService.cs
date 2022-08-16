using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.Repositories;
using ItAccept.Teste.Domain.Interfaces.Services;
using ItAccept.Teste.Domain.ViewModels.Ofertas;

namespace ItAccept.Teste.Services
{
    public class OfertasService : IOfertasService
    {
        private readonly IOfertasRepository _repository;

        public OfertasService(IOfertasRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<int> AtualizarAsync(Oferta entity)
            => await _repository.AtualizarAsync(entity);

        public async Task<IEnumerable<OfertaParaConsultarVM>> ConsultarPelaEmbarcadoraAsync(int embarcadoraId)
            => await _repository.ConsultarPelaEmbarcadoraAsync(embarcadoraId);

        public async Task<IEnumerable<OfertaParaConsultarVM>> ConsultarAtivosPelaEmbarcadoraAsync(int embarcadoraId)
            => await _repository.ConsultarAtivosPelaEmbarcadoraAsync(embarcadoraId);

        public async Task<IEnumerable<OfertaParaConsultarVM>> ConsultarPeloProdutoAsync(int produtoId)
           => await _repository.ConsultarPeloProdutoAsync(produtoId);

        public async Task<IEnumerable<OfertaParaConsultarVM>> ConsultarAtivosPeloProdutoAsync(int produtoId)
            => await _repository.ConsultarAtivosPeloProdutoAsync(produtoId);

        public async Task<IEnumerable<OfertaParaConsultarVM>> ConsultarAtivosPelaTransportadoraAsync(int produtoId)
           => await _repository.ConsultarAtivosPelaTransportadoraAsync(produtoId);

        public async Task<OfertaParaConsultarVM> ConsultarPeloIdAsync(int id)
            => await _repository.ConsultarPeloIdAsync(id);

        public async Task<int> InativarAsync(Oferta entity)
            => await _repository.InativarAsync(entity);

        public async Task<int> InserirAsync(Oferta entity)
            => await _repository.InserirAsync(entity);
    }
}
