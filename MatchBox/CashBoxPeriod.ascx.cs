using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public partial class CashBoxPeriod : UserControl
    {
        public int CashBoxID { get; set; }

        private int n_user_id = 0;
        private int n_company_id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CashBoxID <= 0) { return; }

            n_user_id = ((UserModel)Session["User"]).ID;

            if (ViewState["CompanyID"] != null) { n_company_id = (int)ViewState["CompanyID"]; }

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

                if (Request.QueryString["period"] != null)
                {
                    // SHOW CashBox Period FORM ( period = CashBoxPeriodID )

                    CommandEventArgs o_args = new CommandEventArgs("Edit_Period", Request.QueryString.Get("period"));

                    Edit_Period(null, o_args);
                }
            }

            CashBoxModel o_cashbox = new CashBoxModel();

            o_cashbox.ID = CashBoxID;
            o_cashbox.UserID = n_user_id;

            CashBoxAction.Select(ref o_cashbox);

            if (o_cashbox.ErrorMessage != "")
            {
                lblError.Text = o_cashbox.ErrorMessage;
                return;
            }

            ViewState["CompanyID"] = o_cashbox.CompanyID;

            Bind_Form(o_cashbox);
        }

        protected void repCashBoxPeriod_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView o_data_row = (DataRowView)e.Item.DataItem;

                Label lblStart = (Label)e.Item.FindControl("lblDateStart");
                Label lblEnd = (Label)e.Item.FindControl("lblDateEnd");
                Label lblTerminal = (Label)e.Item.FindControl("lblTerminal");

                int n_cashbox_period_id = 0, n_terminal = 0;

                int.TryParse(o_data_row["ID"].ToString(), out n_cashbox_period_id);
                int.TryParse(o_data_row["TerminalID"].ToString(), out n_terminal);

                DateTime d_start = (DateTime)o_data_row["DateStart"];
                DateTime? d_end = null;

                if (o_data_row["DateEnd"].ToString() != "") { d_end = (DateTime)o_data_row["DateEnd"]; }

                lblStart.Text = String.Format("{0:dd/MM/yyyy}", d_start);

                if (d_end != null)
                {
                    lblEnd.Text = String.Format("{0:dd/MM/yyyy}", d_end);
                }
                else
                {
                    lblEnd.Text = "__/__/____";
                }

                if (n_terminal > 0)
                {
                    lblTerminal.Text = o_data_row["Terminal"].ToString();
                }
                else
                {
                    lblTerminal.Text = "N / A";
                }

                DataTable dt_cashbox_period_terminal = (DataTable)ViewState["TableCashBoxPeriodTerminal"];

                if (dt_cashbox_period_terminal != null || dt_cashbox_period_terminal.Rows.Count > 0)
                {
                    DataRow[] dr_cashbox_period_terminal = dt_cashbox_period_terminal.Select("CashBoxPeriodID = " + n_cashbox_period_id);

                    if (dr_cashbox_period_terminal.Length > 0)
                    {
                        Repeater repCashBoxPeriodTerminal = (Repeater)e.Item.FindControl("repCashBoxPeriodTerminal");

                        repCashBoxPeriodTerminal.DataSource = dr_cashbox_period_terminal.CopyToDataTable();
                        repCashBoxPeriodTerminal.DataBind();
                    }
                }
            }
        }

        protected void Edit_Period(object sender, CommandEventArgs e)
        {
            if (CashBoxID <= 0) { return; }

            int n_cashbox_period_id = 0;
            int.TryParse(e.CommandArgument.ToString(), out n_cashbox_period_id);

            Bind_Form_Period(n_cashbox_period_id);

            divTerminalForm.Visible = false;
            divPeriodForm.Visible = true;

            txtDateStart.Focus();
        }

        protected void Add_Terminal(object sender, CommandEventArgs e)
        {
            if (CashBoxID <= 0) { return; }

            int n_cashbox_period_id = 0;
            int.TryParse(e.CommandArgument.ToString(), out n_cashbox_period_id);

            Bind_Form_Terminal(n_cashbox_period_id);

            divPeriodForm.Visible = false;
            divTerminalForm.Visible = true;

            ddlTerminal.Focus();
        }

        protected void ddlTerminal_SelectedIndexChanged(object sender, EventArgs e)
        {
            repAgreementItemTerminal.DataSource = null;
            repAgreementItemTerminal.DataBind();

            int n_terminal_id = Convert.ToInt32(ddlTerminal.SelectedValue);

            if (n_terminal_id == 0) { return; }

            string s_error = "";

            DataTable dt_agreement_item_terminal = AgreementItemAction.Select_Terminal(n_user_id, n_terminal_id, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            if (dt_agreement_item_terminal.Rows.Count > 0)
            {
                repAgreementItemTerminal.DataSource = dt_agreement_item_terminal;
                repAgreementItemTerminal.DataBind();
            }
        }

        protected void cvDateRange_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool b_valid = true;

            string s_date_start = txtDateStart.Text.Trim();
            string s_date_end = txtDateEnd.Text.Trim();

            DateTime? d_date_start = null;
            DateTime? d_date_end = null;

            if (s_date_start == "")
            {
                b_valid = false;
                cvDateRange.ErrorMessage = "Enter 'Start Date'.";
            }
            else
            {
                d_date_start = Common.Convert_To_Date(s_date_start);

                if (d_date_start == null)
                {
                    b_valid = false;
                    cvDateRange.ErrorMessage = "'Start Date' not valid.";
                }
            }

            if (b_valid == false)
            {
                txtDateStart.Focus();
                goto Finish;
            }

            if (s_date_end != "")
            {
                d_date_end = Common.Convert_To_Date(s_date_end);

                if (d_date_end == null)
                {
                    b_valid = false;
                    cvDateRange.ErrorMessage = "'End Date' not valid.";
                }
                else if (d_date_start > d_date_end)
                {
                    b_valid = false;
                    cvDateRange.ErrorMessage = "'Date Start' can't be greater than 'Date End'.";
                }

                if (b_valid == false)
                {
                    txtDateEnd.Focus();
                    goto Finish;
                }
            }

        Finish:

            args.IsValid = b_valid;
        }

        protected void btnSavePeriod_Click(object sender, EventArgs e)
        {
            if (CashBoxID <= 0) { return; }

            if (!Page.IsValid) { return; }

            CashBoxPeriodModel o_cashbox_period = new CashBoxPeriodModel();

            o_cashbox_period.ID = Convert.ToInt32(hidCashBoxPeriodID.Value);
            o_cashbox_period.UserID = n_user_id;
            o_cashbox_period.CashBoxID = CashBoxID;
            o_cashbox_period.TerminalID = Convert.ToInt32(hidTerminalID.Value);
            
            o_cashbox_period.DateStart = Common.Convert_To_Date(txtDateStart.Text);

            DateTime? d_date_end = Common.Convert_To_Date(txtDateEnd.Text);

            if (d_date_end != null) { o_cashbox_period.DateEnd = d_date_end; }

            CashBoxPeriodAction.Check(ref o_cashbox_period);

            if (o_cashbox_period.ErrorMessage != "")
            {
                lblError.Text = o_cashbox_period.ErrorMessage;
                return;
            }

            CashBoxPeriodAction.Update(ref o_cashbox_period);

            if (o_cashbox_period.ErrorMessage != "")
            {
                lblError.Text = o_cashbox_period.ErrorMessage;
                return;
            }

            string s_interface = Request.QueryString.Get("i");
            string s_guid = Request.QueryString.Get("q");

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}&p={3}&period={4}", "updated-period", s_interface, s_guid, "period", o_cashbox_period.ID));
        }

        protected void btnSaveCashBoxPeriodTerminal_Click(object sender, EventArgs e)
        {
            if (CashBoxID <= 0) { return; }

            if (!Page.IsValid) { return; }

            if (repAgreementItemTerminal.Items.Count == 0)
            {
                lblError.Text = "No Agreement Item/s available for specified Period and Terminal.";
                return;
            }

            int n_cashbox_period_id = Convert.ToInt32(hidCashBoxPeriodID.Value);
            int n_terminal_id = Convert.ToInt32(ddlTerminal.SelectedValue);

            DateTime d_start = (DateTime)ViewState["DateStart"];
            DateTime? d_end = (DateTime?)ViewState["DateEnd"];

            DataTable dt_agreement_item_terminal = new DataTable();

            dt_agreement_item_terminal.Columns.Add("ID");

            foreach (RepeaterItem item in repAgreementItemTerminal.Items)
            {
                CheckBox o_check_box = (CheckBox)item.FindControl("chkIsSelected");

                if (o_check_box.Checked)
                {
                    HiddenField o_hidden = (HiddenField)item.FindControl("hidID");

                    DataRow dr_agreement_item_terminal = dt_agreement_item_terminal.NewRow();

                    dr_agreement_item_terminal["ID"] = Convert.ToInt32(o_hidden.Value);

                    dt_agreement_item_terminal.Rows.Add(dr_agreement_item_terminal);
                }
            }

            if (dt_agreement_item_terminal.Rows.Count == 0)
            {
                lblError.Text = "Select one or more Agreement Item/s.";
                return;
            }

            string s_error = "";

            CashBoxPeriodAction.Check_Terminal(n_user_id, n_cashbox_period_id, n_terminal_id, d_start, d_end, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            CashBoxPeriodAction.Update_Terminal(n_user_id, n_cashbox_period_id, n_terminal_id, d_start, d_end, dt_agreement_item_terminal, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            string s_interface = Request.QueryString.Get("i");
            string s_guid = Request.QueryString.Get("q");

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}&p={3}", "updated-terminal", s_interface, s_guid, "period"));
        }

        protected void Close_Form(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("FrameForm.aspx?i={0}&q={1}&p={2}", Request.QueryString.Get("i"), Request.QueryString.Get("q"), "period"));
        }

        protected void btnDeletePeriod_Command(object sender, CommandEventArgs e)
        {
            CashBoxPeriodModel o_cashbox_period = new CashBoxPeriodModel();

            o_cashbox_period.ID = Convert.ToInt32(e.CommandArgument);
            o_cashbox_period.UserID = n_user_id;
            o_cashbox_period.CashBoxID = CashBoxID;

            bool b_deleted = CashBoxPeriodAction.Delete(ref o_cashbox_period);

            if (o_cashbox_period.ErrorMessage != "")
            {
                lblError.Text = o_cashbox_period.ErrorMessage;
                return;
            }

            if (b_deleted == false)
            {
                lblError.Text = "No item was deleted.";
                return;
            }

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}&p={3}", "deleted-period", Request.QueryString.Get("i"), Request.QueryString.Get("q"), "period"));
        }

        protected void btnDeleteCashBoxPeriodTerminal_Command(object sender, CommandEventArgs e)
        {
            string s_error = "";
            string[] arr_arguments = e.CommandArgument.ToString().Split(',');

            int n_id = Convert.ToInt32(arr_arguments[0]);
            int n_cashbox_period_id = Convert.ToInt32(arr_arguments[1]);

            bool b_deleted = CashBoxPeriodAction.Delete_Terminal(n_id, n_user_id, n_cashbox_period_id, ref s_error);

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

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}&p={3}", "deleted-terminal", Request.QueryString.Get("i"), Request.QueryString.Get("q"), "period"));
        }

        private void Bind_Form(CashBoxModel o_cashbox)
        {
            string s_error = "";

            lblCashBoxNumber.Text = o_cashbox.CashBoxNumber;
            lblCashBoxName.Text = o_cashbox.CashBoxName;

            DataTable dt_cashbox_period = new DataTable();
            DataTable dt_cashbox_period_terminal = new DataTable();

            CashBoxPeriodAction.Select(ref dt_cashbox_period, ref dt_cashbox_period_terminal, ref s_error, n_user_id, CashBoxID);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            ViewState["TableCashBoxPeriodTerminal"] = dt_cashbox_period_terminal;

            repCashBoxPeriod.DataSource = dt_cashbox_period;
            repCashBoxPeriod.DataBind();
        }

        private void Bind_Form_Period(int n_cashbox_period_id)
        {
            hidCashBoxPeriodID.Value = n_cashbox_period_id.ToString();

            if (n_cashbox_period_id == 0) { return; }

            CashBoxPeriodModel o_cashbox_period = new CashBoxPeriodModel();

            o_cashbox_period.ID = n_cashbox_period_id;
            o_cashbox_period.UserID = n_user_id;
            o_cashbox_period.CashBoxID = CashBoxID;

            CashBoxPeriodAction.Select(ref o_cashbox_period);

            if (o_cashbox_period.ErrorMessage != "")
            {
                lblError.Text = o_cashbox_period.ErrorMessage;
                return;
            }

            txtDateStart.Text = String.Format("{0:dd/MM/yyyy}", o_cashbox_period.DateStart);

            if (o_cashbox_period.DateEnd != null)
            {
                txtDateEnd.Text = String.Format("{0:dd/MM/yyyy}", o_cashbox_period.DateEnd);
            }

            hidTerminalID.Value = o_cashbox_period.TerminalID.ToString();
            txtTerminal.Text = o_cashbox_period.Terminal;
        }

        private void Bind_Form_Terminal(int n_cashbox_period_id)
        {
            string s_error = "";

            repAgreementItemTerminal.DataSource = null;
            repAgreementItemTerminal.DataBind();

            ddlTerminal.Items.Clear();

            lblDateStart.Text = "";
            lblDateEnd.Text = "";

            CashBoxPeriodModel o_cashbox_period = new CashBoxPeriodModel();

            o_cashbox_period.ID = n_cashbox_period_id;
            o_cashbox_period.UserID = n_user_id;
            o_cashbox_period.CashBoxID = CashBoxID;

            CashBoxPeriodAction.Select(ref o_cashbox_period);

            if (o_cashbox_period.ErrorMessage != "")
            {
                lblError.Text = o_cashbox_period.ErrorMessage;
                return;
            }

            ViewState["DateStart"] = o_cashbox_period.DateStart;
            ViewState["DateEnd"] = o_cashbox_period.DateEnd;

            lblDateStart.Text = String.Format("{0:dd/MM/yyyy}", o_cashbox_period.DateStart);

            if (o_cashbox_period.DateEnd != null)
            {
                lblDateEnd.Text = String.Format("{0:dd/MM/yyyy}", o_cashbox_period.DateEnd);
            }
            else
            {
                lblDateEnd.Text = "__/__/____";
            }

            hidCashBoxPeriodID.Value = n_cashbox_period_id.ToString();

            DataTable dt_cashbox_period_terminal_list = new DataTable();

            CashBoxPeriodAction.Select_Terminal(ref dt_cashbox_period_terminal_list, ref s_error, n_user_id, n_company_id, (DateTime)o_cashbox_period.DateStart, o_cashbox_period.DateEnd);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            if (dt_cashbox_period_terminal_list.Rows.Count > 0)
            {
                ddlTerminal.DataSource = dt_cashbox_period_terminal_list;
                ddlTerminal.DataValueField = "TerminalID";
                ddlTerminal.DataTextField = "Terminal";
                ddlTerminal.DataBind();
            }
            else
            {
                lblError.Text = "No Terminal/s available for specified Period.";
            }

            ddlTerminal.Items.Insert(0, new ListItem("Terminal", "0"));
        }
    }
}
