using ItAccept.Teste.Domain.Models;
using ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth.Models;

namespace ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth.Interfaces
{
    public interface IPasswordsService
    {
        HashPassword GerarPasswordRandomico();
        HashPassword Encriptar(string password);
        string GerarHashPassword(string password, byte[] salt);
        Task<byte[]> ConsultarHashPasswordPorUsernameAsync(string username);
        Task<PasswordInfo> ConsultarPasswordInfoPorUsernameAsync(string username);
    }
}
