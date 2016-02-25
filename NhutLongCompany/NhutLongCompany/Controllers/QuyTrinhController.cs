using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhutLongCompany.Controllers
{
    public class QuyTrinhController : Controller
    {
        // GET: QuyTrinh
        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }
    }
}