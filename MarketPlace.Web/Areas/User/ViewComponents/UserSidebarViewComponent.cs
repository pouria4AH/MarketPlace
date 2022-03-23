using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Areas.User.ViewComponents
{
    public class UserSidebarViewComponent : ViewComponent
    {
        private ISellerService _sellerService;

        public UserSidebarViewComponent(ISellerService sellerService)
        {
            _sellerService = sellerService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.hasAnyActiveSellerPanel = await _sellerService.HasUserAnyActivePanel(User.GetUserId());
            return View("UserSidebar");
        }
    }
}
