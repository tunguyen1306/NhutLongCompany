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
    public class FlowController : Controller
    {
        private NhutLongCompanyEntities db = new NhutLongCompanyEntities();

        // GET: /Flow/
        public ActionResult Index()
        {
            return View(db.tbl_Config_FlowName.ToList());
        }
        public ActionResult Config()
        {

            ConfigStepInFlow config = new ConfigStepInFlow();
            config.ListFlow = db.tbl_Config_FlowName.Where(T => T.TrangThai == 1).OrderBy(T=>T.ThuTu).ToList();
            config.ListStep = db.tbl_Config_Step.Where(T => T.TrangThai >= 1).OrderBy(T => T.ThuTu).ToList();
            config.ListConfig = db.tbl_Config_StepInFlow.Where(T => T.TrangThai == 1).ToList();
            return View(config);
        }
        [HttpPost]
        public JsonResult UpdateConfig(int idFlow, int idStep,bool check)
        {
            try
            {
                if (!check)
                {
                    List<tbl_Config_StepInFlow> list = db.tbl_Config_StepInFlow.Where(T => T.ID_Flow == idFlow && T.ID_Step == idStep && T.TrangThai == 1).ToList();
                    if (list.Count > 0)
                    {
                        list[0].TrangThai = 0;
                        db.Entry(list[0]).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                  
                }
                else
                {
                    List<tbl_Config_StepInFlow> list = db.tbl_Config_StepInFlow.Where(T => T.ID_Flow == idFlow && T.ID_Step == idStep).ToList();
                    if (list.Count > 0)
                    {
                        list[0].TrangThai = 1;
                        db.Entry(list[0]).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        tbl_Config_StepInFlow tmp=new tbl_Config_StepInFlow();
                        tmp.TrangThai = 1;
                        tmp.ID_Flow = idFlow;
                        tmp.ID_Step = idStep;
                        db.tbl_Config_StepInFlow.Add(tmp);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {

                return Json("Error - " + e.Message);
            }

            return Json("Success");
        }
        // GET: /Flow/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Config_FlowName tbl_config_flowname = db.tbl_Config_FlowName.Find(id);
            if (tbl_config_flowname == null)
            {
                return HttpNotFound();
            }
            return View(tbl_config_flowname);
        }

        // GET: /Flow/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Flow/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,TenQuyTrinh,DienGiai,ThuTu,TrangThai")] tbl_Config_FlowName tbl_config_flowname)
        {
            if (ModelState.IsValid)
            {
                db.tbl_Config_FlowName.Add(tbl_config_flowname);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_config_flowname);
        }

        // GET: /Flow/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Config_FlowName tbl_config_flowname = db.tbl_Config_FlowName.Find(id);
            if (tbl_config_flowname == null)
            {
                return HttpNotFound();
            }
            return View(tbl_config_flowname);
        }

        // POST: /Flow/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,TenQuyTrinh,DienGiai,ThuTu,TrangThai")] tbl_Config_FlowName tbl_config_flowname)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_config_flowname).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_config_flowname);
        }

        // GET: /Flow/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_Config_FlowName tbl_config_flowname = db.tbl_Config_FlowName.Find(id);
            if (tbl_config_flowname == null)
            {
                return HttpNotFound();
            }
            return View(tbl_config_flowname);
        }

        // POST: /Flow/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_Config_FlowName tbl_config_flowname = db.tbl_Config_FlowName.Find(id);
            db.tbl_Config_FlowName.Remove(tbl_config_flowname);
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
