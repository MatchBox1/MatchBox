using System;
using System.Web.UI;

namespace MatchBox
{
    public partial class Main : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
         {
            UserModel o_user = new UserModel();

            if (Session["User"] != null) { o_user = (UserModel)Session["User"]; }

            if (o_user.IsAdmin == true)
            {
                pnlNavOutside.Visible = false;
                pnlNavInside.Visible = true;
                divMenuAdmin.Visible = true;

                lnkUserForm.Text = o_user.UserName;
                lnkUserForm.NavigateUrl = "AdminData.aspx";
            }
            else if (o_user.IsUser && o_user.ID > 0)
            {
                Check_Permission(o_user);
            }
        }

        private void Check_Permission(UserModel o_user)
        {
            // StatusID : 1 = Active / 2 = Pending / 3 = Deleted

            if (o_user.StatusID == 3) { return; }

            pnlNavOutside.Visible = false;
            pnlNavInside.Visible = true;

            lnkUserForm.Text = o_user.UserName;
            lnkUserForm.NavigateUrl = "UserDetails.aspx";

            if (o_user.StatusID == 2)
            {
                divPendingMessage.Visible = true;
                return;
            }

            divMenuCustomer.Visible = true;

            // SHOW MENU BY PERMISSION ...
        }
    }
}
