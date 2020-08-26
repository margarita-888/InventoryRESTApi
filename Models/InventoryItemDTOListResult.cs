using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace InventoryRESTApi.Models
{
    public class InventoryItemDTOListResult
    {
        [JsonPropertyName("items")]
        public List<InventoryItemDTO> Items { get; set; }
    }
}