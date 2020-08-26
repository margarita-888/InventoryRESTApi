using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using InventoryRESTApi.Interfaces;
using InventoryRESTApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryRESTApi.Data
{
    public class InventoryService : IInventoryService
    {
        private readonly InventoryContext _context;

        private readonly ILogger<Program> _logger;

        public InventoryService(InventoryContext context, ILogger<Program> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ActionResult<InventoryItemDTOListResult>> GetInventoryItems(string name = "")
        {
            InventoryItemDTOListResult result = new InventoryItemDTOListResult();
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    result.Items = await _context.InventoryItem.Select(p => InventoryItemToDTO(p)).ToListAsync();
                    _logger.LogInformation($"InventoryService::GetInventory. Found {result.Items.Count()} inventory items.");
                }
                else
                {
                    result.Items = await _context.InventoryItem.Where(p => p.Name == name.ToLower()).Select(p => InventoryItemToDTO(p)).ToListAsync();
                    _logger.LogInformation($"InventoryService::GetInventory. Found {result.Items.Count()} Inventory with name {name}.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"InventoryService::GetInventory. Exception: {ex.Message}", ex);
            }

            return new OkObjectResult(result.Items);
        }

        public async Task<ActionResult<InventoryItemDTOListResult>> GetTop3InventoryItems()
        {
            InventoryItemDTOListResult result = new InventoryItemDTOListResult();
            try
            {
                result.Items = await _context.InventoryItem.OrderByDescending(p => p.Price).Take(3).Select(p => InventoryItemToDTO(p)).ToListAsync();
                _logger.LogInformation($"InventoryService::GetTop3InventoryItems. Successfully found top 3 inventory items.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"InventoryService::GetTop3InventoryItems. Exception: {ex.Message}", ex);
            }

            return new OkObjectResult(result.Items);
        }

        public async Task<ActionResult<InventoryItemDTOListResult>> GetInventoryItemsWithOptionByName(string name)
        {
            InventoryItemDTOListResult result = new InventoryItemDTOListResult();
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    _logger.LogInformation($"InventoryService::GetInventoryItemsWithOptionByName. Error: Inventory item option name must be provided.");
                    return new BadRequestObjectResult(new { statuscode = 400, message = "Option name must be provided" });
                }
                else
                {
                    var items = await _context.InventoryItem.Include(i => i.InventoryItemOptions).Where(p => p.InventoryItemOptions.Any(o => o.Name.ToLower() == name.ToLower())).ToListAsync();

                    if (items == null)
                        return new BadRequestObjectResult(new { statuscode = 400, message = $"No inventory items with option name {name} found." });

                    result.Items = items.Select(i => InventoryItemToDTO(i)).ToList();
                    _logger.LogInformation($"InventoryService::GetInventoryItemsWithOptionByName. Found {result.Items.Count()} Inventory items with option name {name}.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"InventoryService::GetInventoryItemsWithOptionByName. Exception: {ex.Message}", ex);
            }

            return new OkObjectResult(result.Items);
        }

        public async Task<ActionResult<InventoryItemDTO>> GetInventoryItemById(Guid id)
        {
            InventoryItem InventoryItem = null;

            if (id == Guid.Empty)
            {
                _logger.LogError("InventoryService::GetInventoryItemById. Error: id must be a valid Guid.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "id must be a valid Guid" });
            }

            try
            {
                InventoryItem = await _context.InventoryItem.FindAsync(id);

                if (InventoryItem == null)
                {
                    _logger.LogInformation($"InventoryService::GetInventoryItemById. No InventoryItem with id {id} was found.");
                    return new NotFoundResult();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"InventoryService::GetInventoryItemById. Exception: {ex.Message}", ex);
            }

            _logger.LogInformation($"InventoryService::GetInventoryItemById. Successfully found Inventory item with id {id}.");
            return new OkObjectResult(InventoryItemToDTO(InventoryItem));
        }

        public async Task<IActionResult> UpdateInventoryItem(Guid id, InventoryItemRequest InventoryItemRequest)
        {
            if (id == Guid.Empty || id == null)
            {
                _logger.LogError("InventoryService::UpdateInventoryItem. Error: id must be a valid Guid.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "id must be a valid Guid." });
            }

            if (InventoryItemRequest == null)
            {
                _logger.LogError("InventoryService::UpdateInventoryItem. Error: Unable to update Inventory item. Inventory item must be provided..");
                return new BadRequestObjectResult(new { statuscode = 400, message = "Inventory item must be provided." });
            }

            var InventoryItem = await _context.InventoryItem.FindAsync(id);
            if (InventoryItem == null)
            {
                _logger.LogError($"InventoryService::UpdateInventoryItem. Error. Unable to find Inventory item with id {id}.");
                return new NotFoundObjectResult($"InventoryService::UpdateInventoryItem. Error. Unable to find Inventory item with id {id}.");
            }

            InventoryItem.Name = InventoryItemRequest.Name;
            InventoryItem.Description = InventoryItemRequest.Description;
            InventoryItem.Price = InventoryItemRequest.Price;
            InventoryItem.DeliveryPrice = InventoryItemRequest.DeliveryPrice;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex) when (!InventoryItemExists(id))
            {
                _logger.LogError($"InventoryService::UpdateInventoryItem. Error. Unable to find Inventory item with id {id}. Exception: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"InventoryService::UpdateInventoryItem. Exception: {ex.Message}", ex);
            }

            return new NoContentResult();
        }

        public async Task<ActionResult<InventoryItemDTO>> CreateInventoryItem(InventoryItemRequest InventoryItemRequest)
        {
            if (InventoryItemRequest == null)
            {
                _logger.LogError("InventoryService::CreateInventoryItem. Error: Unable to create Inventory item. A new Inventory item must be provided.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "A new Inventory item must be provided." });
            }

            var InventoryItem = new InventoryItem()
            {
                Id = new Guid(),
                Name = InventoryItemRequest.Name,
                Description = InventoryItemRequest.Description,
                Price = InventoryItemRequest.Price,
                DeliveryPrice = InventoryItemRequest.DeliveryPrice
            };

            try
            {
                _context.InventoryItem.Add(InventoryItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"InventoryService::CreateInventoryItem. Exception: {ex.Message}", ex);
            }

            InventoryItem = await _context.InventoryItem.FindAsync(InventoryItem.Id);
            if (InventoryItem == null)
            {
                _logger.LogError("InventoryService::CreateInventoryItem. Error creating a new Inventory item.");
                return new NotFoundResult();
            }
            return new OkObjectResult(InventoryItemToDTO(InventoryItem));
        }

        public async Task<ActionResult> DeleteInventoryItem(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError("InventoryService::DeleteInventoryItem. Error: id must be a valid Guid.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "id must be a valid Guid." });
            }

            var InventoryItem = await _context.InventoryItem.FindAsync(id);

            if (InventoryItem == null)
            {
                return new NotFoundResult();
            }

            if (InventoryItem.InventoryItemOptions != null && InventoryItem.InventoryItemOptions.Count > 0)
            {
                foreach (var option in InventoryItem.InventoryItemOptions)
                {
                    _context.InventoryItemOptions.Remove(option);
                }
            }

            try
            {
                _context.InventoryItem.Remove(InventoryItem);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"InventoryService::DeleteInventoryItem. Exception: {ex.Message}", ex);
            }

            return new NoContentResult();
        }

        public async Task<ActionResult<InventoryItemOptionDTOListResult>> GetInventoryItemOptions(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError("InventoryService::GetInventoryItemOptions. Error: Inventory item id must be a valid Guid.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "Inventory item id must be a valid Guid." });
            }

            InventoryItemOptionDTOListResult result = new InventoryItemOptionDTOListResult();

            try
            {
                var InventoryItem = await _context.InventoryItem.FindAsync(id);
                result.Items = await _context.InventoryItemOptions.Where(po => po.InventoryItemId == id).Select(po => InventoryItemOptionToDTO(po)).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"InventoryService::GetInventoryItemOptions. Exception: {ex.Message}", ex);
            }

            return new OkObjectResult(result.Items);
        }

        public async Task<ActionResult<InventoryItemOptionDTO>> GetInventoryItemOption(Guid id, Guid optionId)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError("InventoryService::GetInventoryItemOptions. Error: Inventory item id must be a valid Guid.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "Inventory item id must be a valid Guid." });
            }

            if (optionId == Guid.Empty)
            {
                _logger.LogError("InventoryService::GetInventoryItemOptions. Error: Inventory item option id must be a valid Guid.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "option id must be a valid Guid." });
            }

            InventoryItemOptionDTO InventoryItemOption = null;

            try
            {
                InventoryItemOption = await _context.InventoryItemOptions.Where(po => po.Id == optionId && po.InventoryItemId == id).Select(po => InventoryItemOptionToDTO(po)).FirstOrDefaultAsync();
                if (InventoryItemOption == null)
                {
                    return new NotFoundObjectResult($"Inventory item option with id {optionId} was not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"InventoryService::GetInventoryItemOption. Exception: {ex.Message}", ex);
            }

            return new OkObjectResult(InventoryItemOption);
        }

        public async Task<IActionResult> UpdateInventoryItemOption(Guid id, Guid optionId, InventoryItemOptionRequest InventoryItemOptionRequest)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError("InventoryService::UpdateInventoryItemOption. Error: Inventory item id must be a valid Guid.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "Inventory item id must be a valid Guid." });
            }

            if (optionId == Guid.Empty)
            {
                _logger.LogError("InventoryService::UpdateInventoryItemOption. Error: Inventory item option id must be a valid Guid.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "Inventory item option id must be a valid Guid." });
            }

            if (InventoryItemOptionRequest == null)
            {
                _logger.LogError("InventoryService::UpdateInventoryItemOption. Error: no Inventory item option to update was provided.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "Inventory item option must be provided." });
            }

            var InventoryItemOption = await _context.InventoryItemOptions.FindAsync(optionId);
            if (InventoryItemOption == null)
            {
                return new NotFoundResult();
            }
            if (id != InventoryItemOption.InventoryItemId)
            {
                _logger.LogError("InventoryService::UpdateInventoryItemOption. Error: InventoryItem id provided does not match InventoryItemId of the option to be updated.");
                return new BadRequestResult();
            }

            InventoryItemOption.Name = InventoryItemOptionRequest.Name;
            InventoryItemOption.Description = InventoryItemOptionRequest.Description;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex) when (!InventoryItemOptionExists(id))
            {
                _logger.LogError($"InventoryService::UpdateInventoryItem. Error. Unable to find InventoryItem with id {id}. Exception: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"InventoryService::UpdateInventoryItem. Exception: {ex.Message}", ex);
            }

            return new NoContentResult();
        }

        public async Task<ActionResult<InventoryItemOptionDTO>> CreateInventoryItemOption(Guid InventoryItemId, InventoryItemOptionRequest InventoryItemOptionRequest)
        {
            if (InventoryItemId == Guid.Empty)
            {
                _logger.LogError("InventoryService::CreateInventoryItemOption. Error: Inventory item id must be a valid Guid.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "Inventory item id must be a valid Guid." });
            }

            if (InventoryItemOptionRequest == null)
            {
                _logger.LogError("InventoryService::CreateInventoryItemOption. Error: no Inventory item option to update was provided.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "Inventory item option must be provided." });
            }

            var InventoryItem = await _context.InventoryItem.FindAsync(InventoryItemId);
            if (InventoryItem == null)
            {
                return new NotFoundResult();
            }

            var InventoryItemOption = new InventoryItemOption()
            {
                Id = new Guid(),
                InventoryItemId = InventoryItemId,
                Name = InventoryItemOptionRequest.Name,
                Description = InventoryItemOptionRequest.Description
            };

            try
            {
                _context.InventoryItemOptions.Add(InventoryItemOption);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"InventoryService::CreateInventoryItemOption. Exception: {ex.Message}", ex);
            }

            InventoryItemOption = await _context.InventoryItemOptions.FindAsync(InventoryItemOption.Id);
            if (InventoryItemOption == null)
            {
                _logger.LogError("InventoryService::CreateInventoryItemOption. Error creating a new InventoryItem.");
                return new NotFoundResult();
            }

            return new OkObjectResult(InventoryItemOptionToDTO(InventoryItemOption));
        }

        public async Task<IActionResult> DeleteInventoryItemOption(Guid id, Guid optionId)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError("InventoryService::DeleteInventoryItemOption. Error: Inventory item id must be a valid Guid.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "Inventory item id must be a valid Guid." });
            }

            if (optionId == Guid.Empty)
            {
                _logger.LogError("InventoryService::DeleteInventoryItemOption. Error: Inventory item option id must be a valid Guid.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "Inventory item option id must be a valid Guid." });
            }

            var InventoryItemOption = await _context.InventoryItemOptions.FindAsync(optionId);
            if (InventoryItemOption == null)
            {
                return new NotFoundResult();
            }

            if (id != InventoryItemOption.InventoryItemId)
            {
                _logger.LogError("InventoryService::DeleteInventoryItemOption. Error: Inventory item id of the option must match the InventoryItem this option belongs to..");
                return new BadRequestObjectResult(new { statuscode = 400, message = "Inventory item id of the option must match the InventoryItem this option belongs to." });
            }

            try
            {
                _context.InventoryItemOptions.Remove(InventoryItemOption);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"InventoryService::DeleteInventoryItemOption. Exception: {ex.Message}", ex);
            }
            return new NoContentResult();
        }

        private bool InventoryItemExists(Guid id)
        {
            return _context.InventoryItem.Any(e => e.Id == id);
        }

        private bool InventoryItemOptionExists(Guid id)
        {
            return _context.InventoryItemOptions.Any(e => e.Id == id);
        }

        private static InventoryItemDTO InventoryItemToDTO(InventoryItem InventoryItem) =>
        new InventoryItemDTO
        {
            Id = InventoryItem.Id,
            Name = InventoryItem.Name,
            Description = InventoryItem.Description,
            Price = InventoryItem.Price,
            DeliveryPrice = InventoryItem.DeliveryPrice
        };

        private static InventoryItemOptionDTO InventoryItemOptionToDTO(InventoryItemOption InventoryItemOption) =>
        new InventoryItemOptionDTO
        {
            Id = InventoryItemOption.Id,
            InventoryItemId = InventoryItemOption.InventoryItemId,
            Name = InventoryItemOption.Name,
            Description = InventoryItemOption.Description
        };
    }
}