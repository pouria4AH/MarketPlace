﻿using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.ViewComponents
{

    #region SiteHeder
    public class SiteHeaderViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;
        public SiteHeaderViewComponent(ISiteService siteService, IUserService userService, IProductService productService)
        {
            _siteService = siteService;
            _userService = userService;
            _productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.siteSetting = await _siteService.GetDefaultSiteSetting();
            ViewBag.user = null;
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.user = await _userService.GetUserByMobile(User.Identity.Name);
            }

            ViewBag.ProductCategories = await _productService.GetAllActiveProductCategories();
            return View("SiteHeader");
        }
    }
    #endregion

    #region SiteFooter
    public class SiteFooterViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;

        public SiteFooterViewComponent(ISiteService siteService)
        {
            _siteService = siteService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.siteSetting = await _siteService.GetDefaultSiteSetting();
            return View("SiteFooter");
        }
    }
    #endregion

    #region home slider

    public class HomeSliderViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;

        public HomeSliderViewComponent(ISiteService siteService)
        {
            _siteService = siteService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var slider = await _siteService.GetAllActiveSlide();
            return View("HomeSlider", slider);
        }
    }

    #endregion

}
