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
    public class tbl_OrderTemController : Controller
    {
        private NhutLongCompanyEntities db = new NhutLongCompanyEntities();

        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var qr = (from data in db.tbl_OrderTem
                      join cus in db.tbl_Customers on data.customer_id equals cus.IDCustomers
                      where data.status >=0 orderby data.status ascending
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
                                         select new BaoGiaTemDetailView {Step_Flow=u.step_index, Status=u.status, Code_Detail=u.code_detail, Date_Working = u.date_working, date_deliver = itemBG.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
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


        public ActionResult IndexBaoGia()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var qr = (from data in db.tbl_OrderTem
                      join cus in db.tbl_Customers on data.customer_id equals cus.IDCustomers
                      where data.status ==-1
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

        // GET: tbl_OrderTem/Details/5
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
            tbl_OrderTem tbl_OrderTem = db.tbl_OrderTem.Find(id);
            if (tbl_OrderTem == null)
            {
                return HttpNotFound();
            }
            return View(tbl_OrderTem);
        }

        // GET: tbl_OrderTem/Create
        public ActionResult Create(int? id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            DonHangView d = new DonHangView();
            d.customer_id = id;
           
            var queryMax = (from u in db.tbl_OrderTem
                            orderby u.id descending
                            select u).Take(1);
            int max = queryMax.ToList().Count == 0 ? 1 : queryMax.ToList()[0].id + 1;
            d.code = String.Format("BG_N{0}T{1}N{2}_{3}", +DateTime.Now.Year, DateTime.Now.Month.ToString("00"), DateTime.Now.Day.ToString("00"), max.ToString("000"));
            if (id.HasValue)
            {
                var list = from tt in db.tbl_Customers where tt.IDCustomers == id.Value select tt;
                d.Customer = list.ToList()[0];
            }
            d.tbl_Customers = db.tbl_Customers.ToList();
            return View(d);
        }

        // POST: tbl_OrderTem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DonHangView donHang)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            donHang.status = -1;
            donHang.BaoGiaTemView.status = 0;
            tbl_OrderTem temValue = new tbl_OrderTem { create_date = DateTime.Now, create_user = Session["username"].ToString(), customer_id = donHang.customer_id, code = donHang.code, date_begin = donHang.date_begin, date_end = donHang.date_end, status = donHang.status, id = donHang.id };
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
                tbl_Products itemP = new tbl_Products
                {
                    CodeProducts = "",
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
                    StatusProducts = item.StatusProducts
                };
                itemP = db.tbl_Products.Add(itemP);
                db.SaveChanges();
                item.ID_Products = itemP.ID_Products;
             
                
                tbl_OrderTem_BaoGia_Detail detail = new tbl_OrderTem_BaoGia_Detail { code_detail = donHang.code + "_" + countindex.ToString("00"), design = item.Design, baogia_id = donHang.BaoGiaTemView.id, money = double.Parse(item.GiaProducts), soluong = item.SoLuong, sanpam_id = itemP.ID_Products };
                db.tbl_OrderTem_BaoGia_Detail.Add(detail);
              
                db.SaveChanges();
            }
            return RedirectToAction("EditBaoGia", new
            {
                id = donHang.id
            });

        }
        public ActionResult EditBaoGia(int? id)
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
                                     select new BaoGiaTemDetailView { Date_Working = u.date_working, date_deliver = d.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
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
        public ActionResult EditBaoGia(DonHangView donHang)
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

                    var queryMax = (from u in db.tbl_Products
                                    orderby u.ID_Products descending
                                    select u).Take(1);
                    int maxSP = queryMax.ToList().Count == 0 ? 1 : queryMax.ToList()[0].ID_Products + 1;
                    String masp = String.Format("SP{0}", maxSP.ToString("000000"));
                    item.CodeProducts = masp;
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
                        StatusProducts = item.StatusProducts
                    };
                    itemP = db.tbl_Products.Add(itemP);
                    db.SaveChanges();
                    item.ID_Products = itemP.ID_Products;
                    var qr = (from cus in db.tbl_Customers where cus.IDCustomers == donHang.customer_id select cus).ToList();
                    foreach (var code in qr)
                    {
                   
                    tbl_OrderTem_BaoGia_Detail detail = new tbl_OrderTem_BaoGia_Detail {code_detail=code.CodeCustomers+"_"+ donHang.code+"_"+countindex.ToString("00"), design = item.Design, baogia_id = donHang.BaoGiaTemView.id, money = double.Parse(item.GiaProducts), soluong = item.SoLuong, sanpam_id = itemP.ID_Products };
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
                    return RedirectToAction("Index", "tbl_OrderTem");
                }
            }
            //if (donHang.action == 4)
            //{
            //    donHang.action = 0;
            //    tbl_OrderTem order = db.tbl_OrderTem.Find(donHang.id);
            //    order.date_begin_plan = donHang.date_begin_plan;
            //    order.date_end_plan = donHang.date_end_plan;

            //    order.update_date = DateTime.Now;
            //    order.update_user = Session["username"].ToString();
            //    order.status = donHang.status.Value;
            //    db.Entry(order).State = EntityState.Modified;
            //    db.SaveChanges();
            //    var queryGiaoGiaCT = from u in db.tbl_OrderTem_BaoGia_Detail
            //                         join y in db.tbl_Products on u.sanpam_id equals y.ID_Products
            //                         where u.baogia_id.Value.Equals(donHang.BaoGiaTemView.id)
            //                         select new BaoGiaTemDetailView { Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
            //    List<BaoGiaTemDetailView> listSP = queryGiaoGiaCT.ToList<BaoGiaTemDetailView>();
            //    foreach (var item in listSP)
            //    {
            //        tbl_OrderTem_BaoGia_Detail tbl_OrderTem_BaoGia_Detail = db.tbl_OrderTem_BaoGia_Detail.Find(item.id);
            //        tbl_OrderTem_BaoGia_Detail.status = 0;
            //        db.Entry(tbl_OrderTem_BaoGia_Detail).State = EntityState.Modified;
            //        if (item.OffsetFlexoProducts.Equals("Offset"))
            //        {
            //            tbl_QuyTrinh qt1 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 0, TrangThai = 0, TenBuoc = "Nhận tờ in offset" };
            //            db.tbl_QuyTrinh.Add(qt1);
            //            tbl_QuyTrinh qt2 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 1, TrangThai = 0, TenBuoc = "Sản xuất giấy tấm" };
            //            db.tbl_QuyTrinh.Add(qt2);
            //            tbl_QuyTrinh qt3 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 2, TrangThai = 0, TenBuoc = "Bồi" };
            //            db.tbl_QuyTrinh.Add(qt3);
            //            tbl_QuyTrinh qt4 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 3, TrangThai = 0, TenBuoc = "Bế" };
            //            db.tbl_QuyTrinh.Add(qt4);
            //            tbl_QuyTrinh qt5 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 4, TrangThai = 0, TenBuoc = "Bấm kim/dán" };
            //            db.tbl_QuyTrinh.Add(qt5);
            //            tbl_QuyTrinh qt6 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 5, TrangThai = 0, TenBuoc = "Đóng gói" };
            //            db.tbl_QuyTrinh.Add(qt6);
            //            tbl_QuyTrinh qt7 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 6, TrangThai = 0, TenBuoc = "Giao hàng" };
            //            db.tbl_QuyTrinh.Add(qt7);
            //            tbl_QuyTrinh qt8 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 7, TrangThai = 0, TenBuoc = "Thanh toán" };
            //            db.tbl_QuyTrinh.Add(qt8);
            //            tbl_QuyTrinh qt9 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 8, TrangThai = 0, TenBuoc = "Kết thúc đơn hàng" };
            //            db.tbl_QuyTrinh.Add(qt9);

            //            db.SaveChanges();

            //        }
            //        else
            //        {
            //            tbl_QuyTrinh qt1 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 0, TrangThai = 0, TenBuoc = "Sản xuất giấy tấm" };
            //            db.tbl_QuyTrinh.Add(qt1);
            //            tbl_QuyTrinh qt2 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 1, TrangThai = 0, TenBuoc = "Xã biên,cán lằn" };
            //            db.tbl_QuyTrinh.Add(qt2);
            //            tbl_QuyTrinh qt3 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 2, TrangThai = 0, TenBuoc = "In Flexo " };
            //            db.tbl_QuyTrinh.Add(qt3);
            //            tbl_QuyTrinh qt4 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 3, TrangThai = 0, TenBuoc = "Chạp khoe" };
            //            db.tbl_QuyTrinh.Add(qt4);
            //            tbl_QuyTrinh qt5 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 4, TrangThai = 0, TenBuoc = "Đóng kim/Dán" };
            //            db.tbl_QuyTrinh.Add(qt5);
            //            tbl_QuyTrinh qt6 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 5, TrangThai = 0, TenBuoc = "Đóng gói" };
            //            db.tbl_QuyTrinh.Add(qt6);
            //            tbl_QuyTrinh qt7 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 6, TrangThai = 0, TenBuoc = "Giao hàng" };
            //            db.tbl_QuyTrinh.Add(qt7);
            //            tbl_QuyTrinh qt8 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 7, TrangThai = 0, TenBuoc = "Thanh toán" };
            //            db.tbl_QuyTrinh.Add(qt8);
            //            tbl_QuyTrinh qt9 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 8, TrangThai = 0, TenBuoc = "Kết thúc đơn hàng" };
            //            db.tbl_QuyTrinh.Add(qt9);
            //            db.SaveChanges();

            //        }
            //    }
            //    return RedirectToAction("Edit", "SanXuat", new { id = donHang.id });
            //}
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
                                     select new BaoGiaTemDetailView { Date_Working = u.date_working, date_deliver = d.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
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

        // GET: tbl_OrderTem/Edit/5
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
                                     select new BaoGiaTemDetailView { Date_Working=u.date_working,date_deliver = d.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
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

        // POST: tbl_OrderTem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            //if (donHang.action == 2)
            //{
            //    donHang.action = 0;
            //    donHang.BaoGiaTemView.status = 0;
            //    donHang.BaoGiaTemView.date_begin = DateTime.Now;
            //    tbl_OrderTem_BaoGia tbl_OrderTem_BaoGia = new tbl_OrderTem_BaoGia { date_begin = donHang.BaoGiaTemView.date_begin, date_end = donHang.BaoGiaTemView.date_end, id = donHang.BaoGiaTemView.id, order_id = donHang.id, status = donHang.BaoGiaTemView.status, total_money = donHang.BaoGiaTemView.total_money };
            //    tbl_OrderTem_BaoGia = db.tbl_OrderTem_BaoGia.Add(tbl_OrderTem_BaoGia);
            //    db.SaveChanges();
            //    donHang.BaoGiaTemView.id = tbl_OrderTem_BaoGia.id;
            //    foreach (var item in donHang.BaoGiaTemView.BaoGiaTemDetailViews)
            //    {
            //        item.StatusProducts = -1;
            //        item.CreatedDateProducts = DateTime.Now;

            //        var queryMax = (from u in db.tbl_Products
            //                        orderby u.ID_Products descending
            //                        select u).Take(1);
            //        int maxSP = queryMax.ToList().Count == 0 ? 1 : queryMax.ToList()[0].ID_Products + 1;
            //        String masp = String.Format("SP{0}", maxSP.ToString("000000"));
            //        item.CodeProducts = masp;
            //        tbl_Products itemP = new tbl_Products
            //        {
            //            CodeProducts = masp,
            //            CreatedDateProducts = item.CreatedDateProducts,
            //            CreateUserProducts = item.CreateUserProducts,
            //            DanKimProducts = item.DanKimProducts,
            //            GiaProducts = item.GiaProducts,
            //            ID_Products = item.ID_Products,
            //            LoaigiayProducts = item.LoaigiayProducts,
            //            ModifyDateProducts = item.ModifyDateProducts,
            //            ModifyUserProducts = item.ModifyUserProducts,
            //            NameProducts = item.NameProducts,
            //            OffsetFlexoProducts = item.OffsetFlexoProducts,
            //            QuyCachProducts = item.QuyCachProducts,
            //            SolopProducts = item.SolopProducts,
            //            StatusProducts = item.StatusProducts
            //        };
            //        itemP = db.tbl_Products.Add(itemP);
            //        db.SaveChanges();
            //        item.ID_Products = itemP.ID_Products;
            //        tbl_OrderTem_BaoGia_Detail detail = new tbl_OrderTem_BaoGia_Detail { baogia_id = donHang.BaoGiaTemView.id, money = double.Parse(item.GiaProducts), soluong = item.SoLuong, sanpam_id = itemP.ID_Products };
            //        db.tbl_OrderTem_BaoGia_Detail.Add(detail);
            //        db.SaveChanges();
            //    }

            //}
            //if (donHang.action == 3)
            //{
            //    donHang.action = 0;


            //    var queryCount = from a in db.tbl_OrderTem_BaoGia_Detail where a.baogia_id.Value.Equals(donHang.BaoGiaTemView.id) select a;
            //    int countItem = queryCount.ToList().Count();


            //    tbl_OrderTem order = db.tbl_OrderTem.Find(donHang.id);
            //    order.code = order.code + (countItem > 1 ? "01" : "00");
            //    order.date_deliver = donHang.date_deliver;
            //    order.address_deliver = donHang.address_deliver;
            //    order.update_date = DateTime.Now;
            //    order.update_user = Session["username"].ToString();
            //    db.Entry(order).State = EntityState.Modified;
            //    db.SaveChanges();


            //    tbl_OrderTem_BaoGia baogia = db.tbl_OrderTem_BaoGia.Find(donHang.BaoGiaTemView.id);
            //    baogia.status = donHang.BaoGiaTemView.status.Value;
            //    baogia.commission = donHang.BaoGiaTemView.commission;
            //    baogia.date_end = DateTime.Now;
            //    baogia.note = donHang.BaoGiaTemView.note;
            //    db.Entry(baogia).State = EntityState.Modified;
            //    db.SaveChanges();

            //}
            if (donHang.action == 4)
            {
                donHang.action = 0;
                tbl_OrderTem order = db.tbl_OrderTem.Find(donHang.id);
                order.date_begin_plan = donHang.date_begin_plan;
                order.date_end_plan = donHang.date_end_plan;

                order.update_date = DateTime.Now;
                order.update_user = Session["username"].ToString();
                order.status = donHang.status.Value;
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
                    tbl_OrderTem_BaoGia_Detail.status = 0;
                    db.Entry(tbl_OrderTem_BaoGia_Detail).State = EntityState.Modified;
                    if (item.OffsetFlexoProducts.Equals("Offset"))
                    {
                        tbl_QuyTrinh qt1 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 0, TrangThai = 0, TenBuoc = "Nhận tờ in offset" };
                        db.tbl_QuyTrinh.Add(qt1);
                        tbl_QuyTrinh qt2 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 1, TrangThai = 0, TenBuoc = "Sản xuất giấy tấm" };
                        db.tbl_QuyTrinh.Add(qt2);
                        tbl_QuyTrinh qt3 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 2, TrangThai = 0, TenBuoc = "Bồi" };
                        db.tbl_QuyTrinh.Add(qt3);
                        tbl_QuyTrinh qt4 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 3, TrangThai = 0, TenBuoc = "Bế" };
                        db.tbl_QuyTrinh.Add(qt4);
                        tbl_QuyTrinh qt5 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 4, TrangThai = 0, TenBuoc = "Bấm kim/dán" };
                        db.tbl_QuyTrinh.Add(qt5);
                        tbl_QuyTrinh qt6 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 5, TrangThai = 0, TenBuoc = "Đóng gói" };
                        db.tbl_QuyTrinh.Add(qt6);
                        tbl_QuyTrinh qt7 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 6, TrangThai = 0, TenBuoc = "Giao hàng" };
                        db.tbl_QuyTrinh.Add(qt7);
                        tbl_QuyTrinh qt8 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 7, TrangThai = 0, TenBuoc = "Thanh toán" };
                        db.tbl_QuyTrinh.Add(qt8);
                        tbl_QuyTrinh qt9 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 8, TrangThai = 0, TenBuoc = "Kết thúc đơn hàng" };
                        db.tbl_QuyTrinh.Add(qt9);

                        db.SaveChanges();

                    }
                    else
                    {
                        tbl_QuyTrinh qt1 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 0, TrangThai = 0, TenBuoc = "Sản xuất giấy tấm" };
                        db.tbl_QuyTrinh.Add(qt1);
                        tbl_QuyTrinh qt2 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 1, TrangThai = 0, TenBuoc = "Xã biên,cán lằn" };
                        db.tbl_QuyTrinh.Add(qt2);
                        tbl_QuyTrinh qt3 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 2, TrangThai = 0, TenBuoc = "In Flexo " };
                        db.tbl_QuyTrinh.Add(qt3);
                        tbl_QuyTrinh qt4 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 3, TrangThai = 0, TenBuoc = "Chạp khoe" };
                        db.tbl_QuyTrinh.Add(qt4);
                        tbl_QuyTrinh qt5 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 4, TrangThai = 0, TenBuoc = "Đóng kim/Dán" };
                        db.tbl_QuyTrinh.Add(qt5);
                        tbl_QuyTrinh qt6 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 5, TrangThai = 0, TenBuoc = "Đóng gói" };
                        db.tbl_QuyTrinh.Add(qt6);
                        tbl_QuyTrinh qt7 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 6, TrangThai = 0, TenBuoc = "Giao hàng" };
                        db.tbl_QuyTrinh.Add(qt7);
                        tbl_QuyTrinh qt8 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 7, TrangThai = 0, TenBuoc = "Thanh toán" };
                        db.tbl_QuyTrinh.Add(qt8);
                        tbl_QuyTrinh qt9 = new tbl_QuyTrinh { ID_BaoGiaDetail = item.id, ThuTu = 8, TrangThai = 0, TenBuoc = "Kết thúc đơn hàng" };
                        db.tbl_QuyTrinh.Add(qt9);
                        db.SaveChanges();

                    }
                }
                return RedirectToAction("Index", "SanXuat");
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
                                     select new BaoGiaTemDetailView { Date_Working = u.date_working, date_deliver = d.date_deliver, Design = u.design, Design_Date = u.design_date, Design_Img = u.design_img, id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
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

        // GET: tbl_OrderTem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            tbl_OrderTem tbl_OrderTem = db.tbl_OrderTem.Find(id);
            db.tbl_OrderTem.Remove(tbl_OrderTem);
            db.SaveChanges();
            return RedirectToAction("Index", new { id = tbl_OrderTem.customer_id });
        }

        // POST: tbl_OrderTem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbl_OrderTem tbl_OrderTem = db.tbl_OrderTem.Find(id);
            db.tbl_OrderTem.Remove(tbl_OrderTem);
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


        public ActionResult PrintOrder(int id)
        {

            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

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



            BaoGiaTemView temBG = new BaoGiaTemView { address_deliver = tbl_OrderTem.address_deliver, note = item.note, date_begin = item.date_begin, date_end = item.date_end, id = item.id, order_id = item.order_id, status = item.status, total_money = item.total_money };
            var queryGiaoGiaCT = from u in db.tbl_OrderTem_BaoGia_Detail
                                 join y in db.tbl_Products on u.sanpam_id equals y.ID_Products
                                 where u.baogia_id.Value.Equals(temBG.id)
                                 select new BaoGiaTemDetailView { id = u.id, ID_Products = u.sanpam_id.Value, CodeProducts = y.CodeProducts, CreatedDateProducts = y.CreatedDateProducts, CreateUserProducts = y.CreateUserProducts, DanKimProducts = y.DanKimProducts, GiaProducts = u.money.Value.ToString(), LoaigiayProducts = y.LoaigiayProducts, ModifyDateProducts = y.ModifyDateProducts, ModifyUserProducts = y.ModifyUserProducts, NameProducts = y.NameProducts, OffsetFlexoProducts = y.OffsetFlexoProducts, QuyCachProducts = y.QuyCachProducts, SolopProducts = y.SolopProducts, SoLuong = u.soluong.Value, StatusProducts = y.StatusProducts };
            temBG.BaoGiaTemDetailViews = queryGiaoGiaCT.ToList<BaoGiaTemDetailView>();

            d.BaoGiaTemView = temBG;
            d.BaoGiaTemView.buildPrint();
            var list = from tt in db.tbl_Customers where tt.IDCustomers == d.customer_id select tt;
            d.Customer = list.ToList()[0];
            return View(d);

        }

        [HttpPost]
        public ActionResult UpdateDesign(int id, DateTime date, HttpPostedFileBase file)
        {
            tbl_OrderTem_BaoGia_Detail item = db.tbl_OrderTem_BaoGia_Detail.Find(id);
            item.design_date = date;
            item.design = 2;
            item.design_img = file.FileName;
            file.SaveAs(Server.MapPath("~/Upload/ThietKe") + "/" + item.id + "_" + file.FileName);
            db.Entry(item).State = EntityState.Modified;
            db.SaveChanges();
            return Json(item);
        }


    }
}
