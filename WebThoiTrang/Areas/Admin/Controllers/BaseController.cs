using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using WebThoiTrang.Models;

namespace WebThoiTrang.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        private KDTShop db = new KDTShop();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = Session["MaTK"];
            TaiKhoan u;
            try
            {
                u = db.TaiKhoan.Find(session);
            }
            catch (Exception e)
            {
                u = new TaiKhoan();
            }


            if (session == null || u.VaiTro != "Administrator")
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Home", action = "Login", Area = "" }));
            }
            base.OnActionExecuting(filterContext);
        }
    }
}