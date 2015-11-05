using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugTracker.Helpers
{
    public static class HtmlHelpers
    {
        public static bool IsCurrentAction(this HtmlHelper helper, string controller, string action = null)
        {
            return (string)helper.ViewContext.RouteData.Values["controller"] == controller
                    && (string)helper.ViewContext.RouteData.Values["action"] == action;
        }
    }
}