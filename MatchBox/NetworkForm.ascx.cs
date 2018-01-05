using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.UI;

namespace MatchBox
{
    public partial class NetworkForm : UserControl
    {
        public int NetworkID { get; set; }

        private int n_user_id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            n_user_id = ((UserModel)Session["User"]).ID;

            if (Page.IsPostBack) { return; }

            if (NetworkID > 0) { hedTitle.InnerHtml = "Network #" + NetworkID; } else { hedTitle.InnerHtml = "New Network"; }

            if (Request.QueryString["result"] != null && Request.QueryString.Get("result") == "updated") { lblMessage.Text = "Item successfully updated."; }

            string s_error = "";

            List<SqlParameter> o_company_parameters = new List<SqlParameter>();
            o_company_parameters.Add(new SqlParameter("@UserID", n_user_id));

            DB.Bind_List_Control(ddlCompany, "sprCompany", ref s_error, null, o_company_parameters);

            NetworkModel o_network = new NetworkModel();

            if (NetworkID > 0)
            {
                o_network.ID = NetworkID;
                o_network.UserID = n_user_id;

                NetworkAction.Select(ref o_network);

                if (o_network.ErrorMessage != "")
                {
                    lblError.Text = o_network.ErrorMessage;
                    return;
                }
            }

            Bind_Form(o_network);

            txtNetworkNumber.Focus();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) { return; }

            NetworkModel o_network = new NetworkModel();

            o_network.ID = NetworkID;
            o_network.UserID = n_user_id;
            o_network.CompanyID = Convert.ToInt32(ddlCompany.SelectedValue);

            o_network.NetworkNumber = txtNetworkNumber.Text.Trim();
            o_network.NetworkName = txtNetworkName.Text.Trim();

            NetworkAction.Update(ref o_network);

            if (o_network.ErrorMessage != "")
            {
                lblError.Text = o_network.ErrorMessage;
                return;
            }

            string s_guid = Request.QueryString.Get("q");
            string s_result = "updated";

            if (NetworkID == 0)
            {
                s_guid = Guid.NewGuid().ToString();
                s_result = "inserted";

                ((Dictionary<string, int>)Session["NetworkLookupTable"]).Add(s_guid, o_network.ID);
            }

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}", s_result, Request.QueryString.Get("i"), s_guid));
        }

        private void Bind_Form(NetworkModel o_network)
        {
            ddlCompany.SelectedValue = o_network.CompanyID.ToString();

            txtNetworkNumber.Text = o_network.NetworkNumber;
            txtNetworkName.Text = o_network.NetworkName;
        }
    }
}
