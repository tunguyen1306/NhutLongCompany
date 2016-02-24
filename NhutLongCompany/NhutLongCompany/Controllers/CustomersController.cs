using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NhutLongCompany.Models;

namespace NhutLongCompany.Controllers
{
    public class CustomersController : Controller
    {
        private NhutLongCompanyEntities db = new NhutLongCompanyEntities();
      

        // GET: Customers
        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View(db.tbl_Customers.ToList());
        }
        public ActionResult DetailCustomers(int id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            tbl_Customers tblCustomers = db.tbl_Customers.Find(id);
            return View("DetailCustomers",tblCustomers);
        }


        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Customers tbl_Customers = db.tbl_Customers.Find(id);
            if (tbl_Customers == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Customers);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDCustomers,NameCustomers,ChucvuCustomers,CongTyCustomers,CodeCustomers,EmailCustomers,PhoneCustomers,FaxCustomers,DiachiCustomers,MasothueCustomers,StatusCustomers,CreateDateCustomers,ModifyDateCustomers,CreateUserCustomers,ModifyUserCustomers")] tbl_Customers tbl_Customers)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (ModelState.IsValid)
            {
                db.tbl_Customers.Add(tbl_Customers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Customers);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Customers tbl_Customers = db.tbl_Customers.Find(id);
            if (tbl_Customers == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Customers);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDCustomers,NameCustomers,ChucvuCustomers,CongTyCustomers,CodeCustomers,EmailCustomers,PhoneCustomers,FaxCustomers,DiachiCustomers,MasothueCustomers,StatusCustomers,CreateDateCustomers,ModifyDateCustomers,CreateUserCustomers,ModifyUserCustomers")] tbl_Customers tbl_Customers)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Customers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Customers);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var qr = from datadh in db.tbl_OrderTem
                     where datadh.customer_id == id
                     select datadh;
            if (qr.ToList().Count > 0)
            {
                ViewData["t"] = "Khách hàng có đơn hàng không được xóa";
              
                return RedirectToAction("DetailCustomers",new{id});
            }
            else
            {
               
                tbl_Customers tbl_Customers = db.tbl_Customers.Find(id);
                db.tbl_Customers.Remove(tbl_Customers);
                db.SaveChanges();
            }
           
            return RedirectToAction("Index");

        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            tbl_Customers tbl_Customers = db.tbl_Customers.Find(id);
            db.tbl_Customers.Remove(tbl_Customers);
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
    }
}
