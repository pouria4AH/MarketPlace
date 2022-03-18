using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.DataLayer.Common;
using MarketPlace.DataLayer.DTOs.Seller;
using MarketPlace.Web.Http;

namespace MarketPlace.Web.Areas.Admin.Controllers
{
    public class SellerController : AdminBaseController
    {
        #region ctor

        private readonly ISellerService _sellerService;
        public SellerController(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        #endregion

        #region Seller requset

        public async Task<IActionResult> SellerRequests(FilterSellerDTO filter)
        {
            filter.TakeEntities = 5;
            return View(await _sellerService.FilterSellers(filter));
        }

        #endregion

        #region acceot seller requests

        public async Task<IActionResult> AcceptSellerRequests(long requestsId)
        {
            var result = await _sellerService.AcceptSellerRequests(requestsId);
            if (result)
            {
                return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success,
                    "در خواست شما با موفقیت ثبت شد",
                    null);
            }
            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger,
                "اطلاعات مورد نظر یافت نشد",
                null);
        }
        public async Task<IActionResult> RejectSellerRequests(RejectItemDTO reject)
        {
            var result = await _sellerService.RejectSellerRequests(reject);
            if (result)
            {
                return JsonResponseStatus.SendStatus(JsonResponseStatusType.Success,
                    "در خواست شما با موفقیت ثبت شد",
                    reject);
            }
            return JsonResponseStatus.SendStatus(JsonResponseStatusType.Danger,
                "اطلاعات مورد نظر یافت نشد",
                null);
        }
    }

    #endregion
}

