namespace Shared.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Sku { get; set; }
        public string Descricao { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public decimal Saldo { get; set; }
    }
}