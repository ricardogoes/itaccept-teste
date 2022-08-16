using ItAccept.Teste.Domain.ViewModels.Usuarios;

namespace ItAccept.Teste.Infrastructure.Crosscutting.TokenAuth.Models
{
    public class AuthResponse
    {
        public DateTime RequestAt { get; set; }
        public double ExpiresIn { get; set; }
        public string TokenType { get; set; }
        public string AccessToken { get; set; }
        public UsuarioParaConsultarVM UsuarioLogado { get; set; }
    }
}
