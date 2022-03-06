﻿using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.ViewComponents
{

    #region SiteHeder
    public class SiteHeaderViewComponent : ViewComponent
    {
        private readonly ISiteService _siteService;

        public SiteHeaderViewComponent(ISiteService siteService)
        {
            _siteService = siteService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.siteSetting = await _siteService.GetDefaultSiteSetting();
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
