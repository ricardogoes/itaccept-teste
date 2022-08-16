using System.ComponentModel.DataAnnotations;

namespace ItAccept.Teste.Domain.ViewModels.Empresas
{
    public class EmpresaParaAtualizarVM
    {
        [Key, Required(ErrorMessage = "EmpresaId obrigatório")]
        public int? EmpresaId { get; set; }
        
        [Required(ErrorMessage = "NomeEmpresa obrigatório")]
        public string NomeEmpresa { get; set; }

        [Required(ErrorMessage = "TipoEmpresa obrigatório")]
        public string TipoEmpresa { get; set; }
    }
}
