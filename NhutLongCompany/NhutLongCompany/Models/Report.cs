using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NhutLongCompany.Models
{
    public class Report
    {

                 
                               
        [System.ComponentModel.DataAnnotations.Display(Name = "Mã sản phẩm")]
        public string codeProducts { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Tên sản phẩm")]
        public string nameProducts { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Nhận tờ in offset")]
        public int? qt1 { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Sản xuất giấy tấm")]
        public int? qt2 { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Bồi")]
        public int? qt3 { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Xả biến, cán lằn")]
        public int? qt4 { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "In FLEXO")]
        public int? qt5 { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Bế")]
        public int? qt6 { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Chập khe")]
        public int? qt7 { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Bấm kim")]
        public int? qt8 { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Dán")]
        public int? qt9 { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Đóng gói")]
        public int? qt10 { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Giao hàng")]
        public int? qt11 { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Thanh toán")]
        public int? qt12 { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = " Kết thúc đơn hàng")]
        public int? qt13 { get; set; }
      
    }
}