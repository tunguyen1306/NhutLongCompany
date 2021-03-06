﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NhutLongCompany.Models;
using NhutLongCompany.Attribute;

namespace NhutLongCompany.Controllers
{
    /*[RedirectOnError]*/
    public class SanXuatController : Controller
    {
        private NhutLongCompanyEntities db = new NhutLongCompanyEntities();
        // GET: SanXuat

        // [ActionAuthorizeAttribute("SanXuat")]
        public ActionResult Index()
        {


            var qr = (from data in db.tbl_OrderTem
                      join cus in db.tbl_Customers on data.customer_id equals cus.IDCustomers
                      where data.status == 1
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

            return View(qr.ToList());
        }

        // [ActionAuthorizeAttribute("SanXuat")]
        public ActionResult LichSanXuat()
        {


            return View();
        }

        // [ActionAuthorizeAttribute("SanXuat")]
        public ActionResult LichSanXuatOnDay()
        {


            return View();
        }

        //  [ActionAuthorizeAttribute("SanXuat")]
        public PartialViewResult PartialLichSanXuat()
        {
            var querySanPhamSanXuat = from a in db.tbl_OrderTem
                                      join b in db.tbl_OrderTem_BaoGia on a.id equals b.order_id.Value
                                      join u in db.tbl_OrderTem_BaoGia_Detail on b.id equals u.baogia_id.Value
                                      join y in db.tbl_Products on u.sanpam_id.Value equals y.ID_Products
                                      join z in db.tbl_Stack on u.id equals z.baoGia_detail_id
                                      where (a.status.Value == 3 || a.status.Value == 1) && b.status.Value == 1 && (u.status == 1 || u.status == 0)
                                      orderby z.index_view ascending
                                      select new BaoGiaTemDetailView { idOrderTem = a.id, pause = a.status_pause, Status_Pause = u.status_pause, Step_Flow = u.step_index, Code_Detail = u.code_detail, Status = u.status, Date_Working = u.date_working, Index_View = z.index_view, Timer = 0, date_deliver = a.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
            var list = querySanPhamSanXuat.ToList();
            foreach (var item in list)
            {
                if (item.Step_Flow.HasValue)
                {
                    var queryQT = from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(item.id) && u.ThuTu.Value.Equals(item.Step_Flow.Value) orderby u.ThuTu ascending select u;
                    item.QuyTrinhs = queryQT.ToList<tbl_QuyTrinh>();
                }
                var query = (from a in db.tbl_FlowPauseTime where a.baoGia_detail_id.Value.Equals(item.id) && a.status == 1 orderby a.id descending select a);
                var listFlowPause = query.ToList();
                if (listFlowPause.Count > 0)
                {
                    item.Current_FlowPauseTime = listFlowPause;
                }
            }
            return PartialView(list);
        }

        // [ActionAuthorizeAttribute("SanXuat")]
        public PartialViewResult PartialLichSanXuatOnDay()
        {
            var querySanPhamSanXuat = (from a in db.tbl_OrderTem
                                       join b in db.tbl_OrderTem_BaoGia on a.id equals b.order_id.Value
                                       join u in db.tbl_OrderTem_BaoGia_Detail on b.id equals u.baogia_id.Value
                                       join y in db.tbl_Products on u.sanpam_id.Value equals y.ID_Products
                                       join z in db.tbl_Stack on u.id equals z.baoGia_detail_id
                                       where (a.status.Value == 3 || a.status.Value == 1) && b.status.Value == 1 && (u.status == 1 || u.status == 0)
                                       orderby z.index_view ascending
                                       select new BaoGiaTemDetailView { idOrderTem = a.id, pause = a.status_pause, Status_Pause = u.status_pause, Step_Flow = u.step_index, Code_Detail = u.code_detail, Status = u.status, Date_Working = u.date_working, Index_View = z.index_view, Timer = 0, date_deliver = a.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts }).Take(5);
            var list = querySanPhamSanXuat.ToList();
            foreach (var item in list)
            {
                if (item.Step_Flow.HasValue)
                {
                    var queryQT = from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(item.id) && u.ThuTu.Value.Equals(item.Step_Flow.Value) orderby u.ThuTu ascending select u;
                    item.QuyTrinhs = queryQT.ToList<tbl_QuyTrinh>();
                }
                var query = (from a in db.tbl_FlowPauseTime where a.baoGia_detail_id.Value.Equals(item.id) && a.status == 1 orderby a.id descending select a);
                var listFlowPause = query.ToList();
                if (listFlowPause.Count > 0)
                {
                    item.Current_FlowPauseTime = listFlowPause;
                }
            }
            return PartialView(list);
        }

        [ActionAuthorizeAttribute("SanXuat")]
        [HttpPost]
        public ActionResult LichSanXuat(int? id)
        {

            if (id.HasValue)
            {
                tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail = db.tbl_OrderTem_BaoGia_Detail.Find(id.Value);
                tbl_OrderTem_BaoGia_Detail.status = 1;
                tbl_OrderTem_BaoGia_Detail.date_working = DateTime.Now;
                db.Entry(tbl_OrderTem_BaoGia_Detail).State = EntityState.Modified;
                db.SaveChanges();

            }
            var querySanPhamSanXuat = from a in db.tbl_OrderTem
                                      join b in db.tbl_OrderTem_BaoGia on a.id equals b.order_id.Value
                                      join u in db.tbl_OrderTem_BaoGia_Detail on b.id equals u.baogia_id.Value
                                      join y in db.tbl_Products on u.sanpam_id.Value equals y.ID_Products
                                      join z in db.tbl_Stack on u.id equals z.baoGia_detail_id
                                      where a.status.Value == 3 && b.status.Value == 1 && u.status == 0
                                      orderby z.index_view ascending
                                      select new BaoGiaTemDetailView {
                                          idOrderTem=a.id,
                                          Step_Flow = u.step_index, Code_Detail = u.code_detail, Status = u.status, Date_Working = u.date_working, Index_View = z.index_view, Timer = 0, date_deliver = a.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
            var list = querySanPhamSanXuat.ToList();
            foreach (var item in list)
            {
                if (item.Step_Flow.HasValue)
                {
                    var queryQT = from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(item.id) && u.ThuTu.Value.Equals(item.Step_Flow.Value) orderby u.ThuTu ascending select u;
                    item.QuyTrinhs = queryQT.ToList<tbl_QuyTrinh>();
                }
            }
            return View(list);
        }

        [ActionAuthorizeAttribute("SanXuat")]
        [HttpPost]
        public ActionResult LichSanXuatOnDay(int? id)
        {

            if (id.HasValue)
            {
                tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail = db.tbl_OrderTem_BaoGia_Detail.Find(id.Value);
                tbl_OrderTem_BaoGia_Detail.status = 1;
                tbl_OrderTem_BaoGia_Detail.date_working = DateTime.Now;
                db.Entry(tbl_OrderTem_BaoGia_Detail).State = EntityState.Modified;
                db.SaveChanges();

            }
            var querySanPhamSanXuat = (from a in db.tbl_OrderTem
                                       join b in db.tbl_OrderTem_BaoGia on a.id equals b.order_id.Value
                                       join u in db.tbl_OrderTem_BaoGia_Detail on b.id equals u.baogia_id.Value
                                       join y in db.tbl_Products on u.sanpam_id.Value equals y.ID_Products
                                       join z in db.tbl_Stack on u.id equals z.baoGia_detail_id
                                       where a.status.Value == 3 && b.status.Value == 1 && u.status == 0
                                       orderby z.index_view ascending
                                       select new BaoGiaTemDetailView { Step_Flow = u.step_index, Code_Detail = u.code_detail, Status = u.status, Date_Working = u.date_working, Index_View = z.index_view, Timer = 0, date_deliver = a.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts }).Take(5);
            var list = querySanPhamSanXuat.ToList();
            foreach (var item in list)
            {
                if (item.Step_Flow.HasValue)
                {
                    var queryQT = from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(item.id) && u.ThuTu.Value.Equals(item.Step_Flow.Value) orderby u.ThuTu ascending select u;
                    item.QuyTrinhs = queryQT.ToList<tbl_QuyTrinh>();
                }
            }
            return View(list);
        }

        public ActionResult IndexSX()
        {


            var qr = (from data in db.tbl_OrderTem
                      join cus in db.tbl_Customers on data.customer_id equals cus.IDCustomers
                      where data.status == 3
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

            return View(qr.ToList());
        }

        // [ActionAuthorizeAttribute("SanXuat")]
        public ActionResult IndexSanXuat()
        {

            return View();
        }

        // [ActionAuthorizeAttribute("SanXuat")]
        public PartialViewResult IndexSanXuatByDate(DateTime? date, int? update)
        {

            var querySanPhamSanXuat = from a in db.tbl_OrderTem
                                      join b in db.tbl_OrderTem_BaoGia on a.id equals b.order_id.Value
                                      join u in db.tbl_OrderTem_BaoGia_Detail on b.id equals u.baogia_id.Value
                                      join y in db.tbl_Products on u.sanpam_id.Value equals y.ID_Products
                                      join z in db.tbl_Stack on u.id equals z.baoGia_detail_id
                                      where (a.status.Value == 3 || a.status.Value == 1 || a.status.Value == 4) && b.status.Value == 1 && u.status == 1 && u.date_working.Value <= DateTime.Now
                                      orderby z.index_view ascending
                                      select new BaoGiaTemDetailView { order_code = a.code, address_deliver = a.address_deliver, pause = a.status_pause, Step_Flow = u.step_index, Status_Pause = u.status_pause, Code_Detail = u.code_detail, Status = u.status, Date_Working = u.date_working, Index_View = z.index_view, Timer = 0, date_deliver = a.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };

            if (date.HasValue)
            {
                querySanPhamSanXuat = from a in db.tbl_OrderTem
                                      join b in db.tbl_OrderTem_BaoGia on a.id equals b.order_id.Value
                                      join u in db.tbl_OrderTem_BaoGia_Detail on b.id equals u.baogia_id.Value
                                      join y in db.tbl_Products on u.sanpam_id.Value equals y.ID_Products
                                      join z in db.tbl_Stack on u.id equals z.baoGia_detail_id
                                      where (a.status.Value == 3 || a.status.Value == 1 || a.status.Value == 4) && b.status.Value == 1 && u.status == 1 && u.date_working.Value == date.Value
                                      orderby z.index_view ascending
                                      select new BaoGiaTemDetailView { order_code = a.code, address_deliver = a.address_deliver, pause = a.status_pause, Step_Flow = u.step_index, Status_Pause = u.status_pause, Code_Detail = u.code_detail, Status = u.status, Date_Working = u.date_working, Index_View = z.index_view, Timer = 0, date_deliver = a.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };


            }
            List<BaoGiaTemDetailView> list = querySanPhamSanXuat.ToList();
            foreach (var item in list)
            {
                item.Timer = (int)DateTime.Parse(item.date_deliver.Value.ToShortDateString()).Subtract(DateTime.Parse(DateTime.Now.ToShortDateString())).TotalDays;
                var queryQT = from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(item.id) && u.ThuTu < 10 orderby u.ThuTu ascending select u;
                item.QuyTrinhs = queryQT.ToList<tbl_QuyTrinh>();
                var query = (from a in db.tbl_FlowPauseTime where a.baoGia_detail_id.Value.Equals(item.id) && a.status == 1 orderby a.id descending select a);
                var listFlowPause = query.ToList();
                if (listFlowPause.Count > 0)
                {
                    item.Current_FlowPauseTime = listFlowPause;
                }

            }
            // list.Sort(new CoordinatesBasedComparer());

            ViewData["Update"] = update;
            return PartialView(list);
        }

        [ActionAuthorizeAttribute("SanXuat")]
        public ActionResult Edit(int? id)
        {
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
            d.address_deliver = tbl_OrderTem.address_deliver;
            d.date_deliver = tbl_OrderTem.date_deliver;
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
                    BaoGiaTemView temBG = new BaoGiaTemView { commission = item.commission, commission_money = item.commission_monney, note = item.note, date_begin = item.date_begin, date_end = item.date_end, id = item.id, order_id = item.order_id, status = item.status, total_money = item.total_money };
                    var queryGiaoGiaCT = from u in db.tbl_OrderTem_BaoGia_Detail
                                         join y in db.tbl_Products on u.sanpam_id equals y.ID_Products
                                         where u.baogia_id.Value.Equals(temBG.id)
                                         select new BaoGiaTemDetailView { Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
                    temBG.BaoGiaTemDetailViews = queryGiaoGiaCT.ToList<BaoGiaTemDetailView>();
                    lisBGTem.Add(temBG);
                }
            }

            d.BaoGiaTemViews = lisBGTem;
            queryBaoGia = from u in db.tbl_OrderTem_BaoGia where u.order_id.Value.Equals(tbl_OrderTem.id) orderby u.id descending select u;
            lisBG = queryBaoGia.ToList<tbl_OrderTem_BaoGia>();
            foreach (var item in lisBG)
            {
                BaoGiaTemView temBG = new BaoGiaTemView { commission = item.commission, commission_money = item.commission_monney, note = item.note, date_begin = item.date_begin, date_end = item.date_end, id = item.id, order_id = item.order_id, status = item.status, total_money = item.total_money };
                var queryGiaoGiaCT = from u in db.tbl_OrderTem_BaoGia_Detail
                                     join y in db.tbl_Products on u.sanpam_id equals y.ID_Products
                                     where u.baogia_id.Value.Equals(temBG.id)
                                     select new BaoGiaTemDetailView { Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
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

        [ActionAuthorizeAttribute("SanXuat")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DonHangView donHang)
        {

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
                    var q = from a in db.tbl_OrderTem_BaoGia_Detail where a.baogia_id.Value == donHang.BaoGiaTemView.id select a;
                    List<tbl_OrderTem_BaoGia_Detail> listDetail = q.ToList();
                    var queryMax = (from u in db.tbl_Stack
                                    orderby u.index_view descending
                                    select u).Take(1);
                    int maxStackView = queryMax.ToList().Count == 0 ? 1 : queryMax.ToList()[0].index_view.Value;

                    for (int i = 0; i < listDetail.Count; i++)
                    {
                        var item = listDetail[i];
                        // item.status = 1;
                        // item.date_working = donHang.date_begin;
                        db.Entry(item).State = EntityState.Modified;
                        tbl_Stack tbl_Stack = new tbl_Stack { baoGia_detail_id = item.id, date_create = DateTime.Now, index_view = maxStackView + i + 1 };
                        db.tbl_Stack.Add(tbl_Stack);

                    }
                    //  order.date_begin = donHang.date_begin;
                }
                if (donHang.status.Value == 4)
                {
                    order.date_end = DateTime.Now;
                }
                order.status = donHang.status.Value;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("LichSanXuat", "SanXuat");

            }
            /* if (donHang.action == 6)
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
             }*/
            /* if (donHang.action == 7)
             {
                 foreach (var item in donHang.BaoGiaTemView.BaoGiaTemDetailViews)
                 {
                     int step = 0;
                     int idDetail = 0;
                     foreach (var itemSP in item.QuyTrinhs)
                     {
                         tbl_QuyTrinh tbQT = db.tbl_QuyTrinh.Find(itemSP.ID);
                         idDetail = tbQT.ID_BaoGiaDetail;
                         tbQT.NgayBatDau_TT = itemSP.NgayBatDau_TT;
                         tbQT.NgayKetThuc_TT = itemSP.NgayKetThuc_TT;
                         if (tbQT.NgayKetThuc_TT.HasValue && step<= tbQT.ThuTu.Value)
                         {
                             step = tbQT.ThuTu.Value;

                         }
                         db.Entry(tbQT).State = EntityState.Modified;
                     }
                     tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail=db.tbl_OrderTem_BaoGia_Detail.Find(idDetail);
                     if (tbl_OrderTem_BaoGia_Detail!=null)
                     {
                         if (!tbl_OrderTem_BaoGia_Detail.step_index.HasValue)
                         {
                             tbl_OrderTem_BaoGia_Detail.step_index = step;
                             if (tbl_OrderTem_BaoGia_Detail.step_index.Value == 8)
                             {
                                 tbl_OrderTem_BaoGia_Detail.status = 2;
                             }

                         }
                         else if (tbl_OrderTem_BaoGia_Detail.step_index.Value <= step)
                         {
                             tbl_OrderTem_BaoGia_Detail.step_index = step;
                             if (tbl_OrderTem_BaoGia_Detail.step_index.Value == 8)
                             {
                                 tbl_OrderTem_BaoGia_Detail.status = 2;
                             }
                         }
                         db.Entry(tbl_OrderTem_BaoGia_Detail).State = EntityState.Modified;
                     }                    

                     db.SaveChanges();
                 }
             }*/
            DonHangView d = new DonHangView();
            d.address_deliver = tbl_OrderTem.address_deliver;
            d.date_deliver = tbl_OrderTem.date_deliver;
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
                    BaoGiaTemView temBG = new BaoGiaTemView { commission = item.commission, commission_money = item.commission_monney, note = item.note, date_begin = item.date_begin, date_end = item.date_end, id = item.id, order_id = item.order_id, status = item.status, total_money = item.total_money };
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
                BaoGiaTemView temBG = new BaoGiaTemView { commission = item.commission, commission_money = item.commission_monney, note = item.note, date_begin = item.date_begin, date_end = item.date_end, id = item.id, order_id = item.order_id, status = item.status, total_money = item.total_money };
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

        [ActionAuthorizeAttribute("SanXuat")]
        [HttpPost]
        public PartialViewResult UpdateFlowProduct(int id, int idflow, int status)
        {
            tbl_QuyTrinh tbQT = db.tbl_QuyTrinh.Find(idflow);
            if (status == 1)
            {
                tbQT.NgayBatDau_TT = DateTime.Now;
            }
            if (status == 2)
            {
                tbQT.NgayKetThuc_TT = DateTime.Now;
                var queryPrev = from a in db.tbl_QuyTrinh where a.ID_BaoGiaDetail == tbQT.ID_BaoGiaDetail && a.ThuTu.Value <= tbQT.ThuTu.Value && a.TrangThai != 2 && tbQT.ID != idflow select a;
                if (queryPrev.ToList().Count == 0)
                {
                    var queryNext = from a in db.tbl_QuyTrinh where a.ID_BaoGiaDetail == tbQT.ID_BaoGiaDetail && a.ThuTu.Value > tbQT.ThuTu.Value select a;
                    var listNext = queryNext.ToList<tbl_QuyTrinh>();
                    for (int i = 0; i < listNext.Count; i++)
                    {
                        if (listNext[i].ThucHien.Value == 1)
                        {
                            break;
                        }
                        listNext[i].NgayBatDau_TT = DateTime.Now;
                        listNext[i].NgayKetThuc_TT = DateTime.Now;
                        listNext[i].TrangThai = 2;
                        db.Entry(listNext[i]).State = EntityState.Modified;
                    }
                }

            }
            tbQT.TrangThai = status;
            if (tbQT.ThucHien == 0)
            {
                tbQT.NgayBatDau_TT = DateTime.Now;
                tbQT.NgayKetThuc_TT = DateTime.Now;
            }
            db.Entry(tbQT).State = EntityState.Modified;
            db.SaveChanges();
            List<tbl_QuyTrinh> listData = db.tbl_QuyTrinh.Where(T => T.ID_BaoGiaDetail == tbQT.ID_BaoGiaDetail && T.ThuTu <= 9 && T.TrangThai != 2).ToList();
            tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail = db.tbl_OrderTem_BaoGia_Detail.Find(id);
            if (listData.Count == 0)
            {
                if (tbQT.ThucHien == 1)
                {
                    tbl_OrderTem_BaoGia_Detail.step_index = tbl_OrderTem_BaoGia_Detail.step_index > tbQT.ThuTu ? tbl_OrderTem_BaoGia_Detail.step_index : tbQT.ThuTu;
                }
                if (status == 2)
                {
                    if (listData.Count == 0)
                    {
                        tbl_Stack tbl_Stack = db.tbl_Stack.Find(tbl_OrderTem_BaoGia_Detail.id);
                        db.Entry(tbl_Stack).State = EntityState.Deleted;
                        tbl_OrderTem_BaoGia_Detail.status = 2;
                    }


                }

                db.Entry(tbl_OrderTem_BaoGia_Detail).State = EntityState.Modified;

            }
            db.SaveChanges();
            tbl_OrderTem_BaoGia bg = db.tbl_OrderTem_BaoGia.Find(tbl_OrderTem_BaoGia_Detail.baogia_id);
            tbl_OrderTem order = db.tbl_OrderTem.Find(bg.order_id);
            var queryQT = (from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(id) orderby u.ThuTu ascending select u).Take(1);
            List<tbl_QuyTrinh> listAT = queryQT.ToList();
            int indexBG = 0;
            if (listAT.Count > 0)
            {
                indexBG = listAT[0].ThuTu.Value;
            }
            if (tbQT.ThuTu.Value == indexBG)
            {

                order.status = 3;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }
            if (listData.Count == 0 && status == 2)
            {
                order.status = 4;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }
            if (tbQT.ThuTu.Value == 12 && status == 2)
            {
                order.status = 5;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }
            return PartialView(tbl_OrderTem_BaoGia_Detail);
        }

        [ActionAuthorizeAttribute("SanXuat")]
        [HttpPost]
        public PartialViewResult UpdateFlow(int idflow, String begin, String end, String dataPause, int type = 0)
        {



            DateTime? _begin = null;
            DateTime? _end = null;
            if (begin != null && begin.Trim().Length > 0)
            {
                _begin = DateTime.ParseExact(begin, "MM/dd/yyyy HH:mm:ss", new CultureInfo("en"));
            }
            if (end != null && end.Trim().Length > 0)
            {
                _end = DateTime.ParseExact(end, "MM/dd/yyyy HH:mm:ss", new CultureInfo("en"));
            }

            tbl_QuyTrinh tbQT = db.tbl_QuyTrinh.Find(idflow);

            tbQT.NgayBatDau_TT = _begin;
            tbQT.NgayKetThuc_TT = _end;

            if (_begin.HasValue)
            {
                tbQT.TrangThai = 1;

            }
            if (_end.HasValue)
            {
                tbQT.TrangThai = 2;

                var queryNext = from a in db.tbl_QuyTrinh where a.ID_BaoGiaDetail == tbQT.ID_BaoGiaDetail && a.ThuTu.Value > tbQT.ThuTu.Value select a;
                var listNext = queryNext.ToList<tbl_QuyTrinh>();
                for (int i = 0; i < listNext.Count; i++)
                {
                    if (listNext[i].ThucHien.Value == 1)
                    {
                        break;
                    }
                    listNext[i].NgayBatDau_TT = DateTime.Now;
                    listNext[i].NgayKetThuc_TT = DateTime.Now;
                    listNext[i].TrangThai = 2;
                    db.Entry(listNext[i]).State = EntityState.Modified;
                }

            }
            if (tbQT.ThucHien == 0)
            {
                tbQT.NgayBatDau_TT = DateTime.Now;
                tbQT.NgayKetThuc_TT = DateTime.Now;
            }
            db.Entry(tbQT).State = EntityState.Modified;
            db.SaveChanges();

            List<tbl_QuyTrinh> listData = db.tbl_QuyTrinh.Where(T => T.ID_BaoGiaDetail == tbQT.ID_BaoGiaDetail && T.ThuTu <= 9 && T.TrangThai != 2).ToList();
            tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail = db.tbl_OrderTem_BaoGia_Detail.Find(tbQT.ID_BaoGiaDetail);
            if (listData.Count == 0)
            {
                if (tbQT.ThucHien == 1)
                {
                    tbl_OrderTem_BaoGia_Detail.step_index = tbl_OrderTem_BaoGia_Detail.step_index > tbQT.ThuTu ? tbl_OrderTem_BaoGia_Detail.step_index : tbQT.ThuTu;
                }
                if (tbQT.TrangThai == 2)
                {
                    if (listData.Count == 0)
                    {
                        tbl_Stack tbl_Stack = db.tbl_Stack.Find(tbl_OrderTem_BaoGia_Detail.id);
                        if (tbl_Stack != null)
                        {
                            db.Entry(tbl_Stack).State = EntityState.Deleted;
                            tbl_OrderTem_BaoGia_Detail.status = 2;
                        }

                    }

                }

                db.Entry(tbl_OrderTem_BaoGia_Detail).State = EntityState.Modified;

            }
            db.SaveChanges();
            tbl_OrderTem_BaoGia bg = db.tbl_OrderTem_BaoGia.Find(tbl_OrderTem_BaoGia_Detail.baogia_id);
            tbl_OrderTem order = db.tbl_OrderTem.Find(bg.order_id);
            var queryQT = (from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(tbQT.ID_BaoGiaDetail) orderby u.ThuTu ascending select u).Take(1);
            List<tbl_QuyTrinh> listAT = queryQT.ToList();
            int indexBG = 0;
            if (listAT.Count > 0)
            {
                indexBG = listAT[0].ThuTu.Value;
            }
            if (tbQT.ThuTu.Value == indexBG)
            {

                order.status = 3;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }
            if (listData.Count == 0 && tbQT.TrangThai == 2)
            {
                order.status = 4;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }
            if (tbQT.ThuTu.Value == 12 && tbQT.TrangThai == 2)
            {
                order.status = 5;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }
            try
            {
                if (dataPause != null)
                {
                    String[] array = dataPause.Split('|');
                    foreach (var str in array)
                    {
                        if (str.Trim().Length > 0)
                        {
                            String[] flowPauseData = str.Split(';');
                            int idPasue = int.Parse(flowPauseData[0]);
                            int statusPause = int.Parse(flowPauseData[1]);
                            String dateBeginPasue = flowPauseData[2];
                            String dateEndPasue = flowPauseData[3];
                            DateTime? beginP = null;
                            DateTime? endP = null;
                            int stautsRemmove = int.Parse(flowPauseData[4]);
                            if (dateBeginPasue.Trim().Length > 0)
                            {
                                beginP = DateTime.ParseExact(dateBeginPasue.Trim(), "MM/dd/yyyy HH:mm:ss", new CultureInfo("en"));
                            }
                            if (dateEndPasue.Trim().Length > 0)
                            {
                                endP = DateTime.ParseExact(dateEndPasue.Trim(), "MM/dd/yyyy HH:mm:ss", new CultureInfo("en"));
                            }
                            tbl_FlowPauseTime itemPause = db.tbl_FlowPauseTime.Find(idPasue);
                            if (itemPause != null)
                            {
                                itemPause.date_begin = beginP;
                                itemPause.date_end = endP;
                            }
                            else
                            {
                                itemPause = new tbl_FlowPauseTime();
                                itemPause.id = 0;
                                itemPause.date_begin = beginP;
                                itemPause.date_end = endP;
                                itemPause.status = 1;
                                itemPause.id_flow = idflow;
                            }
                            if (beginP != null && endP != null)
                            {
                                itemPause.status = 2;
                            }
                            else
                            {
                                itemPause.status = 1;
                            }
                            if (itemPause.status == 1 && tbQT.TrangThai == 2)
                            {
                                itemPause.status = 2;
                                itemPause.date_end = tbQT.NgayKetThuc_TT;
                            }
                            if (stautsRemmove == 1 && itemPause != null)
                            {
                                db.Entry(itemPause).State = EntityState.Deleted;
                            }
                            else
                            {
                                if (itemPause != null && itemPause.id > 0)
                                {
                                    db.Entry(itemPause).State = EntityState.Modified;
                                }
                                else
                                {
                                    db.Entry(itemPause).State = EntityState.Added;
                                }
                            }

                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {

            }
            ViewData["type"] = type;
            return PartialView(tbl_OrderTem_BaoGia_Detail);
        }

        [ActionAuthorizeAttribute("SanXuat")]
        public PartialViewResult ViewFlowProduct(int id)
        {
            BaoGiaTemDetailView bgdt = new BaoGiaTemDetailView();
            var queryGiaoGiaCT = from a in db.tbl_OrderTem
                                 join b in db.tbl_OrderTem_BaoGia on a.id equals b.order_id.Value
                                 join u in db.tbl_OrderTem_BaoGia_Detail on b.id equals u.baogia_id.Value
                                 join y in db.tbl_Products on u.sanpam_id equals y.ID_Products
                                 where u.id.Equals(id)
                                 select new BaoGiaTemDetailView { order_code = a.code, Date_Working = u.date_working, Code_Detail = u.code_detail, Step_Flow = u.step_index, Status = u.status, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
            List<BaoGiaTemDetailView> listData = queryGiaoGiaCT.ToList<BaoGiaTemDetailView>();
            foreach (var itemSP in listData)
            {
                var queryQT = from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(itemSP.id) && u.ThuTu < 10 orderby u.ThuTu ascending select u;
                itemSP.QuyTrinhs = queryQT.ToList<tbl_QuyTrinh>();
                /* var query = (from a in db.tbl_FlowPauseTime where a.baoGia_detail_id.Value.Equals(itemSP.id) && a.status == 1 orderby a.id descending select a);
                 var listFlowPause = query.ToList();
                 if (listFlowPause.Count > 0)
                 {
                     itemSP.Current_FlowPauseTime = listFlowPause;
                 }*/
            }


            return PartialView(listData);
        }

        [ActionAuthorizeAttribute("SanXuat")]
        [HttpPost]
        public JsonResult UpdateStack(int id, int order)
        {
            try
            {
                tbl_Stack tbl_Stack = db.tbl_Stack.Find(id);
                tbl_Stack.index_view = order;
                db.Entry(tbl_Stack).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {

                return Json("Error - " + e.Message);
            }

            return Json("Success");
        }

        [ActionAuthorizeAttribute("SanXuat")]
        [HttpPost]
        public JsonResult UpdateStateFlow(int id, int idflow, String note, int state)
        {
            try
            {
                if (state == 2)
                {
                    var query = (from a in db.tbl_FlowPauseTime where a.baoGia_detail_id.Value == id && a.id_flow == idflow orderby a.id descending select a).Take(1);
                    var list = query.ToList();
                    if (list.Count > 0)
                    {
                        var cItem = list[0];
                        cItem.date_end = DateTime.Now;
                        cItem.status = state;
                        db.Entry(cItem).State = EntityState.Modified;
                        tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail = db.tbl_OrderTem_BaoGia_Detail.Find(id);
                        tbl_OrderTem_BaoGia_Detail.status_pause = 0;
                        db.Entry(tbl_OrderTem_BaoGia_Detail).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                if (state == 1)
                {
                    tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail = db.tbl_OrderTem_BaoGia_Detail.Find(id);
                    tbl_OrderTem_BaoGia_Detail.status_pause = 1;
                    db.Entry(tbl_OrderTem_BaoGia_Detail).State = EntityState.Modified;
                    tbl_FlowPauseTime citem = new tbl_FlowPauseTime { id_flow = idflow, baoGia_detail_id = id, date_begin = DateTime.Now, index_step = tbl_OrderTem_BaoGia_Detail.step_index, note = note, status = 1 };

                    db.tbl_FlowPauseTime.Add(citem);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

                return Json("Error - " + e.Message);
            }

            return Json("Success");
        }

        [ActionAuthorizeAttribute("SanXuat")]
        [HttpPost]
        public JsonResult UpdateLenhSanXuat(int id)
        {
            try
            {
                tbl_OrderTem item = (from a in db.tbl_OrderTem
                                     join b in db.tbl_OrderTem_BaoGia on a.id equals b.order_id
                                     join c in db.tbl_OrderTem_BaoGia_Detail on b.id equals c.baogia_id
                                     where c.id == id
                                     select a).FirstOrDefault();
                item.status = 3;
                db.Entry(item).State = EntityState.Modified;



                tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail = db.tbl_OrderTem_BaoGia_Detail.Find(id);
                tbl_OrderTem_BaoGia_Detail.status = 1;
                tbl_OrderTem_BaoGia_Detail.date_working = DateTime.Now;
                db.Entry(tbl_OrderTem_BaoGia_Detail).State = EntityState.Modified;

                var queryNext = from a in db.tbl_QuyTrinh where a.ID_BaoGiaDetail == tbl_OrderTem_BaoGia_Detail.id && a.ThuTu.Value >= 0 select a;
                var listNext = queryNext.ToList<tbl_QuyTrinh>();
                for (int i = 0; i < listNext.Count; i++)
                {
                    if (listNext[i].ThucHien.Value == 1)
                    {
                        //listNext[i].NgayBatDau_TT = DateTime.Now;
                        //listNext[i].TrangThai = 1;
                        //db.Entry(listNext[i]).State = EntityState.Modified;
                        //tbl_OrderTem_BaoGia_Detail.step_index = listNext[i].ThuTu;
                        //db.Entry(tbl_OrderTem_BaoGia_Detail).State = EntityState.Modified;

                        break;
                    }
                    listNext[i].NgayBatDau_TT = DateTime.Now;
                    listNext[i].NgayKetThuc_TT = DateTime.Now;
                    listNext[i].TrangThai = 2;
                    db.Entry(listNext[i]).State = EntityState.Modified;
                }

                db.SaveChanges();
            }
            catch (Exception e)
            {

                return Json("Error - " + e.Message);
            }

            return Json("Success");
        }

        public ActionResult EditFlow(int? id)
        {

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
            d.address_deliver = tbl_OrderTem.address_deliver;
            d.date_deliver = tbl_OrderTem.date_deliver;
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
                    BaoGiaTemView temBG = new BaoGiaTemView { commission = item.commission, commission_money = item.commission_monney, note = item.note, date_begin = item.date_begin, date_end = item.date_end, id = item.id, order_id = item.order_id, status = item.status, total_money = item.total_money };
                    var queryGiaoGiaCT = from u in db.tbl_OrderTem_BaoGia_Detail
                                         join y in db.tbl_Products on u.sanpam_id equals y.ID_Products
                                         where u.baogia_id.Value.Equals(temBG.id)
                                         select new BaoGiaTemDetailView { Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
                    temBG.BaoGiaTemDetailViews = queryGiaoGiaCT.ToList<BaoGiaTemDetailView>();
                    lisBGTem.Add(temBG);
                }
            }

            d.BaoGiaTemViews = lisBGTem;
            queryBaoGia = from u in db.tbl_OrderTem_BaoGia where u.order_id.Value.Equals(tbl_OrderTem.id) orderby u.id descending select u;
            lisBG = queryBaoGia.ToList<tbl_OrderTem_BaoGia>();
            foreach (var item in lisBG)
            {
                BaoGiaTemView temBG = new BaoGiaTemView { commission = item.commission, commission_money = item.commission_monney, note = item.note, date_begin = item.date_begin, date_end = item.date_end, id = item.id, order_id = item.order_id, status = item.status, total_money = item.total_money };
                var queryGiaoGiaCT = from u in db.tbl_OrderTem_BaoGia_Detail
                                     join y in db.tbl_Products on u.sanpam_id equals y.ID_Products
                                     where u.baogia_id.Value.Equals(temBG.id)
                                     select new BaoGiaTemDetailView { Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
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
        public ActionResult EditFlow(DonHangView donHang)
        {

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
                    var q = from a in db.tbl_OrderTem_BaoGia_Detail where a.baogia_id.Value == donHang.BaoGiaTemView.id select a;
                    List<tbl_OrderTem_BaoGia_Detail> listDetail = q.ToList();
                    var queryMax = (from u in db.tbl_Stack
                                    orderby u.index_view descending
                                    select u).Take(1);
                    int maxStackView = queryMax.ToList().Count == 0 ? 1 : queryMax.ToList()[0].index_view.Value;

                    for (int i = 0; i < listDetail.Count; i++)
                    {
                        var item = listDetail[i];
                        // item.status = 1;
                        // item.date_working = donHang.date_begin;
                        db.Entry(item).State = EntityState.Modified;
                        tbl_Stack tbl_Stack = new tbl_Stack { baoGia_detail_id = item.id, date_create = DateTime.Now, index_view = maxStackView + i + 1 };
                        db.tbl_Stack.Add(tbl_Stack);

                    }
                    //  order.date_begin = donHang.date_begin;
                }
                if (donHang.status.Value == 4)
                {
                    order.date_end = DateTime.Now;
                }
                order.status = donHang.status.Value;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("ThongTinDonHang", "tbl_OrderTem");

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
                    int step = 0;
                    int idDetail = 0;
                    foreach (var itemSP in item.QuyTrinhs)
                    {
                        tbl_QuyTrinh tbQT = db.tbl_QuyTrinh.Find(itemSP.ID);
                        idDetail = tbQT.ID_BaoGiaDetail;
                        tbQT.NgayBatDau_TT = itemSP.NgayBatDau_TT;
                        tbQT.NgayKetThuc_TT = itemSP.NgayKetThuc_TT;
                        if (tbQT.NgayKetThuc_TT.HasValue && step <= tbQT.ThuTu.Value)
                        {
                            step = tbQT.ThuTu.Value;

                        }
                        db.Entry(tbQT).State = EntityState.Modified;
                    }
                    tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail = db.tbl_OrderTem_BaoGia_Detail.Find(idDetail);
                    if (tbl_OrderTem_BaoGia_Detail != null)
                    {
                        if (!tbl_OrderTem_BaoGia_Detail.step_index.HasValue)
                        {
                            tbl_OrderTem_BaoGia_Detail.step_index = step;
                            if (tbl_OrderTem_BaoGia_Detail.step_index.Value == 8)
                            {
                                tbl_OrderTem_BaoGia_Detail.status = 2;
                            }

                        }
                        else if (tbl_OrderTem_BaoGia_Detail.step_index.Value <= step)
                        {
                            tbl_OrderTem_BaoGia_Detail.step_index = step;
                            if (tbl_OrderTem_BaoGia_Detail.step_index.Value == 8)
                            {
                                tbl_OrderTem_BaoGia_Detail.status = 2;
                            }
                        }
                        db.Entry(tbl_OrderTem_BaoGia_Detail).State = EntityState.Modified;
                    }

                    db.SaveChanges();
                }
            }
            DonHangView d = new DonHangView();
            d.address_deliver = tbl_OrderTem.address_deliver;
            d.date_deliver = tbl_OrderTem.date_deliver;
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
                    BaoGiaTemView temBG = new BaoGiaTemView { commission = item.commission, commission_money = item.commission_monney, note = item.note, date_begin = item.date_begin, date_end = item.date_end, id = item.id, order_id = item.order_id, status = item.status, total_money = item.total_money };
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
                BaoGiaTemView temBG = new BaoGiaTemView { commission = item.commission, commission_money = item.commission_monney, note = item.note, date_begin = item.date_begin, date_end = item.date_end, id = item.id, order_id = item.order_id, status = item.status, total_money = item.total_money };
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

        public PartialViewResult ViewPauseFlow(int? id)
        {
            //
            var listPause = db.tbl_FlowPauseTime.Where(T => T.id_flow == id).ToList();
            return PartialView(listPause);
        }
        public ActionResult InSanXuat(int id)
        {
            
            var model = (from data in db.tblPrints

                         where data.IdOrderTem == id
                         select data).Count();
         
             var querySanPhamSanXuat = from a in db.tbl_OrderTem
                                      //join k in db.tblPrints on a.id equals  k.IdOrderTem
                                      join b in db.tbl_OrderTem_BaoGia on a.id equals b.order_id.Value
                                      join u in db.tbl_OrderTem_BaoGia_Detail on b.id equals u.baogia_id.Value
                                      join y in db.tbl_Products on u.sanpam_id.Value equals y.ID_Products
                                      join z in db.tbl_Stack on u.id equals z.baoGia_detail_id
                                      join o in db.tbl_Customers on a.customer_id equals o.IDCustomers
                                   
                                      where a.id == id
                                      orderby z.index_view ascending
                                      select new BaoGiaTemDetailView
                                      {
                                          LoaiSongProducts = y.LoaiSongProducts,
                                          idOrderTem = a.id,
                                          name_customer = o.NameCustomers,
                                          order_code = a.code,
                                          address_deliver = a.address_deliver,
                                          pause = a.status_pause,
                                          Step_Flow = u.step_index,
                                          Status_Pause = u.status_pause,
                                          Code_Detail = u.code_detail,
                                          Status = u.status,
                                          Date_Working = u.date_working,
                                          Date_Begin = a.date_begin,
                                          Index_View = z.index_view,
                                          Timer = 0,
                                          date_deliver = a.date_deliver,
                                          Design = u.design,
                                          Design_Date = u.design_date,
                                          Design_Img = u.design_img,
                                          id = u.id,
                                          ID_Products = u.sanpam_id.Value,
                                          CodeProducts = y.CodeProducts,
                                          CreatedDateProducts = y.CreatedDateProducts,
                                          CreateUserProducts = y.CreateUserProducts,
                                          DanKimProducts = y.DanKimProducts,
                                          GiaProducts = u.money.Value.ToString(),
                                          LoaigiayProducts = y.LoaigiayProducts,
                                          ModifyDateProducts = y.ModifyDateProducts,
                                          ModifyUserProducts = y.ModifyUserProducts,
                                          NameProducts = y.NameProducts,
                                          OffsetFlexoProducts = y.OffsetFlexoProducts,
                                          QuyCachProducts = y.QuyCachProducts,
                                          SolopProducts = y.SolopProducts,
                                          SoLuong = u.soluong.Value,
                                          StatusProducts = y.StatusProducts,
                                          InFlexoProducts = y.InFlexoProducts,
                                      };



          
           
            return View(querySanPhamSanXuat.ToList());

        }
    }
}