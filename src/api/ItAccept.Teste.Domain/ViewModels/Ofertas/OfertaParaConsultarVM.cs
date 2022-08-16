namespace ItAccept.Teste.Domain.ViewModels.Ofertas
{
    public class OfertaParaConsultarVM
    {
        public int OfertaId { get; set; }
        public int EmbarcadoraId { get; set; }
        public string NomeEmbarcadora { get; set; }
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; }
        public decimal Quantidade { get; set; }
        public decimal QuantidadeDisponivel { get; set; }
        public string StatusOferta { get; set; }
        public string EnderecoOrigem { get; set; }
        public string EnderecoDestino { get; set; }
        public bool Status { get; set; }
    }
}
