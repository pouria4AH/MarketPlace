﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using GoogleReCaptcha.V3.Interface;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.DataLayer.DTOs.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MarketPlace.Web.Controllers
{
    public class AccountController : SiteBaseController
    {
        #region constractore

        private readonly IUserService _userService;
        private readonly ICaptchaValidator _captchaValidator;
        public AccountController(IUserService userService, ICaptchaValidator captchaValidator)
        {
            _userService = userService;
            _captchaValidator = captchaValidator;
        }

        #endregion

        #region regerster

        [HttpGet("register")]
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            return View();
        }
        [HttpPost("register"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserDTO register)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(register.Captcha))
            {
                TempData[ErrorMessage] = "کد کپجای شما تایید نشد";
                return View("Login");
            }
            if (ModelState.IsValid)
            {
                var res = await _userService.RegisterUser(register);
                switch (res)
                {
                    case RegisterUserResult.MobileExists:
                        TempData[ErrorMessage] = "تلفن همراه وارد شده تکراری می باشد";
                        ModelState.AddModelError("Mobile", "تلفن همراه وارد شده تکراری می باشد");
                        break;

                    #region coment

                    //case RegisterUserResult.Error:
                    //    TempData[ErrorMessage] = "مشکلی پیش امده است";
                    //    ModelState.AddModelError("Mobile", "مشکلی پیش امده است");
                    //    break;

                    #endregion
                    case RegisterUserResult.Success:
                        TempData[SuccessMessage] = "ثبت نام شما با موفقیت انجام شد";
                        TempData[InfoMessage] = "کد تایید تلفن همراه برای شما ارسال شد";
                        return RedirectToAction("ActivateMobile", "Account", new { mobile = register.Mobile });
                }
            }

            return View(register);
        }

        #endregion

        #region ActiveteMobile

        [HttpGet("activate-mobile/{mobile}")]
        public IActionResult ActivateMobile(string mobile)
        {
            if (User.Identity.IsAuthenticated) return Redirect("/");
            var activeDTO = new ActivateMobileDTO { Mobile = mobile };
            return View(activeDTO);
        }
        [HttpPost("activate-mobile/{mobile}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivateMobile(ActivateMobileDTO activate)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(activate.Captcha))
            {
                TempData[ErrorMessage] = "کد کپجای شما تایید نشد";
                return View("Login");
            }
            if (ModelState.IsValid)
            {
                var res = await _userService.ActiveMobile(activate);
                if (res)
                {
                    TempData[SuccessMessage] = "حساب فعال شد";
                    return RedirectToAction("Login");
                }

                TempData[ErrorMessage] = "کاربری یافت نشد";
            }

            return View(activate);
        }


        #endregion

        #region Login

        [HttpGet("login")]
        public IActionResult Login()
        {

            if (User.Identity.IsAuthenticated) return Redirect("/");
            return View();
        }
        [HttpPost("login"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserDTO login)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(login.Captcha))
            {
                TempData[ErrorMessage] = "کد کپجای شما تایید نشد";
                return View("Login");
            }
            if (ModelState.IsValid)
            {
                var res = await _userService.GetUserForLogin(login);
                switch (res)
                {
                    case LoginUserResult.NotFound:
                        TempData[ErrorMessage] = "کاربر پیدا نشد";
                        break;
                    case LoginUserResult.NotActivated:
                        TempData[WarningMessage] = "حساب کاربری اکتیو نشده است";
                        break;
                    case LoginUserResult.Success:

                        var user = await _userService.GetUserByMobile(login.Mobile);

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Mobile),
                            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())

                        };
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        var properties = new AuthenticationProperties
                        {
                            IsPersistent = login.RememberMe
                        };
                        await HttpContext.SignInAsync(principal, properties);
                        TempData[SuccessMessage] = "عملیات موفق بود";
                        return Redirect("/");
                }
            }
            return View();
        }

        #endregion

        #region log out

        [HttpGet("log-out")]
        public async Task<IActionResult> LogOut()
        {

            await HttpContext.SignOutAsync();
            return Redirect("./");
        }
        #endregion

        #region forgot pass
        [HttpGet("forgot-pass")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost("forgot-pass"), ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPassUserDTO forgot)
        {
            if (!await _captchaValidator.IsCaptchaPassedAsync(forgot.Captcha))
            {
                TempData[ErrorMessage] = "کد کپجای شما تایید نشد";
                return View(forgot);
            }

            if (ModelState.IsValid)
            {
                var res = await _userService.RecoverUserPassword(forgot);
                switch (res)
                {
                    case ForgotPassUserResult.NotFound:
                        TempData[ErrorMessage] = "کاربری با این شماره وجود ندارد";
                        break;
                    case ForgotPassUserResult.Success:
                        TempData[SuccessMessage] = "عملیات موقق امیز بود";
                        TempData[InfoMessage] = "لطفا پسورد خود بعد از ورود تغیر دهید";
                        return RedirectToAction("Login");
                }

            }

            return View(forgot);
        }

        #endregion
    }
}
