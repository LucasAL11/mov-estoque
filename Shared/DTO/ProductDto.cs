using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        public decimal Balance { get; set; }
    }
}
