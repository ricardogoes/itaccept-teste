namespace ItAccept.Teste.Domain.ViewModels.Lances
{
    public class LanceParaConsultarVM
    {
        public int LanceId { get; set; }
        public int OfertaId { get; set; }
        public int TransportadoraId { get; set; }
        public string NomeTransportadora { get; set; }
        public decimal Volume { get; set; }
        public decimal Preco { get; set; }
        public bool LanceVencedor { get; set; }
        public bool Status { get; set; }
    }
}
