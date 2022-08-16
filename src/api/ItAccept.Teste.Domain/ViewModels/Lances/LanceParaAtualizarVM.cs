using System.ComponentModel.DataAnnotations;

namespace ItAccept.Teste.Domain.ViewModels.Lances
{
    public class LanceParaAtualizarVM
    {
        [Key, Required(ErrorMessage = "LanceId obrigatório")]
        public int? LanceId { get; set; }
        
        [Required(ErrorMessage = "OfertaId obrigatório")]
        public int? OfertaId { get; set; }
        
        [Required(ErrorMessage = "TransportadoraId obrigatório")]
        public int? TransportadoraId { get; set; }
        
        [Required(ErrorMessage = "Volume obrigatório")]
        public decimal? Volume { get; set; }
        
        [Required(ErrorMessage = "Preco obrigatório")]
        public decimal? Preco { get; set; }
        
    }
}
