using ItAccept.Teste.Domain.Entities;
using ItAccept.Teste.Domain.ViewModels.Usuarios;

namespace ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth.Interfaces
{
    public interface ITokenAuthService
    {
        string GenerateToken(UsuarioParaConsultarVM usuario);
    }
}
