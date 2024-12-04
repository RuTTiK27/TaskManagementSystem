using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TaskManagementSystem.Filters
{
    public class AuthorizeSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context) 
        {
            var session = context.HttpContext.Session.GetString("UserDetails");
            if (session == null)
            {
                context.Result = new RedirectToActionResult("Login","User",null);
            }
            base.OnActionExecuting(context);
        }
    }
}
