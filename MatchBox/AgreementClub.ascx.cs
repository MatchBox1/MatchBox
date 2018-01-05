using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public partial class AgreementClub : UserControl
    {
        public int AgreementID { get; set; }

        public bool IsVisibleClubForm { get; set; }

        private int n_user_id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (AgreementID <= 0) { return; }

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

            AgreementModel o_agreement = new AgreementModel();

            o_agreement.ID = AgreementID;
            o_agreement.UserID = n_user_id;

            AgreementAction.Select(ref o_agreement);

            if (o_agreement.ErrorMessage != "")
            {
                lblError.Text = o_agreement.ErrorMessage;
                return;
            }

            Bind_Form(o_agreement);
        }

        protected void repAgreementPeriod_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView o_data_row = (DataRowView)e.Item.DataItem;

                int n_agreement_period_id = 0;
                int.TryParse(o_data_row["ID"].ToString(), out n_agreement_period_id);

                DataTable dt_agreement_club = (DataTable)ViewState["TableAgreementClub"];

                DataRow[] dr_agreement_club = dt_agreement_club.Select(" AgreementPeriodID = " + n_agreement_period_id);

                if (dr_agreement_club.Length > 0)
                {
                    Repeater repAgreementClub = (Repeater)e.Item.FindControl("repAgreementClub");

                    repAgreementClub.DataSource = dr_agreement_club.CopyToDataTable();
                    repAgreementClub.DataBind();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (AgreementID <= 0) { return; }

            if (!Page.IsValid) { return; }
            
            AgreementClubModel o_agreement_club = new AgreementClubModel();

            o_agreement_club.UserID = n_user_id;
            o_agreement_club.AgreementID = AgreementID;
            o_agreement_club.CompanyID = (int)ViewState["CompanyID"];
            o_agreement_club.CreditID = (int)ViewState["CreditID"];

            o_agreement_club.ClubNumber = Convert.ToInt64(txtClubNumber.Text.Trim());

            o_agreement_club.Commission = Convert.ToDouble(txtCommission.Text.Trim());
            o_agreement_club.Saving = Convert.ToDouble(txtSaving.Text.Trim());

            int n_day_start = Convert.ToInt32(ddlDayStart.SelectedValue);
            int n_month_start = Convert.ToInt32(ddlMonthStart.SelectedValue);
            int n_year_start = Convert.ToInt32(ddlYearStart.SelectedValue);

            int n_day_end = Convert.ToInt32(ddlDayEnd.SelectedValue);
            int n_month_end = Convert.ToInt32(ddlMonthEnd.SelectedValue);
            int n_year_end = Convert.ToInt32(ddlYearEnd.SelectedValue);

            o_agreement_club.DateStart = new DateTime(n_year_start, n_month_start, n_day_start);
            o_agreement_club.DateEnd = new DateTime(n_year_end, n_month_end, n_day_end);

            o_agreement_club.ManagementCompany = txtManagementCompany.Text.Trim();
            o_agreement_club.ContactName = txtContactName.Text.Trim();
            o_agreement_club.Phone = txtPhone.Text.Trim();
            o_agreement_club.Mail = txtMail.Text.Trim();
            o_agreement_club.Address = txtAddress.Text.Trim();
            o_agreement_club.ClubName = txtClubName.Text.Trim();

            AgreementClubAction.Check(ref o_agreement_club);

            if (o_agreement_club.ErrorMessage != "")
            {
                lblError.Text = o_agreement_club.ErrorMessage;
                IsVisibleClubForm = true;
                return;
            }

            AgreementClubAction.Update(ref o_agreement_club);

            if (o_agreement_club.ErrorMessage != "")
            {
                lblError.Text = o_agreement_club.ErrorMessage;
                IsVisibleClubForm = true;
                return;
            }

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}&p={3}", "updated", Request.QueryString.Get("i"), Request.QueryString.Get("q"), "club"));
        }

        protected void cvDateRange_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool b_valid = true;

            lblDateStartError.Text = "";
            lblDateEndError.Text = "";
            lblError.Text = "";

            int n_day_start = 0, n_month_start = 0, n_year_start = 0;
            int n_day_end = 0, n_month_end = 0, n_year_end = 0;

            int.TryParse(ddlDayStart.SelectedValue, out n_day_start);
            int.TryParse(ddlMonthStart.SelectedValue, out n_month_start);
            int.TryParse(ddlYearStart.SelectedValue, out n_year_start);

            int.TryParse(ddlDayEnd.SelectedValue, out n_day_end);
            int.TryParse(ddlMonthEnd.SelectedValue, out n_month_end);
            int.TryParse(ddlYearEnd.SelectedValue, out n_year_end);

            if (n_day_start == 0 || n_month_start == 0 || n_year_start == 0)
            {
                b_valid = false;
                lblDateStartError.Text = "Select 'Start Date'.";

                if (n_year_start == 0) { ddlYearStart.Focus(); }
                if (n_month_start == 0) { ddlMonthStart.Focus(); }
                if (n_day_start == 0) { ddlDayStart.Focus(); }
            }

            if (n_day_end == 0 || n_month_end == 0 || n_year_end == 0)
            {
                b_valid = false;
                lblDateEndError.Text = "Select 'End Date'.";

                if (n_year_end == 0) { ddlYearEnd.Focus(); }
                if (n_month_end == 0) { ddlMonthEnd.Focus(); }
                if (n_day_end == 0) { ddlDayEnd.Focus(); }
            }

            if (b_valid == true)
            {
                b_valid = DateRange_IsValid(n_day_start, n_month_start, n_year_start, n_day_end, n_month_end, n_year_end);
            }

            IsVisibleClubForm = !b_valid;

            args.IsValid = b_valid;
        }

        protected void btnDeleteAgreementClub_Command(object sender, CommandEventArgs e)
        {
            string s_error = "";

            int n_id = Convert.ToInt32(e.CommandArgument);

            bool b_deleted = AgreementClubAction.Delete(n_id, n_user_id, AgreementID, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            if (b_deleted == false)
            {
                lblError.Text = "No item was deleted.";
                return;
            }

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}&p={3}", "deleted", Request.QueryString.Get("i"), Request.QueryString.Get("q"), "club"));
        }

        private bool DateRange_IsValid(int n_day_start, int n_month_start, int n_year_start, int n_day_end, int n_month_end, int n_year_end)
        {
            bool b_valid = true;

            if (n_day_start > DateTime.DaysInMonth(n_year_start, n_month_start))
            {
                b_valid = false;
                lblDateStartError.Text = "'Start Date' not valid.";
                ddlDayStart.Focus();
            }

            if (n_day_end > DateTime.DaysInMonth(n_year_end, n_month_end))
            {
                b_valid = false;
                lblDateEndError.Text = "'End Date' not valid.";
                ddlDayEnd.Focus();
            }

            if (b_valid == false) { goto Finish; }

            DateTime d_date_start = new DateTime(n_year_start, n_month_start, n_day_start);
            DateTime d_date_end = new DateTime(n_year_end, n_month_end, n_day_end);

            if (d_date_start > d_date_end)
            {
                b_valid = false;
                lblError.Text = "'Start Date' can't be greater than 'End Date'.";
                goto Finish;
            }

            DataTable dt_agreement_period = (DataTable)ViewState["TableAgreementPeriod"];

            var o_agreement_period = from agreement_period in dt_agreement_period.AsEnumerable()
                                     where
                                     (
                                         agreement_period.Field<DateTime?>("DateEnd") == null
                                         &&
                                         d_date_start >= agreement_period.Field<DateTime>("DateStart")
                                         &&
                                         d_date_end >= agreement_period.Field<DateTime>("DateStart")
                                     )
                                     ||
                                     (
                                         (
                                             d_date_start >= agreement_period.Field<DateTime>("DateStart")
                                             &&
                                             d_date_start <= agreement_period.Field<DateTime>("DateEnd")
                                         )
                                         &&
                                         (
                                             d_date_end >= agreement_period.Field<DateTime>("DateStart")
                                             &&
                                             d_date_end <= agreement_period.Field<DateTime>("DateEnd")
                                         )
                                     )
                                     select agreement_period;


            if (o_agreement_period.Any() == false)
            {
                b_valid = false;
                lblError.Text = "The date range must be a part of service period.";
                goto Finish;
            }

            Finish:

            return b_valid;
        }

        private void Bind_Form(AgreementModel o_agreement)
        {
            string s_error = "";

            ViewState["CompanyID"] = o_agreement.CompanyID;
            ViewState["CreditID"] = o_agreement.CreditID;

            lblCompanyName.Text = o_agreement.CompanyName;
            lblCreditName.Text = o_agreement.CreditName;

            for (int i = 1; i <= 31; i++)
            {
                ddlDayStart.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlDayEnd.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            for (int i = 1; i <= 12; i++)
            {
                ddlMonthStart.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlMonthEnd.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            int n_year = DateTime.Now.Year;

            for (int i = n_year + 5; i >= n_year - 15; i--)
            {
                ddlYearStart.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlYearEnd.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            DataTable dt_agreement_period = new DataTable();
            DataTable dt_agreement_club = new DataTable();

            AgreementClubAction.Select(ref dt_agreement_period, ref dt_agreement_club, ref s_error, n_user_id, null, AgreementID);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            ViewState["TableAgreementPeriod"] = dt_agreement_period;
            ViewState["TableAgreementClub"] = dt_agreement_club;

            repAgreementPeriod.DataSource = dt_agreement_period;
            repAgreementPeriod.DataBind();
        }
    }
}
