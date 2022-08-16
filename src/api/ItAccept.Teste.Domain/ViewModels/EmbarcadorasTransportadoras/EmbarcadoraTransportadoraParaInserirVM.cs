using System.ComponentModel.DataAnnotations;

namespace ItAccept.Teste.Domain.ViewModels.EmbarcadorasTransportadoras
{
    public class EmbarcadoraTransportadoraParaInserirVM
    {
        [Required(ErrorMessage = "EmbarcadoraId obrigatório")]
        public int? EmbarcadoraId { get; set; }
        
        [Required(ErrorMessage = "TransportadoraId obrigatório")]
        public int? TransportadoraId { get; set; }        
    }
}
