using System;
using System.Web.UI;

namespace MatchBox
{
    public partial class AdminData : BasePageAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string s_after_update = "";

            if (Request.QueryString["after_update"] != null) { s_after_update = Request.QueryString.Get("after_update"); }

            if (s_after_update == "1")
            {
                lblMessage.CssClass = "message";
                lblMessage.Text = "Item successfully updated.";
            }

            if (!Page.IsPostBack)
            {
                string s_error = "";

                DB.Bind_List_Control(ddlCity, "sprCity", ref s_error);

                Bind_Form();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) { return; }

            AdminDataModel o_admin_data = new AdminDataModel();

            o_admin_data.CityID = Convert.ToInt32(ddlCity.SelectedValue);

            o_admin_data.CompanyName = txtCompanyName.Text.Trim();
            o_admin_data.Phone = txtPhone.Text.Trim();
            o_admin_data.Fax = txtFax.Text.Trim();
            o_admin_data.Mail = txtMail.Text.Trim();
            o_admin_data.City = ddlCity.SelectedItem.Text;
            o_admin_data.Address = txtAddress.Text.Trim();

            if (o_admin_data.CityID == 0) { o_admin_data.City = ""; }

            AdminDataAction.Update(ref o_admin_data);

            if (o_admin_data.ErrorMessage != "")
            {
                lblMessage.Text = o_admin_data.ErrorMessage;
                return;
            }

            Application.Lock();
            Application["AdminData"] = o_admin_data;
            Application.UnLock();

            Response.Redirect("AdminData.aspx?after_update=1");
        }

        private void Bind_Form()
        {
            AdminDataModel o_admin_data = (AdminDataModel)Application["AdminData"];

            ddlCity.SelectedValue = o_admin_data.CityID.ToString();

            txtCompanyName.Text = o_admin_data.CompanyName;
            txtPhone.Text = o_admin_data.Phone;
            txtFax.Text = o_admin_data.Fax;
            txtMail.Text = o_admin_data.Mail;
            txtAddress.Text = o_admin_data.Address;
        }
    }
}