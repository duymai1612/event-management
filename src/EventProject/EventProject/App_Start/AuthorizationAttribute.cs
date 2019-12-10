using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventProject.App_Start
{
    public class AuthorizationAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["UserID"] == null ||
                filterContext.HttpContext.Session["Role"] == null)
            {
                filterContext.HttpContext.Response.Redirect("/Login/Index");
            }
        }
    }
}