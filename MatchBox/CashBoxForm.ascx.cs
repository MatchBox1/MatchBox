using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public partial class CashBoxForm : UserControl
    {
        public int CashBoxID { get; set; }

        private int n_user_id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            n_user_id = ((UserModel)Session["User"]).ID;

            if (Page.IsPostBack) { return; }

            if (Request.QueryString["result"] != null)
            {
                string s_message = "";
                string s_result = Request.QueryString.Get("result");

                if (s_result.Contains("updated"))
                {
                    s_message = "updated";
                }
                else if (s_result.Contains("deleted"))
                {
                    s_message = "deleted";
                }
                else
                {
                    s_message = "???";
                }

                s_message = String.Format("Item successfully {0}.", s_message);

                lblMessage.Text = s_message;
            }

            CashBoxModel o_cash_box = new CashBoxModel();

            if (CashBoxID > 0)
            {
                o_cash_box.ID = CashBoxID;
                o_cash_box.UserID = n_user_id;

                CashBoxAction.Select(ref o_cash_box);

                if (o_cash_box.ErrorMessage != "")
                {
                    lblError.Text = o_cash_box.ErrorMessage;
                    return;
                }
            }

            Bind_Form(o_cash_box);

            ddlCompany.Focus();
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n_company_id = Convert.ToInt32(ddlCompany.SelectedValue);

            while (ddlBranch.Items.Count > 1)
            {
                ddlBranch.Items.Remove(ddlBranch.Items[1]);
            }

            while (ddlNetwork.Items.Count > 1)
            {
                ddlNetwork.Items.Remove(ddlNetwork.Items[1]);
            }

            if (n_company_id > 0)
            {
                string s_error = "";

                List<SqlParameter> o_network_parameters = new List<SqlParameter>();
                o_network_parameters.Add(new SqlParameter("@UserID", n_user_id));
                o_network_parameters.Add(new SqlParameter("@CompanyID", n_company_id));
                
                DB.Bind_List_Control(ddlNetwork, "sprNetwork", ref s_error, null, o_network_parameters);
            }
        }

        protected void ddlNetwork_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n_network_id = Convert.ToInt32(ddlNetwork.SelectedValue);

            while (ddlBranch.Items.Count > 1)
            {
                ddlBranch.Items.Remove(ddlBranch.Items[1]);
            }

            if (n_network_id > 0)
            {
                string s_error = "";

                List<SqlParameter> o_branch_parameters = new List<SqlParameter>();
                o_branch_parameters.Add(new SqlParameter("@UserID", n_user_id));
                o_branch_parameters.Add(new SqlParameter("@NetworkID", n_network_id));

                DB.Bind_List_Control(ddlBranch, "sprBranch", ref s_error, null, o_branch_parameters);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) { return; }

            CashBoxModel o_cash_box = new CashBoxModel();

            o_cash_box.ID = CashBoxID;
            o_cash_box.UserID = n_user_id;
            o_cash_box.BranchID = Convert.ToInt32(ddlBranch.SelectedValue);

            o_cash_box.CashBoxNumber = txtCashBoxNumber.Text.Trim();
            o_cash_box.CashBoxName = txtCashBoxName.Text.Trim();

            CashBoxAction.Update(ref o_cash_box);

            if (o_cash_box.ErrorMessage != "")
            {
                lblError.Text = o_cash_box.ErrorMessage;
                return;
            }

            string s_guid = Request.QueryString.Get("q");
            string s_result = "updated";

            if (CashBoxID == 0)
            {
                s_guid = Guid.NewGuid().ToString();
                s_result = "inserted";

                ((Dictionary<string, int>)Session["CashBoxLookupTable"]).Add(s_guid, o_cash_box.ID);
            }

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}", s_result, Request.QueryString.Get("i"), s_guid));
        }

        private void Bind_Form(CashBoxModel o_cash_box)
        {
            string s_error = "";

            List<SqlParameter> o_company_parameters = new List<SqlParameter>();
            o_company_parameters.Add(new SqlParameter("@UserID", n_user_id));

            DB.Bind_List_Control(ddlCompany, "sprCompany", ref s_error, null, o_company_parameters);

            if (o_cash_box.ID > 0)
            {
                List<SqlParameter> o_network_parameters = new List<SqlParameter>();
                o_network_parameters.Add(new SqlParameter("@UserID", n_user_id));
                o_network_parameters.Add(new SqlParameter("@CompanyID", o_cash_box.CompanyID));

                List<SqlParameter> o_branch_parameters = new List<SqlParameter>();
                o_branch_parameters.Add(new SqlParameter("@UserID", n_user_id));
                o_branch_parameters.Add(new SqlParameter("@NetworkID", o_cash_box.NetworkID));

                DB.Bind_List_Control(ddlNetwork, "sprNetwork", ref s_error, null, o_network_parameters);
                DB.Bind_List_Control(ddlBranch, "sprBranch", ref s_error, null, o_branch_parameters);
            }

            ddlCompany.SelectedValue = o_cash_box.CompanyID.ToString();
            ddlNetwork.SelectedValue = o_cash_box.NetworkID.ToString();
            ddlBranch.SelectedValue = o_cash_box.BranchID.ToString();

            txtCashBoxNumber.Text = o_cash_box.CashBoxNumber;
            txtCashBoxName.Text = o_cash_box.CashBoxName;
        }
    }
}