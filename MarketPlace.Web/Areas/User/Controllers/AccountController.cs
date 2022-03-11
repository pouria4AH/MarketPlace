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

        #region edit profile
        [HttpGet("edit-profile")]
        public async Task<IActionResult> EditProfile()
        {
            var userProfile = await _userService.GetProfileForEdit(User.GetUserId());
            if (userProfile == null) return NotFound();
            return View(userProfile);
        }
        [HttpPost("edit-profile"), ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileDTO profile)
        {
            var res = await _userService.EditUserProfile(profile, User.GetUserId());
            if (ModelState.IsValid)
            {
                switch (res)
                {
                    case EditUserProfileResult.IsBlocked:
                        TempData[ErrorMessage] = "شما مسدود شده اید";
                        break;
                    case EditUserProfileResult.IsNotActive:
                        TempData[ErrorMessage] = "حساب کاربری شما فعال نشده";
                        break;
                    case EditUserProfileResult.NotFound:
                        TempData[ErrorMessage] = "کاربری با مشخصات زیر یافت نشد";
                        break;
                    case EditUserProfileResult.Success:
                        TempData[SuccessMessage] = "عملیات با موفقعیت انجام شد";
                        break;
                }
            }
            return View(profile);
        }

        #endregion
    }
}
