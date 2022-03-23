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

        [HttpGet("products-list")]
        public async Task<IActionResult> Index(FilterProductDTO filter)
        {
            var seller = await _sellerService.GetLastActiveSellerByUser(User.GetUserId());
            filter.SellerId = seller.Id;
            filter.FilterProductState = FilterProductState.All;
            filter.TakeEntities = 1000;
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
        public async Task<IActionResult> CreateProduct(CreateProductDTO product, IFormFile productImage)
        {
            //var errors = ModelState
            //    .Where(x => x.Value.Errors.Count > 0)
            //    .Select(x => new { x.Key, x.Value.Errors })
            //    .ToArray();


            if (ModelState.IsValid)
            {
                var seller = await _sellerService.GetLastActiveSellerByUser(User.GetUserId());

                var res = await _productService.CreateProduct(product, seller.Id, productImage);
                switch (res)
                {
                    case CreateProductResult.IsNotImage:
                        TempData[WarningMessage] = "لطفا عکس را وارد کنبد";
                        break;
                    case CreateProductResult.Error:
                        TempData[ErrorMessage] = "عملیات با خطا مواجه شد";
                        break;
                    case CreateProductResult.Success:
                        TempData[SuccessMessage] = "عملیات با موفقیت انجام شد";
                        return RedirectToAction("Index");
                }
            }

            ViewBag.Categories = await _productService.GetAllActiveProductCategories();
            return View(product);
        }
        #endregion

        #region edit product
        [HttpGet("edit-product/{productId}")]
        public async Task<IActionResult> EditProduct(long productId)
        {
            var product = await _productService.GetProductForEdit(productId);
            if (product == null) return NotFound();
            ViewBag.Categories = await _productService.GetAllActiveProductCategories();
            return View(product);
        }
        [HttpPost("edit-product/{productId}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(EditProductDTO product, IFormFile productImage)
        {
            if (ModelState.IsValid)
            {
                var res = await _productService.EditSellerProduct(product, User.GetUserId(), productImage);
                switch (res)
                {
                    case EditProductResult.NotForUser:
                        TempData[WarningMessage] = "این محصول برای شما نیست در صورت تکرار دسترسی شما قطع خواهد شد";
                        break;
                    case EditProductResult.NotFound:
                        TempData[ErrorMessage] = "محصولی با این مشخصات یافت نشد";
                        break;
                    case EditProductResult.Success:
                        TempData[SuccessMessage] = "عملیات با موفقیت انجام شد";
                        return RedirectToAction("Index");
                }
            }
            ViewBag.Categories = await _productService.GetAllActiveProductCategories();
            return View(product);
        }
        #endregion

        #region product gallery

        public async Task<IActionResult> GetProductGalleries(long id)
        {
            return View(await _productService.GetAllProductGalleryForSeller(id, User.GetUserId()));
        }
        #endregion
    }
}
