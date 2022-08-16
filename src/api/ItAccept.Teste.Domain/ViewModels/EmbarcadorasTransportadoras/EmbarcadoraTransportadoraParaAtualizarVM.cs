using System.ComponentModel.DataAnnotations;

namespace ItAccept.Teste.Domain.ViewModels.EmbarcadorasTransportadoras
{
    public class EmbarcadoraTransportadoraParaAtualizarVM
    {
        [Key, Required(ErrorMessage = "EmbarcadoraTransportadoraId obrigatório")]
        public int EmbarcadoraTransportadoraId { get; set; }
                
        [Required(ErrorMessage = "EmbarcadoraId obrigatório")]
        public int? EmbarcadoraId { get; set; }
        
        [Required(ErrorMessage = "TransportadoraId obrigatório")]
        public int? TransportadoraId { get; set; }        
    }
}
