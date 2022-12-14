using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.Generic;

namespace ItAccept.Teste.Domain.Interfaces.Repositories
{
    public interface IEmpresasRepository : ICrud<Empresa, Empresa>, IInativar<Empresa>
    {
        Task<IEnumerable<Empresa>> ConsultarEmbarcadorasAsync();
        Task<IEnumerable<Empresa>> ConsultarEmbarcadorasAtivasAsync();
        Task<IEnumerable<Empresa>> ConsultarTransportadorasAsync();
        Task<IEnumerable<Empresa>> ConsultarTransportadorasAtivasAsync();
    }
}
