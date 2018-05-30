using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public partial class DataFileForm : UserControl
    {
        public int DataFileID { get; set; }

        private int n_user_id = 0;

        private DateTime d_date_min = new DateTime(2000, 1, 1);

        protected void Page_Load(object sender, EventArgs e)
        {
            n_user_id = ((UserModel)Session["User"]).ID;

            if (Page.IsPostBack) { return; }

            if (Request.QueryString["result"] != null && Request.QueryString.Get("result") == "updated") { lblMessage.Text = "Item successfully updated."; }

            DataFileModel o_data_file = new DataFileModel();

            if (DataFileID > 0)
            {
                hedTitle.InnerHtml = "Data File #" + DataFileID;

                o_data_file.ID = DataFileID;
                o_data_file.UserID = n_user_id;

                DataFileAction.Select(ref o_data_file);

                if (o_data_file.ErrorMessage != "")
                {
                    lblError.Text = o_data_file.ErrorMessage;
                    return;
                }
            }
            else
            {
                hedTitle.InnerHtml = "New Data File";
            }

            Bind_Form(o_data_file);

            if (o_data_file.ErrorMessage != "")
            {
                lblError.Text = o_data_file.ErrorMessage;
                return;
            }

            if (lstDateMode.SelectedValue == "month")
            {
                ddlMonthFrom.Focus();
            }
            else if (lstDateMode.SelectedValue == "range")
            {
                ddlDayFrom.Focus();
            }
        }

        protected void ddlTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            Hide_CashBox();
            Hide_Currency();
            Hide_Credit();
            Hide_Card();
            Hide_Operation_Type();

            trStopIfCardIsEmpty.Visible = false;
            trStopIfTerminalIsZero.Visible = false;
            trStopIfSupplierIsZero.Visible = false;
            trStopIfTerminalNotExists.Visible = false;
            trStopIfSupplierNotExists.Visible = false;
            trStopIfAgreementNotExists.Visible = false;

            chkStopIfCardIsEmpty.Checked = true;
            chkStopIfTerminalIsZero.Checked = true;
            chkStopIfSupplierIsZero.Checked = true;
            chkStopIfTerminalNotExists.Checked = true;
            chkStopIfSupplierNotExists.Checked = true;
            chkStopIfAgreementNotExists.Checked = true;

            int n_template_id = Convert.ToInt32(ddlTemplate.SelectedValue);

            if (n_template_id == 0) { return; }

            DataTable dt_template = (DataTable)ViewState["TableTemplate"];
            DataTable dt_template_field = (DataTable)ViewState["TableTemplateField"];

            DataRow dr_template = dt_template.Select(" ID = " + n_template_id).First();

            int n_data_source_id = Convert.ToInt32(dr_template["DataSourceID"]);
            int n_credit_id = Convert.ToInt32(dr_template["CreditID"]);
            int n_discount_id = Convert.ToInt32(dr_template["DiscountID"]);

            string s_rows = dr_template["HeaderRowsCount"].ToString();
            string s_terminal_field = dr_template["TerminalField"].ToString();

            chkIgnoreCard.Visible = (n_data_source_id > 0);

            hidDataSourceID.Value = n_data_source_id.ToString();
            hidCreditID.Value = n_credit_id.ToString();
            hidDiscountID.Value = n_discount_id.ToString();

            txtHeaderRowsCount.Text = s_rows;

            divCard_Change.Visible = (dt_template_field.Select(" TemplateID = " + n_template_id + " AND FieldFromDB = 'CardBrand' ").FirstOrDefault() == null);

            if (s_terminal_field.Contains("CompanyNumber")) { divCompany_Change.Visible = false; }

            bool b_cashbox_visible = true;

            switch (s_terminal_field)
            {
                case "":
                    divNetwork_Change.Visible = true;
                    divBranch_Change.Visible = true;
                    divCashBox_Change.Visible = true;
                    break;
                // CompanyNumber
                case "CompanyNumber":
                    divNetworkNumber_Change.Visible = true;
                    divBranchNumber_Change.Visible = true;
                    divCashBoxNumber_Change.Visible = true;
                    break;
                case "CompanyNumber,NetworkNumber":
                    divBranchNumber_Change.Visible = true;
                    divCashBoxNumber_Change.Visible = true;
                    break;
                case "CompanyNumber,BranchNumber":
                    divNetworkNumber_Change.Visible = true;
                    divCashBoxNumber_Change.Visible = true;
                    break;
                case "CompanyNumber,CashBoxNumber":
                    divNetworkNumber_Change.Visible = true;
                    divBranchNumber_Change.Visible = true;
                    break;
                case "CompanyNumber,NetworkNumber,BranchNumber":
                    divCashBoxNumber_Change.Visible = true;
                    break;
                case "CompanyNumber,NetworkNumber,CashBoxNumber":
                    divBranchNumber_Change.Visible = true;
                    break;
                case "CompanyNumber,BranchNumber,CashBoxNumber":
                    divNetworkNumber_Change.Visible = true;
                    break;
                // NetworkNumber
                case "NetworkNumber":
                    divBranchNumber_Change.Visible = true;
                    divCashBoxNumber_Change.Visible = true;
                    break;
                case "NetworkNumber,BranchNumber":
                    divCashBoxNumber_Change.Visible = true;
                    break;
                case "NetworkNumber,CashBoxNumber":
                    divBranchNumber_Change.Visible = true;
                    break;
                case "NetworkNumber,BranchNumber,CashBoxNumber":
                    //
                    break;
                // BranchNumber
                case "BranchNumber":
                    divNetwork_Change.Visible = true;
                    divCashBoxNumber_Change.Visible = true;
                    break;
                case "BranchNumber,CashBoxNumber":
                    divNetwork_Change.Visible = true;
                    break;
                // CashBoxNumber
                case "CashBoxNumber":
                    divNetwork_Change.Visible = true;
                    divBranch_Change.Visible = true;
                    break;
                // Visible = false:
                case "CompanyNumber,NetworkNumber,BranchNumber,CashBoxNumber":
                case "TerminalNumber,SupplierNumber":
                case "TerminalNumber":
                case "SupplierNumber":
                    b_cashbox_visible = false;
                    break;
            }

            divCashBox.Visible = b_cashbox_visible;

            if (dt_template_field.Select(" TemplateID = " + n_template_id + " AND FieldFromDB = 'CardBrand' ").FirstOrDefault() != null)
            {
                trStopIfCardIsEmpty.Visible = true;
            }

            if (n_credit_id > 0 || n_discount_id > 0)
            {
                if (s_terminal_field.Contains("TerminalNumber"))
                {
                    trStopIfTerminalIsZero.Visible = true;
                    trStopIfTerminalNotExists.Visible = true;
                }

                if (s_terminal_field.Contains("SupplierNumber"))
                {
                    trStopIfSupplierIsZero.Visible = true;
                    trStopIfSupplierNotExists.Visible = true;
                }
            }

            trStopIfAgreementNotExists.Visible = true;
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n_company_id = Convert.ToInt32(ddlCompany.SelectedValue);

            while (ddlCashBox.Items.Count > 1)
            {
                ddlCashBox.Items.Remove(ddlCashBox.Items[1]);
            }

            while (ddlBranch.Items.Count > 1)
            {
                ddlBranch.Items.Remove(ddlBranch.Items[1]);
            }

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

        protected void ddlNetwork_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n_network_id = Convert.ToInt32(ddlNetwork.SelectedValue);

            while (ddlCashBox.Items.Count > 1)
            {
                ddlCashBox.Items.Remove(ddlCashBox.Items[1]);
            }

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

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n_branch_id = Convert.ToInt32(ddlBranch.SelectedValue);

            while (ddlCashBox.Items.Count > 1)
            {
                ddlCashBox.Items.Remove(ddlCashBox.Items[1]);
            }

            if (n_branch_id > 0)
            {
                string s_error = "";

                List<SqlParameter> o_cashbox_parameters = new List<SqlParameter>();
                o_cashbox_parameters.Add(new SqlParameter("@UserID", n_user_id));
                o_cashbox_parameters.Add(new SqlParameter("@BranchID", n_branch_id));

                DB.Bind_List_Control(ddlCashBox, "sprCashBox", ref s_error, null, o_cashbox_parameters);
            }
        }

        protected void chkStopIfAgreementNotExists_CheckedChanged(object sender, EventArgs e)
        {
            string s_error = "";

            DataTable dt_credit = new DataTable();
            DataTable dt_card = new DataTable();

            if (chkStopIfAgreementNotExists.Checked == true)
            {
                DB.Bind_Data_Table("sprUserCredit", ref dt_credit, ref s_error, "@UserID", n_user_id.ToString());
                DB.Bind_Data_Table("sprUserCard", ref dt_card, ref s_error, "@UserID", n_user_id.ToString());
            }
            else
            {
                DB.Bind_Data_Table("sprCredit", ref dt_credit, ref s_error);
                DB.Bind_Data_Table("sprCard", ref dt_card, ref s_error);
            }

            ViewState["TableCredit"] = dt_credit;
            ViewState["TableCard"] = dt_card;

            foreach (RepeaterItem o_item in repCredit.Items)
            {
                if (o_item.ItemType == ListItemType.Item || o_item.ItemType == ListItemType.AlternatingItem)
                {
                    DropDownList ddl_credit = (DropDownList)o_item.FindControl("ddlCredit");

                    ddl_credit.DataSource = dt_credit;
                    ddl_credit.DataValueField = "ID";
                    ddl_credit.DataTextField = "CreditName";
                    ddl_credit.DataBind();

                    ddl_credit.Items.Insert(0, new ListItem("", "0"));
                }
            }

            foreach (RepeaterItem o_item in repCard.Items)
            {
                if (o_item.ItemType == ListItemType.Item || o_item.ItemType == ListItemType.AlternatingItem)
                {
                    DropDownList ddl_card = (DropDownList)o_item.FindControl("ddlCard");

                    ddl_card.DataSource = dt_card;
                    ddl_card.DataValueField = "ID";
                    ddl_card.DataTextField = "CardName";
                    ddl_card.DataBind();

                    ddl_card.Items.Insert(0, new ListItem("", "0"));
                }
            }
        }

        protected void cvDateMode_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool b_valid = true;

            switch (lstDateMode.SelectedValue)
            {
                case "month":
                    int n_month = 0, n_year = 0;

                    int.TryParse(ddlMonthFrom.SelectedValue, out n_month);
                    int.TryParse(ddlYearFrom.SelectedValue, out n_year);

                    if (n_month == 0 || n_year == 0)
                    {
                        b_valid = false;
                        cvDateMode.ErrorMessage = "Select 'Month' and 'Year'.";
                    }

                    break;
                case "range":
                    int n_day_from = 0, n_month_from = 0, n_year_from = 0, n_day_to = 0, n_month_to = 0, n_year_to = 0;

                    int.TryParse(ddlDayFrom.SelectedValue, out n_day_from);
                    int.TryParse(ddlMonthFrom.SelectedValue, out n_month_from);
                    int.TryParse(ddlYearFrom.SelectedValue, out n_year_from);
                    int.TryParse(ddlDayTo.SelectedValue, out n_day_to);
                    int.TryParse(ddlMonthTo.SelectedValue, out n_month_to);
                    int.TryParse(ddlYearTo.SelectedValue, out n_year_to);

                    if (n_day_from == 0 || n_month_from == 0 || n_year_from == 0 || n_day_to == 0 || n_month_to == 0 || n_year_to == 0)
                    {
                        b_valid = false;
                        cvDateMode.ErrorMessage = "Select 'Date From' and 'Date To'.";
                    }
                    else
                    {
                        if (n_day_from > DateTime.DaysInMonth(n_year_from, n_month_from))
                        {
                            b_valid = false;
                            ddlDayFrom.Focus();
                            cvDateMode.ErrorMessage = "'Date From' not valid.";
                        }
                        else if (n_day_to > DateTime.DaysInMonth(n_year_to, n_month_to))
                        {
                            b_valid = false;
                            ddlDayTo.Focus();
                            cvDateMode.ErrorMessage = "'Date To' not valid.";
                        }
                        else
                        {
                            DateTime d_date_from = new DateTime(n_year_from, n_month_from, n_day_from);
                            DateTime d_date_to = new DateTime(n_year_to, n_month_to, n_day_to);

                            if (d_date_from > d_date_to)
                            {
                                b_valid = false;
                                cvDateMode.ErrorMessage = "'Date From' can't be greater than 'Date To'.";
                            }
                        }
                    }

                    break;
            }

            args.IsValid = b_valid;
        }

        protected void repCurrency_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataTable dt_currency = (DataTable)ViewState["TableCurrency"];

                DropDownList ddl_currency = (DropDownList)e.Item.FindControl("ddlCurrency");

                ddl_currency.DataSource = dt_currency;
                ddl_currency.DataValueField = "ID";
                ddl_currency.DataTextField = "CurrencyCode";
                ddl_currency.DataBind();

                ddl_currency.Items.Insert(0, new ListItem("", "0"));
            }
        }

        protected void repCredit_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataTable dt_credit = (DataTable)ViewState["TableCredit"];

                DropDownList ddl_credit = (DropDownList)e.Item.FindControl("ddlCredit");

                ddl_credit.DataSource = dt_credit;
                ddl_credit.DataValueField = "ID";
                ddl_credit.DataTextField = "CreditName";
                ddl_credit.DataBind();

                ddl_credit.Items.Insert(0, new ListItem("", "0"));
            }
        }

        protected void repCard_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataTable dt_card = (DataTable)ViewState["TableCard"];

                DropDownList ddl_card = (DropDownList)e.Item.FindControl("ddlCard");

                ddl_card.DataSource = dt_card;
                ddl_card.DataValueField = "ID";
                ddl_card.DataTextField = "CardName";
                ddl_card.DataBind();

                ddl_card.Items.Insert(0, new ListItem("", "0"));
            }
        }

        protected void repOperationType_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataTable dt_operation_type = (DataTable)ViewState["TableOperationType"];

                DropDownList ddl_operation_type = (DropDownList)e.Item.FindControl("ddlOperationType");

                ddl_operation_type.DataSource = dt_operation_type;
                ddl_operation_type.DataValueField = "ID";
                ddl_operation_type.DataTextField = "OperationTypeName";
                ddl_operation_type.DataBind();

                ddl_operation_type.Items.Insert(0, new ListItem("", "0"));
            }
        }

        protected void repAgreementFromCashBox_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView o_data_row = (DataRowView)e.Item.DataItem;

                string s_company = o_data_row["CompanyName"].ToString().Trim();
                string s_network = o_data_row["NetworkName"].ToString().Trim();
                string s_branch = o_data_row["BranchName"].ToString().Trim();
                string s_cashbox = o_data_row["CashBoxName"].ToString().Trim();

                string s_card = o_data_row["CardBrand"].ToString().Trim();
                string s_date = String.Format("{0:dd/MM/yyyy} ({1})", o_data_row["TransactionDate_Converted"], o_data_row["TransactionDate"].ToString().Trim());

                string s_payments = "";

                if (o_data_row.DataView.ToTable().Columns.Contains("PaymentsCount"))
                {
                    s_payments = o_data_row["PaymentsCount"].ToString().Trim();
                }

                if (s_company == "") { s_company = o_data_row["CompanyNumber"].ToString().Trim(); }
                if (s_network == "") { s_network = o_data_row["NetworkNumber"].ToString().Trim(); }
                if (s_branch == "") { s_branch = o_data_row["BranchNumber"].ToString().Trim(); }
                if (s_cashbox == "") { s_cashbox = o_data_row["CashBoxNumber"].ToString().Trim(); }

                Label lblCompany = (Label)e.Item.FindControl("lblCompany");
                Label lblNetwork = (Label)e.Item.FindControl("lblNetwork");
                Label lblBranch = (Label)e.Item.FindControl("lblBranch");
                Label lblCashBox = (Label)e.Item.FindControl("lblCashBox");

                Label lblCard = (Label)e.Item.FindControl("lblCard");
                Label lblDate = (Label)e.Item.FindControl("lblDate");
                Label lblPayments = (Label)e.Item.FindControl("lblPayments");

                Label lblGroup = (Label)e.Item.FindControl("lblGroup");
                Label lblSupplier = (Label)e.Item.FindControl("lblSupplier");
                Label lblTerminal = (Label)e.Item.FindControl("lblTerminal");
                Label lblAgreement = (Label)e.Item.FindControl("lblAgreement");

                lblCompany.Text = s_company;
                lblNetwork.Text = s_network;
                lblBranch.Text = s_branch;
                lblCashBox.Text = s_cashbox;
                lblCard.Text = s_card;
                lblDate.Text = s_date;
                lblPayments.Text = s_payments;
            }
        }

        protected void repAgreementFromTerminal_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView o_data_row = (DataRowView)e.Item.DataItem;

                string s_card = o_data_row["CardBrand"].ToString().Trim();
                string s_date = String.Format("{0:dd/MM/yyyy} ({1})", o_data_row["TransactionDate_Converted"], o_data_row["TransactionDate"].ToString().Trim());

                string s_terminal = "", s_supplier = "", s_credit = "", s_operation = "", s_payments = "";

                if (o_data_row.DataView.ToTable().Columns.Contains("TerminalNumber"))
                {
                    s_terminal = o_data_row["TerminalNumber"].ToString().Trim();
                }

                if (o_data_row.DataView.ToTable().Columns.Contains("SupplierNumber"))
                {
                    s_supplier = o_data_row["SupplierNumber"].ToString().Trim();
                }

                if (o_data_row.DataView.ToTable().Columns.Contains("CreditBrand"))
                {
                    s_credit = o_data_row["CreditBrand"].ToString().Trim();
                }

                if (o_data_row.DataView.ToTable().Columns.Contains("OperationType"))
                {
                    s_operation = o_data_row["OperationType"].ToString().Trim();
                }

                if (o_data_row.DataView.ToTable().Columns.Contains("PaymentsCount"))
                {
                    s_payments = o_data_row["PaymentsCount"].ToString().Trim();
                }

                Label lblTerminal = (Label)e.Item.FindControl("lblTerminal");
                Label lblSupplier = (Label)e.Item.FindControl("lblSupplier");
                Label lblCard = (Label)e.Item.FindControl("lblCard");
                Label lblCredit = (Label)e.Item.FindControl("lblCredit");
                Label lblOperation = (Label)e.Item.FindControl("lblOperation");
                Label lblDate = (Label)e.Item.FindControl("lblDate");
                Label lblPayments = (Label)e.Item.FindControl("lblPayments");

                lblTerminal.Text = s_terminal;
                lblSupplier.Text = s_supplier;
                lblCard.Text = s_card;
                lblCredit.Text = s_credit;
                lblOperation.Text = s_operation;
                lblDate.Text = s_date;
                lblPayments.Text = s_payments;
            }
        }

        protected void Download_Excel(object sender, EventArgs e)
        {
            string s_error = "", s_procedure = "", s_sheet = "";

            string s_button_id = ((LinkButton)sender).ID;

            if (s_button_id == "btnDownloadOriginalData")
            {
                s_procedure = "sprDataFileSelectOriginal";
                s_sheet = "Original";
            }
            else if (s_button_id == "btnDownloadReceivedData")
            {
                s_procedure = "sprDataFileSelectReceived";
                s_sheet = "Received";
            }
            else
            {
                return;
            }

            DataTable o_data_table = new DataTable();

            SqlCommand o_command = new SqlCommand(s_procedure, DB.Get_Connection()) { CommandType = CommandType.StoredProcedure, CommandTimeout = 0 };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            o_command.Parameters.Add(new SqlParameter("@DataFileID", SqlDbType.Int) { Value = DataFileID });

            DB.Bind_Data_Table(o_command, ref o_data_table, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            using (XLWorkbook xl_workbook = new XLWorkbook())
            {
                xl_workbook.Worksheets.Add(o_data_table, s_sheet);

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=DataFile_" + DataFileID + "_" + s_sheet + ".xlsx");

                using (MemoryStream o_memory_stream = new MemoryStream())
                {
                    xl_workbook.SaveAs(o_memory_stream);

                    o_memory_stream.WriteTo(Response.OutputStream);

                    Response.Flush();
                    Response.End();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) { return; }

            string s_error = "";
            string s_result = "";

            int n_template_id = Convert.ToInt32(ddlTemplate.SelectedValue);

            if (repCurrency.Items.Count > 0)
            {
                // Check Template Currency

                Check_Currency(n_template_id, ref s_error);

                if (s_error != "")
                {
                    lblError.Text = s_error;
                    return;
                }
            }

            if (repCredit.Items.Count > 0)
            {
                // Check Template Credit

                Check_Credit(n_template_id, ref s_error);

                if (s_error != "")
                {
                    lblError.Text = s_error;
                    return;
                }
            }

            if (repCard.Items.Count > 0)
            {
                // Check Template Card

                Check_Card(n_template_id, ref s_error);

                if (s_error != "")
                {
                    lblError.Text = s_error;
                    return;
                }
            }

            if (repOperationType.Items.Count > 0)
            {
                // Check Template OperationType

                Check_Operation_Type(n_template_id, ref s_error);

                if (s_error != "")
                {
                    lblError.Text = s_error;
                    return;
                }
            }

            int n_company_id = 0, n_network_id = 0, n_branch_id = 0, n_cashbox_id = 0, n_card_id = 0;

            string s_network_number = "", s_branch_number = "", s_cashbox_number = "";

            if (divCompany_Change.Visible == true) { n_company_id = Convert.ToInt32(ddlCompany.SelectedValue); }

            if (divCashBox.Visible == true)
            {
                if (divNetwork_Change.Visible == true) { n_network_id = Convert.ToInt32(ddlNetwork.SelectedValue); }
                if (divBranch_Change.Visible == true) { n_branch_id = Convert.ToInt32(ddlBranch.SelectedValue); }
                if (divCashBox_Change.Visible == true) { n_cashbox_id = Convert.ToInt32(ddlCashBox.SelectedValue); }

                if (divNetworkNumber_Change.Visible == true) { s_network_number = txtNetworkNumber.Text.Trim(); }
                if (divBranchNumber_Change.Visible == true) { s_branch_number = txtBranchNumber.Text.Trim(); }
                if (divCashBoxNumber_Change.Visible == true) { s_cashbox_number = txtCashBoxNumber.Text.Trim(); }
            }

            if (divCard_Change.Visible == true) { n_card_id = Convert.ToInt32(ddlCard.SelectedValue); }

            DataFileModel o_data_file = new DataFileModel();

            o_data_file.ID = DataFileID;
            o_data_file.UserID = n_user_id;
            o_data_file.TemplateID = n_template_id;
            o_data_file.CompanyID = n_company_id;
            o_data_file.NetworkID = n_network_id;
            o_data_file.BranchID = n_branch_id;
            o_data_file.CashBoxID = n_cashbox_id;
            o_data_file.CardID = n_card_id;

            o_data_file.NetworkNumber = s_network_number;
            o_data_file.BranchNumber = s_branch_number;
            o_data_file.CashBoxNumber = s_cashbox_number;

            o_data_file.StopIfCardIsEmpty = chkStopIfCardIsEmpty.Checked;
            o_data_file.StopIfTerminalIsZero = chkStopIfTerminalIsZero.Checked;
            o_data_file.StopIfSupplierIsZero = chkStopIfSupplierIsZero.Checked;
            o_data_file.StopIfTerminalNotExists = chkStopIfTerminalNotExists.Checked;
            o_data_file.StopIfSupplierNotExists = chkStopIfSupplierNotExists.Checked;
            o_data_file.StopIfAgreementNotExists = chkStopIfAgreementNotExists.Checked;

            o_data_file.BranchNumber = s_branch_number;
            o_data_file.CashBoxNumber = s_cashbox_number;

            int n_day_from = 0, n_month_from = 0, n_year_from = 0, n_day_to = 0, n_month_to = 0, n_year_to = 0;

            switch (lstDateMode.SelectedValue)
            {
                case "month":
                    n_day_from = 1;
                    n_month_from = Convert.ToInt32(ddlMonthFrom.SelectedValue);
                    n_year_from = Convert.ToInt32(ddlYearFrom.SelectedValue);

                    n_day_to = DateTime.DaysInMonth(n_year_from, n_month_from);
                    n_month_to = n_month_from;
                    n_year_to = n_year_from;

                    break;
                case "range":
                    n_day_from = Convert.ToInt32(ddlDayFrom.SelectedValue);
                    n_month_from = Convert.ToInt32(ddlMonthFrom.SelectedValue);
                    n_year_from = Convert.ToInt32(ddlYearFrom.SelectedValue);

                    n_day_to = Convert.ToInt32(ddlDayTo.SelectedValue);
                    n_month_to = Convert.ToInt32(ddlMonthTo.SelectedValue);
                    n_year_to = Convert.ToInt32(ddlYearTo.SelectedValue);

                    break;
                case "auto":
                    // DateFrom & DateTo will be set after upload and validate excel file
                    break;
            }

            if (n_day_from > 0 && n_month_from > 0 && n_year_from > 0 && n_day_to > 0 && n_month_to > 0 && n_year_to > 0)
            {
                o_data_file.DateFrom = new DateTime(n_year_from, n_month_from, n_day_from);
                o_data_file.DateTo = new DateTime(n_year_to, n_month_to, n_day_to);
            }

            // Bind Data

            DateTime d_execution_start = DateTime.Now;

            Bind_Data(ref o_data_file, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            DateTime d_execution_end = DateTime.Now;

            o_data_file.ExecutionDuration = d_execution_end - d_execution_start;

            // Update

            DataFileAction.Update(ref o_data_file);

            if (o_data_file.ErrorMessage != "")
            {
                lblError.Text = o_data_file.ErrorMessage;
                return;
            }

            // ===

            string s_guid = "";

            if (DataFileID == 0)
            {
                s_result = "inserted";
                s_guid = Guid.NewGuid().ToString();

                ((Dictionary<string, int>)Session["DataFileLookupTable"]).Add(s_guid, o_data_file.ID);
            }
            else
            {
                s_result = "updated";
                s_guid = Request.QueryString.Get("q");
            }

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}", s_result, Request.QueryString.Get("i"), s_guid));
        }

        private void Check_Currency(int n_template_id, ref string s_error)
        {
            DataTable dt_template_currency = new DataTable();

            dt_template_currency.Columns.Add("CurrencyID");
            dt_template_currency.Columns.Add("CurrencyFromUser");

            foreach (RepeaterItem o_item in repCurrency.Items)
            {
                DropDownList ddl_currency = (DropDownList)o_item.FindControl("ddlCurrency");
                HiddenField hid_currency_from_user = (HiddenField)o_item.FindControl("hidCurrencyFromUser");

                int n_currency_id = Convert.ToInt32(ddl_currency.SelectedValue);
                string s_currency_from_user = hid_currency_from_user.Value.Trim();

                if (n_currency_id > 0 && s_currency_from_user != "")
                {
                    DataRow o_data_row = dt_template_currency.NewRow();

                    o_data_row["CurrencyID"] = n_currency_id.ToString();
                    o_data_row["CurrencyFromUser"] = s_currency_from_user;

                    dt_template_currency.Rows.Add(o_data_row);
                }
            }

            if (dt_template_currency.Rows.Count > 0)
            {
                TemplateAction.Update_Currency(n_user_id, n_template_id, dt_template_currency, ref s_error);
            }
        }

        private void Check_Credit(int n_template_id, ref string s_error)
        {
            DataTable dt_template_credit = new DataTable();

            dt_template_credit.Columns.Add("CreditID");
            dt_template_credit.Columns.Add("CreditFromUser");

            foreach (RepeaterItem o_item in repCredit.Items)
            {
                DropDownList ddl_credit = (DropDownList)o_item.FindControl("ddlCredit");
                HiddenField hid_credit_from_user = (HiddenField)o_item.FindControl("hidCreditFromUser");

                int n_credit_id = Convert.ToInt32(ddl_credit.SelectedValue);
                string s_credit_from_user = hid_credit_from_user.Value.Trim();

                if (n_credit_id > 0 && s_credit_from_user != "")
                {
                    DataRow o_data_row = dt_template_credit.NewRow();

                    o_data_row["CreditID"] = n_credit_id.ToString();
                    o_data_row["CreditFromUser"] = s_credit_from_user;

                    dt_template_credit.Rows.Add(o_data_row);
                }
            }

            if (dt_template_credit.Rows.Count > 0)
            {
                TemplateAction.Update_Credit(n_user_id, n_template_id, dt_template_credit, ref s_error);
            }
        }

        private void Check_Card(int n_template_id, ref string s_error)
        {
            DataTable dt_template_card = new DataTable();

            dt_template_card.Columns.Add("CardID");
            dt_template_card.Columns.Add("CardFromUser");

            foreach (RepeaterItem o_item in repCard.Items)
            {
                DropDownList ddl_card = (DropDownList)o_item.FindControl("ddlCard");
                HiddenField hid_card_from_user = (HiddenField)o_item.FindControl("hidCardFromUser");

                int n_card_id = Convert.ToInt32(ddl_card.SelectedValue);
                string s_card_from_user = hid_card_from_user.Value.Trim();

                if (n_card_id > 0 && s_card_from_user != "")
                {
                    DataRow o_data_row = dt_template_card.NewRow();

                    o_data_row["CardID"] = n_card_id.ToString();
                    o_data_row["CardFromUser"] = s_card_from_user;

                    dt_template_card.Rows.Add(o_data_row);
                }
            }

            if (dt_template_card.Rows.Count > 0)
            {
                TemplateAction.Update_Card(n_user_id, n_template_id, dt_template_card, ref s_error);
            }
        }

        private void Check_Operation_Type(int n_template_id, ref string s_error)
        {
            DataTable dt_template_operation_type = new DataTable();

            dt_template_operation_type.Columns.Add("OperationTypeID");
            dt_template_operation_type.Columns.Add("OperationTypeFromUser");

            foreach (RepeaterItem o_item in repOperationType.Items)
            {
                DropDownList ddl_operation_type = (DropDownList)o_item.FindControl("ddlOperationType");
                HiddenField hid_operation_type_from_user = (HiddenField)o_item.FindControl("hidOperationTypeFromUser");

                int n_operation_type_id = Convert.ToInt32(ddl_operation_type.SelectedValue);
                string s_operation_type_from_user = hid_operation_type_from_user.Value.Trim();

                if (n_operation_type_id > 0 && s_operation_type_from_user != "")
                {
                    DataRow o_data_row = dt_template_operation_type.NewRow();

                    o_data_row["OperationTypeID"] = n_operation_type_id.ToString();
                    o_data_row["OperationTypeFromUser"] = s_operation_type_from_user;

                    dt_template_operation_type.Rows.Add(o_data_row);
                }
            }

            if (dt_template_operation_type.Rows.Count > 0)
            {
                TemplateAction.Update_Operation_Type(n_user_id, n_template_id, dt_template_operation_type, ref s_error);
            }
        }

        private void Bind_Data(ref DataFileModel o_data_file, ref string s_error)
        {
            o_data_file.FileName = Path.GetFileName(fuUploadExcel.PostedFile.FileName);

            string s_file_name = ExcelAction.Save(fuUploadExcel, ref s_error);

            if (s_error != "") { return; }

            string s_path = ExcelAction.ExcelFolder + "\\" + s_file_name;

            // Bind Data

            DataTable dt_excel = new DataTable();

            //ExcelAction.Bind_Data_Table(s_path, ref dt_excel, ref s_error, "", "NO");
            ExcelAction.Bind_Data_Table_New(s_path, ref dt_excel, ref s_error, "", "NO");

            if (s_error != "") { goto Finish; }

            // CHECK Empty Rows

            DataTable dt_excel_without_empty = dt_excel.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is DBNull || string.IsNullOrWhiteSpace(field as string))).CopyToDataTable();

            if (chkRemoveEmptyRows.Checked == true)
            {
                dt_excel = dt_excel_without_empty;
            }
            else if (dt_excel.Rows.Count != dt_excel_without_empty.Rows.Count)
            {
                s_error = "Excel file contains empty rows.";
                goto Finish;
            }

            // VALIDATE Template Headers / Remove Empty Columns From dt_excel / Remove Header Rows From dt_excel / Set dt_excel Columns Name

            int n_header_rows_count = Convert.ToInt32(txtHeaderRowsCount.Text.Trim());

            DataTable dt_excel_headers = dt_excel.AsEnumerable().Take(n_header_rows_count).CopyToDataTable();

            string s_validate = "";

            Validate_Template(dt_excel_headers, ref dt_excel, ref s_validate, ref s_error);

            if (s_error != "") { goto Finish; }

            if (s_validate != "OK")
            {
                s_error = s_validate;
                goto Finish;
            }

            if (dt_excel.Rows.Count == 0)
            {
                s_error = "Excel file is empty.";
                goto Finish;
            }

            ////CHECK Duplicate Row / s In dt_excel

            //List<string> lst_columns = new List<string>();

            //foreach (DataColumn o_column in dt_excel.Columns)
            //{
            //    lst_columns.Add(o_column.ColumnName);
            //}

            // new
            DataTable dt_excel_without_duplicate = dt_excel.AsEnumerable().GroupBy(row => row).Where(group => group.Count() == 1).Select(g => g.Key).CopyToDataTable();
            // old
            //DataTable dt_excel_without_duplicate = new DataView(dt_excel).ToTable(true, lst_columns.ToArray());

            if (chkRemoveDuplicateRows.Checked == true)
            {
                dt_excel = dt_excel_without_duplicate;
            }
            else if (dt_excel.Rows.Count != dt_excel_without_duplicate.Rows.Count)
            {
                s_error = "Excel file contains duplicate rows.";
                goto Finish;
            }

            // Validate Data

            Validate_Data(dt_excel, ref o_data_file, ref s_error);

            if (s_error != "") { goto Finish; }

        Finish:

            ExcelAction.Remove(s_path, ref s_error);
        }

        private void Validate_Template(DataTable dt_excel_headers, ref DataTable dt_excel, ref string s_validate, ref string s_error)
        {
            DataTable dt_template_field = new DataTable();

            dt_template_field.Columns.Add("RowData");   // RowData = FieldFromExcel

            List<int> lst_empty_columns = new List<int>();

            for (int i_col = 0; i_col < dt_excel_headers.Columns.Count; i_col++)
            {
                string s_field_name = "";

                for (int i_row = 0; i_row < dt_excel_headers.Rows.Count; i_row++)
                {
                    s_field_name += " " + dt_excel_headers.Rows[i_row][i_col].ToString().Trim();
                }

                s_field_name = ExcelAction.Valid_Field_Name(s_field_name);

                if (s_field_name != "")
                {
                    DataRow o_data_row = dt_template_field.NewRow();

                    o_data_row["RowData"] = s_field_name;

                    dt_template_field.Rows.Add(o_data_row);
                }
                else
                {
                    lst_empty_columns.Add(i_col);
                }
            }

            int n_template_id = 0;
            int.TryParse(ddlTemplate.SelectedValue, out n_template_id);

            SqlCommand o_command = new SqlCommand("sprDataFileValidateTemplate", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure };

            o_command.Parameters.Add(new SqlParameter("@TemplateID", SqlDbType.Int) { Value = n_template_id });

            o_command.Parameters.AddWithValue("@TableTemplateField", dt_template_field);
            o_command.Parameters["@TableTemplateField"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();

                s_validate = o_command.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            if (s_validate != "OK")
            {
                s_error = s_validate;
                return;
            }

            // Remove Empty Columns From dt_excel

            lst_empty_columns = lst_empty_columns.OrderByDescending(i => i).ToList();

            foreach (int i in lst_empty_columns)
            {
                dt_excel.Columns.RemoveAt(i);
            }

            // Remove Header Rows From dt_excel

            for (int i = 0; i < dt_excel_headers.Rows.Count; i++)
            {
                dt_excel.Rows.Remove(dt_excel.Rows[0]);
            }

            // Set dt_excel Columns Name

            for (int i = 0; i < dt_template_field.Rows.Count; i++)
            {
                dt_excel.Columns[i].ColumnName = String.Format("[{0}]", dt_template_field.Rows[i]["RowData"]);
            }
        }

        private void Validate_Data(DataTable dt_excel, ref DataFileModel o_data_file, ref string s_error)
        {
            // Get Template

            TemplateModel o_template = new TemplateModel();

            o_template.ID = o_data_file.TemplateID;
            o_template.UserID = n_user_id;

            TemplateAction.Select(ref o_template);

            if (o_template.ErrorMessage != "")
            {
                s_error = o_template.ErrorMessage;
                return;
            }

            bool b_inside = (o_template.DataSourceID > 0);
            bool b_outside = (o_template.CreditID > 0 || o_template.DiscountID > 0);

            if (b_inside == false && b_outside == false)
            {
                s_error = "Error on select template in data file.";
                return;
            }

            // Get Dictionary === ( dic_field_db_excel - Key : FieldFromDB / Value : FieldFromExcel ) === ( dic_field_format - Key : FieldFromDB / Value : FieldFormat )

            Dictionary<string, string> dic_field_db_excel = new Dictionary<string, string>();
            Dictionary<string, string> dic_field_format = new Dictionary<string, string>();
            Dictionary<string, string> dic_field_exclude = new Dictionary<string, string>();

            foreach (DataRow o_data_row in o_template.TableTemplateField.Rows)
            {
                // FieldFromExcel / FieldFromDB / FieldExclude / FieldFormat / FieldType / FieldDescription

                string s_field_from_db = o_data_row["FieldFromDB"].ToString();
                string s_field_from_excel = String.Format("[{0}]", o_data_row["FieldFromExcel"].ToString());
                string s_field_exclude = o_data_row["FieldExclude"].ToString();
                string s_field_format = o_data_row["FieldFormat"].ToString();
                string s_field_type = o_data_row["FieldType"].ToString();
                string s_field_description = o_data_row["FieldDescription"].ToString();

                if (s_field_from_db != "")
                {
                    dic_field_db_excel.Add(s_field_from_db, s_field_from_excel);
                    dic_field_format.Add(s_field_from_db, s_field_format);
                }

                if (s_field_exclude != "")
                {
                    dic_field_exclude.Add(s_field_from_excel, s_field_exclude);
                }
            }

            // GET ONLY NOT EXCLUDED ROWS INTO dt_excel_without_excluded_rows

            DataTable dt_excel_without_excluded_rows = dt_excel.Copy();

            if (dic_field_exclude.Count > 0)
            {
                string s_query = "";

                int i_column = 0;

                foreach (KeyValuePair<string, string> o_key_value in dic_field_exclude)
                {
                    string s_field = o_key_value.Key;
                    string[] arr_exclude = o_key_value.Value.Split(',');

                    i_column++;

                    string s_column = "F" + i_column;

                    dt_excel_without_excluded_rows.Columns[s_field].ColumnName = s_column;

                    for (int i = 0; i < arr_exclude.Length; i++)
                    {
                        string s_value = arr_exclude[i].Trim();

                        string s_query_part = String.Format(" {0} <> '{1}' ", s_column, s_value);

                        if (s_query.Contains(s_query_part) == false)
                        {
                            if (s_query != "") { s_query += " AND "; }

                            s_query += s_query_part;
                        }
                    }
                }

                DataRow[] dr_excel_without_excluded_rows = dt_excel_without_excluded_rows.Select(s_query);

                if (dr_excel_without_excluded_rows.Length > 0)
                {
                    dt_excel_without_excluded_rows = dr_excel_without_excluded_rows.CopyToDataTable();
                }

                i_column = 0;

                foreach (KeyValuePair<string, string> o_key_value in dic_field_exclude)
                {
                    string s_field = o_key_value.Key;

                    i_column++;

                    string s_column = "F" + i_column;

                    dt_excel_without_excluded_rows.Columns[s_column].ColumnName = s_field;
                }
            }

            // VALIDATE DateFrom & DateTo ===

            Validate_Date_Range(dt_excel_without_excluded_rows, o_template, b_inside, ref o_data_file, ref s_error);

            if (s_error != "") { return; }

            // VALIDATE Currency, Credit, Card / Operation Type Conversion Tables. The s_error variable is common for Currency, Credit, Card / Operation Type validations.

            if (dic_field_db_excel.Keys.Contains("TransactionCurrency") || dic_field_db_excel.Keys.Contains("PaymentCurrency"))
            {
                DataTable dt_excel_currency = null;

                if (dic_field_db_excel.Keys.Contains("TransactionCurrency"))
                {
                    dt_excel_currency = new DataView(dt_excel_without_excluded_rows).ToTable(true, dic_field_db_excel["TransactionCurrency"]);
                }

                if (dic_field_db_excel.Keys.Contains("PaymentCurrency"))
                {
                    DataTable dt_temp = new DataView(dt_excel_without_excluded_rows).ToTable(true, dic_field_db_excel["PaymentCurrency"]);

                    if (dt_excel_currency == null)
                    {
                        dt_excel_currency = dt_temp;
                    }
                    else
                    {
                        dt_excel_currency.Merge(dt_temp);

                        dt_excel_currency = new DataView(dt_excel_currency).ToTable(true, dt_excel_currency.Columns[0].ColumnName);
                    }
                }

                dt_excel_currency.Columns[0].ColumnName = "CurrencyFromUser";

                foreach (DataRow dr in dt_excel_currency.Rows)
                {
                    dr["CurrencyFromUser"] = ExcelAction.Valid_Field_Value(dr["CurrencyFromUser"].ToString());
                }

                Validate_Currency(dt_excel_currency, o_template, ref s_error);
            }

            if (dic_field_db_excel.Keys.Contains("CreditBrand"))
            {
                DataTable dt_excel_credit = new DataView(dt_excel_without_excluded_rows).ToTable(true, dic_field_db_excel["CreditBrand"]);

                dt_excel_credit.Columns[0].ColumnName = "CreditFromUser";

                foreach (DataRow dr in dt_excel_credit.Rows)
                {
                    dr["CreditFromUser"] = ExcelAction.Valid_Field_Value(dr["CreditFromUser"].ToString());
                }

                Validate_Credit(dt_excel_credit, o_template, ref s_error);
            }

            if (dic_field_db_excel.Keys.Contains("CardBrand"))
            {
                DataTable dt_excel_card = new DataView(dt_excel_without_excluded_rows).ToTable(true, dic_field_db_excel["CardBrand"]);

                dt_excel_card.Columns[0].ColumnName = "CardFromUser";

                foreach (DataRow dr in dt_excel_card.Rows)
                {
                    dr["CardFromUser"] = ExcelAction.Valid_Field_Value(dr["CardFromUser"].ToString());
                }

                Validate_Card(dt_excel_card, o_template, ref s_error);
            }

            if (dic_field_db_excel.Keys.Contains("OperationType"))
            {
                DataTable dt_excel_operation_type = new DataView(dt_excel_without_excluded_rows).ToTable(true, dic_field_db_excel["OperationType"]);

                dt_excel_operation_type.Columns[0].ColumnName = "OperationTypeFromUser";

                foreach (DataRow dr in dt_excel_operation_type.Rows)
                {
                    dr["OperationTypeFromUser"] = ExcelAction.Valid_Field_Value(dr["OperationTypeFromUser"].ToString());
                }

                Validate_Operation_Type(dt_excel_operation_type, o_template, ref s_error);
            }

            if (s_error != "") { return; }

            // TO DO !!! VALIDATE
            // Validate ClubNumber
            // Validate ExchangeRate vs Bank Israel
            // ==================

            // VALIDATE Agreement Item. Get Agreement Item Table from CashBoxPeriodTerminal. Options : When Inside - from CashBox OR Terminal, When Outside - from Terminal AND / OR Supplier

            DataTable dt_agreement_item = new DataTable();

            bool b_cashbox = (o_template.TerminalField == "" || o_template.TerminalField.Contains("CompanyNumber") || o_template.TerminalField.Contains("NetworkNumber") || o_template.TerminalField.Contains("BranchNumber") || o_template.TerminalField.Contains("CashBoxNumber"));
            
            // Available Fields in all scenarios : { CardBrand,TransactionDate,PaymentsCount }

            List<string> lst_field = new List<string>();
            List<string> lst_excel_field = new List<string>();

            if (dic_field_db_excel.Keys.Contains("CardBrand"))
            {
                lst_field.Add("CardBrand");
                lst_excel_field.Add(dic_field_db_excel["CardBrand"]);
            }

            if (dic_field_db_excel.Keys.Contains("TransactionDate"))
            {
                lst_field.Add("TransactionDate");
                lst_excel_field.Add(dic_field_db_excel["TransactionDate"]);
            }
            else
            {
                s_error = dic_field_db_excel["TransactionDate"] + " not exists in template fields.";
                return;
            }

            if (dic_field_db_excel.Keys.Contains("PaymentsCount"))
            {
                lst_field.Add("PaymentsCount");
                lst_excel_field.Add(dic_field_db_excel["PaymentsCount"]);
            }

            if (b_cashbox == true)
            {
                // Inside Only
                // Get Sub Table from dt_excel. Add Available Fields : { CompanyNumber,NetworkNumber,BranchNumber,CashBoxNumber }

                if (o_template.TerminalField.Contains("CompanyNumber"))
                {
                    lst_field.Add("CompanyNumber");
                    lst_excel_field.Add(dic_field_db_excel["CompanyNumber"]);
                }

                if (o_template.TerminalField.Contains("NetworkNumber"))
                {
                    lst_field.Add("NetworkNumber");
                    lst_excel_field.Add(dic_field_db_excel["NetworkNumber"]);
                }

                if (o_template.TerminalField.Contains("BranchNumber"))
                {
                    lst_field.Add("BranchNumber");
                    lst_excel_field.Add(dic_field_db_excel["BranchNumber"]);
                }

                if (o_template.TerminalField.Contains("CashBoxNumber"))
                {
                    lst_field.Add("CashBoxNumber");
                    lst_excel_field.Add(dic_field_db_excel["CashBoxNumber"]);
                }

                if (lst_field.Count > 0)
                {
                    DataTable dt_excel_cashbox = new DataView(dt_excel_without_excluded_rows).ToTable(true, lst_excel_field.ToArray());

                    foreach (string s_field in lst_field)
                    {
                        dt_excel_cashbox.Columns[dic_field_db_excel[s_field]].ColumnName = s_field;
                    }

                    Validate_Agreement_From_Cashbox(dt_excel_cashbox, o_template, ref dt_agreement_item, ref s_error);
                }
            }
            else
            {
                // Outside Only - ( TEMPLATE REMOVE TerminalNumber & SupplierNumber IF CASHBOX EXISTS )
                // Get Sub Table from dt_excel. Add Available Fields : { TerminalNumber,SupplierNumber,CreditBrand,OperationType }

                if (o_template.TerminalField.Contains("TerminalNumber"))
                {
                    lst_field.Add("TerminalNumber");
                    lst_excel_field.Add(dic_field_db_excel["TerminalNumber"]);
                }

                if (b_outside)
                {
                    if (o_template.TerminalField.Contains("SupplierNumber"))
                    {
                        lst_field.Add("SupplierNumber");
                        lst_excel_field.Add(dic_field_db_excel["SupplierNumber"]);
                    }

                    if (dic_field_db_excel.Keys.Contains("CreditBrand") && o_template.DiscountID > 0)
                    {
                        lst_field.Add("CreditBrand");
                        lst_excel_field.Add(dic_field_db_excel["CreditBrand"]);
                    }

                    if (dic_field_db_excel.Keys.Contains("OperationType"))
                    {
                        lst_field.Add("OperationType");
                        lst_excel_field.Add(dic_field_db_excel["OperationType"]);
                    }
                }

                if (lst_field.Count > 0)
                {
                    DataTable dt_excel_terminal = new DataView(dt_excel_without_excluded_rows).ToTable(true, lst_excel_field.ToArray());

                    foreach (string s_field in lst_field)
                    {
                        dt_excel_terminal.Columns[dic_field_db_excel[s_field]].ColumnName = s_field;
                    }

                    Validate_Agreement_From_Terminal(dt_excel_terminal, o_template, ref dt_agreement_item, ref s_error);
                }
            }

            if (s_error != "") { return; }

            // Get Clearing & Discount Agreement Payment Settings. Required to check PaymentDate for inside data file.

            DataTable dt_agreement_payment_settings = new DataTable();
            DataTable dt_agreement_discount = new DataTable();
            DataTable dt_agreement_discount_terminal = new DataTable();
            DataTable dt_agreement_discount_settings = new DataTable();

            if (b_inside)
            {
                DB.Bind_Data_Table("sprAgreementPaymentSettings", ref dt_agreement_payment_settings, ref s_error, "@UserID", n_user_id.ToString());

                if (s_error != "") { return; }

                AgreementDiscountAction.Select(ref dt_agreement_discount, ref dt_agreement_discount_terminal, ref dt_agreement_discount_settings, ref s_error, n_user_id);

                if (s_error != "") { return; }
            }

            // Get Bank-Branch Table. AccountNumber - will be changed to AccountID.

            DataTable dt_bank_branch = new DataTable();
            dt_bank_branch.Columns.Add("BankID");
            dt_bank_branch.Columns.Add("BankNumber");
            dt_bank_branch.Columns.Add("BranchID");
            dt_bank_branch.Columns.Add("BranchNumber");

            DB.Bind_Data_Table("sprBankBranch", ref dt_bank_branch, ref s_error);

            if (s_error != "") { return; }

            // GET DataTable (dt_data) to keep inside or outside operating data

            DataTable dt_data = b_inside ? o_data_file.TableDataInside : o_data_file.TableDataOutside;

            // ADD TO dt_excel : UniqueID AS FIRST COLUMN / ErrorMessage AS LAST COLUMN

            dt_excel.Columns.Add("UniqueID", typeof(Guid)).SetOrdinal(0);
            dt_excel.Columns.Add("ErrorMessage");

            // VALIDATE dt_excel

            bool b_valid = true;

            string s_field_money = "TransactionGrossAmount";

            if (b_outside == true && dic_field_db_excel.Keys.Contains("DutyPaymentAmount")) { s_field_money = "DutyPaymentAmount"; }

            foreach (DataRow dr_excel in dt_excel.Rows)
            {
                // CHECK exclude

                bool b_exclude = false;

                foreach (KeyValuePair<string, string> o_key_value in dic_field_exclude)
                {
                    string s_field = o_key_value.Key;
                    string s_value = dr_excel[s_field].ToString().Trim();
                    string[] arr_exclude = o_key_value.Value.Split(',');

                    for (int i = 0; i < arr_exclude.Length; i++)
                    {
                        string s_exclude = arr_exclude[i].Trim();

                        if (s_value == s_exclude)
                        {
                            b_exclude = true;
                            break;
                        }
                    }

                    if (b_exclude == true) { break; }
                }

                if (b_exclude == true) { continue; }

                string s_row_error = "";

                // CREATE new row in dt_data

                DataRow dr_data = dt_data.NewRow();

                // UniqueID --- dr_data & dr_excel have same UniqueID. When UniqueID is null in dt_excel, then this riw is excluded.

                Guid o_guid = Guid.NewGuid();

                dr_data["UniqueID"] = o_guid;
                dr_excel["UniqueID"] = o_guid;

                // COMMON FIELDS :  CompanyID / CashBoxPeriodTerminalID / AgreementDiscountTerminalID / CardID / OperationTypeID / TransactionCurrencyID / CardPrefix / CardNumber / DutyPaymentNumber / PaymentsCount / TransmissionNumber / TransactionGrossAmount / DutyPaymentAmount / RemainingPaymentsAmount / TransactionDate / TransmissionDate / PaymentDate/ VoucherNumber / ConfirmationNumber / UniqueID

                DataField o_data_field = new DataField();

                // CHECK Agreement Item (CashBoxPeriodTerminal). Agreement Item Table returned from CashBoxPeriodTerminal.

                if (dt_agreement_item.Rows.Count > 0)
                {
                    // Check Options : When Inside - from CashBox OR Terminal, When Outside - from Terminal AND / OR Supplier

                    // Inside
                    // -- tblData_Field:    CompanyNumber / NetworkNumber / BranchNumber / CashBoxNumber / TerminalNumber
                    // -- tblData_Inside:   CompanyID / NetworkID / BranchID / CashBoxID / CashBoxPeriodTerminalID / AgreementItemTerminalID / AgreementDiscountTerminalID === CardID / CreditID
                    // Outside
                    // -- tblData_Field:    TerminalNumber / SupplierNumber / SupplierGroupNumber
                    // -- tblData_Outside:  CompanyID / CashBoxPeriodTerminalID / AgreementItemTerminalID / AgreementDiscountTerminalID / TerminalID / SupplierID / SupplierGroupID === CardID / CreditID / OperationTypeID

                    string s_agreement_item_query = "";

                    if (dic_field_db_excel.Keys.Contains("CardBrand"))
                    {
                        if (s_agreement_item_query != "") { s_agreement_item_query += " AND "; }

                        string s_card_brand = ExcelAction.Valid_Field_Value(dr_excel[dic_field_db_excel["CardBrand"]].ToString());

                        s_agreement_item_query += " CardBrand = '" + s_card_brand + "' ";
                    }

                    if (dic_field_db_excel.Keys.Contains("TransactionDate"))
                    {
                        if (s_agreement_item_query != "") { s_agreement_item_query += " AND "; }

                        s_agreement_item_query += " TransactionDate = '" + dr_excel[dic_field_db_excel["TransactionDate"]] + "' ";
                    }

                    if (dic_field_db_excel.Keys.Contains("PaymentsCount"))
                    {
                        if (s_agreement_item_query != "") { s_agreement_item_query += " AND "; }

                        s_agreement_item_query += " PaymentsCount = '" + dr_excel[dic_field_db_excel["PaymentsCount"]] + "' ";
                    }

                    if (b_cashbox)
                    {
                        // INSIDE

                        if (o_template.TerminalField.Contains("CompanyNumber"))
                        {
                            if (s_agreement_item_query != "") { s_agreement_item_query += " AND "; }

                            s_agreement_item_query += " CompanyNumber = '" + dr_excel[dic_field_db_excel["CompanyNumber"]] + "' ";
                        }

                        if (o_template.TerminalField.Contains("NetworkNumber"))
                        {
                            if (s_agreement_item_query != "") { s_agreement_item_query += " AND "; }

                            s_agreement_item_query += " NetworkNumber = '" + dr_excel[dic_field_db_excel["NetworkNumber"]] + "' ";
                        }

                        if (o_template.TerminalField.Contains("BranchNumber"))
                        {
                            if (s_agreement_item_query != "") { s_agreement_item_query += " AND "; }

                            s_agreement_item_query += " BranchNumber = '" + dr_excel[dic_field_db_excel["BranchNumber"]] + "' ";
                        }

                        if (o_template.TerminalField.Contains("CashBoxNumber"))
                        {
                            if (s_agreement_item_query != "") { s_agreement_item_query += " AND "; }

                            s_agreement_item_query += " CashBoxNumber = '" + dr_excel[dic_field_db_excel["CashBoxNumber"]] + "' ";
                        }
                    }
                    else
                    {
                        // OUTSIDE

                        if (o_template.TerminalField.Contains("TerminalNumber"))
                        {
                            if (s_agreement_item_query != "") { s_agreement_item_query += " AND "; }

                            s_agreement_item_query += " TerminalNumber = '" + dr_excel[dic_field_db_excel["TerminalNumber"]] + "' ";
                        }

                        if (o_template.TerminalField.Contains("SupplierNumber"))
                        {
                            if (s_agreement_item_query != "") { s_agreement_item_query += " AND "; }

                            s_agreement_item_query += " SupplierNumber = '" + dr_excel[dic_field_db_excel["SupplierNumber"]] + "' ";
                        }

                        if (dic_field_db_excel.Keys.Contains("CreditBrand"))
                        {
                            if (o_template.CreditID > 0)
                            {
                                if (s_agreement_item_query != "") { s_agreement_item_query += " AND "; }

                                s_agreement_item_query += " CreditID = '" + o_template.CreditID + "' ";
                            }
                            else if (o_template.DiscountID > 0)
                            {
                                if (s_agreement_item_query != "") { s_agreement_item_query += " AND "; }

                                string s_credit_brand = ExcelAction.Valid_Field_Value(dr_excel[dic_field_db_excel["CreditBrand"]].ToString());

                                s_agreement_item_query += " CreditBrand = '" + s_credit_brand + "' ";
                            }
                        }

                        if (dic_field_db_excel.Keys.Contains("OperationType"))
                        {
                            if (s_agreement_item_query != "") { s_agreement_item_query += " AND "; }

                            string s_operation_type = ExcelAction.Valid_Field_Value(dr_excel[dic_field_db_excel["OperationType"]].ToString());

                            s_agreement_item_query += " OperationType = '" + s_operation_type + "' ";
                        }
                    }

                    // RETURNED FIELDS FROM dt_agreement_item :
                    // IF sprAgreement_From_Cashbox :   CompanyID NetworkID BranchID CashBoxID CashBoxPeriodTerminalID AgreementItemTerminalID CardID CreditID
                    // IF sprAgreement_From_Terminal :  CompanyID CashBoxPeriodTerminalID AgreementItemTerminalID TerminalID SupplierID SupplierGroupID CardID CreditID OperationTypeID

                    DataRow[] dr_agreement_item = dt_agreement_item.Select(s_agreement_item_query);

                    if (dr_agreement_item.Length == 1)
                    {
                        o_data_field.CompanyID = Convert.ToInt32(dr_agreement_item[0]["CompanyID"]);
                        o_data_field.CashBoxPeriodTerminalID = Convert.ToInt32(dr_agreement_item[0]["CashBoxPeriodTerminalID"]);
                        o_data_field.AgreementItemTerminalID = Convert.ToInt32(dr_agreement_item[0]["AgreementItemTerminalID"]);

                        if (b_inside == true)
                        {
                            o_data_field.NetworkID = Convert.ToInt32(dr_agreement_item[0]["NetworkID"]);
                            o_data_field.BranchID = Convert.ToInt32(dr_agreement_item[0]["BranchID"]);
                            o_data_field.CashBoxID = Convert.ToInt32(dr_agreement_item[0]["CashBoxID"]);
                            o_data_field.OperationTypeID = 1;
                        }
                        else
                        {
                            o_data_field.TerminalID = Convert.ToInt32(dr_agreement_item[0]["TerminalID"]);
                            o_data_field.SupplierID = Convert.ToInt32(dr_agreement_item[0]["SupplierID"]);
                            o_data_field.SupplierGroupID = Convert.ToInt32(dr_agreement_item[0]["SupplierGroupID"]);
                            o_data_field.OperationTypeID = Convert.ToInt32(dr_agreement_item[0]["OperationTypeID"]);
                        }

                        o_data_field.CreditID = Convert.ToInt32(dr_agreement_item[0]["CreditID"]);
                        o_data_field.CardID = Convert.ToInt32(dr_agreement_item[0]["CardID"]);

                        // !!! AgreementDiscountTerminalID WILL BE UPDATED AFTER TransactionDate & PaymentsCount VALIDATION
                    }

                    if (o_data_field.CashBoxPeriodTerminalID == 0 && chkStopIfAgreementNotExists.Checked == true)
                    {
                        if (dr_agreement_item.Length <= 1)
                        {
                            Row_Error_Add("Agreement item not exists.", ref s_row_error);
                        }
                        else if (dr_agreement_item.Length > 1)
                        {
                            Row_Error_Add(String.Format("Too many agreement items returned ({0}).", dr_agreement_item.Length), ref s_row_error);
                        }
                    }
                }

                // CompanyID

                if (o_data_field.CompanyID == 0)
                {
                    o_data_field.CompanyID = Convert.ToInt32(ddlCompany.SelectedValue);

                    if (o_data_field.CompanyID == 0)
                    {
                        Row_Error_Add("Company not exists.", ref s_row_error);
                    }
                }

                // CardID

                if (o_data_field.CardID == 0)
                {
                    if (dic_field_db_excel.Keys.Contains("CardBrand"))
                    {
                        string s_card_brand = ExcelAction.Valid_Field_Value(dr_excel[dic_field_db_excel["CardBrand"]].ToString());

                        if (s_card_brand != "")
                        {
                            DataRow dr_template_card = o_template.TableTemplateCard.Select(" CardFromUser = '" + s_card_brand + "' ").FirstOrDefault();

                            if (dr_template_card != null)
                            {
                                o_data_field.CardID = Convert.ToInt32(dr_template_card["CardID"]);
                            }
                        }

                        if (o_data_field.CardID == 0 && chkStopIfCardIsEmpty.Checked == true)
                        {
                            Row_Error_Add(String.Format("{0} not exists.", dic_field_db_excel["CardBrand"]), ref s_row_error);
                        }
                    }
                }

                // OperationTypeID

                if (dic_field_db_excel.Keys.Contains("OperationType") && o_data_field.OperationTypeID == 0)
                {
                    string s_operation_type = ExcelAction.Valid_Field_Value(dr_excel[dic_field_db_excel["OperationType"]].ToString());

                    if (s_operation_type != "")
                    {
                        DataRow dr_template_operation_type = o_template.TableTemplateOperationType.Select(" OperationTypeFromUser = '" + s_operation_type + "' ").FirstOrDefault();

                        if (dr_template_operation_type != null)
                        {
                            o_data_field.OperationTypeID = Convert.ToInt32(dr_template_operation_type["OperationTypeID"]);
                        }
                    }

                    if (o_data_field.OperationTypeID == 0)
                    {
                        Row_Error_Add(String.Format("{0} not exists.", dic_field_db_excel["OperationType"]), ref s_row_error);
                    }
                }

                // TransactionCurrencyID / DEFAULT = 1 (ILS)

                if (dic_field_db_excel.Keys.Contains("TransactionCurrency"))
                {
                    string s_transaction_currency = ExcelAction.Valid_Field_Value(dr_excel[dic_field_db_excel["TransactionCurrency"]].ToString());

                    if (s_transaction_currency != "")
                    {
                        DataRow dr_template_currency = o_template.TableTemplateCurrency.Select(" CurrencyFromUser = '" + s_transaction_currency + "' ").FirstOrDefault();

                        if (dr_template_currency != null)
                        {
                            o_data_field.TransactionCurrencyID = Convert.ToInt32(dr_template_currency["CurrencyID"]);
                        }
                    }
                }

                if (o_data_field.TransactionCurrencyID < 1) { o_data_field.TransactionCurrencyID = 1; }

                // CardPrefix / CardNumber

                string s_card_prefix = "", s_card_number = "";
                long n_card_prefix = 0, n_card_number = 0;

                if (dic_field_db_excel.Keys.Contains("CardPrefix") && dic_field_db_excel.Keys.Contains("CardNumber"))
                {
                    s_card_prefix = dr_excel[dic_field_db_excel["CardPrefix"]].ToString().Trim();
                    s_card_number = dr_excel[dic_field_db_excel["CardNumber"]].ToString().Trim().ToLower().Replace("*", "").Replace("x", "");

                    long.TryParse(s_card_prefix, out n_card_prefix);
                    long.TryParse(s_card_number, out n_card_number);

                    if (n_card_prefix > 0) { s_card_prefix = n_card_prefix.ToString(); } else { s_card_prefix = ""; }
                    if (n_card_number > 0) { s_card_number = n_card_number.ToString(); } else { s_card_number = ""; }

                    n_card_prefix = 0;
                    n_card_number = 0;

                    if (s_card_prefix.Length == 5 && s_card_number != "")
                    {
                        s_card_prefix = s_card_prefix + s_card_number.Substring(0, 1);
                        s_card_number = s_card_number.Substring(1);

                        if (s_card_prefix.StartsWith("37") && s_card_number.EndsWith("0")) { s_card_number = s_card_number.Substring(0, s_card_number.Length - 1); }
                    }
                }
                else if (dic_field_db_excel.Keys.Contains("CardNumber"))
                {
                    char[] arr_char = dr_excel[dic_field_db_excel["CardNumber"]].ToString().Trim().ToCharArray();

                    string s_card = "";

                    for (int i = 0; i < arr_char.Length; i++)
                    {
                        char c_char = arr_char[i];

                        if (c_char >= 48 && c_char <= 57)
                        {
                            s_card += c_char.ToString();
                        }
                        else if (s_card.Contains("x") == false)
                        {
                            s_card += "x";
                        }
                    }

                    if (s_card.Contains("x") == true)
                    {
                        string[] arr_card = s_card.Split('x');

                        s_card_prefix = arr_card[0];
                        s_card_number = arr_card[1];

                        long.TryParse(s_card_prefix, out n_card_prefix);
                        long.TryParse(s_card_number, out n_card_number);

                        if (n_card_prefix > 0) { s_card_prefix = n_card_prefix.ToString(); } else { s_card_prefix = ""; }
                        if (n_card_number > 0) { s_card_number = n_card_number.ToString(); } else { s_card_number = ""; }

                        n_card_prefix = 0;
                        n_card_number = 0;
                    }
                    else
                    {
                        long n_card = 0;
                        long.TryParse(s_card, out n_card);

                        if (n_card > 0) { s_card = n_card.ToString(); } else { s_card = ""; }

                        s_card_number = s_card;
                    }
                }
                else if (dic_field_db_excel.Keys.Contains("CardPrefix"))
                {
                    s_card_prefix = dr_excel[dic_field_db_excel["CardPrefix"]].ToString().Trim();

                    long.TryParse(s_card_prefix, out n_card_prefix);

                    if (n_card_prefix > 0) { s_card_prefix = n_card_prefix.ToString(); } else { s_card_prefix = ""; }

                    n_card_prefix = 0;
                }

                if (s_card_prefix == "" && s_card_number.Length > 7)
                {
                    int n_prefix_length = (s_card_number.Length >= 14) ? 6 : 2;

                    s_card_prefix = s_card_number.Substring(0, n_prefix_length);
                    s_card_number = s_card_number.Substring(n_prefix_length);
                }

                if (s_card_prefix.Length > 6) { s_card_prefix = s_card_prefix.Substring(0, 6); }
                if (s_card_number.Length > 4) { s_card_number = s_card_number.Substring(s_card_number.Length - 4); }

                long.TryParse(s_card_prefix, out n_card_prefix);
                long.TryParse(s_card_number, out n_card_number);

                o_data_field.CardPrefix = (int)n_card_prefix;
                o_data_field.CardNumber = (int)n_card_number;

                // PaymentsCount

                if (dic_field_db_excel.Keys.Contains("PaymentsCount"))
                {
                    int n_payments_count = 0;

                    if (b_inside == true)
                    {
                        int.TryParse(dr_excel[dic_field_db_excel["PaymentsCount"]].ToString(), out n_payments_count);
                    }
                    else
                    {
                        string s_payments_count = dr_excel[dic_field_db_excel["PaymentsCount"]].ToString().Trim();

                        char c_separator = (char)0;

                        if (s_payments_count.Contains("/"))
                        {
                            c_separator = '/';
                        }
                        else if (s_payments_count.Contains("-"))
                        {
                            c_separator = '-';
                        }

                        if (c_separator == (char)0)
                        {
                            int.TryParse(s_payments_count, out n_payments_count);
                        }
                        else
                        {
                            string[] arr_payments_count = s_payments_count.Split(c_separator);

                            o_data_field.DutyPaymentNumber = Convert.ToInt32(arr_payments_count[0]);

                            n_payments_count = Convert.ToInt32(arr_payments_count[1]);
                        }
                    }

                    o_data_field.PaymentsCount = n_payments_count;
                }

                if (o_data_field.PaymentsCount < 1) { o_data_field.PaymentsCount = 1; }

                // DutyPaymentNumber

                if (dic_field_db_excel.Keys.Contains("DutyPaymentNumber"))
                {
                    int n_duty_payment_number = 0;
                    int.TryParse(dr_excel[dic_field_db_excel["DutyPaymentNumber"]].ToString(), out n_duty_payment_number);

                    o_data_field.DutyPaymentNumber = n_duty_payment_number;
                }

                if (o_data_field.DutyPaymentNumber < 1) { o_data_field.DutyPaymentNumber = 1; }

                if (o_data_field.DutyPaymentNumber > o_data_field.PaymentsCount)
                {
                    Row_Error_Add(String.Format("{0} greater than {1}.", dic_field_db_excel["DutyPaymentNumber"], o_data_field.PaymentsCount), ref s_row_error);
                }

                // TransmissionNumber

                if (dic_field_db_excel.Keys.Contains("TransmissionNumber"))
                {
                    long n_transmission_number = 0;
                    long.TryParse(dr_excel[dic_field_db_excel["TransmissionNumber"]].ToString(), out n_transmission_number);

                    o_data_field.TransmissionNumber = n_transmission_number;
                }

                // TransactionGrossAmount

                if (dic_field_db_excel.Keys.Contains("TransactionGrossAmount"))
                {
                    o_data_field.TransactionGrossAmount = ExcelAction.Valid_Amount(dr_excel[dic_field_db_excel["TransactionGrossAmount"]].ToString());
                }

                // DutyPaymentAmount

                if (dic_field_db_excel.Keys.Contains("DutyPaymentAmount"))
                {
                    o_data_field.DutyPaymentAmount = ExcelAction.Valid_Amount(dr_excel[dic_field_db_excel["DutyPaymentAmount"]].ToString());
                }

                if (b_inside == true && o_data_field.PaymentsCount == 1)
                {
                    o_data_field.DutyPaymentAmount = o_data_field.TransactionGrossAmount;
                }

                // RemainingPaymentsAmount

                if (dic_field_db_excel.Keys.Contains("RemainingPaymentsAmount"))
                {
                    o_data_field.RemainingPaymentsAmount = ExcelAction.Valid_Amount(dr_excel[dic_field_db_excel["RemainingPaymentsAmount"]].ToString());
                }

                // TransactionDate

                if (dic_field_db_excel.Keys.Contains("TransactionDate"))
                {
                    string s_date = dr_excel[dic_field_db_excel["TransactionDate"]].ToString().Trim();

                    DateTime d_date = new DateTime();

                    bool b_date = Common.Convert_To_Date(s_date, dic_field_format["TransactionDate"], ref d_date);

                    if (b_date && d_date < d_date_min)
                    {
                        b_date = false;
                    }

                    if (b_date)
                    {
                        o_data_field.TransactionDate = d_date;
                    }
                    else
                    {
                        // INSIDE:  TransactionDate REQUIRED.
                        // OUTSIDE: TransactionDate NOT REQUIRED IF DutyPaymentAmount & CardNumber & CardPrefix NOT EXISTS. THIS IS A COMMISSION TRANSACTION AND ERROR MESSAGE NOT REQUIRED

                        if ((b_inside == true) || (b_outside == true && (o_data_field.DutyPaymentAmount > 0 || o_data_field.CardNumber > 0 || o_data_field.CardPrefix > 0)))
                        {
                            Row_Error_Add(String.Format("{0} not valid.", dic_field_db_excel["TransactionDate"]), ref s_row_error);
                        }
                    }
                }

                // TransmissionDate

                if (dic_field_db_excel.Keys.Contains("TransmissionDate"))
                {
                    string s_date = dr_excel[dic_field_db_excel["TransmissionDate"]].ToString().Trim();

                    DateTime d_date = new DateTime();

                    bool b_date = Common.Convert_To_Date(s_date, dic_field_format["TransmissionDate"], ref d_date);

                    if (b_date && d_date < d_date_min)
                    {
                        b_date = false;
                    }

                    if (b_date) { o_data_field.TransmissionDate = d_date; }
                }

                // AgreementDiscountTerminalID ( CHECK IF EXISTS Discount Agreement ) : CAN BE UPDATED AFTER TransactionDate & PaymentsCount VALIDATION / NECESSARY FOR [Calculate PaymentDate] WHEN b_inside

                int n_agreement_discount_id = 0;

                if (o_data_field.AgreementItemTerminalID > 0 && dt_agreement_discount_terminal.Rows.Count > 0)
                {
                    string s_query =
                           " AgreementItemTerminalID = " + o_data_field.AgreementItemTerminalID +
                           " AND DateStart <= '" + o_data_field.TransactionDate + "' AND DateEnd >= '" + o_data_field.TransactionDate + "'" +
                           " AND PaymentsFrom <= " + o_data_field.PaymentsCount + " AND PaymentsTo >= " + o_data_field.PaymentsCount;

                    DataRow dr_agreement_discount_terminal = dt_agreement_discount_terminal.Select(s_query).FirstOrDefault();

                    if (dr_agreement_discount_terminal != null)
                    {
                        o_data_field.AgreementDiscountTerminalID = Convert.ToInt32(dr_agreement_discount_terminal["ID"]);

                        n_agreement_discount_id = Convert.ToInt32(dr_agreement_discount_terminal["AgreementDiscountID"]);
                    }
                }

                // PaymentDate

                if (b_inside == true)
                {
                    if (o_data_field.TransactionDate == null || (o_data_field.AgreementItemTerminalID == 0))
                    {
                        if (chkStopIfAgreementNotExists.Checked == true)
                        {
                            Row_Error_Add("Can't calculate Payment Date without Transaction Date and Agreement item.", ref s_row_error);
                        }
                    }
                    else
                    {
                        string s_query = "";

                        // Calculate PaymentDate : IF Discount EXISTS THEN FROM Discount ELSE FROM Clearing

                        if (n_agreement_discount_id > 0)
                        {
                            DateTime? d_base_date = null;

                            DataRow dr_agreement_discount = dt_agreement_discount.Select(" ID = " + n_agreement_discount_id).FirstOrDefault();

                            string s_discount_period_type_key = dr_agreement_discount["DiscountPeriodTypeKey"].ToString();
                            string s_base_date = dr_agreement_discount["BaseDate"].ToString();

                            int n_week_start = Convert.ToInt32(dr_agreement_discount["WeekStart"]);
                            int n_payment_after_days = Convert.ToInt32(dr_agreement_discount["PaymentAfterDays"]);
                            int n_days_add = 0;

                            if (s_base_date == "TransactionDate")
                            {
                                d_base_date = o_data_field.TransactionDate;
                            }
                            else if (s_base_date == "TransmissionDate" || s_base_date == "AbsorptionDate")
                            {
                                d_base_date = o_data_field.TransmissionDate;
                            }

                            if (d_base_date != null)
                            {
                                switch (s_discount_period_type_key)
                                {
                                    case "daily":
                                        o_data_field.PaymentDate = d_base_date.Value.AddDays(n_payment_after_days);
                                        break;
                                    case "weekly":
                                    case "fortnightly":
                                        int n_days_period = s_discount_period_type_key == "weekly" ? 7 : 14;
                                        int n_day_of_week = (int)d_base_date.Value.DayOfWeek;

                                        n_days_add = n_days_period + n_payment_after_days - n_day_of_week - 1;

                                        o_data_field.PaymentDate = d_base_date.Value.AddDays(n_days_add);

                                        break;
                                    case "one-third-monthly":
                                        s_query =
                                           " AgreementDiscountID = " + n_agreement_discount_id +
                                           " AND " +
                                           " ( " +
                                                " ( DayFrom < DayTo AND DayFrom <= " + d_base_date.Value.Day + " AND DayTo >= " + d_base_date.Value.Day + " ) " +
                                                " OR " +
                                                " ( DayFrom > DayTo AND DayTo <= " + d_base_date.Value.Day + " AND DayFrom >= " + d_base_date.Value.Day + " ) " +
                                                " OR " +
                                                " ( DayFrom = DayTo AND DayFrom = " + d_base_date.Value.Day + " AND DayTo = " + d_base_date.Value.Day + " ) " +
                                           " ) ";

                                        DataRow dr_agreement_discount_settings = dt_agreement_discount_settings.Select(s_query).FirstOrDefault();

                                        if (dr_agreement_discount_settings != null)
                                        {
                                            int n_day_from = Convert.ToInt32(dr_agreement_discount_settings["DayFrom"]);
                                            int n_day_to = Convert.ToInt32(dr_agreement_discount_settings["DayTo"]);
                                            int n_days_in_month = DateTime.DaysInMonth(d_base_date.Value.Year, d_base_date.Value.Month);

                                            if (n_day_from > n_days_in_month) { n_day_from = n_days_in_month; }
                                            if (n_day_to > n_days_in_month) { n_day_to = n_days_in_month; }

                                            n_payment_after_days = Convert.ToInt32(dr_agreement_discount_settings["PaymentAfterDays"]);

                                            if (n_day_from < n_day_to)
                                            {
                                                n_days_add = n_day_to - d_base_date.Value.Day + n_payment_after_days;
                                            }
                                            else if (n_day_from > n_day_to)
                                            {
                                                n_days_add = n_days_in_month - d_base_date.Value.Day + n_day_to + n_payment_after_days;
                                            }
                                            else if (n_day_from == n_day_to)
                                            {
                                                n_days_add = n_payment_after_days;
                                            }

                                            o_data_field.PaymentDate = d_base_date.Value.AddDays(n_days_add);
                                        }

                                        break;
                                    case "monthly":
                                        if (d_base_date.Value.Month == 12)
                                        {
                                            o_data_field.PaymentDate = new DateTime(d_base_date.Value.Year + 1, 1, n_payment_after_days);
                                        }
                                        else
                                        {
                                            o_data_field.PaymentDate = new DateTime(d_base_date.Value.Year, d_base_date.Value.Month + 1, n_payment_after_days);
                                        }

                                        break;
                                }
                            }
                            else
                            {
                                if (chkStopIfAgreementNotExists.Checked == true)
                                {
                                    Row_Error_Add("Can't calculate Payment Date without Base Date.", ref s_row_error);
                                }
                            }

                            if (d_base_date != null && o_data_field.PaymentDate == null)
                            {
                                if (chkStopIfAgreementNotExists.Checked == true)
                                {
                                    Row_Error_Add("Error on calculate Payment Date from Discount Settings.", ref s_row_error);
                                }
                            }
                        }
                        else
                        {
                            s_query =
                                " CardID = " + o_data_field.CardID +
                                " AND CreditID = " + o_data_field.CreditID +
                                " AND DateStart <= '" + o_data_field.TransactionDate + "' AND DateEnd >= '" + o_data_field.TransactionDate + "'" +
                                " AND DayFrom <= " + o_data_field.TransactionDate.Value.Day + " AND DayTo >= " + o_data_field.TransactionDate.Value.Day;

                            DataRow[] dr_agreement_payment_settings = dt_agreement_payment_settings.Select(s_query);

                            if (dr_agreement_payment_settings.Length == 1)
                            {
                                int n_day_payment = Convert.ToInt32(dr_agreement_payment_settings[0]["DayPayment"]);
                                int n_month_payment = 0, n_year_payment = 0;

                                if (o_data_field.TransactionDate.Value.Month == 12)
                                {
                                    n_month_payment = 1;
                                    n_year_payment = o_data_field.TransactionDate.Value.Year + 1;
                                }
                                else
                                {
                                    n_month_payment = o_data_field.TransactionDate.Value.Month + 1;
                                    n_year_payment = o_data_field.TransactionDate.Value.Year;
                                }

                                o_data_field.PaymentDate = new DateTime(n_year_payment, n_month_payment, n_day_payment);
                            }
                            else
                            {
                                if (chkStopIfAgreementNotExists.Checked == true)
                                {
                                    if (dr_agreement_payment_settings.Length == 0)
                                    {
                                        Row_Error_Add("Payment settings not exists.", ref s_row_error);
                                    }
                                    else if (dr_agreement_payment_settings.Length > 1)
                                    {
                                        Row_Error_Add(String.Format("Too many payment settings returned ({0}).", dr_agreement_payment_settings.Length), ref s_row_error);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (b_outside == true && dic_field_db_excel.Keys.Contains("PaymentDate"))
                {
                    string s_date = dr_excel[dic_field_db_excel["PaymentDate"]].ToString().Trim();

                    DateTime d_date = new DateTime();

                    bool b_date = Common.Convert_To_Date(s_date, dic_field_format["PaymentDate"], ref d_date);

                    if (b_date && d_date < d_date_min)
                    {
                        b_date = false;
                    }

                    if (b_date)
                    {
                        o_data_field.PaymentDate = d_date;
                    }
                    else
                    {
                        Row_Error_Add(String.Format("{0} not valid.", dic_field_db_excel["PaymentDate"]), ref s_row_error);
                    }
                }

                // VoucherNumber

                if (dic_field_db_excel.Keys.Contains("VoucherNumber"))
                {
                    string s_voucher_number = dr_excel[dic_field_db_excel["VoucherNumber"]].ToString();

                    long n_voucher_number = 0;

                    bool b_number = long.TryParse(s_voucher_number, out n_voucher_number);

                    if (b_number == true)
                    {
                        if (n_voucher_number == 0) { s_voucher_number = ""; } else { s_voucher_number = n_voucher_number.ToString(); }
                    }

                    if (s_voucher_number.Length > 15) { s_voucher_number = s_voucher_number.Substring(0, 15); }

                    o_data_field.VoucherNumber = s_voucher_number;
                }

                // ConfirmationNumber

                if (dic_field_db_excel.Keys.Contains("ConfirmationNumber"))
                {
                    string s_confirmation_number = dr_excel[dic_field_db_excel["ConfirmationNumber"]].ToString();

                    long n_confirmation_number = 0;

                    bool b_number = long.TryParse(s_confirmation_number, out n_confirmation_number);

                    if (b_number == true)
                    {
                        if (n_confirmation_number == 0) { s_confirmation_number = ""; } else { s_confirmation_number = n_confirmation_number.ToString(); }
                    }

                    if (s_confirmation_number.Length > 15) { s_confirmation_number = s_confirmation_number.Substring(0, 15); }

                    o_data_field.ConfirmationNumber = s_confirmation_number;
                }

                // int
                dr_data["CompanyID"] = o_data_field.CompanyID;
                dr_data["CashBoxPeriodTerminalID"] = o_data_field.CashBoxPeriodTerminalID;
                dr_data["AgreementDiscountTerminalID"] = o_data_field.AgreementDiscountTerminalID;

                if (b_inside == true)
                {
                    dr_data["NetworkID"] = o_data_field.NetworkID;
                    dr_data["BranchID"] = o_data_field.BranchID;
                    dr_data["CashBoxID"] = o_data_field.CashBoxID;
                }
                else
                {
                    dr_data["TerminalID"] = o_data_field.TerminalID;
                    dr_data["SupplierID"] = o_data_field.SupplierID;
                    dr_data["SupplierGroupID"] = o_data_field.SupplierGroupID;
                }

                dr_data["CardID"] = o_data_field.CardID;
                dr_data["OperationTypeID"] = o_data_field.OperationTypeID;
                dr_data["TransactionCurrencyID"] = o_data_field.TransactionCurrencyID;
                dr_data["CardPrefix"] = o_data_field.CardPrefix;
                dr_data["CardNumber"] = o_data_field.CardNumber;
                dr_data["PaymentsCount"] = o_data_field.PaymentsCount;
                dr_data["DutyPaymentNumber"] = o_data_field.DutyPaymentNumber;

                // bigint
                dr_data["TransmissionNumber"] = o_data_field.TransmissionNumber;

                // float
                dr_data["TransactionGrossAmount"] = o_data_field.TransactionGrossAmount;
                dr_data["DutyPaymentAmount"] = o_data_field.DutyPaymentAmount;
                dr_data["RemainingPaymentsAmount"] = o_data_field.RemainingPaymentsAmount;

                // date

                if (o_data_field.TransactionDate != null)
                {
                    DateTime d_date = (DateTime)o_data_field.TransactionDate;

                    dr_data["TransactionDate"] = d_date.Year + "-" + d_date.Month + "-" + d_date.Day;
                }
                else
                {
                    dr_data["TransactionDate"] = "";
                }

                if (o_data_field.TransmissionDate != null)
                {
                    DateTime d_date = (DateTime)o_data_field.TransmissionDate;

                    dr_data["TransmissionDate"] = d_date.Year + "-" + d_date.Month + "-" + d_date.Day;
                }
                else
                {
                    dr_data["TransmissionDate"] = "";
                }

                if (o_data_field.PaymentDate != null)
                {
                    DateTime d_date = (DateTime)o_data_field.PaymentDate;

                    dr_data["PaymentDate"] = d_date.Year + "-" + d_date.Month + "-" + d_date.Day;
                }
                else
                {
                    dr_data["PaymentDate"] = "";
                }

                // varchar(15)
                dr_data["VoucherNumber"] = o_data_field.VoucherNumber;
                dr_data["ConfirmationNumber"] = o_data_field.ConfirmationNumber;

                if (b_inside)
                {
                    // INSIDE FIELDS :  FirstPaymentAmount / ReceiptNumber

                    // FirstPaymentAmount

                    if (dic_field_db_excel.Keys.Contains("FirstPaymentAmount"))
                    {
                        o_data_field.FirstPaymentAmount = ExcelAction.Valid_Amount(dr_excel[dic_field_db_excel["FirstPaymentAmount"]].ToString());

                        if (o_data_field.FirstPaymentAmount == 0 && o_data_field.PaymentsCount == 1)
                        {
                            o_data_field.FirstPaymentAmount = o_data_field.TransactionGrossAmount;
                        }
                    }

                    // ReceiptNumber

                    if (dic_field_db_excel.Keys.Contains("ReceiptNumber"))
                    {
                        string s_receipt_number = dr_excel[dic_field_db_excel["ReceiptNumber"]].ToString();

                        long n_receipt_number = 0;

                        bool b_number = long.TryParse(s_receipt_number, out n_receipt_number);

                        if (b_number == true)
                        {
                            if (n_receipt_number == 0) { s_receipt_number = ""; } else { s_receipt_number = n_receipt_number.ToString(); }
                        }

                        if (s_receipt_number.Length > 15) { s_receipt_number = s_receipt_number.Substring(0, 15); }

                        o_data_field.ReceiptNumber = s_receipt_number;
                    }

                    // float
                    dr_data["FirstPaymentAmount"] = o_data_field.FirstPaymentAmount;
                    // varchar(15)
                    dr_data["ReceiptNumber"] = o_data_field.ReceiptNumber;
                    // bit
                    dr_data["IsSplitted"] = false;
                }
                else if (b_outside)
                {
                    // OUTSIDE FIELDS : CreditID / PaymentCurrencyID / BankID / BankBranchID / AccountNumber / InvoiceNumber / ClubNumber / NetPaymentAmount / ClearingCommission / NotElectronicCommission / ManualCommission / CancellationCommission / TelephoneCommission / DiscountCommission / ClubManagementCommission / ClubSaving / VAT / ExchangeRate / AbsorptionDate / PaymentDateActual / InvoiceDate / IsAbroad

                    // CreditID

                    if (o_data_field.CreditID == 0)
                    {
                        if (o_template.CreditID > 0)
                        {
                            o_data_field.CreditID = o_template.CreditID;
                        }
                        else if (o_template.DiscountID > 0 && o_data_field.CreditID == 0)
                        {
                            if (dic_field_db_excel.Keys.Contains("CreditBrand"))
                            {
                                string s_credit_brand = ExcelAction.Valid_Field_Value(dr_excel[dic_field_db_excel["CreditBrand"]].ToString());

                                if (s_credit_brand != "")
                                {
                                    DataRow dr_template_credit = o_template.TableTemplateCredit.Select(" CreditFromUser = '" + s_credit_brand + "' ").FirstOrDefault();

                                    if (dr_template_credit != null)
                                    {
                                        o_data_field.CreditID = Convert.ToInt32(dr_template_credit["CreditID"]);
                                    }
                                }

                                if (o_data_field.CreditID == 0)
                                {
                                    Row_Error_Add(String.Format("{0} not exists.", dic_field_db_excel["CreditBrand"]), ref s_row_error);
                                }
                            }
                        }
                    }

                    // PaymentCurrencyID / DEFAULT = 1 (ILS)

                    if (dic_field_db_excel.Keys.Contains("PaymentCurrency"))
                    {
                        string s_payment_currency = ExcelAction.Valid_Field_Value(dr_excel[dic_field_db_excel["PaymentCurrency"]].ToString());

                        if (s_payment_currency != "")
                        {
                            DataRow dr_template_currency = o_template.TableTemplateCurrency.Select(" CurrencyFromUser = '" + s_payment_currency + "' ").FirstOrDefault();

                            if (dr_template_currency != null)
                            {
                                o_data_field.PaymentCurrencyID = Convert.ToInt32(dr_template_currency["CurrencyID"]);
                            }
                            else
                            {
                                Row_Error_Add(String.Format("{0} not exists.", dic_field_db_excel["PaymentCurrency"]), ref s_row_error);
                            }
                        }
                    }

                    if (o_data_field.PaymentCurrencyID < 1) { o_data_field.PaymentCurrencyID = 1; }

                    // BankID / BankBranchID / AccountNumber

                    long n_bank_number = 0, n_bank_branch_number = 0;

                    if (dic_field_db_excel.Keys.Contains("BankNumber"))
                    {
                        long.TryParse(dr_excel[dic_field_db_excel["BankNumber"]].ToString(), out n_bank_number);

                        if (n_bank_number > 0)
                        {
                            string s_bank_error = "";

                            o_data_field.BankID = Get_BankID(dt_bank_branch, n_bank_number, ref s_bank_error);

                            if (s_bank_error == "not-exists")
                            {
                                Row_Error_Add(String.Format("{0} not exists.", dic_field_db_excel["BankNumber"]), ref s_row_error);
                            }
                        }
                    }

                    if (dic_field_db_excel.Keys.Contains("BankBranchNumber") && o_data_field.BankID > 0)
                    {
                        long.TryParse(dr_excel[dic_field_db_excel["BankBranchNumber"]].ToString(), out n_bank_branch_number);

                        if (n_bank_branch_number > 0)
                        {
                            string s_bank_branch_error = "";

                            o_data_field.BankBranchID = Get_BankBranchID(dt_bank_branch, n_bank_number, n_bank_branch_number, ref s_bank_branch_error);

                            if (s_bank_branch_error == "not-exists")
                            {
                                Row_Error_Add(String.Format("{0} not exists.", dic_field_db_excel["BankBranchNumber"]), ref s_row_error);
                            }
                        }
                    }

                    if (dic_field_db_excel.Keys.Contains("AccountNumber"))
                    {
                        int n_account_number = 0;
                        int.TryParse(dr_excel[dic_field_db_excel["AccountNumber"]].ToString(), out n_account_number);

                        if (n_account_number < 0) { n_account_number = 0; }

                        if (n_account_number == 0)
                        {
                            Row_Error_Add(String.Format("{0} not exists.", dic_field_db_excel["AccountNumber"]), ref s_row_error);
                        }

                        o_data_field.AccountNumber = n_account_number;
                    }

                    // InvoiceNumber

                    if (dic_field_db_excel.Keys.Contains("InvoiceNumber"))
                    {
                        long n_invoice_number = 0;
                        long.TryParse(dr_excel[dic_field_db_excel["InvoiceNumber"]].ToString(), out n_invoice_number);

                        o_data_field.InvoiceNumber = n_invoice_number;
                    }

                    // ClubNumber

                    if (dic_field_db_excel.Keys.Contains("ClubNumber"))
                    {
                        long n_club_number = 0;
                        long.TryParse(dr_excel[dic_field_db_excel["ClubNumber"]].ToString(), out n_club_number);

                        if (n_club_number < 0) { n_club_number = 0; }

                        o_data_field.ClubNumber = n_club_number;
                    }

                    // NetPaymentAmount

                    if (dic_field_db_excel.Keys.Contains("NetPaymentAmount"))
                    {
                        o_data_field.NetPaymentAmount = ExcelAction.Valid_Amount(dr_excel[dic_field_db_excel["NetPaymentAmount"]].ToString());
                    }

                    // ClearingCommission

                    if (dic_field_db_excel.Keys.Contains("ClearingCommission"))
                    {
                        double n_clearing_commission = 0;
                        double.TryParse(dr_excel[dic_field_db_excel["ClearingCommission"]].ToString(), out n_clearing_commission);

                        o_data_field.ClearingCommission = n_clearing_commission;
                    }

                    // NotElectronicCommission

                    if (dic_field_db_excel.Keys.Contains("NotElectronicCommission"))
                    {
                        double n_not_electronic_commission = 0;
                        double.TryParse(dr_excel[dic_field_db_excel["NotElectronicCommission"]].ToString(), out n_not_electronic_commission);

                        o_data_field.NotElectronicCommission = n_not_electronic_commission;
                    }

                    // ManualCommission

                    if (dic_field_db_excel.Keys.Contains("ManualCommission"))
                    {
                        double n_manual_commission = 0;
                        double.TryParse(dr_excel[dic_field_db_excel["ManualCommission"]].ToString(), out n_manual_commission);

                        o_data_field.ManualCommission = n_manual_commission;
                    }

                    // CancellationCommission

                    if (dic_field_db_excel.Keys.Contains("CancellationCommission"))
                    {
                        double n_cancellation_commission = 0;
                        double.TryParse(dr_excel[dic_field_db_excel["CancellationCommission"]].ToString(), out n_cancellation_commission);

                        o_data_field.CancellationCommission = n_cancellation_commission;
                    }

                    // TelephoneCommission

                    if (dic_field_db_excel.Keys.Contains("TelephoneCommission"))
                    {
                        double n_telephone_commission = 0;
                        double.TryParse(dr_excel[dic_field_db_excel["TelephoneCommission"]].ToString(), out n_telephone_commission);

                        o_data_field.TelephoneCommission = n_telephone_commission;
                    }

                    // DiscountCommission

                    if (dic_field_db_excel.Keys.Contains("DiscountCommission"))
                    {
                        double n_discount_commission = 0;
                        double.TryParse(dr_excel[dic_field_db_excel["DiscountCommission"]].ToString(), out n_discount_commission);

                        o_data_field.DiscountCommission = n_discount_commission;
                    }

                    // ClubManagementCommission

                    if (dic_field_db_excel.Keys.Contains("ClubManagementCommission"))
                    {
                        double n_club_management_commission = 0;
                        double.TryParse(dr_excel[dic_field_db_excel["ClubManagementCommission"]].ToString(), out n_club_management_commission);

                        o_data_field.ClubManagementCommission = n_club_management_commission;
                    }

                    // ClubSaving

                    if (dic_field_db_excel.Keys.Contains("ClubSaving"))
                    {
                        double n_club_saving = 0;
                        double.TryParse(dr_excel[dic_field_db_excel["ClubSaving"]].ToString(), out n_club_saving);

                        o_data_field.ClubSaving = n_club_saving;
                    }

                    // VAT

                    if (dic_field_db_excel.Keys.Contains("VAT"))
                    {
                        double n_vat = 0;
                        double.TryParse(dr_excel[dic_field_db_excel["VAT"]].ToString(), out n_vat);

                        o_data_field.VAT = n_vat;
                    }

                    // ExchangeRate

                    if (dic_field_db_excel.Keys.Contains("ExchangeRate"))
                    {
                        double n_exchange_rate = 0;
                        double.TryParse(dr_excel[dic_field_db_excel["ExchangeRate"]].ToString(), out n_exchange_rate);

                        o_data_field.ExchangeRate = n_exchange_rate;

                        if (o_data_field.ExchangeRate < 0)
                        {
                            Row_Error_Add(String.Format("{0} lower than 0.", dic_field_db_excel["ExchangeRate"]), ref s_row_error);
                        }
                    }

                    if (o_data_field.ExchangeRate <= 0) { o_data_field.ExchangeRate = 1; }

                    // AbsorptionDate

                    if (dic_field_db_excel.Keys.Contains("AbsorptionDate"))
                    {
                        string s_date = dr_excel[dic_field_db_excel["AbsorptionDate"]].ToString().Trim();

                        DateTime d_date = new DateTime();

                        bool b_date = Common.Convert_To_Date(s_date, dic_field_format["AbsorptionDate"], ref d_date);

                        if (b_date && d_date < d_date_min)
                        {
                            b_date = false;
                        }

                        if (b_date) { o_data_field.AbsorptionDate = d_date; }
                    }

                    // PaymentDateActual

                    if (dic_field_db_excel.Keys.Contains("PaymentDateActual"))
                    {
                        string s_date = dr_excel[dic_field_db_excel["PaymentDateActual"]].ToString().Trim();

                        DateTime d_date = new DateTime();

                        bool b_date = Common.Convert_To_Date(s_date, dic_field_format["PaymentDateActual"], ref d_date);

                        if (b_date && d_date < d_date_min)
                        {
                            b_date = false;
                        }

                        if (b_date)
                        {
                            o_data_field.PaymentDateActual = d_date;
                        }
                        else
                        {
                            Row_Error_Add(String.Format("{0} not valid.", dic_field_db_excel["PaymentDateActual"]), ref s_row_error);
                        }
                    }

                    // InvoiceDate

                    if (dic_field_db_excel.Keys.Contains("InvoiceDate"))
                    {
                        string s_date = dr_excel[dic_field_db_excel["InvoiceDate"]].ToString().Trim();

                        DateTime d_date = new DateTime();

                        bool b_date = Common.Convert_To_Date(s_date, dic_field_format["InvoiceDate"], ref d_date);

                        if (b_date && d_date < d_date_min)
                        {
                            b_date = false;
                        }

                        if (b_date)
                        {
                            o_data_field.InvoiceDate = d_date;
                        }
                        else if (o_data_field.InvoiceNumber > 0)
                        {
                            Row_Error_Add(String.Format("{0} not valid.", dic_field_db_excel["InvoiceDate"]), ref s_row_error);
                        }
                    }

                    // IsAbroad

                    if (dic_field_db_excel.Keys.Contains("IsAbroad"))
                    {
                        string s_value = dr_excel[dic_field_db_excel["IsAbroad"]].ToString().Trim();

                        string[] arr_format = dic_field_format["IsAbroad"].Split(',');

                        o_data_field.IsAbroad = arr_format.Contains(s_value);
                    }

                    // int
                    dr_data["CreditID"] = o_data_field.CreditID;
                    dr_data["PaymentCurrencyID"] = o_data_field.PaymentCurrencyID;
                    dr_data["BankID"] = o_data_field.BankID;
                    dr_data["BankBranchID"] = o_data_field.BankBranchID;
                    dr_data["AccountNumber"] = o_data_field.AccountNumber;

                    // bigint
                    dr_data["InvoiceNumber"] = o_data_field.InvoiceNumber;
                    dr_data["ClubNumber"] = o_data_field.ClubNumber;

                    // float
                    dr_data["NetPaymentAmount"] = o_data_field.NetPaymentAmount;
                    dr_data["ClearingCommission"] = o_data_field.ClearingCommission;
                    dr_data["NotElectronicCommission"] = o_data_field.NotElectronicCommission;
                    dr_data["ManualCommission"] = o_data_field.ManualCommission;
                    dr_data["CancellationCommission"] = o_data_field.CancellationCommission;
                    dr_data["TelephoneCommission"] = o_data_field.TelephoneCommission;
                    dr_data["DiscountCommission"] = o_data_field.DiscountCommission;
                    dr_data["ClubManagementCommission"] = o_data_field.ClubManagementCommission;
                    dr_data["ClubSaving"] = o_data_field.ClubSaving;
                    dr_data["VAT"] = o_data_field.VAT;
                    dr_data["ExchangeRate"] = o_data_field.ExchangeRate;

                    // date

                    if (o_data_field.AbsorptionDate != null)
                    {
                        DateTime d_date = (DateTime)o_data_field.AbsorptionDate;

                        dr_data["AbsorptionDate"] = d_date.Year + "-" + d_date.Month + "-" + d_date.Day;
                    }
                    else
                    {
                        dr_data["AbsorptionDate"] = "";
                    }

                    if (o_data_field.PaymentDateActual != null)
                    {
                        DateTime d_date = (DateTime)o_data_field.PaymentDateActual;

                        dr_data["PaymentDateActual"] = d_date.Year + "-" + d_date.Month + "-" + d_date.Day;
                    }
                    else
                    {
                        dr_data["PaymentDateActual"] = "";
                    }

                    if (o_data_field.InvoiceDate != null)
                    {
                        DateTime d_date = (DateTime)o_data_field.InvoiceDate;

                        dr_data["InvoiceDate"] = d_date.Year + "-" + d_date.Month + "-" + d_date.Day;
                    }
                    else
                    {
                        dr_data["InvoiceDate"] = "";
                    }

                    // bit
                    dr_data["IsAbroad"] = o_data_field.IsAbroad;
                }

                // INSIDE ONLY : UPDATE ROW : DutyPaymentAmount / RemainingPaymentsAmount / IsSplitted // SPLIT ROW : IF PAYMENTS > 1 AND NOT DISCOUNT / DutyPaymentNumber

                DataTable dt_split = new DataTable();

                if (b_inside == true)
                {
                    if (o_data_field.PaymentsCount == 1 && o_data_field.TransactionGrossAmount != o_data_field.FirstPaymentAmount)
                    {
                        Row_Error_Add("Payments Count is equal to 1, and Gross Amount is not equal to First Payment Amount.", ref s_row_error);
                    }

                    if (o_data_field.PaymentsCount > 1)
                    {
                        if (o_data_field.TransactionGrossAmount == o_data_field.FirstPaymentAmount)
                        {
                            dr_data["DutyPaymentAmount"] = o_data_field.TransactionGrossAmount;
                            // Row_Error_Add("Payments Count is greater than 1, and Gross Amount is equal to First Payment Amount.", ref s_row_error);
                        }
                        else if (o_data_field.AgreementDiscountTerminalID == 0)
                        {
                            string s_update_row_error = "";

                            DataAction.Update_Row(ref dr_data, o_data_field.PaymentsCount, o_data_field.TransactionGrossAmount, o_data_field.FirstPaymentAmount, ref s_update_row_error);

                            if (s_update_row_error != "")
                            {
                                Row_Error_Add(s_update_row_error, ref s_row_error);
                            }

                            dt_split = dt_data.Clone();

                            // PAYMENT # 1 ALREADY EXISTS IN dr_data. BEGIN SPLIT FROM PAYMENT # 2.

                            int n_start_payment_number = 2;
                            int n_end_payment_number = o_data_field.PaymentsCount;
                            int n_remaining_payments_count = o_data_field.PaymentsCount - 1;

                            double n_remaining_payments_amount = 0;
                            double.TryParse(dr_data["RemainingPaymentsAmount"].ToString(), out n_remaining_payments_amount);

                            double n_duty_payment_amount = n_remaining_payments_amount / (double)n_remaining_payments_count;

                            // BEGIN SPLIT.

                            DataAction.Split_Row(ref dt_split, dr_data, n_start_payment_number, n_end_payment_number, n_duty_payment_amount, n_remaining_payments_amount, o_data_field.PaymentDate);

                            // SET dr_data IsSplitted.

                            dr_data["IsSplitted"] = (dt_split.Rows.Count > 0);
                        }
                    }
                }

                // ErrorMessage

                s_row_error = s_row_error.Trim();

                if (s_row_error != "") { b_valid = false; }

                dr_excel["ErrorMessage"] = s_row_error;

                // ADD Row To TableInside Or TableOutside

                dt_data.Rows.Add(dr_data);

                if (dt_split.Rows.Count > 0)
                {
                    dt_data.Merge(dt_split);
                }
            }

            // GET TableExcelShort TABLE. Contains : UniqueID / [s_field_money] - TransactionGrossAmount OR DutyPaymentAmount AS Amount

            o_data_file.TableExcelShort = new DataView(dt_excel).ToTable(false, new string[] { "UniqueID", dic_field_db_excel[s_field_money] });

            o_data_file.TableExcelShort.Columns[1].ColumnName = "Amount";

            // CHANGE dt_excel Columns Name for adapting to typExcel. (o_data_file.TableExcel is type of typExcel)

            for (int i = 0; i < dt_excel.Columns.Count; i++)
            {
                string s_column = "FIELD_" + (i + 1);
                string s_column_money = "";

                //if ((b_inside == true && dt_excel.Columns[i].ColumnName == dic_field_db_excel["TransactionGrossAmount"]) || (b_outside == true && dt_excel.Columns[i].ColumnName == dic_field_db_excel["DutyPaymentAmount"]))
                if ((b_inside == true && dt_excel.Columns[i].ColumnName == dic_field_db_excel["TransactionGrossAmount"]))
                {
                    s_column_money = s_column;
                }

                dt_excel.Columns[i].ColumnName = s_column;

                if (o_data_file.ListColumns != "") { o_data_file.ListColumns += ", "; }

                o_data_file.ListColumns += s_column;
            }

            // ADD Columns to dt_excel for adapting to typExcel. (typExcel contains [ExcelAction.MaxFieldsCount])

            for (int i = dt_excel.Columns.Count; i < ExcelAction.MaxFieldsCount; i++)
            {
                string s_column = "FIELD_" + (i + 1);
                //try
                //{
                    dt_excel.Columns.Add(s_column);
                //}
                //catch (Exception ex)
                //{
                //}
            }

            // SET o_data_file TABLES

            o_data_file.TableExcel = dt_excel;

            // !!! INSIDE & OUTSIDE TABLES CONTAINS ONLY VALID ROWS !!!

            if (b_valid)
            {
                if (b_inside)
                {
                    o_data_file.TableDataInside = dt_data;
                }
                else
                {
                    o_data_file.TableDataOutside = dt_data;
                }
            }

            o_data_file.IsValid = b_valid;
        }

        private void Row_Error_Add(string s_error, ref string s_row_error)
        {
            if (s_row_error != "") { s_row_error += " /// "; }

            s_row_error += s_error;
        }

        private void Validate_Agreement_From_Cashbox(DataTable dt_excel_cashbox, TemplateModel o_template, ref DataTable dt_agreement_item, ref string s_error)
        {
            // CompanyNumber,NetworkNumber,BranchNumber,CashBoxNumber,CardBrand,TransactionDate,PaymentsCount

            int n_company_id = 0, n_network_id = 0, n_branch_id = 0, n_cashbox_id = 0;
            string s_network_number = "", s_branch_number = "", s_cashbox_number = "";

            string s_field_format = o_template.TableTemplateField.Select(" FieldFromDB = 'TransactionDate' ")[0]["FieldFormat"].ToString();

            switch (o_template.TerminalField)
            {
                case "":
                    n_cashbox_id = Convert.ToInt32(ddlCashBox.SelectedValue);
                    break;
                // CompanyNumber
                case "CompanyNumber":
                    s_network_number = txtNetworkNumber.Text.Trim();
                    s_branch_number = txtBranchNumber.Text.Trim();
                    s_cashbox_number = txtCashBoxNumber.Text.Trim();
                    break;
                case "CompanyNumber,NetworkNumber":
                    s_branch_number = txtBranchNumber.Text.Trim();
                    s_cashbox_number = txtCashBoxNumber.Text.Trim();
                    break;
                case "CompanyNumber,BranchNumber":
                    s_network_number = txtNetworkNumber.Text.Trim();
                    s_cashbox_number = txtCashBoxNumber.Text.Trim();
                    break;
                case "CompanyNumber,CashBoxNumber":
                    s_network_number = txtNetworkNumber.Text.Trim();
                    s_branch_number = txtBranchNumber.Text.Trim();
                    break;
                case "CompanyNumber,NetworkNumber,BranchNumber":
                    s_cashbox_number = txtCashBoxNumber.Text.Trim();
                    break;
                case "CompanyNumber,NetworkNumber,CashBoxNumber":
                    s_branch_number = txtBranchNumber.Text.Trim();
                    break;
                case "CompanyNumber,BranchNumber,CashBoxNumber":
                    s_network_number = txtNetworkNumber.Text.Trim();
                    break;
                // NetworkNumber
                case "NetworkNumber":
                    n_company_id = Convert.ToInt32(ddlCompany.SelectedValue);
                    s_branch_number = txtBranchNumber.Text.Trim();
                    s_cashbox_number = txtCashBoxNumber.Text.Trim();
                    break;
                case "NetworkNumber,BranchNumber":
                    n_company_id = Convert.ToInt32(ddlCompany.SelectedValue);
                    s_cashbox_number = txtCashBoxNumber.Text.Trim();
                    break;
                case "NetworkNumber,CashBoxNumber":
                    n_company_id = Convert.ToInt32(ddlCompany.SelectedValue);
                    s_branch_number = txtBranchNumber.Text.Trim();
                    break;
                case "NetworkNumber,BranchNumber,CashBoxNumber":
                    n_company_id = Convert.ToInt32(ddlCompany.SelectedValue);
                    break;
                // BranchNumber
                case "BranchNumber":
                    n_company_id = Convert.ToInt32(ddlCompany.SelectedValue);
                    n_network_id = Convert.ToInt32(ddlNetwork.SelectedValue);
                    s_cashbox_number = txtCashBoxNumber.Text.Trim();
                    break;
                case "BranchNumber,CashBoxNumber":
                    n_company_id = Convert.ToInt32(ddlCompany.SelectedValue);
                    n_network_id = Convert.ToInt32(ddlNetwork.SelectedValue);
                    break;
                // CashBoxNumber
                case "CashBoxNumber":
                    n_company_id = Convert.ToInt32(ddlCompany.SelectedValue);
                    n_network_id = Convert.ToInt32(ddlNetwork.SelectedValue);
                    n_branch_id = Convert.ToInt32(ddlBranch.SelectedValue);
                    break;
            }

            DataTable dt_cashbox = new DataTable();

            dt_cashbox.Columns.Add("CompanyID");
            dt_cashbox.Columns.Add("CompanyNumber");
            dt_cashbox.Columns.Add("CompanyNumber_Converted");
            dt_cashbox.Columns.Add("CompanyName");

            dt_cashbox.Columns.Add("NetworkID");
            dt_cashbox.Columns.Add("NetworkNumber");
            dt_cashbox.Columns.Add("NetworkName");

            dt_cashbox.Columns.Add("BranchID");
            dt_cashbox.Columns.Add("BranchNumber");
            dt_cashbox.Columns.Add("BranchName");

            dt_cashbox.Columns.Add("CashBoxID");
            dt_cashbox.Columns.Add("CashBoxNumber");
            dt_cashbox.Columns.Add("CashBoxName");

            dt_cashbox.Columns.Add("CardID");
            dt_cashbox.Columns.Add("CardBrand");

            dt_cashbox.Columns.Add("TransactionDate");
            dt_cashbox.Columns.Add("TransactionDate_Converted");

            dt_cashbox.Columns.Add("PaymentsCount");
            dt_cashbox.Columns.Add("PaymentsCount_Converted");

            dt_cashbox.Columns.Add("Error");

            foreach (DataRow dr_excel_cashbox in dt_excel_cashbox.Rows)
            {
                string s_error_in_row = "";

                DataRow dr_cashbox = dt_cashbox.NewRow();

                foreach (DataColumn dc_excel_cashbox in dt_excel_cashbox.Columns)
                {
                    string s_column_name = dc_excel_cashbox.ColumnName;

                    dr_cashbox[s_column_name] = dr_excel_cashbox[s_column_name];
                }

                if (o_template.TerminalField == "")
                {
                    dr_cashbox["CompanyName"] = ddlCompany.SelectedItem.Text;
                    dr_cashbox["NetworkName"] = ddlNetwork.SelectedItem.Text;
                    dr_cashbox["BranchName"] = ddlBranch.SelectedItem.Text;
                    dr_cashbox["CashBoxName"] = ddlCashBox.SelectedItem.Text;

                    dr_cashbox["CashBoxID"] = n_cashbox_id;
                }

                // Company

                if (n_company_id > 0)
                {
                    dr_cashbox["CompanyID"] = n_company_id;
                    dr_cashbox["CompanyName"] = ddlCompany.SelectedItem.Text;
                }
                else if (dt_excel_cashbox.Columns.Contains("CompanyNumber"))
                {
                    long n_company_converted = 0;
                    long.TryParse(dr_excel_cashbox["CompanyNumber"].ToString().Trim(), out n_company_converted);

                    dr_cashbox["CompanyNumber_Converted"] = n_company_converted;

                    if (n_company_converted == 0)
                    {
                        if (s_error_in_row != "") { s_error_in_row += " // "; }

                        s_error_in_row += "Company not exists.";
                    }
                }

                // Network

                if (n_network_id > 0)
                {
                    dr_cashbox["NetworkID"] = n_network_id;
                    dr_cashbox["NetworkName"] = ddlNetwork.SelectedItem.Text;
                }
                else if (dt_excel_cashbox.Columns.Contains("NetworkNumber"))
                {
                    string s_network_converted = dr_excel_cashbox["NetworkNumber"].ToString().Trim();

                    if (s_network_converted == "")
                    {
                        if (s_error_in_row != "") { s_error_in_row += " // "; }

                        s_error_in_row += "Network not exists.";
                    }
                }

                // Branch

                if (n_branch_id > 0)
                {
                    dr_cashbox["BranchID"] = n_branch_id;
                    dr_cashbox["BranchName"] = ddlBranch.SelectedItem.Text;
                }
                else if (dt_excel_cashbox.Columns.Contains("BranchNumber"))
                {
                    string s_branch_converted = dr_excel_cashbox["BranchNumber"].ToString().Trim();

                    if (s_branch_converted == "")
                    {
                        if (s_error_in_row != "") { s_error_in_row += " // "; }

                        s_error_in_row += "Branch not exists.";
                    }
                }

                // CashBox

                if (n_cashbox_id > 0)
                {
                    dr_cashbox["CashBoxID"] = n_cashbox_id;
                    dr_cashbox["CashBoxName"] = ddlCashBox.SelectedItem.Text;
                }
                else if (dt_excel_cashbox.Columns.Contains("CashBoxNumber"))
                {
                    string s_cashbox_converted = dr_excel_cashbox["CashBoxNumber"].ToString().Trim();

                    if (s_cashbox_converted == "")
                    {
                        if (s_error_in_row != "") { s_error_in_row += " // "; }

                        s_error_in_row += "Cashbox not exists.";
                    }
                }

                if (s_network_number != "") { dr_cashbox["NetworkNumber"] = s_network_number; }
                if (s_branch_number != "") { dr_cashbox["BranchNumber"] = s_branch_number; }
                if (s_cashbox_number != "") { dr_cashbox["CashBoxNumber"] = s_cashbox_number; }

                // CardBrand

                int n_card_id = 0;

                if (dt_excel_cashbox.Columns.Contains("CardBrand"))
                {
                    string s_card_brand = ExcelAction.Valid_Field_Value(dr_excel_cashbox["CardBrand"].ToString());

                    if (s_card_brand != "")
                    {
                        DataRow[] dr_template_card = o_template.TableTemplateCard.Select(" CardFromUser = '" + s_card_brand + "' ");

                        if (dr_template_card.Length > 0)
                        {
                            int.TryParse(dr_template_card[0]["CardID"].ToString(), out n_card_id);
                        }
                    }
                }

                if (n_card_id == 0 && chkStopIfCardIsEmpty.Checked == true)
                {
                    if (s_error_in_row != "") { s_error_in_row += " // "; }

                    s_error_in_row += "Card not exists or not valid.";
                }

                dr_cashbox["CardID"] = n_card_id;

                // TransactionDate - REQUIRED IN INSIDE.

                string s_date = "";

                if (dt_excel_cashbox.Columns.Contains("TransactionDate"))
                {
                    s_date = dr_excel_cashbox["TransactionDate"].ToString().Trim();

                    DateTime d_date = new DateTime();

                    bool b_date = Common.Convert_To_Date(s_date, s_field_format, ref d_date);

                    if (b_date && d_date < d_date_min)
                    {
                        b_date = false;
                    }

                    if (b_date) { s_date = d_date.Year + "-" + d_date.Month + "-" + d_date.Day; } else { s_date = ""; }
                }

                if (s_date == "")
                {
                    if (s_error_in_row != "") { s_error_in_row += " // "; }

                    s_error_in_row += "Date not valid.";
                }

                dr_cashbox["TransactionDate_Converted"] = s_date;

                // PaymentsCount

                int n_payments = 0;

                if (dt_excel_cashbox.Columns.Contains("PaymentsCount")) { int.TryParse(dr_excel_cashbox["PaymentsCount"].ToString(), out n_payments); }

                if (n_payments < 1) { n_payments = 1; }

                dr_cashbox["PaymentsCount_Converted"] = n_payments;

                // Error

                dr_cashbox["Error"] = s_error_in_row;

                dt_cashbox.Rows.Add(dr_cashbox);
            }

            if (dt_cashbox.Select(" Error <> '' ").Length > 0)
            {
                DataView dv_cashbox = dt_cashbox.DefaultView;

                dv_cashbox.Sort = "Error DESC";

                s_error = "Can't validate related agreement items from cashbox. Some data is wrong. Please see the following table.";

                divAgreementFromCashBox.Visible = true;
                repAgreementFromCashBox.DataSource = dv_cashbox.ToTable();
                repAgreementFromCashBox.DataBind();

                return;
            }

            dt_cashbox.Columns.Remove("Error");

            SqlCommand o_command = new SqlCommand("sprAgreement_From_Cashbox", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure, CommandTimeout = 0 };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });

            o_command.Parameters.AddWithValue("@TableAgreementFromCashbox", dt_cashbox);
            o_command.Parameters["@TableAgreementFromCashbox"].SqlDbType = SqlDbType.Structured;

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);

            try
            {
                o_data_adapter.Fill(dt_agreement_item);
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on validate agreement items from cashbox.";
                return;
            }
            finally
            {
                o_data_adapter.Dispose();
                o_command.Dispose();
            }

            if (chkStopIfAgreementNotExists.Checked == true && dt_agreement_item.Select("Error <> ''").Length > 0)
            {
                DataView dv_agreement_item = dt_agreement_item.DefaultView;

                dv_agreement_item.Sort = "Error DESC";

                s_error = "Error on validate related agreement items from cashbox. Some data is wrong. Please see the following table.";

                divAgreementFromCashBox.Visible = true;
                repAgreementFromCashBox.DataSource = dv_agreement_item.ToTable();
                repAgreementFromCashBox.DataBind();
            }
        }

        private void Validate_Agreement_From_Terminal(DataTable dt_excel_terminal, TemplateModel o_template, ref DataTable dt_agreement_item, ref string s_error)
        {
            // TerminalNumber,SupplierNumber,CardBrand,CreditBrand,OperationType,TransactionDate,PaymentsCount

            string s_field_format = o_template.TableTemplateField.Select(" FieldFromDB = 'TransactionDate' ")[0]["FieldFormat"].ToString();

            int n_company_id = Convert.ToInt32(ddlCompany.SelectedValue);

            DataTable dt_terminal = new DataTable();

            dt_terminal.Columns.Add("CompanyID");

            dt_terminal.Columns.Add("TerminalNumber");
            dt_terminal.Columns.Add("TerminalNumber_Converted");

            dt_terminal.Columns.Add("SupplierNumber");
            dt_terminal.Columns.Add("SupplierNumber_Converted");

            dt_terminal.Columns.Add("CardID");
            dt_terminal.Columns.Add("CardBrand");

            dt_terminal.Columns.Add("CreditID");
            dt_terminal.Columns.Add("CreditBrand");

            dt_terminal.Columns.Add("OperationTypeID");
            dt_terminal.Columns.Add("OperationType");

            dt_terminal.Columns.Add("TransactionDate");
            dt_terminal.Columns.Add("TransactionDate_Converted");

            dt_terminal.Columns.Add("PaymentsCount");
            dt_terminal.Columns.Add("PaymentsCount_Converted");

            dt_terminal.Columns.Add("Error");

            foreach (DataRow dr_excel_terminal in dt_excel_terminal.Rows)
            {
                string s_error_in_row = "";

                DataRow dr_terminal = dt_terminal.NewRow();

                foreach (DataColumn dc_excel_terminal in dt_excel_terminal.Columns)
                {
                    string s_column_name = dc_excel_terminal.ColumnName;

                    dr_terminal[s_column_name] = dr_excel_terminal[s_column_name];
                }

                dr_terminal["CompanyID"] = n_company_id;

                // TerminalNumber

                long n_terminal_converted = 0;

                if (dt_excel_terminal.Columns.Contains("TerminalNumber"))
                {
                    long.TryParse(dr_excel_terminal["TerminalNumber"].ToString(), out n_terminal_converted);

                    if (n_terminal_converted == 0 && chkStopIfTerminalIsZero.Checked == true)
                    {
                        if (s_error_in_row != "") { s_error_in_row += " // "; }

                        s_error_in_row += "Terminal is zero or is empty or not is numeric.";
                    }
                }

                dr_terminal["TerminalNumber_Converted"] = n_terminal_converted;

                // SupplierNumber

                long n_supplier_converted = 0;

                if (dt_excel_terminal.Columns.Contains("SupplierNumber"))
                {
                    long.TryParse(dr_excel_terminal["SupplierNumber"].ToString(), out n_supplier_converted);

                    if (n_supplier_converted == 0 && chkStopIfSupplierIsZero.Checked == true)
                    {
                        if (s_error_in_row != "") { s_error_in_row += " // "; }

                        s_error_in_row += "Supplier is zero or is empty or not is numeric.";
                    }
                }

                dr_terminal["SupplierNumber_Converted"] = n_supplier_converted;

                // CardBrand

                int n_card_id = 0;

                if (dt_excel_terminal.Columns.Contains("CardBrand"))
                {
                    string s_card_brand = ExcelAction.Valid_Field_Value(dr_excel_terminal["CardBrand"].ToString());

                    if (s_card_brand != "")
                    {
                        DataRow[] dr_template_card = o_template.TableTemplateCard.Select(" CardFromUser = '" + s_card_brand + "' ");

                        if (dr_template_card.Length > 0)
                        {
                            int.TryParse(dr_template_card[0]["CardID"].ToString(), out n_card_id);
                        }
                    }
                }
                else if (divCard_Change.Visible == true)
                {
                    n_card_id = Convert.ToInt32(ddlCard.SelectedValue);
                }

                if (n_card_id == 0 && chkStopIfCardIsEmpty.Checked == true)
                {
                    if (s_error_in_row != "") { s_error_in_row += " // "; }

                    s_error_in_row += "Card not exists or not valid.";
                }

                dr_terminal["CardID"] = n_card_id;

                // CreditBrand

                int n_credit_id = 0;

                if (o_template.CreditID > 0)
                {
                    n_credit_id = o_template.CreditID;
                }
                else if (o_template.DiscountID > 0)
                {
                    if (dt_excel_terminal.Columns.Contains("CreditBrand"))
                    {
                        string s_credit_brand = ExcelAction.Valid_Field_Value(dr_excel_terminal["CreditBrand"].ToString());

                        if (s_credit_brand != "")
                        {
                            DataRow[] dr_template_credit = o_template.TableTemplateCredit.Select(" CreditFromUser = '" + s_credit_brand + "' ");

                            if (dr_template_credit.Length > 0)
                            {
                                int.TryParse(dr_template_credit[0]["CreditID"].ToString(), out n_credit_id);
                            }
                        }
                    }

                    if (n_credit_id == 0)
                    {
                        if (s_error_in_row != "") { s_error_in_row += " // "; }

                        s_error_in_row += "Credit not exists.";
                    }
                }

                dr_terminal["CreditID"] = n_credit_id;

                // OperationType

                int n_operation_type_id = 0;

                if (dt_excel_terminal.Columns.Contains("OperationType"))
                {
                    string s_operation_type = ExcelAction.Valid_Field_Value(dr_excel_terminal["OperationType"].ToString());

                    if (s_operation_type != "")
                    {
                        DataRow[] dr_template_operation_type = o_template.TableTemplateOperationType.Select(" OperationTypeFromUser = '" + s_operation_type + "' ");

                        if (dr_template_operation_type.Length > 0)
                        {
                            int.TryParse(dr_template_operation_type[0]["OperationTypeID"].ToString(), out n_operation_type_id);
                        }
                    }

                    if (n_operation_type_id == 0)
                    {
                        if (s_error_in_row != "") { s_error_in_row += " // "; }

                        s_error_in_row += "Operation Type not exists.";
                    }
                }

                if (n_operation_type_id == 0) { n_operation_type_id = 1; }      // אשראי רגיל

                dr_terminal["OperationTypeID"] = n_operation_type_id;

                // TransactionDate - NOT REQUIRED IN OUTSIDE ( MAYBE EMPTY IN COMMISSION TRANSACTIONS ). IGNORE ROW IF NOT EXISTS

                string s_transaction_date = "";

                if (dt_excel_terminal.Columns.Contains("TransactionDate"))
                {
                    s_transaction_date = dr_excel_terminal["TransactionDate"].ToString().Trim();

                    DateTime d_date = new DateTime();

                    bool b_date = Common.Convert_To_Date(s_transaction_date, s_field_format, ref d_date);

                    if (b_date && d_date < d_date_min)
                    {
                        b_date = false;
                    }

                    if (b_date) { s_transaction_date = d_date.Year + "-" + d_date.Month + "-" + d_date.Day; } else { s_transaction_date = ""; }
                }

                dr_terminal["TransactionDate_Converted"] = s_transaction_date;

                // PaymentsCount

                int n_payments_count = 0;

                if (dt_excel_terminal.Columns.Contains("PaymentsCount"))
                {
                    string s_payments_count = dr_excel_terminal["PaymentsCount"].ToString().Trim();

                    char c_separator = (char)0;

                    if (s_payments_count.Contains("/"))
                    {
                        c_separator = '/';
                    }
                    else if (s_payments_count.Contains("-"))
                    {
                        c_separator = '-';
                    }

                    if (c_separator == (char)0)
                    {
                        int.TryParse(s_payments_count, out n_payments_count);
                    }
                    else
                    {
                        string[] arr_payments_count = s_payments_count.Split(c_separator);

                        n_payments_count = Convert.ToInt32(arr_payments_count[1]);
                    }
                }

                if (n_payments_count < 1) { n_payments_count = 1; }

                dr_terminal["PaymentsCount_Converted"] = n_payments_count;

                // Error

                dr_terminal["Error"] = s_error_in_row;

                // IGNORE ROW IF TransactionDate NOT EXISTS

                if (s_transaction_date != "")
                {
                    dt_terminal.Rows.Add(dr_terminal);
                }
            }

            //rep.DataSource = dt_terminal;
            //rep.DataBind();

            if (dt_terminal.Select("Error <> ''").Length > 0)
            {
                DataView dv_terminal = dt_terminal.DefaultView;

                dv_terminal.Sort = "Error DESC";

                s_error = "Can't validate related agreement items from terminal. Some data is wrong. Please see the following table.";

                divAgreementFromTerminal.Visible = true;
                repAgreementFromTerminal.DataSource = dv_terminal.ToTable();
                repAgreementFromTerminal.DataBind();

                return;
            }

            dt_terminal.Columns.Remove("Error");

            SqlCommand o_command = new SqlCommand("sprAgreement_From_Terminal", DB.Get_Connection()) { CommandType = CommandType.StoredProcedure, CommandTimeout = 0 };

            o_command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.Int) { Value = n_user_id });
            o_command.Parameters.Add(new SqlParameter("@StopIfTerminalNotExists", SqlDbType.Bit) { Value = chkStopIfTerminalNotExists.Checked });
            o_command.Parameters.Add(new SqlParameter("@StopIfSupplierNotExists", SqlDbType.Bit) { Value = chkStopIfSupplierNotExists.Checked });
            o_command.Parameters.Add(new SqlParameter("@StopIfAgreementNotExists", SqlDbType.Bit) { Value = chkStopIfAgreementNotExists.Checked });

            o_command.Parameters.AddWithValue("@TableAgreementFromTerminal", dt_terminal);
            o_command.Parameters["@TableAgreementFromTerminal"].SqlDbType = SqlDbType.Structured;

            SqlDataAdapter o_data_adapter = new SqlDataAdapter(o_command);

            try
            {
                o_data_adapter.Fill(dt_agreement_item);
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
                s_error = "Error on validate agreement items from terminal.";
                return;
            }
            finally
            {
                o_data_adapter.Dispose();
                o_command.Dispose();
            }

            if (dt_agreement_item.Select("Error <> ''").Length > 0)
            {
                DataView dv_agreement_item = dt_agreement_item.DefaultView;

                dv_agreement_item.Sort = "Error DESC";

                s_error = "Error on validate related agreement items from terminal. Some data is wrong. Please see the following table.";

                divAgreementFromTerminal.Visible = true;
                repAgreementFromTerminal.DataSource = dv_agreement_item.ToTable();
                repAgreementFromTerminal.DataBind();
            }
        }

        private void Validate_Date_Range(DataTable dt_excel, TemplateModel o_template, bool b_inside, ref DataFileModel o_data_file, ref string s_error)
        {
            string s_field_from_db = b_inside ? "TransactionDate" : "PaymentDate";

            DataRow o_data_row = o_template.TableTemplateField.Select(" FieldFromDB = '" + s_field_from_db + "' ").First();

            string s_field_from_excel = String.Format("[{0}]", o_data_row["FieldFromExcel"].ToString());
            string s_field_format = o_data_row["FieldFormat"].ToString();

            DataTable dt_excel_check_date = new DataView(dt_excel).ToTable(true, s_field_from_excel);

            // Get min & max date of data file

            DateTime? d_date_from = null, d_date_to = null;

            foreach (DataRow dr_excel_check_date in dt_excel_check_date.Rows)
            {
                DateTime d_date = new DateTime();

                string s_date = dr_excel_check_date[0].ToString();

                bool b_date = Common.Convert_To_Date(s_date, s_field_format, ref d_date);

                if (b_date)
                {
                    if ((d_date_from == null) || (d_date_from > d_date)) { d_date_from = d_date; }
                    if ((d_date_to == null) || (d_date_to < d_date)) { d_date_to = d_date; }
                }
            }

            if (d_date_from == null || d_date_to == null)
            {
                s_error = "Can't load data file without valid date column.";
                return;
            }

            // If auto check dates

            if (o_data_file.DateFrom == null || o_data_file.DateTo == null)
            {
                o_data_file.DateFrom = d_date_from;
                o_data_file.DateTo = d_date_to;
            }
            else
            {
                if (d_date_from < o_data_file.DateFrom || d_date_to > o_data_file.DateTo)
                {
                    s_error = "Some dates in data file is out of range.";
                    return;
                }
            }

            // Check date range of data file

            string s_result = DataFileAction.Check(ref o_data_file);

            if (o_data_file.ErrorMessage != "")
            {
                s_error = o_data_file.ErrorMessage;
                return;
            }

            if (s_result != "OK")
            {
                s_error = s_result;
                return;
            }
        }

        private void Validate_Currency(DataTable dt_excel_currency, TemplateModel o_template, ref string s_error)
        {
            for (int i_row = dt_excel_currency.Rows.Count - 1; i_row >= 0; i_row--)
            {
                string s_currency = dt_excel_currency.Rows[i_row][0].ToString().Trim();

                if ((s_currency == "") || (o_template.TableTemplateCurrency.Select(" CurrencyFromUser = '" + s_currency + "' ").Length > 0))
                {
                    dt_excel_currency.Rows.RemoveAt(i_row);
                }
            }

            if (dt_excel_currency.Rows.Count > 0)
            {
                if (s_error != "") { s_error += "<br />"; }

                s_error = "Select currency.";

                divCurrency.Visible = true;
                repCurrency.DataSource = dt_excel_currency;
                repCurrency.DataBind();
            }
            else
            {
                Hide_Currency();
            }
        }

        private void Validate_Credit(DataTable dt_excel_credit, TemplateModel o_template, ref string s_error)
        {
            for (int i_row = dt_excel_credit.Rows.Count - 1; i_row >= 0; i_row--)
            {
                string s_credit = dt_excel_credit.Rows[i_row][0].ToString().Trim();

                if ((s_credit == "") || (o_template.TableTemplateCredit.Select(" CreditFromUser = '" + s_credit + "' ").Length > 0))
                {
                    dt_excel_credit.Rows.RemoveAt(i_row);
                }
            }

            if (dt_excel_credit.Rows.Count > 0)
            {
                if (s_error != "") { s_error += "<br />"; }

                s_error += "Select credit.";

                divCredit.Visible = true;
                repCredit.DataSource = dt_excel_credit;
                repCredit.DataBind();
            }
            else
            {
                Hide_Credit();
            }
        }

        private void Validate_Card(DataTable dt_excel_card, TemplateModel o_template, ref string s_error)
        {
            for (int i_row = dt_excel_card.Rows.Count - 1; i_row >= 0; i_row--)
            {
                string s_card = dt_excel_card.Rows[i_row][0].ToString().Trim();

                if ((s_card == "") || (o_template.TableTemplateCard.Select(" CardFromUser = '" + s_card + "' ").Length > 0))
                {
                    dt_excel_card.Rows.RemoveAt(i_row);
                }
            }

            if (dt_excel_card.Rows.Count > 0)
            {
                if (s_error != "") { s_error += "<br />"; }

                s_error += "Select card.";

                divCard.Visible = true;
                repCard.DataSource = dt_excel_card;
                repCard.DataBind();
            }
            else
            {
                Hide_Card();
            }
        }

        private void Validate_Operation_Type(DataTable dt_excel_operation_type, TemplateModel o_template, ref string s_error)
        {
            for (int i_row = dt_excel_operation_type.Rows.Count - 1; i_row >= 0; i_row--)
            {
                string s_operation_type = dt_excel_operation_type.Rows[i_row][0].ToString().Trim();

                if ((s_operation_type == "") || (o_template.TableTemplateOperationType.Select(" OperationTypeFromUser = '" + s_operation_type + "' ").Length > 0))
                {
                    dt_excel_operation_type.Rows.RemoveAt(i_row);
                }
            }

            if (dt_excel_operation_type.Rows.Count > 0)
            {
                if (s_error != "") { s_error += "<br />"; }

                s_error += "Select operation type.";

                divOperationType.Visible = true;
                repOperationType.DataSource = dt_excel_operation_type;
                repOperationType.DataBind();
            }
            else
            {
                Hide_Operation_Type();
            }
        }

        private void Hide_CashBox()
        {
            txtCashBoxNumber.Text = "";
            txtBranchNumber.Text = "";
            txtNetworkNumber.Text = "";

            if (ddlCompany.SelectedIndex > 0)
            {
                ddlCompany.SelectedIndex = 0;
                ddlCompany_SelectedIndexChanged(null, null);
            }

            divCashBoxNumber_Change.Visible = false;
            divCashBox_Change.Visible = false;
            divBranchNumber_Change.Visible = false;
            divBranch_Change.Visible = false;
            divNetworkNumber_Change.Visible = false;
            divNetwork_Change.Visible = false;

            divCashBox.Visible = false;
        }

        private void Hide_Currency()
        {
            divCurrency.Visible = false;
            repCurrency.DataSource = null;
            repCurrency.DataBind();
        }

        private void Hide_Credit()
        {
            divCredit.Visible = false;
            repCredit.DataSource = null;
            repCredit.DataBind();
        }

        private void Hide_Card()
        {
            divCard.Visible = false;
            repCard.DataSource = null;
            repCard.DataBind();
        }

        private void Hide_Operation_Type()
        {
            divOperationType.Visible = false;
            repOperationType.DataSource = null;
            repOperationType.DataBind();
        }

        private int Get_BankID(DataTable dt_bank_branch, long n_bank_number, ref string s_error)
        {
            int n_bank_id = 0;

            DataRow[] dr_bank_branch = dt_bank_branch.Select(" BankNumber = " + n_bank_number);

            if (dr_bank_branch.Length > 0)
            {
                int.TryParse(dr_bank_branch[0]["BankID"].ToString(), out n_bank_id);
            }
            else
            {
                s_error = "not-exists";
            }

            return n_bank_id;
        }

        private int Get_BankBranchID(DataTable dt_bank_branch, long n_bank_number, long n_bank_branch_number, ref string s_error)
        {
            int n_bank_branch_id = 0;

            DataRow[] dr_bank_branch = dt_bank_branch.Select(" BankNumber = " + n_bank_number + " AND BranchNumber = " + n_bank_branch_number);

            if (dr_bank_branch.Length > 0)
            {
                int.TryParse(dr_bank_branch[0]["BranchID"].ToString(), out n_bank_branch_id);
            }
            else
            {
                s_error = "not-exists";
            }

            return n_bank_branch_id;
        }

        private void Bind_Form(DataFileModel o_data_file)
        {
            string s_error = "";

            // Bind Lookup Tables

            DataTable dt_currency = new DataTable();
            DataTable dt_credit = new DataTable();
            DataTable dt_card = new DataTable();
            DataTable dt_operation_type = new DataTable();

            DB.Bind_Data_Table("sprUserCurrency", ref dt_currency, ref s_error, "@UserID", n_user_id.ToString());

            if (chkStopIfAgreementNotExists.Checked == true)
            {
                DB.Bind_Data_Table("sprUserCredit", ref dt_credit, ref s_error, "@UserID", n_user_id.ToString());
                DB.Bind_Data_Table("sprUserCard", ref dt_card, ref s_error, "@UserID", n_user_id.ToString());
            }
            else
            {
                DB.Bind_Data_Table("sprCredit", ref dt_credit, ref s_error);
                DB.Bind_Data_Table("sprCard", ref dt_card, ref s_error);
            }

            DB.Bind_Data_Table("sprOperationType", ref dt_operation_type, ref s_error);

            ViewState["TableCurrency"] = dt_currency;
            ViewState["TableCredit"] = dt_credit;
            ViewState["TableCard"] = dt_card;
            ViewState["TableOperationType"] = dt_operation_type;

            if (s_error != "")
            {
                o_data_file.ErrorMessage = s_error;
                return;
            }

            // Bind ddlTemplate

            DataTable dt_template = new DataTable();

            DB.Bind_Data_Table("sprTemplate", ref dt_template, ref s_error, "@UserID", n_user_id.ToString());

            if (s_error != "")
            {
                o_data_file.ErrorMessage = s_error;
                return;
            }

            ddlTemplate.DataSource = dt_template;
            ddlTemplate.DataValueField = "ID";
            ddlTemplate.DataTextField = "TemplateName";
            ddlTemplate.DataBind();

            ddlTemplate.Items.Insert(0, new ListItem("", "0"));

            ViewState["TableTemplate"] = dt_template;

            // GET Template Field List

            DataTable dt_template_field = new DataTable();

            DB.Bind_Data_Table("sprTemplateFieldList", ref dt_template_field, ref s_error, "@UserID", n_user_id.ToString());

            if (s_error != "")
            {
                o_data_file.ErrorMessage = s_error;
                return;
            }

            ViewState["TableTemplateField"] = dt_template_field;

            // Bind ddlCard

            ddlCard.DataSource = dt_card;
            ddlCard.DataValueField = "ID";
            ddlCard.DataTextField = "CardName";
            ddlCard.DataBind();

            ddlCard.Items.Insert(0, new ListItem("", "0"));

            // Bind ddlCompany

            List<SqlParameter> o_company_parameters = new List<SqlParameter>();
            o_company_parameters.Add(new SqlParameter("@UserID", n_user_id));

            DB.Bind_List_Control(ddlCompany, "sprCompany", ref s_error, null, o_company_parameters);

            if (s_error != "")
            {
                o_data_file.ErrorMessage = s_error;
                return;
            }

            // Bind Dates

            for (int i = 1; i <= 31; i++)
            {
                ddlDayFrom.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlDayTo.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            for (int i = 1; i <= 12; i++)
            {
                ddlMonthFrom.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlMonthTo.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            int n_year = DateTime.Now.Year;

            for (int i = n_year; i >= n_year - 15; i--)
            {
                ddlYearFrom.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlYearTo.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            if (o_data_file.ID == 0) { return; }

            lstDateMode.SelectedValue = "range";

            if (o_data_file.IsValid)
            {
                lblStatusTop.Text = "Valid";
                lblStatusTop.ForeColor = Color.Green;
            }
            else
            {
                lblStatusTop.Text = "Not Valid";
                lblStatusTop.ForeColor = Color.Red;
            }

            lblStatusBottom.Text = lblStatusTop.Text;
            lblStatusBottom.ForeColor = lblStatusTop.ForeColor;

            ddlDayFrom.SelectedValue = o_data_file.DateFrom.Value.Day.ToString();
            ddlMonthFrom.SelectedValue = o_data_file.DateFrom.Value.Month.ToString();
            ddlYearFrom.SelectedValue = o_data_file.DateFrom.Value.Year.ToString();

            ddlDayTo.SelectedValue = o_data_file.DateTo.Value.Day.ToString();
            ddlMonthTo.SelectedValue = o_data_file.DateTo.Value.Month.ToString();
            ddlYearTo.SelectedValue = o_data_file.DateTo.Value.Year.ToString();

            ddlTemplate.SelectedValue = o_data_file.TemplateID.ToString();
            ddlCard.SelectedValue = o_data_file.CardID.ToString();

            txtHeaderRowsCount.Text = o_data_file.HeaderRowsCount.ToString();

            lblDataRowsCount.Text = String.Format("{0:n0}", o_data_file.DataRowsCount);
            lblSplittedRowsCount.Text = String.Format("{0:n0}", o_data_file.SplittedRowsCount);
            lblExcelRowsCount.Text = String.Format("{0:n0}", o_data_file.ExcelRowsCount);
            lblExcludedRowsCount.Text = String.Format("{0:n0}", o_data_file.ExcludedRowsCount);

            lblDataMoneyAmount.Text = String.Format("{0:n2}", o_data_file.DataMoneyAmount);
            lblExcelMoneyAmount.Text = String.Format("{0:n2}", o_data_file.ExcelMoneyAmount);
            lblExcludedMoneyAmount.Text = String.Format("{0:n2}", o_data_file.ExcludedMoneyAmount);

            lblDateUpload.Text = String.Format("{0:dd/MM/yyyy}", o_data_file.DateUpload);
            lblTimeUpload.Text = String.Format("{0:hh\\:mm\\:ss\\:ff}", o_data_file.TimeUpload);
            lblExecutionDuration.Text = String.Format("{0:hh\\:mm\\:ss\\:ff}", o_data_file.ExecutionDuration);

            divFileName.Visible = true;
            lblFileName.Text = o_data_file.FileName;

            ddlTemplate_SelectedIndexChanged(null, null);

            if (o_data_file.CompanyID > 0)
            {
                List<SqlParameter> o_network_parameters = new List<SqlParameter>();
                o_network_parameters.Add(new SqlParameter("@UserID", n_user_id));
                o_network_parameters.Add(new SqlParameter("@CompanyID", o_data_file.CompanyID));

                DB.Bind_List_Control(ddlNetwork, "sprNetwork", ref s_error, null, o_network_parameters);
            }

            if (o_data_file.NetworkID > 0)
            {
                List<SqlParameter> o_branch_parameters = new List<SqlParameter>();
                o_branch_parameters.Add(new SqlParameter("@UserID", n_user_id));
                o_branch_parameters.Add(new SqlParameter("@NetworkID", o_data_file.NetworkID));

                DB.Bind_List_Control(ddlBranch, "sprBranch", ref s_error, null, o_branch_parameters);
            }

            if (o_data_file.BranchID > 0)
            {
                List<SqlParameter> o_cashbox_parameters = new List<SqlParameter>();
                o_cashbox_parameters.Add(new SqlParameter("@UserID", n_user_id));
                o_cashbox_parameters.Add(new SqlParameter("@BranchID", o_data_file.BranchID));

                DB.Bind_List_Control(ddlCashBox, "sprCashBox", ref s_error, null, o_cashbox_parameters);
            }

            if (divCompany_Change.Visible == true) { ddlCompany.SelectedValue = o_data_file.CompanyID.ToString(); }
            if (divNetwork_Change.Visible == true) { ddlNetwork.SelectedValue = o_data_file.NetworkID.ToString(); }
            if (divBranch_Change.Visible == true) { ddlBranch.SelectedValue = o_data_file.BranchID.ToString(); }
            if (divCashBox_Change.Visible == true) { ddlCashBox.SelectedValue = o_data_file.CashBoxID.ToString(); }

            if (divNetworkNumber_Change.Visible == true) { txtNetworkNumber.Text = o_data_file.NetworkNumber; }
            if (divBranchNumber_Change.Visible == true) { txtBranchNumber.Text = o_data_file.BranchNumber; }
            if (divCashBoxNumber_Change.Visible == true) { txtCashBoxNumber.Text = o_data_file.CashBoxNumber; }

            if (trStopIfCardIsEmpty.Visible == true) { chkStopIfCardIsEmpty.Checked = o_data_file.StopIfCardIsEmpty; }
            if (trStopIfTerminalIsZero.Visible == true) { chkStopIfTerminalIsZero.Checked = o_data_file.StopIfTerminalIsZero; }
            if (trStopIfSupplierIsZero.Visible == true) { chkStopIfSupplierIsZero.Checked = o_data_file.StopIfSupplierIsZero; }
            if (trStopIfTerminalNotExists.Visible == true) { chkStopIfTerminalNotExists.Checked = o_data_file.StopIfTerminalNotExists; }
            if (trStopIfSupplierNotExists.Visible == true) { chkStopIfSupplierNotExists.Checked = o_data_file.StopIfSupplierNotExists; }
            if (trStopIfAgreementNotExists.Visible == true) { chkStopIfAgreementNotExists.Checked = o_data_file.StopIfAgreementNotExists; }
        }
    }
}
