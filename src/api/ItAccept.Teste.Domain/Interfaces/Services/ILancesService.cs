using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.Generic;
using ItAccept.Teste.Domain.ViewModels.Lances;

namespace ItAccept.Teste.Domain.Interfaces.Services
{
    public interface ILancesService : ICrud<Lance, LanceParaConsultarVM>, IInativar<Lance>
    {
        Task<IEnumerable<LanceParaConsultarVM>> ConsultarPelaOfertaAsync(int ofertaId);
        Task<IEnumerable<LanceParaConsultarVM>> ConsultarAtivosPelaOfertaAsync(int ofertaId);
        Task<IEnumerable<LanceParaConsultarVM>> ConsultarPelaTransportadoraAsync(int transportadoraId);
        Task<IEnumerable<LanceParaConsultarVM>> ConsultarAtivosPelaTransportadoraAsync(int transportadoraId);
    }
}
