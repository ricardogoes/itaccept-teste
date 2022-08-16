using System.ComponentModel.DataAnnotations;

namespace ItAccept.Teste.Domain.ViewModels.Ofertas
{
    public class OfertaParaAtualizarVM
    {
        [Key, Required(ErrorMessage = "OfertaId obrigatório")]
        public int? OfertaId { get; set; }

        [Required(ErrorMessage = "EmbarcadoraId obrigatório")]
        public int? EmbarcadoraId { get; set; }
        
        [Required(ErrorMessage = "ProdutoId obrigatório")]
        public int? ProdutoId { get; set; }
        
        [Required(ErrorMessage = "Quantidade obrigatório")]
        public decimal? Quantidade { get; set; }
        
        [Required(ErrorMessage = "EnderecoOrigem obrigatório")]
        public string EnderecoOrigem { get; set; }
        
        [Required(ErrorMessage = "EnderecoDestino obrigatório")]
        public string EnderecoDestino { get; set; }
    }
}
