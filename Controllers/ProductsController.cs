using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsRESTApi.Interfaces;
using ProductsRESTApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsRESTApi.Controllers
{
    /// <summary>Products Controller Class that allows to API calls.</summary>
    [Route("api/products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productsService;

        public ProductsController(IProductsService productService)
        {
            _productsService = productService;
        }

        /// <summary>
        /// Retrieve either a list of all products or products with a particular name when the name is provided in the query string
        /// </summary>
        /// <param name="name">Product name</param>
        /// <returns>List of products</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// GET: api/Products
        /// GET: api/products?name={name}
        [HttpGet]
        [Route("")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductListResult))]
        public async Task<ActionResult<ProductListResult>> GetProducts([FromQuery] string name = "")
        {
            //if (string.IsNullOrEmpty(name))
            //    return await _productsService.GetProducts();

            return await _productsService.GetProducts(name);
        }

        /// <summary>Gets the product by identifier.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The product.</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// GET: api/Products/07f98a2c-72b6-40af-b50d-d3cc1bc96f91
        [HttpGet("{id:Guid}")]
        [Produces("application/json")]
        public async Task<ActionResult<ProductDTO>> GetProductById([FromRoute] Guid id)
        {
            return await _productsService.GetProductById(id);
        }

        /// <summary>Updates the product.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="product">The product to be updated.</param>
        /// <returns>Status code</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// PUT: api/Products/07f98a2c-72b6-40af-b50d-d3cc1bc96f91
        [HttpPut("{id:Guid}")]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] ProductRequest product)
        {
            if (!ModelState.IsValid)

            {
                return BadRequest(ModelState);
            }

            return await _productsService.UpdateProduct(id, product);
        }

        // POST: api/Products
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        /// <summary>Creates the product.</summary>
        /// <param name="productRequest">The product request.</param>
        /// <returns>Created product.</returns>
        [HttpPost]
        [Route("")]
        [Produces("application/json")]
        public async Task<ActionResult<ProductDTO>> CreateProduct([FromBody] ProductRequest productRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _productsService.CreateProduct(productRequest);
        }

        /// DELETE: api/Products/07f98a2c-72b6-40af-b50d-d3cc1bc96f91
        /// <summary>Deletes the product.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Status code</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        [HttpDelete("{id:Guid}")]
        [Produces("application/json")]
        public async Task<ActionResult> DeleteProduct([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _productsService.DeleteProduct(id);
        }

        /// GET: api/products/07f98a2c-72b6-40af-b50d-d3cc1bc96f91/options
        /// <summary>Gets the product options.</summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Product options</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        [HttpGet("{id:Guid}/options")]
        public async Task<ActionResult<ProductOptionDTOListResult>> GetProductOptions([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _productsService.GetProductOptions(id);
        }

        /// GET: api/products/07f98a2c-72b6-40af-b50d-d3cc1bc96f91/options/ff3bfa42-ea12-46a3-9bae-9c13ae550610
        /// <summary>Gets the product option by product and option identifiers.</summary>
        /// <param name="id">The product identifier.</param>
        /// <param name="optionId">The option identifier.</param>
        /// <returns>Product option.</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        [HttpGet("{id}/options/{optionId}")]
        public async Task<ActionResult<ProductOptionDTO>> GetProductOption([FromRoute] Guid id, [FromRoute] Guid optionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _productsService.GetProductOption(id, optionId);
        }

        /// PUT: api/products/07f98a2c-72b6-40af-b50d-d3cc1bc96f91/options/ff3bfa42-ea12-46a3-9bae-9c13ae550610
        /// <summary>Updates the product option.</summary>
        /// <param name="id">The product identifier.</param>
        /// <param name="optionId">The option identifier.</param>
        /// <param name="productOption">The product option.</param>
        /// <returns>Status code</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        [HttpPut("{id:Guid}/options/{optionId:Guid}")]
        public async Task<IActionResult> UpdateProductOption([FromRoute] Guid id, [FromRoute] Guid optionId, [FromBody] ProductOptionRequest productOption)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _productsService.UpdateProductOption(id, optionId, productOption);
        }

        /// POST: api/products/07f98a2c-72b6-40af-b50d-d3cc1bc96f91/options
        /// <summary>Creates the product option.</summary>
        /// <param name="id">The product identifier.</param>
        /// <param name="productOption">The product option.</param>
        /// <returns>Created product option</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        [HttpPost("{id:Guid}")]
        public async Task<ActionResult<ProductOptionDTO>> CreateProductOption([FromRoute] Guid id, [FromBody] ProductOptionRequest productOption)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _productsService.CreateProductOption(id, productOption);
        }

        /// DELETE: api/products/07f98a2c-72b6-40af-b50d-d3cc1bc96f91/options/ff3bfa42-ea12-46a3-9bae-9c13ae550610
        /// <summary>Deletes the product option.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="optionId">The option identifier.</param>
        /// <returns>Status code</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        [HttpDelete("{id:Guid}/options/{optionId:Guid}")]
        public async Task<ActionResult> DeleteProductOption([FromRoute] Guid id, [FromRoute] Guid optionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return await _productsService.DeleteProductOption(id, optionId);
        }
    }
}