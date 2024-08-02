using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

using Microsoft.AspNetCore.Http.Extensions;
using WebApplication1.Helper.Service;

namespace WebApplication1.Helper
{
    public class AutorizacijaAtribut : TypeFilterAttribute
    {
        public AutorizacijaAtribut(): base(typeof(MyAuthorizeImpl))
        {
            //Arguments = new object[] { };
        }
    }


    public class MyAuthorizeImpl : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var authService = context.HttpContext.RequestServices.GetService<MyAuthService>()!;
            // var actionLogService = context.HttpContext.RequestServices.GetService<MyActionLogService>()!;

            if (!authService.isLogiran())
            {
                context.Result = new UnauthorizedObjectResult("niste logirani na sistem");
                return;
            }
            else if (!authService.isAdmin())
            {
                context.Result = new UnauthorizedObjectResult("niste administrator");
                return;
            }
            MyAuthInfo myAuthInfo = authService.GetAuthInfo();
          
            await next();
           // await actionLogService.Create(context.HttpContext);
        }
    }
}
