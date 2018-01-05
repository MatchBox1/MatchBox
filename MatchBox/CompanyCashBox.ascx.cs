using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public partial class CompanyCashBox : UserControl
    {
        public int CompanyID { get; set; }

        private int n_user_id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CompanyID <= 0) { return; }

            n_user_id = ((UserModel)Session["User"]).ID;

            CompanyModel o_company = new CompanyModel();

            o_company.ID = CompanyID;
            o_company.UserID = n_user_id;

            CompanyAction.Select(ref o_company);

            if (o_company.ErrorMessage != "")
            {
                lblError.Text = o_company.ErrorMessage;
                return;
            }

            lblCompanyNumber.Text = o_company.CompanyNumber.ToString();
            lblCompanyName.Text = o_company.CompanyName;

        }
    }
}