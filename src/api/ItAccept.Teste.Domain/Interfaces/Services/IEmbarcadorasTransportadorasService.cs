using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.Generic;
using ItAccept.Teste.Domain.ViewModels.EmbarcadorasTransportadoras;

namespace ItAccept.Teste.Domain.Interfaces.Services
{
    public interface IEmbarcadorasTransportadorasService : ICrud<EmbarcadoraTransportadora, EmbarcadoraTransportadoraParaConsultarVM>, IApagar<EmbarcadoraTransportadora>
    {
        Task<IEnumerable<Empresa>> ConsultarAssociadosPelaEmbarcadoraAsync(int embarcadoraId);
        Task<IEnumerable<Empresa>> ConsultarNaoAssociadosPelaEmbarcadoraAsync(int embarcadoraId);
        Task<IEnumerable<Empresa>> ConsultarAssociadosPelaTransportadoraAsync(int transportadoraId);
        Task<IEnumerable<Empresa>> ConsultarNaoAssociadosPelaTransportadoraAsync(int transportadoraId);
    }
}
