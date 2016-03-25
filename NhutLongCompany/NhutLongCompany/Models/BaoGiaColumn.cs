using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NhutLongCompany.Models
{
    public class BaoGiaColumn
    {

        public int ID_BaoGia { get; set; }
        public Nullable<int> ID_Customers { get; set; }
        public Nullable<int> BaoGia_ID_Default { get; set; }
        public string TongTien { get; set; }
        public Nullable<int> LamMau { get; set; }
        public Nullable<System.DateTime> NgayCoMau { get; set; }
        public Nullable<System.DateTime> NgayBaoGia { get; set; }
        public Nullable<int> DuyetBaoGia { get; set; }
        public string CodeDonHang { get; set; }
        public Nullable<System.DateTime> NgayBatDauDuKien { get; set; }
        public Nullable<System.DateTime> NgayKetThucDuKien { get; set; }
        public Nullable<System.DateTime> NgayBatDauThucTe { get; set; }
        public Nullable<System.DateTime> NgayKetThucThucTe { get; set; }
        public Nullable<System.DateTime> NgayGiao { get; set; }
        public Nullable<int> QuyTrinh { get; set; }
        public string ChiTietQuyTrinh { get; set; }
        public Nullable<int> LanBaoGia { get; set; }
        public string NameCustomers { get; set; }
        public string ChucvuCustomers { get; set; }
        public string CongTyCustomers { get; set; }
        public string CodeCustomers { get; set; }
        public string EmailCustomers { get; set; }
        public string PhoneCustomers { get; set; }
        public string FaxCustomers { get; set; }
        public string DiaChiCustomers { get; set; }
        public string MasothueCustomers { get; set; }


        public List<tbl_Products> Products { get; set; }
    }
}