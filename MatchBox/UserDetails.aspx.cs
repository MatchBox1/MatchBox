using System;

namespace MatchBox
{
    public partial class UserDetails : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ucUserForm.UserID = o_user.ID;
        }
    }
}