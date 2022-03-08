using System.Collections.Generic;
using System.Threading.Tasks;
using GoogleReCaptcha.V3.Interface;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.DataLayer.DTOs.Contact;
using MarketPlace.DataLayer.Entities.Site;
using MarketPlace.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Controllers
{
    public class HomeController : SiteBaseController
    {
        #region constractore

        private readonly IContactService _contactService;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly ISiteService _siteService;
        public HomeController(IContactService contactService, ICaptchaValidator captchaValidator, ISiteService siteService)
        {
            _contactService = contactService;
            _captchaValidator = captchaValidator;
            _siteService = siteService;
        }

        #endregion

        #region index
        public async Task<IActionResult> Index()
        {
            ViewBag.banners = await _siteService.GetSiteBannerByPlacement(new List<SiteBannerPlacement>
            {
                SiteBannerPlacement.Home_1,
                SiteBannerPlacement.Home_2,
                SiteBannerPlacement.Home_3
            });
            return View();
        }
        #endregion

        #region contact us
        [HttpGet("contact-us")]
        public async Task<IActionResult> ContactUs()
        {
            return View();
        }

         [HttpPost("contact-us"),ValidateAntiForgeryToken]
         public async Task<IActionResult> ContactUs(ContactUsDTO contact)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(contact.Captcha))
            {
                TempData[ErrorMessage] = "کد کپجای شما تایید نشد";
                return View(contact);
            }

            if (ModelState.IsValid)
            {
                var ip = HttpContext.GetUserIp();
                await _contactService.CreateContactUs(contact, ip, User.GetUserId());
                TempData[SuccessMessage] = "پیام ارسال شد";
                return RedirectToAction("ContactUs");
            }
            return View(contact);
        }

        #endregion

        #region about us
        [HttpGet("about-us")]
        public async Task<IActionResult> AboutUs()
        {
            var siteSetting = await _siteService.GetDefaultSiteSetting();
            return View(siteSetting);
        }
        #endregion
    }
}
