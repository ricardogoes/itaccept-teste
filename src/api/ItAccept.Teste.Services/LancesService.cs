using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.Repositories;
using ItAccept.Teste.Domain.Interfaces.Services;
using ItAccept.Teste.Domain.ViewModels.Lances;

namespace ItAccept.Teste.Services
{
    public class LancesService : ILancesService
    {
        private readonly ILancesRepository _repository;

        public LancesService(ILancesRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<int> AtualizarAsync(Lance entity)
            => await _repository.AtualizarAsync(entity);

        public async Task<IEnumerable<LanceParaConsultarVM>> ConsultarPelaOfertaAsync(int ofertaId)
            => await _repository.ConsultarPelaOfertaAsync(ofertaId);

        public async Task<IEnumerable<LanceParaConsultarVM>> ConsultarAtivosPelaOfertaAsync(int ofertaId)
            => await _repository.ConsultarAtivosPelaOfertaAsync(ofertaId);

        public async Task<IEnumerable<LanceParaConsultarVM>> ConsultarPelaTransportadoraAsync(int transportadoraId)
            => await _repository.ConsultarPelaTransportadoraAsync(transportadoraId);

        public async Task<IEnumerable<LanceParaConsultarVM>> ConsultarAtivosPelaTransportadoraAsync(int transportadoraId)
            => await _repository.ConsultarAtivosPelaTransportadoraAsync(transportadoraId);

        public async Task<LanceParaConsultarVM> ConsultarPeloIdAsync(int id)
            => await _repository.ConsultarPeloIdAsync(id);

        public async Task<int> InativarAsync(Lance entity)
            => await _repository.InativarAsync(entity);

        public async Task<int> InserirAsync(Lance entity)
            => await _repository.InserirAsync(entity);
    }
}
