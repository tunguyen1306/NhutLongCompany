﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NhutLongCompany.Models
{
    public class BaoGiaTemDetailView
    { 
        public int id { get; set; }
        public int idOrderTem { get; set; }
        public int ID_Products { get; set; }
        public string NameProducts { get; set; }
        public Nullable<int> SolopProducts { get; set; }
        public string LoaigiayProducts { get; set; }

        public string LoaiSongProducts { get; set; }
        public Nullable<int> InFlexoProducts { get; set; }

        public string OffsetFlexoProducts { get; set; }
        public string DanKimProducts { get; set; }
        public string GiaProducts { get; set; }
        public Nullable<System.DateTime> CreatedDateProducts { get; set; }
        public Nullable<System.DateTime> ModifyDateProducts { get; set; }
        public string CreateUserProducts { get; set; }
        public string ModifyUserProducts { get; set; }
        public Nullable<int> StatusProducts { get; set; }
        public string CodeProducts { get; set; }
        public string QuyCachProducts { get; set; }
        public int SoLuong { get; set; }
        public float BuHao { get; set; }
        public List<tbl_QuyTrinh> QuyTrinhs { get; set; }
        public string loai_design { get; set; }

        public Nullable<int> Design { get; set; }
        public String Design_Img { get; set; }
        public Nullable<DateTime> Design_Date { get; set; }
        public Nullable<DateTime> Date_end { get; set; }
        public Nullable<int> Timer { get; set; }
        public Nullable<System.DateTime> date_deliver { get; set; }
        public string address_deliver { get; set; }
        public int? datenumber { get; set; }

        public Nullable<int> Index_View { get; set; }
        public Nullable<System.DateTime> Date_Working { get; set; }

        public Nullable<int> Status { get; set; }
        public string Code_Detail { get; set; }
        public Nullable<int> Step_Flow { get; set; }

        public Nullable<int> Status_Pause { get; set; }

        public List<tbl_FlowPauseTime> Current_FlowPauseTime;

        public int? pause { get; set; }
        public tbl_OrderTemPause tbl_OrderTemPause;
        public String order_code;
        public Nullable<System.DateTime> Date_Begin { get; set; }
        public string name_customer { get; set; }
        public List<tblPrint> tblPrint { get; set; }



    }
}