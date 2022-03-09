using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MarketPlace.DataLayer.DTOs.Account;

namespace MarketPlace.Web.Areas.User.Controllers
{
    public class AccountController : UserBaseController
    {
        #region constructor



        #endregion

        #region change passworld
        [HttpGet("change-password")]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }
        [HttpPost("change-password"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDto)
        {
            return View();
        }
        #endregion

    }
}
