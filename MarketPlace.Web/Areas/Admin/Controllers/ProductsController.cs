using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.DataLayer.Common;
using MarketPlace.DataLayer.DTOs.Product;
using MarketPlace.Web.Http;

namespace MarketPlace.Web.Areas.Admin.Controllers
{
    public class ProductsController : AdminBaseController
    {
        #region ctor

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        #endregion

        #region product list
        public async Task<IActionResult> Index(FilterProductDTO filter)
        {
            return View(await _productService.FilterProducts(filter));
        }
        #endregion

        #region Accpet product

        public async Task<IActionResult> AcceptSellerProduct(long id)
        {
            var res = await _productService.AcceptedSellerProduct(id);
            if (res)
            {
                return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, "عمیات با موفقیت انجام شد", null);
            }
            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger, "رکوردی پیدا نشد", null);
        }
        #endregion

        #region reject product

        public async Task<IActionResult> RejectSellerProduct(RejectItemDTO reject)
        {
            if (ModelState.IsValid)
            {
                var res = await _productService.RejectSellerProduct(reject);
                if (res)
                {
                    return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success, "عمیات رد شدن با موفقیت انجام شد",reject);
                }
                return JsonResponseStatus.SendStatus(JsonResponseStatusType.Warning, "لطفا اطلاعات را درست وارد کنید", null);
            }
            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger, "رکوردی پیدا نشد", null);
        }

        #endregion
    }
}
