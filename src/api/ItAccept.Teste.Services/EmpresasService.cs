using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.Repositories;
using ItAccept.Teste.Domain.Interfaces.Services;

namespace ItAccept.Teste.Services
{
    public class EmpresasService : IEmpresasService
    {
        private readonly IEmpresasRepository _repository;

        public EmpresasService(IEmpresasRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<int> AtualizarAsync(Empresa entity)
            => await _repository.AtualizarAsync(entity);

        public async Task<IEnumerable<Empresa>> ConsultarEmbarcadorasAsync()
            => await _repository.ConsultarEmbarcadorasAsync();
        public async Task<IEnumerable<Empresa>> ConsultarEmbarcadorasAtivasAsync()
            => await _repository.ConsultarEmbarcadorasAtivasAsync();


        public async Task<Empresa> ConsultarPeloIdAsync(int id)
            => await _repository.ConsultarPeloIdAsync(id);

        public async Task<IEnumerable<Empresa>> ConsultarTransportadorasAsync()
            => await _repository.ConsultarTransportadorasAsync();
        public async Task<IEnumerable<Empresa>> ConsultarTransportadorasAtivasAsync()
            => await _repository.ConsultarTransportadorasAtivasAsync();


        public async Task<int> InativarAsync(Empresa entity)
            => await _repository.InativarAsync(entity);

        public async Task<int> InserirAsync(Empresa entity)
            => await _repository.InserirAsync(entity);
    }
}
