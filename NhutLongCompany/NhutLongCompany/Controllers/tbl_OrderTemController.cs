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
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;

namespace NhutLongCompany.Controllers
{
    [RedirectOnError]
    public class tbl_OrderTemController : Controller
    {
        private NhutLongCompanyEntities db = new NhutLongCompanyEntities();
        //[ActionAuthorizeAttribute("DonHang")]
        public PartialViewResult IndexSanXuatByDate(DateTime? date, int? update)
        {
          
            var querySanPhamSanXuat = from a in db.tbl_OrderTem
                                      join b in db.tbl_OrderTem_BaoGia on a.id equals b.order_id.Value
                                      join u in db.tbl_OrderTem_BaoGia_Detail on b.id equals u.baogia_id.Value
                                      join y in db.tbl_Products on u.sanpam_id.Value equals y.ID_Products
                                    //  join z in db.tbl_Stack on u.id equals z.baoGia_detail_id
                                      where (a.status.Value == 3 || a.status.Value == 1 || a.status.Value == 4) && b.status.Value == 1 && u.date_working.Value <= DateTime.Now
                                     // orderby z.index_view ascending
                                      select new BaoGiaTemDetailView { order_code=a.code, address_deliver = a.address_deliver, pause =a.status_pause,  Step_Flow = u.step_index, Status_Pause = u.status_pause, Code_Detail = u.code_detail, Status = u.status, Date_Working = u.date_working, Index_View = 0, Timer = 0, date_deliver = a.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };

            if (date.HasValue)
            {
                querySanPhamSanXuat = from a in db.tbl_OrderTem
                                      join b in db.tbl_OrderTem_BaoGia on a.id equals b.order_id.Value
                                      join u in db.tbl_OrderTem_BaoGia_Detail on b.id equals u.baogia_id.Value
                                      join y in db.tbl_Products on u.sanpam_id.Value equals y.ID_Products
                                  //    join z in db.tbl_Stack on u.id equals z.baoGia_detail_id
                                      where (a.status.Value == 3 || a.status.Value == 1 || a.status.Value == 4) && b.status.Value == 1 && u.date_working.Value == date.Value
                                 //     orderby z.index_view ascending
                                      select new BaoGiaTemDetailView { order_code = a.code, address_deliver = a.address_deliver, pause = a.status_pause, Step_Flow = u.step_index, Status_Pause = u.status_pause, Code_Detail = u.code_detail, Status = u.status, Date_Working = u.date_working, Index_View =0, Timer = 0, date_deliver = a.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };


            }
            List<BaoGiaTemDetailView> list = querySanPhamSanXuat.ToList();
            foreach (var item in list)
            {
                item.Timer = item.date_deliver.HasValue ? ((int)item.date_deliver.Value.Subtract(DateTime.Parse(DateTime.Now.ToShortDateString())).TotalDays == null ? 0 :
(int)item.date_deliver.Value.Subtract(DateTime.Parse(DateTime.Now.ToShortDateString())).TotalDays) : 0;
                var queryQT = from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(item.id)  orderby u.ThuTu ascending select u;
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

        [ActionAuthorizeAttribute("DonHang")]
        public PartialViewResult IndexSanXuatByDate_DonHang(DateTime? date, int? update,int id)
        {
         
            var querySanPhamSanXuat = from a in db.tbl_OrderTem
                                      join b in db.tbl_OrderTem_BaoGia on a.id equals b.order_id.Value
                                      join u in db.tbl_OrderTem_BaoGia_Detail on b.id equals u.baogia_id.Value
                                      join y in db.tbl_Products on u.sanpam_id.Value equals y.ID_Products
                                      //  join z in db.tbl_Stack on u.id equals z.baoGia_detail_id
                                      where a.id == id && (a.status.Value == 3 || a.status.Value == 1 || a.status.Value == 4 || a.status.Value == 5) && b.status.Value == 1 && u.date_working.Value <= DateTime.Now
                                      // orderby z.index_view ascending
                                      select new BaoGiaTemDetailView { order_code = a.code, address_deliver = a.address_deliver, pause = a.status_pause, Step_Flow = u.step_index, Status_Pause = u.status_pause, Code_Detail = u.code_detail, Status = u.status, Date_Working = u.date_working, Index_View = 0, Timer = 0, date_deliver = a.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };

            if (date.HasValue)
            {
                querySanPhamSanXuat = from a in db.tbl_OrderTem
                                      join b in db.tbl_OrderTem_BaoGia on a.id equals b.order_id.Value
                                      join u in db.tbl_OrderTem_BaoGia_Detail on b.id equals u.baogia_id.Value
                                      join y in db.tbl_Products on u.sanpam_id.Value equals y.ID_Products
                                      //    join z in db.tbl_Stack on u.id equals z.baoGia_detail_id
                                      where a.id==id && (a.status.Value == 3 || a.status.Value == 1 || a.status.Value == 4 || a.status.Value == 5) && b.status.Value == 1 && u.date_working.Value == date.Value
                                      //     orderby z.index_view ascending
                                      select new BaoGiaTemDetailView { order_code = a.code, address_deliver = a.address_deliver, pause = a.status_pause, Step_Flow = u.step_index, Status_Pause = u.status_pause, Code_Detail = u.code_detail, Status = u.status, Date_Working = u.date_working, Index_View = 0, Timer = 0, date_deliver = a.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };


            }
            List<BaoGiaTemDetailView> list = querySanPhamSanXuat.ToList();
            foreach (var item in list)
            {
                item.Timer = (int)DateTime.Parse(item.date_deliver.Value.ToShortDateString()).Subtract(DateTime.Parse(DateTime.Now.ToShortDateString())).TotalDays;
                var queryQT = from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(item.id) orderby u.ThuTu ascending select u;
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

        //[ActionAuthorizeAttribute("DonHang")]
        public ActionResult TheoDoiDonHang()
        {

            return View();
        }

        [ActionAuthorizeAttribute("DonHang")]
        [HttpPost]
        public ActionResult UpdateFlow_GD_TT_KTDH(int id, int index)
        {
            var queryBaoGia = from u in db.tbl_OrderTem_BaoGia where u.order_id.Value.Equals(id) orderby u.id descending select u;
            var lisBG = queryBaoGia.ToList<tbl_OrderTem_BaoGia>();
            foreach (var item in lisBG)
            {
                var queryBaoGiaDetail = from u in db.tbl_OrderTem_BaoGia_Detail where u.baogia_id.Value.Equals(item.id) orderby u.id descending select u;
                var lisBGDetail = queryBaoGiaDetail.ToList<tbl_OrderTem_BaoGia_Detail>();
                foreach (var itemDetail in lisBGDetail)
                {
                    var queryQT = from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(itemDetail.id) && u.ThuTu==index orderby u.ThuTu ascending select u;
                    List<tbl_QuyTrinh> listQ = queryQT.ToList<tbl_QuyTrinh>();
                    foreach (var itemQT in listQ)
                    {
                        itemQT.TrangThai = 2;
                        itemQT.NgayBatDau_TT = DateTime.Now;
                        itemQT.NgayKetThuc_TT = DateTime.Now;
                        db.Entry(itemQT).State = EntityState.Modified;
                    }
                    if (index == 12)
                    {

                        var listGH = from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(itemDetail.id) && u.ThuTu == 10 orderby u.ThuTu ascending select u;
                        listQ = listGH.ToList<tbl_QuyTrinh>();
                        foreach (var itemQT in listQ)
                        {
                            if (itemQT.TrangThai != 2)
                            {
                                itemQT.TrangThai = 2;
                                itemQT.NgayBatDau_TT = DateTime.Now;
                                itemQT.NgayKetThuc_TT = DateTime.Now;
                                db.Entry(itemQT).State = EntityState.Modified;
                            }

                        }
                        listGH = from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(itemDetail.id) && u.ThuTu == 11 orderby u.ThuTu ascending select u;
                        listQ = listGH.ToList<tbl_QuyTrinh>();
                        foreach (var itemQT in listQ)
                        {
                            if (itemQT.TrangThai != 2)
                            {
                                itemQT.TrangThai = 2;
                                itemQT.NgayBatDau_TT = DateTime.Now;
                                itemQT.NgayKetThuc_TT = DateTime.Now;
                                db.Entry(itemQT).State = EntityState.Modified;
                            }

                        }
                        tbl_OrderTem tbl_OrderTem = db.tbl_OrderTem.Find(id);
                        tbl_OrderTem.date_end = DateTime.Now;
                        tbl_OrderTem.status = 5;
                        db.Entry(tbl_OrderTem).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                }
                
                break;
            }
            return RedirectToAction("ThongTinDonHang", "tbl_OrderTem");
        }

        //[ActionAuthorizeAttribute("DonHang")]
        public PartialViewResult PartialThongTinDonHang()
        {
            var qr = (from data in db.tbl_OrderTem
                      join cus in db.tbl_Customers on data.customer_id equals cus.IDCustomers
                      join bgdetail in db.tbl_OrderTem_BaoGia_Detail on data.id equals bgdetail.baogia_id
                      join sp in db.tbl_Products on bgdetail.sanpam_id  equals sp.ID_Products
                     // join qt in db.tbl_QuyTrinh on bgdetail.id equals qt.ID_BaoGiaDetail
                      where data.status >= 0
                      orderby data.update_date descending
                      select new DonHangView
                      {
                          id = data.id,
                          customer_id = cus.IDCustomers,
                          Customer = cus,
                          code = data.code,
                          date_deliver = data.date_deliver,
                          address_deliver = data.address_deliver,
                          status = data.status,
                          pause=data.status_pause,
                          TblProductses=sp//,
                         // QuyTrinh = qt
                      });
            List<DonHangView> list = qr.ToList();
            foreach (var itemBG in list)
            {

                var queryBaoGia = from u in db.tbl_OrderTem_BaoGia where u.order_id.Value.Equals(itemBG.id) orderby u.id descending select u;
                var lisBG = queryBaoGia.ToList<tbl_OrderTem_BaoGia>();
                foreach (var item in lisBG)
                {
                    BaoGiaTemView temBG = new BaoGiaTemView { commission = item.commission, commission_money = item.commission_monney, note = item.note, date_begin = item.date_begin, date_end = item.date_end, id = item.id, order_id = item.order_id, status = item.status, total_money = item.total_money };

                    var queryGiaoGiaCT = from u in db.tbl_OrderTem_BaoGia_Detail
                                         join y in db.tbl_Products on u.sanpam_id equals y.ID_Products
                                         where u.baogia_id.Value.Equals(temBG.id)
                                         select new BaoGiaTemDetailView { loai_design = u.loai_design, Step_Flow = u.step_index, Status = u.status, Code_Detail = u.code_detail, Date_Working = u.date_working, date_deliver = itemBG.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
                    temBG.BaoGiaTemDetailViews = queryGiaoGiaCT.ToList<BaoGiaTemDetailView>();
                    itemBG.BaoGiaTemView = temBG;


                    foreach (var itemSP in itemBG.BaoGiaTemView.BaoGiaTemDetailViews)
                    {
                        if (itemSP.Step_Flow.HasValue)
                        {
                            
                            //var queryQt = from u in db.tbl_QuyTrinh
                            //              where u.ID_BaoGiaDetail.Equals(itemSP.id) 
                            //    orderby u.ThuTu ascending
                            //    select u;
                           
                            //itemSP.QuyTrinhs = queryQt.ToList<tbl_QuyTrinh>();
                            var listGH =(from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(itemSP.id) && u.ThuTu == 10 orderby u.ThuTu ascending select u).Take(1);
                            itemSP.QuyTrinhs = listGH.ToList<tbl_QuyTrinh>();
                            itemBG.BaoGiaTemView.QuyTrinhs = itemSP.QuyTrinhs;
                            foreach (var itsp in itemBG.BaoGiaTemView.QuyTrinhs)
                            {
                                itemBG.BaoGiaTemView.StatusQuyTrinhGiaoHang = (int)itsp.TrangThai;
                            }

                            var listDG = (from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(itemSP.id) && u.ThuTu == 11 orderby u.ThuTu ascending select u).Take(1);
                            itemSP.QuyTrinhs = listDG.ToList<tbl_QuyTrinh>();
                            itemBG.BaoGiaTemView.QuyTrinhs = itemSP.QuyTrinhs;
                            foreach (var itsp in itemBG.BaoGiaTemView.QuyTrinhs)
                            {
                                itemBG.BaoGiaTemView.StatusQuyTrinhDongGoi = (int)itsp.TrangThai;
                            }
                            var listTT= (from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(itemSP.id) && u.ThuTu == 12 orderby u.ThuTu ascending select u).Take(1);
                            itemSP.QuyTrinhs = listTT.ToList<tbl_QuyTrinh>();
                            itemBG.BaoGiaTemView.QuyTrinhs = itemSP.QuyTrinhs;
                            foreach (var itsp in itemBG.BaoGiaTemView.QuyTrinhs)
                            {
                                itemBG.BaoGiaTemView.StatusQuyTrinhThanhToan = (int)itsp.TrangThai;
                            }
                            
                           
                        }

                    }
                    break;
                }
                
            }
           
            return PartialView(list);
        }

        [ActionAuthorizeAttribute("DonHang")]
        [HttpPost]
        public ActionResult IndexSXSubmit(int? id,int status,int idBG)
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
            
              
                tbl_OrderTem order = db.tbl_OrderTem.Find(id.Value);
             

                order.update_date = DateTime.Now;
                order.update_user = Session["username"].ToString();
                order.status = status;
                order.date_begin = DateTime.Now;
                // order.status = 3;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                var queryGiaoGiaCT = from u in db.tbl_OrderTem_BaoGia_Detail
                                     join y in db.tbl_Products on u.sanpam_id equals y.ID_Products
                                     where u.baogia_id.Value.Equals(idBG)
                                     select new BaoGiaTemDetailView { Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
                List<BaoGiaTemDetailView> listSP = queryGiaoGiaCT.ToList<BaoGiaTemDetailView>();
                foreach (var item in listSP)
                {
                    tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail = db.tbl_OrderTem_BaoGia_Detail.Find(item.id);
                    // tbl_OrderTem_BaoGia_Detail.status = 0;
                    tbl_OrderTem_BaoGia_Detail.status = 0;
                    db.Entry(tbl_OrderTem_BaoGia_Detail).State = EntityState.Modified;
                    //FLEXO - BẤM KIM	FLEXO - DÁN	FLEXO - Bế - BK	FLEXO - Bế - DÁN	OFFSET -Bế  - BK	OFFSET - Bế - DÁN
                    if (item.OffsetFlexoProducts.Equals("FLEXO - BẤM KIM"))
                    {
                    tbl_QuyTrinh qt0 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 0, TrangThai = 0, TenBuoc = "Nhận tờ in offset",SongSong = 1};
                    db.tbl_QuyTrinh.Add(qt0);
                    tbl_QuyTrinh qt1 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 1, TrangThai = 0, TenBuoc = "Sản xuất giấy tấm", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt1);
                        tbl_QuyTrinh qt2 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 2, TrangThai = 0, TenBuoc = "Bồi", SongSong = 1 };
                    db.tbl_QuyTrinh.Add(qt2);
                    tbl_QuyTrinh qt3 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 3, TrangThai = 0, TenBuoc = "Xả biến, cán lằn", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt3);
                        tbl_QuyTrinh qt4 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 4, TrangThai = 0, TenBuoc = "In FLEXO", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt4);
                        tbl_QuyTrinh qt5 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 5, TrangThai = 0, TenBuoc = "Bế", SongSong = 1 };
                    db.tbl_QuyTrinh.Add(qt5);
                    tbl_QuyTrinh qt6 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 6, TrangThai = 0, TenBuoc = "Chập khe", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt6);
                        tbl_QuyTrinh qt7 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 7, TrangThai = 0, TenBuoc = "Bấm kim", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt7);
                        tbl_QuyTrinh qt8 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 8, TrangThai = 0, TenBuoc = "Dán", SongSong = 1 };
                    db.tbl_QuyTrinh.Add(qt8);

                }
                    if (item.OffsetFlexoProducts.Equals("FLEXO - DÁN"))
                    {
                        tbl_QuyTrinh qt0 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 0, TrangThai = 0, TenBuoc = "Nhận tờ in offset", SongSong = 1 };
                    db.tbl_QuyTrinh.Add(qt0);
                    tbl_QuyTrinh qt1 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 1, TrangThai = 0, TenBuoc = "Sản xuất giấy tấm", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt1);
                        tbl_QuyTrinh qt2 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 2, TrangThai = 0, TenBuoc = "Bồi", SongSong = 1 };
                    db.tbl_QuyTrinh.Add(qt2);
                    tbl_QuyTrinh qt3 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 3, TrangThai = 0, TenBuoc = "Xả biến, cán lằn", SongSong = 1 };
                  
                    db.tbl_QuyTrinh.Add(qt3);
                    tbl_QuyTrinh qt4 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 4, TrangThai = 0, TenBuoc = "In FLEXO", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt4);
                        tbl_QuyTrinh qt5 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 5, TrangThai = 0, TenBuoc = "Bế", SongSong = 1 };
                    db.tbl_QuyTrinh.Add(qt5);
                    tbl_QuyTrinh qt6 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 6, TrangThai = 0, TenBuoc = "Chập khe", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt6);
                        tbl_QuyTrinh qt7 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 7, TrangThai = 0, TenBuoc = "Bấm kim", SongSong = 1 };
                    db.tbl_QuyTrinh.Add(qt7);
                    tbl_QuyTrinh qt8 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 8, TrangThai = 0, TenBuoc = "Dán", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt8);
                    }
                    if (item.OffsetFlexoProducts.Equals("FLEXO - Bế"))
                    {
                        tbl_QuyTrinh qt0 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 0, TrangThai = 0, TenBuoc = "Nhận tờ in offset", SongSong = 1 };
                    db.tbl_QuyTrinh.Add(qt0);

                    tbl_QuyTrinh qt1 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 1, TrangThai = 0, TenBuoc = "Sản xuất giấy tấm", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt1);

                        tbl_QuyTrinh qt2 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 2, TrangThai = 0, TenBuoc = "Bồi", SongSong = 1 };
                    db.tbl_QuyTrinh.Add(qt2);

                    tbl_QuyTrinh qt3 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 3, TrangThai = 0, TenBuoc = "Xả biến, cán lằn", SongSong = 1 };
                    db.tbl_QuyTrinh.Add(qt3);

                    tbl_QuyTrinh qt4 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 4, TrangThai = 0, TenBuoc = "In FLEXO", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt4);
                        tbl_QuyTrinh qt5 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 5, TrangThai = 0, TenBuoc = "Bế", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt5);
                        tbl_QuyTrinh qt6 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 6, TrangThai = 0, TenBuoc = "Chập khe", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt6);
                        tbl_QuyTrinh qt7 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 7, TrangThai = 0, TenBuoc = "Bấm kim", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt7);
                        tbl_QuyTrinh qt8 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 8, TrangThai = 0, TenBuoc = "Dán", SongSong = 1 };
                    db.tbl_QuyTrinh.Add(qt8);
                }
                    if (item.OffsetFlexoProducts.Equals("FLEXO - Bế - DÁN"))
                    {

                        tbl_QuyTrinh qt0 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 0, TrangThai = 0, TenBuoc = "Nhận tờ in offset", SongSong = 1 };
                    db.tbl_QuyTrinh.Add(qt0);

                    tbl_QuyTrinh qt1 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 1, TrangThai = 0, TenBuoc = "Sản xuất giấy tấm", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt1);

                        tbl_QuyTrinh qt2 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 2, TrangThai = 0, TenBuoc = "Bồi", SongSong = 1 };
                    db.tbl_QuyTrinh.Add(qt2);

                    tbl_QuyTrinh qt3 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 3, TrangThai = 0, TenBuoc = "Xả biến, cán lằn", SongSong = 1 };
                    db.tbl_QuyTrinh.Add(qt3);

                    tbl_QuyTrinh qt4 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 4, TrangThai = 0, TenBuoc = "In FLEXO", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt4);
                        tbl_QuyTrinh qt5 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 5, TrangThai = 0, TenBuoc = "Bế", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt5);
                        tbl_QuyTrinh qt6 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 6, TrangThai = 0, TenBuoc = "Chập khe", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt6);


                        tbl_QuyTrinh qt7 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 7, TrangThai = 0, TenBuoc = "Bấm kim", SongSong = 1 };
                    db.tbl_QuyTrinh.Add(qt7);

                    tbl_QuyTrinh qt8 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 8, TrangThai = 0, TenBuoc = "Dán", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt8);
                    }
                    if (item.OffsetFlexoProducts.Equals("OFFSET -Bế - BK"))
                    {
                        tbl_QuyTrinh qt0 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 0, TrangThai = 0, TenBuoc = "Nhận tờ in offset", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt0);
                        tbl_QuyTrinh qt1 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 1, TrangThai = 0, TenBuoc = "Sản xuất giấy tấm", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt1);
                        tbl_QuyTrinh qt2 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 2, TrangThai = 0, TenBuoc = "Bồi", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt2);

                        tbl_QuyTrinh qt3 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 3, TrangThai = 0, TenBuoc = "Xả biến, cán lằn", SongSong = 1 };
                    db.tbl_QuyTrinh.Add(qt3);
                    tbl_QuyTrinh qt4 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 4, TrangThai = 0, TenBuoc = "In FLEXO", SongSong = 1 };
                    db.tbl_QuyTrinh.Add(qt4);

                    tbl_QuyTrinh qt5 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 5, TrangThai = 0, TenBuoc = "Bế", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt5);

                        tbl_QuyTrinh qt6 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 6, TrangThai = 0, TenBuoc = "Chập khe", SongSong = 1 };
                    db.tbl_QuyTrinh.Add(qt6);

                    tbl_QuyTrinh qt7 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 7, TrangThai = 0, TenBuoc = "Bấm kim", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt7);
                        tbl_QuyTrinh qt8 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 8, TrangThai = 0, TenBuoc = "Dán", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt8);

                    }
                    if (item.OffsetFlexoProducts.Equals("OFFSET - Bế - DÁN"))
                    {
                        tbl_QuyTrinh qt0 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 0, TrangThai = 0, TenBuoc = "Nhận tờ in offset", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt0);
                        tbl_QuyTrinh qt1 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 1, TrangThai = 0, TenBuoc = "Sản xuất giấy tấm", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt1);
                        tbl_QuyTrinh qt2 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 2, TrangThai = 0, TenBuoc = "Bồi", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt2);

                        tbl_QuyTrinh qt3 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 3, TrangThai = 0, TenBuoc = "Xả biến, cán lằn", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt3);
                        tbl_QuyTrinh qt4 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 4, TrangThai = 0, TenBuoc = "In FLEXO", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt4);

                        tbl_QuyTrinh qt5 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 5, TrangThai = 0, TenBuoc = "Bế", SongSong = 1 };
                            db.tbl_QuyTrinh.Add(qt5);

                            tbl_QuyTrinh qt6 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 6, TrangThai = 0, TenBuoc = "Chập khe", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt6);

                        tbl_QuyTrinh qt7 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 7, TrangThai = 0, TenBuoc = "Bấm kim", SongSong = 1 };
                        db.tbl_QuyTrinh.Add(qt7);

                        tbl_QuyTrinh qt8 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 8, TrangThai = 0, TenBuoc = "Dán", SongSong = 1 };
                            db.tbl_QuyTrinh.Add(qt8);
                        }
                        tbl_QuyTrinh qt9 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 9, TrangThai = 0, TenBuoc = "Đóng gói" };
                        db.tbl_QuyTrinh.Add(qt9);
                        tbl_QuyTrinh qt10 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 10, TrangThai = 0, TenBuoc = "Giao hàng" };
                        db.tbl_QuyTrinh.Add(qt10);
                        tbl_QuyTrinh qt11 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 11, TrangThai = 0, TenBuoc = "Thanh toán" };
                        db.tbl_QuyTrinh.Add(qt11);
                        tbl_QuyTrinh qt12 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 12, TrangThai = 0, TenBuoc = "Kết thúc đơn hàng" };
                        db.tbl_QuyTrinh.Add(qt12);
                    
                }


                var q = from a in db.tbl_OrderTem_BaoGia_Detail where a.baogia_id.Value == idBG select a;
                List<tbl_OrderTem_BaoGia_Detail> listDetail = q.ToList();
                var queryMax = (from u in db.tbl_Stack
                                orderby u.index_view descending
                                select u).Take(1);
                int maxStackView = queryMax.ToList().Count == 0 ? 1 : queryMax.ToList()[0].index_view.Value;

                for (int i = 0; i < listDetail.Count; i++)
                {
                    var item = listDetail[i];
                    item.status = 0;
                    item.date_working = DateTime.Now;
                    db.Entry(item).State = EntityState.Modified;
                    tbl_Stack tbl_Stack = new tbl_Stack { baoGia_detail_id = item.id, date_create = DateTime.Now, index_view = maxStackView + i + 1 };
                    db.tbl_Stack.Add(tbl_Stack);

                }
                db.SaveChanges();
                return RedirectToAction("ThongTinDonHang", "tbl_OrderTem");
           
        }

        //[ActionAuthorizeAttribute("DonHang")]
        public ActionResult ThongTinDonHang()
        {

            var qr = (from data in db.tbl_OrderTem
                      join cus in db.tbl_Customers on data.customer_id equals cus.IDCustomers
                                          join bgdetail in db.tbl_OrderTem_BaoGia_Detail on data.id equals bgdetail.baogia_id
                   join sp in db.tbl_Products on bgdetail.sanpam_id  equals sp.ID_Products

                      where data.status >= 0
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
                var lisBG = queryBaoGia.ToList<tbl_OrderTem_BaoGia>();
                foreach (var item in lisBG)
                {
                    BaoGiaTemView temBG = new BaoGiaTemView { commission = item.commission, commission_money = item.commission_monney, note = item.note, date_begin = item.date_begin, date_end = item.date_end, id = item.id, order_id = item.order_id, status = item.status, total_money = item.total_money };

                    var queryGiaoGiaCT = from u in db.tbl_OrderTem_BaoGia_Detail
                                         join y in db.tbl_Products on u.sanpam_id equals y.ID_Products
                                         where u.baogia_id.Value.Equals(temBG.id)
                                         select new BaoGiaTemDetailView { loai_design = u.loai_design, Step_Flow = u.step_index, Status = u.status, Code_Detail = u.code_detail, Date_Working = u.date_working, address_deliver = itemBG.address_deliver, date_deliver = itemBG.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
                    temBG.BaoGiaTemDetailViews = queryGiaoGiaCT.ToList<BaoGiaTemDetailView>();
                    itemBG.BaoGiaTemView = temBG;


                    foreach (var itemSP in itemBG.BaoGiaTemView.BaoGiaTemDetailViews)
                    {
                        if (itemSP.Step_Flow.HasValue)
                        {
                            var queryQT = from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(itemSP.id) orderby u.ThuTu ascending select u;
                            itemSP.QuyTrinhs = queryQT.ToList<tbl_QuyTrinh>();
                        }

                    }
                    break;
                }
            }

            return View(list);

            

        }

        //[ActionAuthorizeAttribute("DonHang")]
        public ActionResult Index()
        {
            var qr = (from data in db.tbl_OrderTem
                      join cus in db.tbl_Customers on data.customer_id equals cus.IDCustomers
                      where data.status >=0 orderby data.update_date descending
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
                var lisBG = queryBaoGia.ToList<tbl_OrderTem_BaoGia>();
                foreach (var item in lisBG)
                {
                    BaoGiaTemView temBG = new BaoGiaTemView { commission = item.commission, commission_money = item.commission_monney, note = item.note, date_begin = item.date_begin, date_end = item.date_end, id = item.id, order_id = item.order_id, status = item.status, total_money = item.total_money };

                    var queryGiaoGiaCT = from u in db.tbl_OrderTem_BaoGia_Detail
                                         join y in db.tbl_Products on u.sanpam_id equals y.ID_Products
                                         where u.baogia_id.Value.Equals(temBG.id)
                                         select new BaoGiaTemDetailView { loai_design = u.loai_design, Step_Flow = u.step_index, Status = u.status, Code_Detail = u.code_detail, Date_Working = u.date_working, address_deliver = itemBG.address_deliver, date_deliver = itemBG.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
                    temBG.BaoGiaTemDetailViews = queryGiaoGiaCT.ToList<BaoGiaTemDetailView>();
                    itemBG.BaoGiaTemView = temBG;


                    foreach (var itemSP in itemBG.BaoGiaTemView.BaoGiaTemDetailViews)
                    {
                        if (itemSP.Step_Flow.HasValue)
                        {
                            var queryQT = from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(itemSP.id) && u.ThuTu.Value.Equals(itemSP.Step_Flow.Value) orderby u.ThuTu ascending select u;
                            itemSP.QuyTrinhs = queryQT.ToList<tbl_QuyTrinh>();
                        }
                      
                    }
                    break;
                }
            }

            return View(list);
        }

       // [ActionAuthorizeAttribute("BaoGia")]
        [HttpPost]
        public ActionResult IndexBaoGia(DonHangView donHang)
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
           
                donHang.action = 0;


                var queryCount = from a in db.tbl_OrderTem_BaoGia_Detail where a.baogia_id.Value.Equals(donHang.BaoGiaTemView.id) select a;
                int countItem = queryCount.ToList().Count();


                tbl_OrderTem order = db.tbl_OrderTem.Find(donHang.id);
                if (donHang.BaoGiaTemView.status.Value == 1)
                {
                    order.code = order.code;
                    order.status = 0;


                }
                order.date_deliver = donHang.date_deliver;
                 order.datenumber = (int)donHang.date_deliver.Value.Subtract(DateTime.Now).TotalDays;
                order.address_deliver = donHang.address_deliver;
                order.update_date = DateTime.Now;
                order.update_user = Session["username"].ToString();
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();


                tbl_OrderTem_BaoGia baogia = db.tbl_OrderTem_BaoGia.Find(donHang.BaoGiaTemView.id);
                baogia.status = donHang.BaoGiaTemView.status.Value;
                baogia.commission = donHang.BaoGiaTemView.commission;
                baogia.date_end = DateTime.Now;
                baogia.note = donHang.BaoGiaTemView.note;
                db.Entry(baogia).State = EntityState.Modified;
                db.SaveChanges();
               
           return IndexBaoGia();
        }

      //  [ActionAuthorizeAttribute("BaoGia")] // all view bao gia
        public ActionResult IndexBaoGia()
        {
           
            var qr = (from data in db.tbl_OrderTem
                      join cus in db.tbl_Customers on data.customer_id equals cus.IDCustomers
                      join bgdetail in db.tbl_OrderTem_BaoGia_Detail on data.id equals bgdetail.baogia_id
                      join sp in db.tbl_Products on bgdetail.sanpam_id equals sp.ID_Products
                     // where data.status ==-1
                        orderby data.update_date descending
                      select new DonHangView
                      {
                          id = data.id,
                          customer_id = cus.IDCustomers,
                          Customer = cus,
                          code = data.code,
                          date_deliver = data.date_deliver,
                          address_deliver = data.address_deliver,
                          status = data.status,
                          TblProductses = sp
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

        
        [ActionAuthorizeAttribute("BaoGia")]      
        public ActionResult Create(int? id)
        {
          
            DonHangView d = new DonHangView();
            d.customer_id = id;
           
            var queryMax = (from u in db.tbl_OrderTem
                            orderby u.id descending
                            select u).Take(1);
            int max = queryMax.ToList().Count == 0 ? 1 : queryMax.ToList()[0].id + 1;
            d.code = String.Format("N{0}T{1}N{2}_{3}", +DateTime.Now.Year, DateTime.Now.Month.ToString("00"), DateTime.Now.Day.ToString("00"), max.ToString("000"));
            if (id.HasValue)
            {
                var list = from tt in db.tbl_Customers where tt.IDCustomers == id.Value select tt;
                d.Customer = list.ToList()[0];
            }
            d.tbl_Customers = db.tbl_Customers.ToList();
            return View(d);
        }

        [ActionAuthorizeAttribute("BaoGia")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DonHangView donHang)
        {
           
            var qr = from custom in db.tbl_Customers
                     where custom.IDCustomers == donHang.customer_id
                     select custom;
            var code1 = "";
            foreach (var it in qr)
            {
                code1 = it.CodeCustomers;
            }
            donHang.status = -1;
            donHang.BaoGiaTemView.status = 0;
            tbl_OrderTem temValue = new tbl_OrderTem { datenumber=donHang.datenumber, create_date = DateTime.Now, create_user = Session["username"].ToString(), customer_id = donHang.customer_id, code = code1 + "_" + donHang.code, date_begin = donHang.date_begin, date_end = donHang.date_end, status = donHang.status, id = donHang.id };
            temValue = db.tbl_OrderTem.Add(temValue);
            db.SaveChanges();
            donHang.id = temValue.id;
            donHang.BaoGiaTemView.date_begin = DateTime.Now;
            tbl_OrderTem_BaoGia tbl_OrderTem_BaoGia = new tbl_OrderTem_BaoGia { date_begin = donHang.BaoGiaTemView.date_begin, date_end = donHang.BaoGiaTemView.date_end, id = donHang.BaoGiaTemView.id, order_id = donHang.id, status = donHang.BaoGiaTemView.status, total_money = donHang.BaoGiaTemView.total_money };
            tbl_OrderTem_BaoGia = db.tbl_OrderTem_BaoGia.Add(tbl_OrderTem_BaoGia);
            db.SaveChanges();
            donHang.BaoGiaTemView.id = tbl_OrderTem_BaoGia.id;

            int countindex = 0;
           
            foreach (var item in donHang.BaoGiaTemView.BaoGiaTemDetailViews)
            {
                countindex++;
                item.StatusProducts = -1;
                item.CreatedDateProducts = DateTime.Now;
                var queryMax = (from u in db.tbl_Products
                                orderby u.ID_Products descending
                                select u).Take(1);
                int maxSP = queryMax.ToList().Count == 0 ? 1 : queryMax.ToList()[0].ID_Products + 1;

                String masp = String.Format("SP{0}", maxSP.ToString("000000"));
                item.CodeProducts = masp;
                var tblOrderTem=new tbl_OrderTem();
                tbl_Products itemP = new tbl_Products
                {
                    CodeProducts = masp,
                    CreatedDateProducts = item.CreatedDateProducts,
                    CreateUserProducts = item.CreateUserProducts,
                    DanKimProducts = item.DanKimProducts,
                    GiaProducts = item.GiaProducts,
                    ID_Products = item.ID_Products,
                    LoaigiayProducts = item.LoaigiayProducts,
                    ModifyDateProducts = item.ModifyDateProducts,
                    ModifyUserProducts = item.ModifyUserProducts,
                    NameProducts = item.NameProducts,
                    OffsetFlexoProducts = item.OffsetFlexoProducts,
                    QuyCachProducts = item.QuyCachProducts,
                    SolopProducts = item.SolopProducts,
                    StatusProducts = item.StatusProducts,
                    LoaiSongProducts=item.LoaiSongProducts,
                    InFlexoProducts = item.InFlexoProducts
                  
                  
                };
                itemP = db.tbl_Products.Add(itemP);
                db.SaveChanges();              
                tbl_OrderTem_BaoGia_Detail detail = new tbl_OrderTem_BaoGia_Detail { code_detail = code1 + "_" + donHang.code + "_" + countindex.ToString("00"),design = item.Design, baogia_id = donHang.BaoGiaTemView.id, money = double.Parse(item.GiaProducts), soluong = item.SoLuong, sanpam_id = itemP.ID_Products };
                db.tbl_OrderTem_BaoGia_Detail.Add(detail);              
                db.SaveChanges();
            }
           
            return RedirectToAction("EditBaoGia", new
            {
                id = donHang.id
            });

        }

        [ActionAuthorizeAttribute("BaoGia")]
        public ActionResult EditBaoGia(int? id)
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
            d.date_deliver = tbl_OrderTem.date_deliver;
            d.address_deliver = tbl_OrderTem.address_deliver;
            d.datenumber =  tbl_OrderTem.datenumber;
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
                                         select new BaoGiaTemDetailView {loai_design = u.loai_design,Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
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
                                     select new BaoGiaTemDetailView {LoaiSongProducts=y.LoaiSongProducts,InFlexoProducts=y.InFlexoProducts, datenumber = d.datenumber, loai_design = u.loai_design, Date_Working = u.date_working, address_deliver = d.address_deliver, date_deliver = d.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
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

        [ActionAuthorizeAttribute("BaoGia")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBaoGia(DonHangView donHang)
        {
            int? id = donHang.id;
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_OrderTem tbl_OrderTem = db.tbl_OrderTem.Find(id);
            if (tbl_OrderTem == null)
            {
                return HttpNotFound();
            }
            if (donHang.action == 2)
            {
                donHang.action = 0;
                donHang.BaoGiaTemView.status = 0;
                donHang.BaoGiaTemView.date_begin = DateTime.Now;
                tbl_OrderTem_BaoGia tbl_OrderTem_BaoGia = new tbl_OrderTem_BaoGia { date_begin = donHang.BaoGiaTemView.date_begin, date_end = donHang.BaoGiaTemView.date_end, id = donHang.BaoGiaTemView.id, order_id = donHang.id, status = donHang.BaoGiaTemView.status, total_money = donHang.BaoGiaTemView.total_money };
                tbl_OrderTem_BaoGia = db.tbl_OrderTem_BaoGia.Add(tbl_OrderTem_BaoGia);
                db.SaveChanges();
                donHang.BaoGiaTemView.id = tbl_OrderTem_BaoGia.id;
                int countindex = 0;
                foreach (var item in donHang.BaoGiaTemView.BaoGiaTemDetailViews)
                {
                    countindex++;
                    item.StatusProducts = -1;
                    item.CreatedDateProducts = DateTime.Now;
                    if (item.ID_Products==0)
                    {
                        var queryMax = (from u in db.tbl_Products
                                        orderby u.ID_Products descending
                                        select u).Take(1);
                        int maxSP = queryMax.ToList().Count == 0 ? 1 : queryMax.ToList()[0].ID_Products + 1;
                        String masp = String.Format("SP{0}", maxSP.ToString("000000"));
                        item.CodeProducts = masp;
                        tbl_Products itemP = new tbl_Products
                        {
                            CodeProducts = masp,
                            CreatedDateProducts = item.CreatedDateProducts.HasValue ? item.CreatedDateProducts : DateTime.Now,
                            CreateUserProducts = item.CreateUserProducts,
                            DanKimProducts = item.DanKimProducts,
                            GiaProducts = item.GiaProducts,
                            ID_Products = item.ID_Products,
                            LoaigiayProducts = item.LoaigiayProducts,
                          //  ModifyDateProducts = item.ModifyDateProducts,
                          //  ModifyUserProducts = item.ModifyUserProducts,
                            NameProducts = item.NameProducts,
                            OffsetFlexoProducts = item.OffsetFlexoProducts,
                            QuyCachProducts = item.QuyCachProducts,
                            SolopProducts = item.SolopProducts,
                            StatusProducts = item.StatusProducts,
                            LoaiSongProducts = item.LoaiSongProducts,
                            InFlexoProducts = item.InFlexoProducts
                        };
                        itemP = db.tbl_Products.Add(itemP);
                        db.SaveChanges();
                        item.ID_Products = itemP.ID_Products;
                    }                    
                    var qr = (from cus in db.tbl_Customers where cus.IDCustomers == donHang.customer_id select cus).ToList();
                    foreach (var code in qr)
                    {
                        tbl_OrderTem_BaoGia_Detail detail = new tbl_OrderTem_BaoGia_Detail { loai_design = item.loai_design, design_date = item.Design_Date, design_img = item.Design_Img, code_detail = code.CodeCustomers + "_" + donHang.code + "_" + countindex.ToString("00"), design = item.Design, baogia_id = donHang.BaoGiaTemView.id, money = double.Parse(item.GiaProducts), soluong = item.SoLuong, sanpam_id = item.ID_Products };
                        db.tbl_OrderTem_BaoGia_Detail.Add(detail);
                    }
                    db.SaveChanges();
                }

            }
            if (donHang.action == 3)
            {
                donHang.action = 0;


                var queryCount = from a in db.tbl_OrderTem_BaoGia_Detail where a.baogia_id.Value.Equals(donHang.BaoGiaTemView.id) select a;
                int countItem = queryCount.ToList().Count();


                tbl_OrderTem order = db.tbl_OrderTem.Find(donHang.id);
                if (donHang.BaoGiaTemView.status.Value==1)
                {
                    order.code = order.code;
                    order.status = 0;


                }               
                order.date_deliver = donHang.date_deliver;
                order.address_deliver = donHang.address_deliver;
                order.datenumber = donHang.datenumber;
                order.update_date = DateTime.Now;
                order.update_user = Session["username"].ToString();
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();


                tbl_OrderTem_BaoGia baogia = db.tbl_OrderTem_BaoGia.Find(donHang.BaoGiaTemView.id);
                baogia.status = donHang.BaoGiaTemView.status.Value;
                baogia.commission = donHang.BaoGiaTemView.commission;
                baogia.date_end = DateTime.Now;
                baogia.note = donHang.BaoGiaTemView.note;
                db.Entry(baogia).State = EntityState.Modified;
                db.SaveChanges();
                if (donHang.BaoGiaTemView.status.Value == 1)
                {
                    return RedirectToAction("ThongTinDonHang", "tbl_OrderTem");
                }
            }
            if (donHang.action == 4)
            {


                DonHangView d1 = new DonHangView();
                d1.date_deliver = tbl_OrderTem.date_deliver;
                d1.address_deliver = tbl_OrderTem.address_deliver;
                d1.datenumber = tbl_OrderTem.datenumber;

                d1.customer_id = tbl_OrderTem.customer_id;
                d1.code = tbl_OrderTem.code;
                d1.date_begin = tbl_OrderTem.date_begin;
                d1.date_end = tbl_OrderTem.date_end;
                d1.id = tbl_OrderTem.id;
                d1.status = tbl_OrderTem.status;
                d1.date_begin_plan = tbl_OrderTem.date_begin_plan;
                d1.date_end_plan = tbl_OrderTem.date_end_plan;

                var queryBaoGia1 = from u in db.tbl_OrderTem_BaoGia where u.order_id.Value.Equals(tbl_OrderTem.id) orderby u.id descending select u;
                List<tbl_OrderTem_BaoGia> lisBG1 = queryBaoGia1.ToList<tbl_OrderTem_BaoGia>();
                List<BaoGiaTemView> lisBGTem1 = new List<BaoGiaTemView>();

                if (lisBG1.Count > 1)
                {
                    for (int i = 1; i < lisBG1.Count; i++)
                    {
                        var item = lisBG1[i];
                        BaoGiaTemView temBG = new BaoGiaTemView { commission = item.commission, commission_money = item.commission_monney, note = item.note, date_begin = item.date_begin, date_end = item.date_end, id = item.id, order_id = item.order_id, status = item.status, total_money = item.total_money };
                        var queryGiaoGiaCT = from u in db.tbl_OrderTem_BaoGia_Detail
                                             join y in db.tbl_Products on u.sanpam_id equals y.ID_Products
                                             where u.baogia_id.Value.Equals(temBG.id)
                                             select new BaoGiaTemDetailView { loai_design = u.loai_design, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
                        temBG.BaoGiaTemDetailViews = queryGiaoGiaCT.ToList<BaoGiaTemDetailView>();
                        lisBGTem1.Add(temBG);
                    }
                }

                d1.BaoGiaTemViews = lisBGTem1;
                queryBaoGia1 = from u in db.tbl_OrderTem_BaoGia where u.order_id.Value.Equals(tbl_OrderTem.id) orderby u.id descending select u;
                lisBG1 = queryBaoGia1.ToList<tbl_OrderTem_BaoGia>();
                foreach (var item in lisBG1)
                {
                    BaoGiaTemView temBG = new BaoGiaTemView { commission = item.commission, commission_money = item.commission_monney, note = item.note, date_begin = item.date_begin, date_end = item.date_end, id = item.id, order_id = item.order_id, status = item.status, total_money = item.total_money };
                    var queryGiaoGiaCT = from u in db.tbl_OrderTem_BaoGia_Detail
                                         join y in db.tbl_Products on u.sanpam_id equals y.ID_Products
                                         where u.baogia_id.Value.Equals(temBG.id)
                                         select new BaoGiaTemDetailView { datenumber = d1.datenumber, loai_design = u.loai_design, Date_Working = u.date_working, address_deliver = d1.address_deliver, date_deliver = d1.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
                    temBG.BaoGiaTemDetailViews = queryGiaoGiaCT.ToList<BaoGiaTemDetailView>();
                    d1.BaoGiaTemView = temBG;
                    foreach (var itemSP in d1.BaoGiaTemView.BaoGiaTemDetailViews)
                    {
                        var queryQT = from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(itemSP.id) orderby u.ThuTu ascending select u;
                        itemSP.QuyTrinhs = queryQT.ToList<tbl_QuyTrinh>();
                    }
                    break;
                }
                foreach (var item in donHang.BaoGiaTemView.BaoGiaTemDetailViews)
                {

                    tbl_Products itemP = new tbl_Products();
                    var detail = new tbl_OrderTem_BaoGia_Detail();
                    var tblbaogia = new tbl_OrderTem_BaoGia();
                    foreach (var item1 in d1.BaoGiaTemView.BaoGiaTemDetailViews)
                    {
                        itemP = db.tbl_Products.Find(item1.ID_Products);
                        detail = db.tbl_OrderTem_BaoGia_Detail.Find(item1.id);
                        tblbaogia = db.tbl_OrderTem_BaoGia.Find(id);
                    //   itemP.CreatedDateProducts = item.CreatedDateProducts;
                     //   itemP.CreateUserProducts = item.CreateUserProducts;
                        itemP.DanKimProducts = item.DanKimProducts;
                        itemP.GiaProducts = item.GiaProducts;
                        itemP.LoaigiayProducts = item.LoaigiayProducts;
                        itemP.ModifyDateProducts = item.ModifyDateProducts.HasValue? item.ModifyDateProducts:DateTime.Now;
                        itemP.ModifyUserProducts = item.ModifyUserProducts;
                        itemP.NameProducts = item.NameProducts;
                        itemP.OffsetFlexoProducts = item.OffsetFlexoProducts;
                        itemP.QuyCachProducts = item.QuyCachProducts;
                        itemP.SolopProducts = item.SolopProducts;
                        itemP.StatusProducts = item.StatusProducts;
                        itemP.LoaiSongProducts = item.LoaiSongProducts;
                        itemP.InFlexoProducts = item.InFlexoProducts;
                        db.Entry(itemP).State = EntityState.Modified;   
                        detail.money = double.Parse(item.GiaProducts);
                        detail.soluong = item.SoLuong;
                        tblbaogia.total_money = double.Parse(item.GiaProducts) * item.SoLuong;
                  
                        
                      
                    }
                    db.SaveChanges();

                    return RedirectToAction("IndexBaoGia", "tbl_OrderTem");
                }
                    
                
            }
          
            DonHangView d = new DonHangView();
            d.action = donHang.action;
            d.address_deliver = tbl_OrderTem.address_deliver;
            d.date_deliver = tbl_OrderTem.date_deliver;
            d.datenumber = tbl_OrderTem.datenumber;
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
                                         select new BaoGiaTemDetailView { loai_design = u.loai_design, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
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
                                     select new BaoGiaTemDetailView { LoaiSongProducts=y.LoaiSongProducts,InFlexoProducts=y.InFlexoProducts,datenumber = d.datenumber, loai_design = u.loai_design, Date_Working = u.date_working, address_deliver = d.address_deliver, date_deliver = d.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
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

        [ActionAuthorizeAttribute("DonHang")]  
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
            d.date_deliver = tbl_OrderTem.date_deliver;
            d.address_deliver = tbl_OrderTem.address_deliver;
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
                                     select new BaoGiaTemDetailView { Date_Working = u.date_working, address_deliver = d.address_deliver, date_deliver = d.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
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

        [ActionAuthorizeAttribute("DonHang")]
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
          
            if (donHang.action == 4)
            {
                donHang.action = 0;
                tbl_OrderTem order = db.tbl_OrderTem.Find(donHang.id);
                order.date_begin_plan = donHang.date_begin_plan;
                order.date_end_plan = donHang.date_end_plan;

                order.update_date = DateTime.Now;
                order.update_user = Session["username"].ToString();
                order.status = donHang.status.Value;
                order.date_begin = DateTime.Now;
               // order.status = 3;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                var queryGiaoGiaCT = from u in db.tbl_OrderTem_BaoGia_Detail
                                     join y in db.tbl_Products on u.sanpam_id equals y.ID_Products
                                     where u.baogia_id.Value.Equals(donHang.BaoGiaTemView.id)
                                     select new BaoGiaTemDetailView { Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
                List<BaoGiaTemDetailView> listSP = queryGiaoGiaCT.ToList<BaoGiaTemDetailView>();
                foreach (var item in listSP)
                {
                    tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail = db.tbl_OrderTem_BaoGia_Detail.Find(item.id);
                   // tbl_OrderTem_BaoGia_Detail.status = 0;
                    tbl_OrderTem_BaoGia_Detail.status = 0;
                    db.Entry(tbl_OrderTem_BaoGia_Detail).State = EntityState.Modified;
                    //FLEXO - BẤM KIM	FLEXO - DÁN	FLEXO - Bế - BK	FLEXO - Bế - DÁN	OFFSET -Bế  - BK	OFFSET - Bế - DÁN
                    if (item.OffsetFlexoProducts.Equals("FLEXO - BẤM KIM"))
                    {
                        tbl_QuyTrinh qt0 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 0, TrangThai = 0, TenBuoc = "Nhận tờ in offset" };
                        db.tbl_QuyTrinh.Add(qt0);
                        tbl_QuyTrinh qt1 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 1, TrangThai = 0, TenBuoc = "Sản xuất giấy tấm" };
                        db.tbl_QuyTrinh.Add(qt1);
                        tbl_QuyTrinh qt2 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 2, TrangThai = 0, TenBuoc = "Bồi" };
                        db.tbl_QuyTrinh.Add(qt2);
                        tbl_QuyTrinh qt3 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 3, TrangThai = 0, TenBuoc = "Xả biến, cán lằn" };
                        db.tbl_QuyTrinh.Add(qt3);
                        tbl_QuyTrinh qt4 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 4, TrangThai = 0, TenBuoc = "In FLEXO" };
                        db.tbl_QuyTrinh.Add(qt4);
                        tbl_QuyTrinh qt5 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 5, TrangThai = 0, TenBuoc = "Bế" };
                        db.tbl_QuyTrinh.Add(qt5);
                        tbl_QuyTrinh qt6 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 6, TrangThai = 0, TenBuoc = "Chập khe" };
                        db.tbl_QuyTrinh.Add(qt6);
                        tbl_QuyTrinh qt7 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 7, TrangThai = 0, TenBuoc = "Bấm kim" };
                        db.tbl_QuyTrinh.Add(qt7);
                        tbl_QuyTrinh qt8 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 8, TrangThai = 0, TenBuoc = "Dán" };
                        db.tbl_QuyTrinh.Add(qt8);

                    }
                    if (item.OffsetFlexoProducts.Equals("FLEXO - DÁN"))
                    {
                        tbl_QuyTrinh qt0 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 0, TrangThai = 0, TenBuoc = "Nhận tờ in offset" };
                        db.tbl_QuyTrinh.Add(qt0);
                        tbl_QuyTrinh qt1 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 1, TrangThai = 0, TenBuoc = "Sản xuất giấy tấm" };
                        db.tbl_QuyTrinh.Add(qt1);
                        tbl_QuyTrinh qt2 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 2, TrangThai = 0, TenBuoc = "Bồi" };
                        db.tbl_QuyTrinh.Add(qt2);
                        tbl_QuyTrinh qt3 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 3, TrangThai = 0, TenBuoc = "Xả biến, cán lằn" };

                        db.tbl_QuyTrinh.Add(qt3);
                        tbl_QuyTrinh qt4 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 4, TrangThai = 0, TenBuoc = "In FLEXO" };
                        db.tbl_QuyTrinh.Add(qt4);
                        tbl_QuyTrinh qt5 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 5, TrangThai = 0, TenBuoc = "Bế" };
                        db.tbl_QuyTrinh.Add(qt5);
                        tbl_QuyTrinh qt6 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 6, TrangThai = 0, TenBuoc = "Chập khe" };
                        db.tbl_QuyTrinh.Add(qt6);
                        tbl_QuyTrinh qt7 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 7, TrangThai = 0, TenBuoc = "Bấm kim" };
                        db.tbl_QuyTrinh.Add(qt7);
                        tbl_QuyTrinh qt8 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 8, TrangThai = 0, TenBuoc = "Dán" };
                        db.tbl_QuyTrinh.Add(qt8);
                    }
                    if (item.OffsetFlexoProducts.Equals("FLEXO - Bế"))
                    {
                        tbl_QuyTrinh qt0 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 0, TrangThai = 0, TenBuoc = "Nhận tờ in offset" };
                        db.tbl_QuyTrinh.Add(qt0);

                        tbl_QuyTrinh qt1 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 1, TrangThai = 0, TenBuoc = "Sản xuất giấy tấm" };
                        db.tbl_QuyTrinh.Add(qt1);

                        tbl_QuyTrinh qt2 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 2, TrangThai = 0, TenBuoc = "Bồi" };
                        db.tbl_QuyTrinh.Add(qt2);

                        tbl_QuyTrinh qt3 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 3, TrangThai = 0, TenBuoc = "Xả biến, cán lằn" };
                        db.tbl_QuyTrinh.Add(qt3);

                        tbl_QuyTrinh qt4 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 4, TrangThai = 0, TenBuoc = "In FLEXO" };
                        db.tbl_QuyTrinh.Add(qt4);
                        tbl_QuyTrinh qt5 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 5, TrangThai = 0, TenBuoc = "Bế" };
                        db.tbl_QuyTrinh.Add(qt5);
                        tbl_QuyTrinh qt6 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 6, TrangThai = 0, TenBuoc = "Chập khe" };
                        db.tbl_QuyTrinh.Add(qt6);
                        tbl_QuyTrinh qt7 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 7, TrangThai = 0, TenBuoc = "Bấm kim" };
                        db.tbl_QuyTrinh.Add(qt7);
                        tbl_QuyTrinh qt8 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 8, TrangThai = 0, TenBuoc = "Dán" };
                        db.tbl_QuyTrinh.Add(qt8);
                    }
                    if (item.OffsetFlexoProducts.Equals("FLEXO - Bế - DÁN"))
                    {

                        tbl_QuyTrinh qt0 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 0, TrangThai = 0, TenBuoc = "Nhận tờ in offset" };
                        db.tbl_QuyTrinh.Add(qt0);

                        tbl_QuyTrinh qt1 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 1, TrangThai = 0, TenBuoc = "Sản xuất giấy tấm" };
                        db.tbl_QuyTrinh.Add(qt1);

                        tbl_QuyTrinh qt2 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 2, TrangThai = 0, TenBuoc = "Bồi" };
                        db.tbl_QuyTrinh.Add(qt2);

                        tbl_QuyTrinh qt3 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 3, TrangThai = 0, TenBuoc = "Xả biến, cán lằn" };
                        db.tbl_QuyTrinh.Add(qt3);

                        tbl_QuyTrinh qt4 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 4, TrangThai = 0, TenBuoc = "In FLEXO" };
                        db.tbl_QuyTrinh.Add(qt4);
                        tbl_QuyTrinh qt5 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 5, TrangThai = 0, TenBuoc = "Bế" };
                        db.tbl_QuyTrinh.Add(qt5);
                        tbl_QuyTrinh qt6 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 6, TrangThai = 0, TenBuoc = "Chập khe" };
                        db.tbl_QuyTrinh.Add(qt6);


                        tbl_QuyTrinh qt7 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 7, TrangThai = 0, TenBuoc = "Bấm kim" };
                        db.tbl_QuyTrinh.Add(qt7);

                        tbl_QuyTrinh qt8 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 8, TrangThai = 0, TenBuoc = "Dán" };
                        db.tbl_QuyTrinh.Add(qt8);
                    }
                    if (item.OffsetFlexoProducts.Equals("OFFSET -Bế - BK"))
                    {
                        tbl_QuyTrinh qt0 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 0, TrangThai = 0, TenBuoc = "Nhận tờ in offset" };
                        db.tbl_QuyTrinh.Add(qt0);
                        tbl_QuyTrinh qt1 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 1, TrangThai = 0, TenBuoc = "Sản xuất giấy tấm" };
                        db.tbl_QuyTrinh.Add(qt1);
                        tbl_QuyTrinh qt2 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 2, TrangThai = 0, TenBuoc = "Bồi" };
                        db.tbl_QuyTrinh.Add(qt2);

                        tbl_QuyTrinh qt3 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 3, TrangThai = 0, TenBuoc = "Xả biến, cán lằn" };
                        db.tbl_QuyTrinh.Add(qt3);
                        tbl_QuyTrinh qt4 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 4, TrangThai = 0, TenBuoc = "In FLEXO" };
                        db.tbl_QuyTrinh.Add(qt4);

                        tbl_QuyTrinh qt5 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 5, TrangThai = 0, TenBuoc = "Bế" };
                        db.tbl_QuyTrinh.Add(qt5);

                        tbl_QuyTrinh qt6 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 6, TrangThai = 0, TenBuoc = "Chập khe" };
                        db.tbl_QuyTrinh.Add(qt6);

                        tbl_QuyTrinh qt7 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 7, TrangThai = 0, TenBuoc = "Bấm kim" };
                        db.tbl_QuyTrinh.Add(qt7);
                        tbl_QuyTrinh qt8 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 8, TrangThai = 0, TenBuoc = "Dán" };
                        db.tbl_QuyTrinh.Add(qt8);
                    }
                    if (item.OffsetFlexoProducts.Equals("OFFSET - Bế - DÁN"))
                    {
                        tbl_QuyTrinh qt0 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 0, TrangThai = 0, TenBuoc = "Nhận tờ in offset" };
                        db.tbl_QuyTrinh.Add(qt0);
                        tbl_QuyTrinh qt1 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 1, TrangThai = 0, TenBuoc = "Sản xuất giấy tấm" };
                        db.tbl_QuyTrinh.Add(qt1);
                        tbl_QuyTrinh qt2 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 2, TrangThai = 0, TenBuoc = "Bồi" };
                        db.tbl_QuyTrinh.Add(qt2);

                        tbl_QuyTrinh qt3 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 3, TrangThai = 0, TenBuoc = "Xả biến, cán lằn" };
                        db.tbl_QuyTrinh.Add(qt3);
                        tbl_QuyTrinh qt4 = new tbl_QuyTrinh { ThucHien = 0, ID_BaoGiaDetail = item.id, ThuTu = 4, TrangThai = 0, TenBuoc = "In FLEXO" };
                        db.tbl_QuyTrinh.Add(qt4);

                        tbl_QuyTrinh qt5 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 5, TrangThai = 0, TenBuoc = "Bế" };
                        db.tbl_QuyTrinh.Add(qt5);

                        tbl_QuyTrinh qt6 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 6, TrangThai = 0, TenBuoc = "Chập khe" };
                        db.tbl_QuyTrinh.Add(qt6);

                        tbl_QuyTrinh qt7 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 7, TrangThai = 0, TenBuoc = "Bấm kim" };
                        db.tbl_QuyTrinh.Add(qt7);

                        tbl_QuyTrinh qt8 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 8, TrangThai = 0, TenBuoc = "Dán" };
                        db.tbl_QuyTrinh.Add(qt8);
                    }
                    tbl_QuyTrinh qt9 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 9, TrangThai = 0, TenBuoc = "Đóng gói" };
                    db.tbl_QuyTrinh.Add(qt9);
                    tbl_QuyTrinh qt10 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 10, TrangThai = 0, TenBuoc = "Giao hàng" };
                    db.tbl_QuyTrinh.Add(qt10);
                    tbl_QuyTrinh qt11 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 11, TrangThai = 0, TenBuoc = "Thanh toán" };
                    db.tbl_QuyTrinh.Add(qt11);
                    tbl_QuyTrinh qt12 = new tbl_QuyTrinh { ThucHien = 1, ID_BaoGiaDetail = item.id, ThuTu = 12, TrangThai = 0, TenBuoc = "Kết thúc đơn hàng" };
                    db.tbl_QuyTrinh.Add(qt12);
                  
                }


                var q = from a in db.tbl_OrderTem_BaoGia_Detail where a.baogia_id.Value == donHang.BaoGiaTemView.id select a;
                List<tbl_OrderTem_BaoGia_Detail> listDetail = q.ToList();
                var queryMax = (from u in db.tbl_Stack
                                orderby u.index_view descending
                                select u).Take(1);
                int maxStackView = queryMax.ToList().Count == 0 ? 1 : queryMax.ToList()[0].index_view.Value;

                for (int i = 0; i < listDetail.Count; i++)
                {
                    var item = listDetail[i];
                     item.status = 0;
                     item.date_working = DateTime.Now;
                    db.Entry(item).State = EntityState.Modified;
                    tbl_Stack tbl_Stack = new tbl_Stack { baoGia_detail_id = item.id, date_create = DateTime.Now, index_view = maxStackView + i + 1 };
                    db.tbl_Stack.Add(tbl_Stack);

                }
                db.SaveChanges();

                return RedirectToAction("LichSanXuat", "SanXuat");
            }
            DonHangView d = new DonHangView();
            d.action = donHang.action;
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
                                     select new BaoGiaTemDetailView { Date_Working = u.date_working,address_deliver = d.address_deliver, date_deliver = d.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
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

        [ActionAuthorizeAttribute("DonHang")]      
        public ActionResult Delete(int? id)
        {
          
            tbl_OrderTem tbl_OrderTem = db.tbl_OrderTem.Find(id);
            db.tbl_OrderTem.Remove(tbl_OrderTem);
            db.SaveChanges();
            return RedirectToAction("ThongTinDonHang", new { id = tbl_OrderTem.customer_id });
        }

        [ActionAuthorizeAttribute("DonHang")]
        public ActionResult DeleteBaoGia(int? id)
        {
           

            tbl_OrderTem tbl_OrderTem = db.tbl_OrderTem.Find(id);
            db.tbl_OrderTem.Remove(tbl_OrderTem);
            db.SaveChanges();
            return RedirectToAction("IndexBaoGia", new { id = tbl_OrderTem.customer_id });
        }


        [ActionAuthorizeAttribute("BaoGia")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_OrderTem tbl_OrderTem = db.tbl_OrderTem.Find(id);
            db.tbl_OrderTem.Remove(tbl_OrderTem);
            db.SaveChanges();
            return RedirectToAction("IndexBaoGia");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //[ActionAuthorizeAttribute("BaoGia,DonHang")]
        public ActionResult PrintOrder(int id)
        {

         
            tbl_OrderTem_BaoGia item = db.tbl_OrderTem_BaoGia.Find(id);
            tbl_OrderTem tbl_OrderTem = db.tbl_OrderTem.Find(item.order_id);
            DonHangView d = new DonHangView();
            d.customer_id = tbl_OrderTem.customer_id;
            d.code = tbl_OrderTem.code;
            d.date_begin = tbl_OrderTem.date_begin;
            d.date_end = tbl_OrderTem.date_end;
            d.id = tbl_OrderTem.id;
            d.status = tbl_OrderTem.status;
            d.date_begin_plan = tbl_OrderTem.date_begin_plan;
            d.date_end_plan = tbl_OrderTem.date_end_plan;
            d.date_deliver = tbl_OrderTem.date_deliver;
            d.address_deliver = tbl_OrderTem.address_deliver;
            d.datenumber = tbl_OrderTem.datenumber ;
         



            BaoGiaTemView temBG = new BaoGiaTemView { datenumber =  tbl_OrderTem.datenumber, date_deliver = tbl_OrderTem.date_deliver, address_deliver = tbl_OrderTem.address_deliver, note = item.note, date_begin = item.date_begin, date_end = item.date_end, id = item.id, order_id = item.order_id, status = item.status, total_money = item.total_money };
            var queryGiaoGiaCT = from u in db.tbl_OrderTem_BaoGia_Detail
                                 join y in db.tbl_Products on u.sanpam_id equals y.ID_Products
                                 where u.baogia_id.Value.Equals(temBG.id)
                                 select new BaoGiaTemDetailView {InFlexoProducts=y.InFlexoProducts,LoaiSongProducts=y.LoaiSongProducts, loai_design =u.loai_design,id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
            temBG.BaoGiaTemDetailViews = queryGiaoGiaCT.ToList<BaoGiaTemDetailView>();

            d.BaoGiaTemView = temBG;
            d.BaoGiaTemView.buildPrint();
            var list = from tt in db.tbl_Customers where tt.IDCustomers == d.customer_id select tt;
            d.Customer = list.ToList()[0];
            return View(d);

        }
        public ActionResult GeneratePDF(int id)
        {
            return new Rotativa.ActionAsPdf("PrintOrder",new { id=id});
        }


        [ActionAuthorizeAttribute("BaoGia")]
        [HttpPost]
        public ActionResult UpdateDesign(int id, DateTime date, string loai)
        {
            tbl_OrderTem_BaoGia_Detail item = db.tbl_OrderTem_BaoGia_Detail.Find(id);
            item.design_date = date;
            item.design = 2;
            item.loai_design = loai;
            //item.design_img = item.id + "_";
            //file.SaveAs(Server.MapPath("~/Upload/ThietKe") + "/" + item.id + "_" + file.FileName);
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            return Json(item);
        }

        [ActionAuthorizeAttribute("BaoGia")]
        public ActionResult IndexDH(int id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var qr = (from data in db.tbl_OrderTem
                      join cus in db.tbl_Customers on data.customer_id equals cus.IDCustomers
                      where data.status >= 0 && cus.IDCustomers==id
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
                var lisBG = queryBaoGia.ToList<tbl_OrderTem_BaoGia>();
                foreach (var item in lisBG)
                {
                    BaoGiaTemView temBG = new BaoGiaTemView { commission = item.commission, commission_money = item.commission_monney, note = item.note, date_begin = item.date_begin, date_end = item.date_end, id = item.id, order_id = item.order_id, status = item.status, total_money = item.total_money };

                    var queryGiaoGiaCT = from u in db.tbl_OrderTem_BaoGia_Detail
                                         join y in db.tbl_Products on u.sanpam_id equals y.ID_Products
                                         where u.baogia_id.Value.Equals(temBG.id)
                                         select new BaoGiaTemDetailView { loai_design = u.loai_design, Step_Flow = u.step_index, Status = u.status, Code_Detail = u.code_detail, Date_Working = u.date_working, address_deliver = itemBG.address_deliver, date_deliver = itemBG.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
                    temBG.BaoGiaTemDetailViews = queryGiaoGiaCT.ToList<BaoGiaTemDetailView>();
                    itemBG.BaoGiaTemView = temBG;


                    foreach (var itemSP in itemBG.BaoGiaTemView.BaoGiaTemDetailViews)
                    {
                        if (itemSP.Step_Flow.HasValue)
                        {
                            var queryQT = from u in db.tbl_QuyTrinh where u.ID_BaoGiaDetail.Equals(itemSP.id) && u.ThuTu.Value.Equals(itemSP.Step_Flow.Value) orderby u.ThuTu ascending select u;
                            itemSP.QuyTrinhs = queryQT.ToList<tbl_QuyTrinh>();
                        }

                    }
                    break;
                }
            }

            return View(list);
        }

        [ActionAuthorizeAttribute("DonHang")]
        [HttpPost]
        public JsonResult UpdatePauseOrder(int id, String note, int state)
        {
            try
            {
                if (state == 2)
                {
                    var query = (from a in db.tbl_OrderTemPause where a.order_id.Value.Equals(id) orderby a.id descending select a).Take(1);
                    var list = query.ToList();
                    if (list.Count > 0)
                    {
                        var cItem = list[0];
                        cItem.date_end = DateTime.Now;
                        cItem.status = state;
                        db.Entry(cItem).State = EntityState.Modified;
                        tbl_OrderTem tbl_OrderTem = db.tbl_OrderTem.Find(id);
                        tbl_OrderTem.status_pause = 0;
                        db.Entry(tbl_OrderTem).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                if (state == 1)
                {
                    tbl_OrderTem tbl_OrderTem = db.tbl_OrderTem.Find(id);
                    tbl_OrderTem.status_pause = 1;
                    db.Entry(tbl_OrderTem).State = EntityState.Modified;
                    tbl_OrderTemPause citem = new tbl_OrderTemPause { order_id = id, date_begin = DateTime.Now,  note = note, status = 1 };
                    db.tbl_OrderTemPause.Add(citem);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {

                return Json("Error - " + e.Message);
            }

            return Json("Success");
        }

        public ActionResult Report()
        {
            var model = db.Database.SqlQuery<Report>(@"
        SELECT tbl_OrderTem.code'codeProducts',tbl_Products.NameProducts'nameProducts',
        (SELECT  DATEDIFF(MI,tbl_QuyTrinh1.NgayBatDau_TT,tbl_QuyTrinh1.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh1
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail1 on tbl_QuyTrinh1.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail1.id
          left join tbl_Products  tbl_Products1 on tbl_Products1.ID_Products=tbl_OrderTem_BaoGia_Detail1.sanpam_id
          left join tbl_OrderTem tbl_OrderTem1 on tbl_OrderTem1.id=tbl_OrderTem_BaoGia_Detail1.baogia_id
          where  tbl_QuyTrinh1.TenBuoc in(N'Nhận tờ in offset') and tbl_QuyTrinh1.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'NhanToInOffset',
          (SELECT DATEDIFF(MI,tbl_QuyTrinh2.NgayBatDau_TT,tbl_QuyTrinh2.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh2
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail2 on tbl_QuyTrinh2.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail2.id
          left join tbl_Products  tbl_Products2 on tbl_Products2.ID_Products=tbl_OrderTem_BaoGia_Detail2.sanpam_id
          left join tbl_OrderTem tbl_OrderTem2 on tbl_OrderTem2.id=tbl_OrderTem_BaoGia_Detail2.baogia_id
          where  tbl_QuyTrinh2.TenBuoc in(N'Sản xuất giấy tấm')and tbl_QuyTrinh2.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'SanXuatGiayTam',
          (SELECT DATEDIFF(MI,tbl_QuyTrinh3.NgayBatDau_TT,tbl_QuyTrinh3.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh3
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail3 on tbl_QuyTrinh3.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail3.id
          left join tbl_Products  tbl_Products3 on tbl_Products3.ID_Products=tbl_OrderTem_BaoGia_Detail3.sanpam_id
          left join tbl_OrderTem tbl_OrderTem3 on tbl_OrderTem3.id=tbl_OrderTem_BaoGia_Detail3.baogia_id
          where  tbl_QuyTrinh3.TenBuoc in(N'Bồi')and tbl_QuyTrinh3.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'Boi',
          (SELECT DATEDIFF(MI,tbl_QuyTrinh4.NgayBatDau_TT,tbl_QuyTrinh4.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh4
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail4 on tbl_QuyTrinh4.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail4.id
          left join tbl_Products  tbl_Products4 on tbl_Products4.ID_Products=tbl_OrderTem_BaoGia_Detail4.sanpam_id
          left join tbl_OrderTem tbl_OrderTem4 on tbl_OrderTem4.id=tbl_OrderTem_BaoGia_Detail4.baogia_id
          where  tbl_QuyTrinh4.TenBuoc in(N'Xả biến, cán lằn')and tbl_QuyTrinh4.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'XaBienCanLan',
          (SELECT DATEDIFF(MI,tbl_QuyTrinh5.NgayBatDau_TT,tbl_QuyTrinh5.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh5
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail5 on tbl_QuyTrinh5.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail5.id
          left join tbl_Products  tbl_Products5 on tbl_Products5.ID_Products=tbl_OrderTem_BaoGia_Detail5.sanpam_id
          left join tbl_OrderTem tbl_OrderTem5 on tbl_OrderTem5.id=tbl_OrderTem_BaoGia_Detail5.baogia_id
          where tbl_QuyTrinh5.TenBuoc in(N'In FLEXO')and tbl_QuyTrinh5.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'InFlexo',
          (SELECT DATEDIFF(MI,tbl_QuyTrinh6.NgayBatDau_TT,tbl_QuyTrinh6.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh6
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail6 on tbl_QuyTrinh6.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail6.id
          left join tbl_Products  tbl_Products6 on tbl_Products6.ID_Products=tbl_OrderTem_BaoGia_Detail6.sanpam_id
          left join tbl_OrderTem tbl_OrderTem6 on tbl_OrderTem6.id=tbl_OrderTem_BaoGia_Detail6.baogia_id
          where tbl_QuyTrinh6.TenBuoc in(N'Bế')and tbl_QuyTrinh6.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'Be',
          (SELECT DATEDIFF(MI,tbl_QuyTrinh7.NgayBatDau_TT,tbl_QuyTrinh7.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh7
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail7 on tbl_QuyTrinh7.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail7.id
          left join tbl_Products  tbl_Products7 on tbl_Products7.ID_Products=tbl_OrderTem_BaoGia_Detail7.sanpam_id
          left join tbl_OrderTem tbl_OrderTem7 on tbl_OrderTem7.id=tbl_OrderTem_BaoGia_Detail7.baogia_id
          where  tbl_QuyTrinh7.TenBuoc in(N'Chập khe')and tbl_QuyTrinh7.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'ChapKhe',
          (SELECT DATEDIFF(MI,tbl_QuyTrinh8.NgayBatDau_TT,tbl_QuyTrinh8.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh8
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail8 on tbl_QuyTrinh8.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail8.id
          left join tbl_Products  tbl_Products8 on tbl_Products8.ID_Products=tbl_OrderTem_BaoGia_Detail8.sanpam_id
          left join tbl_OrderTem tbl_OrderTem8 on tbl_OrderTem8.id=tbl_OrderTem_BaoGia_Detail8.baogia_id
          where  tbl_QuyTrinh8.TenBuoc in(N'Bấm kim')and tbl_QuyTrinh8.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'BamKim',
          (SELECT DATEDIFF(MI,tbl_QuyTrinh9.NgayBatDau_TT,tbl_QuyTrinh9.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh9
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail9 on tbl_QuyTrinh9.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail9.id
          left join tbl_Products  tbl_Products9 on tbl_Products9.ID_Products=tbl_OrderTem_BaoGia_Detail9.sanpam_id
          left join tbl_OrderTem tbl_OrderTem9 on tbl_OrderTem9.id=tbl_OrderTem_BaoGia_Detail9.baogia_id
          where  tbl_QuyTrinh9.TenBuoc in(N'Dán')and tbl_QuyTrinh9.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'Dan',
          (SELECT DATEDIFF(MI,tbl_QuyTrinh10.NgayBatDau_TT,tbl_QuyTrinh10.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh10
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail10 on tbl_QuyTrinh10.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail10.id
          left join tbl_Products  tbl_Products10 on tbl_Products10.ID_Products=tbl_OrderTem_BaoGia_Detail10.sanpam_id
          left join tbl_OrderTem tbl_OrderTem10 on tbl_OrderTem10.id=tbl_OrderTem_BaoGia_Detail10.baogia_id
          where  tbl_QuyTrinh10.TenBuoc in(N'Đóng gói')and tbl_QuyTrinh10.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'DongGoi',
          (SELECT DATEDIFF(MI,tbl_QuyTrinh11.NgayBatDau_TT,tbl_QuyTrinh11.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh11
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail11 on tbl_QuyTrinh11.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail11.id
          left join tbl_Products  tbl_Products11 on tbl_Products11.ID_Products=tbl_OrderTem_BaoGia_Detail11.sanpam_id
          left join tbl_OrderTem tbl_OrderTem11 on tbl_OrderTem11.id=tbl_OrderTem_BaoGia_Detail11.baogia_id
          where  tbl_QuyTrinh11.TenBuoc in(N'Giao hàng')and tbl_QuyTrinh11.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'GiaoHang',
          (SELECT DATEDIFF(MI,tbl_QuyTrinh12.NgayBatDau_TT,tbl_QuyTrinh12.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh12
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail12 on tbl_QuyTrinh12.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail12.id
          left join tbl_Products  tbl_Products12 on tbl_Products12.ID_Products=tbl_OrderTem_BaoGia_Detail12.sanpam_id
          left join tbl_OrderTem tbl_OrderTem12 on tbl_OrderTem12.id=tbl_OrderTem_BaoGia_Detail12.baogia_id
          where tbl_QuyTrinh12.TenBuoc in(N'Thanh toán')and tbl_QuyTrinh12.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'ThanhToan',
            (SELECT DATEDIFF(MI,tbl_QuyTrinh13.NgayBatDau_TT,tbl_QuyTrinh13.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh13
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail13 on tbl_QuyTrinh13.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail13.id
          left join tbl_Products  tbl_Products13 on tbl_Products13.ID_Products=tbl_OrderTem_BaoGia_Detail13.sanpam_id
          left join tbl_OrderTem tbl_OrderTem13 on tbl_OrderTem13.id=tbl_OrderTem_BaoGia_Detail13.baogia_id
          where  tbl_QuyTrinh13.TenBuoc in(N'Kết thúc đơn hàng')and tbl_QuyTrinh13.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'KetThucDonHang',
          tbl_Products.SolopProducts'SoLop',
          tbl_Products.LoaiSongProducts'LoaiSong',
          LoaigiayProducts'LoaiGiay',
          case when InFlexoProducts =1 then '1 màu' when InFlexoProducts =2 then '2 màu' when InFlexoProducts =3 then ' 3 màu'  when InFlexoProducts =4 then '4 màu' when InFlexoProducts =5 then '5 màu' when InFlexoProducts =6 then 'Không in' end    'InFlexoProduct',
          QuyCachProducts'QuyCach',
          tbl_OrderTem_BaoGia_Detail.soluong'SoLuong',
          tbl_OrderTem_BaoGia_Detail.[money]'Money'
          FROM tbl_QuyTrinh
          left join tbl_OrderTem_BaoGia_Detail on tbl_QuyTrinh.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail.id
          left join tbl_Products on tbl_Products.ID_Products=tbl_OrderTem_BaoGia_Detail.sanpam_id
          left join tbl_OrderTem  on tbl_OrderTem.id=tbl_OrderTem_BaoGia_Detail.baogia_id

          group by tbl_Products.CreatedDateProducts, tbl_OrderTem.code,tbl_Products.NameProducts,tbl_QuyTrinh.ID_BaoGiaDetail,tbl_Products.SolopProducts,tbl_Products.LoaiSongProducts,LoaigiayProducts,InFlexoProducts,QuyCachProducts,tbl_OrderTem_BaoGia_Detail.soluong,tbl_OrderTem_BaoGia_Detail.[money]
          order by tbl_Products.CreatedDateProducts desc").ToList();
                    return View(model);
                }


         public ActionResult ExportReport()
                {
                    var model = db.Database.SqlQuery<Report>(@"
        SELECT tbl_OrderTem.code'codeProducts',tbl_Products.NameProducts'nameProducts',
        (SELECT  DATEDIFF(MI,tbl_QuyTrinh1.NgayBatDau_TT,tbl_QuyTrinh1.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh1
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail1 on tbl_QuyTrinh1.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail1.id
          left join tbl_Products  tbl_Products1 on tbl_Products1.ID_Products=tbl_OrderTem_BaoGia_Detail1.sanpam_id
          left join tbl_OrderTem tbl_OrderTem1 on tbl_OrderTem1.id=tbl_OrderTem_BaoGia_Detail1.baogia_id
          where  tbl_QuyTrinh1.TenBuoc in(N'Nhận tờ in offset') and tbl_QuyTrinh1.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'NhanToInOffset',
          (SELECT DATEDIFF(MI,tbl_QuyTrinh2.NgayBatDau_TT,tbl_QuyTrinh2.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh2
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail2 on tbl_QuyTrinh2.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail2.id
          left join tbl_Products  tbl_Products2 on tbl_Products2.ID_Products=tbl_OrderTem_BaoGia_Detail2.sanpam_id
          left join tbl_OrderTem tbl_OrderTem2 on tbl_OrderTem2.id=tbl_OrderTem_BaoGia_Detail2.baogia_id
          where  tbl_QuyTrinh2.TenBuoc in(N'Sản xuất giấy tấm')and tbl_QuyTrinh2.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'SanXuatGiayTam',
          (SELECT DATEDIFF(MI,tbl_QuyTrinh3.NgayBatDau_TT,tbl_QuyTrinh3.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh3
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail3 on tbl_QuyTrinh3.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail3.id
          left join tbl_Products  tbl_Products3 on tbl_Products3.ID_Products=tbl_OrderTem_BaoGia_Detail3.sanpam_id
          left join tbl_OrderTem tbl_OrderTem3 on tbl_OrderTem3.id=tbl_OrderTem_BaoGia_Detail3.baogia_id
          where  tbl_QuyTrinh3.TenBuoc in(N'Bồi')and tbl_QuyTrinh3.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'Boi',
          (SELECT DATEDIFF(MI,tbl_QuyTrinh4.NgayBatDau_TT,tbl_QuyTrinh4.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh4
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail4 on tbl_QuyTrinh4.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail4.id
          left join tbl_Products  tbl_Products4 on tbl_Products4.ID_Products=tbl_OrderTem_BaoGia_Detail4.sanpam_id
          left join tbl_OrderTem tbl_OrderTem4 on tbl_OrderTem4.id=tbl_OrderTem_BaoGia_Detail4.baogia_id
          where  tbl_QuyTrinh4.TenBuoc in(N'Xả biến, cán lằn')and tbl_QuyTrinh4.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'XaBienCanLan',
          (SELECT DATEDIFF(MI,tbl_QuyTrinh5.NgayBatDau_TT,tbl_QuyTrinh5.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh5
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail5 on tbl_QuyTrinh5.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail5.id
          left join tbl_Products  tbl_Products5 on tbl_Products5.ID_Products=tbl_OrderTem_BaoGia_Detail5.sanpam_id
          left join tbl_OrderTem tbl_OrderTem5 on tbl_OrderTem5.id=tbl_OrderTem_BaoGia_Detail5.baogia_id
          where tbl_QuyTrinh5.TenBuoc in(N'In FLEXO')and tbl_QuyTrinh5.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'InFlexo',
          (SELECT DATEDIFF(MI,tbl_QuyTrinh6.NgayBatDau_TT,tbl_QuyTrinh6.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh6
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail6 on tbl_QuyTrinh6.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail6.id
          left join tbl_Products  tbl_Products6 on tbl_Products6.ID_Products=tbl_OrderTem_BaoGia_Detail6.sanpam_id
          left join tbl_OrderTem tbl_OrderTem6 on tbl_OrderTem6.id=tbl_OrderTem_BaoGia_Detail6.baogia_id
          where tbl_QuyTrinh6.TenBuoc in(N'Bế')and tbl_QuyTrinh6.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'Be',
          (SELECT DATEDIFF(MI,tbl_QuyTrinh7.NgayBatDau_TT,tbl_QuyTrinh7.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh7
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail7 on tbl_QuyTrinh7.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail7.id
          left join tbl_Products  tbl_Products7 on tbl_Products7.ID_Products=tbl_OrderTem_BaoGia_Detail7.sanpam_id
          left join tbl_OrderTem tbl_OrderTem7 on tbl_OrderTem7.id=tbl_OrderTem_BaoGia_Detail7.baogia_id
          where  tbl_QuyTrinh7.TenBuoc in(N'Chập khe')and tbl_QuyTrinh7.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'ChapKhe',
          (SELECT DATEDIFF(MI,tbl_QuyTrinh8.NgayBatDau_TT,tbl_QuyTrinh8.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh8
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail8 on tbl_QuyTrinh8.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail8.id
          left join tbl_Products  tbl_Products8 on tbl_Products8.ID_Products=tbl_OrderTem_BaoGia_Detail8.sanpam_id
          left join tbl_OrderTem tbl_OrderTem8 on tbl_OrderTem8.id=tbl_OrderTem_BaoGia_Detail8.baogia_id
          where  tbl_QuyTrinh8.TenBuoc in(N'Bấm kim')and tbl_QuyTrinh8.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'BamKim',
          (SELECT DATEDIFF(MI,tbl_QuyTrinh9.NgayBatDau_TT,tbl_QuyTrinh9.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh9
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail9 on tbl_QuyTrinh9.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail9.id
          left join tbl_Products  tbl_Products9 on tbl_Products9.ID_Products=tbl_OrderTem_BaoGia_Detail9.sanpam_id
          left join tbl_OrderTem tbl_OrderTem9 on tbl_OrderTem9.id=tbl_OrderTem_BaoGia_Detail9.baogia_id
          where  tbl_QuyTrinh9.TenBuoc in(N'Dán')and tbl_QuyTrinh9.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'Dan',
          (SELECT DATEDIFF(MI,tbl_QuyTrinh10.NgayBatDau_TT,tbl_QuyTrinh10.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh10
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail10 on tbl_QuyTrinh10.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail10.id
          left join tbl_Products  tbl_Products10 on tbl_Products10.ID_Products=tbl_OrderTem_BaoGia_Detail10.sanpam_id
          left join tbl_OrderTem tbl_OrderTem10 on tbl_OrderTem10.id=tbl_OrderTem_BaoGia_Detail10.baogia_id
          where  tbl_QuyTrinh10.TenBuoc in(N'Đóng gói')and tbl_QuyTrinh10.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'DongGoi',
          (SELECT DATEDIFF(MI,tbl_QuyTrinh11.NgayBatDau_TT,tbl_QuyTrinh11.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh11
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail11 on tbl_QuyTrinh11.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail11.id
          left join tbl_Products  tbl_Products11 on tbl_Products11.ID_Products=tbl_OrderTem_BaoGia_Detail11.sanpam_id
          left join tbl_OrderTem tbl_OrderTem11 on tbl_OrderTem11.id=tbl_OrderTem_BaoGia_Detail11.baogia_id
          where  tbl_QuyTrinh11.TenBuoc in(N'Giao hàng')and tbl_QuyTrinh11.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'GiaoHang',
          (SELECT DATEDIFF(MI,tbl_QuyTrinh12.NgayBatDau_TT,tbl_QuyTrinh12.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh12
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail12 on tbl_QuyTrinh12.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail12.id
          left join tbl_Products  tbl_Products12 on tbl_Products12.ID_Products=tbl_OrderTem_BaoGia_Detail12.sanpam_id
          left join tbl_OrderTem tbl_OrderTem12 on tbl_OrderTem12.id=tbl_OrderTem_BaoGia_Detail12.baogia_id
          where tbl_QuyTrinh12.TenBuoc in(N'Thanh toán')and tbl_QuyTrinh12.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'ThanhToan',
            (SELECT DATEDIFF(MI,tbl_QuyTrinh13.NgayBatDau_TT,tbl_QuyTrinh13.NgayKetThuc_TT)
          FROM tbl_QuyTrinh tbl_QuyTrinh13
          left join tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail13 on tbl_QuyTrinh13.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail13.id
          left join tbl_Products  tbl_Products13 on tbl_Products13.ID_Products=tbl_OrderTem_BaoGia_Detail13.sanpam_id
          left join tbl_OrderTem tbl_OrderTem13 on tbl_OrderTem13.id=tbl_OrderTem_BaoGia_Detail13.baogia_id
          where  tbl_QuyTrinh13.TenBuoc in(N'Kết thúc đơn hàng')and tbl_QuyTrinh13.ID_BaoGiaDetail=tbl_QuyTrinh.ID_BaoGiaDetail
          )'KetThucDonHang',
          tbl_Products.SolopProducts'SoLop',
          tbl_Products.LoaiSongProducts'LoaiSong',
          LoaigiayProducts'LoaiGiay',
          case when InFlexoProducts =1 then '1 màu' when InFlexoProducts =2 then '2 màu' when InFlexoProducts =3 then ' 3 màu'  when InFlexoProducts =4 then '4 màu' when InFlexoProducts =5 then '5 màu' when InFlexoProducts =6 then 'Không in' end    'InFlexoProduct',
          QuyCachProducts'QuyCach',
          tbl_OrderTem_BaoGia_Detail.soluong'SoLuong',
          tbl_OrderTem_BaoGia_Detail.[money]'Money'
          FROM tbl_QuyTrinh
          left join tbl_OrderTem_BaoGia_Detail on tbl_QuyTrinh.ID_BaoGiaDetail=tbl_OrderTem_BaoGia_Detail.id
          left join tbl_Products on tbl_Products.ID_Products=tbl_OrderTem_BaoGia_Detail.sanpam_id
          left join tbl_OrderTem  on tbl_OrderTem.id=tbl_OrderTem_BaoGia_Detail.baogia_id

          group by tbl_Products.CreatedDateProducts, tbl_OrderTem.code,tbl_Products.NameProducts,tbl_QuyTrinh.ID_BaoGiaDetail,tbl_Products.SolopProducts,tbl_Products.LoaiSongProducts,LoaigiayProducts,InFlexoProducts,QuyCachProducts,tbl_OrderTem_BaoGia_Detail.soluong,tbl_OrderTem_BaoGia_Detail.[money]
          order by tbl_Products.CreatedDateProducts desc").ToList();
            GridView gv = new GridView();
            gv.DataSource = model;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Marklist.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Report");
         
        }
    }
}
