using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HPPlc.Models
{
	public class SessionExpireAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			HttpContext ctx = HttpContext.Current;
			
			// check if session is supported
			string UserLoggedIn = SessionManagement.GetCurrentSession<string>(SessionType.IsLoggedIn);
			if (UserLoggedIn == null)
			{
				// check if a new session id was generated
				filterContext.Result = new RedirectResult("login");
				return;
			}

			base.OnActionExecuting(filterContext);
		}


		public static bool CheckUserLoggedIn(string loggedOrNot = "")
		{
			string culture = HPPlc.Models.CultureName.GetCultureName();
			bool isLoggedInUser = false;
			if (HPPlc.Models.SessionManagement.GetCurrentSession<string>(HPPlc.Models.SessionType.IsLoggedIn) == "Y"
												&& HPPlc.Models.SessionManagement.GetCurrentSession<int>(HPPlc.Models.SessionType.UserId) != 0)
			{
				isLoggedInUser = true;
			}
			else
			{
				isLoggedInUser = false;
			}

			if (loggedOrNot == "YES" && isLoggedInUser == true)
				HttpContext.Current.Response.Redirect(culture == "/" ? "" : culture + "/", false);
			else if(loggedOrNot != "YES" && loggedOrNot != "Subsptn" && isLoggedInUser == false)
					HttpContext.Current.Response.Redirect(culture + "/my-account/login", false);
			else if (loggedOrNot == "Subsptn" && isLoggedInUser == false)
				HttpContext.Current.Response.Redirect(culture + "/my-account/login?ref=u", false);


			return isLoggedInUser;
		}

        public static bool UserLoggedIn()
        {
            if (SessionManagement.GetCurrentSession<string>(SessionType.IsLoggedIn) == "Y"
                                                && SessionManagement.GetCurrentSession<int>(SessionType.UserId) != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}