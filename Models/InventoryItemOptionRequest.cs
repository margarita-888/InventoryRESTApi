using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InventoryRESTApi.Models
{
    public class InventoryItemOptionRequest
    {
        [JsonPropertyName("name")]
        [Required]
        [StringLength(35)]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        [Required]
        [StringLength(100)]
        public string Description { get; set; }
    }
}