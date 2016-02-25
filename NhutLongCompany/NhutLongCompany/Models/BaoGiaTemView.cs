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
        public Nullable<int> offset { get; set; }
        public List<BaoGiaTemDetailView> BaoGiaTemDetailViews { get; set; }
        public String note { get; set; }

        public Nullable<int> flow { get; set; }

      

        public List<String> PrintTen { get; set; }
        public List<int> PrintSoLop { get; set; }
        public List<String> PrintLoaiGiay { get; set; }
        public List<String> PrintOffset_Flexo { get; set; }
        public List<String> PrintDan_Kim { get; set; }
        public List<int> PrintSoLuong { get; set; }
        public List<double> PrintDonGia { get; set; }
        public List<double> PrintThanhTien { get; set; }

        public void buildPrint()
        {
            PrintTen = new List<string>();
            PrintSoLop = new List<int>();
            PrintLoaiGiay = new List<string>();
            PrintOffset_Flexo = new List<string>();
            PrintDan_Kim = new List<string>();
            PrintSoLuong = new List<int>();
            PrintDonGia = new List<double>();
            PrintThanhTien = new List<double>();
            for (int i = 0; i < BaoGiaTemDetailViews.Count; i++)
            { var item = BaoGiaTemDetailViews[i];
                PrintTen.Add(item.NameProducts);
                PrintSoLop.Add(item.SolopProducts.Value);
                PrintLoaiGiay.Add(item.LoaigiayProducts);
                PrintOffset_Flexo.Add(item.OffsetFlexoProducts);
                PrintDan_Kim.Add(item.DanKimProducts);
                PrintSoLuong.Add(item.SoLuong);
                PrintDonGia.Add(double.Parse( item.GiaProducts));
                PrintThanhTien.Add(double.Parse(item.GiaProducts)* item.SoLuong);
              
            }
        }
    }
}