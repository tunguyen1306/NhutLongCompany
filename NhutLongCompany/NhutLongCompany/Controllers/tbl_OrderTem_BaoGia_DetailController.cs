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
    public class tbl_OrderTem_BaoGia_DetailController : Controller
    {
        private NhutLongCompanyEntities db = new NhutLongCompanyEntities();

        // GET: tbl_OrderTem_BaoGia_Detail
        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View(db.tbl_OrderTem_BaoGia_Detail.ToList());
        }

        // GET: tbl_OrderTem_BaoGia_Detail/Details/5
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
            tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail = db.tbl_OrderTem_BaoGia_Detail.Find(id);
            if (tbl_OrderTem_BaoGia_Detail == null)
            {
                return HttpNotFound();
            }
            return View(tbl_OrderTem_BaoGia_Detail);
        }

        // GET: tbl_OrderTem_BaoGia_Detail/Create
        public ActionResult Create()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }

        // POST: tbl_OrderTem_BaoGia_Detail/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,baogia_id,sanpam_id,soluong,money")] tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (ModelState.IsValid)
            {
                db.tbl_OrderTem_BaoGia_Detail.Add(tbl_OrderTem_BaoGia_Detail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_OrderTem_BaoGia_Detail);
        }

        // GET: tbl_OrderTem_BaoGia_Detail/Edit/5
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
            tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail = db.tbl_OrderTem_BaoGia_Detail.Find(id);
            if (tbl_OrderTem_BaoGia_Detail == null)
            {
                return HttpNotFound();
            }
            return View(tbl_OrderTem_BaoGia_Detail);
        }

        // POST: tbl_OrderTem_BaoGia_Detail/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,baogia_id,sanpam_id,soluong,money")] tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (ModelState.IsValid)
            {
                db.Entry(tbl_OrderTem_BaoGia_Detail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_OrderTem_BaoGia_Detail);
        }

        // GET: tbl_OrderTem_BaoGia_Detail/Delete/5
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
            tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail = db.tbl_OrderTem_BaoGia_Detail.Find(id);
            if (tbl_OrderTem_BaoGia_Detail == null)
            {
                return HttpNotFound();
            }
            return View(tbl_OrderTem_BaoGia_Detail);
        }

        // POST: tbl_OrderTem_BaoGia_Detail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail = db.tbl_OrderTem_BaoGia_Detail.Find(id);
            db.tbl_OrderTem_BaoGia_Detail.Remove(tbl_OrderTem_BaoGia_Detail);
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
