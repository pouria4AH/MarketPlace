using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.DataLayer.DTOs.Product;

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
        public async Task<IActionResult> Index(FilterProductDTO filter)
        {
            return View(await _productService.FilterProducts(filter));
        }
    }
}
