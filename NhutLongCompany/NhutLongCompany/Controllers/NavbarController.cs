using NhutLongCompany.Domain;
using NhutLongCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NhutLongCompany.Controllers
{
    public class NavbarController : Controller
    {
        // GET: Navbar
        public ActionResult Index()
        {
            var data = new Data();

            ViewBag.ten = Session["username"];
            return PartialView("_Navbar", data.navbarItems().ToList());
        }
    }
}