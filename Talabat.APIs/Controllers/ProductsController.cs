using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talbat.Core;
using Talbat.Core.Entities;
using Talbat.Core.Repositories.Contract;
using Talbat.Core.Services.Contract;
using Talbat.Core.Specifications;
using Talbat.Core.Specifications.Product_Specs;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(
            IProductService productService,
            IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }
        [CashedAttribute]// Action filter to cache the response
        //[Authorize/*(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)*/]
        [HttpGet]        // /api/products
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecificationsParameters specParam)
        {
            var Products = await _productService.GetProductsAsync(specParam); // Get products with specifications

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(Products);

            var Count = await _productService.GetCountAsync(specParam); // Get total count of products

            return Ok(new Pagination<ProductToReturnDto>(specParam.pageIndex,specParam.PageSize,Count,data));
        }
        // /api/products/1
        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var product = await _productService.GetProductByIdAsync(id); // Get product by ID with specifications
            if (product == null)
                return NotFound(new ApiResponse(404)); // 404 Not Found

            return Ok(_mapper.Map<Product, ProductToReturnDto>(product)); // 200 OK
        }
        //[HttpGet("test-notfound")]
        //public async Task<ActionResult> GetNotFoundRequest()
        //{
        //    var product = await _productRepo.GetByIdAsync(1000); // Assuming 1000 is an ID that does not exist
        //    if (product == null)
        //        return NotFound(new ApiResponse(404)); // 404 Not Found
        //    return Ok(product); // 200 Ok
        //}
        [HttpGet("brands")]//Get: Api/api/products/brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            var brands = await _productService.GetBrandsAsync(); // Get all product brands
            return Ok(brands);
        }
        [HttpGet("categories")]//Get: Api/api/products/categories
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetProductCategories()
        {
            var categories = await _productService.GetCategoriesAsync(); // Get all product categories
            return Ok(categories);
        }
    }
}
