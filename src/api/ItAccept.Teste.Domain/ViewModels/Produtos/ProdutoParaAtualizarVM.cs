using System.ComponentModel.DataAnnotations;

namespace ItAccept.Teste.Domain.ViewModels.Produtos
{
    public class ProdutoParaAtualizarVM
    {
        [Key, Required(ErrorMessage = "ProdutoId obrigatório")]
        public int? ProdutoId { get; set; }
        
        [Required(ErrorMessage = "EmbarcadoraId obrigatório")]
        public int? EmbarcadoraId { get; set; }
        
        [Required(ErrorMessage = "EmbarcadNomeProdutooraId obrigatório")]
        public string NomeProduto { get; set; }
    }
}
