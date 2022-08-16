using ItAccept.Teste.Domain.Enums;

namespace ItAccept.Teste.Domain.ViewModels.Usuarios
{
    public class UsuarioParaConsultarVM
    {
        public int UsuarioId { get; set; }
        public string NomeUsuario { get; set; }
        public string Username { get; set; }
        public int EmpresaId { get; set; }
        public string NomeEmpresa { get; set; }
        public bool Status { get; set; }
        public string TipoUsuario { get; set; }
        public string TipoEmpresa { get; set; }
    }
}
