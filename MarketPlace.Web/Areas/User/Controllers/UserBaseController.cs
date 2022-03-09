﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MarketPlace.Web.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    [Route("user")]
    public class UserBaseController : Controller
    {
       
    }
}
