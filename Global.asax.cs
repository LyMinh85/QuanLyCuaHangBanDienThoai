using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using static System.Collections.Specialized.BitVector32;

namespace QuanLyCuaHangBanDienThoai
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
        }
    }
    public class RequestFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            string controllerName = actionContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = actionContext.ActionDescriptor.ActionName;

            if (actionContext.HttpContext.Session["userId"] == null)
            {

                if (!actionName.Equals("login", StringComparison.InvariantCultureIgnoreCase))
                {
                    actionContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { action = "Login", controller = "User" }));
                }
            } else
            {
                var quyenHan = actionContext.HttpContext.Session["role"];
                if (!quyenHan.Equals("admin"))
                {
                    System.Diagnostics.Debug.WriteLine(controllerName);
                    if (controllerName.Equals("NhanVien") || controllerName.Equals("NhaCungCap")
                        || controllerName.Equals("KhoHang"))
                    {
                        actionContext.Result = new RedirectToRouteResult(
                            new RouteValueDictionary(new {
                                action = "Index", controller = "Home"
                        }));
                    }
                }
            }

        }
    }
}
