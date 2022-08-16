namespace ItAccept.Teste.Domain.ViewModels.Produtos
{
    public class ProdutoParaConsultarVM
    {
        public int ProdutoId { get; set; }
        public int EmbarcadoraId { get; set; }
        public string NomeEmbarcadora { get; set; }
        public string NomeProduto { get; set; }
        public bool Status { get; set; }
    }
}
