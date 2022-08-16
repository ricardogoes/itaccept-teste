using System.ComponentModel.DataAnnotations;

namespace ItAccept.Teste.Domain.ViewModels.Produtos
{
    public class ProdutoParaInserirVM
    {
        [Required(ErrorMessage = "EmbarcadoraId obrigatório")]
        public int? EmbarcadoraId { get; set; }
        
        [Required(ErrorMessage = "NomeProduto obrigatório")]
        public string NomeProduto { get; set; }
    }
}
