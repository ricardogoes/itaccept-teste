using ItAccept.Teste.Domain.Enums;

namespace ItAccept.Teste.Domain.Entities
{
    public class Empresa
    {
        public int EmpresaId { get; set; }
        public string NomeEmpresa { get; set; }
        public bool Status { get; set; }
        public string TipoEmpresa { get; set; }
    }
}
