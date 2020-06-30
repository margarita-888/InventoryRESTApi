using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductsRESTApi.Models
{
    public class ProductListResult
    {
        [JsonPropertyName("items")]
        public List<ProductDTO> Items { get; set; }
    }
}