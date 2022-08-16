using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.Repositories;
using ItAccept.Teste.Domain.Interfaces.Services;
using ItAccept.Teste.Domain.ViewModels.EmbarcadorasTransportadoras;

namespace ItAccept.Teste.Services
{
    public class EmbarcadorasTransportadorasService : IEmbarcadorasTransportadorasService
    {
        private readonly IEmbarcadorasTransportadorasRepository _repository;

        public EmbarcadorasTransportadorasService(IEmbarcadorasTransportadorasRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task ApagarAsync(EmbarcadoraTransportadora entity)
            => await _repository.ApagarAsync(entity);

        public async Task<int> AtualizarAsync(EmbarcadoraTransportadora entity)
            => await _repository.AtualizarAsync(entity);

        public async Task<IEnumerable<Empresa>> ConsultarAssociadosPelaEmbarcadoraAsync(int embarcadoraId)
            => await _repository.ConsultarAssociadosPelaEmbarcadoraAsync(embarcadoraId);

        public async Task<IEnumerable<Empresa>> ConsultarNaoAssociadosPelaEmbarcadoraAsync(int embarcadoraId)
            => await _repository.ConsultarNaoAssociadosPelaEmbarcadoraAsync(embarcadoraId);

        public async Task<IEnumerable<Empresa>> ConsultarAssociadosPelaTransportadoraAsync(int transportadoraId)
            => await _repository.ConsultarAssociadosPelaTransportadoraAsync(transportadoraId);

        public async Task<IEnumerable<Empresa>> ConsultarNaoAssociadosPelaTransportadoraAsync(int transportadoraId)
            => await _repository.ConsultarNaoAssociadosPelaTransportadoraAsync(transportadoraId);

        public async Task<EmbarcadoraTransportadoraParaConsultarVM> ConsultarPeloIdAsync(int id)
            => await _repository.ConsultarPeloIdAsync(id);

        public async Task<int> InserirAsync(EmbarcadoraTransportadora entity)
            => await _repository.InserirAsync(entity);
    }
}