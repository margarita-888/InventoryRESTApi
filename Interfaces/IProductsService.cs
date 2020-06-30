using Microsoft.AspNetCore.Mvc;
using ProductsRESTApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductsRESTApi.Interfaces
{
    public interface IProductsService
    {
        public Task<ActionResult<ProductListResult>> GetProducts(string name = "");

        public Task<ActionResult<ProductDTO>> GetProductById(Guid id);

        public Task<IActionResult> UpdateProduct(Guid id, ProductRequest productRequest);

        public Task<ActionResult<ProductDTO>> CreateProduct(ProductRequest productCreate);

        public Task<ActionResult> DeleteProduct(Guid id);

        public Task<ActionResult<ProductOptionDTOListResult>> GetProductOptions(Guid id);

        public Task<ActionResult<ProductOptionDTO>> GetProductOption(Guid id, Guid optionId);

        public Task<IActionResult> UpdateProductOption(Guid id, Guid optionId, ProductOptionRequest productOptionRequest);

        public Task<ActionResult<ProductOptionDTO>> CreateProductOption(Guid id, ProductOptionRequest productOptionRequest);

        public Task<ActionResult> DeleteProductOption(Guid id, Guid optionId);
    }
}