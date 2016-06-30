using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NhutLongCompany.Models;
using System.Web.Security;
using NhutLongCompany.Attribute;

namespace NhutLongCompany.Controllers
{
    [RedirectOnError]
    public class LoginController : Controller
    {
        private NhutLongCompanyEntities db = new NhutLongCompanyEntities();

        public ActionResult Index()
        {
            //if (Session["username"] == null)
            //{
            //    return RedirectToAction("Login", "Login");
            //}
            return View(db.tbl_User.ToList());
        }

        // GET: Login/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_User tbl_User = db.tbl_User.Find(id);
            if (tbl_User == null)
            {
                return HttpNotFound();
            }
            return View(tbl_User);
        }

        // GET: Login/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Login/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDUser,Username,FullName,Status,ModifyDate,ModifyUser,CreateUser,CreateDate,Password")] tbl_User tbl_User)
        {
            if (ModelState.IsValid)
            {
                db.tbl_User.Add(tbl_User);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_User);
        }

        // GET: Login/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_User tbl_User = db.tbl_User.Find(id);
            if (tbl_User == null)
            {
                return HttpNotFound();
            }
            return View(tbl_User);
        }

        // POST: Login/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDUser,Username,FullName,Status,ModifyDate,ModifyUser,CreateUser,CreateDate,Password")] tbl_User tbl_User)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_User).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_User);
        }

        // GET: Login/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_User tbl_User = db.tbl_User.Find(id);
            if (tbl_User == null)
            {
                return HttpNotFound();
            }
            return View(tbl_User);
        }

        // POST: Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_User tbl_User = db.tbl_User.Find(id);
            db.tbl_User.Remove(tbl_User);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Login()
        {

            //var data = db.tbl_User.Where(x => x.Username == "admin@gmail.com" && x.Password == "1234").Select(x => new { x.Username, x.FullName }).FirstOrDefault();


            //Session["username"] = data.Username;
            //if (data != null)
            //{
            //    return RedirectToAction("Home", "Home");
            //}

            return View();
        }
        public ActionResult NoAuthorize()
        {
            ViewData["Message"] = "Bạn không đủ quyền thực hiện chức năng này";
            return View();
        }
        public PartialViewResult AjaxLogin()
        {
         
            return PartialView();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
       
            Session.Clear();
            var data = db.tbl_User.Where(x => x.Username == username && x.Password == password).Select(x => new { x.Username,x.FullName,x.IDUser,x.RoleName}).FirstOrDefault();
         

            //var qrmenu =(from datamenu in db.AdminMenus where datamenu.IdUser == data.IDUser select datamenu).Select(x => new { x.controller,x.action}).FirstOrDefault();
        
          
            if (data != null)
            {
                FormsAuthentication.SetAuthCookie(data.Username, false);
                String returnUrl = Request.Params["ReturnUrl"];
              
               
                Session["userId"] = data.IDUser;
                Session["username"] = data.Username;
                Session["roleName"] = data.RoleName;
                if (String.IsNullOrEmpty(returnUrl))
                {
                    return RedirectToAction("Home", "Home");
                }
                else
                {
                    return Redirect(Server.UrlDecode( returnUrl));
                }
              
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Remove("username");
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
        }

    }
}