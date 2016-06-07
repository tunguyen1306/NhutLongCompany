using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NhutLongCompany.Models
{
    public class BaoGiaTemView
    {
        public int id { get; set; }
        public Nullable<int> order_id { get; set; }
        public Nullable<double> total_money { get; set; }
        public Nullable<System.DateTime> date_begin { get; set; }
        public Nullable<System.DateTime> date_end { get; set; }
        public Nullable<int> status { get; set; }
        public List<BaoGiaTemDetailView> BaoGiaTemDetailViews { get; set; }
        public String note { get; set; }
        public Nullable<int> commission { get; set; }
        public Nullable<double> commission_money { get; set; }
        public string address_deliver { get; set; }
        public Nullable<System.DateTime> date_deliver { get; set; }

        public int? datenumber { get; set; }
        public List<String> PrintTen { get; set; }
        public List<int> PrintSoLop { get; set; }
        public List<String> PrintLoaiSong { get; set; }
        public List<int?> PrintInFlexo { get; set; }
        public List<String> PrintLoaiGiay { get; set; }
        public List<String> PrintOffset_Flexo { get; set; }
        public List<String> PrintDan_Kim { get; set; }
        public List<String> PrintQuyCach { get; set; }
        public List<int> PrintSoLuong { get; set; }
        public List<double> PrintDonGia { get; set; }
        public List<double> PrintThanhTien { get; set; }
        public List<tbl_QuyTrinh> QuyTrinhs { get; set; }
        public int StatusQuyTrinhGiaoHang { get; set; }
        public int StatusQuyTrinhDongGoi { get; set; }
        public int StatusQuyTrinhThanhToan { get; set; }
        public void buildPrint()
        {
            PrintTen = new List<string>();
            PrintSoLop = new List<int>();
            PrintLoaiGiay = new List<string>();
            PrintLoaiSong = new List<string>();
            PrintInFlexo = new List<int?>();
            PrintOffset_Flexo = new List<string>();
            PrintDan_Kim = new List<string>();
            PrintQuyCach = new List<string>();
            PrintSoLuong = new List<int>();
            PrintDonGia = new List<double>();
            PrintThanhTien = new List<double>();
            for (int i = 0; i < BaoGiaTemDetailViews.Count; i++)
            {
                var item = BaoGiaTemDetailViews[i];
                PrintTen.Add(item.NameProducts);
                PrintSoLop.Add(item.SolopProducts.Value);
                PrintLoaiGiay.Add(item.LoaigiayProducts);
                PrintLoaiSong.Add(item.LoaiSongProducts);
                PrintInFlexo.Add(item.InFlexoProducts);
                PrintOffset_Flexo.Add(item.OffsetFlexoProducts);
                PrintDan_Kim.Add(item.DanKimProducts);
                PrintQuyCach.Add(item.QuyCachProducts);
                PrintSoLuong.Add(item.SoLuong);
                PrintDonGia.Add(double.Parse(item.GiaProducts));
                PrintThanhTien.Add(double.Parse(item.GiaProducts) * item.SoLuong);

            }
        }
    }
}