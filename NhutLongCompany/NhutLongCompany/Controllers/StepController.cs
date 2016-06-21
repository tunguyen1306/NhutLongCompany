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
    public class StepController : Controller
    {
        private NhutLongCompanyEntities db = new NhutLongCompanyEntities();

        // GET: /Step/
        public ActionResult Index()
        {
            return View(db.tbl_Config_Step.ToList());
        }

        // GET: /Step/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Config_Step tbl_config_step = db.tbl_Config_Step.Find(id);
            if (tbl_config_step == null)
            {
                return HttpNotFound();
            }
            return View(tbl_config_step);
        }

        // GET: /Step/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Step/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,TenCongDoan,DienGiai,ThuTu,TrangThai")] tbl_Config_Step tbl_config_step)
        {
            if (ModelState.IsValid)
            {
                db.tbl_Config_Step.Add(tbl_config_step);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_config_step);
        }

        // GET: /Step/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Config_Step tbl_config_step = db.tbl_Config_Step.Find(id);
            if (tbl_config_step == null)
            {
                return HttpNotFound();
            }
            return View(tbl_config_step);
        }

        // POST: /Step/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,TenCongDoan,DienGiai,ThuTu,TrangThai")] tbl_Config_Step tbl_config_step)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_config_step).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_config_step);
        }

        // GET: /Step/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Config_Step tbl_config_step = db.tbl_Config_Step.Find(id);
            if (tbl_config_step == null)
            {
                return HttpNotFound();
            }
            return View(tbl_config_step);
        }

        // POST: /Step/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_Config_Step tbl_config_step = db.tbl_Config_Step.Find(id);
            db.tbl_Config_Step.Remove(tbl_config_step);
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
