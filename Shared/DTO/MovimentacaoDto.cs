using Shared.Enums;

namespace Shared.DTO
{
    public class MovimentacaoDto
    {
        public ETipo Tipo { get; set; }
        public int Quantidade { get; set; } = 0;
        public DateTime Data { get; set; } = DateTime.UtcNow;
    }
}