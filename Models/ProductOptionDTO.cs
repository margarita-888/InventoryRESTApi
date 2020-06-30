using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductsRESTApi.Models
{
    public class ProductOptionDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("productid")]
        [Required]
        public Guid ProductId { get; set; }

        [JsonPropertyName("name")]
        [Required]
        [StringLength(9)]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        [Required]
        [StringLength(23)]
        public string Description { get; set; }
    }
}