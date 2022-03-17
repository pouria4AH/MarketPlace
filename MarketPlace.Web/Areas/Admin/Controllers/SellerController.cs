using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.DataLayer.DTOs.Seller;

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
            return View(await _sellerService.FilterSellers(filter));
        }

        #endregion
    }
}
