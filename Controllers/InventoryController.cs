using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.EntityFrameworkCore;
using InventoryRESTApi.Interfaces;
using InventoryRESTApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryRESTApi.Controllers
{
    /// <summary>Inventory Controller Class that allows to API calls.</summary>
    [Route("api/inventoryitems")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService InventoryService)
        {
            _inventoryService = InventoryService;
        }

        /// <summary>
        /// Retrieve either a list of all inventory items or inventory items with a name provided in the query string
        /// </summary>
        /// <param name="name">Name of inventory item</param>
        /// <returns>List of inventory items </returns>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        // GET: api/inventoryitems
        // GET: api/inventoryitems?name={name}
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InventoryItemDTOListResult))]
        public async Task<ActionResult<InventoryItemDTOListResult>> GetInventoryItems([FromQuery] string name = "")
        {
            //if (string.IsNullOrEmpty(name))
            //    return await _InventoryService.GetInventory();

            return await _inventoryService.GetInventoryItems(name);
        }

        /// <summary>
        /// Based on item price, retrieve top 3 inventory items in descending order with the highest price at the top
        /// </summary>
        /// <returns>List of inventory items </returns>
        /// <response code="200">Ok</response>
        /// <response code="204">No Content success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        // GET: api/inventoryitems/top3
        [HttpGet]
        [Route("top3")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InventoryItemDTOListResult))]
        public async Task<ActionResult<InventoryItemDTOListResult>> GetTop3Products()
        {
            return await _inventoryService.GetTop3InventoryItems();
        }

        /// <summary>Retrieve inventory item by its id.</summary>
        /// <param name="id">Inventory item identifier.</param>
        /// <returns>Inventory item.</returns>
        /// <response code="200">Ok</response>
        /// <response code="204">No Content success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        // GET: api/inventoryitems/07f98a2c-72b6-40af-b50d-d3cc1bc96f91
        [HttpGet("{id:Guid}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InventoryItemDTO))]
        public async Task<ActionResult<InventoryItemDTO>> GetInventoryItemById([FromRoute] Guid id)
        {
            return await _inventoryService.GetInventoryItemById(id);
        }

        /// <summary>
        /// Retrieve a list of inventory items that have option/s with a name provided in the query string
        /// </summary>
        /// <param name="name">Name of inventory item</param>
        /// <returns>List of inventory items </returns>
        /// <response code="200">Ok</response>
        /// <response code="204">No Content success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        // GET: api/inventoryitems/options?name={name}
        [HttpGet]
        [Route("options")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InventoryItemDTOListResult))]
        public async Task<ActionResult<InventoryItemDTOListResult>> GetInventoryItemsWithOptionByName([FromQuery] string name)
        {
            //if (string.IsNullOrEmpty(name))
            //    return await _InventoryService.GetInventory();

            return await _inventoryService.GetInventoryItemsWithOptionByName(name);
        }

        /// <summary>Updates inventory item.</summary>
        /// <param name="id">Inventory item identifier.</param>
        /// <param name="InventoryItem">Inventory item to be updated.</param>
        /// <returns>Status code</returns>
        /// <response code="204">No Content success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        // PUT: api/inventoryitems/07f98a2c-72b6-40af-b50d-d3cc1bc96f91
        [HttpPut("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(IActionResult))]
        public async Task<IActionResult> UpdateInventoryItem([FromRoute] Guid id, [FromBody] InventoryItemRequest InventoryItem)
        {
            if (!ModelState.IsValid)

            {
                return BadRequest(ModelState);
            }

            return await _inventoryService.UpdateInventoryItem(id, InventoryItem);
        }

        /// <summary>Creates a new inventory item.</summary>
        /// <param name="InventoryItemRequest">Inventory item to be created.</param>
        /// <returns>Created Inventory item.</returns>
        /// <response code="200">Ok</response>
        /// <response code="204">No Content success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        // POST: api/inventoryitems
        [HttpPost]
        [Route("")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InventoryItemDTO))]
        public async Task<ActionResult<InventoryItemDTO>> CreateInventoryItem([FromBody] InventoryItemRequest InventoryItemRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _inventoryService.CreateInventoryItem(InventoryItemRequest);
        }

        /// <summary>Delete Inventory item by identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Status code</returns>
        /// <response code="204">No Content success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        // DELETE: api/inventoryitems/07f98a2c-72b6-40af-b50d-d3cc1bc96f91
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(IActionResult))]
        public async Task<IActionResult> DeleteInventoryItem([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _inventoryService.DeleteInventoryItem(id);
        }

        /// <summary>Retrieve options of a particular inventory item by its identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Options of inventory item </returns>
        /// <response code="200">Ok</response>
        /// <response code="204">No Content success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        // GET: api/inventoryitems/07f98a2c-72b6-40af-b50d-d3cc1bc96f91/options
        [HttpGet("{id:Guid}/options")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InventoryItemOptionDTOListResult))]
        public async Task<ActionResult<InventoryItemOptionDTOListResult>> GetInventoryItemOptions([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _inventoryService.GetInventoryItemOptions(id);
        }

        /// <summary>Retrieve an option of an inventory item by item's and option's identifiers.</summary>
        /// <param name="id">Inventory item identifier.</param>
        /// <param name="optionId">Inventory item option identifier.</param>
        /// <returns>Inventory item option.</returns>
        /// <response code="200">Ok</response>
        /// <response code="204">No Content success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        // GET: api/inventoryitems/07f98a2c-72b6-40af-b50d-d3cc1bc96f91/options/ff3bfa42-ea12-46a3-9bae-9c13ae550610
        [HttpGet("{id}/options/{optionId}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InventoryItemOptionDTO))]
        public async Task<ActionResult<InventoryItemOptionDTO>> GetInventoryItemOption([FromRoute] Guid id, [FromRoute] Guid optionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _inventoryService.GetInventoryItemOption(id, optionId);
        }

        /// <summary>Update inventory item option.</summary>
        /// <param name="id">Inventory item identifier.</param>
        /// <param name="optionId">Inventory item option identifier.</param>
        /// <param name="InventoryItemOption">Inventory item option.</param>
        /// <returns>Status code</returns>
        /// <response code="200">Ok</response>
        /// <response code="204">No Content success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        // PUT: api/inventoryitems/07f98a2c-72b6-40af-b50d-d3cc1bc96f91/options/ff3bfa42-ea12-46a3-9bae-9c13ae550610
        [HttpPut("{id:Guid}/options/{optionId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(IActionResult))]
        public async Task<IActionResult> UpdateInventoryItemOption([FromRoute] Guid id, [FromRoute] Guid optionId, [FromBody] InventoryItemOptionRequest InventoryItemOption)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _inventoryService.UpdateInventoryItemOption(id, optionId, InventoryItemOption);
        }

        /// <summary>Create inventory item option.</summary>
        /// <param name="id">Inventory item identifier.</param>
        /// <param name="InventoryItemOption">Inventory item option to be created.</param>
        /// <returns>Inventory item option that has been created</returns>
        /// <response code="200">Ok</response>
        /// <response code="204">No Content success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        // POST: api/inventoryitems/07f98a2c-72b6-40af-b50d-d3cc1bc96f91/options
        [HttpPost("{id:Guid}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(InventoryItemOptionDTO))]
        public async Task<ActionResult<InventoryItemOptionDTO>> CreateInventoryItemOption([FromRoute] Guid id, [FromBody] InventoryItemOptionRequest InventoryItemOption)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _inventoryService.CreateInventoryItemOption(id, InventoryItemOption);
        }

        /// <summary>Delete inventory item option.</summary>
        /// <param name="id">Inventory item identifier.</param>
        /// <param name="optionId">Inventory item option identifier.</param>
        /// <returns>Status code</returns>
        /// <response code="204">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        // DELETE: api/inventoryitems/07f98a2c-72b6-40af-b50d-d3cc1bc96f91/options/ff3bfa42-ea12-46a3-9bae-9c13ae550610
        [HttpDelete("{id:Guid}/options/{optionId:Guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(IActionResult))]
        public async Task<IActionResult> DeleteInventoryItemOption([FromRoute] Guid id, [FromRoute] Guid optionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _inventoryService.DeleteInventoryItemOption(id, optionId);
        }
    }
}