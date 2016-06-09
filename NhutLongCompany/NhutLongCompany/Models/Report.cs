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
        public int? NhanToInOffset { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Sản xuất giấy tấm")]
        public int? SanXuatGiayTam { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Bồi")]
        public int? Boi { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Xả biến, cán lằn")]
        public int? XaBienCanLan { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "In FLEXO")]
        public int? InFlexo { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Bế")]
        public int? Be { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Chập khe")]
        public int? ChapKhe { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Bấm kim")]
        public int? BamKim { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Dán")]
        public int? Dan { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Đóng gói")]
        public int? DongGoi { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Giao hàng")]
        public int? GiaoHang { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = "Thanh toán")]
        public int? ThanhToan { get; set; }
        [System.ComponentModel.DataAnnotations.Display(Name = " Kết thúc đơn hàng")]
        public int? KetThucDonHang { get; set; }
      
    }
}