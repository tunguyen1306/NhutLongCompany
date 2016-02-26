using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NhutLongCompany.Models;

namespace NhutLongCompany.Controllers
{
    public class SanXuatController : Controller
    {
        private NhutLongCompanyEntities db = new NhutLongCompanyEntities();
        // GET: SanXuat
        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var qr = (from data in db.tbl_OrderTem
                      join cus in db.tbl_Customers on data.customer_id equals cus.IDCustomers
                      where data.status >= 1
                      select new DonHangView
                      {
                          id = data.id,
                          customer_id = cus.IDCustomers,
                          Customer = cus,
                          code = data.code,
                          date_begin_plan = data.date_begin_plan,
                          date_end_plan = data.date_end_plan,
                          status = data.status



                      });

            return View(qr.ToList());
        }
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
            tbl_OrderTem tbl_OrderTem = db.tbl_OrderTem.Find(id);
            if (tbl_OrderTem == null)
            {
                return HttpNotFound();
            }

            DonHangView d = new DonHangView();
            d.customer_id = tbl_OrderTem.customer_id;
            d.code = tbl_OrderTem.code;
            d.date_begin = tbl_OrderTem.date_begin;
            d.date_end = tbl_OrderTem.date_end;
            d.id = tbl_OrderTem.id;
            d.status = tbl_OrderTem.status;
            d.date_begin_plan = tbl_OrderTem.date_begin_plan;
            d.date_end_plan = tbl_OrderTem.date_end_plan;
            var queryBaoGia = from u in db.tbl_OrderTem_BaoGia where u.order_id.Value.Equals(tbl_OrderTem.id) orderby u.id descending select u;
            List<tbl_OrderTem_BaoGia> lisBG = queryBaoGia.ToList<tbl_OrderTem_BaoGia>();
            List<BaoGiaTemView> lisBGTem = new List<BaoGiaTemView>();

            if (lisBG.Count > 1)
            {
                for (int i = 1; i < lisBG.Count; i++)
                {
                    var item = lisBG[i];
                    BaoGiaTemView temBG = new BaoGiaTemView { note = item.note, date_begin = item.date_begin, date_end = item.date_end, id = item.id, order_id = item.order_id, status = item.status, total_money = item.total_money };
                    var queryGiaoGiaCT = from u in db.tbl_OrderTem_BaoGia_Detail
                                         join y in db.tbl_Products on u.sanpam_id equals y.ID_Products
                                         where u.baogia_id.Value.Equals(temBG.id)
                                         select new BaoGiaTemDetailView { id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
                    temBG.BaoGiaTemDetailViews = queryGiaoGiaCT.ToList<BaoGiaTemDetailView>();
                    lisBGTem.Add(temBG);
                }
            }

            d.BaoGiaTemViews = lisBGTem;
            queryBaoGia = from u in db.tbl_OrderTem_BaoGia where u.order_id.Value.Equals(tbl_OrderTem.id) orderby u.id descending select u;
            lisBG = queryBaoGia.ToList<tbl_OrderTem_BaoGia>();
            foreach (var item in lisBG)
            {
                BaoGiaTemView temBG = new BaoGiaTemView { note = item.note, date_begin = item.date_begin, date_end = item.date_end, id = item.id, order_id = item.order_id, status = item.status, total_money = item.total_money };
                var queryGiaoGiaCT = from u in db.tbl_OrderTem_BaoGia_Detail
                                     join y in db.tbl_Products on u.sanpam_id equals y.ID_Products
                                     where u.baogia_id.Value.Equals(temBG.id)
                                     select new BaoGiaTemDetailView { id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
                temBG.BaoGiaTemDetailViews = queryGiaoGiaCT.ToList<BaoGiaTemDetailView>();
                d.BaoGiaTemView = temBG;
                foreach (var itemSP in d.BaoGiaTemView.BaoGiaTemDetailViews)
                {
                    var queryQT = from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(itemSP.id) orderby u.ThuTu ascending select u;
                    itemSP.QuyTrinhs = queryQT.ToList<tbl_QuyTrinh>();
                }
                break;
            }



            var list = from tt in db.tbl_Customers where tt.IDCustomers == d.customer_id select tt;
            d.Customer = list.ToList()[0];
            return View(d);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DonHangView donHang)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            int? id = donHang.id;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_OrderTem tbl_OrderTem = db.tbl_OrderTem.Find(id);
            if (tbl_OrderTem == null)
            {
                return HttpNotFound();
            }

            if (donHang.action == 5)
            {
                donHang.action = 0;
                tbl_OrderTem order = db.tbl_OrderTem.Find(donHang.id);
                if (donHang.status.Value == 3)
                {
                    order.date_begin = DateTime.Now;
                }
                if (donHang.status.Value == 4)
                {
                    order.date_end = DateTime.Now;
                }
                order.status = donHang.status.Value;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();

            }
            if (donHang.action == 6)
            {
                foreach (var item in donHang.BaoGiaTemView.BaoGiaTemDetailViews)
                {
                    foreach (var itemSP in item.QuyTrinhs)
                    {
                        tbl_QuyTrinh tbQT = db.tbl_QuyTrinh.Find(itemSP.ID);
                        tbQT.NgayBatDau_DK = itemSP.NgayBatDau_DK;
                        tbQT.NgayKetThuc_DK = itemSP.NgayKetThuc_DK;
                        db.Entry(tbQT).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                }
            }
            if (donHang.action == 7)
            {
                foreach (var item in donHang.BaoGiaTemView.BaoGiaTemDetailViews)
                {
                    foreach (var itemSP in item.QuyTrinhs)
                    {
                        tbl_QuyTrinh tbQT = db.tbl_QuyTrinh.Find(itemSP.ID);
                        tbQT.NgayBatDau_TT = itemSP.NgayBatDau_TT;
                        tbQT.NgayKetThuc_TT = itemSP.NgayKetThuc_TT;
                        db.Entry(tbQT).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                }
            }
            DonHangView d = new DonHangView();
            d.customer_id = tbl_OrderTem.customer_id;
            d.code = tbl_OrderTem.code;
            d.date_begin = tbl_OrderTem.date_begin;
            d.date_end = tbl_OrderTem.date_end;
            d.id = tbl_OrderTem.id;
            d.status = tbl_OrderTem.status;
            d.date_begin_plan = tbl_OrderTem.date_begin_plan;
            d.date_end_plan = tbl_OrderTem.date_end_plan;
            var queryBaoGia = from u in db.tbl_OrderTem_BaoGia where u.order_id.Value.Equals(tbl_OrderTem.id) orderby u.id descending select u;
            List<tbl_OrderTem_BaoGia> lisBG = queryBaoGia.ToList<tbl_OrderTem_BaoGia>();
            List<BaoGiaTemView> lisBGTem = new List<BaoGiaTemView>();

            if (lisBG.Count > 1)
            {
                for (int i = 1; i < lisBG.Count; i++)
                {
                    var item = lisBG[i];
                    BaoGiaTemView temBG = new BaoGiaTemView { note = item.note, date_begin = item.date_begin, date_end = item.date_end, id = item.id, order_id = item.order_id, status = item.status, total_money = item.total_money };
                    var queryGiaoGiaCT = from u in db.tbl_OrderTem_BaoGia_Detail
                                         join y in db.tbl_Products on u.sanpam_id equals y.ID_Products
                                         where u.baogia_id.Value.Equals(temBG.id)
                                         select new BaoGiaTemDetailView { id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
                    temBG.BaoGiaTemDetailViews = queryGiaoGiaCT.ToList<BaoGiaTemDetailView>();
                    lisBGTem.Add(temBG);
                }
            }

            d.BaoGiaTemViews = lisBGTem;
            queryBaoGia = from u in db.tbl_OrderTem_BaoGia where u.order_id.Value.Equals(tbl_OrderTem.id) orderby u.id descending select u;
            lisBG = queryBaoGia.ToList<tbl_OrderTem_BaoGia>();
            foreach (var item in lisBG)
            {
                BaoGiaTemView temBG = new BaoGiaTemView { note = item.note, date_begin = item.date_begin, date_end = item.date_end, id = item.id, order_id = item.order_id, status = item.status, total_money = item.total_money };
                var queryGiaoGiaCT = from u in db.tbl_OrderTem_BaoGia_Detail
                                     join y in db.tbl_Products on u.sanpam_id equals y.ID_Products
                                     where u.baogia_id.Value.Equals(temBG.id)
                                     select new BaoGiaTemDetailView { id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
                temBG.BaoGiaTemDetailViews = queryGiaoGiaCT.ToList<BaoGiaTemDetailView>();
                d.BaoGiaTemView = temBG;
                foreach (var itemSP in d.BaoGiaTemView.BaoGiaTemDetailViews)
                {
                    var queryQT = from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(itemSP.id) orderby u.ThuTu ascending select u;
                    itemSP.QuyTrinhs = queryQT.ToList<tbl_QuyTrinh>();
                }
                break;
            }
            var list = from tt in db.tbl_Customers where tt.IDCustomers == d.customer_id select tt;
            d.Customer = list.ToList()[0];
            return View(d);
        }
    }
}