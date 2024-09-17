using Shared.Enums;

namespace Shared.Models
{
    public class MovimentacoesModel
    {
        public int IdMovimentacao { get; set; }
        public int IdProduto { get; set; } 
        public ETipo Tipo { get; set; }
        public int Quantidade { get; set; } = 0;
        public DateTime Data { get; set; } = DateTime.UtcNow;
    }
}