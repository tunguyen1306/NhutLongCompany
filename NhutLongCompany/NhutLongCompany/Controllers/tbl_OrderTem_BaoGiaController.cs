using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NhutLongCompany.Models;
using NhutLongCompany.Attribute;

namespace NhutLongCompany.Controllers
{
    [RedirectOnError]
    public class tbl_OrderTem_BaoGiaController : Controller
    {
        private NhutLongCompanyEntities db = new NhutLongCompanyEntities();

        // GET: tbl_OrderTem_BaoGia
        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View(db.tbl_OrderTem_BaoGia.ToList());
        }

        // GET: tbl_OrderTem_BaoGia/Details/5
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
            tbl_OrderTem_BaoGia tbl_OrderTem_BaoGia = db.tbl_OrderTem_BaoGia.Find(id);
            if (tbl_OrderTem_BaoGia == null)
            {
                return HttpNotFound();
            }
            return View(tbl_OrderTem_BaoGia);
        }

        // GET: tbl_OrderTem_BaoGia/Create
        public ActionResult Create()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        // POST: tbl_OrderTem_BaoGia/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,order_id,total_money,date_begin,date_end,status,offset")] tbl_OrderTem_BaoGia tbl_OrderTem_BaoGia)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (ModelState.IsValid)
            {
                db.tbl_OrderTem_BaoGia.Add(tbl_OrderTem_BaoGia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_OrderTem_BaoGia);
        }

        // GET: tbl_OrderTem_BaoGia/Edit/5
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
            tbl_OrderTem_BaoGia tbl_OrderTem_BaoGia = db.tbl_OrderTem_BaoGia.Find(id);
            if (tbl_OrderTem_BaoGia == null)
            {
                return HttpNotFound();
            }
            return View(tbl_OrderTem_BaoGia);
        }

        // POST: tbl_OrderTem_BaoGia/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,order_id,total_money,date_begin,date_end,status,offset")] tbl_OrderTem_BaoGia tbl_OrderTem_BaoGia)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (ModelState.IsValid)
            {
                db.Entry(tbl_OrderTem_BaoGia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_OrderTem_BaoGia);
        }

        // GET: tbl_OrderTem_BaoGia/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_OrderTem_BaoGia tbl_OrderTem_BaoGia = db.tbl_OrderTem_BaoGia.Find(id);
            if (tbl_OrderTem_BaoGia == null)
            {
                return HttpNotFound();
            }
            return View(tbl_OrderTem_BaoGia);
        }

        // POST: tbl_OrderTem_BaoGia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            tbl_OrderTem_BaoGia tbl_OrderTem_BaoGia = db.tbl_OrderTem_BaoGia.Find(id);
            db.tbl_OrderTem_BaoGia.Remove(tbl_OrderTem_BaoGia);
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
