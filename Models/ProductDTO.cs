using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProductsRESTApi.Models
{
    public class ProductDTO  // DTO stands for Data Transfer Object
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        [Required]
        [StringLength(17)]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        [Required]
        [StringLength(35)]
        public string Description { get; set; }

        [JsonPropertyName("price")]
        [Required]
        [Range(typeof(decimal), "1", "5000")]
        public decimal Price { get; set; }

        [JsonPropertyName("deliveryprice")]
        [Required]
        [Range(typeof(decimal), "0", "5000")]
        public decimal DeliveryPrice { get; set; }
    }
}