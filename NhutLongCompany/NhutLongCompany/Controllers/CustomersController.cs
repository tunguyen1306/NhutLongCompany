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
    public class CustomersController : Controller
    {
        private NhutLongCompanyEntities db = new NhutLongCompanyEntities();
        private int ck { get; set; }

        [ActionAuthorizeAttribute("BaoGia")]
        // GET: Customers
        public ActionResult Index()
        {
       
            return View(db.tbl_Customers.ToList());
        }
        [ActionAuthorizeAttribute("BaoGia")]
        public ActionResult DetailCustomers(int id)
        {
           
            tbl_Customers tblCustomers = db.tbl_Customers.Find(id);
            if ((string) Session["ck"] =="1")
            {
                ViewBag.thongbao = "Khách hàng có đơn hàng không được xóa";
                Session["ck"] = "0";
            }
            else
            {
                ViewBag.thongbao = "";
            }
            return View("DetailCustomers",tblCustomers);
        }

        [ActionAuthorizeAttribute("BaoGia")]
        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
           
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
        [ActionAuthorizeAttribute("BaoGia")]
        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [ActionAuthorizeAttribute("BaoGia")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDCustomers,NameCustomers,ChucvuCustomers,CongTyCustomers,CodeCustomers,EmailCustomers,PhoneCustomers,FaxCustomers,DiaChiCustomers,MasothueCustomers,StatusCustomers,CreateDateCustomers,ModifyDateCustomers,CreateUserCustomers,ModifyUserCustomers,NoteCustomer")] tbl_Customers tbl_Customers)
        {
          
            if (ModelState.IsValid)
            {
                db.tbl_Customers.Add(tbl_Customers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tbl_Customers);
        }

        // GET: Customers/Edit/5
        [ActionAuthorizeAttribute("BaoGia")]
        public ActionResult Edit(int? id)
        {
          
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
        [ActionAuthorizeAttribute("BaoGia")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDCustomers,NameCustomers,ChucvuCustomers,CongTyCustomers,CodeCustomers,EmailCustomers,PhoneCustomers,FaxCustomers,DiaChiCustomers,MasothueCustomers,StatusCustomers,CreateDateCustomers,ModifyDateCustomers,CreateUserCustomers,ModifyUserCustomers,NoteCustomer")] tbl_Customers tbl_Customers)
        {
           
            if (ModelState.IsValid)
            {
                db.Entry(tbl_Customers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tbl_Customers);
        }

        // GET: Customers/Delete/5
        [ActionAuthorizeAttribute("BaoGia")]
        public ActionResult Delete(int? id)
        {
           
            
            var qr = from datadh in db.tbl_OrderTem
                     where datadh.customer_id == id
                     select datadh;
            if (qr.ToList().Count > 0)
            {
                Session["ck"] = "1";
                
                return RedirectToAction("DetailCustomers", new { id });
            }
            else
            {
               
                tbl_Customers tbl_Customers = db.tbl_Customers.Find(id);
                db.tbl_Customers.Remove(tbl_Customers);
                db.SaveChanges();
                Session["ck"] ="0";
            }
           
            return RedirectToAction("Index");

        }

        // POST: Customers/Delete/5
        [ActionAuthorizeAttribute("BaoGia")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
         
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
        [ActionAuthorizeAttribute("BaoGia")]
        public ActionResult ViewNote()
        {
            return View();
        }
        [ActionAuthorizeAttribute("BaoGia")]
        public ActionResult IndexBaoGia(int id)
        {
           
            var qr = (from data in db.tbl_OrderTem
                      join cus in db.tbl_Customers on data.customer_id equals cus.IDCustomers
                      where cus.IDCustomers == id
                      orderby data.update_date descending
                      select new DonHangView
                      {
                          id = data.id,
                          customer_id = cus.IDCustomers,
                          Customer = cus,
                          code = data.code,
                          date_deliver = data.date_deliver,
                          address_deliver = data.address_deliver,
                          status = data.status
                      });
            List<DonHangView> list = qr.ToList();
            foreach (var itemBG in list)
            {
                var queryBaoGia = from u in db.tbl_OrderTem_BaoGia where u.order_id.Value.Equals(itemBG.id) orderby u.id descending select u;
                List<tbl_OrderTem_BaoGia> lisBG = queryBaoGia.ToList<tbl_OrderTem_BaoGia>();
                List<BaoGiaTemView> lisBGTem = new List<BaoGiaTemView>();
                if (lisBG.Count > 1)
                {
                    for (int i = 1; i < lisBG.Count; i++)
                    {
                        var item = lisBG[i];
                        BaoGiaTemView temBG = new BaoGiaTemView { commission = item.commission, commission_money = item.commission_monney, note = item.note, date_begin = item.date_begin, date_end = item.date_end, id = item.id, order_id = item.order_id, status = item.status, total_money = item.total_money };

                        lisBGTem.Add(temBG);
                    }
                }
                itemBG.BaoGiaTemViews = lisBGTem;
                queryBaoGia = from u in db.tbl_OrderTem_BaoGia where u.order_id.Value.Equals(itemBG.id) orderby u.id descending select u;
                lisBG = queryBaoGia.ToList<tbl_OrderTem_BaoGia>();
                foreach (var item in lisBG)
                {
                    BaoGiaTemView temBG = new BaoGiaTemView { commission = item.commission, commission_money = item.commission_monney, note = item.note, date_begin = item.date_begin, date_end = item.date_end, id = item.id, order_id = item.order_id, status = item.status, total_money = item.total_money };

                    itemBG.BaoGiaTemView = temBG;

                    break;
                }
            }
            return View(list);
        }

    }
}
