using ClosedXML.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public partial class DataInspector : BasePage
    {
        private int n_user_id = 0;

        private string s_cache_inside = "";
        private string s_cache_outside = "";

        private string s_cache_inside_match = "";
        private string s_cache_outside_match = "";

        private string[] arr_payment_exclude_columns = { "IsSelected", "ID", "StatusName", "UniqueID", "TemplateName", "DataFileID", "StrategyName", "MatchingID", "MatchingActionName", "QueryNumber", "MatchingTypeName", "QueryID" };

        private char c_range_separator = '+';

        private const int PAGE_SIZE = 20;

        protected void Page_Load(object sender, EventArgs e)
        {
            divMatchingBalanceRow.Visible = true;

            string strInsideAmount = lblInsideAmount.Text;
            string strOutsideAmount = lblOutsideAmount.Text;
            if (!string.IsNullOrEmpty(strOutsideAmount) && !string.IsNullOrEmpty(strInsideAmount))
            {
                decimal val = Convert.ToDecimal(strOutsideAmount) - Convert.ToDecimal(strInsideAmount);
                try
                {
                    if (val == 0)
                    {
                        divMatchingBalanceRow.Visible = false;
                    }
                }
                catch (Exception ex)
                { }
            }
            n_user_id = o_user.ID;
            hdnUserId.Value = Convert.ToString(n_user_id);
            s_cache_inside = String.Format("TableInside_{0}", n_user_id);
            s_cache_outside = String.Format("TableOutside_{0}", n_user_id);

            s_cache_inside_match = String.Format("TableInside_Match_{0}", n_user_id);
            s_cache_outside_match = String.Format("TableOutside_Match_{0}", n_user_id);

            if (gvInside.Rows.Count == 0 && gvOutside.Rows.Count == 0) { secSearch.Style["display"] = "block"; }    // DISPLAY SEARCH FORM

            if (Page.IsPostBack) { return; }
            if (!Page.IsPostBack)
            {
                Session["sortColumnName_Inside"] = "";
                Session["hdnOrderSort_Inside"] = "";
                //Session["hdnTableType_Inside"] = "";
                Session["sortColumnName_Outside"] = "";
                Session["hdnOrderSort_Outside"] = "";
                //Session["hdnTableType_Outside"] = "";
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "localStorage", "<script type=\"text/javascript\"  language=\"javascript\">localStorage.isLoading=\"false\";</script>");
            }
            Bind_Search();
            Bind_Status_Change();

        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            divMessage.Visible = lblMessage.Text != "" || lblError.Text != "";

            if (lblError.Text != "") { lblMessage.Text = ""; }
        }

        protected void ddlTransactions_SelectedIndexChanged(object sender, EventArgs e)
        {
            cblStatus.SelectedIndex = -1;
            cblMatchingType.SelectedIndex = -1;
            cblMatchingAction.SelectedIndex = -1;
            cblStrategy.SelectedIndex = -1;
            cblMatching.SelectedIndex = -1;

            divStatus.Visible = false;
            divMatchingType.Visible = false;
            divMatchingAction.Visible = false;
            divStrategy.Visible = false;
            divMatching.Visible = false;
            divMatchingDate.Visible = false;

            txtMatchingDate.Text = "";
            txtMatchingDateOutside.Text = "";
            lblMatchingDateError.Text = "";
            chk_txtMatchingDateOutside.Checked = true;
            chkExcludeMatchingDate.Checked = false;

            if (ddlTransactions.SelectedValue == "all") { return; }

            switch (ddlTransactions.SelectedValue)
            {
                case "matching":
                    divMatchingType.Visible = true;
                    divMatchingAction.Visible = true;
                    divStrategy.Visible = true;
                    divMatching.Visible = true;
                    divMatchingDate.Visible = true;
                    break;
                case "not-matching":
                    divStatus.Visible = true;
                    break;
            }
        }

        protected void cblMatchingAction_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListItem itm_auto_matching = cblMatchingAction.Items.Cast<ListItem>().Where(i => i.Selected == true && i.Value == "1").FirstOrDefault();

            if (itm_auto_matching != null || cblMatchingAction.SelectedIndex == -1)
            {
                divStrategy.Visible = true;
                divMatching.Visible = true;
            }
            else
            {
                cblStrategy.SelectedIndex = -1;
                cblMatching.SelectedIndex = -1;

                divStrategy.Visible = false;
                divMatching.Visible = false;
            }
        }

        protected void cblStrategy_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<int> lst_strategy_id = new List<int>();

            foreach (ListItem o_item in cblStrategy.Items.Cast<ListItem>().Where(i => i.Selected == true))
            {
                lst_strategy_id.Add(Convert.ToInt32(o_item.Value));
            }

            DataTable dt_matching = ((DataTable)ViewState["TableMatching"]).Copy();

            if (lst_strategy_id.Count > 0)
            {
                IEnumerable<DataRow> dr_matching = from r_matching in dt_matching.AsEnumerable() where lst_strategy_id.Contains(r_matching.Field<int>("StrategyID")) select r_matching;

                if (dr_matching == null || dr_matching.Count<DataRow>() == 0)
                {
                    dt_matching.Rows.Clear();
                }
                else
                {
                    dt_matching = dr_matching.CopyToDataTable();
                }
            }

            cblMatching.DataSource = dt_matching;
            cblMatching.DataBind();

            divMatching.Visible = (dt_matching.Rows.Count > 0);
        }

        protected void rblSelectBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            repCompany.DataSource = null;
            repCompany.DataBind();

            DataTable dt_company = (DataTable)ViewState["TableCompany"];

            repCompany.DataSource = dt_company;
            repCompany.DataBind();
        }

        protected void chkCompany_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkCompany = (CheckBox)sender;

            RepeaterItem repItemCompany = (RepeaterItem)chkCompany.NamingContainer;

            Repeater repNetwork = (Repeater)repItemCompany.FindControl("repNetwork");
            Repeater repSupplierGroup = (Repeater)repItemCompany.FindControl("repSupplierGroup");

            repNetwork.DataSource = null;
            repNetwork.DataBind();

            repSupplierGroup.DataSource = null;
            repSupplierGroup.DataBind();

            if (chkCompany.Checked == false) { return; }

            HiddenField hidCompanyID = (HiddenField)repItemCompany.FindControl("hidCompanyID");

            int n_company_id = Convert.ToInt32(hidCompanyID.Value);

            string s_select_by = rblSelectBy.SelectedValue;

            switch (rblSelectBy.SelectedValue)
            {
                case "cashbox":
                    repNetwork.Visible = true;
                    repSupplierGroup.Visible = false;

                    DataTable dt_network = (DataTable)ViewState["TableNetwork"];
                    DataRow[] dr_network = dt_network.Select(" CompanyID = " + n_company_id);

                    if (dr_network.Length > 0)
                    {
                        repNetwork.DataSource = dr_network.CopyToDataTable();
                        repNetwork.DataBind();
                    }

                    break;
                case "terminal":
                    repNetwork.Visible = false;
                    repSupplierGroup.Visible = true;

                    DataTable dt_supplier_group = (DataTable)ViewState["TableSupplierGroup"];
                    DataRow[] dr_supplier_group = dt_supplier_group.Select(" CompanyID = " + n_company_id);

                    if (dr_supplier_group.Length > 0)
                    {
                        repSupplierGroup.DataSource = dr_supplier_group.CopyToDataTable();
                        repSupplierGroup.DataBind();
                    }

                    break;
            }
        }

        protected void chkNetwork_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkNetwork = (CheckBox)sender;

            RepeaterItem repNetworkItem = (RepeaterItem)chkNetwork.NamingContainer;

            Repeater repBranch = (Repeater)repNetworkItem.FindControl("repBranch");

            repBranch.DataSource = null;
            repBranch.DataBind();

            if (chkNetwork.Checked == false) { return; }

            HiddenField hidNetworkID = (HiddenField)repNetworkItem.FindControl("hidNetworkID");

            int n_network_id = Convert.ToInt32(hidNetworkID.Value);

            DataTable dt_branch = (DataTable)ViewState["TableBranch"];

            DataRow[] dr_branch = dt_branch.Select(" NetworkID = " + n_network_id);

            if (dr_branch.Length > 0)
            {
                repBranch.DataSource = dr_branch.CopyToDataTable();
                repBranch.DataBind();
            }
        }

        protected void chkBranch_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkBranch = (CheckBox)sender;

            RepeaterItem repBranchItem = (RepeaterItem)chkBranch.NamingContainer;

            Repeater repCashBox = (Repeater)repBranchItem.FindControl("repCashBox");

            repCashBox.DataSource = null;
            repCashBox.DataBind();

            if (chkBranch.Checked == false) { return; }

            HiddenField hidBranchID = (HiddenField)repBranchItem.FindControl("hidBranchID");

            int n_branch_id = Convert.ToInt32(hidBranchID.Value);

            DataTable dt_cashbox = (DataTable)ViewState["TableCashBox"];

            DataRow[] dr_cashbox = dt_cashbox.Select(" BranchID = " + n_branch_id);

            if (dr_cashbox.Length > 0)
            {
                repCashBox.DataSource = dr_cashbox.CopyToDataTable();
                repCashBox.DataBind();
            }
        }

        protected void chkSupplierGroup_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkSupplierGroup = (CheckBox)sender;

            RepeaterItem repSupplierGroupItem = (RepeaterItem)chkSupplierGroup.NamingContainer;

            Repeater repSupplier = (Repeater)repSupplierGroupItem.FindControl("repSupplier");

            repSupplier.DataSource = null;
            repSupplier.DataBind();

            if (chkSupplierGroup.Checked == false) { return; }

            HiddenField hidSupplierGroupID = (HiddenField)repSupplierGroupItem.FindControl("hidSupplierGroupID");

            int n_supplier_group_id = Convert.ToInt32(hidSupplierGroupID.Value);

            DataTable dt_supplier = (DataTable)ViewState["TableSupplier"];

            DataRow[] dr_supplier = dt_supplier.Select(" SupplierGroupID = " + n_supplier_group_id);

            if (dr_supplier.Length > 0)
            {
                repSupplier.DataSource = dr_supplier.CopyToDataTable();
                repSupplier.DataBind();
            }
        }

        protected void chkSupplier_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkSupplier = (CheckBox)sender;

            RepeaterItem repSupplierItem = (RepeaterItem)chkSupplier.NamingContainer;

            Repeater repTerminal = (Repeater)repSupplierItem.FindControl("repTerminal");

            repTerminal.DataSource = null;
            repTerminal.DataBind();

            if (chkSupplier.Checked == false) { return; }

            HiddenField hidSupplierID = (HiddenField)repSupplierItem.FindControl("hidSupplierID");

            int n_supplier_id = Convert.ToInt32(hidSupplierID.Value);

            DataTable dt_terminal = (DataTable)ViewState["TableTerminal"];

            DataRow[] dr_terminal = dt_terminal.Select(" SupplierID = " + n_supplier_id);

            if (dr_terminal.Length > 0)
            {
                repTerminal.DataSource = dr_terminal.CopyToDataTable();
                repTerminal.DataBind();
            }
        }

        protected void hidUniqueID_ValueChanged(object sender, EventArgs e)
        {
            if (hidUniqueID.Value == "") { hidTable.Value = ""; }
        }

        protected void Search_Click(object sender, EventArgs e)
        {
            bool b_error = false;

            string s_order_inside = "", s_order_outside = "";
            string s_where = "", s_operator = "", s_error = "";

            // Sort Inside

            if (ddlInsideSort_1.SelectedValue != "")
            {
                s_order_inside = String.Format(" {0} {1} ", ddlInsideSort_1.SelectedValue, ddlInsideOrder_1.SelectedValue);
            }

            if (ddlInsideSort_2.SelectedValue != "")
            {
                if (s_order_inside != "") { s_order_inside += ","; }

                s_order_inside += String.Format(" {0} {1} ", ddlInsideSort_2.SelectedValue, ddlInsideOrder_2.SelectedValue);
            }

            if (ddlInsideSort_3.SelectedValue != "")
            {
                if (s_order_inside != "") { s_order_inside += ","; }

                s_order_inside += String.Format(" {0} {1} ", ddlInsideSort_3.SelectedValue, ddlInsideOrder_3.SelectedValue);
            }

            // Sort Outside

            if (ddlOutsideSort_1.SelectedValue != "")
            {
                s_order_outside = String.Format(" {0} {1} ", ddlOutsideSort_1.SelectedValue, ddlOutsideOrder_1.SelectedValue);
            }

            if (ddlOutsideSort_2.SelectedValue != "")
            {
                if (s_order_outside != "") { s_order_outside += ","; }

                s_order_outside += String.Format(" {0} {1} ", ddlOutsideSort_2.SelectedValue, ddlOutsideOrder_2.SelectedValue);
            }

            if (ddlOutsideSort_3.SelectedValue != "")
            {
                if (s_order_outside != "") { s_order_outside += ","; }

                s_order_outside += String.Format(" {0} {1} ", ddlOutsideSort_3.SelectedValue, ddlOutsideOrder_3.SelectedValue);
            }

            // Transactions

            switch (ddlTransactions.SelectedValue)
            {
                case "matching":
                    //s_where = " AND QueryID IS NOT NULL ";
                    s_where = " AND QueryID IS NOT NULL AND MATCHINGID IS NOT NULL AND STRATEGYID IS NOT NULL ";
                    // MatchingAction

                    if (divMatchingAction.Visible == true)
                    {
                        string s_matching_action = Parameter_CheckBoxList(cblMatchingAction);

                        if (s_matching_action != "") { s_where += " AND MatchingActionID IN ( " + s_matching_action + " ) "; }
                    }

                    // MatchingType

                    if (divMatchingType.Visible == true)
                    {
                        string s_matching_type = Parameter_CheckBoxList(cblMatchingType);

                        if (s_matching_type != "") { s_where += " AND MatchingTypeID IN ( " + s_matching_type + " ) "; }
                    }
                    divMatchingBalanceRow.Visible = false;
                    
                    break;
                case "not-matching":
                    s_where = " AND QueryID IS NULL ";
                    //s_where = " AND QueryID IS NULL AND MATCHINGID IS NULL AND STRATEGYID IS NULL ";

                    // Status

                    if (divStatus.Visible == true)
                    {
                        string s_status = Parameter_CheckBoxList(cblStatus);

                        if (s_status != "") { s_where += " AND StatusID IN ( " + s_status + " ) "; }
                    }

                    break;
                //case "all":
                //    pnlMatchingBalance.Visible = false;
                //    break;
            }

            if(ddlTransactions.SelectedValue=="all")
                pnlMatchingBalance.Visible = false;
            else
                pnlMatchingBalance.Visible = true;
            // OperationType
            if(ddlTransactions.SelectedValue == "matching")
                btnMatchingBalanceChange.Enabled = false;
            else
                btnMatchingBalanceChange.Enabled = true;

            string s_operation_type = Parameter_CheckBoxList(cblOperationType);

            if (s_operation_type != "") { s_where += " AND OperationTypeID IN ( " + s_operation_type + " ) "; }

            // Template

            string s_template = Parameter_CheckBoxList(cblTemplate);

            if (s_template != "") { s_where += " AND TemplateID IN ( " + s_template + " ) "; }

            // DataFile

            string s_data_file = Parameter_CheckBoxList(cblDataFile);

            if (s_data_file != "") { s_where += " AND DataFileID IN ( " + s_data_file + " ) "; }

            // Strategy

            string s_strategy = Parameter_CheckBoxList(cblStrategy);

            if (s_strategy != "") { s_where += " AND StrategyID IN ( " + s_strategy + " ) "; }

            // Matching

            string s_matching = Parameter_CheckBoxList(cblMatching);

            if (s_matching != "") { s_where += " AND MatchingID IN ( " + s_matching + " ) "; }

            // Company

            string s_company = "", s_network = "", s_branch = "", s_cashbox = "", s_supplier_groupI = "", s_supplier = "", s_terminal = "";

            foreach (RepeaterItem rep_item_company in repCompany.Items)
            {
                if (rep_item_company.ItemType == ListItemType.Item || rep_item_company.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox chkCompany = (CheckBox)rep_item_company.FindControl("chkCompany");

                    if (chkCompany.Checked == true)
                    {
                        HiddenField hidCompanyID = (HiddenField)rep_item_company.FindControl("hidCompanyID");

                        if (s_company != "") { s_company += ","; }

                        s_company += hidCompanyID.Value;

                        if (rblSelectBy.SelectedValue == "cashbox")
                        {
                            Repeater repNetwork = (Repeater)rep_item_company.FindControl("repNetwork");

                            foreach (RepeaterItem rep_item_network in repNetwork.Items)
                            {
                                if (rep_item_network.ItemType == ListItemType.Item || rep_item_network.ItemType == ListItemType.AlternatingItem)
                                {
                                    CheckBox chkNetwork = (CheckBox)rep_item_network.FindControl("chkNetwork");

                                    if (chkNetwork.Checked == true)
                                    {
                                        HiddenField hidNetworkID = (HiddenField)rep_item_network.FindControl("hidNetworkID");

                                        if (s_network != "") { s_network += ","; }

                                        s_network += hidNetworkID.Value;

                                        Repeater repBranch = (Repeater)rep_item_network.FindControl("repBranch");

                                        foreach (RepeaterItem rep_item_branch in repBranch.Items)
                                        {
                                            if (rep_item_branch.ItemType == ListItemType.Item || rep_item_branch.ItemType == ListItemType.AlternatingItem)
                                            {
                                                CheckBox chkBranch = (CheckBox)rep_item_branch.FindControl("chkBranch");

                                                if (chkBranch.Checked == true)
                                                {
                                                    HiddenField hidBranchID = (HiddenField)rep_item_branch.FindControl("hidBranchID");

                                                    if (s_branch != "") { s_branch += ","; }

                                                    s_branch += hidBranchID.Value;

                                                    Repeater repCashBox = (Repeater)rep_item_branch.FindControl("repCashBox");

                                                    foreach (RepeaterItem rep_item_cashbox in repCashBox.Items)
                                                    {
                                                        if (rep_item_cashbox.ItemType == ListItemType.Item || rep_item_cashbox.ItemType == ListItemType.AlternatingItem)
                                                        {
                                                            CheckBox chkCashBox = (CheckBox)rep_item_cashbox.FindControl("chkCashBox");

                                                            if (chkCashBox.Checked == true)
                                                            {
                                                                HiddenField hidCashBoxID = (HiddenField)rep_item_cashbox.FindControl("hidCashBoxID");

                                                                if (s_cashbox != "") { s_cashbox += ","; }

                                                                s_cashbox += hidCashBoxID.Value;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Repeater repSupplierGroup = (Repeater)rep_item_company.FindControl("repSupplierGroup");

                            foreach (RepeaterItem rep_item_supplier_group in repSupplierGroup.Items)
                            {
                                if (rep_item_supplier_group.ItemType == ListItemType.Item || rep_item_supplier_group.ItemType == ListItemType.AlternatingItem)
                                {
                                    CheckBox chkSupplierGroup = (CheckBox)rep_item_supplier_group.FindControl("chkSupplierGroup");

                                    if (chkSupplierGroup.Checked == true)
                                    {
                                        HiddenField hidSupplierGroupID = (HiddenField)rep_item_supplier_group.FindControl("hidSupplierGroupID");

                                        if (s_supplier_groupI != "") { s_supplier_groupI += ","; }

                                        s_supplier_groupI += hidSupplierGroupID.Value;

                                        Repeater repSupplier = (Repeater)rep_item_supplier_group.FindControl("repSupplier");

                                        foreach (RepeaterItem rep_item_supplier in repSupplier.Items)
                                        {
                                            if (rep_item_supplier.ItemType == ListItemType.Item || rep_item_supplier.ItemType == ListItemType.AlternatingItem)
                                            {
                                                CheckBox chkSupplier = (CheckBox)rep_item_supplier.FindControl("chkSupplier");

                                                if (chkSupplier.Checked == true)
                                                {
                                                    HiddenField hidSupplierID = (HiddenField)rep_item_supplier.FindControl("hidSupplierID");

                                                    if (s_supplier != "") { s_supplier += ","; }

                                                    s_supplier += hidSupplierID.Value;

                                                    Repeater repTerminal = (Repeater)rep_item_supplier.FindControl("repTerminal");

                                                    foreach (RepeaterItem rep_item_terminal in repTerminal.Items)
                                                    {
                                                        if (rep_item_terminal.ItemType == ListItemType.Item || rep_item_terminal.ItemType == ListItemType.AlternatingItem)
                                                        {
                                                            CheckBox chkTerminal = (CheckBox)rep_item_terminal.FindControl("chkTerminal");

                                                            if (chkTerminal.Checked == true)
                                                            {
                                                                HiddenField hidTerminalID = (HiddenField)rep_item_terminal.FindControl("hidTerminalID");

                                                                if (s_terminal != "") { s_terminal += ","; }

                                                                s_terminal += hidTerminalID.Value;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (s_company != "") { s_where += " AND CompanyID IN ( " + s_company + " ) "; }

            if (s_network != "") { s_where += " AND NetworkID IN ( " + s_network + " ) "; }

            if (s_branch != "") { s_where += " AND BranchID IN ( " + s_branch + " ) "; }

            if (s_cashbox != "") { s_where += " AND CashBoxID IN ( " + s_cashbox + " ) "; }

            if (s_supplier_groupI != "") { s_where += " AND SupplierGroupID IN ( " + s_supplier_groupI + " ) "; }

            if (s_supplier != "") { s_where += " AND SupplierID IN ( " + s_supplier + " ) "; }

            if (s_terminal != "") { s_where += " AND TerminalID IN ( " + s_terminal + " ) "; }

            // CreditID

            string s_credit = Parameter_CheckBoxList(cblCredit);

            if (s_credit != "") { s_where += " AND CreditID IN ( " + s_credit + " ) "; }

            // CardID

            string s_card = Parameter_CheckBoxList(cblCard);

            if (s_card != "") { s_where += " AND CardID IN ( " + s_card + " ) "; }

            // TransactionCurrencyID

            string s_transaction_currency = Parameter_CheckBoxList(cblTransactionCurrency);

            if (s_transaction_currency != "") { s_where += " AND TransactionCurrencyID IN ( " + s_transaction_currency + " ) "; }

            // === !!!

            string s_where_inside = s_where;
            string s_where_outside = s_where;

            // TransactionType

            if (chkIsSplitted.Checked == true)
            {
                s_where_inside += " AND IsSplitted = 1 ";
            }

            if (chkIsBalance.Checked == true)
            {
                s_where_inside += " AND IsBalance = 1 ";
            }

            if (chkIsAbroad.Checked == true)
            {
                s_where_outside += " AND IsAbroad = 1 ";
            }

            // TransactionDate

            s_where = "";

            string s_transaction_date = Parameter_TextBox(txtTransactionDate, "date", ref s_error, true);

            if (s_error != "")
            {
                b_error = true;

                lblTransactionDateError.Text = s_error;
            }
            else if (s_transaction_date != "")
            {
                string s_sql = Parameter_SQL("TransactionDate", s_transaction_date, chkExcludeTransactionDate.Checked);

                if (chkEmptyTransactionDate.Checked == true)
                {
                    if (s_sql != "") { s_sql += " OR "; }

                    s_operator = (chkExcludeTransactionDate.Checked == true) ? " NOT NULL " : " NULL ";

                    s_sql += String.Format(" TransactionDate IS {0} ", s_operator);
                }

                if (s_sql != "")
                {
                    s_where = String.Format(" AND ( {0} ) ", s_sql);

                    s_where_inside += s_where;
                }
            }

            if (chk_txtTransactionDateOutside.Checked == true)
            {
                s_where_outside += s_where;
            }
            else
            {
                string s_transaction_date_outside = Parameter_TextBox(txtTransactionDateOutside, "date", ref s_error, true);

                if (s_error != "")
                {
                    b_error = true;

                    lblTransactionDateError.Text = s_error;
                }
                else if (s_transaction_date_outside != "")
                {
                    string s_sql = Parameter_SQL("TransactionDate", s_transaction_date_outside, chkExcludeTransactionDate.Checked);

                    if (chkEmptyTransactionDate.Checked == true)
                    {
                        if (s_sql != "") { s_sql += " OR "; }

                        s_operator = (chkExcludeTransactionDate.Checked == true) ? " NOT NULL " : " NULL ";

                        s_sql += String.Format(" TransactionDate IS {0} ", s_operator);
                    }

                    if (s_sql != "")
                    {
                        s_where_outside += String.Format(" AND ( {0} ) ", s_sql);
                    }
                }
            }

            // TransmissionDate

            s_where = "";

            string s_transmission_date = Parameter_TextBox(txtTransmissionDate, "date", ref s_error, true);

            if (s_error != "")
            {
                b_error = true;

                lblTransmissionDateError.Text = s_error;
            }
            else if (s_transmission_date != "")
            {
                string s_sql = Parameter_SQL("TransmissionDate", s_transmission_date, chkExcludeTransmissionDate.Checked);

                if (chkEmptyTransmissionDate.Checked == true)
                {
                    if (s_sql != "") { s_sql += " OR "; }

                    s_operator = (chkExcludeTransmissionDate.Checked == true) ? " NOT NULL " : " NULL ";

                    s_sql += String.Format(" TransmissionDate IS {0} ", s_operator);
                }

                if (s_sql != "")
                {
                    s_where = String.Format(" AND ( {0} ) ", s_sql);

                    s_where_inside += s_where;
                }
            }

            if (chk_txtTransmissionDateOutside.Checked == true)
            {
                s_where_outside += s_where;
            }
            else
            {
                string s_transmission_date_outside = Parameter_TextBox(txtTransmissionDateOutside, "date", ref s_error, true);

                if (s_error != "")
                {
                    b_error = true;

                    lblTransmissionDateError.Text = s_error;
                }
                else if (s_transmission_date_outside != "")
                {
                    string s_sql = Parameter_SQL("TransmissionDate", s_transmission_date_outside, chkExcludeTransmissionDate.Checked);

                    if (chkEmptyTransmissionDate.Checked == true)
                    {
                        if (s_sql != "") { s_sql += " OR "; }

                        s_operator = (chkExcludeTransmissionDate.Checked == true) ? " NOT NULL " : " NULL ";

                        s_sql += String.Format(" TransmissionDate IS {0} ", s_operator);
                    }

                    if (s_sql != "")
                    {
                        s_where_outside += String.Format(" AND ( {0} ) ", s_sql);
                    }
                }
            }

            // PaymentDate

            s_where = "";

            string s_payment_date = Parameter_TextBox(txtPaymentDate, "date", ref s_error, true);

            if (s_error != "")
            {
                b_error = true;

                lblPaymentDateError.Text = s_error;
            }
            else if (s_payment_date != "")
            {
                string s_sql = Parameter_SQL("PaymentDate", s_payment_date, chkExcludePaymentDate.Checked);

                if (chkEmptyPaymentDate.Checked == true)
                {
                    if (s_sql != "") { s_sql += " OR "; }

                    s_operator = (chkExcludePaymentDate.Checked == true) ? " NOT NULL " : " NULL ";

                    s_sql += String.Format(" PaymentDate IS {0} ", s_operator);
                }

                if (s_sql != "")
                {
                    s_where = String.Format(" AND ( {0} ) ", s_sql);

                    s_where_inside += s_where;
                }
            }

            if (chk_txtPaymentDateOutside.Checked == true)
            {
                s_where_outside += s_where;
            }
            else
            {
                string s_payment_date_outside = Parameter_TextBox(txtPaymentDateOutside, "date", ref s_error, true);

                if (s_error != "")
                {
                    b_error = true;

                    lblPaymentDateError.Text = s_error;
                }
                else if (s_payment_date_outside != "")
                {
                    string s_sql = Parameter_SQL("PaymentDate", s_payment_date_outside, chkExcludePaymentDate.Checked);

                    if (chkEmptyPaymentDate.Checked == true)
                    {
                        if (s_sql != "") { s_sql += " OR "; }

                        s_operator = (chkExcludePaymentDate.Checked == true) ? " NOT NULL " : " NULL ";

                        s_sql += String.Format(" PaymentDate IS {0} ", s_operator);
                    }

                    if (s_sql != "")
                    {
                        s_where_outside += String.Format(" AND ( {0} ) ", s_sql);
                    }
                }
            }

            // MatchingDate

            s_where = "";

            string s_matching_date = Parameter_TextBox(txtMatchingDate, "date", ref s_error, true);

            if (s_error != "")
            {
                b_error = true;

                lblMatchingDateError.Text = s_error;
            }
            else if (s_matching_date != "")
            {
                string s_sql = Parameter_SQL("CAST(MatchingDate AS date)", s_matching_date, chkExcludeMatchingDate.Checked);

                if (s_sql != "")
                {
                    s_where = String.Format(" AND ( {0} ) ", s_sql);

                    s_where_inside += s_where;
                }
            }

            if (chk_txtMatchingDateOutside.Checked == true)
            {
                s_where_outside += s_where;
            }
            else
            {
                string s_matching_date_outside = Parameter_TextBox(txtMatchingDateOutside, "date", ref s_error, true);

                if (s_error != "")
                {
                    b_error = true;

                    lblMatchingDateError.Text = s_error;
                }
                else if (s_matching_date_outside != "")
                {
                    string s_sql = Parameter_SQL("CAST(MatchingDate AS date)", s_matching_date_outside, chkExcludeMatchingDate.Checked);

                    if (s_sql != "")
                    {
                        s_where_outside += String.Format(" AND ( {0} ) ", s_sql);
                    }
                }
            }

            // CardPrefix

            s_where = "";

            string s_card_prefix = Parameter_TextBox(txtCardPrefix, "uint", ref s_error, false, 6);

            if (s_error != "")
            {
                b_error = true;

                lblCardPrefixError.Text = s_error;
            }
            else if (s_card_prefix != "")
            {
                s_operator = (chkExcludeCardPrefix.Checked == true) ? " NOT IN " : " IN ";

                s_where = String.Format(" AND CardPrefix {0} ( {1} ) ", s_operator, s_card_prefix);

                s_where_inside += s_where;
            }

            if (chk_txtCardPrefixOutside.Checked == true)
            {
                s_where_outside += s_where;
            }
            else
            {
                string s_card_prefix_outside = Parameter_TextBox(txtCardPrefixOutside, "uint", ref s_error, false, 6);

                if (s_error != "")
                {
                    b_error = true;

                    lblCardPrefixError.Text = s_error;
                }
                else if (s_card_prefix_outside != "")
                {
                    s_operator = (chkExcludeCardPrefix.Checked == true) ? " NOT IN " : " IN ";

                    s_where_outside += String.Format(" AND CardPrefix {0} ( {1} ) ", s_operator, s_card_prefix_outside);
                }
            }

            // CardNumber

            s_where = "";

            string s_card_number = Parameter_TextBox(txtCardNumber, "uint", ref s_error, false, 4);

            if (s_error != "")
            {
                b_error = true;

                lblCardNumberError.Text = s_error;
            }
            else if (s_card_number != "")
            {
                s_operator = (chkExcludeCardNumber.Checked == true) ? " NOT IN " : " IN ";

                s_where = String.Format(" AND CardNumber {0} ( {1} ) ", s_operator, s_card_number);

                s_where_inside += s_where;
            }

            if (chk_txtCardNumberOutside.Checked == true)
            {
                s_where_outside += s_where;
            }
            else
            {
                string s_card_number_outside = Parameter_TextBox(txtCardNumberOutside, "uint", ref s_error, false, 4);

                if (s_error != "")
                {
                    b_error = true;

                    lblCardNumberError.Text = s_error;
                }
                else if (s_card_number_outside != "")
                {
                    s_operator = (chkExcludeCardNumber.Checked == true) ? " NOT IN " : " IN ";

                    s_where_outside += String.Format(" AND CardNumber {0} ( {1} ) ", s_operator, s_card_number_outside);
                }
            }

            // TransmissionNumber

            s_where = "";

            string s_transmission_number = Parameter_TextBox(txtTransmissionNumber, "ulong", ref s_error);

            if (s_error != "")
            {
                b_error = true;

                lblTransmissionNumberError.Text = s_error;
            }
            else if (s_transmission_number != "")
            {
                s_operator = (chkExcludeTransmissionNumber.Checked == true) ? " NOT IN " : " IN ";

                s_where = String.Format(" AND TransmissionNumber {0} ( {1} ) ", s_operator, s_transmission_number);

                s_where_inside += s_where;
            }

            if (chk_txtTransmissionNumberOutside.Checked == true)
            {
                s_where_outside += s_where;
            }
            else
            {
                string s_transmission_number_outside = Parameter_TextBox(txtTransmissionNumberOutside, "ulong", ref s_error);

                if (s_error != "")
                {
                    b_error = true;

                    lblTransmissionNumberError.Text = s_error;
                }
                else if (s_transmission_number_outside != "")
                {
                    s_operator = (chkExcludeTransmissionNumber.Checked == true) ? " NOT IN " : " IN ";

                    s_where_outside += String.Format(" AND TransmissionNumber {0} ( {1} ) ", s_operator, s_transmission_number_outside);
                }
            }

            // VoucherNumber

            s_where = "";

            string s_voucher_number = Parameter_TextBox(txtVoucherNumber, "string", ref s_error);

            if (s_error != "")
            {
                b_error = true;

                lblVoucherNumberError.Text = s_error;
            }
            else if (s_voucher_number != "")
            {
                s_operator = (chkExcludeVoucherNumber.Checked == true) ? " NOT IN " : " IN ";

                s_where = String.Format(" AND VoucherNumber {0} ( {1} ) ", s_operator, s_voucher_number);

                s_where_inside += s_where;
            }

            if (chk_txtVoucherNumberOutside.Checked == true)
            {
                s_where_outside += s_where;
            }
            else
            {
                string s_voucher_number_outside = Parameter_TextBox(txtVoucherNumberOutside, "string", ref s_error);

                if (s_error != "")
                {
                    b_error = true;

                    lblVoucherNumberError.Text = s_error;
                }
                else if (s_voucher_number_outside != "")
                {
                    s_operator = (chkExcludeVoucherNumber.Checked == true) ? " NOT IN " : " IN ";

                    s_where_outside += String.Format(" AND VoucherNumber {0} ( {1} ) ", s_operator, s_voucher_number_outside);
                }
            }

            // ConfirmationNumber

            s_where = "";

            string s_confirmation_number = Parameter_TextBox(txtConfirmationNumber, "string", ref s_error);

            if (s_error != "")
            {
                b_error = true;

                lblConfirmationNumberError.Text = s_error;
            }
            else if (s_confirmation_number != "")
            {
                s_operator = (chkExcludeConfirmationNumber.Checked == true) ? " NOT IN " : " IN ";

                s_where = String.Format(" AND ConfirmationNumber {0} ( {1} ) ", s_operator, s_confirmation_number);

                s_where_inside += s_where;
            }

            if (chk_txtConfirmationNumberOutside.Checked == true)
            {
                s_where_outside += s_where;
            }
            else
            {
                string s_confirmation_number_outside = Parameter_TextBox(txtConfirmationNumberOutside, "string", ref s_error);

                if (s_error != "")
                {
                    b_error = true;

                    lblConfirmationNumberError.Text = s_error;
                }
                else if (s_confirmation_number_outside != "")
                {
                    s_operator = (chkExcludeConfirmationNumber.Checked == true) ? " NOT IN " : " IN ";

                    s_where_outside += String.Format(" AND ConfirmationNumber {0} ( {1} ) ", s_operator, s_confirmation_number_outside);
                }
            }

            // PaymentsCount

            s_where = "";

            string s_payments_count = Parameter_TextBox(txtPaymentsCount, "uint", ref s_error, true);

            if (s_error != "")
            {
                b_error = true;

                lblPaymentsCountError.Text = s_error;
            }
            else if (s_payments_count != "")
            {
                string s_sql = Parameter_SQL("PaymentsCount", s_payments_count, chkExcludePaymentsCount.Checked);

                s_where = " AND ( " + s_sql + " ) ";

                s_where_inside += s_where;
            }

            if (chk_txtPaymentsCountOutside.Checked == true)
            {
                s_where_outside += s_where;
            }
            else
            {
                string s_payments_count_outside = Parameter_TextBox(txtPaymentsCountOutside, "uint", ref s_error, true);

                if (s_error != "")
                {
                    b_error = true;

                    lblPaymentsCountError.Text = s_error;
                }
                else if (s_payments_count_outside != "")
                {
                    string s_sql = Parameter_SQL("PaymentsCount", s_payments_count_outside, chkExcludePaymentsCount.Checked);

                    s_where_outside += " AND ( " + s_sql + " ) ";
                }
            }

            // DutyPaymentNumber

            s_where = "";

            string s_duty_payment_number = Parameter_TextBox(txtDutyPaymentNumber, "uint", ref s_error, true);

            if (s_error != "")
            {
                b_error = true;

                lblDutyPaymentNumberError.Text = s_error;
            }
            else if (s_duty_payment_number != "")
            {
                string s_sql = Parameter_SQL("DutyPaymentNumber", s_duty_payment_number, chkExcludeDutyPaymentNumber.Checked);

                s_where = " AND ( " + s_sql + " ) ";

                s_where_inside += s_where;
            }

            if (chk_txtDutyPaymentNumberOutside.Checked == true)
            {
                s_where_outside += s_where;
            }
            else
            {
                string s_duty_payment_number_outside = Parameter_TextBox(txtDutyPaymentNumberOutside, "uint", ref s_error, true);

                if (s_error != "")
                {
                    b_error = true;

                    lblDutyPaymentNumberError.Text = s_error;
                }
                else if (s_duty_payment_number_outside != "")
                {
                    string s_sql = Parameter_SQL("DutyPaymentNumber", s_duty_payment_number_outside, chkExcludeDutyPaymentNumber.Checked);

                    s_where_outside += " AND ( " + s_sql + " ) ";
                }
            }

            // TransactionGrossAmount

            s_where = "";

            string s_transaction_gross_amount = Parameter_TextBox(txtTransactionGrossAmount, "double", ref s_error, true);

            if (s_error != "")
            {
                b_error = true;

                lblTransactionGrossAmountError.Text = s_error;
            }
            else if (s_transaction_gross_amount != "")
            {
                string s_sql = Parameter_SQL("TransactionGrossAmount", s_transaction_gross_amount, chkExcludeTransactionGrossAmount.Checked);

                s_where = " AND ( " + s_sql + " ) ";

                s_where_inside += s_where;
            }

            if (chk_txtTransactionGrossAmountOutside.Checked == true)
            {
                s_where_outside += s_where;
            }
            else
            {
                string s_transaction_gross_amount_outside = Parameter_TextBox(txtTransactionGrossAmountOutside, "double", ref s_error, true);

                if (s_error != "")
                {
                    b_error = true;

                    lblTransactionGrossAmountError.Text = s_error;
                }
                else if (s_transaction_gross_amount_outside != "")
                {
                    string s_sql = Parameter_SQL("TransactionGrossAmount", s_transaction_gross_amount_outside, chkExcludeTransactionGrossAmount.Checked);

                    s_where_outside += " AND ( " + s_sql + " ) ";
                }
            }

            // DutyPaymentAmount

            s_where = "";

            string s_duty_payment_amount = Parameter_TextBox(txtDutyPaymentAmount, "double", ref s_error, true);

            if (s_error != "")
            {
                b_error = true;

                lblDutyPaymentAmountError.Text = s_error;
            }
            else if (s_duty_payment_amount != "")
            {
                string s_sql = Parameter_SQL("DutyPaymentAmount", s_duty_payment_amount, chkExcludeDutyPaymentAmount.Checked);

                s_where = " AND ( " + s_sql + " ) ";

                s_where_inside += s_where;
            }

            if (chk_txtDutyPaymentAmountOutside.Checked == true)
            {
                s_where_outside += s_where;
            }
            else
            {
                string s_duty_payment_amount_outside = Parameter_TextBox(txtDutyPaymentAmountOutside, "double", ref s_error, true);

                if (s_error != "")
                {
                    b_error = true;

                    lblDutyPaymentAmountError.Text = s_error;
                }
                else if (s_duty_payment_amount_outside != "")
                {
                    string s_sql = Parameter_SQL("DutyPaymentAmount", s_duty_payment_amount_outside, chkExcludeDutyPaymentAmount.Checked);

                    s_where_outside += " AND ( " + s_sql + " ) ";
                }
            }

            // RemainingPaymentsAmount

            s_where = "";

            string s_remaining_payments_amount = Parameter_TextBox(txtRemainingPaymentsAmount, "double", ref s_error, true);

            if (s_error != "")
            {
                b_error = true;

                lblRemainingPaymentsAmountError.Text = s_error;
            }
            else if (s_remaining_payments_amount != "")
            {
                string s_sql = Parameter_SQL("RemainingPaymentsAmount", s_remaining_payments_amount, chkExcludeRemainingPaymentsAmount.Checked);

                s_where = " AND ( " + s_sql + " ) ";

                s_where_inside += s_where;
            }

            if (chk_txtRemainingPaymentsAmountOutside.Checked == true)
            {
                s_where_outside += s_where;
            }
            else
            {
                string s_remaining_payments_amount_outside = Parameter_TextBox(txtRemainingPaymentsAmountOutside, "double", ref s_error, true);

                if (s_error != "")
                {
                    b_error = true;

                    lblRemainingPaymentsAmountError.Text = s_error;
                }
                else if (s_remaining_payments_amount_outside != "")
                {
                    string s_sql = Parameter_SQL("RemainingPaymentsAmount", s_remaining_payments_amount_outside, chkExcludeRemainingPaymentsAmount.Checked);

                    s_where_outside += " AND ( " + s_sql + " ) ";
                }
            }

            // CompanyNumber

            s_where = "";

            string s_company_number = Parameter_TextBox(txtCompanyNumber, "ulong", ref s_error, true);

            if (s_error != "")
            {
                b_error = true;

                lblCompanyNumberError.Text = s_error;
            }
            else if (s_company_number != "")
            {
                string s_sql = Parameter_SQL("CompanyNumber", s_company_number, chkExcludeCompanyNumber.Checked);

                s_where = " AND ( " + s_sql + " ) ";

                s_where_inside += s_where;
            }

            if (chk_txtCompanyNumberOutside.Checked == true)
            {
                s_where_outside += s_where;
            }
            else
            {
                string s_company_number_outside = Parameter_TextBox(txtCompanyNumberOutside, "ulong", ref s_error, true);

                if (s_error != "")
                {
                    b_error = true;

                    lblCompanyNumberError.Text = s_error;
                }
                else if (s_company_number_outside != "")
                {
                    string s_sql = Parameter_SQL("CompanyNumber", s_company_number_outside, chkExcludeCompanyNumber.Checked);

                    s_where_outside += " AND ( " + s_sql + " ) ";
                }
            }

            // NetworkNumber

            s_where = "";

            string s_network_number = Parameter_TextBox(txtNetworkNumber, "string", ref s_error);

            if (s_error != "")
            {
                b_error = true;

                lblNetworkNumberError.Text = s_error;
            }
            else if (s_network_number != "")
            {
                string s_sql = Parameter_SQL("NetworkNumber", s_network_number, chkExcludeNetworkNumber.Checked);

                s_where = " AND ( " + s_sql + " ) ";

                s_where_inside += s_where;
            }

            if (chk_txtNetworkNumberOutside.Checked == true)
            {
                s_where_outside += s_where;
            }
            else
            {
                string s_network_number_outside = Parameter_TextBox(txtNetworkNumberOutside, "string", ref s_error);

                if (s_error != "")
                {
                    b_error = true;

                    lblNetworkNumberError.Text = s_error;
                }
                else if (s_network_number_outside != "")
                {
                    string s_sql = Parameter_SQL("NetworkNumber", s_network_number_outside, chkExcludeNetworkNumber.Checked);

                    s_where_outside += " AND ( " + s_sql + " ) ";
                }
            }

            // BranchNumber

            s_where = "";

            string s_branch_number = Parameter_TextBox(txtBranchNumber, "string", ref s_error);

            if (s_error != "")
            {
                b_error = true;

                lblBranchNumberError.Text = s_error;
            }
            else if (s_branch_number != "")
            {
                string s_sql = Parameter_SQL("BranchNumber", s_branch_number, chkExcludeBranchNumber.Checked);

                s_where = " AND ( " + s_sql + " ) ";

                s_where_inside += s_where;
            }

            if (chk_txtBranchNumberOutside.Checked == true)
            {
                s_where_outside += s_where;
            }
            else
            {
                string s_branch_number_outside = Parameter_TextBox(txtBranchNumberOutside, "string", ref s_error);

                if (s_error != "")
                {
                    b_error = true;

                    lblBranchNumberError.Text = s_error;
                }
                else if (s_branch_number_outside != "")
                {
                    string s_sql = Parameter_SQL("BranchNumber", s_branch_number_outside, chkExcludeBranchNumber.Checked);

                    s_where_outside += " AND ( " + s_sql + " ) ";
                }
            }

            // CashBoxNumber

            s_where = "";

            string s_cashbox_number = Parameter_TextBox(txtCashBoxNumber, "string", ref s_error);

            if (s_error != "")
            {
                b_error = true;

                lblCashBoxNumberError.Text = s_error;
            }
            else if (s_cashbox_number != "")
            {
                string s_sql = Parameter_SQL("CashBoxNumber", s_cashbox_number, chkExcludeCashBoxNumber.Checked);

                s_where = " AND ( " + s_sql + " ) ";

                s_where_inside += s_where;
            }

            if (chk_txtCashBoxNumberOutside.Checked == true)
            {
                s_where_outside += s_where;
            }
            else
            {
                string s_cashbox_number_outside = Parameter_TextBox(txtCashBoxNumberOutside, "string", ref s_error);

                if (s_error != "")
                {
                    b_error = true;

                    lblCashBoxNumberError.Text = s_error;
                }
                else if (s_cashbox_number_outside != "")
                {
                    string s_sql = Parameter_SQL("CashBoxNumber", s_cashbox_number_outside, chkExcludeCashBoxNumber.Checked);

                    s_where_outside += " AND ( " + s_sql + " ) ";
                }
            }

            // SupplierGroupNumber

            s_where = "";

            string s_supplier_group_number = Parameter_TextBox(txtSupplierGroupNumber, "ulong", ref s_error, true);

            if (s_error != "")
            {
                b_error = true;

                lblSupplierGroupNumberError.Text = s_error;
            }
            else if (s_supplier_group_number != "")
            {
                string s_sql = Parameter_SQL("SupplierGroupNumber", s_supplier_group_number, chkExcludeSupplierGroupNumber.Checked);

                s_where = " AND ( " + s_sql + " ) ";

                s_where_inside += s_where;
            }

            if (chk_txtSupplierGroupNumberOutside.Checked == true)
            {
                s_where_outside += s_where;
            }
            else
            {
                string s_supplier_group_number_outside = Parameter_TextBox(txtSupplierGroupNumberOutside, "ulong", ref s_error, true);

                if (s_error != "")
                {
                    b_error = true;

                    lblSupplierGroupNumberError.Text = s_error;
                }
                else if (s_supplier_group_number_outside != "")
                {
                    string s_sql = Parameter_SQL("SupplierGroupNumber", s_supplier_group_number_outside, chkExcludeSupplierGroupNumber.Checked);

                    s_where_outside += " AND ( " + s_sql + " ) ";
                }
            }

            // SupplierNumber

            s_where = "";

            string s_supplier_number = Parameter_TextBox(txtSupplierNumber, "ulong", ref s_error, true);

            if (s_error != "")
            {
                b_error = true;

                lblSupplierNumberError.Text = s_error;
            }
            else if (s_supplier_number != "")
            {
                string s_sql = Parameter_SQL("SupplierNumber", s_supplier_number, chkExcludeSupplierNumber.Checked);

                s_where = " AND ( " + s_sql + " ) ";

                s_where_inside += s_where;
            }

            if (chk_txtSupplierNumberOutside.Checked == true)
            {
                s_where_outside += s_where;
            }
            else
            {
                string s_supplier_number_outside = Parameter_TextBox(txtSupplierNumberOutside, "ulong", ref s_error, true);

                if (s_error != "")
                {
                    b_error = true;

                    lblSupplierNumberError.Text = s_error;
                }
                else if (s_supplier_number_outside != "")
                {
                    string s_sql = Parameter_SQL("SupplierNumber", s_supplier_number_outside, chkExcludeSupplierNumber.Checked);

                    s_where_outside += " AND ( " + s_sql + " ) ";
                }
            }

            // TerminalNumber

            s_where = "";

            string s_terminal_number = Parameter_TextBox(txtTerminalNumber, "ulong", ref s_error, true);

            if (s_error != "")
            {
                b_error = true;

                lblTerminalNumberError.Text = s_error;
            }
            else if (s_terminal_number != "")
            {
                string s_sql = Parameter_SQL("TerminalNumber", s_terminal_number, chkExcludeTerminalNumber.Checked);

                s_where = " AND ( " + s_sql + " ) ";

                s_where_inside += s_where;
            }

            if (chk_txtTerminalNumberOutside.Checked == true)
            {
                s_where_outside += s_where;
            }
            else
            {
                string s_terminal_number_outside = Parameter_TextBox(txtTerminalNumberOutside, "ulong", ref s_error, true);

                if (s_error != "")
                {
                    b_error = true;

                    lblTerminalNumberError.Text = s_error;
                }
                else if (s_terminal_number_outside != "")
                {
                    string s_sql = Parameter_SQL("TerminalNumber", s_terminal_number_outside, chkExcludeTerminalNumber.Checked);

                    s_where_outside += " AND ( " + s_sql + " ) ";
                }
            }

            // Comment

            s_where = "";

            string s_comment = txtComment.Text.Trim();

            if (s_comment != "")
            {
                s_operator = (chkExcludeComment.Checked == true) ? " NOT LIKE " : " LIKE ";

                s_where = String.Format(" AND Comment {0} '%{1}%' ", s_operator, s_comment);

                s_where_inside += s_where;
            }

            if (chk_txtCommentOutside.Checked == true)
            {
                s_where_outside += s_where;
            }
            else
            {
                string s_comment_outside = txtCommentOutside.Text.Trim();

                if (s_error != "")
                {
                    b_error = true;

                    lblCommentError.Text = s_error;
                }
                else if (s_comment_outside != "")
                {
                    s_operator = (chkExcludeComment.Checked == true) ? " NOT LIKE " : " LIKE ";

                    s_where_outside += String.Format(" AND Comment {0} '%{1}%' ", s_operator, s_comment_outside);
                }
            }

            // ID

            s_where = "";

            string s_id_inside = Parameter_TextBox(txtIDInside, "decimal", ref s_error, true, 36);

            if (s_error != "")
            {
                b_error = true;

                lblIDError.Text = s_error;
            }
            else if (s_id_inside != "")
            {
                string s_sql = Parameter_SQL("ID", s_id_inside, chkExcludeID.Checked);

                s_where_inside += " AND ( " + s_sql + " ) ";
            }

            string s_id_outside = Parameter_TextBox(txtIDOutside, "decimal", ref s_error, true, 36);

            if (s_error != "")
            {
                b_error = true;

                lblIDError.Text = s_error;
            }
            else if (s_id_outside != "")
            {
                string s_sql = Parameter_SQL("ID", s_id_outside, chkExcludeID.Checked);

                s_where_outside += " AND ( " + s_sql + " ) ";
            }

            // IF NO ERROR THAN SHOW RESULT ELSE SHOW SEARCH FORM

            if (b_error == true)
            {
                secSearch.Style["display"] = "block";
            }
            else
            {
                ViewState["WhereInside"] = s_where_inside;
                ViewState["WhereOutside"] = s_where_outside;

                ViewState["OrderInside"] = s_order_inside;
                ViewState["OrderOutside"] = s_order_outside;
                Session["WhereInside"] = s_where_inside;
                Session["WhereOutside"] = s_where_outside;
                Session["OrderInside"] = s_order_inside;
                Session["OrderOutside"] = s_order_outside;
                // GET INSIDE & OUTSIDE TABLES

                DataTable dt_inside = new DataTable();
                DataTable dt_outside = new DataTable();

                DataTable dt_inside_sum = new DataTable();
                DataTable dt_outside_sum = new DataTable();

                DataAction.Select(n_user_id, s_where_inside, s_where_outside, s_order_inside, s_order_outside, 1, 1, 20, ref dt_inside, ref dt_inside_sum, ref dt_outside, ref dt_outside_sum, ref s_error, "", "","","");
                //DataAction.Select(n_user_id, s_where_inside, s_where_outside, s_order_inside, s_order_outside, 1, 1, Convert.ToInt32(ddlPageSize.SelectedValue), ref dt_inside, ref dt_inside_sum, ref dt_outside, ref dt_outside_sum, ref s_error);

                ///DataAction.SelectInside(n_user_id, s_where_inside, s_order_inside, 1, 20, ref dt_inside, ref dt_inside_sum, ref s_error);

                if (s_error != "")

                {
                    lblError.Text = s_error;
                    return;
                }

                // USE Cache CLASS TO STORE A LARGE DATA

                Cache.Insert(s_cache_inside, dt_inside, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);
                Cache.Insert(s_cache_outside, dt_outside, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);

                Cache.Insert(s_cache_inside + "_Sum", dt_inside_sum, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);
                Cache.Insert(s_cache_outside + "_Sum", dt_outside_sum, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);

                // SHOW RESULT / HIDE SEARCH FORM

                secSearch.Style.Remove("display");

                pnlSearchResult.Visible = true;

                Enable_Table();

                Bind_Table(dt_inside, dt_outside, dt_inside_sum, dt_outside_sum);

                Bind_Paging(Convert.ToInt32(dt_inside_sum.Rows[0]["PagesCount"]), Convert.ToInt32(dt_outside_sum.Rows[0]["PagesCount"]));
            }
        }

        protected void btnCheckSort_Click(object sender, EventArgs e)
        {
            Session["sortColumnName_Inside"] = hdnColumnName.Value;
            Session["hdnOrderSort_Inside"] = Convert.ToBoolean(hdnOrderSort.Value) == true ? "asc" : "desc";
            //Session["hdnTableType_Inside"] = hdnTableType.Value;
            string sortColumnName = hdnColumnName.Value;
            string sortType = Convert.ToBoolean(hdnOrderSort.Value)==true?"asc":"desc";
            //string sortTableType = hdnTableType.Value;
            ///
            //Session["sortColumnName_Outside"] = hdnColumnName1.Value;
            //Session["hdnOrderSort_Outside"] = Convert.ToBoolean(hdnOrderSort1.Value) == true ? "asc" : "desc";
            //Session["hdnTableType_Outside"] = hdnTableType1.Value;
            string sortColumnName_Out = Session["sortColumnName_Outside"] != null ? Session["sortColumnName_Outside"].ToString() : "";
            string sortType_Out = Session["hdnOrderSort_Outside"] != null ? Session["hdnOrderSort_Outside"].ToString() : "";
            //string sortTableType = hdnTableType1.Value;

            // GET INSIDE & OUTSIDE TABLES

            DataTable dt_inside = new DataTable();
            DataTable dt_outside = new DataTable();

            DataTable dt_inside_sum = new DataTable();
            DataTable dt_outside_sum = new DataTable();

            string s_error = string.Empty;
            string s_where_inside=string.Empty, s_where_outside = string.Empty, s_order_inside = string.Empty, s_order_outside = string.Empty;
            ///////
            s_where_inside = ViewState["WhereInside"].ToString();
            s_where_outside= ViewState["WhereOutside"].ToString();
            s_order_inside= ViewState["OrderInside"].ToString();
            s_order_outside = ViewState["OrderOutside"].ToString();
            s_where_inside = Session["WhereInside"] != null ? Session["WhereInside"].ToString() : "";
            s_where_outside = Session["WhereOutside"] != null ? Session["WhereOutside"].ToString() : "";
            s_order_inside = Session["OrderInside"] != null ? Session["OrderInside"].ToString() : "";
            s_order_outside = Session["OrderOutside"] != null ? Session["OrderOutside"].ToString() : "";
            ///////
            DataAction.Select(n_user_id, s_where_inside, s_where_outside, s_order_inside, s_order_outside, 1, 1, 20, ref dt_inside, ref dt_inside_sum, ref dt_outside, ref dt_outside_sum, ref s_error, sortColumnName, sortType, sortColumnName_Out, sortType_Out);
            //DataAction.Select(n_user_id, s_where_inside, s_where_outside, s_order_inside, s_order_outside, 1, 1, Convert.ToInt32(ddlPageSize.SelectedValue), ref dt_inside, ref dt_inside_sum, ref dt_outside, ref dt_outside_sum, ref s_error);

            ///DataAction.SelectInside(n_user_id, s_where_inside, s_order_inside, 1, 20, ref dt_inside, ref dt_inside_sum, ref s_error);

            if (s_error != "")

            {
                lblError.Text = s_error;
                return;
            }

            // USE Cache CLASS TO STORE A LARGE DATA

            Cache.Insert(s_cache_inside, dt_inside, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);
            Cache.Insert(s_cache_outside, dt_outside, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);

            Cache.Insert(s_cache_inside + "_Sum", dt_inside_sum, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);
            Cache.Insert(s_cache_outside + "_Sum", dt_outside_sum, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);

            // SHOW RESULT / HIDE SEARCH FORM

            secSearch.Style.Remove("display");
            pnlSearchResult.Visible = true;
            Enable_Table();
            Bind_Table(dt_inside, dt_outside, dt_inside_sum, dt_outside_sum);
            Bind_Paging(Convert.ToInt32(dt_inside_sum.Rows[0]["PagesCount"]), Convert.ToInt32(dt_outside_sum.Rows[0]["PagesCount"]));

        }

        protected void btnCheckSort_Click1(object sender, EventArgs e)
        {
            //Session["sortColumnName_Inside"] = hdnColumnName.Value;
            //Session["hdnOrderSort_Inside"] = Convert.ToBoolean(hdnOrderSort.Value) == true ? "asc" : "desc";
            //Session["hdnTableType_Inside"] = hdnTableType.Value;
            string sortColumnName = Session["sortColumnName_Inside"] != null ? Session["sortColumnName_Inside"].ToString() : "";
            string sortType = Session["hdnOrderSort_Inside"] != null ? Session["hdnOrderSort_Inside"].ToString() : "";
            //string sortTableType = hdnTableType.Value;
            //////
            Session["sortColumnName_Outside"] = hdnColumnName1.Value;
            Session["hdnOrderSort_Outside"] = Convert.ToBoolean(hdnOrderSort1.Value) == true ? "asc" : "desc";
            //Session["hdnTableType_Outside"] = hdnTableType1.Value;
            string sortColumnName_Out = hdnColumnName1.Value;
            string sortType_Out = Convert.ToBoolean(hdnOrderSort1.Value) == true ? "asc" : "desc";
            //string sortTableType = hdnTableType1.Value;

            // GET INSIDE & OUTSIDE TABLES

            DataTable dt_inside = new DataTable();
            DataTable dt_outside = new DataTable();

            DataTable dt_inside_sum = new DataTable();
            DataTable dt_outside_sum = new DataTable();

            string s_error = string.Empty;
            string s_where_inside = string.Empty, s_where_outside = string.Empty, s_order_inside = string.Empty, s_order_outside = string.Empty;

            ///////
            s_where_inside = ViewState["WhereInside"].ToString();
            s_where_outside = ViewState["WhereOutside"].ToString();
            s_order_inside = ViewState["OrderInside"].ToString();
            s_order_outside = ViewState["OrderOutside"].ToString();
            s_where_inside = Session["WhereInside"] != null ? Session["WhereInside"].ToString() : "";
            s_where_outside = Session["WhereOutside"] != null ? Session["WhereOutside"].ToString() : "";
            s_order_inside = Session["OrderInside"] != null ? Session["OrderInside"].ToString() : "";
            s_order_outside = Session["OrderOutside"] != null ? Session["OrderOutside"].ToString() : "";
            ///////

            DataAction.Select(n_user_id, s_where_inside, s_where_outside, s_order_inside, s_order_outside, 1, 1, 20, ref dt_inside, ref dt_inside_sum, ref dt_outside, ref dt_outside_sum, ref s_error, sortColumnName, sortType, sortColumnName_Out, sortType_Out);
            //DataAction.Select(n_user_id, s_where_inside, s_where_outside, s_order_inside, s_order_outside, 1, 1, Convert.ToInt32(ddlPageSize.SelectedValue), ref dt_inside, ref dt_inside_sum, ref dt_outside, ref dt_outside_sum, ref s_error);

            ///DataAction.SelectInside(n_user_id, s_where_inside, s_order_inside, 1, 20, ref dt_inside, ref dt_inside_sum, ref s_error);

            if (s_error != "")

            {
                lblError.Text = s_error;
                return;
            }

            // USE Cache CLASS TO STORE A LARGE DATA

            Cache.Insert(s_cache_inside, dt_inside, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);
            Cache.Insert(s_cache_outside, dt_outside, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);

            Cache.Insert(s_cache_inside + "_Sum", dt_inside_sum, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);
            Cache.Insert(s_cache_outside + "_Sum", dt_outside_sum, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);

            // SHOW RESULT / HIDE SEARCH FORM

            secSearch.Style.Remove("display");
            pnlSearchResult.Visible = true;
            Enable_Table();
            Bind_Table(dt_inside, dt_outside, dt_inside_sum, dt_outside_sum);
            Bind_Paging(Convert.ToInt32(dt_inside_sum.Rows[0]["PagesCount"]), Convert.ToInt32(dt_outside_sum.Rows[0]["PagesCount"]));

        }

        protected void btnCheck_Click(object sender, EventArgs e)
        {
            string s_error = "";
            string s_mode = Get_Mode();

            DataTable dt_inside = null, dt_inside_sum = null, dt_outside = null, dt_outside_sum = null, dt_source = null;

            if (Allow_Recalculate() == false) { goto Finish; }

            switch (s_mode)
            {
                case "payment":
                    dt_inside = (DataTable)ViewState["TablePaymentData"];
                    dt_source = (DataTable)ViewState["TablePaymentSource"];
                    break;
                case "match":
                    dt_inside = ((DataTable)Cache[s_cache_inside_match]).Copy();
                    dt_outside = ((DataTable)Cache[s_cache_outside_match]).Copy();
                    break;
                case "matching":
                case "not-matching":
                    dt_inside = ((DataTable)Cache[s_cache_inside]).Copy();
                    dt_outside = ((DataTable)Cache[s_cache_outside]).Copy();
                    dt_inside_sum = ((DataTable)Cache[s_cache_inside + "_Sum"]).Copy();
                    dt_outside_sum = ((DataTable)Cache[s_cache_outside + "_Sum"]).Copy();
                    break;
            }

            string s_select_inside = hidSelectInside.Value;
            string s_select_outside = hidSelectOutside.Value;

            if (s_select_inside == "" && s_select_outside == "")
            {
                lblInsideRowsSelected.Text = "0";
                lblInsideAmountSelected.Text = "0.00";
                lblInsideRowsRemaining.Text = lblInsideRows.Text;
                lblInsideAmountRemaining.Text = lblInsideAmount.Text;
                lblOutsideRowsSelected.Text = "0";
                lblOutsideAmountSelected.Text = "0.00";
                lblOutsideRowsRemaining.Text = lblOutsideRows.Text;
                lblOutsideAmountRemaining.Text = lblOutsideAmount.Text;
                /// Banance amont blank
                txtBalanceAmount.Text = "0";
                goto Finish;
            }

            if (s_mode == "payment")
            {
                int n_payment_rows = 0;
                double n_payment_amount = 0;

                DataAction.Recalculate_Payment(dt_inside, s_select_inside, ref n_payment_rows, ref n_payment_amount, ref s_error);

                hidPaymentsAmount_Selected.Value = String.Format("{0}", n_payment_amount);

                lblInsideRowsSelected.Text = String.Format("{0:n0}", n_payment_rows);
                lblInsideAmountSelected.Text = String.Format("{0:n2}", Math.Round(n_payment_amount, 2));

                trInsideSelected.Visible = true;
                pnlPaymentChange.Visible = true;
            }
            else
            {
                int n_inside_rows = (s_mode == "match") ? dt_inside.Rows.Count : Convert.ToInt32(dt_inside_sum.Rows[0]["RowsCount"]);
                int n_outside_rows = (s_mode == "match") ? dt_outside.Rows.Count : Convert.ToInt32(dt_outside_sum.Rows[0]["RowsCount"]);

                double n_inside_amount = (s_mode == "match") ? dt_inside.AsEnumerable().Sum(dr => dr.Field<double>("DutyPaymentAmount")) : Convert.ToDouble(dt_inside_sum.Rows[0]["AmountSum"]);
                double n_outside_amount = (s_mode == "match") ? dt_outside.AsEnumerable().Sum(dr => dr.Field<double>("DutyPaymentAmount")) : Convert.ToDouble(dt_outside_sum.Rows[0]["AmountSum"]);

                int n_inside_rows_selected = 0, n_outside_rows_selected = 0;
                double n_inside_amount_selected = 0, n_outside_amount_selected = 0;

                int n_inside_rows_remaining = 0, n_outside_rows_remaining = 0;
                double n_inside_amount_remaining = 0, n_outside_amount_remaining = 0;

                int n_company_id = 0;

                switch (s_mode)
                {
                    case "match":
                        DataAction.Recalculate_Match(dt_inside, dt_outside, s_select_inside, s_select_outside, ref n_inside_rows_remaining, ref n_outside_rows_remaining, ref n_inside_amount_remaining, ref n_outside_amount_remaining, ref s_error);
                        break;
                    case "matching":
                        DataAction.Recalculate_Matching(n_user_id, s_select_inside, s_select_outside, ref n_inside_rows_selected, ref n_outside_rows_selected, ref n_inside_amount_selected, ref n_outside_amount_selected, ref s_error);
                        break;
                    case "not-matching":
                        DataAction.Recalculate_Not_Matching(n_user_id, s_select_inside, s_select_outside, ref n_company_id, ref n_inside_rows_selected, ref n_outside_rows_selected, ref n_inside_amount_selected, ref n_outside_amount_selected, ref s_error);
                        break;
                }

                if (s_mode == "match")
                {
                    n_inside_rows_selected = n_inside_rows - n_inside_rows_remaining;
                    n_outside_rows_selected = n_outside_rows - n_outside_rows_remaining;

                    n_inside_amount_selected = n_inside_amount - n_inside_amount_remaining;
                    n_outside_amount_selected = n_outside_amount - n_outside_amount_remaining;
                }
                else
                {
                    n_inside_rows_remaining = n_inside_rows - n_inside_rows_selected;
                    n_outside_rows_remaining = n_outside_rows - n_outside_rows_selected;

                    n_inside_amount_remaining = n_inside_amount - n_inside_amount_selected;
                    n_outside_amount_remaining = n_outside_amount - n_outside_amount_selected;
                }

                Bind_Matching_Balance(n_company_id, n_inside_rows_selected, n_outside_rows_selected, n_inside_amount_selected, n_outside_amount_selected, n_inside_rows_remaining, n_outside_rows_remaining, n_inside_amount_remaining, n_outside_amount_remaining);
            }

            //ddlInsidePage.Enabled = false;
            //ddlOutsidePage.Enabled = false;

            //tdRecalculate.Visible = false;
            //tdPayment.Visible = false;
            //tdMatchingAuto.Visible = false;
            //tdStatus.Visible = false;
            //tdComment.Visible = false;

            btnPaymentChange.Visible = false;
            btnPaymentRecalculate.Enabled = (s_error == "");
            btnMatchingBalanceChange.Enabled = (s_error == "");

            Finish:

            lblError.Text = s_error;

            if (s_mode == "payment")
            {
                Bind_Table_Payment(dt_inside, dt_source);
            }
            else
            {
                Bind_Table(dt_inside, dt_outside, dt_inside_sum, dt_outside_sum);
            }


        }

        protected void Recalculate_Click(object sender, EventArgs e)
        {
            string s_error = "";
            string s_mode = Get_Mode();

            DataTable dt_inside = null, dt_inside_sum = null, dt_outside = null, dt_outside_sum = null, dt_source = null;

            if (Allow_Recalculate() == false) { goto Finish; }

            switch (s_mode)
            {
                case "payment":
                    dt_inside = (DataTable)ViewState["TablePaymentData"];
                    dt_source = (DataTable)ViewState["TablePaymentSource"];
                    break;
                case "match":
                    dt_inside = ((DataTable)Cache[s_cache_inside_match]).Copy();
                    dt_outside = ((DataTable)Cache[s_cache_outside_match]).Copy();
                    break;
                case "matching":
                case "not-matching":
                    dt_inside = ((DataTable)Cache[s_cache_inside]).Copy();
                    dt_outside = ((DataTable)Cache[s_cache_outside]).Copy();
                    dt_inside_sum = ((DataTable)Cache[s_cache_inside + "_Sum"]).Copy();
                    dt_outside_sum = ((DataTable)Cache[s_cache_outside + "_Sum"]).Copy();
                    // Enable Save changes button on recalculate for matching 
                    btnMatchingBalanceChange.Enabled = true;
                    chkAllCheckBox.Disabled = true;
                    chkAllCheckBox.Checked = false;
                    break;
            }

            string s_select_inside = hidSelectInside.Value;
            string s_select_outside = hidSelectOutside.Value;

            if (s_select_inside == "" && s_select_outside == "") { goto Finish; }

            if (s_mode == "payment")
            {
                int n_payment_rows = 0;
                double n_payment_amount = 0;

                DataAction.Recalculate_Payment(dt_inside, s_select_inside, ref n_payment_rows, ref n_payment_amount, ref s_error);

                hidPaymentsAmount_Selected.Value = String.Format("{0}", n_payment_amount);

                lblInsideRowsSelected.Text = String.Format("{0:n0}", n_payment_rows);
                lblInsideAmountSelected.Text = String.Format("{0:n2}", Math.Round(n_payment_amount, 2));

                trInsideSelected.Visible = true;
                pnlPaymentChange.Visible = true;
            }
            else
            {
                int n_inside_rows = (s_mode == "match") ? dt_inside.Rows.Count : Convert.ToInt32(dt_inside_sum.Rows[0]["RowsCount"]);
                int n_outside_rows = (s_mode == "match") ? dt_outside.Rows.Count : Convert.ToInt32(dt_outside_sum.Rows[0]["RowsCount"]);

                double n_inside_amount = (s_mode == "match") ? dt_inside.AsEnumerable().Sum(dr => dr.Field<double>("DutyPaymentAmount")) : Convert.ToDouble(dt_inside_sum.Rows[0]["AmountSum"]);
                double n_outside_amount = (s_mode == "match") ? dt_outside.AsEnumerable().Sum(dr => dr.Field<double>("DutyPaymentAmount")) : Convert.ToDouble(dt_outside_sum.Rows[0]["AmountSum"]);

                int n_inside_rows_selected = 0, n_outside_rows_selected = 0;
                double n_inside_amount_selected = 0, n_outside_amount_selected = 0;

                int n_inside_rows_remaining = 0, n_outside_rows_remaining = 0;
                double n_inside_amount_remaining = 0, n_outside_amount_remaining = 0;

                int n_company_id = 0;

                switch (s_mode)
                {
                    case "match":
                        DataAction.Recalculate_Match(dt_inside, dt_outside, s_select_inside, s_select_outside, ref n_inside_rows_remaining, ref n_outside_rows_remaining, ref n_inside_amount_remaining, ref n_outside_amount_remaining, ref s_error);
                        break;
                    case "matching":
                        DataAction.Recalculate_Matching(n_user_id, s_select_inside, s_select_outside, ref n_inside_rows_selected, ref n_outside_rows_selected, ref n_inside_amount_selected, ref n_outside_amount_selected, ref s_error);
                        break;
                    case "not-matching":
                        DataAction.Recalculate_Not_Matching(n_user_id, s_select_inside, s_select_outside, ref n_company_id, ref n_inside_rows_selected, ref n_outside_rows_selected, ref n_inside_amount_selected, ref n_outside_amount_selected, ref s_error);
                        break;
                }

                if (s_mode == "match")
                {
                    n_inside_rows_selected = n_inside_rows - n_inside_rows_remaining;
                    n_outside_rows_selected = n_outside_rows - n_outside_rows_remaining;

                    n_inside_amount_selected = n_inside_amount - n_inside_amount_remaining;
                    n_outside_amount_selected = n_outside_amount - n_outside_amount_remaining;
                }
                else
                {
                    n_inside_rows_remaining = n_inside_rows - n_inside_rows_selected;
                    n_outside_rows_remaining = n_outside_rows - n_outside_rows_selected;

                    n_inside_amount_remaining = n_inside_amount - n_inside_amount_selected;
                    n_outside_amount_remaining = n_outside_amount - n_outside_amount_selected;
                }

                Bind_Matching_Balance(n_company_id, n_inside_rows_selected, n_outside_rows_selected, n_inside_amount_selected, n_outside_amount_selected, n_inside_rows_remaining, n_outside_rows_remaining, n_inside_amount_remaining, n_outside_amount_remaining);
            }

            //ddlInsidePage.Enabled = false;
            //ddlOutsidePage.Enabled = false;

            //tdRecalculate.Visible = false;
            //tdPayment.Visible = false;
            //tdMatchingAuto.Visible = false;
            //tdStatus.Visible = false;
            //tdComment.Visible = false;

            btnPaymentChange.Visible = false;
            btnPaymentRecalculate.Enabled = (s_error == "");
            btnMatchingBalanceChange.Enabled = (s_error == "");

            Finish:

            lblError.Text = s_error;

            if (s_mode == "payment")
            {
                Bind_Table_Payment(dt_inside, dt_source);
            }
            else
            {
                Bind_Table(dt_inside, dt_outside, dt_inside_sum, dt_outside_sum);
            }
        }

        protected void Return_Click(object sender, EventArgs e)
        {
            DataTable dt_inside = null, dt_inside_sum = null, dt_outside = null, dt_outside_sum = null;

            dt_inside = ((DataTable)Cache[s_cache_inside]).Copy();
            dt_inside_sum = ((DataTable)Cache[s_cache_inside + "_Sum"]).Copy();

            dt_outside = ((DataTable)Cache[s_cache_outside]).Copy();
            dt_outside_sum = ((DataTable)Cache[s_cache_outside + "_Sum"]).Copy();

            divInside.Visible = true;
            divOutside.Visible = true;
            divDataSeparator.Visible = true;

            divSource.Visible = false;
            divSourceSeparator.Visible = false;

            Enable_Table();

            Bind_Table(dt_inside, dt_outside, dt_inside_sum, dt_outside_sum);
        }

        protected void Match_Click(object sender, EventArgs e)
        {
            string s_query_id = hidQueryID.Value;

            DataTable dt_inside = new DataTable();
            DataTable dt_outside = new DataTable();

            string s_error = "";

            DataAction.Select_Match(n_user_id, s_query_id, ref dt_inside, ref dt_outside, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            Cache.Insert(s_cache_inside_match, dt_inside, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);
            Cache.Insert(s_cache_outside_match, dt_outside, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);

            Enable_Table("", s_query_id);

            Bind_Table(dt_inside, dt_outside);
        }

        protected void Matching_Command(object sender, CommandEventArgs e)
        {
            //bool ChkOne = chkOne.Checked;
            //bool ChkMany = chkMany.Checked;
            //bool ChkZero = chkZero.Checked;

            string s_error = "";

            Enable_Table();

            DataTable dt_inside = ((DataTable)Cache[s_cache_inside]).Copy();
            DataTable dt_inside_sum = ((DataTable)Cache[s_cache_inside + "_Sum"]).Copy();

            DataTable dt_outside = ((DataTable)Cache[s_cache_outside]).Copy();
            DataTable dt_outside_sum = ((DataTable)Cache[s_cache_outside + "_Sum"]).Copy();

            bool b_show_form = false;

            if (Get_Mode() != "not-matching" || Allow_Recalculate() == false) { goto Finish; }

            if (dt_inside.Rows.Count == 0 || dt_outside.Rows.Count == 0)
            {
                s_error = "Can't create matching without data in both sides (inside & outside).";
                goto Finish;
            }

            switch (e.CommandArgument.ToString())
            {
                case "Confirm":
                    DataTable dt_data_field = new DataTable();

                    DB.Bind_Data_Table("sprData_Field_List", ref dt_data_field, ref s_error);

                    if (s_error != "") { goto Finish; }

                    foreach (HtmlTableCell o_cell in trMatchingFieldName.Cells)
                    {
                        string s_sql = String.Format(" FieldName = '{0}' ", o_cell.InnerHtml.Trim());

                        DataRow o_row = dt_data_field.Select(s_sql).FirstOrDefault();

                        if (o_row != null)
                        {
                            o_cell.InnerHtml = o_row["FieldDescription"].ToString();
                        }
                    }

                    b_show_form = true;

                    break;
                case "Save":
                    // GET lst_matching_field FROM List_Strategy_Field AND INSERT CardPrefix AFTER CardNumber

                    List<string> lst_matching_field = StrategyModel.List_Strategy_Field();

                    int i_card_number = lst_matching_field.FindIndex(a => a == "CardNumber");

                    lst_matching_field.Insert(i_card_number + 1, "CardPrefix");

                    // CREATE QUERY TABLE

                    DataTable dt_query = new DataTable();

                    dt_query.Columns.Add("ID", typeof(string));
                    dt_query.Columns.Add("MatchingTypeID", typeof(int));
                    dt_query.Columns.Add("QueryNumber", typeof(int));

                    DataRow dr_query = dt_query.NewRow();

                    dr_query["ID"] = Guid.NewGuid().ToString();
                    dr_query["MatchingTypeID"] = 1;
                    dr_query["QueryNumber"] = 1;

                    foreach (string s_field in lst_matching_field)
                    {
                        dt_query.Columns.Add(s_field);

                        string s_checkbox_id = String.Format("chk{0}", s_field);

                        CheckBox o_checkbox = (CheckBox)tblMatchingField.FindControl(s_checkbox_id);

                        dr_query[s_field] = (o_checkbox.Checked == true) ? "*" : "";
                    }
                    //if (ChkOne)
                    //{
                    dt_query.Rows.Add(dr_query);
                    //}

                    float n_tolerance_transaction_gross_amount = 0, n_tolerance_duty_payment_amount = 0;

                    float.TryParse(txtToleranceTransactionGrossAmount.Text, out n_tolerance_transaction_gross_amount);
                    float.TryParse(txtToleranceDutyPaymentAmount.Text, out n_tolerance_duty_payment_amount);

                    int n_tolerance_transaction_date = 0, n_tolerance_payment_date = 0, n_tolerance_voucher_number = 0, n_tolerance_confirmation_number = 0;

                    int.TryParse(txtToleranceTransactionDate.Text, out n_tolerance_transaction_date);
                    int.TryParse(txtTolerancePaymentDate.Text, out n_tolerance_payment_date);
                    int.TryParse(txtToleranceVoucherNumber.Text, out n_tolerance_voucher_number);
                    int.TryParse(txtToleranceConfirmationNumber.Text, out n_tolerance_confirmation_number);

                    bool _b_tolerance = (n_tolerance_transaction_gross_amount != 0 || n_tolerance_duty_payment_amount != 0 || n_tolerance_transaction_date != 0 || n_tolerance_payment_date != 0 || n_tolerance_voucher_number != 0 || n_tolerance_confirmation_number != 0);

                    if (_b_tolerance == true)
                    {
                        DataTable dt_temp = dt_query.Copy();
                        DataRow dr_temp = dt_temp.Rows[0];

                        dr_temp["ID"] = Guid.NewGuid().ToString();
                        dr_temp["QueryNumber"] = 2;

                        if (n_tolerance_transaction_gross_amount != 0) { dr_temp["TransactionGrossAmount"] = n_tolerance_transaction_gross_amount; }
                        if (n_tolerance_duty_payment_amount != 0) { dr_temp["DutyPaymentAmount"] = n_tolerance_duty_payment_amount; }
                        if (n_tolerance_transaction_date != 0) { dr_temp["TransactionDate"] = n_tolerance_transaction_date; }
                        if (n_tolerance_payment_date != 0) { dr_temp["PaymentDate"] = n_tolerance_payment_date; }
                        if (n_tolerance_voucher_number != 0) { dr_temp["VoucherNumber"] = n_tolerance_voucher_number; }
                        if (n_tolerance_confirmation_number != 0) { dr_temp["ConfirmationNumber"] = n_tolerance_confirmation_number; }

                        dt_query.Merge(dt_temp);
                    }

                    DataTable dt_query_many_to_many = dt_query.Copy();

                    dt_query_many_to_many.Rows[0]["ID"] = Guid.NewGuid().ToString();
                    dt_query_many_to_many.Rows[0]["MatchingTypeID"] = 2;
                    dt_query_many_to_many.Rows[0]["DutyPaymentNumber"] = "";
                    dt_query_many_to_many.Rows[0]["PaymentDate"] = "";

                    if (dt_query_many_to_many.Rows.Count > 1)
                    {
                        dt_query_many_to_many.Rows[1]["ID"] = Guid.NewGuid().ToString();
                        dt_query_many_to_many.Rows[1]["MatchingTypeID"] = 2;
                        dt_query_many_to_many.Rows[1]["DutyPaymentNumber"] = "";
                        dt_query_many_to_many.Rows[1]["PaymentDate"] = "";
                    }

                    //if (ChkMany)
                    //{
                    dt_query.Merge(dt_query_many_to_many);
                    //}

                    string s_where_inside = ViewState["WhereInside"].ToString();
                    string s_where_outside = ViewState["WhereOutside"].ToString();

                    int n_matching_id = MatchingAction.Update_Quick(n_user_id, s_where_inside, s_where_outside, dt_query, ref s_error);

                    if (s_error != "") { goto Finish; }

                    if (n_matching_id <= 0)
                    {
                        s_error = "Matching was not created.";
                        goto Finish;
                    }

                    lblMessage.Text = String.Format("Matching #{0} successfully created. Matching currently in process.", n_matching_id);

                    break;
            }

            Finish:

            divMatchingAuto.Visible = b_show_form;

            tdRecalculate.Visible = !b_show_form;
            tdMatchingAuto.Visible = !b_show_form;
            tdStatus.Visible = !b_show_form;
            tdComment.Visible = !b_show_form;

            if (s_error != "")
            {
                lblError.Text = s_error;
            }

            Bind_Table(dt_inside, dt_outside, dt_inside_sum, dt_outside_sum);
        }

        protected void Payment_Click(object sender, EventArgs e)
        {
            string s_error = "";
            string s_table = hidTable.Value;
            string s_unique_id = hidUniqueID.Value;

            DataTable dt_data = new DataTable();
            DataTable dt_source = new DataTable();

            DataAction.Select_Payment(n_user_id, s_unique_id, s_table, ref dt_data, ref dt_source, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            ViewState["TablePaymentData"] = dt_data;
            ViewState["TablePaymentSource"] = dt_source;

            string[] arr_columns_remove = { "UserID", "DataFileID" };

            foreach (string s_column in arr_columns_remove)
            {
                if (dt_source.Columns.Contains(s_column)) { dt_source.Columns.Remove(s_column); }
            }

            Enable_Table(s_unique_id);

            Bind_Table_Payment(dt_data, dt_source);
        }

        protected void Payment_Recalculate(object sender, EventArgs e)
        {
            string s_error = "";

            DataTable dt_data = null, dt_source = null;

            if (Allow_Recalculate() == false) { goto Finish; }

            dt_data = (DataTable)ViewState["TablePaymentData"];
            dt_source = (DataTable)ViewState["TablePaymentSource"];

            int n_split_payments_count = 0;
            double n_split_first_payment_amount = 0, n_split_remaining_payments_amount = 0;

            Validate_Payment(dt_data.Rows.Count, ref n_split_payments_count, ref n_split_first_payment_amount, ref n_split_remaining_payments_amount, ref s_error);

            if (s_error != "") { goto Finish; }

            // GET SORTED LIST OF SELECTED PAYMENTS NUMBERS - lst_payment_number / n_first_payment_number

            string s_payment_number_array = hidSelectInside.Value.Trim();

            List<int> lst_payment_number = Common.Convert_To_List(s_payment_number_array);

            int n_first_payment_number = lst_payment_number[0];

            // GET FIRST SELECTED PAYMENT ROW : dr_data / b_splitted / n_remaining_payments_amount / d_payment_date

            string s_sql = String.Format(" DutyPaymentNumber = {0} ", n_first_payment_number);

            DataRow dr_data = dt_data.Select(s_sql).First();

            bool b_splitted = Convert.ToBoolean(dr_data["IsSplitted"]);

            double n_remaining_payments_amount = 0;

            if (b_splitted == true)
            {
                if (dt_data.Rows.Count == 1)
                {
                    s_error = "Error : Payment row marked as splitted, but Payment Group contains only 1 row.";
                    goto Finish;
                }

                n_remaining_payments_amount = Convert.ToDouble(dr_data["RemainingPaymentsAmount"]) + Convert.ToDouble(dr_data["DutyPaymentAmount"]);
            }
            else
            {
                if (dt_data.Rows.Count > 1)
                {
                    s_error = "Error : Payment Group contains row that not marked as splitted.";
                    goto Finish;
                }

                n_remaining_payments_amount = Convert.ToDouble(dr_data["TransactionGrossAmount"]);
            }

            n_remaining_payments_amount = n_remaining_payments_amount - n_split_first_payment_amount;

            DateTime? d_payment_date = null;

            if (dr_data["PaymentDate"].ToString().Trim() != "")
            {
                d_payment_date = (DateTime?)dr_data["PaymentDate"];
            }

            // GET OperationType # 7 FOR BALANCE TABLE // !!! 'רישום הפרש' !!!

            DataTable dt_operation_type = (DataTable)ViewState["TableOperationType"];

            string s_operation_type = dt_operation_type.Select(" ID = 7 ")[0]["OperationTypeName"].ToString();

            // GET BALANCE TABLE

            s_sql = String.Format(" DutyPaymentNumber IN ( {0} ) ", s_payment_number_array);

            DataTable dt_balance = dt_data.Select(s_sql).CopyToDataTable();

            foreach (DataRow dr_balance in dt_balance.Rows)
            {
                dr_balance["OperationType"] = s_operation_type;     // # 7 - רישום הפרש
                dr_balance["TransactionGrossAmount"] = 0 - Convert.ToDouble(dr_balance["TransactionGrossAmount"]);
                dr_balance["FirstPaymentAmount"] = 0 - Convert.ToDouble(dr_balance["FirstPaymentAmount"]);
                dr_balance["DutyPaymentAmount"] = 0 - Convert.ToDouble(dr_balance["DutyPaymentAmount"]);
                dr_balance["RemainingPaymentsAmount"] = 0 - Convert.ToDouble(dr_balance["RemainingPaymentsAmount"]);
                dr_balance["IsBalance"] = true;
            }

            // GET SPLIT TABLE

            int n_active_payments_count = dt_data.Select(" MatchingActionName IS NULL OR MatchingActionName <> 'Resplit' ").Length;

            if (n_active_payments_count < 1)
            {
                s_error = "Error : Payment Group not contains active rows.";
                goto Finish;
            }

            int n_new_payments_count = n_active_payments_count - lst_payment_number.Count + n_split_payments_count;

            double n_split_duty_payment_amount = n_split_first_payment_amount;

            DataTable dt_split = dt_data.Clone();

            DataRow dr_first_payment = dt_split.NewRow();

            dr_first_payment.ItemArray = (object[])dr_data.ItemArray.Clone();

            dr_first_payment["PaymentsCount"] = n_new_payments_count;
            dr_first_payment["FirstPaymentAmount"] = n_split_first_payment_amount;
            dr_first_payment["DutyPaymentAmount"] = n_split_duty_payment_amount;
            dr_first_payment["RemainingPaymentsAmount"] = n_remaining_payments_amount;
            dr_first_payment["IsSplitted"] = (n_new_payments_count > 1);
            dr_first_payment["IsBalance"] = true;

            dt_split.Rows.Add(dr_first_payment);

            if (n_split_payments_count > 1)
            {
                // BEGIN SPLIT FROM NEXT PAYMENT (FIRST ROW ALREADY ADDED TO dt_split).

                int n_split_start_payment_number = n_first_payment_number + 1;
                int n_split_remaining_payments_count = n_split_payments_count - 1;
                int n_split_end_payment_number = n_split_start_payment_number + n_split_remaining_payments_count - 1;

                n_split_duty_payment_amount = n_split_remaining_payments_amount / n_split_remaining_payments_count;

                DataAction.Split_Row(ref dt_split, dr_first_payment, n_split_start_payment_number, n_split_end_payment_number, n_split_duty_payment_amount, n_remaining_payments_amount, d_payment_date);
            }

            // KEEP VARIABLES FOR USING IN Payment_Change() FUNCTION

            ViewState["PaymentsCount_Split"] = n_split_payments_count;
            ViewState["FirstPaymentAmount_Split"] = n_split_first_payment_amount;
            ViewState["DutyPaymentAmount_Split"] = n_split_duty_payment_amount;
            ViewState["RemainingPaymentsAmount"] = n_remaining_payments_amount;
            ViewState["IsSplitted"] = (n_new_payments_count > 1);

            ViewState["TableCanceledID"] = new DataView(dt_balance).ToTable(false, "ID");

            // BIND BALANCE & SPLIT TABLES

            foreach (string s_column in arr_payment_exclude_columns)
            {
                if (dt_balance.Columns.Contains(s_column)) { dt_balance.Columns.Remove(s_column); }
                if (dt_split.Columns.Contains(s_column)) { dt_split.Columns.Remove(s_column); }
            }

            divBalance.Visible = true;
            gvBalance.DataSource = dt_balance;
            gvBalance.DataBind();

            divSplit.Visible = true;
            gvSplit.DataSource = dt_split;
            gvSplit.DataBind();

            btnPaymentChange.Visible = (dt_balance.Rows.Count > 0 && dt_split.Rows.Count > 0);

            Finish:

            if (s_error != "")
            {
                lblError.Text = s_error;

                divBalance.Visible = false;
                divSplit.Visible = false;

                btnPaymentChange.Visible = false;
            }
            else if (btnPaymentChange.Visible == false)
            {
                lblError.Text = "Error : 'Balance' or 'Split' is missing.";
            }

            Bind_Table_Payment(dt_data, dt_source);
        }

        protected void Payment_Restore(object sender, EventArgs e)
        {
            string s_error = "";

            DataTable dt_data = null, dt_source = null;

            if (Allow_Recalculate() == false) { goto Finish; }

            dt_data = (DataTable)ViewState["TablePaymentData"];
            dt_source = (DataTable)ViewState["TablePaymentSource"];

            if (dt_data.Select(" MatchingActionName IS NOT NULL AND MatchingActionName <> 'Resplit' AND IsBalance = True ").FirstOrDefault() != null)
            {
                s_error = "Can't restore 'Payment Group' when contains match rows marked as 'Balance'.";
                goto Finish;
            }

            int n_duty_payment_number_resplit_first = 0, n_duty_payment_number_match_last = 0;

            DataRow dr_resplit = dt_data.Select(" MatchingActionName IS NOT NULL AND MatchingActionName = 'Resplit' ").FirstOrDefault();
            DataRow dr_match = dt_data.Select(" MatchingActionName IS NOT NULL AND MatchingActionName <> 'Resplit' ").LastOrDefault();

            if (dr_resplit != null) { int.TryParse(dr_resplit["DutyPaymentNumber"].ToString(), out n_duty_payment_number_resplit_first); }
            if (dr_match != null) { int.TryParse(dr_match["DutyPaymentNumber"].ToString(), out n_duty_payment_number_match_last); }

            if (n_duty_payment_number_resplit_first < n_duty_payment_number_match_last)
            {
                s_error = "Can't restore 'Payment Group' when contains match rows after rows marked as 'Resplit'.";
                goto Finish;
            }

            DataRow[] dr_restore = dt_data.Select(" IsBalance = False ");

            if (dr_restore.Length == 0)
            {
                s_error = "Error : 'Payment Group' not contains original rows.";
                goto Finish;
            }

            dr_restore = dr_restore.OrderBy(dr => dr["DutyPaymentNumber"]).ToArray();

            DataTable dt_restore = dr_restore.CopyToDataTable();

            int n_payments_count = dt_restore.Rows.Count;

            DateTime? d_payment_date = null;

            for (int i = 0; i < n_payments_count; i++)
            {
                DataRow dr = dt_restore.Rows[i];

                dr["PaymentsCount"] = n_payments_count;
                dr["DutyPaymentNumber"] = i + 1;

                if (dr["MatchingActionName"].ToString() == "Resplit")
                {
                    dr["QueryNumber"] = DBNull.Value;
                    dr["MatchingTypeName"] = "";
                    dr["MatchingActionName"] = "";
                    dr["QueryID"] = "";
                }

                if (i == 0 && dr["PaymentDate"].ToString().Trim() != "")
                {
                    d_payment_date = (DateTime?)dr["PaymentDate"];
                }

                if (d_payment_date != null)
                {
                    dr["PaymentDate"] = d_payment_date.Value.AddMonths(i);
                }
            }

            tdRecalculate.Visible = false;

            hidSelectInside.Value = "payment-restore-mode";      // FOR MODE INDICATION AND DISABLE THE CHECKBOX

            btnPaymentRestore.Visible = false;
            lblPaymentChange.Visible = true;

            Bind_Table_Payment(dt_restore, dt_source);

            return;

            Finish:

            lblError.Text = s_error;

            Bind_Table_Payment(dt_data, dt_source);
        }

        protected void Payment_Change(object sender, EventArgs e)
        {
            string s_error = "";

            DataTable dt_inside = ((DataTable)Cache[s_cache_inside]).Copy();
            DataTable dt_inside_sum = ((DataTable)Cache[s_cache_inside + "_Sum"]).Copy();

            DataTable dt_outside = ((DataTable)Cache[s_cache_outside]).Copy();
            DataTable dt_outside_sum = ((DataTable)Cache[s_cache_outside + "_Sum"]).Copy();

            if (Get_Mode() != "payment") { goto Error; }

            string s_unique_id = hidUniqueID.Value;
            string s_table = hidTable.Value;

            int n_rows_affected = 0;

            if (hidSelectInside.Value == "payment-restore-mode")
            {
                if (Allow_Payment_Restore() == false) { goto Error; }

                n_rows_affected = DataAction.Restore_Payment(n_user_id, hidUniqueID.Value, ref s_error);
            }
            else
            {
                if (Allow_Recalculate() == false) { goto Error; }

                int n_split_payments_count = (int)ViewState["PaymentsCount_Split"];

                double n_split_first_payment_amount = (double)ViewState["FirstPaymentAmount_Split"];
                double n_split_duty_payment_amount = (double)ViewState["DutyPaymentAmount_Split"];
                double n_remaining_payments_amount = (double)ViewState["RemainingPaymentsAmount"];

                bool b_splitted = (bool)ViewState["IsSplitted"];

                DataTable dt_canceled = (DataTable)ViewState["TableCanceledID"];

                n_rows_affected = DataAction.Change_Payment(dt_canceled, n_user_id, n_split_payments_count, n_split_first_payment_amount, n_split_duty_payment_amount, n_remaining_payments_amount, s_unique_id, b_splitted, ref s_error);
            }

            if (s_error != "") { goto Error; }

            // SELECT INSIDE & OTSIDE TABLES FROM DB

            string s_where_inside = ViewState["WhereInside"].ToString();
            string s_where_outside = ViewState["WhereOutside"].ToString();

            string s_order_inside = ViewState["OrderInside"].ToString();
            string s_order_outside = ViewState["OrderOutside"].ToString();

            int n_page_inside = Convert.ToInt32(ddlInsidePage.SelectedValue);
            int n_page_outside = Convert.ToInt32(ddlOutsidePage.SelectedValue);

            DataAction.Select(n_user_id, s_where_inside, s_where_outside, s_order_inside, s_order_outside, n_page_inside, n_page_outside, 100, ref dt_inside, ref dt_inside_sum, ref dt_outside, ref dt_outside_sum, ref s_error, "", "", "", "");

            if (s_error != "") { goto Error; }

            // USE Cache CLASS TO STORE A LARGE DATA

            Cache.Insert(s_cache_inside, dt_inside, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);
            Cache.Insert(s_cache_outside, dt_outside, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);

            Cache.Insert(s_cache_inside + "_Sum", dt_inside_sum, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);
            Cache.Insert(s_cache_outside + "_Sum", dt_outside_sum, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);

            // DISPLAY UPDATED PAYMENT GROUP

            hidUniqueID.Value = s_unique_id;
            hidTable.Value = s_table;

            Payment_Click(sender, e);

            lblMessage.Text = (n_rows_affected > 0) ? "Item/s successfully updated." : "No item was updated.";

            return;

            Error:

            lblError.Text = s_error;

            Bind_Table(dt_inside, dt_outside, dt_inside_sum, dt_outside_sum);
        }

        protected void Status_Change(object sender, EventArgs e)
        {
            DataTable dt_inside = ((DataTable)Cache[s_cache_inside]).Copy();
            DataTable dt_inside_sum = ((DataTable)Cache[s_cache_inside + "_Sum"]).Copy();

            DataTable dt_outside = ((DataTable)Cache[s_cache_outside]).Copy();
            DataTable dt_outside_sum = ((DataTable)Cache[s_cache_outside + "_Sum"]).Copy();

            int n_status_id = Convert.ToInt32(ddlStatusChange.SelectedValue);

            string s_inside_id_array = hidSelectInside.Value;
            string s_outside_id_array = hidSelectOutside.Value;

            if (n_status_id == -1 || (s_inside_id_array == "" && s_outside_id_array == "")) { goto Finish; }

            string s_error = "";

            int n_rows_affected = DataAction.Change_Status(n_user_id, n_status_id, s_inside_id_array, s_outside_id_array, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                goto Finish;
            }

            // SELECT INSIDE & OTSIDE TABLES FROM DB

            string s_where_inside = ViewState["WhereInside"].ToString();
            string s_where_outside = ViewState["WhereOutside"].ToString();

            string s_order_inside = ViewState["OrderInside"].ToString();
            string s_order_outside = ViewState["OrderOutside"].ToString();

            int n_page_inside = Convert.ToInt32(ddlInsidePage.SelectedValue);
            int n_page_outside = Convert.ToInt32(ddlOutsidePage.SelectedValue);

            DataAction.Select(n_user_id, s_where_inside, s_where_outside, s_order_inside, s_order_outside, n_page_inside, n_page_outside, 100, ref dt_inside, ref dt_inside_sum, ref dt_outside, ref dt_outside_sum, ref s_error, "", "", "", "");

            if (s_error != "")
            {
                lblError.Text = s_error;
                goto Finish;
            }

            // USE Cache CLASS TO STORE A LARGE DATA

            Cache.Insert(s_cache_inside, dt_inside, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);
            Cache.Insert(s_cache_outside, dt_outside, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);

            Cache.Insert(s_cache_inside + "_Sum", dt_inside_sum, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);
            Cache.Insert(s_cache_outside + "_Sum", dt_outside_sum, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);

            lblMessage.Text = (n_rows_affected > 0) ? "Item/s successfully updated." : "No item was updated.";

            Finish:

            Bind_Table(dt_inside, dt_outside, dt_inside_sum, dt_outside_sum);
        }

        protected void Comment_Change(object sender, EventArgs e)
        {
            DataTable dt_inside = ((DataTable)Cache[s_cache_inside]).Copy();
            DataTable dt_inside_sum = ((DataTable)Cache[s_cache_inside + "_Sum"]).Copy();

            DataTable dt_outside = ((DataTable)Cache[s_cache_outside]).Copy();
            DataTable dt_outside_sum = ((DataTable)Cache[s_cache_outside + "_Sum"]).Copy();

            string s_inside_id_array = hidSelectInside.Value;
            string s_outside_id_array = hidSelectOutside.Value;

            if (s_inside_id_array == "" && s_outside_id_array == "") { goto Finish; }

            string s_error = "";

            string s_comment = txtCommentChange.Text.Trim();

            int n_rows_affected = DataAction.Change_Comment(n_user_id, s_comment, s_inside_id_array, s_outside_id_array, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                goto Finish;
            }

            // SELECT INSIDE & OTSIDE TABLES FROM DB

            string s_where_inside = ViewState["WhereInside"].ToString();
            string s_where_outside = ViewState["WhereOutside"].ToString();

            string s_order_inside = ViewState["OrderInside"].ToString();
            string s_order_outside = ViewState["OrderOutside"].ToString();

            int n_page_inside = Convert.ToInt32(ddlInsidePage.SelectedValue);
            int n_page_outside = Convert.ToInt32(ddlOutsidePage.SelectedValue);

            DataAction.Select(n_user_id, s_where_inside, s_where_outside, s_order_inside, s_order_outside, n_page_inside, n_page_outside, 100, ref dt_inside, ref dt_inside_sum, ref dt_outside, ref dt_outside_sum, ref s_error, "", "", "", "");

            if (s_error != "")
            {
                lblError.Text = s_error;
                goto Finish;
            }

            // USE Cache CLASS TO STORE A LARGE DATA

            Cache.Insert(s_cache_inside, dt_inside, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);
            Cache.Insert(s_cache_outside, dt_outside, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);

            Cache.Insert(s_cache_inside + "_Sum", dt_inside_sum, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);
            Cache.Insert(s_cache_outside + "_Sum", dt_outside_sum, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);

            lblMessage.Text = (n_rows_affected > 0) ? "Item/s successfully updated." : "No item was updated.";

            Finish:

            Bind_Table(dt_inside, dt_outside, dt_inside_sum, dt_outside_sum);
        }

        protected void Download_Excel(object sender, EventArgs e)
        {
            string s_mode = Get_Mode();
            string s_button_id = ((LinkButton)sender).ID;
            string s_sheet = (s_button_id == "btnDownloadInside") ? "Inside" : "Outside";

            DataTable dt_download = null;

            switch (s_mode)
            {
                case "payment":
                    dt_download = (DataTable)ViewState["TablePaymentData"];
                    break;
                case "match":
                    dt_download = (s_sheet == "Inside") ? ((DataTable)Cache[s_cache_inside_match]).Copy() : ((DataTable)Cache[s_cache_outside_match]).Copy();
                    break;
                case "all":
                case "matching":
                case "not-matching":
                    string s_error = "";
                    string s_where_inside = (s_sheet == "Inside") ? ViewState["WhereInside"].ToString() : "NONE";
                    string s_where_outside = (s_sheet == "Outside") ? ViewState["WhereOutside"].ToString() : "NONE";

                    dt_download = DataAction.Select_Side(n_user_id, s_where_inside, s_where_outside, ref s_error);

                    if (s_error != "")
                    {
                        lblError.Text = s_error;
                        return;
                    }

                    break;
            }

            dt_download.Columns.RemoveAt(0);

            using (XLWorkbook xl_workbook = new XLWorkbook())
            {
                xl_workbook.Worksheets.Add(dt_download, s_sheet);

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + s_sheet + "_" + n_user_id + ".xlsx");

                using (MemoryStream o_memory_stream = new MemoryStream())
                {
                    xl_workbook.SaveAs(o_memory_stream);

                    o_memory_stream.WriteTo(Response.OutputStream);

                    Response.Flush();
                    Response.End();
                }
            }
        }
        //protected void btnChkAllCheckBoxBelow_Click (object sender, EventArgs e)
        //{
        //    string strChkboxes = hdnAllSelectedType.Value;
        //}
        protected void Save_Changes(object sender, EventArgs e)
        {
            if (Allow_Recalculate() == false) { return; }

            string allCheckBoxChecked = string.Empty;
            if (chkAllCheckBox.Checked)
                allCheckBoxChecked = "all";
            else
                allCheckBoxChecked = "";

            string s_inside_id_array = hidSelectInside.Value;
            string s_outside_id_array = hidSelectOutside.Value;

            if (allCheckBoxChecked == "")
            {
                if (s_inside_id_array == "" && s_outside_id_array == "")
                {
                    return;
                }
            }

            string s_mode = Get_Mode();

            string s_error = "";

            int n_rows_affected = 0;

            bool b_rebind = (s_mode == "match") ? false : true;

            // SELECT INSIDE & OTSIDE TABLES FROM DB
            string s_where_inside = ViewState["WhereInside"].ToString();
            string s_where_outside = ViewState["WhereOutside"].ToString();

            switch (s_mode)
            {
                case "match":
                    n_rows_affected = DataAction.Delete_Match_Item(n_user_id, hidQueryID.Value, s_inside_id_array, s_outside_id_array, ref s_error);

                    if (s_error != "") { goto Error; }

                    break;
                case "matching":
                    string comment = txtMatchingComment.Text;
                    string s_query_id_array = Common.Get_Distinct_Values(s_inside_id_array, s_outside_id_array);
                    n_rows_affected = DataAction.Delete_Match(n_user_id, null, s_query_id_array, ref s_error, comment);

                    if (s_error != "") { goto Error; }

                    break;
                case "not-matching":
                    MatchingBalanceModel o_matching_balance = null;

                    if (divMatchingBalanceRow.Visible == true)
                    {
                        try
                        {
                            o_matching_balance = new MatchingBalanceModel();
                            if (!string.IsNullOrEmpty(hidCompanyID.Value))
                                o_matching_balance.CompanyID = Convert.ToInt32(hidCompanyID.Value);
                            if (!string.IsNullOrEmpty(ddlOperationType_Balance.SelectedValue))
                                o_matching_balance.OperationTypeID = Convert.ToInt32(ddlOperationType_Balance.SelectedValue);
                            if (!string.IsNullOrEmpty(hidBalanceAmount.Value))
                                o_matching_balance.DutyPaymentAmount = Convert.ToDouble(hidBalanceAmount.Value);
                        }
                        catch (Exception ex)
                        { }
                    }

                    n_rows_affected = DataAction.Update_Matching(n_user_id, txtMatchingComment.Text.Trim(), s_inside_id_array, s_outside_id_array, o_matching_balance, ref s_error, allCheckBoxChecked, s_where_inside, s_where_outside);

                    if (s_error != "") { goto Error; }

                    break;
                default:
                    return;
            }

            // SELECT INSIDE & OTSIDE TABLES FROM DB

            //string s_where_inside = ViewState["WhereInside"].ToString();
            //string s_where_outside = ViewState["WhereOutside"].ToString();

            string s_order_inside = ViewState["OrderInside"].ToString();
            string s_order_outside = ViewState["OrderOutside"].ToString();

            int n_inside_page = Convert.ToInt32(ddlInsidePage.SelectedValue);
            int n_outside_page = Convert.ToInt32(ddlOutsidePage.SelectedValue);

            DataTable dt_inside = null, dt_outside = null, dt_inside_sum = null, dt_outside_sum = null;

            DataAction.Select(n_user_id, s_where_inside, s_where_outside, s_order_inside, s_order_outside, n_inside_page, n_outside_page, 100, ref dt_inside, ref dt_inside_sum, ref dt_outside, ref dt_outside_sum, ref s_error, "", "", "", "");

            if (s_error != "") { goto Error; }

            // USE Cache CLASS TO STORE A LARGE DATA

            Cache.Insert(s_cache_inside, dt_inside, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);
            Cache.Insert(s_cache_outside, dt_outside, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);

            Cache.Insert(s_cache_inside + "_Sum", dt_inside_sum, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);
            Cache.Insert(s_cache_outside + "_Sum", dt_outside_sum, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);

            if (s_mode == "match")
            {
                string s_sql = " NOT ID IN ( {0} ) ";

                string s_inside_sql = "", s_outside_sql = "";

                if (s_inside_id_array != "") { s_inside_sql = String.Format(s_sql, s_inside_id_array); }
                if (s_outside_id_array != "") { s_outside_sql = String.Format(s_sql, s_outside_id_array); }

                DataTable dt_inside_match = ((DataTable)Cache[s_cache_inside_match]).Copy();
                DataTable dt_outside_match = ((DataTable)Cache[s_cache_outside_match]).Copy();

                if (s_inside_sql != "")
                {
                    DataRow[] dr_inside_match = dt_inside_match.Select(s_inside_sql);

                    if (dr_inside_match.Length > 0)
                    {
                        dt_inside_match = dr_inside_match.CopyToDataTable();
                    }
                    else
                    {
                        dt_inside_match.Rows.Clear();
                    }
                }

                if (s_outside_sql != "")
                {
                    DataRow[] dr_outside_match = dt_outside_match.Select(s_outside_sql);

                    if (dr_outside_match.Length > 0)
                    {
                        dt_outside_match = dr_outside_match.CopyToDataTable();
                    }
                    else
                    {
                        dt_outside_match.Rows.Clear();
                    }
                }

                if (dt_inside_match.Rows.Count == 0 && dt_outside_match.Rows.Count == 0)
                {
                    b_rebind = true;

                    hidQueryID.Value = "";

                    Cache.Remove(s_cache_inside_match);
                    Cache.Remove(s_cache_outside_match);
                }
                else
                {
                    Match_Click(sender, e);
                }
            }

            if (b_rebind == true)
            {
                Enable_Table(hidUniqueID.Value, hidQueryID.Value);

                Bind_Table(dt_inside, dt_outside, dt_inside_sum, dt_outside_sum);
            }

            lblMessage.Text = (n_rows_affected > 0) ? "Item/s successfully updated." : "No item was updated.";

            return;

            Error:

            lblError.Text = s_error;
        }

        protected void Cancel_Changes(object sender, EventArgs e)
        {
            DataTable dt_inside = null, dt_inside_sum = null, dt_outside = null, dt_outside_sum = null, dt_source = null;

            string s_mode = Get_Mode();

            switch (s_mode)
            {
                case "payment":
                    dt_inside = (DataTable)ViewState["TablePaymentData"];
                    dt_source = (DataTable)ViewState["TablePaymentSource"];
                    break;
                case "match":
                    dt_inside = ((DataTable)Cache[s_cache_inside_match]).Copy();
                    dt_outside = ((DataTable)Cache[s_cache_outside_match]).Copy();
                    break;
                case "matching":
                case "not-matching":
                    dt_inside = ((DataTable)Cache[s_cache_inside]).Copy();
                    dt_outside = ((DataTable)Cache[s_cache_outside]).Copy();
                    dt_inside_sum = ((DataTable)Cache[s_cache_inside + "_Sum"]).Copy();
                    dt_outside_sum = ((DataTable)Cache[s_cache_outside + "_Sum"]).Copy();
                    break;
            }

            Enable_Table(hidUniqueID.Value, hidQueryID.Value);

            if (s_mode == "payment")
            {
                Bind_Table_Payment(dt_inside, dt_source);
            }
            else
            {
                Bind_Table(dt_inside, dt_outside, dt_inside_sum, dt_outside_sum);
            }
        }

        protected void Page_Changed(object sender, EventArgs e)
        {
            string s_error = "";

            string s_ddl_id = ((DropDownList)sender).ID;

            string s_where_inside = ViewState["WhereInside"].ToString();
            string s_where_outside = ViewState["WhereOutside"].ToString();

            string s_order_inside = ViewState["OrderInside"].ToString();
            string s_order_outside = ViewState["OrderOutside"].ToString();

            int n_page_inside = Convert.ToInt32(ddlInsidePage.SelectedValue);
            int n_page_outside = Convert.ToInt32(ddlOutsidePage.SelectedValue);

            DataTable dt_inside = new DataTable();
            DataTable dt_inside_sum = new DataTable();

            DataTable dt_outside = new DataTable();
            DataTable dt_outside_sum = new DataTable();

            DataAction.Select(n_user_id, s_where_inside, s_where_outside, s_order_inside, s_order_outside, n_page_inside, n_page_outside, 100, ref dt_inside, ref dt_inside_sum, ref dt_outside, ref dt_outside_sum, ref s_error, "", "", "", "");

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            // USE Cache CLASS TO STORE A LARGE DATA

            Cache.Insert(s_cache_inside, dt_inside, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);
            Cache.Insert(s_cache_outside, dt_outside, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);

            Cache.Insert(s_cache_inside + "_Sum", dt_inside_sum, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);
            Cache.Insert(s_cache_outside + "_Sum", dt_outside_sum, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);

            if (hidSelectInside.Value == "" && hidSelectOutside.Value == "")
            {
                Bind_Table(dt_inside, dt_outside, dt_inside_sum, dt_outside_sum);
            }
            else
            {
                Recalculate_Click(sender, e);
            }
        }

        protected void Inside_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gv_row = e.Row;

            string s_mode = Get_Mode();

            if (gv_row.RowType == DataControlRowType.Header)
            {
                DataTable dt_data_field = (DataTable)ViewState["TableDataField"];

                bool b_checked = (s_mode == "match" && hidSelectInside.Value == "") || (s_mode == "matching" && hidSelectInside.Value == "" && hidSelectOutside.Value == "");
                bool b_disabled = (s_mode == "all");
                DataAction.Bind_Grid_Data_Header(gv_row, dt_data_field, "Inside", b_checked, b_disabled);
            }

            if (gv_row.RowType == DataControlRowType.DataRow)
            {
                gv_row.CssClass = "nowrap";

                List<string> lst_inside_field_priority = (List<string>)ViewState["ListInsideFieldPriority"];

                DataAction.Bind_Grid_Data_Row(gv_row, lst_inside_field_priority, hidSelectInside.Value, hidSelectOutside.Value, "Inside", s_mode);
            }
        }

        protected void Outside_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gv_row = e.Row;

            string s_mode = Get_Mode();

            if (gv_row.RowType == DataControlRowType.Header)
            {
                //var headerCheck = gv_row.Controls[0];     
                //((System.Web.UI.HtmlControls.HtmlControl)headerCheck).

                DataTable dt_data_field = (DataTable)ViewState["TableDataField"];

                bool b_checked = (s_mode == "match" && hidSelectOutside.Value == "") || (s_mode == "matching" && hidSelectInside.Value == "" && hidSelectOutside.Value == "");
                bool b_disabled = (s_mode == "all") || (s_mode == "payment");

                DataAction.Bind_Grid_Data_Header(gv_row, dt_data_field, "Outside", b_checked, b_disabled);
            }

            if (gv_row.RowType == DataControlRowType.DataRow)
            {
                gv_row.CssClass = "nowrap";

                List<string> lst_outside_field_priority = (List<string>)ViewState["ListOutsideFieldPriority"];

                DataAction.Bind_Grid_Data_Row(gv_row, lst_outside_field_priority, hidSelectInside.Value, hidSelectOutside.Value, "Outside", s_mode);
            }
        }

        protected void Source_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataAction.Bind_Grid_Source(e.Row, hidTable.Value);
        }

        protected void Payment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridViewRow gv_row = e.Row;

            if (gv_row.RowType == DataControlRowType.Header)
            {
                DataTable dt_data_field = (DataTable)ViewState["TableDataField"];

                foreach (TableCell o_cell in gv_row.Cells)
                {
                    string s_cell_text = o_cell.Text.Trim();

                    DataRow dr_data_field = dt_data_field.Select(" FieldName = '" + s_cell_text + "' ").FirstOrDefault();

                    if (dr_data_field != null)
                    {
                        s_cell_text = dr_data_field["FieldDescription"].ToString();
                    }

                    o_cell.Text = s_cell_text;
                }
            }

            if (gv_row.RowType == DataControlRowType.DataRow)
            {
                List<string> lst_payment_field_priority = (List<string>)ViewState["ListPaymentFieldPriority"];

                int i_transaction_date = lst_payment_field_priority.IndexOf("TransactionDate");
                int i_transmission_date = lst_payment_field_priority.IndexOf("TransmissionDate");
                int i_paymen_date = lst_payment_field_priority.IndexOf("PaymentDate");

                TableCell tc_transaction_date = gv_row.Cells[i_transaction_date];
                TableCell tc_transmission_date = gv_row.Cells[i_transmission_date];
                TableCell tc_paymen_date = gv_row.Cells[i_paymen_date];

                // TransactionDate

                try
                {
                    tc_transaction_date.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(tc_transaction_date.Text));
                }
                catch (Exception ex)
                {
                    tc_transaction_date.Text = "";
                }

                // TransmissionDate

                try
                {
                    tc_transmission_date.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(tc_transmission_date.Text));
                }
                catch (Exception ex)
                {
                    tc_transmission_date.Text = "";
                }

                // PaymentDate

                try
                {
                    tc_paymen_date.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(tc_paymen_date.Text));
                }
                catch (Exception ex)
                {
                    tc_paymen_date.Text = "";
                }
            }
        }

        private string Parameter_CheckBoxList(CheckBoxList o_checkbox_list)
        {
            List<string> lst_value = new List<string>();

            foreach (ListItem o_checkbox in o_checkbox_list.Items.Cast<ListItem>().Where(i => i.Selected == true))
            {
                lst_value.Add(o_checkbox.Value);
            }

            string s_value = (lst_value.Count > 0) ? String.Join(",", lst_value.ToArray()) : "";

            return s_value;
        }

        private string Parameter_TextBox(TextBox o_textbox, string s_type, ref string s_error, bool b_range = false, int n_max_length = 0)
        {
            string s_value = "";

            string s_text = o_textbox.Text.Trim().Replace("'", "").Replace('"', (char)0);

            s_error = "";

            if (s_text == "") { goto Finish; }

            List<string> lst_value = new List<string>();
            List<string> lst_error = new List<string>();

            string[] arr_text = s_text.Split(',');

            for (int i = 0; i < arr_text.Length; i++)
            {
                string s_item = arr_text[i].Trim();

                if (s_item != "")
                {
                    bool b_valid = false;

                    if (b_range == true)
                    {
                        b_valid = Parameter_Range_Valid(ref s_item, s_type, n_max_length);
                    }
                    else if (s_item.Contains(c_range_separator.ToString()) == false)
                    {
                        b_valid = Parameter_Valid(ref s_item, s_type, n_max_length);
                    }

                    if (b_valid == true)
                    {
                        if (lst_value.Contains(s_item) == false)
                        {
                            lst_value.Add(s_item);
                        }
                    }
                    else
                    {
                        if (lst_error.Contains(s_item) == false)
                        {
                            lst_error.Add(s_item);
                        }
                    }
                }
            }

            if ((lst_error.Count > 0)) { s_error = String.Join(",", lst_error.ToArray()); }

            if (s_error != "") { s_error = String.Format("<b>Not valid :</b> {0}", s_error); }

            if ((lst_value.Count > 0)) { s_value = String.Join(",", lst_value.ToArray()); }

            Finish:

            return s_value;
        }

        private bool Parameter_Valid(ref string s_value, string s_type, int n_max_length = 0)
        {
            bool b_valid = false;

            s_value = s_value.Trim();

            switch (s_type)
            {
                case "int":
                case "uint":
                case "ulong":
                case "decimal":
                    decimal n_decimal_value = 0;

                    b_valid = decimal.TryParse(s_value, out n_decimal_value);

                    if (b_valid == true && (s_type == "uint" || s_type == "ulong" || s_type == "decimal"))
                    {
                        b_valid = (n_decimal_value >= 0);
                    }

                    if (b_valid == true && n_max_length > 0)
                    {
                        b_valid = (n_decimal_value.ToString().Length <= n_max_length);
                    }

                    if (b_valid == true)
                    {
                        s_value = n_decimal_value.ToString();
                    }

                    break;
                case "double":
                    double n_double_value = 0;

                    b_valid = double.TryParse(s_value, out n_double_value);

                    if (b_valid == true)
                    {
                        string s_double_value = n_double_value.ToString();

                        if (s_double_value.Contains(".") == true && s_double_value.Split('.')[1].Length > 2)
                        {
                            n_double_value = Math.Round(n_double_value, 2);
                        }

                        s_value = n_double_value.ToString();
                    }

                    break;
                case "date":
                    string s_date = "";

                    string[] arr_date = s_value.Split('/');

                    b_valid = (arr_date.Length == 3);

                    if (b_valid == true)
                    {
                        string s_day = arr_date[0];
                        string s_month = arr_date[1];
                        string s_year = arr_date[2];

                        s_date = String.Format("{0}-{1}-{2}", s_year, s_month, s_day);

                        DateTime d_date = new DateTime();

                        b_valid = DateTime.TryParse(s_date, out d_date);
                    }

                    if (b_valid == true)
                    {
                        s_value = String.Format("'{0}'", s_date);
                    }

                    break;
                case "string":
                    b_valid = true;

                    s_value = String.Format("'{0}'", s_value);

                    break;
            }

            return b_valid;
        }

        private bool Parameter_Range_Valid(ref string s_value, string s_type, int n_max_length = 0)
        {
            bool b_valid = false;

            if (s_value.Contains(c_range_separator.ToString()) == false)
            {
                b_valid = Parameter_Valid(ref s_value, s_type, n_max_length);
                goto Finish;
            }

            string[] arr_value = s_value.Split(c_range_separator);

            string s_min = arr_value[0].Trim();
            string s_max = arr_value[1].Trim();

            b_valid = (s_min != "" || s_max != "");

            if (b_valid == false) { goto Finish; }

            if (s_min != "")
            {
                b_valid = Parameter_Valid(ref s_min, s_type, n_max_length);

                if (b_valid == false) { goto Finish; }
            }

            if (s_max != "")
            {
                b_valid = Parameter_Valid(ref s_max, s_type, n_max_length);

                if (b_valid == false) { goto Finish; }
            }

            if (s_min != "" && s_max != "")
            {
                switch (s_type)
                {
                    case "int":
                    case "uint":
                    case "ulong":
                    case "decimal":
                        decimal n_decimal_min = 0, n_decimal_max = 0;

                        decimal.TryParse(s_min, out n_decimal_min);
                        decimal.TryParse(s_max, out n_decimal_max);

                        b_valid = (n_decimal_min <= n_decimal_max);

                        break;
                    case "double":
                        double n_double_min = 0, n_double_max = 0;

                        double.TryParse(s_min, out n_double_min);
                        double.TryParse(s_max, out n_double_max);

                        b_valid = (n_double_min <= n_double_max);

                        break;
                    case "date":
                        DateTime d_min = new DateTime();
                        DateTime d_max = new DateTime();

                        DateTime.TryParse(s_min.Replace("'", ""), out d_min);
                        DateTime.TryParse(s_max.Replace("'", ""), out d_max);

                        b_valid = (d_min <= d_max);

                        break;
                }
            }

            if (b_valid == true)
            {
                s_value = String.Format("{0}{1}{2}", s_min, c_range_separator.ToString(), s_max);
            }

            Finish:

            return b_valid;
        }

        private string Parameter_SQL(string s_name, string s_value, bool b_exclude)
        {
            string s_sql = "", s_operator = "";

            if (s_value.Contains(c_range_separator.ToString()) == false)
            {
                s_operator = (b_exclude == true) ? " NOT IN " : " IN ";

                s_sql = String.Format(" {0} {1} ( {2} ) ", s_name, s_operator, s_value);
            }
            else
            {
                string[] arr_value = s_value.Split(',');

                List<string> lst_comma_separated = new List<string>();

                foreach (string s_item in arr_value)
                {
                    if (s_item.Contains(c_range_separator.ToString()) == false)
                    {
                        lst_comma_separated.Add(s_item);
                    }
                    else
                    {
                        string[] arr_item = s_item.Split(c_range_separator);

                        string s_min = arr_item[0];
                        string s_max = arr_item[1];

                        if (s_sql != "") { s_sql += " OR "; }

                        if (s_min != "" && s_max != "")
                        {
                            s_operator = (b_exclude == true) ? " NOT BETWEEN " : " BETWEEN ";

                            s_sql += String.Format(" {0} {1} {2} AND {3} ", s_name, s_operator, s_min, s_max);
                        }
                        else if (s_min != "")
                        {
                            s_operator = (b_exclude == true) ? " < " : " >= ";

                            s_sql += String.Format(" {0} {1} {2} ", s_name, s_operator, s_min);
                        }
                        else if (s_max != "")
                        {
                            s_operator = (b_exclude == true) ? " > " : " <= ";

                            s_sql += String.Format(" {0} {1} {2} ", s_name, s_operator, s_max);
                        }
                    }
                }

                if (lst_comma_separated.Count > 0)
                {
                    if (s_sql != "") { s_sql += " OR "; }

                    s_operator = (b_exclude == true) ? " NOT IN " : " IN ";

                    s_sql += String.Format(" {0} {1} ( {2} ) ", s_name, s_operator, String.Join(",", lst_comma_separated.ToArray()));
                }
            }

            return s_sql;
        }

        private void Validate_Payment(int n_transaction_payments_count, ref int n_split_payments_count, ref double n_split_first_payment_amount, ref double n_split_remaining_payments_amount, ref string s_error)
        {
            // ERROR LEVEL 1

            double n_selected_payments_amount = 0;
            double.TryParse(hidPaymentsAmount_Selected.Value, out n_selected_payments_amount);

            if (n_selected_payments_amount == 0)
            {
                s_error = "'Selected Payments Amount' is zero.";
                return;
            }

            // ERROR LEVEL 2

            int.TryParse(txtPaymentsCount_Split.Text, out n_split_payments_count);

            double.TryParse(txtFirstPaymentAmount_Split.Text, out n_split_first_payment_amount);

            if (n_split_payments_count <= 0)
            {
                s_error = "'New Payments Count' must be a whole number greater than zero.";
            }
            else if (n_split_payments_count == 1 && n_transaction_payments_count == 1)
            {
                s_error = "'New Payments Count' must be a whole number greater than 1.";
            }

            if (n_split_first_payment_amount == 0)
            {
                if (s_error != "") { s_error = String.Format("{0}<br />", s_error); }

                s_error = String.Format("{0} {1}", s_error, "'First Payment Amount' must be a number not equal to zero.");
            }

            s_error = s_error.Trim();

            if (s_error != "") { return; }

            // ERROR LEVEL 3

            n_split_remaining_payments_amount = n_selected_payments_amount - n_split_first_payment_amount;

            if (n_split_payments_count == 1 && n_split_remaining_payments_amount != 0)
            {
                s_error = "'New Payments Count' must be greater than 1, or 'First Payment Amount' must be equal to 'Selected Payments Amount'.";
            }

            if (n_split_payments_count > 1)
            {
                if (n_split_remaining_payments_amount == 0)
                {
                    s_error = "'New Payments Count' must be equal to 1, or 'First Payment Amount' must be not equal to 'Selected Payments Amount'.";
                }
                else
                {
                    long n_cent = (long)(Math.Round(n_split_remaining_payments_amount, 2) * 100);
                    long n_mod = n_cent % (n_split_payments_count - 1);

                    if (n_mod != 0)
                    {
                        s_error = "'First Payment Amount' does not allow to evenly distribute the balance.";
                    }
                }
            }

            s_error = s_error.Trim();
        }

        private string Get_Mode()
        {
            string s_mode = "";

            if (hidUniqueID != null && hidUniqueID.Value != "")
            {
                s_mode = "payment";
            }
            else if (hidQueryID != null && hidQueryID.Value != "")
            {
                s_mode = "match";
            }
            else
            {
                s_mode = hidQueryID != null ? ddlTransactions.SelectedValue : "";
            }

            return s_mode;
        }

        private string Get_AjaxMode(string hidUniqueIDVal, string hidQueryIDVal, string ddlTransactionsVal)
        {
            string s_mode = "";

            if (!String.IsNullOrEmpty(hidUniqueIDVal))
            {
                s_mode = "payment";
            }
            else if (!String.IsNullOrEmpty(hidQueryIDVal))
            {
                s_mode = "match";
            }
            else
            {
                s_mode = ddlTransactionsVal != null ? ddlTransactionsVal : "";
            }

            return s_mode;
        }

        private bool Allow_Recalculate()
        {
            string s_mode = Get_Mode();
            string s_table = hidTable.Value.ToLower();

            List<ListItem> lst_status = cblStatus.Items.Cast<ListItem>().Where(i => i.Selected == true).ToList();

            bool b_recalculate = (
                (s_mode == "payment" && s_table == "inside")
                ||
                (s_mode == "match")
                ||
                (s_mode == "matching")
                ||
                (s_mode == "not-matching")
            );

            // (s_mode == "not-matching" && lst_status.Count() > 0 && lst_status.Where(i => i.Value == "2").Count() == 0)      // 2 = 'Out Of Matching'

            return b_recalculate;
        }

        private bool Allow_Payment_Restore()
        {
            bool b_restore = false;

            DataTable dt_data = (DataTable)ViewState["TablePaymentData"];

            if (dt_data != null) { b_restore = (dt_data.Select(" MatchingActionName = 'Resplit' ").FirstOrDefault() != null); }

            return b_restore;
        }

        private void Enable_Table(string s_unique_id = "", string s_query_id = "")
        {
            // CLEAR PAYMENT & MATCH MODE

            hidUniqueID.Value = s_unique_id;
            hidQueryID.Value = s_query_id;

            string s_mode = Get_Mode();
            string s_table = hidTable.Value.ToLower();

            // CLEAR SELECTED ITEMS

            hidSelectInside.Value = "";
            hidSelectOutside.Value = "";
            hidSelectSource.Value = "";

            // CLEAR STATUS CHANGES

            ddlStatusChange.SelectedIndex = 0;

            // CLEAR COMMENT CHANGES

            txtCommentChange.Text = "";

            // CLEAR MATCHING & PAYMENT CHANGES // HIDE MATCHING & PAYMENT CHANGE TOOLS

            hidPaymentsAmount_Selected.Value = "";

            txtPaymentsCount_Split.Text = "";
            txtFirstPaymentAmount_Split.Text = "";

            btnPaymentChange.Visible = false;

            divBalance.Visible = false;
            divSplit.Visible = false;
            pnlPaymentChange.Visible = false;

            btnPaymentRestore.Visible = true;
            lblPaymentChange.Visible = false;

            lblInsideRowsSelected.Text = "0";
            lblInsideAmountSelected.Text = "0";

            trInsideSelected.Visible = true;

            lblInsideRowsRemaining.Text = "0";
            lblInsideAmountRemaining.Text = "0";

            trInsideRemaining.Visible = true;

            lblOutsideRowsSelected.Text = "0";
            lblOutsideAmountSelected.Text = "0";

            trOutsideSelected.Visible = true;

            lblOutsideRowsRemaining.Text = "0";
            lblOutsideAmountRemaining.Text = "0";

            trOutsideRemaining.Visible = true;

            if (divMatchingBalanceRow.Visible == true)
            {
                txtCompanyName.Text = "";
                hidCompanyID.Value = "";

                txtBalanceAmount.Text = "0";
                hidBalanceAmount.Value = "";

                //divMatchingBalanceRow.Visible = false;
            }

            if (pnlMatchingBalance.Visible == true)
            {
                txtMatchingComment.Text = "0";

                //pnlMatchingBalance.Visible = false;
            }

            divMatchingAuto.Visible = false;

            // SHOW-HIDE ACTION BUTTONS

            bool b_allow_recalculate = Allow_Recalculate();
            bool b_allow_payment_restore = (s_mode == "payment" && Allow_Payment_Restore() == true);

            tdRecalculate.Visible = b_allow_recalculate;
            tdPayment.Visible = b_allow_payment_restore;
            tdMatchingAuto.Visible = (s_mode == "not-matching" && b_allow_recalculate == true);
            tdReturn.Visible = (s_unique_id != "" || s_query_id != "");
            tdStatus.Visible = (s_mode == "not-matching");
            tdComment.Visible = (s_mode == "matching" || s_mode == "not-matching");

            // SHOW-HIDE INSIDE-OUTSIDE-SOURCE TABLES

            if (s_mode == "payment")
            {
                tdDisplayInside.Visible = (s_table == "inside");
                tdDisplayOutside.Visible = (s_table == "outside");
                tdDisplaySource.Visible = true;

                divInside.Visible = (s_table == "inside");
                divOutside.Visible = (s_table == "outside");
                divDataSeparator.Visible = false;

                divSource.Visible = true;
                divSourceSeparator.Visible = true;
            }
            else
            {
                tdDisplayInside.Visible = true;
                tdDisplayOutside.Visible = true;
                tdDisplaySource.Visible = false;

                divInside.Visible = true;
                divOutside.Visible = true;
                divDataSeparator.Visible = true;

                divSource.Visible = false;
                divSourceSeparator.Visible = false;
            }

            // ENABLE-DISABLE PAGING

            ddlInsidePage.Enabled = (s_unique_id == "" && s_query_id == "");
            ddlOutsidePage.Enabled = (s_unique_id == "" && s_query_id == "");

            // DISPLAY MESSAGE BY CURRENT MODE

            if (b_allow_recalculate == true)
            {
                string s_note = "";

                switch (s_mode)
                {
                    case "payment":
                        s_note = "'Recalculate' will change 'Payment Group'.";

                        if (b_allow_payment_restore == true)
                        {
                            s_note = String.Format("{0} {1}", s_note, "'Restore' will return 'Payment Group' to original mode.");
                        }

                        break;
                    case "match":
                        s_note = "'Recalculate' will remove unselected items from 'Match Group'.";
                        break;
                    case "matching":
                        s_note = "'Recalculate' will remove 'Match Group' of unselected items.";
                        break;
                    case "not-matching":
                        s_note = "'Recalculate' will create new manual 'Matching'.";
                        break;
                }

                lblNote.Text = s_note;
            }
            else
            {
                lblNote.Text = "";
            }
        }

        private void Bind_Search()
        {
            // Bind Cassh Tables

            DataSearchTables o_data_search_tables = new DataSearchTables();

            o_data_search_tables.UserID = n_user_id;

            DataAction.Select_Search_Tables(ref o_data_search_tables);

            if (o_data_search_tables.ErrorMessage != "")
            {
                lblError.Text = o_data_search_tables.ErrorMessage;
                return;
            }

            ViewState["TableCompany"] = o_data_search_tables.TableCompany;
            ViewState["TableNetwork"] = o_data_search_tables.TableNetwork;
            ViewState["TableBranch"] = o_data_search_tables.TableBranch;
            ViewState["TableCashBox"] = o_data_search_tables.TableCashbox;

            ViewState["TableSupplierGroup"] = o_data_search_tables.TableSupplierGroup;
            ViewState["TableSupplier"] = o_data_search_tables.TableSupplier;
            ViewState["TableTerminal"] = o_data_search_tables.TableTerminal;

            ViewState["TableCredit"] = o_data_search_tables.TableCredit;
            ViewState["TableCard"] = o_data_search_tables.TableCard;

            ViewState["TableOperationType"] = o_data_search_tables.TableOperationType;
            ViewState["TableDataStatus"] = o_data_search_tables.TableDataStatus;
            ViewState["TableMatchingAction"] = o_data_search_tables.TableMatchingAction;
            ViewState["TableMatchingType"] = o_data_search_tables.TableMatchingType;

            ViewState["TableTemplate"] = o_data_search_tables.TableTemplate;
            ViewState["TableDataFile"] = o_data_search_tables.TableDataFile;
            ViewState["TableStrategy"] = o_data_search_tables.TableStrategy;
            ViewState["TableMatching"] = o_data_search_tables.TableMatching;
            ViewState["TableCurrency"] = o_data_search_tables.TableCurrency;

            ViewState["TableDataField"] = o_data_search_tables.TableDataField;

            // Bind Sort

            DataRow dr_field_priority = o_data_search_tables.TableDataPriority.Rows[0];
            DataRow dr_field_sort = o_data_search_tables.TableDataSort.Rows[0];

            string s_inside_field_priority = dr_field_priority["InsideFieldPriority"].ToString();
            string s_outside_field_priority = dr_field_priority["OutsideFieldPriority"].ToString();

            string s_inside_sort_order = dr_field_sort["InsideSortOrder"].ToString().Trim();
            string s_outside_sort_order = dr_field_sort["OutsideSortOrder"].ToString().Trim();

            DataTable dt_inside_field_priority = new DataTable();
            dt_inside_field_priority.Columns.Add("FieldName");
            dt_inside_field_priority.Columns.Add("FieldDescription");

            DataTable dt_outside_field_priority = new DataTable();
            dt_outside_field_priority.Columns.Add("FieldName");
            dt_outside_field_priority.Columns.Add("FieldDescription");

            string[] arr_inside_field_priority = s_inside_field_priority.Split(',');
            string[] arr_outside_field_priority = s_outside_field_priority.Split(',');

            for (int i = 0; i < arr_inside_field_priority.Length; i++)
            {
                string s_field_name = arr_inside_field_priority[i].Trim();

                if (s_field_name != "QueryID" && s_field_name != "UniqueID")
                {
                    string s_field_description = "";

                    DataRow dr_data_field = o_data_search_tables.TableDataField.Select(" FieldName = '" + s_field_name + "' ").FirstOrDefault();

                    if (dr_data_field != null)
                    {
                        s_field_description = dr_data_field["FieldDescription"].ToString();
                    }
                    else
                    {
                        s_field_description = s_field_name;
                    }

                    DataRow dr_inside_field_priority = dt_inside_field_priority.NewRow();

                    dr_inside_field_priority["FieldName"] = s_field_name;
                    dr_inside_field_priority["FieldDescription"] = s_field_description;

                    dt_inside_field_priority.Rows.Add(dr_inside_field_priority);
                }
            }

            for (int i = 0; i < arr_outside_field_priority.Length; i++)
            {
                string s_field_name = arr_outside_field_priority[i].Trim();

                if (s_field_name != "QueryID" && s_field_name != "UniqueID")
                {
                    string s_field_description = "";

                    DataRow dr_data_field = o_data_search_tables.TableDataField.Select(" FieldName = '" + s_field_name + "' ").FirstOrDefault();

                    if (dr_data_field != null)
                    {
                        s_field_description = dr_data_field["FieldDescription"].ToString();
                    }
                    else
                    {
                        s_field_description = s_field_name;
                    }

                    DataRow dr_outside_field_priority = dt_outside_field_priority.NewRow();

                    dr_outside_field_priority["FieldName"] = s_field_name;
                    dr_outside_field_priority["FieldDescription"] = s_field_description;

                    dt_outside_field_priority.Rows.Add(dr_outside_field_priority);
                }
            }

            Bind_Drop_Down(ddlInsideSort_1, dt_inside_field_priority, "");
            Bind_Drop_Down(ddlInsideSort_2, dt_inside_field_priority, "");
            Bind_Drop_Down(ddlInsideSort_3, dt_inside_field_priority, "");

            ddlInsideSort_1.Items.Insert(0, new ListItem("1", ""));
            ddlInsideSort_2.Items.Insert(0, new ListItem("2", ""));
            ddlInsideSort_3.Items.Insert(0, new ListItem("3", ""));

            Bind_Drop_Down(ddlOutsideSort_1, dt_outside_field_priority, "");
            Bind_Drop_Down(ddlOutsideSort_2, dt_outside_field_priority, "");
            Bind_Drop_Down(ddlOutsideSort_3, dt_outside_field_priority, "");

            ddlOutsideSort_1.Items.Insert(0, new ListItem("1", ""));
            ddlOutsideSort_2.Items.Insert(0, new ListItem("2", ""));
            ddlOutsideSort_3.Items.Insert(0, new ListItem("3", ""));

            // Bind Search

            repCompany.DataSource = o_data_search_tables.TableCompany;
            repCompany.DataBind();

            chkIsSplitted.Text = o_data_search_tables.TableDataField.Select(" FieldName = 'IsSplitted' ")[0]["FieldDescription"].ToString();
            chkIsBalance.Text = o_data_search_tables.TableDataField.Select(" FieldName = 'IsBalance' ")[0]["FieldDescription"].ToString();
            chkIsAbroad.Text = o_data_search_tables.TableDataField.Select(" FieldName = 'IsAbroad' ")[0]["FieldDescription"].ToString();

            cblOperationType.DataValueField = "ID";
            cblOperationType.DataTextField = "OperationTypeName";
            cblOperationType.DataSource = o_data_search_tables.TableOperationType;
            cblOperationType.DataBind();

            cblStatus.DataValueField = "ID";
            cblStatus.DataTextField = "StatusName";
            cblStatus.DataSource = o_data_search_tables.TableDataStatus;
            cblStatus.DataBind();

            cblMatchingAction.DataValueField = "ID";
            cblMatchingAction.DataTextField = "MatchingActionName";
            cblMatchingAction.DataSource = o_data_search_tables.TableMatchingAction;
            cblMatchingAction.DataBind();

            cblMatchingType.DataValueField = "ID";
            cblMatchingType.DataTextField = "MatchingTypeName";
            cblMatchingType.DataSource = o_data_search_tables.TableMatchingType;
            cblMatchingType.DataBind();

            cblTemplate.DataValueField = "ID";
            cblTemplate.DataTextField = "TemplateName";
            cblTemplate.DataSource = o_data_search_tables.TableTemplate;
            cblTemplate.DataBind();
            cblTemplate.Items.Insert(0, new ListItem("N / A", "0"));

            cblDataFile.DataValueField = "ID";
            cblDataFile.DataTextField = "DataFileName";
            cblDataFile.DataSource = o_data_search_tables.TableDataFile;
            cblDataFile.DataBind();
            cblDataFile.Items.Insert(0, new ListItem("N / A", "0"));

            cblStrategy.DataValueField = "ID";
            cblStrategy.DataTextField = "StrategyName";
            cblStrategy.DataSource = o_data_search_tables.TableStrategy;
            cblStrategy.DataBind();
            cblStrategy.Items.Insert(0, new ListItem("N / A", "0"));

            cblMatching.DataValueField = "ID";
            cblMatching.DataTextField = "MatchingName";
            cblMatching.DataSource = o_data_search_tables.TableMatching;
            cblMatching.DataBind();

            cblCredit.DataValueField = "ID";
            cblCredit.DataTextField = "CreditName";
            cblCredit.DataSource = o_data_search_tables.TableCredit;
            cblCredit.DataBind();

            cblCard.DataValueField = "ID";
            cblCard.DataTextField = "CardName";
            cblCard.DataSource = o_data_search_tables.TableCard;
            cblCard.DataBind();

            cblTransactionCurrency.DataValueField = "ID";
            cblTransactionCurrency.DataTextField = "CurrencyCode";
            cblTransactionCurrency.DataSource = o_data_search_tables.TableCurrency;
            cblTransactionCurrency.DataBind();
            cblTransactionCurrency.Items.Insert(0, new ListItem("N / A", "0"));
        }

        private void Bind_Drop_Down(DropDownList ddlFieldName, DataTable dt_settings, string s_value)
        {
            ddlFieldName.DataValueField = "FieldName";
            ddlFieldName.DataTextField = "FieldDescription";
            ddlFieldName.DataSource = dt_settings;
            ddlFieldName.DataBind();
            ddlFieldName.SelectedValue = s_value;
        }

        private void Bind_Status_Change()
        {
            DataTable dt_data_status = (DataTable)ViewState["TableDataStatus"];

            ddlStatusChange.DataValueField = "ID";
            ddlStatusChange.DataTextField = "StatusName";
            ddlStatusChange.DataSource = dt_data_status;
            ddlStatusChange.DataBind();
            ddlStatusChange.Items.Insert(0, new ListItem("- Status -", "-1"));
        }

        private void Bind_Matching_Balance(int n_company_id,
            int n_inside_rows_selected, int n_outside_rows_selected, double n_inside_amount_selected, double n_outside_amount_selected,
            int n_inside_rows_remaining, int n_outside_rows_remaining, double n_inside_amount_remaining, double n_outside_amount_remaining)
        {
            double n_balance_amount = (n_company_id > 0) ? n_outside_amount_selected - n_inside_amount_selected : 0;

            if (n_balance_amount == 0)
            {
                divMatchingBalanceRow.Visible = false;
            }
            else
            {
                divMatchingBalanceRow.Visible = true;

                // Company

                DataTable dt_company = (DataTable)ViewState["TableCompany"];

                DataRow dr_company = dt_company.Select(" ID = " + n_company_id).FirstOrDefault();

                txtCompanyName.Text = dr_company["CompanyName"].ToString();
                hidCompanyID.Value = n_company_id.ToString();

                // DutyPaymentAmount

                txtBalanceAmount.Text = String.Format("{0:n2}", n_balance_amount);
                hidBalanceAmount.Value = String.Format("{0}", n_balance_amount);

                // OperationType

                DataTable dt_operation_type = (DataTable)ViewState["TableOperationType"];

                dt_operation_type = dt_operation_type.Select(" ID IN ( 6, 7 ) ").CopyToDataTable();     // 'ביטול יתרה' / 'רישום הפרש'

                ddlOperationType_Balance.DataSource = dt_operation_type;
                ddlOperationType_Balance.DataValueField = "ID";
                ddlOperationType_Balance.DataTextField = "OperationTypeName";
                ddlOperationType_Balance.DataBind();
                ddlOperationType_Balance.Items.Insert(0, new ListItem("", "0"));
            }

            // Inside

            trInsideSelected.Visible = true;

            lblInsideRowsSelected.Text = String.Format("{0:n0}", n_inside_rows_selected);
            lblInsideAmountSelected.Text = String.Format("{0:n2}", n_inside_amount_selected);

            trInsideRemaining.Visible = true;

            lblInsideRowsRemaining.Text = String.Format("{0:n0}", n_inside_rows_remaining);
            lblInsideAmountRemaining.Text = String.Format("{0:n2}", n_inside_amount_remaining);

            // Outside

            trOutsideSelected.Visible = true;

            lblOutsideRowsSelected.Text = String.Format("{0:n0}", n_outside_rows_selected);
            lblOutsideAmountSelected.Text = String.Format("{0:n2}", n_outside_amount_selected);

            trOutsideRemaining.Visible = true;

            lblOutsideRowsRemaining.Text = String.Format("{0:n0}", n_outside_rows_remaining);
            lblOutsideAmountRemaining.Text = String.Format("{0:n2}", n_outside_amount_remaining);

            // DISPLAY pnlMatchingBalance

            pnlMatchingBalance.Visible = true;
        }

        private void Bind_Table_Payment(DataTable dt_data, DataTable dt_source)
        {
            string s_table = hidTable.Value.ToLower();

            lblModeInfo.Text = String.Format("Mode : {0}", "Single Payment Group");

            int n_rows = dt_data.Rows.Count;
            double n_amount = dt_data.AsEnumerable().Sum(dr => dr.Field<double>("DutyPaymentAmount"));

            if (s_table == "inside")
            {
                lblInsideRows.Text = String.Format("{0:n0}", n_rows);
                lblInsideAmount.Text = String.Format("{0:n2}", Math.Round(n_amount, 2));

                gvInside.DataSource = dt_data;
                gvInside.DataBind();
            }
            else
            {
                lblOutsideRows.Text = String.Format("{0:n0}", n_rows);
                lblOutsideAmount.Text = String.Format("{0:n2}", Math.Round(n_amount, 2));

                gvOutside.DataSource = dt_data;
                gvOutside.DataBind();
            }

            gvSource.DataSource = dt_source;
            gvSource.DataBind();
        }

        private void Bind_Paging(int n_pages_count_inside, int n_pages_count_outside, int n_page_number_inside = 1, int n_page_number_outside = 1)
        {
            ddlInsidePage.Items.Clear();
            ddlOutsidePage.Items.Clear();

            for (int i = 1; i <= n_pages_count_inside; i++)
            {
                ddlInsidePage.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            for (int i = 1; i <= n_pages_count_outside; i++)
            {
                ddlOutsidePage.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            if (ddlInsidePage.Items.Count == 0) { ddlInsidePage.Items.Add(new ListItem("1", "1")); }
            if (ddlOutsidePage.Items.Count == 0) { ddlOutsidePage.Items.Add(new ListItem("1", "1")); }

            ddlInsidePage.SelectedValue = n_page_number_inside.ToString();
            ddlOutsidePage.SelectedValue = n_page_number_outside.ToString();
        }

        private void Bind_Table(DataTable dt_inside, DataTable dt_outside, DataTable dt_inside_sum = null, DataTable dt_outside_sum = null)
        {
            // BIND MODE INFO LABEL

            string s_mode = Get_Mode();
            string s_mode_info = "Mode : {0}";

            if (s_mode == "payment")
            {
                s_mode_info = String.Format(s_mode_info, "Single Payment Group");
            }
            else if (s_mode == "match")
            {
                s_mode_info = String.Format(s_mode_info, "Single Match Group");
            }
            else
            {
                s_mode_info = String.Format(s_mode_info, ddlTransactions.SelectedItem.Text);
            }

            lblModeInfo.Text = s_mode_info;

            // BIND Company name and Operation type for Non - Matching 
            if(s_mode== "not-matching")
            {
                if (dt_inside.Rows.Count > 0)
                {
                    string companyNumber = dt_inside.Rows[0]["CompanyNumber"].ToString();
                    // Company
                    DataTable dt_company = (DataTable)ViewState["TableCompany"];
                    //DataRow dr_company = dt_company.Select(" ID = " + n_company_id).FirstOrDefault();
                    DataRow dr_company = dt_company.Select(" CompanyNumber = " + companyNumber).FirstOrDefault();

                    txtCompanyName.Text = dr_company["CompanyName"].ToString();
                    hidCompanyID.Value = dr_company["ID"].ToString();

                    // OperationType
                    DataTable dt_operation_type = (DataTable)ViewState["TableOperationType"];
                    dt_operation_type = dt_operation_type.Select(" ID IN ( 6, 7 ) ").CopyToDataTable();     // 'ביטול יתרה' / 'רישום הפרש'
                    ddlOperationType_Balance.DataSource = dt_operation_type;
                    ddlOperationType_Balance.DataValueField = "ID";
                    ddlOperationType_Balance.DataTextField = "OperationTypeName";
                    ddlOperationType_Balance.DataBind();
                    ddlOperationType_Balance.Items.Insert(0, new ListItem("", "0"));
                }
                else if(dt_outside.Rows.Count>0)
                {                   
                        string companyNumber = dt_outside.Rows[0]["CompanyNumber"].ToString();
                        // Company
                        DataTable dt_company = (DataTable)ViewState["TableCompany"];
                        //DataRow dr_company = dt_company.Select(" ID = " + n_company_id).FirstOrDefault();
                        DataRow dr_company = dt_company.Select(" CompanyNumber = " + companyNumber).FirstOrDefault();

                        txtCompanyName.Text = dr_company["CompanyName"].ToString();
                        hidCompanyID.Value = dr_company["ID"].ToString();

                        // OperationType
                        DataTable dt_operation_type = (DataTable)ViewState["TableOperationType"];
                        dt_operation_type = dt_operation_type.Select(" ID IN ( 6, 7 ) ").CopyToDataTable();     // 'ביטול יתרה' / 'רישום הפרש'
                        ddlOperationType_Balance.DataSource = dt_operation_type;
                        ddlOperationType_Balance.DataValueField = "ID";
                        ddlOperationType_Balance.DataTextField = "OperationTypeName";
                        ddlOperationType_Balance.DataBind();
                        ddlOperationType_Balance.Items.Insert(0, new ListItem("", "0"));                   
                }
            }

            // BIND SUM LABELS & PAGING DDL-S

            int n_rows_count_inside = 0, n_rows_count_outside = 0;

            double n_amount_sum_inside = 0, n_amount_sum_outside = 0;

            if (dt_inside_sum != null && dt_outside_sum != null)
            {
                n_rows_count_inside = Convert.ToInt32(dt_inside_sum.Rows[0]["RowsCount"]);
                n_rows_count_outside = Convert.ToInt32(dt_outside_sum.Rows[0]["RowsCount"]);

                n_amount_sum_inside = Convert.ToDouble(dt_inside_sum.Rows[0]["AmountSum"]);
                n_amount_sum_outside = Convert.ToDouble(dt_outside_sum.Rows[0]["AmountSum"]);
            }
            else
            {
                n_rows_count_inside = dt_inside.Rows.Count;
                n_rows_count_outside = dt_outside.Rows.Count;

                n_amount_sum_inside = dt_inside.AsEnumerable().Sum(dr => dr.Field<double>("DutyPaymentAmount"));
                n_amount_sum_outside = dt_outside.AsEnumerable().Sum(dr => dr.Field<double>("DutyPaymentAmount"));
            }

            lblInsideRows.Text = String.Format("{0:n0}", n_rows_count_inside);
            lblInsideAmount.Text = String.Format("{0:n2}", Math.Round(n_amount_sum_inside, 2));
            lblOutsideRows.Text = String.Format("{0:n0}", n_rows_count_outside);
            lblOutsideAmount.Text = String.Format("{0:n2}", Math.Round(n_amount_sum_outside, 2));

            if (lblInsideRowsRemaining.Text == "0" && lblInsideAmountRemaining.Text == "0")
            {
                lblInsideRowsRemaining.Text = String.Format("{0:n0}", n_rows_count_inside);
                lblInsideAmountRemaining.Text = String.Format("{0:n2}", Math.Round(n_amount_sum_inside, 2));
            }
            if (lblOutsideRowsRemaining.Text == "0" && lblOutsideAmountRemaining.Text == "0")
            {
                lblOutsideRowsRemaining.Text = String.Format("{0:n0}", n_rows_count_outside);
                lblOutsideAmountRemaining.Text = String.Format("{0:n2}", Math.Round(n_amount_sum_outside, 2));
            }
            // KEEP FIELD PRIORITY INTO LIST

            if (ViewState["ListInsideFieldPriority"] == null || ViewState["ListOutsideFieldPriority"] == null || ViewState["ListPaymentFieldPriority"] == null)
            {
                List<string> lst_inside_field_priority = new List<string>();
                List<string> lst_outside_field_priority = new List<string>();
                List<string> lst_payment_field_priority = new List<string>();

                for (int i = 0; i < dt_inside.Columns.Count; i++)
                {
                    string s_column = dt_inside.Columns[i].ColumnName;

                    lst_inside_field_priority.Add(s_column);

                    if (arr_payment_exclude_columns.Contains(s_column) == false) { lst_payment_field_priority.Add(s_column); }
                }

                for (int i = 0; i < dt_outside.Columns.Count; i++)
                {
                    lst_outside_field_priority.Add(dt_outside.Columns[i].ColumnName);
                }

                ViewState["ListInsideFieldPriority"] = lst_inside_field_priority;
                ViewState["ListOutsideFieldPriority"] = lst_outside_field_priority;
                ViewState["ListPaymentFieldPriority"] = lst_payment_field_priority;
            }

            // BIND GridView
            gvInside.DataSource = dt_inside;
            gvInside.DataBind();

            gvOutside.DataSource = dt_outside;
            gvOutside.DataBind();

            // Disable In Process Records   
            string strErrors = string.Empty;
            DataTable dtLockedRecords = new DataTable();
            DataAction.SelectLockedRecords(n_user_id, ref dtLockedRecords, ref strErrors);

            foreach (GridViewRow row in gvInside.Rows)
            {
                if (dtLockedRecords.Rows.Count > 0)
                {
                    var DataFileStrategyID = row.Cells[row.Cells.Count - 1].Text.Replace("&nbsp;", "");// --41
                    foreach (DataRow dr in dtLockedRecords.Rows)
                    {
                        if (DataFileStrategyID.ToLower().Trim().Equals(dr["DataFileID"].ToString().ToLower().Trim()))
                        {
                            var chk = row.Cells[0].Controls[0];//as CheckBox;
                            if (chk != null)
                            {
                                Color lightGrayColor = Color.FromArgb(238, 238, 238);
                                row.BackColor = lightGrayColor;
                                //row.BackColor = System.Drawing.Color.Gray;
                                ((System.Web.UI.HtmlControls.HtmlControl)chk).Disabled = true;
                            }
                        }
                    }
                }
                else if (s_mode == "matching")
                {
                    Color lightBlueColor = Color.FromArgb(221, 235, 247);
                    row.BackColor = lightBlueColor;
                    //row.BackColor = System.Drawing.Color.LightBlue;
                }
                else if (s_mode == "not-matching")
                {
                    if (hidSelectInside.Value != "")
                    {
                        List<string> s_select_inside = hidSelectInside.Value.Split(',').ToList();
                        var chk = row.Cells[0].Controls[0];//as CheckBox;
                        var chkValue = ((System.Web.UI.HtmlControls.HtmlInputControl)chk).Value;
                        var exist = s_select_inside.Find(m => m.Equals(chkValue));
                        if (!string.IsNullOrEmpty(exist))
                        {
                            row.BackColor = System.Drawing.Color.LightYellow;
                        }
                    }
                    else
                        row.BackColor = System.Drawing.Color.White;
                }
                else
                {
                    var MatchingID = row.Cells[7].Text;
                    if (!string.IsNullOrEmpty(MatchingID))
                    {
                        //row.BackColor = System.Drawing.Color.LightBlue;
                        Color lightBlueColor = Color.FromArgb(221, 235, 247);
                        row.BackColor = lightBlueColor;
                    }
                    else
                    {
                        row.BackColor = System.Drawing.Color.White;
                    }
                }
            }

            foreach (GridViewRow row in gvOutside.Rows)
            {
                if (dtLockedRecords.Rows.Count > 0)
                {
                    var DataFileStrategyID = row.Cells[row.Cells.Count - 1].Text.Replace("&nbsp;", ""); // 58
                    foreach (DataRow dr in dtLockedRecords.Rows)
                    {
                        if (DataFileStrategyID.ToLower().Trim().Equals(dr["DataFileID"].ToString().ToLower().Trim()))
                        {
                            var chk = row.Cells[0].Controls[0];//as CheckBox;
                            if (chk != null)
                            {
                                Color lightGrayColor = Color.FromArgb(238, 238, 238);
                                row.BackColor = lightGrayColor;
                                //row.BorderWidth = 3;
                                //row.BorderColor = System.Drawing.Color.Gray;
                                ((System.Web.UI.HtmlControls.HtmlControl)chk).Disabled = true;
                            }
                        }
                    }
                }
                else if (s_mode == "matching")
                {
                    //row.BackColor = System.Drawing.Color.LightBlue;
                    Color lightBlueColor = Color.FromArgb(221, 235, 247);
                    row.BackColor = lightBlueColor;
                }
                else if (s_mode == "not-matching")
                {
                    if (hidSelectOutside.Value != "")
                    {
                        List<string> s_select_outside = hidSelectOutside.Value.Split(',').ToList();
                        var chk = row.Cells[0].Controls[0];//as CheckBox;
                        var chkValue = ((System.Web.UI.HtmlControls.HtmlInputControl)chk).Value;
                        var exist = s_select_outside.Find(m => m.Equals(chkValue));
                        if (!string.IsNullOrEmpty(exist))
                        {
                            row.BackColor = System.Drawing.Color.LightYellow;
                        }
                    }
                    else
                        row.BackColor = System.Drawing.Color.White;
                }
                else
                {
                    var MatchingID = row.Cells[7].Text;
                    if (!string.IsNullOrEmpty(MatchingID))
                    {
                        //row.BackColor = System.Drawing.Color.LightBlue;
                        Color lightBlueColor = Color.FromArgb(221, 235, 247);
                        row.BackColor = lightBlueColor;
                    }
                    else
                    {
                        row.BackColor = System.Drawing.Color.White;
                    }
                }
            }
        }

        public DataSet GetCustomersPageWise(int pageIndex, int pageSize)
        {
            string sortColumnName = Session["sortColumnName_Inside"] != null ? Session["sortColumnName_Inside"].ToString() : "";
            string sortType = Session["hdnOrderSort_Inside"] != null ? Session["hdnOrderSort_Inside"].ToString() : "";
            //string sortTableType = Session["hdnTableType_Inside"] != null ? Session["hdnTableType_Inside"].ToString() : "";

            var Skip = pageIndex == 1 ? 0 : (pageIndex - 1) * pageSize;

            DataTable dt_inside = new DataTable();
            DataTable dt_inside_sum = new DataTable();

            DataTable dt_outside = new DataTable();
            DataTable dt_outside_sum = new DataTable();
            string s_error = "";
            string s_where_inside = Convert.ToString(Session["WhereInside"]);
            string s_where_outside = Convert.ToString(Session["WhereOutside"]);

            string s_order_inside = Convert.ToString(Session["OrderInside"]);
            string s_order_outside = Convert.ToString(Session["OrderOutside"]);

            //DataAction.Select(n_user_id, s_where_inside, s_where_outside, s_order_inside, s_order_outside, pageIndex, pageIndex, pageSize, ref dt_inside, ref dt_inside_sum, ref dt_outside, ref dt_outside_sum, ref s_error);

            DataAction.SelectInside(n_user_id, s_where_inside, s_order_inside, pageIndex, pageSize, ref dt_inside, ref dt_inside_sum, ref s_error, sortColumnName, sortType);

            DataSet ds = new DataSet();

            var dt_inside_Copy = dt_inside.Copy();
            var dt_inside_sum_Copy = dt_inside_sum.Copy();
            var dt_outside_Copy = dt_outside.Copy();
            var dt_outside_sum_Copy = dt_outside_sum.Copy();

            ds.Tables.Add(dt_inside_Copy);
            ds.Tables.Add(dt_inside_sum_Copy);
            ds.Tables.Add(dt_outside_Copy);
            ds.Tables.Add(dt_outside_sum_Copy);
            return ds;
        }

        public DataSet GetInsideDataAll(int pageIndex, int pageSize, int UserId, string ddlTransactions)
        {
            string sortColumnName = Session["sortColumnName_Inside"] != null ? Session["sortColumnName_Inside"].ToString() : "";
            string sortType = Session["hdnOrderSort_Inside"] != null ? Session["hdnOrderSort_Inside"].ToString() : "";
            //string sortTableType = Session["hdnTableType_Inside"] != null ? Session["hdnTableType_Inside"].ToString() : "";

            var Skip = pageIndex == 1 ? 0 : (pageIndex - 1) * pageSize;
            DataTable dt_inside = new DataTable();
            DataTable dt_inside_sum = new DataTable();
            string s_error = "";
            string s_where_inside = "";
            //string s_where_inside = Convert.ToString(Session["WhereInside"]);
            // check why session is null
            //if (string.IsNullOrEmpty(s_where_inside))
            //s_where_inside = " AND QueryID IS NOT NULL ";
            if (ddlTransactions.ToString().ToLower().Trim().Equals("not-matching"))
            {
                s_where_inside = "AND QueryID IS NULL";
            }
            if (ddlTransactions.ToString().ToLower().Trim().Equals("matching"))
            {
                s_where_inside = "AND QueryID IS NOT NULL";
            }
            string s_order_inside = Convert.ToString(Session["OrderInside"]);
            DataAction.SelectInside(UserId, s_where_inside, s_order_inside, pageIndex, pageSize, ref dt_inside, ref dt_inside_sum, ref s_error, sortColumnName, sortType);
            DataSet ds = new DataSet();
            var dt_inside_Copy = dt_inside.Copy();
            var dt_inside_sum_Copy = dt_inside_sum.Copy();

            ds.Tables.Add(dt_inside_Copy);
            ds.Tables.Add(dt_inside_sum_Copy);
            return ds;
        }

        public DataSet GetOutsideDataAll(int pageIndex, int pageSize, int UserId, string ddlTransactions)
        {
            string sortColumnName = Session["sortColumnName_Outside"] != null ? Session["sortColumnName_Outside"].ToString() : "";
            string sortType = Session["hdnOrderSort_Outside"] != null ? Session["hdnOrderSort_Outside"].ToString() : "";
            //string sortTableType = Session["hdnTableType_Outside"] != null ? Session["hdnTableType_Outside"].ToString() : "";

            var Skip = pageIndex == 1 ? 0 : (pageIndex - 1) * pageSize;
            DataTable dt_outside = new DataTable();
            DataTable dt_outside_sum = new DataTable();
            string s_error = "";
            string s_where_outside = ""; // Convert.ToString(Session["WhereOutside"]);
            // check why session is null
            //if (string.IsNullOrEmpty(s_where_outside))
            //s_where_outside = " AND QueryID IS NOT NULL ";
            if (ddlTransactions.ToString().ToLower().Trim().Equals("not-matching"))
            {
                s_where_outside = "AND QueryID IS NULL";
            }
            if (ddlTransactions.ToString().ToLower().Trim().Equals("matching"))
            {
                s_where_outside = "AND QueryID IS NOT NULL";
            }
            string s_order_outside = Convert.ToString(Session["OrderOutside"]);
            DataAction.SelectOutside(UserId, s_where_outside, s_order_outside, pageIndex, pageSize, ref dt_outside, ref dt_outside_sum, ref s_error, sortColumnName, sortType);
            DataSet ds = new DataSet();
            var dt_outside_Copy = dt_outside.Copy();
            var dt_outside_sum_Copy = dt_outside_sum.Copy();

            ds.Tables.Add(dt_outside_Copy);
            ds.Tables.Add(dt_outside_sum_Copy);
            return ds;
        }

        private void Prepare_Table_Inside(DataTable dt_inside, DataTable dt_inside_sum = null)
        {
            // BIND MODE INFO LABEL

            string s_mode = Get_Mode();
            string s_mode_info = "Mode : {0}";

            if (s_mode == "payment")
            {
                s_mode_info = String.Format(s_mode_info, "Single Payment Group");
            }
            else if (s_mode == "match")
            {
                s_mode_info = String.Format(s_mode_info, "Single Match Group");
            }
            else
            {
                s_mode_info = String.Format(s_mode_info, ddlTransactions.SelectedItem.Text);
            }

            lblModeInfo.Text = s_mode_info;

            // BIND SUM LABELS & PAGING DDL-S

            int n_rows_count_inside = 0, n_rows_count_outside = 0;

            double n_amount_sum_inside = 0, n_amount_sum_outside = 0;

            if (dt_inside_sum != null)
            {
                n_rows_count_inside = Convert.ToInt32(dt_inside_sum.Rows[0]["RowsCount"]);

                n_amount_sum_inside = Convert.ToDouble(dt_inside_sum.Rows[0]["AmountSum"]);
            }
            else
            {
                n_rows_count_inside = dt_inside.Rows.Count;

                n_amount_sum_inside = dt_inside.AsEnumerable().Sum(dr => dr.Field<double>("DutyPaymentAmount"));
            }

            lblInsideRows.Text = String.Format("{0:n0}", n_rows_count_inside);
            lblInsideAmount.Text = String.Format("{0:n2}", Math.Round(n_amount_sum_inside, 2));

            lblOutsideRows.Text = String.Format("{0:n0}", n_rows_count_outside);
            lblOutsideAmount.Text = String.Format("{0:n2}", Math.Round(n_amount_sum_outside, 2));

            // KEEP FIELD PRIORITY INTO LIST

            if (ViewState["ListInsideFieldPriority"] == null || ViewState["ListOutsideFieldPriority"] == null || ViewState["ListPaymentFieldPriority"] == null)
            {
                List<string> lst_inside_field_priority = new List<string>();
                List<string> lst_outside_field_priority = new List<string>();
                List<string> lst_payment_field_priority = new List<string>();

                for (int i = 0; i < dt_inside.Columns.Count; i++)
                {
                    string s_column = dt_inside.Columns[i].ColumnName;

                    lst_inside_field_priority.Add(s_column);

                    if (arr_payment_exclude_columns.Contains(s_column) == false) { lst_payment_field_priority.Add(s_column); }
                }

                ViewState["ListInsideFieldPriority"] = lst_inside_field_priority;
                ViewState["ListPaymentFieldPriority"] = lst_payment_field_priority;
            }

            // BIND GridView

            gvInside.DataSource = dt_inside;
            gvInside.DataBind();
        }

        private void CheckBox1_CheckedChanged(Object sender, EventArgs e)
        {

        }

        [WebMethod]
        [ScriptMethod]
        public static string GetInsidedata(int pageIndex, int userId, string hidUniqueID, string hidQueryID, string ddlTransactions, bool IsChkAllCheckBox)
        {
            DataInspector obj = new DataInspector();
            return obj.getInsideHtml(pageIndex, userId, hidUniqueID, hidQueryID, ddlTransactions, IsChkAllCheckBox);
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string GetOutsidedata(int pageIndex, int userId, string hidUniqueID, string hidQueryID, string ddlTransactions, bool IsChkAllCheckBox)
        {
            DataInspector obj = new DataInspector();
            var data = obj.GetOutsideDataAll(pageIndex, 30, userId, ddlTransactions);
            var tableData = data.Tables[0];
            var otherData = data.Tables[1];
            string html = obj.getOutsideHtml(tableData, pageIndex, userId, hidUniqueID, hidQueryID, ddlTransactions, IsChkAllCheckBox);
            return JsonConvert.SerializeObject(new { data = html, description = otherData });
        }

        public string getOutsideHtml(DataTable data, int pageIndex, int userId, string hidUniqueID, string hidQueryID, string ddlTransactions, bool IsChkAllCheckBox)
        {
            DataInspector obj = new DataInspector();
            StringBuilder HTML = new StringBuilder();
            List<string> lst_outside_field_priority = new List<string>();
            // List<string> lst_payment_field_priority = new List<string>();
            for (int i = 0; i < data.Columns.Count; i++)
            {
                string s_column = data.Columns[i].ColumnName;

                lst_outside_field_priority.Add(s_column);
                // if (arr_payment_exclude_columns.Contains(s_column) == false) { lst_payment_field_priority.Add(s_column); }
            }
            TableRow row = null;
            var sWriter = new StringWriter();
            string s_mode = obj.Get_AjaxMode(hidUniqueID, hidQueryID, ddlTransactions);
            /// CHECK ANY Transcation is In Process
            string strErrors = string.Empty;
            DataTable dtLockedRecords = new DataTable();
            DataAction.SelectLockedRecords(userId, ref dtLockedRecords, ref strErrors);
            ///
            using (var htmlWriter = new HtmlTextWriter(sWriter))
            {
                foreach (DataRow dtRow in data.Rows)
                {
                    row = new TableRow();
                    for (int j = 0; j < dtRow.ItemArray.Count(); j++)
                    {
                        TableCell cell = new TableCell();
                        cell.Text = dtRow[j].ToString();
                        row.Cells.Add(cell);

                        //  Modified the code for color change.
                        row.BackColor = System.Drawing.Color.LightYellow;
                        if (dtLockedRecords.Rows.Count > 0)
                        {
                            //var DataFileStrategyID = row.Cells[row.Cells.Count - 1].Text.Replace("&nbsp;", "");
                            var DataFileStrategyID = dtRow.ItemArray[dtRow.ItemArray.Count() - 1].ToString().Replace("&nbsp;", "");
                            if (DataFileStrategyID.ToLower().Trim().Equals(dtRow["DataFileID"].ToString().ToLower().Trim()))
                            {
                                Color lightGrayColor = Color.FromArgb(238, 238, 238);
                                row.BackColor = lightGrayColor;
                            }
                        }
                        else if (s_mode.ToString().ToLower().Trim().Equals("matching"))
                        {
                            Color lightBlueColor = Color.FromArgb(221, 235, 247);
                            row.BackColor = lightBlueColor;
                            if (IsChkAllCheckBox == true)
                            {
                                row.BackColor = System.Drawing.Color.LightYellow;

                            }
                        }
                        else if (s_mode.ToString().ToLower().Trim().Equals("not-matching"))
                        {
                            //if (hidSelectInside.Value != "")
                            //{
                            //    List<string> s_select_inside = hidSelectInside.Value.Split(',').ToList();
                            //    var chk = row.Cells[0].Controls[0];//as CheckBox;
                            //    var chkValue = ((System.Web.UI.HtmlControls.HtmlInputControl)chk).Value;
                            //    var exist = s_select_inside.Find(m => m.Equals(chkValue));
                            //    if (!string.IsNullOrEmpty(exist))
                            //    {
                            //        row.BackColor = System.Drawing.Color.LightYellow;
                            //    }
                            //}
                            //else
                                row.BackColor = System.Drawing.Color.White;
                        }
                        else
                        {
                            //var MatchingID = row.Cells[7].Text;
                            var MatchingID = dtRow.ItemArray[7].ToString();
                            if (!string.IsNullOrEmpty(MatchingID))
                            {
                                //row.BackColor = System.Drawing.Color.LightBlue;
                                Color lightBlueColor = Color.FromArgb(221, 235, 247);
                                row.BackColor = lightBlueColor;
                            }
                            else
                            {
                                row.BackColor = System.Drawing.Color.White;
                            }
                        }
                        // Modified the code for color change.

                    }
                    DataAction.Bind_Grid_Data_Row_Outside(row, lst_outside_field_priority, "", "Outside", s_mode, IsChkAllCheckBox);
                    row.RenderControl(htmlWriter);
                }
            }
            HTML.Append(sWriter.ToString());
            return HTML.ToString();
        }

        public string getInsideHtml(int pageIndex, int userId, string hidUniqueID, string hidQueryID, string ddlTransactions, bool IsChkAllCheckBox)
        {
            DataInspector obj = new DataInspector();
            var data = obj.GetInsideDataAll(pageIndex, 30, userId, ddlTransactions);
            var tableData = data.Tables[0];
            var otherData = data.Tables[1];
            StringBuilder HTML = new StringBuilder();
            List<string> lst_inside_field_priority = new List<string>();
            for (int i = 0; i < tableData.Columns.Count; i++)
            {
                string s_column = tableData.Columns[i].ColumnName;

                lst_inside_field_priority.Add(s_column);
            }
            TableRow row = null;
            var sWriter = new StringWriter();
            string s_mode = obj.Get_AjaxMode(hidUniqueID, hidQueryID, ddlTransactions);
            /////// CHECK ANY Transcation is In Process
            string strErrors = string.Empty;
            DataTable dtLockedRecords = new DataTable();
            DataAction.SelectLockedRecords(userId, ref dtLockedRecords, ref strErrors);
            ////
            using (var htmlWriter = new HtmlTextWriter(sWriter))
            {
                foreach (DataRow dtRow in tableData.Rows)
                {
                    row = new TableRow();
                    for (int j = 0; j < dtRow.ItemArray.Count(); j++)
                    {
                        TableCell cell = new TableCell();
                        cell.Text = dtRow[j].ToString();
                        row.Cells.Add(cell);

                        //  Modified the code for color change.
                        row.BackColor = System.Drawing.Color.LightYellow;
                        if (dtLockedRecords.Rows.Count > 0)
                        {
                            //var DataFileStrategyID = row.Cells[row.Cells.Count - 1].Text.Replace("&nbsp;", "");
                            var DataFileStrategyID = dtRow.ItemArray[dtRow.ItemArray.Count() - 1].ToString().Replace("&nbsp;", "");
                            if (DataFileStrategyID.ToLower().Trim().Equals(dtRow["DataFileID"].ToString().ToLower().Trim()))
                            {
                                Color lightGrayColor = Color.FromArgb(238, 238, 238);
                                row.BackColor = lightGrayColor;
                            }
                        }
                        else if (s_mode.ToString().ToLower().Trim().Equals("matching"))
                        {
                            Color lightBlueColor = Color.FromArgb(221, 235, 247);
                            row.BackColor = lightBlueColor;
                            if (IsChkAllCheckBox == true)
                            {
                                row.BackColor = System.Drawing.Color.LightYellow;
                            }
                        }
                        else if (s_mode.ToString().ToLower().Trim().Equals("not-matching"))
                        {
                        //    if (hidSelectInside.Value != "")
                        //    {
                        //        List<string> s_select_inside = hidSelectInside.Value.Split(',').ToList();
                        //        var chk = row.Cells[0].Controls[0];//as CheckBox;
                        //        var chkValue = ((System.Web.UI.HtmlControls.HtmlInputControl)chk).Value;
                        //        var exist = s_select_inside.Find(m => m.Equals(chkValue));
                        //        if (!string.IsNullOrEmpty(exist))
                        //        {
                        //            row.BackColor = System.Drawing.Color.LightYellow;
                        //        }
                        //    }
                        //    else
                                row.BackColor = System.Drawing.Color.White;
                        }
                        else
                        {
                            //var MatchingID = row.Cells[7].Text;
                            var MatchingID = dtRow.ItemArray[7].ToString();
                            if (!string.IsNullOrEmpty(MatchingID))
                            {
                                //row.BackColor = System.Drawing.Color.LightBlue;
                                Color lightBlueColor = Color.FromArgb(221, 235, 247);
                                row.BackColor = lightBlueColor;
                            }
                            else
                            {
                                row.BackColor = System.Drawing.Color.White;
                            }
                        }
                        // Modified the code for color change.
                    }
                    DataAction.Bind_Grid_Data_Row_Inside(row, lst_inside_field_priority, "", "Inside", s_mode, IsChkAllCheckBox);
                    row.RenderControl(htmlWriter);
                }
            }
            HTML.Append(sWriter.ToString());
            return HTML.ToString();
        }

        [WebMethod]
        [ScriptMethod]
        public static string GetCustomers(int pageIndex)
        {
            System.Threading.Thread.Sleep(1000);
            DataInspector obj = new DataInspector();
            return obj.GetCustomersPageWise(pageIndex, 30).GetXml();
        }
    }
}
