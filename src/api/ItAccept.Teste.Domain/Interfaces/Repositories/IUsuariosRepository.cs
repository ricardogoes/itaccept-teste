using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.Interfaces.Generic;
using ItAccept.Teste.Domain.ViewModels.Usuarios;

namespace ItAccept.Teste.Domain.Interfaces.Repositories
{
    public interface IUsuariosRepository : ICrud<Usuario, UsuarioParaConsultarVM>, IInativar<Usuario>
    {
        Task<IEnumerable<UsuarioParaConsultarVM>> ConsultarPelaEmpresaAsync(int empresaId);
        Task<IEnumerable<UsuarioParaConsultarVM>> ConsultarAtivosPelaEmpresaAsync(int empresaId);
        Task<UsuarioParaConsultarVM> ConsultarPeloUsernameAsync(string username);
        Task<UsuarioParaConsultarVM> ConsultarPeloUsernameEPasswordAsync(string username, string password);
    }
}
