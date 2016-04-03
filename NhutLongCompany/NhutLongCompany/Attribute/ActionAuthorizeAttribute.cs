using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NhutLongCompany.Attribute
{
    [RedirectOnError]
    public sealed class ActionAuthorizeAttribute : AuthorizeAttribute
    {
        readonly string featureName;
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="feature">The feature.</param>
        public ActionAuthorizeAttribute(string feature)
        {
            featureName = feature;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionAuthorizeAttribute"/> class.
        /// </summary>
        public ActionAuthorizeAttribute()
        {
            featureName = string.Empty;
        }

        /// <summary>
        /// Processes HTTP requests that fail authorization.
        /// </summary>
        /// <param name="filterContext">Encapsulates the information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />. The <paramref name="filterContext" /> object contains the controller, HTTP context, request context, action result, and route data.</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
           
                base.HandleUnauthorizedRequest(filterContext);
            
        }

        /// <summary>
        /// Called when a process requests authorization.
        /// </summary>
        /// <param name="filterContext">The filter context, which encapsulates information for using <see cref="T:System.Web.Mvc.AuthorizeAttribute" />.</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["username"]!=null)
            {
                if (!string.IsNullOrEmpty(featureName))
                {
                    var roleUser = filterContext.HttpContext.Session["roleName"];
                    var isAuthorization = CheckRole(featureName, roleUser==null?"":roleUser.ToString());
                    if (!isAuthorization)
                        filterContext.Result = new RedirectResult("~/Login/NoAuthorize");
                    //GoToLogin(filterContext);
                }
        
               
            }
            else
            {
                GoToLogin(filterContext);
            }
        }

        private static bool CheckRole(String currentAction,String roleName)
        {
            if (String.IsNullOrEmpty(currentAction))
            {
                return true;
            }
            if (String.IsNullOrEmpty(roleName))
            {
                return false;
            }
            String[] arrayAction = currentAction.Split(',');
            String[] arrayRoleName = roleName.Split(',');
            foreach (var item in arrayAction)
            {
                foreach (var item1 in arrayRoleName)
                {
                    if (item.Trim().Equals(item1.Trim()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Goes to login.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        private static void GoToLogin(AuthorizationContext filterContext)
        {
            // Fire back an unauthorized response
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                // Fire back an unauthorized response
                filterContext.HttpContext.Response.StatusCode = 403;
                filterContext.HttpContext.Response.AddHeader("REQUIRES_AUTH", "1");
                filterContext.Result = new RedirectResult("~/Login/AjaxLogin");
            }
            else
            {                
                FormsAuthentication.SignOut();
                filterContext.HttpContext.Session.Clear();
                if (filterContext.HttpContext.Request.Url != null)
                {
                    var loginUrl = string.Format("{0}?ReturnUrl={1}"
                        , "~/Login/Login"
                        , HttpUtility.UrlEncode(filterContext.HttpContext.Request.Url.AbsoluteUri));
                    filterContext.Result = new RedirectResult(loginUrl);
                }
            }
        }
    }

}
