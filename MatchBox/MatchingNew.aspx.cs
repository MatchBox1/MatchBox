using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public partial class MatchingNew : BasePage
    {
        private int n_user_id = 0;

        private string s_cache_matching_search = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            n_user_id = o_user.ID;

            s_cache_matching_search = String.Format("Matching_Search_{0}", n_user_id);

            if (Page.IsPostBack) { return; }

            Bind_Search();
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            divMessage.Visible = lblMessage.Text != "" || lblError.Text != "";

            if (lblError.Text != "") { lblMessage.Text = ""; }
        }

        protected void rblSelectBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            //repCompany.DataSource = null;
            //repCompany.DataBind();

            DataTable dt_company = (DataTable)ViewState["TableCompany"];

            //repCompany.DataSource = dt_company;
            //repCompany.DataBind();
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

            #region Old Checkboxes code
            //string s_select_by = rblSelectBy.SelectedValue;

            //switch (rblSelectBy.SelectedValue)
            //{
            //    case "cashbox":
            //        repNetwork.Visible = true;
            //        repSupplierGroup.Visible = false;

            //        DataTable dt_network = (DataTable)ViewState["TableNetwork"];
            //        DataRow[] dr_network = dt_network.Select(" CompanyID = " + n_company_id);

            //        if (dr_network.Length > 0)
            //        {
            //            repNetwork.DataSource = dr_network.CopyToDataTable();
            //            repNetwork.DataBind();
            //        }

            //        break;
            //    case "terminal":
            //        repNetwork.Visible = false;
            //        repSupplierGroup.Visible = true;

            //        DataTable dt_supplier_group = (DataTable)ViewState["TableSupplierGroup"];
            //        DataRow[] dr_supplier_group = dt_supplier_group.Select(" CompanyID = " + n_company_id);

            //        if (dr_supplier_group.Length > 0)
            //        {
            //            repSupplierGroup.DataSource = dr_supplier_group.CopyToDataTable();
            //            repSupplierGroup.DataBind();
            //        }

            //        break;
            //}
            #endregion Old Checkboxes code
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

        //protected bool CheckDate(String date)
        //{
        //    try
        //    {
        //        DateTime dt = DateTime.Parse(date);
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public void TransactionPaymentDate(string txtDate, ref DateTime? strDateFrom, ref DateTime? strDateTo, ref string s_error)
        {
            s_error = string.Empty;
            try
            {
                //DateTime? transactionDateFrom = null, transactionDateTo = null;
                string transactionDate = txtDate.Trim();

                if (transactionDate != "")
                {
                    bool isPlus = transactionDate.Contains("+");
                    if (isPlus)
                    {
                        string[] transactionDateSplit = transactionDate.Split('+');
                        if (transactionDateSplit[0] != "" && transactionDateSplit[1] != "")
                        {
                            strDateFrom = Convert.ToDateTime(transactionDateSplit[0]);
                            strDateTo = Convert.ToDateTime(transactionDateSplit[1]);
                        }
                        else if (transactionDateSplit[0] == "")
                        {
                            strDateFrom = Convert.ToDateTime("01/01/1900"); // null; //Convert.ToDateTime("01/01/1900");
                            strDateTo = Convert.ToDateTime(transactionDateSplit[1]);
                        }
                        else
                        {
                            strDateFrom = Convert.ToDateTime(transactionDateSplit[0]);
                            strDateTo = DateTime.Now; //null; //Convert.ToDateTime(DateTime.Now.ToShortDateString());
                        }
                    }
                    else
                    {
                        strDateFrom = Convert.ToDateTime(transactionDate);
                        strDateTo = Convert.ToDateTime(transactionDate);
                    }
                }
                else
                {
                    strDateFrom = null; //Convert.ToDateTime("01/01/1900");
                    strDateTo = null; // Convert.ToDateTime(DateTime.Now.ToShortDateString());
                }

                //// Now Check Valid date
                //if(strDateFrom !=null)
                //{
                //    bool validDate = CheckDate(strDateFrom.ToString());
                //    if(!validDate)
                //    {
                //        s_error = "Error on validating date.";
                //    }
                //}
                //if (strDateTo != null)
                //{
                //    bool validDate = CheckDate(strDateTo.ToString());
                //    if (!validDate)
                //    {
                //        s_error = "Error on validating date.";
                //    }
                //}

            }
            catch(Exception ex)
            {
                //s_error = ex.Message;
                s_error = "Error on validating date.";
            }
        }

        protected void btnSearch_Click_New(object sender, EventArgs e)
        {
            int n_strategy_id = Convert.ToInt32(ddlStrategy.SelectedValue);

            MatchingSearchModel o_matching_search = new MatchingSearchModel();

            o_matching_search.UserID = n_user_id;

            string s_error = string.Empty;

            // TransactionDate

            DateTime? transactionDateFrom = null;
            DateTime? transactionDateTo = null;
            TransactionPaymentDate(txtTransactionDate.Text.Trim(), ref transactionDateFrom, ref transactionDateTo, ref s_error);

            if (s_error != "")
            {
                //if (transactionDateFrom == null && transactionDateTo == null)
                //{
                    lblTransactionDateError.Text = "Transaction Date is not valid.";
                    txtTransactionDate.Focus();
                    return;
                //}
            }

            if (transactionDateFrom != null && transactionDateTo != null && transactionDateFrom > transactionDateTo)
            {
                lblTransactionDateError.Text = "'Transaction Date From' can't be greater than 'Transaction Date To'.";
                txtTransactionDate.Focus();
                return;
            }

            // Payment Date

            DateTime? paymentDateFrom = null;
            DateTime? paymentDateTo = null;
            TransactionPaymentDate(txtPaymentDate.Text.Trim(), ref paymentDateFrom, ref paymentDateTo, ref s_error);

            if (s_error != "")
            {
                //if (paymentDateFrom == null && paymentDateTo == null)
                //{
                    lblPaymentDateError.Text = "Payment Date is not valid.";
                    txtPaymentDate.Focus();
                    return;
                //}
            }

            if (paymentDateFrom != null && paymentDateTo != null && paymentDateFrom > paymentDateTo)
            {
                lblPaymentDateError.Text = "'Payment Date From' can't be greater than 'Payment Date To'.";
                txtPaymentDate.Focus();
                return;
            }

            // TransactionDate Outside

            DateTime? transactionDateFromOut = null;
            DateTime? transactionDateToOut = null;
            TransactionPaymentDate(txtTransactionDateOutside.Text.Trim(), ref transactionDateFromOut, ref transactionDateToOut, ref s_error);

            if (s_error != "")
            {
                //if (transactionDateFromOut == null && transactionDateToOut == null)
                //{
                    lblTransactionDateErrorOutside.Text = "Transaction Date is not valid.";
                    txtTransactionDateOutside.Focus();
                    return;
                //}
            }

            if (transactionDateFromOut != null && transactionDateToOut != null && transactionDateFromOut > transactionDateToOut)
            {
                lblTransactionDateErrorOutside.Text = "'Transaction Date From' can't be greater than 'Transaction Date To'.";
                txtTransactionDateOutside.Focus();
                return;
            }

            // Payment Date Outside

            DateTime? paymentDateFromOut = null;
            DateTime? paymentDateToOut = null;
            TransactionPaymentDate(txtPaymentDateOutside.Text.Trim(), ref paymentDateFromOut, ref paymentDateToOut, ref s_error);

            if (s_error != "")
            {
                //if (paymentDateFromOut == null && paymentDateToOut == null)
                //{
                    lblPaymentDateErrorOutside.Text = "Payment Date is not valid.";
                    txtPaymentDateOutside.Focus();
                    return;
                //}
            }

            if (paymentDateFromOut != null && paymentDateToOut != null && paymentDateFromOut > paymentDateToOut)
            {
                lblPaymentDateErrorOutside.Text = "'Payment Date From' can't be greater than 'Payment Date To'.";
                txtPaymentDateOutside.Focus();
                return;
            }

            DateFilterDataModel o_date_filter = new DateFilterDataModel();
            // inside
            o_matching_search.TransactionDateFrom = o_date_filter.TransactionDateFrom = transactionDateFrom;
            o_matching_search.TransactionDateTo = o_date_filter.TransactionDateTo = transactionDateTo;
            o_matching_search.PaymentDateFrom = o_date_filter.PaymentDateFrom = paymentDateFrom;
            o_matching_search.PaymentDateTo = o_date_filter.PaymentDateTo = paymentDateTo;
            o_matching_search.IsEmptyTransactionDate = o_date_filter.IsEmptyTransactionDate = chkEmptyTransactionDate.Checked;
            o_matching_search.IsEmptyPaymentDate = o_date_filter.IsEmptyPaymentDate = chkEmptyPaymentDate.Checked;
            //outside
            o_matching_search.TransactionDateFromOut = o_date_filter.TransactionDateFromOut = transactionDateFromOut;
            o_matching_search.TransactionDateToOut = o_date_filter.TransactionDateToOut = transactionDateToOut;
            o_matching_search.PaymentDateFromOut = o_date_filter.PaymentDateFromOut = paymentDateFromOut;
            o_matching_search.PaymentDateToOut = o_date_filter.PaymentDateToOut = paymentDateToOut;
            o_matching_search.IsEmptyTransactionDateOut = o_date_filter.IsEmptyTransactionDateOut = chkEmptyTransactionDateOutside.Checked;
            o_matching_search.IsEmptyPaymentDateOut = o_date_filter.IsEmptyPaymentDateOut = chkEmptyPaymentDateOutside.Checked;

            //////////////
            ViewState["DateFilterData"] = o_date_filter;
            //////////////

            MatchingAction.Select_Search(ref o_matching_search);

            if (o_matching_search.ErrorMessage != "")
            {
                lblError.Text = o_matching_search.ErrorMessage;
                return;
            }

            if (o_matching_search.TableInsideSum.Rows.Count == 0 || o_matching_search.TableOutsideSum.Rows.Count == 0)
            {
                pnlAutoMatching.Visible = false;

                lblError.Text = "Can't create matching without data in both sides (inside & outside).";

                return;
            }

            // USE Cache CLASS TO STORE A LARGE DATA - o_matching_search

            Cache.Insert(s_cache_matching_search, o_matching_search, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);

            // SHOW RESULT

            pnlSearchResult.Visible = true;

            repInsideSum.DataSource = o_matching_search.TableInsideSum;
            repInsideSum.DataBind();

            repOutsideSum.DataSource = o_matching_search.TableOutsideSum;
            repOutsideSum.DataBind();

            pnlAutoMatching.Visible = true;
        }

        protected void repInsideSum_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                MatchingSearchModel o_matching_search = (MatchingSearchModel)Cache[s_cache_matching_search];

                if (o_matching_search.TableInsideSumTotal.Rows.Count > 0)
                {
                    DataRow dr_inside_sum_total = o_matching_search.TableInsideSumTotal.Rows[0];

                    int n_transaction_count_total = 0;
                    int.TryParse(dr_inside_sum_total["TransactionCountTotal"].ToString(), out n_transaction_count_total);

                    double n_amount_sum_total = 0;
                    double.TryParse(dr_inside_sum_total["AmountSumTotal"].ToString(), out n_amount_sum_total);

                    Label lblTransactionCountTotal = (Label)e.Item.FindControl("lblTransactionCountTotal");
                    Label lblAmountSumTotal = (Label)e.Item.FindControl("lblAmountSumTotal");

                    lblTransactionCountTotal.Text = String.Format("{0:n0}", n_transaction_count_total);
                    lblAmountSumTotal.Text = String.Format("{0:n2}", n_amount_sum_total);
                }
            }
        }

        protected void repOutsideSum_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                MatchingSearchModel o_matching_search = (MatchingSearchModel)Cache[s_cache_matching_search];

                if (o_matching_search.TableOutsideSumTotal.Rows.Count > 0)
                {
                    DataRow dr_outside_sum_total = o_matching_search.TableOutsideSumTotal.Rows[0];

                    int n_transaction_count_total = 0;
                    int.TryParse(dr_outside_sum_total["TransactionCountTotal"].ToString(), out n_transaction_count_total);

                    double n_amount_sum_total = 0;
                    double.TryParse(dr_outside_sum_total["AmountSumTotal"].ToString(), out n_amount_sum_total);

                    Label lblTransactionCountTotal = (Label)e.Item.FindControl("lblTransactionCountTotal");
                    Label lblAmountSumTotal = (Label)e.Item.FindControl("lblAmountSumTotal");

                    lblTransactionCountTotal.Text = String.Format("{0:n0}", n_transaction_count_total);
                    lblAmountSumTotal.Text = String.Format("{0:n2}", n_amount_sum_total);
                }
            }
        }

        protected void btnAutoMatching_Click(object sender, EventArgs e)
        {
            int n_strategy_id = Convert.ToInt32(ddlStrategy.SelectedValue);

            int intCountDataFile = Int32.MinValue;
            string s_error = string.Empty;
            MatchingAction.CheckStrategy_DataFile(n_user_id, n_strategy_id, ref intCountDataFile,ref s_error);
            if (intCountDataFile > 0)
            {
                lblErrorBelow.Text = "Please delete all datafiles in strategy before proceeding further with auto matching.";
                return;
            }

            //////// save filter /////////////////////
            DateFilterDataModel o_matching_search = (DateFilterDataModel)ViewState["DateFilterData"];

            int CountDataFile = 0;
            s_error = "";
            MatchingAction.SaveStrategy_DateFilter(o_matching_search, n_strategy_id, ref CountDataFile, ref s_error);
            if (s_error != "")
            {
                lblErrorBelow.Text = s_error;
                return;
            }
            ////// set strategy In Process ////////


            s_error = "";

            DataTable dt_strategy = new DataTable();

            StrategyAction.Select_Single(n_strategy_id, n_user_id,"fromFilterData", ref dt_strategy, ref s_error);

            if (s_error != "") { goto Error; }

            if (dt_strategy.Rows.Count == 0)
            {
                s_error = "Strategy not found.";
                goto Error;
            }

            DataRow dr_strategy = dt_strategy.Rows[0];

            int n_status_id = Convert.ToInt32(dr_strategy["StatusID"]);

            switch (n_status_id)
            {
                case 0:
                    string s_last_execution_date = dr_strategy["LastExecutionDate"].ToString().Trim();

                    if (s_last_execution_date != "")
                    {
                        DateTime d_last_execution_date = Convert.ToDateTime(s_last_execution_date);

                        TimeSpan t_after_execution = DateTime.Now - d_last_execution_date;

                        if (t_after_execution.Minutes < 1)
                        {
                            s_error = String.Format("Strategy already executed. Next execution will be available after {0}.", d_last_execution_date.AddMinutes(1));
                            goto Error;
                        }
                    }

                    n_status_id = 1;

                    break;
                case 1:
                    n_status_id = 0;
                    break;
                case 2:
                    Response.Redirect(String.Format("FrameForm.aspx?i={0}&q={1}&p={2}", Request.QueryString.Get("i"), Request.QueryString.Get("q"), "data"));
                    return;
                default:
                    s_error = "Status not valid.";
                    goto Error;
            }

            //hidStatusID.Value = n_status_id.ToString();

            int n_rows_affected = StrategyAction.Update_Status(n_user_id, n_strategy_id, n_status_id, ref s_error);

            if (s_error != "") { goto Error; }

            //Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}&p={3}", "updated", Request.QueryString.Get("i"), Request.QueryString.Get("q"), "data"));

            return;

            Error:

            lblError.Text = s_error;

            ///////////////////////////////////////////////////



            //StrategyModel o_strategy = new StrategyModel();

            //o_strategy.ID = n_strategy_id;
            //o_strategy.UserID = n_user_id;

            //StrategyAction.Select(ref o_strategy);

            //if (o_strategy.ErrorMessage != "")
            //{
            //    lblError.Text = o_strategy.ErrorMessage;
            //    return;
            //}

            //// Select_Data

            //DataTable dt_inside = new DataTable();
            //DataTable dt_outside = new DataTable();
            //DataTable dt_inside_sum = new DataTable();
            //DataTable dt_outside_sum = new DataTable();

            //MatchingSearchModel o_matching_search = (MatchingSearchModel)Cache[s_cache_matching_search];

            //s_error = "";

            //MatchingAction.Select_Data(o_matching_search, false, ref dt_inside, ref dt_outside, ref dt_inside_sum, ref dt_outside_sum, ref s_error);

            //if (s_error != "")
            //{
            //    lblError.Text = s_error;
            //    return;
            //}

            //// GET dt_matching_field / lst_matching_field / n_company_id. RETURNED CORRECT FIELDS COLLECTION FOR SELECTED DATA ( dt_matching_inside & dt_matching_outside ).

            //List<string> lst_matching_field = new List<string>();

            //int n_company_id = 0;

            //DataTable dt_matching_field = MatchingAction.Table_Matching_Field(dt_inside, dt_outside, o_strategy.TableStrategyField, ref lst_matching_field, ref n_company_id, ref s_error);

            //if (s_error != "")
            //{
            //    lblError.Text = s_error;
            //    return;
            //}

            //// GET dt_query / dt_matching_query / n_last_query

            //int n_last_query = 0;

            //DataTable dt_query = Get_Query_Table(dt_matching_field, lst_matching_field, o_strategy.QueryPercent, ref n_last_query);

            //int n_query_count = dt_query.Rows.Count;

            //if (n_query_count == 0)
            //{
            //    lblError.Text = "No query exists.";
            //    return;
            //}

            //// CLONE dt_matching_query FROM dt_query ( STRUCTURE ONLY )

            //DataTable dt_matching_query = dt_query.Clone();

            //// GET dt_matching_data / dt_matching_balance

            //DataTable dt_matching_data = Get_Matching_Data_Table();
            //DataTable dt_matching_balance = Get_Matching_Balance_Table();

            //// GET dt_matching_inside / dt_matching_outside

            //DataTable dt_matching_inside = new DataView(dt_inside).ToTable(true, lst_matching_field.ToArray());
            //DataTable dt_matching_outside = new DataView(dt_outside).ToTable(true, lst_matching_field.ToArray());

            //// EXECUTE ONE TO ONE MATCHING

            //DateTime d_matching_start = DateTime.Now;

            //Execute_Matching(ref dt_matching_query, ref dt_matching_data, ref dt_matching_inside, ref dt_matching_outside, ref dt_matching_balance, ref dt_query, 1, n_last_query, n_company_id);   // ONE TO ONE - MatchingTypeID = 1

            //DateTime d_matching_end = DateTime.Now;

            //bool b_one_to_one = (dt_matching_data.Rows.Count > 0);

            //// GET MATCHING OBJECT

            //MatchingModel o_matching = new MatchingModel();

            //o_matching.UserID = n_user_id;
            //o_matching.StrategyID = n_strategy_id;

            //TimeSpan t_duration = (d_matching_end - d_matching_start);

            //if (dt_inside_sum.Rows.Count > 0)
            //{
            //    DataRow dr_inside_sum_total = dt_inside_sum.Rows[0];

            //    int n_transaction_count = 0;
            //    int.TryParse(dr_inside_sum_total["TransactionCount"].ToString(), out n_transaction_count);

            //    double n_amount_sum = 0;
            //    double.TryParse(dr_inside_sum_total["AmountSum"].ToString(), out n_amount_sum);

            //    o_matching.AllInsideCount = n_transaction_count;
            //    o_matching.AllInsideAmount = n_amount_sum;
            //}

            //if (dt_outside_sum.Rows.Count > 0)
            //{
            //    DataRow dr_outside_sum_total = dt_outside_sum.Rows[0];

            //    int n_transaction_count = 0;
            //    int.TryParse(dr_outside_sum_total["TransactionCount"].ToString(), out n_transaction_count);

            //    double n_amount_sum = 0;
            //    double.TryParse(dr_outside_sum_total["AmountSum"].ToString(), out n_amount_sum);

            //    o_matching.AllOutsideCount = n_transaction_count;
            //    o_matching.AllOutsideAmount = n_amount_sum;
            //}

            //// UPDATE DATABASE WITH ONE TO ONE MATCHING

            //if (b_one_to_one == true)
            //{
            //    // CORRECT dt_matching_query COLUMNS ORDER

            //    Correct_Matching_Query_Table(ref dt_matching_query);

            //    MatchingAction.Update(ref o_matching, dt_matching_query, dt_matching_data, dt_matching_balance);

            //    if (o_matching.ErrorMessage != "")
            //    {
            //        lblError.Text = o_matching.ErrorMessage;
            //        return;
            //    }

            //    if (o_matching.ID <= 0)
            //    {
            //        lblError.Text = "Matching was not created (one to one).";
            //        return;
            //    }
            //}

            //// PROCESS MANY TO MANY / PROCESS ZERO AMOUNT ( DutyPaymentAmount = 0 )

            //bool b_many_to_many = false, b_zero = false;

            //if (dt_matching_inside.Select(" QueryID = '' ").FirstOrDefault() != null && dt_matching_outside.Select(" QueryID = '' ").FirstOrDefault() != null)
            //{
            //    Process_Many_To_Many(o_matching_search, o_matching, dt_matching_field, lst_matching_field, n_company_id, o_strategy.QueryPercent, ref b_many_to_many, ref b_zero, ref s_error);

            //    if (s_error != "")
            //    {
            //        lblError.Text = s_error;
            //        return;
            //    }
            //}

            //if (b_one_to_one == false && b_many_to_many == false && b_zero == false)
            //{
            //    lblError.Text = "No matching was found.";
            //    lblQueryInfo.Text = String.Format("Total queries: {0}.<br />Queries Percent: {1}%.<br />Last query number: {2}.<br />Execution time: {3}", n_query_count, o_strategy.QueryPercent, n_last_query, t_duration);
            //    return;
            //}

            //// REMOVE Matching_Search+XXX FROM CACHE

            //Cache.Remove(s_cache_matching_search);

            //// REDIRECT

            //string s_guid = Guid.NewGuid().ToString();

            //((Dictionary<string, int>)Session["MatchingLookupTable"]).Add(s_guid, o_matching.ID);

            //Response.Redirect("MatchingView.aspx?result=inserted");

            //// TEMP ===
            ////pnlQuery.Visible = true;

            ////gvQuery.DataSource = dt_matching_query;
            ////gvQuery.DataBind();

            ////DataRow[] dr_matching_inside = dt_matching_inside.Select(" QueryID <> '' ");
            ////DataRow[] dr_matching_outside = dt_matching_outside.Select(" QueryID <> '' ");

            ////if (dr_matching_inside.Length > 0)
            ////{
            ////    gvInside.DataSource = dr_matching_inside.CopyToDataTable();
            ////    gvInside.DataBind();
            ////}

            ////if (dr_matching_outside.Length > 0)
            ////{
            ////    gvOutside.DataSource = dr_matching_outside.CopyToDataTable();
            ////    gvOutside.DataBind();
            ////}

            ////gvMatching.DataSource = dt_matching_data;
            ////gvMatching.DataBind();

            ////lblQueryCount.Text = dt_matching_query.Rows.Count.ToString();
            ////lblInsideCount.Text = dr_matching_inside.Length.ToString();
            ////lblOutsideCount.Text = dr_matching_outside.Length.ToString();
            ////lblMatchingCount.Text = dt_matching_data.Rows.Count.ToString();

            ////return;
            //// TEMP ===
        }

        private DataTable Get_Query_Table(DataTable dt_matching_field, List<string> lst_matching_field, double n_query_percent, ref int n_last_query)
        {
            // IF CardNumber EXISTS THEN Select_Query FUNCTION CREATE ADITIONAL QUERY FOR CardNumber WITHOUT CardPrefix IN LOWER PRIORITY.
            // IF CardPrefix EXISTS AND CardNumber NOT EXISTS THEN Select_Query FUNCTION CREATE QUERY ONLY FOR CardPrefix.

            DataTable dt_query = new DataTable();

            bool b_add_card_prefix = (lst_matching_field.Contains("CardPrefix") == true && dt_matching_field.Select(" StrategyField = 'CardPrefix' ").FirstOrDefault() == null);

            StrategyAction.Select_Query(dt_matching_field, b_add_card_prefix, false, ref dt_query);

            int n_query_count = dt_query.Rows.Count;

            if (n_query_count == 0) { goto Finish; }

            dt_query.Columns.Add("ID");                 // Equivalent of QueryID
            dt_query.Columns.Add("MatchingTypeID");     // Type Of Matching - One To One / Many To Many / Zero Amount

            // CALCULATE LAST QUERY NUMBER

            double n_last_query_double = (double)n_query_count / (double)100 * n_query_percent;

            n_last_query = (int)Math.Round(n_last_query_double, 0);

            if (n_last_query > n_query_count) { n_last_query = n_query_count; }

        Finish:

            return dt_query;
        }

        private DataTable Get_Matching_Data_Table()
        {
            DataTable dt_matching_data = new DataTable();

            dt_matching_data.Columns.Add("QueryID");
            dt_matching_data.Columns.Add("InsideID");
            dt_matching_data.Columns.Add("OutsideID");

            return dt_matching_data;
        }

        private DataTable Get_Matching_Balance_Table()
        {
            DataTable dt_matching_balance = new DataTable();

            dt_matching_balance.Columns.Add("CompanyID", typeof(int));
            dt_matching_balance.Columns.Add("OperationTypeID", typeof(int));
            dt_matching_balance.Columns.Add("DutyPaymentAmount", typeof(double));
            dt_matching_balance.Columns.Add("QueryID", typeof(string));

            return dt_matching_balance;
        }

        private void Execute_Matching(ref DataTable dt_matching_query, ref DataTable dt_matching_data, ref DataTable dt_matching_inside, ref DataTable dt_matching_outside, ref DataTable dt_matching_balance, ref DataTable dt_query, int n_matching_type_id, int n_last_query, int n_company_id)
        {
            // SELECT TABLE WITH MINIMUM ROWS COUNT FOR SCAN - dt_first CONTAINS SMALL DATA AND dt_second LARGE.

            bool b_outside_first = (dt_matching_outside.Rows.Count < dt_matching_inside.Rows.Count);

            DataTable dt_first = new DataTable();
            DataTable dt_second = new DataTable();

            if (b_outside_first == true)
            {
                dt_first = dt_matching_outside.Copy();
                dt_second = dt_matching_inside.Copy();
            }
            else
            {
                dt_first = dt_matching_inside.Copy();
                dt_second = dt_matching_outside.Copy();
            }

            for (int i = 0; i < n_last_query; i++)
            {
                DataRow dr_query = dt_query.Rows[i];

                foreach (DataRow dr_first in dt_first.Select(" QueryID = '' "))
                {
                    string s_query = " QueryID = '' ";

                    if (dt_first.Columns.Contains("CompanyID"))
                    {
                        s_query += " AND CompanyID = " + dr_first["CompanyID"];
                    }

                    foreach (DataColumn dc_query in dt_query.Columns)
                    {
                        string s_query_field = dc_query.ColumnName;

                        if (s_query_field == "ID" || s_query_field == "MatchingTypeID" || s_query_field == "QueryNumber") { continue; }     // !!! CONTINUE IF s_query_field NOT IN STRATEGY

                        string s_query_value = dr_query[s_query_field].ToString().Trim();

                        if (s_query_value == "") { continue; }      // !!! CONTINUE IF s_query_field NOT CONTAINS VALUE

                        s_query += " AND ";

                        switch (s_query_field)
                        {
                            case "TerminalNumber":
                                int n_terminal = 0;
                                int.TryParse(dr_first["TerminalID"].ToString(), out n_terminal);

                                s_query += " TerminalID = " + n_terminal;

                                break;
                            case "SupplierNumber":
                                int n_supplier = 0;
                                int.TryParse(dr_first["SupplierID"].ToString(), out n_supplier);

                                s_query += " SupplierID = " + n_supplier;

                                break;
                            case "CreditBrand":
                                int n_credit = 0;
                                int.TryParse(dr_first["CreditID"].ToString(), out n_credit);

                                s_query += " CreditID = " + n_credit;

                                break;
                            case "CardBrand":
                                int n_card = 0;
                                int.TryParse(dr_first["CardID"].ToString(), out n_card);

                                s_query += " CardID = " + n_card;

                                break;
                            case "TransactionCurrency":
                                int n_currency = 0;
                                int.TryParse(dr_first["TransactionCurrencyID"].ToString(), out n_currency);

                                s_query += " TransactionCurrencyID = " + n_currency;

                                break;
                            case "CardNumber":
                                int n_card_number = 0;
                                int.TryParse(dr_first["CardNumber"].ToString(), out n_card_number);

                                s_query += " CardNumber = " + n_card_number;

                                break;
                            case "CardPrefix":
                                int n_card_prefix = 0;
                                int.TryParse(dr_first["CardPrefix"].ToString(), out n_card_prefix);

                                s_query += " CardPrefix = " + n_card_prefix;

                                break;
                            case "PaymentsCount":
                            case "DutyPaymentNumber":
                            case "TransmissionNumber":
                                // TransmissionNumber is a long
                                long n_long = 0;
                                long.TryParse(dr_first[s_query_field].ToString(), out n_long);

                                s_query += " " + s_query_field + " = " + n_long;

                                break;
                            case "TransactionGrossAmount":
                            case "DutyPaymentAmount":
                                double n_amount = 0;
                                double.TryParse(dr_first[s_query_field].ToString(), out n_amount);

                                if (s_query_value == "*" || n_amount == 0)
                                {
                                    s_query += " " + s_query_field + " = " + dr_first[s_query_field];
                                }
                                else
                                {
                                    double n_amount_tolerance = 0;
                                    double.TryParse(s_query_value, out n_amount_tolerance);

                                    double n_amount_min = n_amount - n_amount_tolerance;
                                    double n_amount_max = n_amount + n_amount_tolerance;

                                    s_query += " ( " + s_query_field + " >= " + n_amount_min + " AND " + s_query_field + " <= " + n_amount_max + " ) ";
                                }

                                break;
                            case "TransactionDate":
                            case "PaymentDate":
                                string s_date = dr_first[s_query_field].ToString().Trim();

                                DateTime d_date = new DateTime();

                                bool b_date = DateTime.TryParse(s_date, out d_date);

                                if (s_query_value != "*" && b_date == true)
                                {
                                    int n_days = 0;
                                    int.TryParse(s_query_value, out n_days);

                                    DateTime d_date_min = d_date.AddDays(0 - n_days);
                                    DateTime d_date_max = d_date.AddDays(n_days);

                                    s_query += " ( " + s_query_field + " >= '" + d_date_min + "' AND " + s_query_field + " <= '" + d_date_max + "' ) ";
                                }
                                else
                                {
                                    s_query += " " + s_query_field + " = '" + dr_first[s_query_field] + "'";
                                }

                                break;
                            case "VoucherNumber":
                            case "ConfirmationNumber":
                                string s_string = dr_first[s_query_field].ToString().Trim();

                                bool b_tolerance = false;

                                int n_start_char = 0;

                                if (s_query_value != "*" && s_string != "")
                                {
                                    int.TryParse(s_query_value, out n_start_char);

                                    b_tolerance = (n_start_char > 0 && s_string.Length > n_start_char);
                                }

                                if (b_tolerance == false)
                                {
                                    s_query += " " + s_query_field + " = '" + dr_first[s_query_field] + "' ";
                                }
                                else
                                {
                                    string s_string_sub = s_string.Substring(n_start_char);

                                    s_query += " " + s_query_field + " LIKE '%" + s_string_sub + "' ";
                                }

                                break;
                        }
                    }

                    dt_second.CaseSensitive = false;

                    DataRow[] dr_second_arr = dt_second.Select(s_query);

                    if (dr_second_arr.Length == 1)
                    {
                        DataRow dr_second = dr_second_arr[0];

                        if (dt_first.Columns.Contains("CompanyID"))
                        {
                            int n_inside_company_id = 0, n_outside_company_id = 0;

                            int.TryParse(dr_first["CompanyID"].ToString(), out n_inside_company_id);
                            int.TryParse(dr_second["CompanyID"].ToString(), out n_outside_company_id);

                            if (n_inside_company_id == n_outside_company_id)
                            {
                                n_company_id = n_inside_company_id;
                            }
                            else
                            {
                                n_company_id = 0;
                            }
                        }

                        if (n_company_id == 0) { continue; }    // !!! CONTINUE IF n_company_id NOT EXISTS

                        string s_guid_query_id = Guid.NewGuid().ToString();

                        double n_amount_first = 0, n_amount_second = 0;

                        double.TryParse(dr_first["DutyPaymentAmount"].ToString(), out n_amount_first);
                        double.TryParse(dr_second["DutyPaymentAmount"].ToString(), out n_amount_second);

                        if (n_amount_first != n_amount_second)
                        {
                            double n_balance_amount = (b_outside_first == true) ? n_amount_first - n_amount_second : n_amount_second - n_amount_first;

                            DataRow dr_matching_balance = dt_matching_balance.NewRow();

                            dr_matching_balance["CompanyID"] = n_company_id;
                            dr_matching_balance["OperationTypeID"] = 6; // !!! 'ביטול יתרה' !!!
                            dr_matching_balance["DutyPaymentAmount"] = n_balance_amount;
                            dr_matching_balance["QueryID"] = s_guid_query_id;

                            dt_matching_balance.Rows.Add(dr_matching_balance);
                        }

                        dr_query["ID"] = s_guid_query_id;
                        dr_query["MatchingTypeID"] = n_matching_type_id;

                        dr_first["QueryID"] = s_guid_query_id;
                        dr_second["QueryID"] = s_guid_query_id;

                        dt_matching_query.ImportRow(dr_query);

                        string[] arr_id_first = dr_first["ID"].ToString().Split(',');
                        string[] arr_id_second = dr_second["ID"].ToString().Split(',');

                        int n_arr_length = (arr_id_first.Length < arr_id_second.Length) ? arr_id_second.Length : arr_id_first.Length;

                        for (int i_arr = 0; i_arr < n_arr_length; i_arr++)
                        {
                            string s_id_first = "", s_id_second = "";

                            if (i_arr < arr_id_first.Length) { s_id_first = arr_id_first[i_arr].Trim(); }
                            if (i_arr < arr_id_second.Length) { s_id_second = arr_id_second[i_arr].Trim(); }

                            decimal n_id_first = 0, n_id_second = 0;

                            decimal.TryParse(s_id_first, out n_id_first);
                            decimal.TryParse(s_id_second, out n_id_second);

                            DataRow dr_matching_data = dt_matching_data.NewRow();

                            dr_matching_data["QueryID"] = s_guid_query_id;

                            if (b_outside_first == true)
                            {
                                dr_matching_data["InsideID"] = n_id_second;
                                dr_matching_data["OutsideID"] = n_id_first;
                            }
                            else
                            {
                                dr_matching_data["InsideID"] = n_id_first;
                                dr_matching_data["OutsideID"] = n_id_second;
                            }

                            dt_matching_data.Rows.Add(dr_matching_data);
                        }
                    }
                }
            }

            // CLEAR dt_query [ID] AND [MatchingTypeID] COLUMNS

            dt_query.Columns.Remove("ID");
            dt_query.Columns.Remove("MatchingTypeID");

            dt_query.Columns.Add("ID");
            dt_query.Columns.Add("MatchingTypeID");

            // RESTORE dt_matching_inside & dt_matching_outside

            if (b_outside_first == true)
            {
                dt_matching_outside = dt_first.Copy();
                dt_matching_inside = dt_second.Copy();
            }
            else
            {
                dt_matching_inside = dt_first.Copy();
                dt_matching_outside = dt_second.Copy();
            }
        }

        private void Process_Many_To_Many(MatchingSearchModel o_matching_search, MatchingModel o_matching, DataTable dt_matching_field, List<string> lst_matching_field, int n_company_id, double n_query_percent, ref bool b_many_to_many, ref bool b_zero, ref string s_error)
        {
            // Select_Data

            DataTable dt_inside = new DataTable();
            DataTable dt_outside = new DataTable();
            DataTable dt_inside_sum = null, dt_outside_sum = null;

            MatchingAction.Select_Data(o_matching_search, true, ref dt_inside, ref dt_outside, ref dt_inside_sum, ref dt_outside_sum, ref s_error);

            if (s_error != "") { return; }

            // CORRECT FIELDS COLLECTION FOR MANY TO MANY

            lst_matching_field.Remove("DutyPaymentNumber");
            lst_matching_field.Remove("PaymentDate");

            foreach (DataRow dr_matching_field in dt_matching_field.Select(" StrategyField = 'DutyPaymentNumber' OR StrategyField = 'PaymentDate' "))
            {
                dt_matching_field.Rows.Remove(dr_matching_field);
            }

            // GET dt_query / n_last_query

            int n_last_query = 0;

            DataTable dt_query = Get_Query_Table(dt_matching_field, lst_matching_field, n_query_percent, ref n_last_query);

            int n_query_count = dt_query.Rows.Count;

            if (n_query_count == 0) { return; }

            // CLONE dt_matching_query FROM dt_query ( STRUCTURE ONLY )

            DataTable dt_matching_query = dt_query.Clone();     

            // GET dt_matching_data / dt_matching_balance

            DataTable dt_matching_data = Get_Matching_Data_Table();
            DataTable dt_matching_balance = Get_Matching_Balance_Table();

            // GET dt_matching_inside / dt_matching_outside

            DataTable dt_matching_inside = new DataView(dt_inside).ToTable(true, lst_matching_field.ToArray());
            DataTable dt_matching_outside = new DataView(dt_outside).ToTable(true, lst_matching_field.ToArray());

            // Execute Many To_Many

            DateTime d_matching_start = DateTime.Now;

            Execute_Matching(ref dt_matching_query, ref dt_matching_data, ref dt_matching_inside, ref dt_matching_outside, ref dt_matching_balance, ref dt_query, 2, n_last_query, n_company_id);   // Many To_Many - MatchingTypeID = 2

            DateTime d_matching_end = DateTime.Now;

            TimeSpan t_duration = (d_matching_end - d_matching_start);

            b_many_to_many = (dt_matching_data.Rows.Count > 0);

            // PROCESS ZERO AMOUNT ( DutyPaymentAmount = 0 )

            DataRow[] dr_matching_inside = dt_matching_inside.Select(" QueryID = '' AND DutyPaymentAmount = 0 ");
            DataRow[] dr_matching_outside = dt_matching_outside.Select(" QueryID = '' AND DutyPaymentAmount = 0 ");

            b_zero = (dr_matching_inside.Length > 0 || dr_matching_outside.Length > 0);

            for (int i = 0; i < dr_matching_inside.Length; i++)
            {
                double n_duty_payment_amount = 0;
                double.TryParse(dr_matching_inside[i]["DutyPaymentAmount"].ToString(), out n_duty_payment_amount);

                if (n_duty_payment_amount == 0)
                {
                    string s_guid_query_id = Guid.NewGuid().ToString();

                    // UPDATE dt_matching_query

                    DataRow dr_matching_query = dt_matching_query.NewRow();

                    dr_matching_query["ID"] = s_guid_query_id;
                    dr_matching_query["MatchingTypeID"] = 3;    // ZERO AMOUNT - MatchingTypeID = 3
                    dr_matching_query["QueryNumber"] = 0;

                    dt_matching_query.Rows.Add(dr_matching_query);

                    // UPDATE dt_matching_data

                    string[] arr_id = dr_matching_inside[i]["ID"].ToString().Split(',');

                    for (int i_arr = 0; i_arr < arr_id.Length; i_arr++)
                    {
                        decimal n_id = 0;
                        decimal.TryParse(arr_id[i_arr].Trim(), out n_id);

                        DataRow dr_matching_data = dt_matching_data.NewRow();

                        dr_matching_data["QueryID"] = s_guid_query_id;
                        dr_matching_data["InsideID"] = n_id;
                        dr_matching_data["OutsideID"] = 0;

                        dt_matching_data.Rows.Add(dr_matching_data);
                    }
                }
            }

            for (int i = 0; i < dr_matching_outside.Length; i++)
            {
                double n_duty_payment_amount = 0;
                double.TryParse(dr_matching_inside[i]["DutyPaymentAmount"].ToString(), out n_duty_payment_amount);

                if (n_duty_payment_amount == 0)
                {
                    string s_guid_query_id = Guid.NewGuid().ToString();

                    // UPDATE dt_matching_query

                    DataRow dr_matching_query = dt_matching_query.NewRow();

                    dr_matching_query["ID"] = s_guid_query_id;
                    dr_matching_query["MatchingTypeID"] = 3;    // ZERO AMOUNT - MatchingTypeID = 3
                    dr_matching_query["QueryNumber"] = 0;

                    dt_matching_query.Rows.Add(dr_matching_query);

                    // UPDATE dt_matching_data

                    string[] arr_id = dr_matching_inside[i]["ID"].ToString().Split(',');

                    for (int i_arr = 0; i_arr < arr_id.Length; i_arr++)
                    {
                        decimal n_id = 0;
                        decimal.TryParse(arr_id[i_arr].Trim(), out n_id);

                        DataRow dr_matching_data = dt_matching_data.NewRow();

                        dr_matching_data["QueryID"] = s_guid_query_id;
                        dr_matching_data["InsideID"] = 0;
                        dr_matching_data["OutsideID"] = n_id;

                        dt_matching_data.Rows.Add(dr_matching_data);
                    }
                }
            }

            // UPDATE

            if (b_many_to_many == true || b_zero == true)
            {
                // CORRECT dt_matching_query COLUMNS ORDER

                Correct_Matching_Query_Table(ref dt_matching_query);

                MatchingAction.Update(ref o_matching, dt_matching_query, dt_matching_data, dt_matching_balance);

                if (o_matching.ErrorMessage != "")
                {
                    s_error = o_matching.ErrorMessage;
                    return;
                }

                if (o_matching.ID <= 0)
                {
                    s_error = "Matching was not created (many to many).";
                    return;
                }
            }
        }

        private void Correct_Matching_Query_Table(ref DataTable dt_matching_query)
        {
            List<string> lst_strategy_field = StrategyModel.List_Strategy_Field();

            int i_card_number = lst_strategy_field.FindIndex(x => x == "CardNumber");

            lst_strategy_field.Insert(i_card_number + 1, "CardPrefix");     // SEPARATELY INCLUDE CardPrefix AFTER CardNumber BECAUSE List_Strategy_Field NOT CONTAINS CardPrefix FIELD

            List<string> lst_matching_query_field = new List<string>();

            lst_matching_query_field.Add("ID");
            lst_matching_query_field.Add("MatchingTypeID");
            lst_matching_query_field.Add("QueryNumber");

            foreach (string s_srategy_field in lst_strategy_field)
            {
                lst_matching_query_field.Add(s_srategy_field);

                if (dt_matching_query.Columns.Contains(s_srategy_field) == false)
                {
                    dt_matching_query.Columns.Add(s_srategy_field);
                }
            }

            dt_matching_query = new DataView(dt_matching_query).ToTable(true, lst_matching_query_field.ToArray());
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
            ViewState["TableStrategy"] = o_data_search_tables.TableStrategy;

            //repCompany.DataSource = o_data_search_tables.TableCompany;
            //repCompany.DataBind();

            //lstCredit.DataSource = o_data_search_tables.TableCredit;
            //lstCredit.DataValueField = "ID";
            //lstCredit.DataTextField = "CreditName";
            //lstCredit.DataBind();

            //lstCard.DataSource = o_data_search_tables.TableCard;
            //lstCard.DataValueField = "ID";
            //lstCard.DataTextField = "CardName";
            //lstCard.DataBind();

            ddlStrategy.DataSource = o_data_search_tables.TableStrategy;
            ddlStrategy.DataValueField = "ID";
            ddlStrategy.DataTextField = "StrategyName";
            ddlStrategy.DataBind();
            ddlStrategy.Items.Insert(0, new ListItem("", "0"));

            // Bind Dates

            //for (int i = 1; i <= 31; i++)
            //{
            //    ddlTransactionDayFrom.Items.Add(new ListItem(i.ToString(), i.ToString()));
            //    ddlTransactionDayTo.Items.Add(new ListItem(i.ToString(), i.ToString()));

            //    ddlPaymentDayFrom.Items.Add(new ListItem(i.ToString(), i.ToString()));
            //    ddlPaymentDayTo.Items.Add(new ListItem(i.ToString(), i.ToString()));
            //}

            //for (int i = 1; i <= 12; i++)
            //{
            //    ddlTransactionMonthFrom.Items.Add(new ListItem(i.ToString(), i.ToString()));
            //    ddlTransactionMonthTo.Items.Add(new ListItem(i.ToString(), i.ToString()));

            //    ddlPaymentMonthFrom.Items.Add(new ListItem(i.ToString(), i.ToString()));
            //    ddlPaymentMonthTo.Items.Add(new ListItem(i.ToString(), i.ToString()));
            //}

            //int n_year = DateTime.Now.Year;

            //for (int i = n_year; i >= n_year - 15; i--)
            //{
            //    ddlTransactionYearFrom.Items.Add(new ListItem(i.ToString(), i.ToString()));
            //    ddlTransactionYearTo.Items.Add(new ListItem(i.ToString(), i.ToString()));

            //    ddlPaymentYearFrom.Items.Add(new ListItem(i.ToString(), i.ToString()));
            //    ddlPaymentYearTo.Items.Add(new ListItem(i.ToString(), i.ToString()));
            //}
        }
    }
}
