using System.Text.Json.Serialization;

namespace Shared.DTO
{
    public class ProductDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string Sku { get; set; }
        public string Description { get; set; }
        [JsonIgnore]
        public DateTime Created { get; set; }
        [JsonIgnore]
        public DateTime Updated { get; set; }
        public int Balance { get; set; }
    }
}