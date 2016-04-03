using NhutLongCompany.Attribute;
using System.Web;
using System.Web.Mvc;

namespace NhutLongCompany
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new RedirectOnErrorAttribute());
            filters.Add(new HandleErrorAttribute());
        }
    }
}
