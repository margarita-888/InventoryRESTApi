using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductsRESTApi.Models
{
    public class ProductOptionRequest
    {
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