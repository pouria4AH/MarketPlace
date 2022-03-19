using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.DataLayer.DTOs.Product;
using MarketPlace.Web.PresentationExtensions;
using Microsoft.AspNetCore.Http;

namespace MarketPlace.Web.Areas.Seller.Controllers
{
    public class ProductController : SellerBaseController
    {
        #region ctor

        private readonly IProductService _productService;
        private readonly ISellerService _sellerService;
        public ProductController(IProductService productService, ISellerService sellerService)
        {
            _productService = productService;
            _sellerService = sellerService;
        }

        #endregion

        #region list

        [HttpGet("products")]
        public async Task<IActionResult> Index(FilterProductDTO filter)
        {
            var seller = await _sellerService.GetLastActiveSellerByUser(User.GetUserId());
            filter.SellerId = seller.Id;
            filter.FilterProductState = FilterProductState.All;
            return View(await _productService.FilterProducts(filter));
        }

        #endregion

        #region create product
        [HttpGet("create-product")]
        public async Task<IActionResult> CreateProduct()
        {
            ViewBag.Categories = await _productService.GetAllActiveProductCategories();
            return View();
        }

        [HttpPost("create-product"), ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(CreateProductDTO product, IFormFile productImag)
        {
            if (ModelState.IsValid)
            {

            }

            ViewBag.Categories = await _productService.GetAllActiveProductCategories();
            return View(product);
        }
        #endregion
    }
}
