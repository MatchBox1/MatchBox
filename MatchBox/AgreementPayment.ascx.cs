using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public partial class AgreementPayment : UserControl
    {
        public int AgreementID { get; set; }

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

                if (Request.QueryString["payment"] != null)
                {
                    // SHOW Agreement Payment FORM ( payment = AgreementPaymentID )

                    CommandEventArgs o_args = new CommandEventArgs("Edit_Payment", Request.QueryString.Get("payment"));

                    Edit_Payment(null, o_args);
                }
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

                DataTable dt_agreement_payment = (DataTable)ViewState["TableAgreementPayment"];

                DataRow[] dr_agreement_payment = dt_agreement_payment.Select(" AgreementPeriodID = " + n_agreement_period_id);

                if (dr_agreement_payment.Length > 0)
                {
                    Repeater repAgreementPayment = (Repeater)e.Item.FindControl("repAgreementPayment");

                    repAgreementPayment.DataSource = dr_agreement_payment.CopyToDataTable();
                    repAgreementPayment.DataBind();
                }
            }
        }

        protected void repAgreementPayment_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView o_data_row = (DataRowView)e.Item.DataItem;

                int n_agreement_payment_id = 0;
                int.TryParse(o_data_row["ID"].ToString(), out n_agreement_payment_id);

                DataTable dt_agreement_payment_settings = (DataTable)ViewState["TableAgreementPaymentSettings"];

                DataRow[] dr_agreement_payment_settings = dt_agreement_payment_settings.Select(" AgreementPaymentID = " + n_agreement_payment_id);

                if (dr_agreement_payment_settings.Length > 0)
                {
                    Repeater repAgreementPaymentSettings = (Repeater)e.Item.FindControl("repAgreementPaymentSettings");

                    repAgreementPaymentSettings.DataSource = dr_agreement_payment_settings.CopyToDataTable();
                    repAgreementPaymentSettings.DataBind();
                }
            }
        }

        protected void repPaymentSettings_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ((Label)e.Item.FindControl("lblRowNumber")).Text = (e.Item.ItemIndex + 1).ToString();
            }
        }

        protected void Edit_Payment(object sender, CommandEventArgs e)
        {
            if (AgreementID <= 0) { return; }

            string s_error = "";

            if (ddlCard.Items.Count == 1)
            {
                List<SqlParameter> o_card_parameters = new List<SqlParameter>();
                o_card_parameters.Add(new SqlParameter("@CreditID", (int)ViewState["CreditID"]));

                DB.Bind_List_Control(ddlCard, "sprCardByCredit", ref s_error, null, o_card_parameters);

                if (s_error != "")
                {
                    lblError.Text = s_error;
                    return;
                }
            }

            ddlCard.SelectedIndex = 0;

            int n_agreement_payment_id = 0;
            int.TryParse(e.CommandArgument.ToString(), out n_agreement_payment_id);

            LinkButton o_link_button = (LinkButton)sender;

            if (o_link_button.ID == "btnEditPayment")
            {
                ViewState["AgreementPaymentID"] = n_agreement_payment_id;
            }
            else
            {
                ViewState["AgreementPaymentID"] = 0;    // if btnCopyPayment
            }

            AgreementPaymentModel o_agreement_payment = new AgreementPaymentModel();

            DataTable dt_payment_settings = new DataTable();
            dt_payment_settings.Columns.Add("DayFrom");
            dt_payment_settings.Columns.Add("DayTo");
            dt_payment_settings.Columns.Add("DayPayment");

            if (n_agreement_payment_id > 0)
            {
                o_agreement_payment.ID = n_agreement_payment_id;
                o_agreement_payment.UserID = n_user_id;
                o_agreement_payment.AgreementID = AgreementID;

                AgreementPaymentAction.Select(ref o_agreement_payment);

                if (o_agreement_payment.ErrorMessage != "")
                {
                    lblError.Text = o_agreement_payment.ErrorMessage;
                    return;
                }

                ddlCard.SelectedValue = o_agreement_payment.CardID.ToString();

                txtDateStart.Text = String.Format("{0:dd/MM/yyyy}", o_agreement_payment.DateStart);
                txtDateEnd.Text = String.Format("{0:dd/MM/yyyy}", o_agreement_payment.DateEnd);

                foreach (DataRow o_data_row in o_agreement_payment.TableAgreementPaymentSettings.Rows)
                {
                    DataRow dr_payment_settings = dt_payment_settings.NewRow();

                    dr_payment_settings["DayFrom"] = o_data_row["DayFrom"];
                    dr_payment_settings["DayTo"] = o_data_row["DayTo"];
                    dr_payment_settings["DayPayment"] = o_data_row["DayPayment"];

                    dt_payment_settings.Rows.Add(dr_payment_settings);
                }
            }

            for (int i = dt_payment_settings.Rows.Count + 1; i <= 31; i++)
            {
                DataRow dr_payment_settings = dt_payment_settings.NewRow();
                dt_payment_settings.Rows.Add(dr_payment_settings);
            }

            repPaymentSettings.DataSource = dt_payment_settings;
            repPaymentSettings.DataBind();

            divAgreementPaymentForm.Visible = true;

            ddlCard.Focus();

            lnkCloseTop.HRef = String.Format("FrameForm.aspx?i={0}&q={1}&p=payment", Request.QueryString.Get("i"), Request.QueryString.Get("q"));
            lnkCloseBottom.HRef = lnkCloseTop.HRef;
        }

        protected void Save_Payment(object sender, EventArgs e)
        {
            if (AgreementID <= 0) { return; }

            if (!Page.IsValid) { return; }

            AgreementPaymentModel o_agreement_payment = new AgreementPaymentModel();

            o_agreement_payment.ID = (int)ViewState["AgreementPaymentID"];
            o_agreement_payment.UserID = n_user_id;
            o_agreement_payment.AgreementID = AgreementID;
            o_agreement_payment.CompanyID = (int)ViewState["CompanyID"];
            o_agreement_payment.CardID = Convert.ToInt32(ddlCard.SelectedValue);

            o_agreement_payment.DateStart = (DateTime)Common.Convert_To_Date(txtDateStart.Text);
            o_agreement_payment.DateEnd = (DateTime)Common.Convert_To_Date(txtDateEnd.Text);

            AgreementPaymentAction.Check(ref o_agreement_payment);  // Returns AgreementPeriodID

            if (o_agreement_payment.ErrorMessage != "")
            {
                lblError.Text = o_agreement_payment.ErrorMessage;
                return;
            }

            Check_Payment_Settings(ref o_agreement_payment);        // Returns TableAgreementPaymentSettings

            if (o_agreement_payment.ErrorMessage != "")
            {
                lblPaymentSettingsError.Text = o_agreement_payment.ErrorMessage;
                return;
            }

            AgreementPaymentAction.Update(ref o_agreement_payment);

            if (o_agreement_payment.ErrorMessage != "")
            {
                lblError.Text = o_agreement_payment.ErrorMessage;
                return;
            }

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}&p={3}", "updated", Request.QueryString.Get("i"), Request.QueryString.Get("q"), "payment"));
        }

        protected void Delete_Payment(object sender, CommandEventArgs e)
        {
            AgreementPaymentModel o_agreement_payment = new AgreementPaymentModel();

            o_agreement_payment.ID = Convert.ToInt32(e.CommandArgument);
            o_agreement_payment.UserID = n_user_id;
            o_agreement_payment.AgreementID = AgreementID;

            bool b_deleted = AgreementPaymentAction.Delete(ref o_agreement_payment);

            if (o_agreement_payment.ErrorMessage != "")
            {
                lblError.Text = o_agreement_payment.ErrorMessage;
                return;
            }

            if (b_deleted == false)
            {
                lblError.Text = "No item was deleted.";
                return;
            }

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}&p={3}", "deleted", Request.QueryString.Get("i"), Request.QueryString.Get("q"), "payment"));
        }

        protected void cvDateRange_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string s_error = "";

            string s_date_start = txtDateStart.Text.Trim();
            string s_date_end = txtDateEnd.Text.Trim();

            DateTime? d_date_start = Common.Convert_To_Date(s_date_start);
            DateTime? d_date_end = Common.Convert_To_Date(s_date_end);

            if (d_date_start == null || d_date_end == null)
            {
                if (d_date_start == null)
                {
                    s_error = " 'Start Date' not valid. ";
                }

                if (d_date_end == null)
                {
                    s_error += " 'End Date' not valid. ";
                }
            }
            else if (d_date_start > d_date_end)
            {
                s_error = "'Date Start' can't be greater than 'Date End'.";
            }

            if (s_error != "")
            {
                cvDateRange.ErrorMessage = s_error;
            }

            args.IsValid = (s_error == "");
        }

        private void Check_Payment_Settings(ref AgreementPaymentModel o_agreement_payment)
        {
            DataTable dt_payment_settings = new DataTable();

            dt_payment_settings.Columns.Add("DayFrom", typeof(int));
            dt_payment_settings.Columns.Add("DayTo", typeof(int));
            dt_payment_settings.Columns.Add("DayPayment", typeof(int));

            int n_days = 0;

            foreach (RepeaterItem o_item in repPaymentSettings.Items)
            {
                TextBox txtDayFrom = (TextBox)o_item.FindControl("txtDayFrom");
                TextBox txtDayTo = (TextBox)o_item.FindControl("txtDayTo");
                TextBox txtDayPayment = (TextBox)o_item.FindControl("txtDayPayment");

                int n_day_from = 0, n_day_to = 0, n_day_payment = 0;

                int.TryParse(txtDayFrom.Text.Trim(), out n_day_from);
                int.TryParse(txtDayTo.Text.Trim(), out n_day_to);
                int.TryParse(txtDayPayment.Text.Trim(), out n_day_payment);

                if (n_day_from > 0 && n_day_to > 0 & n_day_payment > 0)
                {
                    if (n_day_from > n_day_to)
                    {
                        o_agreement_payment.ErrorMessage = "'Day From' can't be greater than 'Day To'.";
                        return;
                    }

                    string s_query = String.Format(" ( {0} >= DayFrom AND {0} <= DayTo ) OR ( {1} >= DayFrom AND {1} <= DayTo ) ", n_day_from, n_day_to);

                    DataRow[] dr_payment_settings = dt_payment_settings.Select(s_query);

                    if (dr_payment_settings.Length > 0)
                    {
                        o_agreement_payment.ErrorMessage = "Days periods can not overlap.";
                        return;
                    }

                    n_days += n_day_to - n_day_from + 1;

                    DataRow o_data_row = dt_payment_settings.NewRow();

                    o_data_row["DayFrom"] = n_day_from;
                    o_data_row["DayTo"] = n_day_to;
                    o_data_row["DayPayment"] = n_day_payment;

                    dt_payment_settings.Rows.Add(o_data_row);
                }
            }

            if (o_agreement_payment.ErrorMessage == "" && n_days != 31)
            {
                o_agreement_payment.ErrorMessage = "Missing days in settings.";
                return;
            }

            o_agreement_payment.TableAgreementPaymentSettings = dt_payment_settings;
        }

        private void Bind_Form(AgreementModel o_agreement)
        {
            string s_error = "";

            ViewState["CompanyID"] = o_agreement.CompanyID;
            ViewState["CreditID"] = o_agreement.CreditID;

            lblCompanyName.Text = o_agreement.CompanyName;
            lblCreditName.Text = o_agreement.CreditName;

            DataTable dt_agreement_period = new DataTable();
            DataTable dt_agreement_payment = new DataTable();
            DataTable dt_agreement_payment_settings = new DataTable();

            AgreementPaymentAction.Select(ref dt_agreement_period, ref dt_agreement_payment, ref dt_agreement_payment_settings, ref s_error, n_user_id, null, AgreementID);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            ViewState["TableAgreementPayment"] = dt_agreement_payment;
            ViewState["TableAgreementPaymentSettings"] = dt_agreement_payment_settings;

            repAgreementPeriod.DataSource = dt_agreement_period;
            repAgreementPeriod.DataBind();
        }
    }
}
