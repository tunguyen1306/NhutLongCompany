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
    
    public partial class tbl_FlowPauseTime
    {
        public long id { get; set; }
        public Nullable<int> baoGia_detail_id { get; set; }
        public Nullable<System.DateTime> date_begin { get; set; }
        public Nullable<System.DateTime> date_end { get; set; }
        public Nullable<int> status { get; set; }
        public Nullable<int> index_step { get; set; }
        public string note { get; set; }
        public Nullable<int> id_flow { get; set; }
    }
}
