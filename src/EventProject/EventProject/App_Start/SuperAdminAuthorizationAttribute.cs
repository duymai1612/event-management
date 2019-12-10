using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventProject.App_Start
{
    public class SuperAdminAuthorizationAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["Role"] != null &&
                filterContext.HttpContext.Session["Role"].ToString() != "SAdmin")
            {
                filterContext.Result = new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden, "Forbidden");
            }
        }
    }
}