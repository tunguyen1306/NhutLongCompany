using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using NhutLongCompany.Domain;
using NhutLongCompany.Models;
using NhutLongCompany.Attribute;

namespace NhutLongCompany.Controllers
{
    [RedirectOnError]
    public class NavbarController : Controller
    {
        private readonly NhutLongCompanyEntities db = new NhutLongCompanyEntities();
        // GET: Navbar
        public ActionResult Index()
        {
            var data = new Data();
            var idu = Convert.ToInt32(Session["userId"]);
            ViewBag.ten = Session["username"];
            var qr = (from dataMenu in db.AdminMenus where dataMenu.status == true && dataMenu.IdUser == idu select dataMenu).ToList()
                    .Select(x => new Navbar
                    {
                        Id=x.Id,
                        action = x.action,
                        nameOption = x.nameOption,
                        controller = x.controller,
                        imageClass = x.imageClass,
                        status = (bool) x.status,
                        parentId = x.parentId,
                        isParent = (bool) x.isParent,
                        isOrder = x.orderId
                    });
            return PartialView("_Navbar", new Data().navbarItems());
        }

        public ActionResult MenuList()
        {
            return PartialView("Menulist");

        }

        public ActionResult AddMenu()
        {
            var list = new List<SelectListItem>();
            list.Add(new SelectListItem { Text = "Do Not Parent Menu", Value = "0" });
            var data = (from datamenu in db.AdminMenus
                                     where datamenu.isParent == true
                                     select datamenu).ToArray();
            for (int i = 0; i < data.Length; i++)
            {
                list.Add(new SelectListItem { Text = data[i].nameOption, Value = data[i].Id.ToString() });
            }


            var listuser = new List<SelectListItem>();
            listuser.Add(new SelectListItem { Text = "Tât cả", Value = "0" });
            var qruser = (from datauser in db.tbl_User
                          where datauser.Status == 1
                          select datauser).ToArray();
            for (int i = 0; i < qruser.Length; i++)
            {
                listuser.Add(new SelectListItem { Text = qruser[i].FullName, Value = qruser[i].IDUser.ToString() });
            }

            ViewData["listuser"] = listuser;
            ViewData["list"] = list;
            
            return View("AddMenu");
        }

        [HttpPost]
        public ActionResult AddMenu(string command, AdminMenu name)
        {
            if (command == "send")
            {
                db.AdminMenus.Add(name);
                db.SaveChanges();
            }

            return PartialView("MenuList");
        }

        public ActionResult EditMenu(int id)
        {
            if (id != null)
            {
                var list = new List<SelectListItem>();
                list.Add(new SelectListItem { Text = "Do Not Parent", Value = "0" });
                var data =
                    (from datamenu in db.AdminMenus where datamenu.isParent == true select datamenu)
                        .ToArray();
                for (int i = 0; i < data.Length; i++)
                {
                    list.Add(new SelectListItem
                    {
                        Text = data[i].nameOption,
                        Value = data[i].Id.ToString()
                    });
                }
                var listuser = new List<SelectListItem>();
                listuser.Add(new SelectListItem { Text = "Tât cả", Value = "0" });
                var qruser = (from datauser in db.tbl_User
                              where datauser.Status == 1
                              select datauser).ToArray();
                for (int i = 0; i < qruser.Length; i++)
                {
                    listuser.Add(new SelectListItem { Text = qruser[i].FullName, Value = qruser[i].IDUser.ToString() });
                }

                ViewData["listuser"] = listuser;
                ViewData["list"] = list;
                AdminMenu webAdminMenu = db.AdminMenus.Find(id);
                return View(webAdminMenu);
            }
            return View();
        }

        [HttpPost]
        public ActionResult EditMenu(string command,
            [Bind(
                Include =
                    "Id,nameOption,controller,action,imageClass,status,orderId,parentId,isParent,IdUser"
                )] AdminMenu name)
        {
            if (command == "send")
            {
                db.Entry(name).State = EntityState.Modified;
                db.SaveChanges();
            }

            return PartialView("MenuList");
        }

        public ActionResult Delete(int? id)
        {
            AdminMenu webAdminMenu = db.AdminMenus.Find(id);
            db.AdminMenus.Remove(webAdminMenu);
            db.SaveChanges();
            return PartialView("MenuList");
        }

    }
}