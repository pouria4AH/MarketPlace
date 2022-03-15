using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.DataLayer.DTOs.Seller;
using MarketPlace.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Areas.User.Controllers
{
    public class SellerController : UserBaseController
    {
        #region ctor

        private readonly ISellerService _sellerService;

        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        #endregion

        [HttpGet("request-seller-panel")]
        public IActionResult RequestSellerPanel()
        {
            return View();
        }
        [HttpPost("request-seller-panel")]
        public async Task<IActionResult> RequestSellerPanel(RequestSellerDTO seller)
        {
            var res = await _sellerService.AddNewSellerRequest(seller, User.GetUserId());
            if (ModelState.IsValid)
            {
                switch (res)
                {
                    case RequestSellerResult.HasNotPermission:
                        TempData[ErrorMessage] = "شما مجاز به این کار نیستید";
                        break;
                    case RequestSellerResult.HasUnderProgressRequest:
                        TempData[WarningMessage] = "در حال حاضر دخواست دارید";
                        TempData[InfoMessage] = "فعلا نمیتوانید در خواست دهید";
                        break;
                    case RequestSellerResult.Success:
                        TempData[SuccessMessage] = "عملیات با موفقیت انجام شد";
                        TempData[InfoMessage] = "درخواست در حال برسی است";
                        // todo: show list
                        break;
                }
            }
            return View(seller);
        }
    }
}
