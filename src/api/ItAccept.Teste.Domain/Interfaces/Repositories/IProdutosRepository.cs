using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.Generic;
using ItAccept.Teste.Domain.ViewModels.Produtos;

namespace ItAccept.Teste.Domain.Interfaces.Repositories
{
    public interface IProdutosRepository : ICrud<Produto, ProdutoParaConsultarVM>, IInativar<Produto>
    {
        Task<IEnumerable<ProdutoParaConsultarVM>> ConsultarPelaEmbarcadoraAsync(int embarcadoraId);
        Task<IEnumerable<ProdutoParaConsultarVM>> ConsultarAtivosPelaEmbarcadoraAsync(int embarcadoraId);
    }
}
