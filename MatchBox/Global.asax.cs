using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace MatchBox
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            Application.Lock();
            Application["AdminData"] = null;
            Application.UnLock();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session["User"] = null;

            if (Application["AdminData"] == null)
            {
                AdminDataModel o_admin_data = new AdminDataModel();
                AdminDataAction.Select(ref o_admin_data);

                Application.Lock();
                Application["AdminData"] = o_admin_data;
                Application.UnLock();
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e) { }

        protected void Application_AuthenticateRequest(object sender, EventArgs e) { }

        protected void Application_Error(object sender, EventArgs e) { }

        protected void Session_End(object sender, EventArgs e) { }

        protected void Application_End(object sender, EventArgs e) { }
    }
}
