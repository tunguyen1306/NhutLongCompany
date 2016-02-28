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
            menu.Add(new Navbar { Id = 2, nameOption = "Khách Hàng", controller = "Customers", action = "Index", imageClass = "fa fa-users fa-2x", status = true, isParent = false, parentId = 0 });
            menu.Add(new Navbar { Id = 3, nameOption = "Sản Phẩm", controller = "Products", action = "Index", imageClass = "fa fa-shopping-cart fa-2x", status = true, isParent = false, parentId = 0 });
            menu.Add(new Navbar { Id = 4, nameOption = "Sản xuất", controller = "Sanxuat", action = "Index", imageClass = "fa fa-gears fa-2x", status = true, isParent = false, parentId = 0 });
            menu.Add(new Navbar { Id = 5, nameOption = "Báo Giá", controller = "tbl_OrderTem", action = "Create", imageClass = "fa fa-file-o fa-2x", status = true, isParent = true, parentId = 0 });
            menu.Add(new Navbar { Id = 6, nameOption = "Báo Giá", controller = "tbl_OrderTem", action = "Create", imageClass = "fa fa-file-o fa-", status = true, isParent = false, parentId = 5 });
            menu.Add(new Navbar { Id = 7, nameOption = "Đơn hàng", controller = "Sanxuat", action = "Index", imageClass = "fa fa-file-o fa-", status = true, isParent = false, parentId = 5 });
           
            return menu.ToList();
        }
    }
}