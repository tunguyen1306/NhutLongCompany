using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NhutLongCompany.Models
{
    public class Navbar
    {
        public int Id { get; set; }
        public string nameOption { get; set; }
        public string controller { get; set; }
        public string action { get; set; }
        public string imageClass { get; set; }
        public bool status { get; set; }
        public int? parentId { get; set; }
        public bool isParent { get; set; }
        public int? isOrder { get; set; }
        public string nameOptionCh { get; set; }
        public int? UserId { get; set; } public string nameUser { get; set; }
    }
}