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
    
    public partial class tbl_BaoGiaChiTiet
    {
        public int ID_BaoGiaChiTiet { get; set; }
        public Nullable<int> ID_BaoGia { get; set; }
        public Nullable<int> ID_SanPham { get; set; }
        public Nullable<int> SoLuong { get; set; }
        public string Tien { get; set; }
    }
}