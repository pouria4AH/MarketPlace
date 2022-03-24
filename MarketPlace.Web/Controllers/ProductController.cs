using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.DataLayer.DTOs.Product;

namespace MarketPlace.Web.Controllers
{
    public class ProductController : SiteBaseController
    {
        #region ctor

        private readonly IProductService _productService ;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region filter product
        [HttpGet("products")]
        public async Task<IActionResult> FilterProducts(FilterProductDTO filter)
        {
            var products = await _productService.FilterProducts(filter);
            return View(products);
        }
        #endregion

    }
}
