using System;
using System.Web.UI;

namespace MatchBox
{
    public partial class Registration : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserModel o_user = new UserModel();

            if (Session["User"] != null) { o_user = (UserModel)Session["User"]; }

            if ((o_user.IsAdmin == true) || (o_user.IsUser == true && o_user.ID > 0))
            {
                Response.Redirect("Default.aspx");
            }

            txtFullName.Focus();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) { return; }

            UserModel o_user = new UserModel();

            o_user.FullName = txtFullName.Text.Trim();
            o_user.UserName = txtUserName.Text.Trim();
            o_user.Password = txtPassword.Text.Trim();
            o_user.Mail = txtMail.Text.Trim();

            UserAction.Register(ref o_user);

            if (o_user.ErrorMessage != "")
            {
                lblMsg.Text = o_user.ErrorMessage;
                return;
            }

            if (o_user.ID > 0)
            {
                Session["User"] = o_user;
                Response.Redirect("Default.aspx");
            }
        }
    }
}
