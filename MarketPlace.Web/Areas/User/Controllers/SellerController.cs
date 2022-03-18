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

        #region request

        [HttpGet("request-seller-panel")]
        public IActionResult RequestSellerPanel()
        {
            return View();
        }
        [HttpPost("request-seller-panel"), ValidateAntiForgeryToken]
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
                        return RedirectToAction("SellerRequests");
                        break;
                }
            }
            return View(seller);
        }

        #endregion

        #region Sellers request 
        [HttpGet("seller-requests")]
        public async Task<IActionResult> SellerRequests(FilterSellerDTO filter)
        {
            filter.TakeEntities = 5;
            filter.UserId = User.GetUserId();
            filter.State = FilterSellerState.All;
            return View(await _sellerService.FilterSellers(filter));
        }
        #endregion


        #region edit request

        [HttpGet("edit-request-seller/{id}")]
        public async Task<IActionResult> EditRequestSeller(long id)
        {
            var requestSeller = await _sellerService.GetRequestSellerForEdit(id, User.GetUserId());
            if (requestSeller == null) return NotFound();
            return View(requestSeller);
        }
        [HttpPost("edit-request-seller/{id}")]
        public async Task<IActionResult> EditRequestSeller(EditRequestSellerDTO request)
        {
            if (ModelState.IsValid)
            {
                var res = await _sellerService.EditRequestsSeller(request, User.GetUserId());
                switch (res)
                {
                    case EditRequestResult.NotFound:
                        TempData[ErrorMessage] = "اطلاعات مورد نظر یافت نشد";
                        break;
                    case EditRequestResult.Success:
                        TempData[SuccessMessage] = "عملیات با موفقیت انجام شد";
                        TempData[WarningMessage] = "درخواست در حال برسی است";
                        return RedirectToAction("SellerRequests");
                }
            }
            return View(request);
        }

        #endregion
    }
}
