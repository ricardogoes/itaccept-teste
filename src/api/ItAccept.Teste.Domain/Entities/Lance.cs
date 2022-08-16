namespace ItAccept.Teste.Domain.Entities
{
    public class Lance
    {
        public int LanceId { get; set; }
        public int OfertaId { get; set; }
        public int TransportadoraId  { get; set; }
        public decimal Volume { get; set; }
        public decimal Preco { get; set; }
        public bool LanceVencedor { get; set; }
        public bool Status { get; set; }
    }
}
