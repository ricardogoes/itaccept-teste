using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.Generic;
using ItAccept.Teste.Domain.ViewModels.Ofertas;

namespace ItAccept.Teste.Domain.Interfaces.Repositories
{
    public interface IOfertasRepository : ICrud<Oferta, OfertaParaConsultarVM>, IInativar<Oferta>
    {
        Task<IEnumerable<OfertaParaConsultarVM>> ConsultarPelaEmbarcadoraAsync(int embarcadoraId);
        Task<IEnumerable<OfertaParaConsultarVM>> ConsultarAtivosPelaEmbarcadoraAsync(int embarcadoraId);
        Task<IEnumerable<OfertaParaConsultarVM>> ConsultarPeloProdutoAsync(int produtoId);
        Task<IEnumerable<OfertaParaConsultarVM>> ConsultarAtivosPeloProdutoAsync(int produtoId);
        Task<IEnumerable<OfertaParaConsultarVM>> ConsultarAtivosPelaTransportadoraAsync(int transportadoraId);
    }
}
