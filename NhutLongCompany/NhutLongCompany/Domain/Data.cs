using NhutLongCompany.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NhutLongCompany.Domain
{
    public class Data
    {
        public IEnumerable<Navbar> navbarItems()
        {
           var menu = new List<Navbar>();
           menu.Add(new Navbar { Id = 1, nameOption = "Trang chủ", controller = "Home", action = "Home", imageClass = "fa fa-home fa-2x", status = true, isParent = false, parentId = 0 });
           menu.Add(new Navbar { Id = 2, nameOption = "Khách Hàng", controller = "Customers", action = "Index", imageClass = "fa fa-users fa-2x", status = true, isParent = true, parentId = 0 });

           menu.Add(new Navbar { Id = 3, nameOption = "Thông tin Khách hàng", controller = "Customers", action = "Index", imageClass = "fa fa-users fa-2x", status = true, isParent = false, parentId = 2 });

           menu.Add(new Navbar { Id = 4, nameOption = "Thông tin báo giá", controller = "tbl_OrderTem", action = "IndexBaoGia", imageClass = "fa fa-file-o fa-", status = true, isParent = false, parentId = 2 });

           menu.Add(new Navbar { Id = 5, nameOption = "Đơn hàng", controller = "tbl_OrderTem", action = "Index", imageClass = "fa fa-file-o fa-2x", status = true, isParent = true, parentId = 0 });

           menu.Add(new Navbar { Id = 6, nameOption = "Thông tin đơn hàng ", controller = "tbl_OrderTem", action = "ThongTinDonHang", imageClass = "fa fa-gears fa-1x", status = true, isParent = false, parentId = 5 });
           menu.Add(new Navbar { Id = 15, nameOption = "Theo dõi đơn hàng ", controller = "tbl_OrderTem", action = "TheoDoiDonHang", imageClass = "fa fa-gears fa-1x", status = true, isParent = false, parentId = 5 });
           //menu.Add(new Navbar { Id = 10, nameOption = "Theo dõi đơn hàng", controller = "tbl_OrderTem", action = "Index", imageClass = "fa fa-file-o fa-", status = true, isParent = false, parentId = 5 });
           //menu.Add(new Navbar { Id = 11, nameOption = "Đơn hàng đang sản xuất", controller = "Sanxuat", action = "IndexSX", imageClass = "fa fa-gears fa-1x", status = true, isParent = false, parentId = 5 });
           //menu.Add(new Navbar { Id = 3, nameOption = "Báo Giá", controller = "tbl_OrderTem", action = "", imageClass = "fa fa-file-o fa-2x", status = true, isParent = true, parentId = 0 });
           //menu.Add(new Navbar { Id = 4, nameOption = "Báo Giá", controller = "tbl_OrderTem", action = "IndexBaoGia", imageClass = "fa fa-file-o fa-", status = true, isParent = false, parentId = 3 });
           //menu.Add(new Navbar { Id = 5, nameOption = "Đơn hàng", controller = "tbl_OrderTem", action = "Index", imageClass = "fa fa-file-o fa-", status = true, isParent = false, parentId = 3 });
           //menu.Add(new Navbar { Id = 14, nameOption = "Thông tin đơn hàng ", controller = "tbl_OrderTem", action = "IndexSX", imageClass = "fa fa-gears fa-1x", status = true, isParent = false, parentId = 3 });
           //menu.Add(new Navbar { Id = 6, nameOption = "Sản Phẩm", controller = "Products", action = "Index", imageClass = "fa fa-shopping-cart fa-2x", status = true, isParent = true, parentId = 0 });
           //menu.Add(new Navbar { Id = 7, nameOption = "Sản Phẩm Đã thực hiện", controller = "Products", action = "Index", imageClass = "fa fa-shopping-cart fa-2x", status = true, isParent = false, parentId =6 });
           ////menu.Add(new Navbar { Id = 8, nameOption = "Sản Phẩm đang thực hiện", controller = "Products", action = "SPWorking", imageClass = "fa fa-shopping-cart fa-2x", status = true, isParent = false, parentId = 6 });
           //menu.Add(new Navbar { Id = 8, nameOption = "Sản Phẩm đang thực hiện", controller = "Products", action = "IndexProcessProduct", imageClass = "fa fa-shopping-cart fa-2x", status = true, isParent = false, parentId = 6 });

           menu.Add(new Navbar { Id = 7, nameOption = "Sản xuất", controller = "Sanxuat", action = "", imageClass = "fa fa-gears fa-2x", status = true, isParent = true, parentId = 0 });
           // menu.Add(new Navbar { Id = 10, nameOption = "Đơn hàng chờ duyệt sản xuất", controller = "Sanxuat", action = "Index", imageClass = "fa fa-gears fa-1x", status = true, isParent = false, parentId = 9 });
           // menu.Add(new Navbar { Id = 11, nameOption = "Đơn hàng đang sản xuất", controller = "Sanxuat", action = "IndexSX", imageClass = "fa fa-gears fa-1x", status = true, isParent = false, parentId = 9 });
           menu.Add(new Navbar { Id = 8, nameOption = "Lịch sản xuất", controller = "Sanxuat", action = "LichSanXuat", imageClass = "fa fa-gears fa-1x", status = true, isParent = false, parentId = 7 });
           menu.Add(new Navbar { Id = 9, nameOption = "Sản phầm đang sản xuất", controller = "Sanxuat", action = "IndexSanXuat", imageClass = "fa fa-gears fa-1x", status = true, isParent = false, parentId = 7 });
           menu.Add(new Navbar { Id = 10, nameOption = "Lịch sản xuất trong ngày", controller = "Sanxuat", action = "LichSanXuatOnDay", imageClass = "fa fa-gears fa-1x", status = true, isParent = false, parentId = 7 });


            return menu.ToList();
        }
    }
}