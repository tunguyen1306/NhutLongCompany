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


        public ActionResult LichSanXuat()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
        
            return View();
        }
        public PartialViewResult PartialLichSanXuat()
        {
            var querySanPhamSanXuat = from a in db.tbl_OrderTem
                                      join b in db.tbl_OrderTem_BaoGia on a.id equals b.order_id.Value
                                      join u in db.tbl_OrderTem_BaoGia_Detail on b.id equals u.baogia_id.Value
                                      join y in db.tbl_Products on u.sanpam_id.Value equals y.ID_Products
                                      join z in db.tbl_Stack on u.id equals z.baoGia_detail_id
                                      where a.status.Value == 3 && b.status.Value == 1 && u.status == 1
                                      orderby z.index_view ascending
                                      select new BaoGiaTemDetailView { Status_Pause=u.status_pause, Step_Flow = u.step_index, Code_Detail = u.code_detail, Status = u.status, Date_Working = u.date_working, Index_View = z.index_view, Timer = 0, date_deliver = a.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
            var list = querySanPhamSanXuat.ToList();
            foreach (var item in list)
            {
                if (item.Step_Flow.HasValue)
                {
                    var queryQT = from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(item.id) && u.ThuTu.Value.Equals(item.Step_Flow.Value) orderby u.ThuTu ascending select u;
                    item.QuyTrinhs = queryQT.ToList<tbl_QuyTrinh>();
                }
            }
            return PartialView(list);
        }
        [HttpPost]
        public ActionResult LichSanXuat(int? id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (id.HasValue)
            {
                tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail= db.tbl_OrderTem_BaoGia_Detail.Find(id.Value);
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
                                      select new BaoGiaTemDetailView { Step_Flow = u.step_index, Code_Detail = u.code_detail, Status = u.status, Date_Working = u.date_working, Index_View = z.index_view, Timer = 0, date_deliver = a.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
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
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

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

        public ActionResult IndexSanXuat()
        {

            return View();
        }


        public PartialViewResult IndexSanXuatByDate(DateTime? date,int? update)
        {
            //if (Session["username"] == null)
            //{
            //    return RedirectToAction("Login", "Login");
            //}
            var querySanPhamSanXuat = from a in db.tbl_OrderTem
                                      join b in db.tbl_OrderTem_BaoGia on a.id equals b.order_id.Value
                                      join u in db.tbl_OrderTem_BaoGia_Detail on b.id equals u.baogia_id.Value
                                      join y in db.tbl_Products on u.sanpam_id.Value equals y.ID_Products
                                      join z in db.tbl_Stack on u.id equals z.baoGia_detail_id
                                      where a.status.Value == 3 && b.status.Value == 1 && u.status == 1 && u.date_working.Value <= DateTime.Now
                                      orderby z.index_view ascending
                                      select new BaoGiaTemDetailView { Status_Pause = u.status_pause, Code_Detail = u.code_detail,  Status = u.status, Date_Working = u.date_working, Index_View = z.index_view, Timer = 0, date_deliver = a.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };

            if (date.HasValue)
            {
                querySanPhamSanXuat = from a in db.tbl_OrderTem
                                      join b in db.tbl_OrderTem_BaoGia on a.id equals b.order_id.Value
                                      join u in db.tbl_OrderTem_BaoGia_Detail on b.id equals u.baogia_id.Value
                                      join y in db.tbl_Products on u.sanpam_id.Value equals y.ID_Products
                                      join z in db.tbl_Stack on u.id equals z.baoGia_detail_id
                                      where a.status.Value == 3 && b.status.Value == 1 && u.status == 1 && u.date_working.Value == date.Value
                                      orderby z.index_view ascending
                                      select new BaoGiaTemDetailView { Status_Pause = u.status_pause, Code_Detail = u.code_detail, Status = u.status, Date_Working = u.date_working, Index_View = z.index_view, Timer = 0, date_deliver = a.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };


            }
            List<BaoGiaTemDetailView> list = querySanPhamSanXuat.ToList();
            foreach (var item in list)
            {
                item.Timer = (int)DateTime.Parse(item.date_deliver.Value.ToShortDateString()).Subtract(DateTime.Parse(DateTime.Now.ToShortDateString())).TotalDays;
                var queryQT = from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(item.id) orderby u.ThuTu ascending select u;
                item.QuyTrinhs = queryQT.ToList<tbl_QuyTrinh>();
                var query = (from a in db.tbl_FlowPauseTime where a.baoGia_detail_id.Value.Equals(item.id) orderby a.id descending select a).Take(1);
                var listFlowPause = query.ToList();
                if (listFlowPause.Count>0)
                {
                    item.Current_FlowPauseTime = listFlowPause[0];
                }
                
            }
            // list.Sort(new CoordinatesBasedComparer());

            ViewData["Update"] = update;
            return PartialView(list);
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
            }
            tbQT.TrangThai = status;
            db.Entry(tbQT).State = EntityState.Modified;

            tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail = db.tbl_OrderTem_BaoGia_Detail.Find(id);
            if (!tbl_OrderTem_BaoGia_Detail.step_index.HasValue || tbl_OrderTem_BaoGia_Detail.step_index.Value <= tbQT.ThuTu)
            {
                tbl_OrderTem_BaoGia_Detail.step_index = tbQT.ThuTu;
                if (status == 2 && tbQT.ThuTu == 8)
                {
                    tbl_Stack tbl_Stack = db.tbl_Stack.Find(tbl_OrderTem_BaoGia_Detail.id);
                    db.Entry(tbl_Stack).State = EntityState.Deleted;
                    tbl_OrderTem_BaoGia_Detail.status = 2;

                }

                db.Entry(tbl_OrderTem_BaoGia_Detail).State = EntityState.Modified;

            }
            db.SaveChanges();

            return PartialView(tbl_OrderTem_BaoGia_Detail);
        }

        public PartialViewResult ViewFlowProduct(int id)
        {
            BaoGiaTemDetailView bgdt = new BaoGiaTemDetailView();
            var queryGiaoGiaCT = from u in db.tbl_OrderTem_BaoGia_Detail
                                 join y in db.tbl_Products on u.sanpam_id equals y.ID_Products
                                 where u.id.Equals(id)
                                 select new BaoGiaTemDetailView { Status = u.status, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
            List<BaoGiaTemDetailView> listData = queryGiaoGiaCT.ToList<BaoGiaTemDetailView>();
            foreach (var itemSP in listData)
            {
                var queryQT = from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(itemSP.id) orderby u.ThuTu ascending select u;
                itemSP.QuyTrinhs = queryQT.ToList<tbl_QuyTrinh>();
            }

            return PartialView(listData);
        }
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
        [HttpPost]
        public JsonResult UpdateStateFlow(int id, String note,int state)
        {
            try
            {
                if (state==2)
                {
                    var query = (from a in db.tbl_FlowPauseTime where a.baoGia_detail_id.Value.Equals(id) orderby a.id descending select a).Take(1);
                    var list = query.ToList();
                    if (list.Count>0)
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
                if (state==1)
                {
                    tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail = db.tbl_OrderTem_BaoGia_Detail.Find(id);
                    tbl_OrderTem_BaoGia_Detail.status_pause = 1;
                    db.Entry(tbl_OrderTem_BaoGia_Detail).State = EntityState.Modified;
                    tbl_FlowPauseTime citem = new tbl_FlowPauseTime { baoGia_detail_id = id, date_begin = DateTime.Now, index_step = tbl_OrderTem_BaoGia_Detail.step_index, note = note, status = 1 };
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

        
    }
}