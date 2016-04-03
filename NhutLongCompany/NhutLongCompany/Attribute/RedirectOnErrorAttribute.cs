using System.Web.Mvc;
using System.Web.Routing;


namespace NhutLongCompany.Attribute
{
    public sealed class RedirectOnErrorAttribute : FilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// Called when an exception occurs.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnException(ExceptionContext filterContext)
        {
         
            // Fire back an unauthorized response
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                filterContext.ExceptionHandled = true;
            }
            else
            {
                //redirect to error handler
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Error" }));

                // Stop any other exception handlers from running
                filterContext.ExceptionHandled = true;

                // CLear out anything already in the response
                filterContext.HttpContext.Response.Clear();
            }
        }
    }
}
