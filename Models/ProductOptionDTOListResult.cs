using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductsRESTApi.Models
{
    public class ProductOptionDTOListResult
    {
        [JsonPropertyName("items")]
        public List<ProductOptionDTO> Items { get; set; }
    }
}