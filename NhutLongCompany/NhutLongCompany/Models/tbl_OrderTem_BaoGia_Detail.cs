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
    
    public partial class tbl_OrderTem_BaoGia_Detail
    {
        public int id { get; set; }
        public Nullable<int> baogia_id { get; set; }
        public Nullable<int> sanpam_id { get; set; }
        public Nullable<int> soluong { get; set; }
        public Nullable<double> money { get; set; }
        public Nullable<int> design { get; set; }
        public string design_img { get; set; }
        public Nullable<System.DateTime> design_date { get; set; }
        public Nullable<int> step_index { get; set; }
        public Nullable<System.DateTime> date_working { get; set; }
        public Nullable<int> status { get; set; }
        public string code_detail { get; set; }
        public Nullable<int> status_pause { get; set; }
        public string loai_design { get; set; }
    }
}
