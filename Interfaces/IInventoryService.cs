using Microsoft.AspNetCore.Mvc;
using InventoryRESTApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryRESTApi.Interfaces
{
    public interface IInventoryService
    {
        public Task<ActionResult<InventoryItemDTOListResult>> GetInventoryItems(string name = "");

        public Task<ActionResult<InventoryItemDTOListResult>> GetTop3InventoryItems();

        public Task<ActionResult<InventoryItemDTOListResult>> GetInventoryItemsWithOptionByName(string name);

        public Task<ActionResult<InventoryItemDTO>> GetInventoryItemById(Guid id);

        public Task<IActionResult> UpdateInventoryItem(Guid id, InventoryItemRequest InventoryItemRequest);

        public Task<ActionResult<InventoryItemDTO>> CreateInventoryItem(InventoryItemRequest InventoryItemCreate);

        public Task<ActionResult> DeleteInventoryItem(Guid id);

        public Task<ActionResult<InventoryItemOptionDTOListResult>> GetInventoryItemOptions(Guid id);

        public Task<ActionResult<InventoryItemOptionDTO>> GetInventoryItemOption(Guid id, Guid optionId);

        public Task<IActionResult> UpdateInventoryItemOption(Guid id, Guid optionId, InventoryItemOptionRequest InventoryItemOptionRequest);

        public Task<ActionResult<InventoryItemOptionDTO>> CreateInventoryItemOption(Guid id, InventoryItemOptionRequest InventoryItemOptionRequest);

        public Task<IActionResult> DeleteInventoryItemOption(Guid id, Guid optionId);
    }
}