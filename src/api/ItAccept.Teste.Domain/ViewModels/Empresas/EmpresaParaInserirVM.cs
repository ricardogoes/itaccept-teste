using ItAccept.Teste.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ItAccept.Teste.Domain.ViewModels.Empresas
{
    public class EmpresaParaInserirVM
    {
        [Required(ErrorMessage = "NomeEmpresa obrigatório")]
        public string NomeEmpresa { get; set; }

        [Required(ErrorMessage = "TipoEmpresa obrigatório")]
        public string TipoEmpresa { get; set; }
    }
}
