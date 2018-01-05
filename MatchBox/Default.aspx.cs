using System;

namespace MatchBox
{
    public partial class Default : BasePage
    {
        private static int x = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (o_user.IsUser == true) { Bind_User_Data(); }
        }

        private void Bind_User_Data()
        {
            pUserData.Visible = true;

            lblMail.Text = o_user.Mail;
            lblPhone.Text = o_user.Phone;
            lblMobile.Text = o_user.Mobile;
            lblCity.Text = o_user.City;
            lblAddress.Text = o_user.Address;
        }
    }
}
