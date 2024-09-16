namespace Shared.Models
{
    public class MovimentacoesModel
    {
        public int IdMovimentacao { get; set; }
        public int IdProduto { get; set; } 
        public bool Tipo { get; set; } = false;
        public int Quantidade { get; set; } = 0;
        public DateTime DataMovimentacao = DateTime.UtcNow;
    }
}