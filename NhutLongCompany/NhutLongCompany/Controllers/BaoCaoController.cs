using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NhutLongCompany.Models;

namespace NhutLongCompany.Controllers
{
    public class BaoCaoController : Controller
    {
        private NhutLongCompanyEntities db = new NhutLongCompanyEntities();
        // GET: BaoCao
        public ActionResult Index()
        {
            return View();
        }
    }
}