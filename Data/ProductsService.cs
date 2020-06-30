using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProductsRESTApi.Interfaces;
using ProductsRESTApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsRESTApi.Data
{
    public class ProductsService : IProductsService
    {
        private readonly ProductsContext _context;

        private readonly ILogger<Program> _logger;

        public ProductsService(ProductsContext context, ILogger<Program> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ActionResult<ProductListResult>> GetProducts(string name = "")
        {
            ProductListResult result = new ProductListResult();
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    result.Items = await _context.Products.Select(p => ProductToDTO(p)).ToListAsync();
                    _logger.LogInformation($"ProductsService::GetProducts. Found {result.Items.Count().ToString()} products.");
                }
                else
                {
                    result.Items = await _context.Products.Where(p => p.Name == name).Select(p => ProductToDTO(p)).ToListAsync();
                    _logger.LogInformation($"ProductsService::GetProducts. Found {result.Items.Count().ToString()} products with name {name}.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductsService::GetProducts. Exception: {ex.Message}", ex);
            }

            return new OkObjectResult(result.Items);
        }

        public async Task<ActionResult<ProductDTO>> GetProductById(Guid id)
        {
            Product product = null;

            if (id == Guid.Empty)
            {
                _logger.LogError("ProductsService::GetProductById. Error: id must be a valid Guid.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "id must be a valid Guid" });
            }

            try
            {
                product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    _logger.LogInformation($"ProductsService::GetProductById. No product with id {id} was found.");
                    return new NotFoundResult();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductsService::GetProductById. Exception: {ex.Message}", ex);
            }

            _logger.LogInformation($"ProductsService::GetProductById. Successfully found product with id {id}.");
            return new OkObjectResult(ProductToDTO(product));
        }

        public async Task<IActionResult> UpdateProduct(Guid id, ProductRequest productRequest)
        {
            if (id == Guid.Empty || id == null)
            {
                _logger.LogError("ProductsService::UpdateProduct. Error: id must be a valid Guid.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "id must be a valid Guid." });
            }

            if (productRequest == null)
            {
                _logger.LogError("ProductsService::UpdateProduct. Error: no product to update was provided.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "product must be provided." });
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                _logger.LogError($"ProductsService::UpdateProduct. Error. Unable to find product with id {id}.");
                return new NotFoundObjectResult($"ProductsService::UpdateProduct. Error. Unable to find product with id {id}.");
            }

            product.Name = productRequest.Name;
            product.Description = productRequest.Description;
            product.Price = productRequest.Price;
            product.DeliveryPrice = productRequest.DeliveryPrice;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex) when (!ProductExists(id))
            {
                _logger.LogError($"ProductsService::UpdateProduct. Error. Unable to find product with id {id}. Exception: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductsService::UpdateProduct. Exception: {ex.Message}", ex);
            }

            return new NoContentResult();
        }

        public async Task<ActionResult<ProductDTO>> CreateProduct(ProductRequest productRequest)
        {
            if (productRequest == null)
            {
                _logger.LogError("ProductsService::CreateProduct. Error: no product to update was provided.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "product must be provided." });
            }

            var product = new Product()
            {
                Id = new Guid(),
                Name = productRequest.Name,
                Description = productRequest.Description,
                Price = productRequest.Price,
                DeliveryPrice = productRequest.DeliveryPrice
            };

            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductsService::CreateProduct. Exception: {ex.Message}", ex);
            }

            product = await _context.Products.FindAsync(product.Id);
            if (product == null)
            {
                _logger.LogError("ProductsService::CreateProduct. Error creating a new product.");
                return new NotFoundResult();
            }
            return new OkObjectResult(ProductToDTO(product));
        }

        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError("ProductsService::DeleteProduct. Error: id must be a valid Guid.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "id must be a valid Guid." });
            }

            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return new NotFoundResult();
            }

            if (product.ProductOptions != null && product.ProductOptions.Count > 0)
            {
                foreach (var option in product.ProductOptions)
                {
                    _context.ProductOptions.Remove(option);
                }
            }

            try
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductsService::DeleteProduct. Exception: {ex.Message}", ex);
            }

            return new NoContentResult();
        }

        public async Task<ActionResult<ProductOptionDTOListResult>> GetProductOptions(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError("ProductsService::GetProductOptions. Error: id must be a valid Guid.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "id must be a valid Guid." });
            }

            ProductOptionDTOListResult result = new ProductOptionDTOListResult();

            try
            {
                var product = await _context.Products.FindAsync(id);
                result.Items = await _context.ProductOptions.Where(po => po.ProductId == id).Select(po => ProductOptionToDTO(po)).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductsService::GetProductOptions. Exception: {ex.Message}", ex);
            }

            return new OkObjectResult(result.Items);
        }

        public async Task<ActionResult<ProductOptionDTO>> GetProductOption(Guid id, Guid optionId)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError("ProductsService::GetProductOptions. Error: id must be a valid Guid.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "id must be a valid Guid." });
            }

            if (optionId == Guid.Empty)
            {
                _logger.LogError("ProductsService::GetProductOptions. Error: id must be a valid Guid.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "option id must be a valid Guid." });
            }

            ProductOptionDTO productOption = null;

            try
            {
                productOption = _context.ProductOptions.Where(po => po.Id == optionId && po.ProductId == id).Select(po => ProductOptionToDTO(po)).FirstOrDefault();
                if (productOption == null)
                {
                    return new NotFoundObjectResult($"product option with id {optionId} was not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductsService::GetProductOption. Exception: {ex.Message}", ex);
            }

            return new OkObjectResult(productOption);
        }

        public async Task<IActionResult> UpdateProductOption(Guid id, Guid optionId, ProductOptionRequest productOptionRequest)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError("ProductsService::UpdateProductOption. Error: id must be a valid Guid.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "id must be a valid Guid." });
            }

            if (optionId == Guid.Empty)
            {
                _logger.LogError("ProductsService::UpdateProductOption. Error: id must be a valid Guid.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "option id must be a valid Guid." });
            }

            if (productOptionRequest == null)
            {
                _logger.LogError("ProductsService::UpdateProductOption. Error: no product option to update was provided.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "product option must be provided." });
            }

            //if (id != productOptionDTO.ProductId)
            //{
            //    _logger.LogError("ProductsService::UpdateProductOption. Error: product id provided does not match ProductId of the option to be updated.");
            //    return new BadRequestResult();
            //}

            //if (optionId != productOptionDTO.Id)
            //{
            //    _logger.LogError("ProductsService::UpdateProductOption. Error: option id provided does not match id of the option to be updated.");
            //    return new BadRequestResult();
            //}

            var productOption = await _context.ProductOptions.FindAsync(optionId);
            if (productOption == null)
            {
                return new NotFoundResult();
            }
            if (id != productOption.ProductId)
            {
                _logger.LogError("ProductsService::UpdateProductOption. Error: product id provided does not match ProductId of the option to be updated.");
                return new BadRequestResult();
            }

            productOption.Name = productOptionRequest.Name;
            productOption.Description = productOptionRequest.Description;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex) when (!ProductOptionExists(id))
            {
                _logger.LogError($"ProductsService::UpdateProduct. Error. Unable to find product with id {id}. Exception: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductsService::UpdateProduct. Exception: {ex.Message}", ex);
            }

            return new NoContentResult();
        }

        public async Task<ActionResult<ProductOptionDTO>> CreateProductOption(Guid productId, ProductOptionRequest productOptionRequest)
        {
            if (productId == Guid.Empty)
            {
                _logger.LogError("ProductsService::CreateProductOption. Error: product id must be a valid Guid.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "product id must be a valid Guid." });
            }

            if (productOptionRequest == null)
            {
                _logger.LogError("ProductsService::CreateProductOption. Error: no product option to update was provided.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "product option must be provided." });
            }

            //if (productId != productOptionRequest.ProductId)
            //{
            //    _logger.LogError("ProductsService::CreateProductOption. Error: product id provided does not match ProductId of the option to be updated.");
            //    return new BadRequestResult();
            //}

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return new NotFoundResult();
            }

            var productOption = new ProductOption()
            {
                Id = new Guid(),
                ProductId = productId,
                Name = productOptionRequest.Name,
                Description = productOptionRequest.Description
            };

            try
            {
                _context.ProductOptions.Add(productOption);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductsService::CreateProductOption. Exception: {ex.Message}", ex);
            }

            productOption = await _context.ProductOptions.FindAsync(productOption.Id);
            if (productOption == null)
            {
                _logger.LogError("ProductsService::CreateProductOption. Error creating a new product.");
                return new NotFoundResult();
            }

            return new OkObjectResult(ProductOptionToDTO(productOption));
        }

        public async Task<ActionResult> DeleteProductOption(Guid id, Guid optionId)
        {
            if (id == Guid.Empty)
            {
                _logger.LogError("ProductsService::DeleteProductOption. Error: id must be a valid Guid.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "id must be a valid Guid." });
            }

            if (optionId == Guid.Empty)
            {
                _logger.LogError("ProductsService::DeleteProductOption. Error: id must be a valid Guid.");
                return new BadRequestObjectResult(new { statuscode = 400, message = "option id must be a valid Guid." });
            }

            var productOption = await _context.ProductOptions.FindAsync(optionId);
            if (productOption == null)
            {
                return new NotFoundResult();
            }

            if (id != productOption.ProductId)
            {
                _logger.LogError("ProductsService::DeleteProductOption. Error: product id of the option must match the product this option belongs to..");
                return new BadRequestObjectResult(new { statuscode = 400, message = "product id of the option must match the product this option belongs to." });
            }

            try
            {
                _context.ProductOptions.Remove(productOption);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"ProductsService::DeleteProductOption. Exception: {ex.Message}", ex);
            }
            return new NoContentResult();
        }

        private bool ProductExists(Guid id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        private bool ProductOptionExists(Guid id)
        {
            return _context.ProductOptions.Any(e => e.Id == id);
        }

        private static ProductDTO ProductToDTO(Product product) =>
        new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            DeliveryPrice = product.DeliveryPrice
        };

        private static ProductOptionDTO ProductOptionToDTO(ProductOption productOption) =>
        new ProductOptionDTO
        {
            Id = productOption.Id,
            ProductId = productOption.ProductId,
            Name = productOption.Name,
            Description = productOption.Description
        };
    }
}