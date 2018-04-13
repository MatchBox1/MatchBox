using System;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public partial class AgreementDiscount : UserControl
    {
        public int AgreementID { get; set; }

        private int n_user_id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (AgreementID <= 0) { return; }

            n_user_id = ((UserModel)Session["User"]).ID;

            if (Page.IsPostBack) { return; }

            lnkClose.HRef = String.Format("FrameForm.aspx?i={0}&q={1}&p={2}", Request.QueryString.Get("i"), Request.QueryString.Get("q"), "discount");

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

            ddlDiscount.Focus();
        }

        protected void repAgreementDiscount_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView o_data_row = (DataRowView)e.Item.DataItem;

                int n_agreement_discount_id = 0;
                int.TryParse(o_data_row["ID"].ToString(), out n_agreement_discount_id);

                string s_period_type_key = o_data_row["DiscountPeriodTypeKey"].ToString();

                if (s_period_type_key == "weekly" || s_period_type_key == "fortnightly" || s_period_type_key == "one-third-monthly")
                {
                    if (s_period_type_key == "one-third-monthly")
                    {
                        DataTable dt_agreement_discount_settings = (DataTable)ViewState["TableAgreementDiscountSettings"];

                        DataRow[] dr_agreement_discount_settings = dt_agreement_discount_settings.Select(" AgreementDiscountID = " + n_agreement_discount_id);

                        if (dr_agreement_discount_settings.Length > 0)
                        {
                            Repeater repAgreementDiscountSettings = (Repeater)e.Item.FindControl("repAgreementDiscountSettings");

                            repAgreementDiscountSettings.DataSource = dr_agreement_discount_settings.CopyToDataTable();
                            repAgreementDiscountSettings.DataBind();
                            repAgreementDiscountSettings.Visible = true;
                        }
                    }
                    else
                    {
                        string[] arr_day_names_short = CultureInfo.CurrentCulture.DateTimeFormat.ShortestDayNames;

                        int n_week_start = 0;
                        int.TryParse(o_data_row["WeekStart"].ToString(), out n_week_start);

                        int n_payment_after_days = 0;
                        int.TryParse(o_data_row["PaymentAfterDays"].ToString(), out n_payment_after_days);

                        string s_week_start = arr_day_names_short[n_week_start];
                        string s_week_end = "";

                        if (n_week_start == 0)
                        {
                            s_week_end = arr_day_names_short[arr_day_names_short.Length - 1];
                        }
                        else
                        {
                            s_week_end = arr_day_names_short[n_week_start - 1];
                        }

                        Label lblPeriodSettings = (Label)e.Item.FindControl("lblPeriodSettings");

                        lblPeriodSettings.Text = s_week_start + " - " + s_week_end + " (+" + n_payment_after_days + ")";
                        lblPeriodSettings.Visible = true;
                    }
                }

                DataTable dt_agreement_discount_terminal = (DataTable)ViewState["TableAgreementDiscountTerminal"];

                DataRow[] dr_agreement_discount_terminal = dt_agreement_discount_terminal.Select(" AgreementDiscountID = " + n_agreement_discount_id);

                if (dr_agreement_discount_terminal.Length > 0)
                {
                    Repeater repAgreementDiscountTerminal = (Repeater)e.Item.FindControl("repAgreementDiscountTerminal");

                    repAgreementDiscountTerminal.DataSource = dr_agreement_discount_terminal.CopyToDataTable();
                    repAgreementDiscountTerminal.DataBind();
                }
            }
        }

        protected void ddlDiscountPeriodType_SelectedIndexChanged(object sender, EventArgs e)
        {
            divWeekStart_Change.Visible = false;
            divPaymentAfterDays_Change.Visible = false;
            divDiscountPeriodOneThirdMonthly_Change.Visible = false;

            ddlWeekStart.SelectedIndex = 0;
            lblWeekEnd.Text = "---";
            txtPaymentAfterDays.Text = "";

            repOneThirdMonthly.DataSource = null;
            repOneThirdMonthly.DataBind();

            int n_id = Convert.ToInt32(ddlDiscountPeriodType.SelectedValue);

            if (n_id == 0) { return; }

            DataTable dt_discount_period_type = (DataTable)ViewState["TableDiscountPeriodType"];

            string s_discount_period_type_key = dt_discount_period_type.Select(" ID = " + n_id)[0]["DiscountPeriodTypeKey"].ToString();

            divPaymentAfterDays_Change.Visible = (s_discount_period_type_key != "one-third-monthly");

            if (s_discount_period_type_key == "one-third-monthly")
            {
                DataTable dt_one_third_monthly = new DataTable();
                dt_one_third_monthly.Columns.Add("DayFrom");
                dt_one_third_monthly.Columns.Add("DayTo");
                dt_one_third_monthly.Columns.Add("DayPayment");

                for (int i = dt_one_third_monthly.Rows.Count + 1; i <= 3; i++)
                {
                    DataRow dr_one_third_monthly = dt_one_third_monthly.NewRow();
                    dt_one_third_monthly.Rows.Add(dr_one_third_monthly);
                }

                repOneThirdMonthly.DataSource = dt_one_third_monthly;
                repOneThirdMonthly.DataBind();

                divDiscountPeriodOneThirdMonthly_Change.Visible = true;
            }
            else if (s_discount_period_type_key == "weekly" || s_discount_period_type_key == "fortnightly")
            {
                divWeekStart_Change.Visible = true;
            }
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

            if (s_error != "") { goto Finish; }

            string[] arr_date_start_item = lblDateStart.Text.Split('-');
            string[] arr_date_end_item = lblDateEnd.Text.Split('-');

            DateTime d_date_start_item = new DateTime(Convert.ToInt32(arr_date_start_item[2]), Convert.ToInt32(arr_date_start_item[1]), Convert.ToInt32(arr_date_start_item[0]));
            DateTime d_date_end_item = new DateTime(Convert.ToInt32(arr_date_end_item[2]), Convert.ToInt32(arr_date_end_item[1]), Convert.ToInt32(arr_date_end_item[0]));

            bool b_part = (d_date_start >= d_date_start_item) && (d_date_start <= d_date_end_item) && (d_date_end >= d_date_start_item) && (d_date_end <= d_date_end_item);

            if (b_part == false)
            {
                s_error = "'Discount Item' date range must be a part of 'Clearing Item' date range.";
                goto Finish;
            }

        Finish:

            if (s_error != "")
            {
                cvDateRange.ErrorMessage = s_error;
            }

            args.IsValid = (s_error == "");
        }

        protected void cvPaymentsRange_ServerValidate(object source, ServerValidateEventArgs args)
        {
            cvPaymentsRange.Text = "";

            int n_payments_from_item = Convert.ToInt32(lblPaymentsFrom.Text);
            int n_payments_to_item = Convert.ToInt32(lblPaymentsTo.Text);

            int n_payments_from = Convert.ToInt32(txtPaymentsFrom.Text);
            int n_payments_to = Convert.ToInt32(txtPaymentsTo.Text);

            bool b_valid = (n_payments_from >= n_payments_from_item) && (n_payments_from <= n_payments_to_item) && (n_payments_to >= n_payments_from_item) && (n_payments_to <= n_payments_to_item);

            if (b_valid == false)
            {
                cvPaymentsRange.Text = "'Discount Item' payments range must be a part of 'Clearing Item' payments range.";
            }

            args.IsValid = b_valid;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (AgreementID <= 0) { return; }

            if (!Page.IsValid) { return; }

            AgreementDiscountModel o_agreement_discount = new AgreementDiscountModel();

            o_agreement_discount.ID = 0;    // New Only !!!
            o_agreement_discount.UserID = n_user_id;
            o_agreement_discount.AgreementItemID = (int)ViewState["AgreementItemID"];
            o_agreement_discount.DiscountID = Convert.ToInt32(ddlDiscount.SelectedValue);
            o_agreement_discount.DiscountPeriodTypeID = Convert.ToInt32(ddlDiscountPeriodType.SelectedValue);


            o_agreement_discount.PaymentsFrom = Convert.ToInt32(txtPaymentsFrom.Text.Trim());
            o_agreement_discount.PaymentsTo = Convert.ToInt32(txtPaymentsTo.Text.Trim());

            DataTable dt_discount_period_type = (DataTable)ViewState["TableDiscountPeriodType"];

            string s_discount_period_type_key = dt_discount_period_type.Select(" ID = " + o_agreement_discount.DiscountPeriodTypeID)[0]["DiscountPeriodTypeKey"].ToString();

            if (s_discount_period_type_key == "weekly" || s_discount_period_type_key == "fortnightly")
            {
                o_agreement_discount.WeekStart = Convert.ToInt32(ddlWeekStart.SelectedValue);
                o_agreement_discount.PaymentAfterDays = Convert.ToInt32(txtPaymentAfterDays.Text.Trim());
            }

            o_agreement_discount.Commission = Convert.ToDouble(txtCommission.Text.Trim());
            o_agreement_discount.BaseDate = ddlBaseDate.SelectedValue;

            o_agreement_discount.DateStart = (DateTime)Common.Convert_To_Date(txtDateStart.Text);
            o_agreement_discount.DateEnd = (DateTime)Common.Convert_To_Date(txtDateEnd.Text);

            if (s_discount_period_type_key == "one-third-monthly")
            {
                Check_Discount_Settings(ref o_agreement_discount);      // Returns TableAgreementDiscountSettings

                if (o_agreement_discount.ErrorMessage != "")
                {
                    lblError.Text = o_agreement_discount.ErrorMessage;
                    return;
                }
            }

            foreach (RepeaterItem o_item in repAgreementItemTerminal.Items)
            {
                if (o_item.ItemType == ListItemType.Item || o_item.ItemType == ListItemType.AlternatingItem)
                {
                    HtmlInputCheckBox chkIsSelected = (HtmlInputCheckBox)o_item.FindControl("chkIsSelected");

                    if (chkIsSelected.Checked == false) { continue; }

                    HiddenField hidAgreementItemTerminalID = (HiddenField)o_item.FindControl("hidAgreementItemTerminalID");

                    DataRow o_data_row = o_agreement_discount.TableAgreementDiscountTerminal.NewRow();

                    o_data_row["ID"] = Convert.ToInt32(hidAgreementItemTerminalID.Value);

                    o_agreement_discount.TableAgreementDiscountTerminal.Rows.Add(o_data_row);
                }
            }

            if (o_agreement_discount.TableAgreementDiscountTerminal.Rows.Count == 0)
            {
                lblError.Text = "Select 'Terminal'";
                return;
            }

            AgreementDiscountAction.Check(ref o_agreement_discount);    // Returns AgreementPeriodID

            if (o_agreement_discount.ErrorMessage != "")
            {
                lblError.Text = o_agreement_discount.ErrorMessage;
                return;
            }

            AgreementDiscountAction.Update(ref o_agreement_discount);

            if (o_agreement_discount.ErrorMessage != "")
            {
                lblError.Text = o_agreement_discount.ErrorMessage;
                return;
            }

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}&p={3}", "updated", Request.QueryString.Get("i"), Request.QueryString.Get("q"), "discount"));
        }

        protected void btnDeleteAgreementDiscount_Command(object sender, CommandEventArgs e)
        {
            string s_error = "";

            int n_id = Convert.ToInt32(e.CommandArgument);

            bool b_deleted = AgreementDiscountAction.Delete(n_id, n_user_id, AgreementID, ref s_error);

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

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}&p={3}", "deleted", Request.QueryString.Get("i"), Request.QueryString.Get("q"), "discount"));
        }

        private void Check_Discount_Settings(ref AgreementDiscountModel o_agreement_discount)
        {
            DataTable dt_discount_settings = new DataTable();

            dt_discount_settings.Columns.Add("DayFrom", typeof(int));
            dt_discount_settings.Columns.Add("DayTo", typeof(int));
            dt_discount_settings.Columns.Add("PaymentAfterDays", typeof(int));

            int n_days = 0, n_days_max = 31;

            foreach (RepeaterItem o_item in repOneThirdMonthly.Items)
            {
                TextBox txtDayFrom = (TextBox)o_item.FindControl("txtDayFrom");
                TextBox txtDayTo = (TextBox)o_item.FindControl("txtDayTo");
                TextBox txtPaymentAfterDays = (TextBox)o_item.FindControl("txtPaymentAfterDays");

                int n_day_from = 0, n_day_to = 0, n_payment_after_day = 0;

                int.TryParse(txtDayFrom.Text.Trim(), out n_day_from);
                int.TryParse(txtDayTo.Text.Trim(), out n_day_to);
                int.TryParse(txtPaymentAfterDays.Text.Trim(), out n_payment_after_day);

                if (n_day_from > 0 && n_day_to > 0 & n_payment_after_day > 0)
                {
                    string s_query = " ( {0} >= DayFrom AND {0} <= DayTo ) OR ( {1} >= DayFrom AND {1} <= DayTo ) ";

                    if (n_day_from > n_day_to)
                    {
                        s_query = String.Format(s_query, n_day_to, n_day_from);
                    }
                    else
                    {
                        s_query = String.Format(s_query, n_day_from, n_day_to);
                    }

                    DataRow[] dr_discount_settings = dt_discount_settings.Select(s_query);

                    if (dr_discount_settings.Length > 0)
                    {
                        o_agreement_discount.ErrorMessage = "Days periods can not overlap.";
                        return;
                    }

                    if (n_day_from > n_day_to)
                    {
                        n_days += n_days_max - n_day_from + n_day_to + 1;
                    }
                    else
                    {
                        n_days += n_day_to - n_day_from + 1;
                    }

                    DataRow o_data_row = dt_discount_settings.NewRow();

                    o_data_row["DayFrom"] = n_day_from;
                    o_data_row["DayTo"] = n_day_to;
                    o_data_row["PaymentAfterDays"] = n_payment_after_day;

                    dt_discount_settings.Rows.Add(o_data_row);
                }
            }

            if (o_agreement_discount.ErrorMessage == "" && n_days != n_days_max)
            {
                o_agreement_discount.ErrorMessage = "Missing days in settings.";
                return;
            }

            o_agreement_discount.TableAgreementDiscountSettings = dt_discount_settings;
        }

        private void Bind_Form(AgreementModel o_agreement)
        {
            lblCompanyName.Text = o_agreement.CompanyName;
            lblCreditName.Text = o_agreement.CreditName;

            string s_error = "";

            DataTable dt_agreement_discount = new DataTable();
            DataTable dt_agreement_discount_terminal = new DataTable();
            DataTable dt_agreement_discount_settings = new DataTable();

            AgreementDiscountAction.Select(ref dt_agreement_discount, ref dt_agreement_discount_terminal, ref dt_agreement_discount_settings, ref s_error, n_user_id, AgreementID);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            ViewState["TableAgreementDiscountTerminal"] = dt_agreement_discount_terminal;
            ViewState["TableAgreementDiscountSettings"] = dt_agreement_discount_settings;

            if (dt_agreement_discount.Rows.Count > 0)
            {
                repAgreementDiscount.DataSource = dt_agreement_discount;
                repAgreementDiscount.DataBind();
            }

            int n_item_id = 0;

            if (Request.QueryString["item"] != null) { int.TryParse(Request.QueryString["item"], out n_item_id); }

            if (n_item_id == 0) { return; }

            string[] arr_day_names = CultureInfo.CurrentCulture.DateTimeFormat.DayNames;

            for (int i = 0; i < arr_day_names.Length; i++)
            {
                ddlWeekStart.Items.Add(new ListItem(arr_day_names[i], i.ToString()));
            }

            DataTable dt_discount_period_type = new DataTable();

            DB.Bind_Data_Table("sprDiscountPeriodType", ref dt_discount_period_type, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            ViewState["TableDiscountPeriodType"] = dt_discount_period_type;

            ddlDiscountPeriodType.DataSource = dt_discount_period_type;
            ddlDiscountPeriodType.DataValueField = "ID";
            ddlDiscountPeriodType.DataTextField = "DiscountPeriodTypeName";
            ddlDiscountPeriodType.DataBind();
            ddlDiscountPeriodType.Items.Insert(0, new ListItem("", "0"));

            DB.Bind_List_Control(ddlDiscount, "sprDiscount", ref s_error);
            DB.Bind_List_Control(ddlBaseDate, "sprBaseDate", ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            AgreementItemModel o_agreement_item = new AgreementItemModel();

            o_agreement_item.ID = n_item_id;
            o_agreement_item.UserID = n_user_id;
            o_agreement_item.AgreementID = AgreementID;

            AgreementItemAction.Select(ref o_agreement_item);

            if (o_agreement_item.ErrorMessage != "")
            {
                lblError.Text = o_agreement_item.ErrorMessage;
                return;
            }

            divAgreementDiscountForm.Visible = true;

            ViewState["AgreementItemID"] = o_agreement_item.ID;

            lblDateStart.Text = String.Format("{0:dd/MM/yyyy}", o_agreement_item.DateStart);
            lblDateEnd.Text = String.Format("{0:dd/MM/yyyy}", o_agreement_item.DateEnd);
            lblPaymentsFrom.Text = o_agreement_item.PaymentsFrom.ToString();
            lblPaymentsTo.Text = o_agreement_item.PaymentsTo.ToString();

            lblCard.Text = o_agreement_item.CardName;
            lblOperationType.Text = o_agreement_item.OperationTypeName;
            lblCommissionLocal.Text = o_agreement_item.CommissionLocal + "%";
            lblCommissionAbroad.Text = o_agreement_item.CommissionAbroad + "%";

            repAgreementItemTerminal.DataSource = o_agreement_item.TableAgreementItemTerminal;
            repAgreementItemTerminal.DataBind();
        }
    }
}
