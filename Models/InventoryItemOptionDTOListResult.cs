using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InventoryRESTApi.Models
{
    public class InventoryItemOptionDTOListResult
    {
        [JsonPropertyName("items")]
        public List<InventoryItemOptionDTO> Items { get; set; }
    }
}