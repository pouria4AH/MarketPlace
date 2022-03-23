using MarketPlace.Application.Services.interfaces;
using MarketPlace.Web.PresentationExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MarketPlace.Web.Http
{
    public class CheckSellerStateAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private  ISellerService _sellerService;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
             _sellerService = (ISellerService)context.HttpContext.RequestServices.GetService(typeof(ISellerService));
             if (context.HttpContext.User.Identity.IsAuthenticated)
             {
                 var userId = context.HttpContext.User.GetUserId();
                 if (!_sellerService.HasUserAnyActivePanel(userId).Result)
                 {
                     context.Result = new RedirectResult("/user/dashboard");
                 }
             }
        }
    }
}
