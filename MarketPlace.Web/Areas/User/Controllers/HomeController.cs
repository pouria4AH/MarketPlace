using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MarketPlace.DataLayer.DTOs.Account;

namespace MarketPlace.Web.Areas.User.Controllers
{
    public class HomeController : UserBaseController
    {
     
        #region dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> Dashboard()
        {
            return View();
        }
        #endregion
    }
}
