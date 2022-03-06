using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Web.Controllers
{
    public class HomeController : SiteBaseController
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }


    }
}
