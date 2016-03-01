using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using NhutLongCompany.Domain;
using NhutLongCompany.Models;

namespace NhutLongCompany.Controllers
{
    public class ProductsController : Controller
    {
        private NhutLongCompanyEntities db = new NhutLongCompanyEntities();

        public ActionResult IndexProcessProduct()
        {

            return View();
        }
        // GET: Products
        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
               
            }
            var qr = (from data in db.tbl_OrderTem_BaoGia_Detail
                      join datapro in db.tbl_Products on data.sanpam_id equals datapro.ID_Products
                      join dataquy in db.tbl_QuyTrinh on data.id equals dataquy.ID_BaoGiaDetail
                      where data.status == 2
                      select new BaoGiaTemDetailView
                      {
                          NameProducts = datapro.NameProducts,
                          CodeProducts = datapro.CodeProducts,
                          Date_Working = data.date_working,
                          Date_end = dataquy.NgayKetThuc_TT

                      });
            return View(qr.ToList());
        }
        public ActionResult SPWorking()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");

            }
            var qr = (from data in db.tbl_OrderTem_BaoGia_Detail
                      join datapro in db.tbl_Products on data.sanpam_id equals datapro.ID_Products
                      join dataquy in db.tbl_QuyTrinh on data.id equals dataquy.ID_BaoGiaDetail
                      where data.status == 1
                      select new BaoGiaTemDetailView
                      {
                          NameProducts = datapro.NameProducts,
                          CodeProducts = datapro.CodeProducts,
                          Date_Working = data.date_working,
                          Date_end = dataquy.NgayKetThuc_TT

                      });
            return View(qr.ToList());
        }
        // GET: Products/Details/5
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
            tbl_Products tbl_Products = db.tbl_Products.Find(id);
            if (tbl_Products == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {


            return View();
        }



        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID_Products,NameProducts,SolopProducts,QuyCachProducts,LoaigiayProducts,OffsetFlexoProducts,DanKimProducts,GiaProducts,CreatedDateProducts,ModifyDateProducts,CreateUserProducts,ModifyUserProducts,StatusProducts,CodeProducts")] tbl_Products tbl_Products)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (ModelState.IsValid)
            {
                db.tbl_Products.Add(tbl_Products);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Products);
        }

        // GET: Products/Edit/5
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
            tbl_Products tbl_Products = db.tbl_Products.Find(id);
            if (tbl_Products == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_Products,NameProducts,SolopProducts,QuyCachProducts,LoaigiayProducts,OffsetFlexoProducts,DanKimProducts,GiaProducts,CreatedDateProducts,ModifyDateProducts,CreateUserProducts,ModifyUserProducts,StatusProducts,CodeProducts")] tbl_Products tbl_Products)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Products).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Products);
        }

        // GET: Products/Delete/5
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
            tbl_Products tbl_Products = db.tbl_Products.Find(id);
            if (tbl_Products == null)
            {
                return HttpNotFound();
            }
            return View(tbl_Products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            tbl_Products tbl_Products = db.tbl_Products.Find(id);
            db.tbl_Products.Remove(tbl_Products);
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
