using System;
using System.Web.UI;

namespace MatchBox
{
    public class BasePage : Page
    {
        public UserModel o_user = null;

        public BasePage()
        {
            this.PreInit += new EventHandler(Page_PreInit);
        }

        private void Page_PreInit(Object sender, EventArgs e)
        {
            if (Request.QueryString["logout"] != null && Request.QueryString.Get("logout") == "1") { Logout(); }

            o_user = new UserModel();

            if (Session["User"] != null) { o_user = (UserModel)Session["User"]; }

            if ((o_user.IsAdmin == false) && (o_user.IsUser == false || o_user.ID <= 0)) { Logout(); }
        }

        public void Logout()
        {
            Session.Abandon();
            Response.Redirect("Login.aspx");
        }
    }
}