namespace ItAccept.Teste.Domain.Entities
{
    public class Oferta
    {
        public int OfertaId { get; set; }
        public int EmbarcadoraId { get; set; }
        public int ProdutoId { get; set; }
        public decimal Quantidade { get; set; }
        public string EnderecoOrigem { get; set; }
        public string EnderecoDestino { get; set; }
        public bool Status { get; set; }
    }
}
