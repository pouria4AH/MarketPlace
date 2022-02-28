﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.DataLayer.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

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
            if (User.Identity.IsAuthenticated) return Redirect("/");
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
                        return RedirectToAction("Login");
                }
            }

            return View(register);
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


    }
}
