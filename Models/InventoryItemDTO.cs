using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InventoryRESTApi.Models
{
    public class InventoryItemDTO  // DTO stands for Data Transfer Object
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        [Required]
        [StringLength(35)]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        [JsonPropertyName("price")]
        [Required]
        [Range(typeof(double), "1", "5000")]
        public double Price { get; set; }

        [JsonPropertyName("deliveryprice")]
        [Required]
        [Range(typeof(double), "0", "5000")]
        public double DeliveryPrice { get; set; }
    }
}