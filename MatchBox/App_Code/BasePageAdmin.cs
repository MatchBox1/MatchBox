using System;

namespace MatchBox
{
    public class BasePageAdmin : BasePage
    {
        public BasePageAdmin()
        {
            this.PreInit += new EventHandler(Page_PreInit);
        }

        private void Page_PreInit(Object sender, EventArgs e)
        {
            if (o_user.IsAdmin == false) { Logout(); }
        }
    }
}