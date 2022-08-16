using ItAccept.Teste.Domain.Enums;

namespace ItAccept.Teste.Domain.Entities
{
    public class Usuario
    {
        public int UsuarioId { get; set; }
        public string NomeUsuario { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public byte[] PasswordHash { get; set; }
        public string EmpresaId { get; set; }
        public bool Status { get; set; }
        public string TipoUsuario { get; set; }
    }
}
