using System.ComponentModel.DataAnnotations;

namespace ItAccept.Teste.Domain.ViewModels.EmbarcadorasTransportadoras
{
    public class EmbarcadoraTransportadoraParaConsultarVM
    {
        public int EmbarcadoraTransportadoraId { get; set; }
        public int EmbarcadoraId { get; set; }
        public string NomeEmbarcadora { get; set; }
        public int TransportadoraId { get; set; }
        public string NomeTransportadora { get; set; }
    }
}
