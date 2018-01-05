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
    public partial class AgreementItem : UserControl
    {
        public int AgreementID { get; set; }

        public bool IsVisibleItemForm { get; set; }

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

                DataTable dt_agreement_item = (DataTable)ViewState["TableAgreementItem"];

                DataRow[] dr_agreement_item = dt_agreement_item.Select(" AgreementPeriodID = " + n_agreement_period_id);

                if (dr_agreement_item.Length > 0)
                {
                    Repeater repAgreementItem = (Repeater)e.Item.FindControl("repAgreementItem");

                    repAgreementItem.DataSource = dr_agreement_item.CopyToDataTable();
                    repAgreementItem.DataBind();
                }
            }
        }

        protected void repAgreementItem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView o_data_row = (DataRowView)e.Item.DataItem;

                int n_agreement_item_id = 0;
                int.TryParse(o_data_row["ID"].ToString(), out n_agreement_item_id);

                DataTable dt_agreement_item_terminal = (DataTable)ViewState["TableAgreementItemTerminal"];

                DataRow[] dr_agreement_item_terminal = dt_agreement_item_terminal.Select(" AgreementItemID = " + n_agreement_item_id);

                if (dr_agreement_item_terminal.Length > 0)
                {
                    Repeater repAgreementItemTerminal = (Repeater)e.Item.FindControl("repAgreementItemTerminal");

                    repAgreementItemTerminal.DataSource = dr_agreement_item_terminal.CopyToDataTable();
                    repAgreementItemTerminal.DataBind();
                }

                HtmlAnchor lnkCopyItem = (HtmlAnchor)e.Item.FindControl("lnkCopyItem");
                HtmlAnchor lnkAddDiscount = (HtmlAnchor)e.Item.FindControl("lnkAddDiscount");

                lnkCopyItem.HRef = String.Format("FrameForm.aspx?i={0}&q={1}&p={2}&item={3}", Request.QueryString.Get("i"), Request.QueryString.Get("q"), "item", n_agreement_item_id);
                lnkAddDiscount.HRef = String.Format("FrameForm.aspx?i={0}&q={1}&p={2}&item={3}", Request.QueryString.Get("i"), Request.QueryString.Get("q"), "discount", n_agreement_item_id);
            }
        }

        protected void Date_Changed(object sender, EventArgs e)
        {
            ViewState["TableSupplierTerminalPeriod"] = null;

            ddlCard.Items.Clear();
            ddlCard.Items.Add(new ListItem("", "0"));

            lstCashBox.Items.Clear();
            lstTerminal.Items.Clear();
            lstSupplier.Items.Clear();
            lstSupplierGroup.Items.Clear();

            chkCashBox.Checked = false;
            chkTerminal.Checked = false;
            chkSupplier.Checked = false;
            chkSupplierGroup.Checked = false;

            chkCashBox.Visible = false;
            chkTerminal.Visible = false;
            chkSupplier.Visible = false;
            chkSupplierGroup.Visible = false;

            string s_date_start = txtDateStart.Text.Trim();
            string s_date_end = txtDateEnd.Text.Trim();

            if (s_date_start == "" || s_date_end == "") { return; }

            ServerValidateEventArgs args = new ServerValidateEventArgs(String.Empty, false);

            cvDateRange_ServerValidate(sender, args);

            cvDateRange.IsValid = args.IsValid;

            if (cvDateRange.IsValid == false) { return; }

            DateTime? d_date_start = Common.Convert_To_Date(s_date_start);
            DateTime? d_date_end = Common.Convert_To_Date(s_date_start);

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
                cvDateRange.IsValid = false;
                cvDateRange.ErrorMessage = "The date range must be a part of service period.";
                return;
            }

            Bind_Card((DateTime)d_date_start, (DateTime)d_date_end);

            string s_error = "";

            DataTable dt_supplier_terminal_period = new DataTable();

            SupplierTerminalAction.Select_Period(ref dt_supplier_terminal_period, ref s_error, n_user_id, AgreementID, (DateTime)d_date_start, (DateTime)d_date_end);

            if (s_error != "")
            {
                cvDateRange.IsValid = false;
                cvDateRange.ErrorMessage = s_error;
                return;
            }

            if (dt_supplier_terminal_period.Rows.Count == 0)
            {
                cvDateRange.IsValid = false;
                cvDateRange.ErrorMessage = "No supplier group exists in specified date range.";
                return;
            }

            ViewState["TableSupplierTerminalPeriod"] = dt_supplier_terminal_period;

            DataTable dt_group = new DataView(dt_supplier_terminal_period).ToTable(true, new string[] { "SupplierGroupID", "SupplierGroup" });

            lstSupplierGroup.DataSource = dt_group;
            lstSupplierGroup.DataValueField = "SupplierGroupID";
            lstSupplierGroup.DataTextField = "SupplierGroup";
            lstSupplierGroup.DataBind();

            chkSupplierGroup.Visible = true;
        }

        protected void chkSupplierGroup_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListItem o_list_item in lstSupplierGroup.Items)
            {
                o_list_item.Selected = chkSupplierGroup.Checked;
            }

            lstSupplierGroup_SelectedIndexChanged(null, null);
        }

        protected void lstSupplierGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstCashBox.Items.Clear();
            lstTerminal.Items.Clear();
            lstSupplier.Items.Clear();

            chkCashBox.Checked = false;
            chkTerminal.Checked = false;
            chkSupplier.Checked = false;

            chkCashBox.Visible = false;
            chkTerminal.Visible = false;
            chkSupplier.Visible = false;

            string s_group_array = "";

            foreach (ListItem o_list_item in lstSupplierGroup.Items)
            {
                if (o_list_item.Selected == true)
                {
                    if (s_group_array != "") { s_group_array += ","; }

                    s_group_array += o_list_item.Value;
                }
            }

            chkSupplierGroup.Checked = s_group_array != "" && s_group_array.Split(',').Length == lstSupplierGroup.Items.Count;

            if (s_group_array == "") { return; }

            DataTable dt_supplier_terminal_period = (DataTable)ViewState["TableSupplierTerminalPeriod"];

            DataRow[] dr_supplier = dt_supplier_terminal_period.Select(" SupplierGroupID IN (" + s_group_array + ") ");

            if (dr_supplier.Length == 0) { return; }

            DataTable dt_supplier = new DataView(dr_supplier.CopyToDataTable()).ToTable(true, new string[] { "SupplierID", "Supplier" });

            lstSupplier.DataSource = dt_supplier;
            lstSupplier.DataValueField = "SupplierID";
            lstSupplier.DataTextField = "Supplier";
            lstSupplier.DataBind();

            chkSupplier.Visible = true;
        }

        protected void chkSupplier_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListItem o_list_item in lstSupplier.Items)
            {
                o_list_item.Selected = chkSupplier.Checked;
            }

            lstSupplier_SelectedIndexChanged(null, null);
        }

        protected void lstSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstCashBox.Items.Clear();
            lstTerminal.Items.Clear();

            chkCashBox.Checked = false;
            chkTerminal.Checked = false;

            chkCashBox.Visible = false;
            chkTerminal.Visible = false;

            string s_supplier_array = "";

            foreach (ListItem o_list_item in lstSupplier.Items)
            {
                if (o_list_item.Selected == true)
                {
                    if (s_supplier_array != "") { s_supplier_array += ","; }

                    s_supplier_array += o_list_item.Value;
                }
            }

            chkSupplier.Checked = s_supplier_array != "" && s_supplier_array.Split(',').Length == lstSupplier.Items.Count;

            if (s_supplier_array == "") { return; }

            DataTable dt_supplier_terminal_period = (DataTable)ViewState["TableSupplierTerminalPeriod"];

            DataRow[] dr_terminal = dt_supplier_terminal_period.Select(" SupplierID IN (" + s_supplier_array + ") ");

            if (dr_terminal.Length == 0) { return; }

            DataTable dt_terminal = new DataView(dr_terminal.CopyToDataTable()).ToTable(true, new string[] { "ID", "TerminalID", "Terminal" });

            dt_terminal.Columns["ID"].ColumnName = "SupplierTerminalID";

            lstTerminal.DataSource = dt_terminal;
            lstTerminal.DataValueField = "SupplierTerminalID";
            lstTerminal.DataTextField = "Terminal";
            lstTerminal.DataBind();

            ViewState["TableTerminal"] = dt_terminal;

            chkTerminal.Visible = true;
        }

        protected void chkTerminal_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListItem o_list_item in lstTerminal.Items)
            {
                o_list_item.Selected = chkTerminal.Checked;
            }

            lstTerminal_SelectedIndexChanged(null, null);
        }

        protected void lstTerminal_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstCashBox.Items.Clear();

            chkCashBox.Checked = false;
            chkCashBox.Visible = false;

            bool b_checked = true;

            DateTime d_date_start = (DateTime)Common.Convert_To_Date(txtDateStart.Text);
            DateTime d_date_end = (DateTime)Common.Convert_To_Date(txtDateEnd.Text);

            DataTable dt_cashbox_period = (DataTable)ViewState["TableCashBoxPeriod"];

            foreach (ListItem o_list_item in lstTerminal.Items)
            {
                if (o_list_item.Selected == false)
                {
                    b_checked = false;
                }
                else
                {
                    int n_supplier_terminal_id = Convert.ToInt32(o_list_item.Value);

                    DataTable dt_supplier_terminal_period = (DataTable)ViewState["TableSupplierTerminalPeriod"];

                    DataRow dr_supplier_terminal_period = dt_supplier_terminal_period.Select(" ID =  " + n_supplier_terminal_id).FirstOrDefault();

                    if (dr_supplier_terminal_period != null)
                    {
                        int n_terminal_id = Convert.ToInt32(dr_supplier_terminal_period["TerminalID"]);

                        var o_cashbox_period = from cashbox_period in dt_cashbox_period.AsEnumerable()
                                               where
                                               cashbox_period.Field<int>("CompanyID") == (int)ViewState["CompanyID"]
                                               &&
                                               cashbox_period.Field<int>("TerminalID") == n_terminal_id
                                               &&
                                               (
                                                   (
                                                       cashbox_period.Field<DateTime?>("DateEnd") == null
                                                       &&
                                                       d_date_start >= cashbox_period.Field<DateTime>("DateStart")
                                                       &&
                                                       d_date_end >= cashbox_period.Field<DateTime>("DateStart")
                                                   )
                                                   ||
                                                   (
                                                       (
                                                           d_date_start >= cashbox_period.Field<DateTime>("DateStart")
                                                           &&
                                                           d_date_start <= cashbox_period.Field<DateTime>("DateEnd")
                                                       )
                                                       &&
                                                       (
                                                           d_date_end >= cashbox_period.Field<DateTime>("DateStart")
                                                           &&
                                                           d_date_end <= cashbox_period.Field<DateTime>("DateEnd")
                                                       )
                                                   )
                                               )
                                               select cashbox_period;

                        if (o_cashbox_period.Any() == true)
                        {
                            DataRow[] dr_cashbox_period = o_cashbox_period.ToArray<DataRow>();

                            if (dr_cashbox_period.Length == 1)
                            {
                                string s_value = String.Format("{0}-{1}", dr_cashbox_period[0]["ID"], n_supplier_terminal_id);

                                string s_text = String.Format("{0} &hArr; ( {1} {2} ) &rarr; ( {3} {4} ) &rarr; ( {5} {6} )", dr_cashbox_period[0]["TerminalNumber"], dr_cashbox_period[0]["NetworkNumber"], dr_cashbox_period[0]["NetworkName"], dr_cashbox_period[0]["BranchNumber"], dr_cashbox_period[0]["BranchName"], dr_cashbox_period[0]["CashBoxNumber"], dr_cashbox_period[0]["CashBoxName"]);
                                
                                lstCashBox.Items.Add(new ListItem(s_text, s_value));
                            }
                        }
                    }
                }
            }

            chkTerminal.Checked = b_checked;

            chkCashBox.Visible = lstCashBox.Items.Count > 0;
        }

        protected void chkCashBox_CheckedChanged(object sender, EventArgs e)
        {
            foreach (ListItem o_list_item in lstCashBox.Items)
            {
                o_list_item.Selected = chkCashBox.Checked;
            }
        }

        protected void lstCashBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool b_checked = true;

            foreach (ListItem o_list_item in lstCashBox.Items)
            {
                if (o_list_item.Selected == false)
                {
                    b_checked = false;
                    break;
                }
            }

            chkCashBox.Checked = b_checked;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (AgreementID <= 0) { return; }

            if (!Page.IsValid) { return; }

            if (lstSupplierGroup.Items.Count == 0)
            {
                lblError.Text = "Select 'Supplier Group'.";
                IsVisibleItemForm = true;
                return;
            }

            if (lstSupplier.Items.Count == 0)
            {
                lblError.Text = "Select 'Supplier'.";
                IsVisibleItemForm = true;
                return;
            }

            if (lstTerminal.Items.Count == 0)
            {
                lblError.Text = "Select 'Terminal'.";
                IsVisibleItemForm = true;
                return;
            }

            AgreementItemModel o_agreement_item = new AgreementItemModel();

            o_agreement_item.UserID = n_user_id;
            o_agreement_item.AgreementID = AgreementID;
            o_agreement_item.CompanyID = (int)ViewState["CompanyID"];
            o_agreement_item.CardID = Convert.ToInt32(ddlCard.SelectedValue);
            o_agreement_item.OperationTypeID = Convert.ToInt32(ddlOperationType.SelectedValue);

            o_agreement_item.PaymentsFrom = Convert.ToInt32(txtPaymentsFrom.Text.Trim());
            o_agreement_item.PaymentsTo = Convert.ToInt32(txtPaymentsTo.Text.Trim());

            o_agreement_item.CommissionLocal = Convert.ToDouble(txtCommissionLocal.Text.Trim());
            o_agreement_item.CommissionAbroad = Convert.ToDouble(txtCommissionAbroad.Text.Trim());

            o_agreement_item.DateStart = (DateTime)Common.Convert_To_Date(txtDateStart.Text);
            o_agreement_item.DateEnd = (DateTime)Common.Convert_To_Date(txtDateEnd.Text);

            DataTable dt_supplier = new DataTable();

            dt_supplier.Columns.Add("ID");

            foreach (ListItem o_list_item in lstSupplier.Items)
            {
                if (o_list_item.Selected == true)
                {
                    DataRow o_data_row = dt_supplier.NewRow();

                    o_data_row["ID"] = o_list_item.Value;

                    dt_supplier.Rows.Add(o_data_row);
                }
            }

            DataTable dt_terminal_temp = (DataTable)ViewState["TableTerminal"];

            DataTable dt_terminal = new DataTable();
            DataTable dt_supplier_terminal = new DataTable();

            dt_terminal.Columns.Add("ID");
            dt_supplier_terminal.Columns.Add("ID");

            foreach (ListItem o_list_item in lstTerminal.Items)
            {
                if (o_list_item.Selected == true)
                {
                    int n_supplier_terminal_id = Convert.ToInt32(o_list_item.Value);
                    int n_terminal_id = Convert.ToInt32(dt_terminal_temp.Select("SupplierTerminalID = " + n_supplier_terminal_id)[0]["TerminalID"]);

                    DataRow dr_supplier_terminal = dt_supplier_terminal.NewRow();
                    dr_supplier_terminal["ID"] = n_supplier_terminal_id;
                    dt_supplier_terminal.Rows.Add(dr_supplier_terminal);

                    DataRow dr_terminal = dt_terminal.NewRow();
                    dr_terminal["ID"] = n_terminal_id;
                    dt_terminal.Rows.Add(dr_terminal);
                }
            }

            AgreementItemAction.Check(ref o_agreement_item, dt_supplier, dt_terminal, dt_supplier_terminal);

            if (o_agreement_item.ErrorMessage != "")
            {
                lblError.Text = o_agreement_item.ErrorMessage;
                IsVisibleItemForm = true;
                return;
            }

            // TABLE dt_cashbox_period_supplier_terminal USED TO ADD A NEW AgreementItemTerminal TO CashBoxPeriodTerminal TABLE

            DataTable dt_cashbox_period_supplier_terminal = new DataTable();

            dt_cashbox_period_supplier_terminal.Columns.Add("F1");  // CashBoxPeriodID
            dt_cashbox_period_supplier_terminal.Columns.Add("F2");  // SupplierTerminalID

            foreach (ListItem o_list_item in lstCashBox.Items)
            {
                string[] arr_value = o_list_item.Value.Split('-');

                DataRow dr_cashbox_period_supplier_terminal = dt_cashbox_period_supplier_terminal.NewRow();

                dr_cashbox_period_supplier_terminal["F1"] = arr_value[0];   // CashBoxPeriodID
                dr_cashbox_period_supplier_terminal["F2"] = arr_value[1];   // SupplierTerminalID

                dt_cashbox_period_supplier_terminal.Rows.Add(dr_cashbox_period_supplier_terminal);
            }

            // ===

            AgreementItemAction.Update(ref o_agreement_item, dt_supplier_terminal, dt_cashbox_period_supplier_terminal);

            if (o_agreement_item.ErrorMessage != "")
            {
                lblError.Text = o_agreement_item.ErrorMessage;
                IsVisibleItemForm = true;
                return;
            }

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}&p={3}", "updated", Request.QueryString.Get("i"), Request.QueryString.Get("q"), "item"));
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
                IsVisibleItemForm = true;
                cvDateRange.ErrorMessage = s_error;
            }

            args.IsValid = (s_error == "");
        }

        protected void btnDeleteAgreementItem_Command(object sender, CommandEventArgs e)
        {
            string s_error = "";

            int n_id = Convert.ToInt32(e.CommandArgument);

            bool b_deleted = AgreementItemAction.Delete(n_id, n_user_id, AgreementID, ref s_error);

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

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}&p={3}", "deleted", Request.QueryString.Get("i"), Request.QueryString.Get("q"), "item"));
        }

        private void Bind_Card(DateTime d_date_start, DateTime d_date_end)
        {
            ddlCard.Items.Clear();

            DataTable dt_agreement_payment_card = (DataTable)ViewState["TableAgreementPaymentCard"];

            var o_agreement_payment_card = from agreement_payment_card in dt_agreement_payment_card.AsEnumerable()
                                           where
                                           (
                                               agreement_payment_card.Field<DateTime?>("DateEnd") == null
                                               &&
                                               d_date_start >= agreement_payment_card.Field<DateTime>("DateStart")
                                               &&
                                               d_date_end >= agreement_payment_card.Field<DateTime>("DateStart")
                                           )
                                           ||
                                           (
                                               (
                                                   d_date_start >= agreement_payment_card.Field<DateTime>("DateStart")
                                                   &&
                                                   d_date_start <= agreement_payment_card.Field<DateTime>("DateEnd")
                                               )
                                               &&
                                               (
                                                   d_date_end >= agreement_payment_card.Field<DateTime>("DateStart")
                                                   &&
                                                   d_date_end <= agreement_payment_card.Field<DateTime>("DateEnd")
                                               )
                                           )
                                           select agreement_payment_card;

            try
            {
                dt_agreement_payment_card = o_agreement_payment_card.CopyToDataTable();

                ddlCard.DataSource = dt_agreement_payment_card;
                ddlCard.DataValueField = "CardID";
                ddlCard.DataTextField = "CardName";
                ddlCard.DataBind();
            }
            catch (Exception ex)
            {
                ddlCard.DataSource = null;
                ddlCard.DataBind();
            }

            ddlCard.Items.Insert(0, new ListItem("", "0"));
        }

        private void Bind_Form(AgreementModel o_agreement)
        {
            ViewState["CompanyID"] = o_agreement.CompanyID;
            ViewState["CreditID"] = o_agreement.CreditID;

            lblCompanyName.Text = o_agreement.CompanyName;
            lblCreditName.Text = o_agreement.CreditName;
            
            string s_error = "";

            //List<SqlParameter> o_card_parameters = new List<SqlParameter>();
            //o_card_parameters.Add(new SqlParameter("@CreditID", o_agreement.CreditID));
            //DB.Bind_List_Control(ddlCard, "sprCardByCredit", ref s_error, null, o_card_parameters);

            List<SqlParameter> o_operation_type_parameters = new List<SqlParameter>();
            o_operation_type_parameters.Add(new SqlParameter("@IsAgreementPart", true));

            DB.Bind_List_Control(ddlOperationType, "sprOperationType", ref s_error, null, o_operation_type_parameters);

            DataTable dt_cashbox_period = new DataTable();
            DataTable dt_agreement_period = new DataTable();
            DataTable dt_agreement_payment_card = new DataTable();
            DataTable dt_agreement_item = new DataTable();
            DataTable dt_agreement_item_terminal = new DataTable();

            AgreementItemAction.Select(ref dt_cashbox_period, ref dt_agreement_period, ref dt_agreement_payment_card, ref dt_agreement_item, ref dt_agreement_item_terminal, ref s_error, n_user_id, null, AgreementID);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            ViewState["TableCashBoxPeriod"] = dt_cashbox_period;
            ViewState["TableAgreementPeriod"] = dt_agreement_period;
            ViewState["TableAgreementPaymentCard"] = dt_agreement_payment_card;
            ViewState["TableAgreementItem"] = dt_agreement_item;
            ViewState["TableAgreementItemTerminal"] = dt_agreement_item_terminal;

            repAgreementPeriod.DataSource = dt_agreement_period;
            repAgreementPeriod.DataBind();

            // COPY EXISTING ITEM FOR CREATE NEW

            int n_item_id = 0;

            if (Request.QueryString["item"] != null) { int.TryParse(Request.QueryString["item"], out n_item_id); }

            if (n_item_id == 0) { return; }

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

            txtDateStart.Text = String.Format("{0:dd/MM/yyyy}", o_agreement_item.DateStart);
            txtDateEnd.Text = String.Format("{0:dd/MM/yyyy}", o_agreement_item.DateEnd);

            Date_Changed(null, null);

            txtPaymentsFrom.Text = o_agreement_item.PaymentsFrom.ToString();
            txtPaymentsTo.Text = o_agreement_item.PaymentsTo.ToString();

            ddlCard.SelectedValue = o_agreement_item.CardID.ToString();
            ddlOperationType.SelectedValue = o_agreement_item.OperationTypeID.ToString();

            txtCommissionLocal.Text = o_agreement_item.CommissionLocal.ToString();
            txtCommissionAbroad.Text = o_agreement_item.CommissionAbroad.ToString();

            IsVisibleItemForm = true;
        }
    }
}
