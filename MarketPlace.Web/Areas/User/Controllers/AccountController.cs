using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.DataLayer.DTOs.Account;
using MarketPlace.Web.PresentationExtensions;
using Microsoft.AspNetCore.Authentication;

namespace MarketPlace.Web.Areas.User.Controllers
{
    public class AccountController : UserBaseController
    {
        #region constructor

        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

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
            if (ModelState.IsValid)
            {
                var res = await _userService.ChangePassword(changePasswordDto, User.GetUserId());
                if (res)
                {
                    TempData[SuccessMessage] = "کلمه عبور با موفقیت تغیر کرد";
                    TempData[InfoMessage] = "لطفا دوباره وارد سایت شوید";
                    await HttpContext.SignOutAsync();
                    return RedirectToAction("Login", "Account", new { area = "" });
                }
                else
                {
                    TempData[WarningMessage] = "رمز جدیدی وارد کنید";
                    ModelState.AddModelError("NewPassword", "پسورد جدیدی وارد کنید");
                }
            }
            return View();
        }
        #endregion

    }
}
