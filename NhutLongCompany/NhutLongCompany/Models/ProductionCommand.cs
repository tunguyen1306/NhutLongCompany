using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NhutLongCompany.Models
{
    public class ProductionCommand
    {
        public tbl_Products Product { get; set; }
        public tbl_ProductionCommand Command { get; set; }
        public tbl_OrderTem_BaoGia_Detail Detail { get; set; }

        public ProductionCommand()
        {
            Command = new tbl_ProductionCommand();
             Detail=new tbl_OrderTem_BaoGia_Detail();
        }
    }
}