//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace NhutLongCompany.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbl_QuyTrinh
    {
        public int ID { get; set; }
        public int ID_BaoGiaDetail { get; set; }
        public string TenBuoc { get; set; }
        public Nullable<int> ThuTu { get; set; }
        public Nullable<int> TrangThai { get; set; }
        public Nullable<System.DateTime> NgayBatDau_DK { get; set; }
        public Nullable<System.DateTime> NgayKetThuc_DK { get; set; }
        public Nullable<System.DateTime> NgayBatDau_TT { get; set; }
        public Nullable<System.DateTime> NgayKetThuc_TT { get; set; }
        public Nullable<int> ThucHien { get; set; }
    }
}
