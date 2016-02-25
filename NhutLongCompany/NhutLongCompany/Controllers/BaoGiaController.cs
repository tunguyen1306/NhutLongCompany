using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using NhutLongCompany.Models;


namespace NhutLongCompany.Controllers
{
    public class BaoGiaController : Controller
    {
        private NhutLongCompanyEntities db = new NhutLongCompanyEntities();

        // GET: BaoGia
      

        public ActionResult Index(int id)
        {
            var query = (from bao in db.tbl_BaoGia
                join cus in db.tbl_Customers on bao.ID_Customers equals cus.IDCustomers
                         where bao.ID_Customers==id
                select new
                {
                    bao,
                    cus
                }).ToList().Select(x => new BaoGiaColumn
                {
                    ID_BaoGia = x.bao.ID_BaoGia,
                    ID_Customers = x.cus.IDCustomers,
                    NameCustomers = x.cus.NameCustomers,
                    BaoGia_ID_Default = x.bao.BaoGia_ID_Default,
                    TongTien = x.bao.TongTien,
                    LamMau = x.bao.LamMau,
                    NgayCoMau = x.bao.NgayCoMau,
                    NgayBaoGia = x.bao.NgayBaoGia,
                    DuyetBaoGia = x.bao.DuyetBaoGia,
                    CodeDonHang = x.bao.CodeDonHang,
                    NgayBatDauDuKien = x.bao.NgayBatDauDuKien,
                    NgayKetThucDuKien = x.bao.NgayKetThucDuKien,
                    NgayBatDauThucTe = x.bao.NgayBatDauThucTe,
                    NgayKetThucThucTe = x.bao.NgayKetThucThucTe,
                    NgayGiao = x.bao.NgayGiao,
                    QuyTrinh = x.bao.QuyTrinh,
                    ChiTietQuyTrinh = x.bao.ChiTietQuyTrinh,
                    LanBaoGia = x.bao.LanBaoGia,
                    ChucvuCustomers = x.cus.ChucvuCustomers,
                    CongTyCustomers = x.cus.CongTyCustomers,
                    CodeCustomers = x.cus.CodeCustomers,
                    EmailCustomers = x.cus.EmailCustomers,
                    PhoneCustomers = x.cus.PhoneCustomers,
                    FaxCustomers = x.cus.FaxCustomers,
                    DiachiCustomers = x.cus.DiachiCustomers,
                    MasothueCustomers = x.cus.MasothueCustomers,
                });
            return View(query.ToList());
        }

        // GET: BaoGia/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_BaoGia tbl_BaoGia = db.tbl_BaoGia.Find(id);
            if (tbl_BaoGia == null)
            {
                return HttpNotFound();
            }
            return View(tbl_BaoGia);
        }

        // GET: BaoGia/Create
        public ActionResult Create()
        {
            BaoGiaColumn  cl=new BaoGiaColumn();
            cl.Products=new List<tbl_Products>();
            return View(cl);
        }

        // POST: BaoGia/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(
                Include =
                    "ID_BaoGia,ID_Customers,BaoGia_ID_Default,TongTien,LamMau,NgayCoMau,NgayBaoGia,DuyetBaoGia,CodeDonHang,NgayBatDauDuKien,NgayKetThucDuKien,NgayBatDauThucTe,NgayKetThucThucTe,NgayGiao,QuyTrinh,ChiTietQuyTrinh,LanBaoGia"
                )] tbl_BaoGia tbl_BaoGia)
        {
            if (ModelState.IsValid)
            {
                db.tbl_BaoGia.Add(tbl_BaoGia);
                db.SaveChanges();
                return RedirectToAction("Index", "Customers");
            }

            return View(tbl_BaoGia);
        }

        // GET: BaoGia/Edit/5
        public ActionResult Edit(int? id)
        {
            var query = (from bao in db.tbl_BaoGia
                         join cus in db.tbl_Customers on bao.ID_Customers equals cus.IDCustomers
                         where bao.ID_BaoGia==id
                         select new
                         {
                             bao,
                             cus
                         }).ToList().Select(x => new BaoGiaColumn
                         {
                             ID_BaoGia = x.bao.ID_BaoGia,
                             NameCustomers = x.cus.NameCustomers,
                             BaoGia_ID_Default = x.bao.BaoGia_ID_Default,
                             TongTien = x.bao.TongTien,
                             LamMau = x.bao.LamMau,
                             NgayCoMau = x.bao.NgayCoMau,
                             NgayBaoGia = x.bao.NgayBaoGia,
                             DuyetBaoGia = x.bao.DuyetBaoGia,
                             CodeDonHang = x.bao.CodeDonHang,
                             NgayBatDauDuKien = x.bao.NgayBatDauDuKien,
                             NgayKetThucDuKien = x.bao.NgayKetThucDuKien,
                             NgayBatDauThucTe = x.bao.NgayBatDauThucTe,
                             NgayKetThucThucTe = x.bao.NgayKetThucThucTe,
                             NgayGiao = x.bao.NgayGiao,
                             QuyTrinh = x.bao.QuyTrinh,
                             ChiTietQuyTrinh = x.bao.ChiTietQuyTrinh,
                             LanBaoGia = x.bao.LanBaoGia,
                             ChucvuCustomers = x.cus.ChucvuCustomers,
                             CongTyCustomers = x.cus.CongTyCustomers,
                             CodeCustomers = x.cus.CodeCustomers,
                             EmailCustomers = x.cus.EmailCustomers,
                             PhoneCustomers = x.cus.PhoneCustomers,
                             FaxCustomers = x.cus.FaxCustomers,
                             DiachiCustomers = x.cus.DiachiCustomers,
                             MasothueCustomers = x.cus.MasothueCustomers,
                         });
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BaoGiaColumn baoGiaColumn = query.First();
            if (baoGiaColumn == null)
            {
                return HttpNotFound();
            }
            var queryPro = (from datapro in db.tbl_Products
                join databagia in db.tbl_BaoGiaChiTiet on datapro.ID_Products equals databagia.ID_SanPham
                where databagia.ID_BaoGia==id
                select datapro);
                //{
                //    datapro

                //}).Select(x=>new tbl_Products{CodeProducts = x.datapro.CodeProducts});
            var li = queryPro.ToList();
            baoGiaColumn.Products = queryPro.ToList();
            return View(baoGiaColumn);
        }

        // POST: BaoGia/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(
                Include =
                    "ID_BaoGia,ID_Customers,BaoGia_ID_Default,TongTien,LamMau,NgayCoMau,NgayBaoGia,DuyetBaoGia,CodeDonHang,NgayBatDauDuKien,NgayKetThucDuKien,NgayBatDauThucTe,NgayKetThucThucTe,NgayGiao,QuyTrinh,ChiTietQuyTrinh,LanBaoGia"
                )] tbl_BaoGia tbl_BaoGia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_BaoGia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Customers");
            }
            return View(tbl_BaoGia);
        }

        // GET: BaoGia/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_BaoGia tbl_BaoGia = db.tbl_BaoGia.Find(id);
            if (tbl_BaoGia == null)
            {
                return HttpNotFound();
            }
            return View(tbl_BaoGia);
        }

        // POST: BaoGia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_BaoGia tbl_BaoGia = db.tbl_BaoGia.Find(id);
            db.tbl_BaoGia.Remove(tbl_BaoGia);
            db.SaveChanges();
            return RedirectToAction("Index", "Customers");
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
