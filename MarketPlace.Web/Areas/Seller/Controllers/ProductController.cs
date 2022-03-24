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
        #region list
        [HttpGet("product-galleries/{id}")]
        public async Task<IActionResult> GetProductGalleries(long id)
        {
            ViewBag.productId = id;
            var seller = await _sellerService.GetLastActiveSellerByUser(User.GetUserId());
            return View(await _productService.GetAllProductGalleryForSeller(id, seller.Id));
        }
        #endregion
        #region create
        [HttpGet("create-product-galleries/{productId}")]
        public async Task<IActionResult> CreateProductGallery(long productId)
        {
            var product = await _productService.GetProductBySellerOwnerId(productId, User.GetUserId());
            if (product == null) return NotFound();
            ViewBag.product = product;
            return View();
        }


        [HttpPost("create-product-galleries/{productId}")]
        public async Task<IActionResult> CreateProductGallery(long productId, CreateOurEditProductGalleryDTO createOurEdit)
        {
            if (ModelState.IsValid)
            {
                var seller = await _sellerService.GetLastActiveSellerByUser(User.GetUserId());
                var res = await _productService.CreateProductGallery(createOurEdit, productId, seller.Id);
                switch (res)
                {
                    case CreateOurEditProductGalleryResult.ImageIsNull:
                        TempData[WarningMessage] = "لطفا عکس را درست وارد کنید";
                        break;
                    case CreateOurEditProductGalleryResult.ProductNotFound:
                        TempData[ErrorMessage] = "محصولی با این مشخصات یافت نشد";
                        break;
                    case CreateOurEditProductGalleryResult.NotForUserProduct:
                        TempData[WarningMessage] = "محصول مورد نظر در لیست محصولات شما وجود ندارد";
                        break;
                    case CreateOurEditProductGalleryResult.Success:
                        TempData[SuccessMessage] = "عملیات با موفقیت انجام شد";
                        return RedirectToAction("GetProductGalleries", "Product", new { id = productId });
                }
            }
            return View();
        }
        #endregion
        #region edit

        [HttpGet("product_{productId}/edit-gallery/{galleryId}")]
        public async Task<IActionResult> EditGallery(long galleryId, long productId)
        {
            var seller = await _sellerService.GetLastActiveSellerByUser(User.GetUserId());
            var mainGallery = await _productService.GetProductGalleryFourEdit(galleryId, seller.Id);
            if (mainGallery == null) return NotFound();
            return View(mainGallery);
        }
        [HttpPost("product_{productId}/edit-gallery/{galleryId}")]
        public async Task<IActionResult> EditGallery(CreateOurEditProductGalleryDTO gallery, long galleryId, long productId)
        {
            if (ModelState.IsValid)
            {
                var seller = await _sellerService.GetLastActiveSellerByUser(User.GetUserId());
                var res = await _productService.EditProductGallery(galleryId, seller.Id, gallery);
                switch (res)
                {
                    case CreateOurEditProductGalleryResult.NotForUserProduct:
                        TempData[WarningMessage] = "درخواست داده شده برای شما مجاز نمی باشد";
                        break;
                    case CreateOurEditProductGalleryResult.ProductNotFound:
                        TempData[ErrorMessage] = "عکسی با مشخصات زیر یافت نشد";
                        break;
                    case CreateOurEditProductGalleryResult.Success:
                        TempData[SuccessMessage] = "عملیات با موفیت انجام شد";
                        return RedirectToAction("GetProductGalleries", "Product", new { id = productId });
                }
            }
            return View();
        }

        #endregion
        #endregion
    }
}
