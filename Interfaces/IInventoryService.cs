using Microsoft.AspNetCore.Mvc;
using InventoryRESTApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InventoryRESTApi.Interfaces
{
    public interface IInventoryService
    {
        Task<ActionResult<InventoryItemDTOListResult>> GetInventoryItems(string name = "");

        Task<ActionResult<InventoryItemDTOListResult>> GetTop3InventoryItems();

        Task<ActionResult<InventoryItemDTOListResult>> GetInventoryItemsWithOptionByName(string name);

        Task<ActionResult<InventoryItemDTO>> GetInventoryItemById(Guid id);

        Task<IActionResult> UpdateInventoryItem(Guid id, InventoryItemRequest InventoryItemRequest);

        Task<ActionResult<InventoryItemDTO>> CreateInventoryItem(InventoryItemRequest InventoryItemCreate);

        Task<ActionResult> DeleteInventoryItem(Guid id);

        Task<ActionResult<InventoryItemOptionDTOListResult>> GetInventoryItemOptions(Guid id);

        Task<ActionResult<InventoryItemOptionDTO>> GetInventoryItemOption(Guid id, Guid optionId);

        Task<IActionResult> UpdateInventoryItemOption(Guid id, Guid optionId, InventoryItemOptionRequest InventoryItemOptionRequest);

        Task<ActionResult<InventoryItemOptionDTO>> CreateInventoryItemOption(Guid id, InventoryItemOptionRequest InventoryItemOptionRequest);

        Task<IActionResult> DeleteInventoryItemOption(Guid id, Guid optionId);
    }
}
