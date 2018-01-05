using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI;

namespace MatchBox
{
    public partial class BranchForm : UserControl
    {
        public int BranchID { get; set; }

        private int n_user_id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            n_user_id = ((UserModel)Session["User"]).ID;

            if (Page.IsPostBack) { return; }

            if (BranchID > 0) { hedTitle.InnerHtml = "Branch #" + BranchID; } else { hedTitle.InnerHtml = "New Branch"; }

            if (Request.QueryString["result"] != null && Request.QueryString.Get("result") == "updated") { lblMessage.Text = "Item successfully updated."; }

            BranchModel o_branch = new BranchModel();

            if (BranchID > 0)
            {
                o_branch.ID = BranchID;
                o_branch.UserID = n_user_id;

                BranchAction.Select(ref o_branch);

                if (o_branch.ErrorMessage != "")
                {
                    lblError.Text = o_branch.ErrorMessage;
                    return;
                }
            }

            Bind_Form(o_branch);

            txtBranchNumber.Focus();
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n_company_id = Convert.ToInt32(ddlCompany.SelectedValue);

            while (ddlNetwork.Items.Count > 1)
            {
                ddlNetwork.Items.Remove(ddlNetwork.Items[1]);
            }

            string s_error = "";

            if (n_company_id > 0)
            {
                List<SqlParameter> o_network_parameters = new List<SqlParameter>();

                o_network_parameters.Add(new SqlParameter("@UserID", n_user_id));
                o_network_parameters.Add(new SqlParameter("@CompanyID", n_company_id));

                DB.Bind_List_Control(ddlNetwork, "sprNetwork", ref s_error, null, o_network_parameters);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) { return; }

            BranchModel o_branch = new BranchModel();

            o_branch.ID = BranchID;
            o_branch.UserID = n_user_id;
            o_branch.NetworkID = Convert.ToInt32(ddlNetwork.SelectedValue);
            o_branch.CityID = Convert.ToInt32(ddlCity.SelectedValue);

            o_branch.BranchNumber = txtBranchNumber.Text.Trim();
            o_branch.BranchName = txtBranchName.Text.Trim();
            o_branch.Phone = txtPhone.Text.Trim();
            o_branch.Fax = txtFax.Text.Trim();
            o_branch.Mail = txtMail.Text.Trim();
            o_branch.Address = txtAddress.Text.Trim();

            BranchAction.Update(ref o_branch);

            if (o_branch.ErrorMessage != "")
            {
                lblError.Text = o_branch.ErrorMessage;
                return;
            }

            string s_guid = Request.QueryString.Get("q");
            string s_result = "updated";

            if (BranchID == 0)
            {
                s_guid = Guid.NewGuid().ToString();
                s_result = "inserted";

                ((Dictionary<string, int>)Session["BranchLookupTable"]).Add(s_guid, o_branch.ID);
            }

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}", s_result, Request.QueryString.Get("i"), s_guid));
        }

        private void Bind_Form(BranchModel o_branch)
        {
            string s_error = "";

            List<SqlParameter> o_company_parameters = new List<SqlParameter>();
            o_company_parameters.Add(new SqlParameter("@UserID", n_user_id));

            DB.Bind_List_Control(ddlCompany, "sprCompany", ref s_error, null, o_company_parameters);
            DB.Bind_List_Control(ddlCity, "sprCity", ref s_error);

            if (o_branch.ID > 0)
            {
                List<SqlParameter> o_network_parameters = new List<SqlParameter>();

                o_network_parameters.Add(new SqlParameter("@UserID", n_user_id));
                o_network_parameters.Add(new SqlParameter("@CompanyID", o_branch.CompanyID));

                DB.Bind_List_Control(ddlNetwork, "sprNetwork", ref s_error, null, o_network_parameters);
            }

            ddlCompany.SelectedValue = o_branch.CompanyID.ToString();
            ddlNetwork.SelectedValue = o_branch.NetworkID.ToString();
            ddlCity.SelectedValue = o_branch.CityID.ToString();

            txtBranchNumber.Text = o_branch.BranchNumber;
            txtBranchName.Text = o_branch.BranchName;
            txtPhone.Text = o_branch.Phone;
            txtFax.Text = o_branch.Fax;
            txtMail.Text = o_branch.Mail;
            txtAddress.Text = o_branch.Address;
        }
    }
}
