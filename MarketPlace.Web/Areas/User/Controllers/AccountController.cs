using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MarketPlace.Web.Areas.User.Controllers
{
    public class AccountController : UserBaseController
    {
        #region constructor



        #endregion

        #region dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            return View();
        }
        #endregion

    }
}
