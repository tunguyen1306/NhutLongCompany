using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NhutLongCompany.Models;

namespace NhutLongCompany.Controllers
{
    public class BaoCaoController : Controller
    {
        private NhutLongCompanyEntities db = new NhutLongCompanyEntities();

        // GET: BaoCao
        public ActionResult Index()
        {
            if (Session["username"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
           var listDHView = new List<DonHangView>();
            var qr = (from data in db.tbl_OrderTem
                      join cus in db.tbl_Customers on data.customer_id equals cus.IDCustomers
                      where data.status >= 3
                      orderby data.status ascending,data.update_date descending
                      select data);
            var listDH= qr.ToList();
            foreach (var tbl_OrderTem in listDH)
            {
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
                var lisBG = queryBaoGia.ToList<tbl_OrderTem_BaoGia>();
                foreach (var item in lisBG)
                {
                    BaoGiaTemView temBG = new BaoGiaTemView { note = item.note, date_begin = item.date_begin, date_end = item.date_end, id = item.id, offset = item.offset, order_id = item.order_id, status = item.status, total_money = item.total_money };
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
                listDHView.Add(d);
            }

            return View(listDHView);
        }
    }
}