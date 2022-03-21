using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MarketPlace.Web.Areas.Seller.Controllers
{
    [Authorize]
    [Area("Seller")]
    [Route("seller")]
    public class SellerBaseController : Controller
    {
        protected string ErrorMessage = "ErrorMessage";
        protected string InfoMessage = "InfoMessage";
        protected string SuccessMessage = "SuccessMessage";
        protected string WarningMessage = "WarningMessage";
    }
}
