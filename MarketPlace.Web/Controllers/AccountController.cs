using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.DataLayer.DTOs;

namespace MarketPlace.Web.Controllers
{
    public class AccountController : SiteBaseController
    {
        #region constractore

        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region regerster

        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost("register"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserDTO register)
        {
            if (ModelState.IsValid)
            {
                var res = await _userService.RegisterUser(register);
                switch (res)
                {
                    case RegisterUserResult.MobileExists:
                        TempData["ErrorMessage"] = "تلفن همراه وارد شده تکراری می باشد";
                        ModelState.AddModelError("Mobile", "تلفن همراه وارد شده تکراری می باشد");
                        break;
                    case RegisterUserResult.Error:
                        TempData["ErrorMessage"] = "مشکلی پیش امده است";
                        ModelState.AddModelError("Mobile", "مشکلی پیش امده است");
                        break;
                    case RegisterUserResult.Success:
                        TempData["SuccessMessage"] = "ثبت نام شما با موفقیت انجام شد";
                        TempData["InfoMessage"] = "کد تایید تلفن همراه برای شما ارسال شد";
                        return RedirectToAction("Login")
                }
            }

            return View(register);
        }

        #endregion

        #region Login

        public IActionResult Login()
        {
            return View();
        }

        #endregion


    }
}
