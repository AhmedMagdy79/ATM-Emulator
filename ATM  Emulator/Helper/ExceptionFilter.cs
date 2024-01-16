using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ATM__Emulator.Helper
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var jsonResult = new JsonResult(new
        {
            StatusCode = 500,
            Message = "Internal Server Error"
        });

        context.Result = jsonResult;
        context.HttpContext.Response.StatusCode = 500;
        context.ExceptionHandled = true;
        }
    }
}
