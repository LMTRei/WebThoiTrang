using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebThoiTrang.Areas.Admin.Controllers
{
    public class ErrorAdminController : Controller
    {
        // GET: Admin/ErrorAdmin
        public ActionResult NotFoundPage()
        {
            return View();
        }

        public ActionResult ServerError()
        {
            return View();
        }
    }
}