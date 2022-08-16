using ItAccept.Teste.Domain.Models;

namespace ItAccept.Teste.Domain.Interfaces.Repositories
{
    public interface IPasswordsRepository
    {
        Task<byte[]> ConsultarHashPasswordPorUsernameAsync(string username);
        Task<PasswordInfo> ConsultarPasswordInfoPorUsernameAsync(string username);
    }
}
