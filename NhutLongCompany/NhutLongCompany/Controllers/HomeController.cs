using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NhutLongCompany.Models;

namespace NhutLongCompany.Controllers
{
    public class HomeController : Controller
    {
        private NhutLongCompanyEntities db=new NhutLongCompanyEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FlotCharts()
        {
            return View("FlotCharts");
        }

        public ActionResult MorrisCharts()
        {
            return View("MorrisCharts");
        }

        public ActionResult Tables()
        {
            return View("Tables");
        }

        public ActionResult Forms()
        {
            return View("Forms");
        }

        public ActionResult Panels()
        {
            return View("Panels");
        }

        public ActionResult Buttons()
        {
            return View("Buttons");
        }

        public ActionResult Notifications()
        {
            return View("Notifications");
        }

        public ActionResult Typography()
        {
            return View("Typography");
        }

        public ActionResult Icons()
        {
            return View("Icons");
        }

        public ActionResult Grid()
        {
            return View("Grid");
        }

        public ActionResult Blank()
        {
            return View("Blank");
        }

        public ActionResult Login()
        {
            return View("Login");
        }

        public ActionResult Home()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var qr = (from data in db.tbl_Customers select data).ToList().Count();
            ViewBag.count = qr;
            return View();
        }
        public ActionResult HomeBaoGia()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            
            return View();
        }
        public ActionResult HomeSanXuat()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return View();
        }
        public ActionResult HomeSanPham()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return View();
        }
        public ActionResult HomeCustomers()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            return View();
        }
    }
}