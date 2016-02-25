using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NhutLongCompany.Models
{
    public class DonHangView
    {
        public int id { get; set; }
        public string code { get; set; }
        public Nullable<System.DateTime> date_begin { get; set; }
        public Nullable<System.DateTime> date_end { get; set; }
        public Nullable<System.DateTime> date_begin_plan { get; set; }
        public Nullable<System.DateTime> date_end_plan { get; set; }
        public Nullable<int> customer_id { get; set; }
        public Nullable<int> status { get; set; }
        public tbl_Customers Customer { get; set; }
        public BaoGiaTemView BaoGiaTemView { get; set; }
        public List<BaoGiaTemView> BaoGiaTemViews { get; set; }
        public int action { get; set; }
        public  DonHangView()
        {
            action = 0;
        }

    }
}