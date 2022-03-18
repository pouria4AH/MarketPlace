using Microsoft.AspNetCore.Mvc;
using MarketPlace.Application.Services.interfaces;

namespace MarketPlace.Web.Areas.Seller.Controllers
{
    public class ProductController : SellerBaseController
    {
        #region ctor

        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion
        //[HttpGet("")]
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
