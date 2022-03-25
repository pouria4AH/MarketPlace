using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.DataLayer.DTOs.Product;

namespace MarketPlace.Web.Controllers
{
    public class ProductController : SiteBaseController
    {
        #region ctor

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region filter product
        [HttpGet("products")]
        [HttpGet("products/{Category}")]
        public async Task<IActionResult> FilterProducts(FilterProductDTO filter)
        {
            filter.TakeEntities = 9;
            filter.FilterProductState = FilterProductState.All;
            filter = await _productService.FilterProducts(filter);
            ViewBag.productCategorySelected = await _productService.GetAllActiveProductCategories();
            if (filter.PageId > filter.GetLastPage() && filter.GetLastPage() != 0) return NotFound();
            return View(filter);
        }
        #endregion

        #region product detail
        [HttpGet("product-detail/{productId}/{title}")]
        public async Task<IActionResult> ProductDetail(long productId, string title)
        {
            var product = await _productService.GetProductDetailBy(productId);
            if (product == null) return NotFound();
            return View(product);
        }

        #endregion
    }
}
