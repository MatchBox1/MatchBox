using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Caching;
using System.Data.SqlClient;
using MatchBox.App_Code;

namespace MatchBox
{
    public partial class CommissionsNew : BasePage
    {

        private int n_user_id = 0;

        private string s_cache_commission_search = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            n_user_id = o_user.ID;

            s_cache_commission_search = String.Format("Commission_Search_{0}", n_user_id);

            if (Page.IsPostBack) { return; }

            Bind_Form();
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            divMessage.Visible = lblMessage.Text != "" || lblError.Text != "";

            if (lblError.Text != "") { lblMessage.Text = ""; }
        }


        private void Bind_Form()
        {
            string s_error = "";

            List<SqlParameter> o_company_parameters = new List<SqlParameter>();
            o_company_parameters.Add(new SqlParameter("@UserID", n_user_id));

            DB.Bind_List_Control(ddlCompany, "sprCompany", ref s_error, null, o_company_parameters);
            //List<SqlParameter> o_company_parameters1 = new List<SqlParameter>();
            //o_company_parameters1.Add(new SqlParameter("@UserID", n_user_id));
            //DB.Bind_List_Control(ddlCreditCompany, "sprCredit_Discount_Name", ref s_error, null, o_company_parameters1);
            //DB.Bind_List_Control(ddlCommissionType, "sprCommissionTypes", ref s_error);
        }

        protected void ddlCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int n_company_id = Convert.ToInt32(ddlCompany.SelectedValue);

            while (ddlCreditCompany.Items.Count > 1)
            {
                ddlCreditCompany.Items.Remove(ddlCreditCompany.Items[1]);
            }
            while (ddlDiscount.Items.Count > 1)
            {
                ddlDiscount.Items.Remove(ddlDiscount.Items[1]);
            }
            while (ddlCommissionType.Items.Count > 1)
            {
                ddlCommissionType.Items.Remove(ddlCommissionType.Items[1]);
            }
            rdoCredit.Checked = false;
            rdoDiscount.Checked = false;

            //string s_error = "";

            //if (n_company_id > 0)
            //{
            //    List<SqlParameter> o_credit_parameters = new List<SqlParameter>();

            //    o_credit_parameters.Add(new SqlParameter("@UserID", n_user_id));
            //    o_credit_parameters.Add(new SqlParameter("@CompanyID", n_company_id));

            //    DB.Bind_List_Control(ddlCreditCompany, "sprCredit_Discount_Name", ref s_error, null, o_credit_parameters);
            //}
        }

        protected void txtPaymentDate_TextChanged(object sender, EventArgs e)
        {
            if (ddlCompany.SelectedValue == "0")
            {
                ddlCompany.Focus();
                return;
            }
            if (ddlCreditCompany.SelectedValue == "0" && ddlDiscount.SelectedValue == "0")
            {
                ddlCreditCompany.Focus();
                return;
            }

            string s_error = string.Empty;

            // Payment Date
            DateTime? paymentDateFrom = null;
            DateTime? paymentDateTo = null;
            Commission.PaymentDate(txtPaymentDate.Text.Trim(), ref paymentDateFrom, ref paymentDateTo, ref s_error);
            if (s_error != "")
            {
                lblError.Text = "Payment Date is not valid.";
                txtPaymentDate.Focus();
                return;
            }
            if (paymentDateFrom != null && paymentDateTo != null && paymentDateFrom > paymentDateTo)
            {
                lblError.Text = "'Payment Date From' can't be greater than 'Payment Date To'.";
                txtPaymentDate.Focus();
                return;
            }
            string type = string.Empty;
            int CreditId = 0;
            if (rdoCredit.Checked)
            {
                type = "rdoCredit";
                CreditId = Convert.ToInt32(ddlCreditCompany.SelectedValue);
            }
            else if (rdoDiscount.Checked)
            {
                type = "rdoDiscount";
                CreditId = Convert.ToInt32(ddlDiscount.SelectedValue);
            }
            else
            {
                return;
            }

            List<SqlParameter> o_credit_parameters = new List<SqlParameter>();
            o_credit_parameters.Add(new SqlParameter("@UserID", n_user_id));
            o_credit_parameters.Add(new SqlParameter("@CompanyID", Convert.ToInt32(ddlCompany.SelectedValue)));
            o_credit_parameters.Add(new SqlParameter("@CreditID", CreditId));
            o_credit_parameters.Add(new SqlParameter("@PaymentDateFrom", paymentDateFrom));
            o_credit_parameters.Add(new SqlParameter("@PaymentDateTo", paymentDateTo));
            o_credit_parameters.Add(new SqlParameter("@Type", type));

            while (ddlCommissionType.Items.Count > 1)
            {
                ddlCommissionType.Items.Remove(ddlCommissionType.Items[1]);
            }
            DataTable dtOverlappingPeriod = new DataTable();
            //DB.Bind_List_Control(ddlCommissionType, "sprCommission_Name", ref s_error, null,o_credit_parameters);
            DataActionCommission.Bind_List_Control_Commission("sprCommission_Name", ref s_error, ref dtOverlappingPeriod, o_credit_parameters);
            if (dtOverlappingPeriod.Rows.Count > 0)
            {
                if (dtOverlappingPeriod.Rows[0][0].ToString() != "period")
                {
                    ddlCommissionType.DataSource = dtOverlappingPeriod;
                    ddlCommissionType.DataBind();
                    ddlCommissionType.DataTextField = "CommissionName";
                    ddlCommissionType.DataValueField = "ID";
                    ddlCommissionType.DataBind();
                }
                else
                {
                    while (ddlCommissionType.Items.Count > 0)
                    {
                        ddlCommissionType.Items.Remove(ddlCommissionType.Items[0]);
                    }
                    lblError.Text = dtOverlappingPeriod.Rows[0][2].ToString();
                }
            }
            //if (ddlCommissionType.Items.Count < 2)
            //{
            //    List<SqlParameter> o_credit_parameters1 = new List<SqlParameter>();
            //    o_credit_parameters1.Add(new SqlParameter("@UserID", n_user_id));
            //    o_credit_parameters1.Add(new SqlParameter("@CompanyID", Convert.ToInt32(ddlCompany.SelectedValue)));
            //    o_credit_parameters1.Add(new SqlParameter("@CreditID", CreditId));
            //    o_credit_parameters1.Add(new SqlParameter("@PaymentDateFrom", paymentDateFrom));
            //    o_credit_parameters1.Add(new SqlParameter("@PaymentDateTo", paymentDateTo));
            //    o_credit_parameters1.Add(new SqlParameter("@Type", type));

            //    //DataTable dtOverlappingPeriod = new DataTable();
            //    DataActionCommission.Bind_List_Control_Commission(ddlCommissionType, "sprCommission_Name", ref s_error, ref dtOverlappingPeriod, o_credit_parameters1);
            //    if(dtOverlappingPeriod.Rows.Count>0)
            //    {
            //        lblError.Text = dtOverlappingPeriod.Rows[0][2].ToString();
            //    }
            //}
        }

        protected void Check_Template_For(object sender, EventArgs e)
        {
            if (ddlCompany.SelectedValue == "0")
            {
                lblError.Text = "Please select Company.";
                ddlCompany.Focus();
                return;
            }
            while (ddlCommissionType.Items.Count > 1)
            {
                ddlCommissionType.Items.Remove(ddlCommissionType.Items[1]);
            }
            txtPaymentDate.Text = string.Empty;

            //repTemplateField.DataSource = null;
            // repTemplateField.DataBind();

            //divTemplateField.Visible = false;

            //if (TemplateID > 0) { divTemplateMessage.InnerHtml = "Template For Changed"; }

            //ddlCreditCompany.Enabled = false;
            //ddlDiscount.Enabled = false;

            string s_radio_button_id = ((RadioButton)sender).ID;

            switch (s_radio_button_id)
            {
                case "rdoCredit":
                    //ddlCreditCompany.Enabled = true;
                    ddlDiscount.SelectedIndex = 0;
                    break;
                case "rdoDiscount":
                    //ddlDiscount.Enabled = true;
                    ddlCreditCompany.SelectedIndex = 0;
                    break;
            }

            int n_company_id = Convert.ToInt32(ddlCompany.SelectedValue);

            while (ddlCreditCompany.Items.Count > 1)
            {
                ddlCreditCompany.Items.Remove(ddlCreditCompany.Items[1]);
            }
            while (ddlDiscount.Items.Count > 1)
            {
                ddlDiscount.Items.Remove(ddlDiscount.Items[1]);
            }

            string s_error = "";

            //if (n_company_id > 0)
            //{
            List<SqlParameter> o_credit_parameters = new List<SqlParameter>();

            o_credit_parameters.Add(new SqlParameter("@UserID", n_user_id));
            o_credit_parameters.Add(new SqlParameter("@CompanyID", n_company_id));
            o_credit_parameters.Add(new SqlParameter("@Type", s_radio_button_id));

            if (s_radio_button_id == "rdoCredit")
            {
                DB.Bind_List_Control(ddlCreditCompany, "sprCredit_Discount_Name", ref s_error, null, o_credit_parameters);
            }
            else
            {
                DB.Bind_List_Control(ddlDiscount, "sprCredit_Discount_Name", ref s_error, null, o_credit_parameters);
            }
            //}

            //string s_error = "";
            //string s_table = s_radio_button_id == "rdoDataSource" ? "tblData_Inside" : "tblData_Outside";

            //DataTable dt_data_field = new DataTable();

            //DB.Bind_Data_Table("sprData_Field", ref dt_data_field, ref s_error, "@TableName", s_table);

            //if (s_error != "")
            //{
            //    lblError.Text = s_error;
            //    return;
            //}

            //ViewState["TableDataField"] = dt_data_field;
        }

        protected void cvTemplateFor_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool b_valid = rdoCredit.Checked || rdoDiscount.Checked;

            if (b_valid == false)
            {
                cvTemplateFor.ErrorMessage = "Select 'Template For'.";
                goto Finish;
            }

            if (rdoCredit.Checked == true)
            {
                int n_credit = 0;
                int.TryParse(ddlCreditCompany.SelectedValue, out n_credit);

                if (n_credit <= 0)
                {
                    b_valid = false;
                    cvTemplateFor.ErrorMessage = "Select 'Credit'.";
                    ddlCreditCompany.Enabled = true;
                    ddlCreditCompany.Focus();
                    goto Finish;
                }
            }

            if (rdoDiscount.Checked == true)
            {
                int n_credit = 0;
                int.TryParse(ddlDiscount.SelectedValue, out n_credit);

                if (n_credit <= 0)
                {
                    b_valid = false;
                    cvTemplateFor.ErrorMessage = "Select 'Discount'.";
                    ddlDiscount.Enabled = true;
                    ddlDiscount.Focus();
                    goto Finish;
                }
            }

            Finish:

            args.IsValid = b_valid;
        }

        protected void btnSearchCommission_Click(object sender, EventArgs e)
        {
            if (ddlCompany.SelectedValue == "0")
            {
                ddlCompany.Focus();
                return;
            }
            if (ddlCreditCompany.SelectedValue == "0" && ddlDiscount.SelectedValue == "0")
            {
                ddlCreditCompany.Focus();
                return;
            }
            if (txtPaymentDate.Text.Trim() == "")
            {
                txtPaymentDate.Focus();
                return;
            }
            if (ddlCommissionType.SelectedValue == "0")
            {
                ddlCommissionType.Focus();
                return;
            }

            string s_error = string.Empty;

            // Payment Date
            DateTime? paymentDateFrom = null;
            DateTime? paymentDateTo = null;
            Commission.PaymentDate(txtPaymentDate.Text.Trim(), ref paymentDateFrom, ref paymentDateTo, ref s_error);
            if (s_error != "")
            {
                lblError.Text = "Payment Date is not valid.";
                txtPaymentDate.Focus();
                return;
            }
            if (paymentDateFrom != null && paymentDateTo != null && paymentDateFrom > paymentDateTo)
            {
                lblError.Text = "'Payment Date From' can't be greater than 'Payment Date To'.";
                txtPaymentDate.Focus();
                return;
            }
           
            //string[] CreditId = ddlCreditCompany.SelectedValue.Split('_');
            string type = string.Empty;
            int CreditId = 0;
            if (rdoCredit.Checked)
            {
                type = "rdoCredit";
                CreditId = Convert.ToInt32(ddlCreditCompany.SelectedValue);
            }
            else if (rdoDiscount.Checked)
            {
                type = "rdoDiscount";
                CreditId = Convert.ToInt32(ddlDiscount.SelectedValue);
            }
            else
            {
                return;
            }

            CommissionFilter filter_search = new CommissionFilter();
            filter_search.Id = 0;
            filter_search.UserId = n_user_id;
            filter_search.CompanyId = Convert.ToInt32(ddlCompany.SelectedValue);
            filter_search.CreditId = CreditId;
            filter_search.CommissionTypeId = Convert.ToInt32(ddlCommissionType.SelectedValue);
            filter_search.PaymentDateFrom = paymentDateFrom;
            filter_search.PaymentDateTo = paymentDateTo;
            //filter.Valid = true;
            filter_search.CreditType = type;

            ViewState["paymentDateFrom"] = paymentDateFrom;
            ViewState["paymentDateTo"] = paymentDateTo;
            ViewState["CommissionTypeId"] = Convert.ToInt32(ddlCommissionType.SelectedValue);
            ViewState["CreditId"] = CreditId;
            ViewState["CompanyId"] = Convert.ToInt32(ddlCompany.SelectedValue);


            //////////////
            //ViewState["DateFilterData"] = filter;
            //////////////

            DataActionCommission.SearchCommissionData(ref filter_search, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }
            //if(n_rows_affected>0)
            //{
            //    lblMessage.Text = "Commission saved Successfully.";
            //}

            if (filter_search.TableOutsideSum.Rows.Count == 0)
            {
                pnlAutoCommission.Visible = false;
                lblError.Text = "Can't create commissions without data in outside.";
                return;
            }

            // USE Cache CLASS TO STORE A LARGE DATA - o_matching_search

            Cache.Insert(s_cache_commission_search, filter_search, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);

            // SHOW RESULT

            pnlSearchResult.Visible = true;

            //repInsideSum.DataSource = o_matching_search.TableInsideSum;
            //repInsideSum.DataBind();

            repOutsideSum.DataSource = filter_search.TableOutsideSum;
            repOutsideSum.DataBind();

            pnlAutoCommission.Visible = true;


            //Processed
            DataActionCommission.SearchProcessedCommissionData(ref filter_search, ref s_error);
            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            //if (filter_search.TableOutsideProcessedSum.Rows.Count == 0)
            //{
            //    pnlAutoCommission.Visible = false;
            //    lblError.Text = "Can't create commissions without data in outside.";
            //    return;
            //}
            Cache.Insert(s_cache_commission_search, filter_search, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);

            // SHOW RESULT

            pnlSearchResult.Visible = true;

            //repInsideSum.DataSource = o_matching_search.TableInsideSum;
            //repInsideSum.DataBind();

            repOutsideProcessed.DataSource = filter_search.TableOutsideProcessedSum;
            repOutsideProcessed.DataBind();

            pnlAutoCommission.Visible = true;
            //Processed end


            //UnProcessed
            DataActionCommission.SearchUnProcessedCommissionData(ref filter_search, ref s_error);
            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            //if (filter_search.TableOutsideUnProcessedSum.Rows.Count == 0)
            //{
            //    pnlAutoCommission.Visible = false;
            //    lblError.Text = "Can't create commissions without data in outside.";
            //    return;
            //}
            Cache.Insert(s_cache_commission_search, filter_search, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);

            // SHOW RESULT

            pnlSearchResult.Visible = true;

            //repInsideSum.DataSource = o_matching_search.TableInsideSum;
            //repInsideSum.DataBind();

            repOutsideUnProcessed.DataSource = filter_search.TableOutsideUnProcessedSum;
            repOutsideUnProcessed.DataBind();

            pnlAutoCommission.Visible = true;
            //UnProcessed End

        }

        protected void repOutsideProcessed_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                CommissionFilter o_commission_search = (CommissionFilter)Cache[s_cache_commission_search];

                if (o_commission_search.TableOutsideProcessedSumTotal.Rows.Count > 0)
                {
                    DataRow dr_outside_processed_sum_total = o_commission_search.TableOutsideProcessedSumTotal.Rows[0];

                    int n_transaction_processed_count_total = 0;
                    int.TryParse(dr_outside_processed_sum_total["TransactionCountTotal"].ToString(), out n_transaction_processed_count_total);

                    double n_amount_processed_sum_total = 0;
                    double.TryParse(dr_outside_processed_sum_total["AmountSumTotal"].ToString(), out n_amount_processed_sum_total);

                    Label lblProcessedCountTotal = (Label)e.Item.FindControl("lblProcessedCountTotal");
                    Label lblProcessedSumTotal = (Label)e.Item.FindControl("lblProcessedSumTotal");

                    lblProcessedCountTotal.Text = String.Format("{0:n0}", n_transaction_processed_count_total);
                    lblProcessedSumTotal.Text = String.Format("{0:n2}", n_amount_processed_sum_total);
                }
            }
        }

        protected void repOutsideSum_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                CommissionFilter o_commission_search = (CommissionFilter)Cache[s_cache_commission_search];

                if (o_commission_search.TableOutsideSumTotal.Rows.Count > 0)
                {
                    DataRow dr_outside_sum_total = o_commission_search.TableOutsideSumTotal.Rows[0];

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

        protected void repOutsideUnProcessed_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                CommissionFilter o_commission_search = (CommissionFilter)Cache[s_cache_commission_search];

                if (o_commission_search.TableOutsideUnProcessedSumTotal.Rows.Count > 0)
                {
                    DataRow dr_outside_unprocessed_sum_total = o_commission_search.TableOutsideUnProcessedSumTotal.Rows[0];

                    int n_transaction_unprocessed_count_total = 0;
                    int.TryParse(dr_outside_unprocessed_sum_total["TransactionCountTotal"].ToString(), out n_transaction_unprocessed_count_total);

                    double n_amount_unprocessed_sum_total = 0;
                    double.TryParse(dr_outside_unprocessed_sum_total["AmountSumTotal"].ToString(), out n_amount_unprocessed_sum_total);

                    Label lbUnProcessedCountTotal = (Label)e.Item.FindControl("lbUnProcessedCountTotal");
                    Label lblUnProcessedSumTotal = (Label)e.Item.FindControl("lblUnProcessedSumTotal");

                    lbUnProcessedCountTotal.Text = String.Format("{0:n0}", n_transaction_unprocessed_count_total);
                    lblUnProcessedSumTotal.Text = String.Format("{0:n2}", n_amount_unprocessed_sum_total);
                }
            }
        }

        protected void btnAutoCommissionProcess_Click(object sender, EventArgs e)
        {
            btnAutoCommission_Click(sender, e);
        }

        protected void btnAutoCommission_Click(object sender, EventArgs e)
        {
            //lblTransactionCountTotal
            //lblAmountSumTotal
            string input = ((Button)(sender)).CommandArgument;
            CommissionFilter o_commission_search = (CommissionFilter)Cache[s_cache_commission_search];

            if (o_commission_search.TableOutsideSumTotal.Rows.Count > 0)
            {
                DataRow dr_outside_sum_total = o_commission_search.TableOutsideSumTotal.Rows[0];
                int n_transaction_count_total = 0;
                int.TryParse(dr_outside_sum_total["TransactionCountTotal"].ToString(), out n_transaction_count_total);

                double n_amount_sum_total = 0;
                double.TryParse(dr_outside_sum_total["AmountSumTotal"].ToString(), out n_amount_sum_total);

                o_commission_search.AllOutsideCount = n_transaction_count_total;
                o_commission_search.AllOutsideAmount = n_amount_sum_total;

                string s_error = string.Empty;
                DataActionCommission.SaveCommissionData(o_commission_search, ref s_error);

                if (s_error != "")
                {
                    lblError.Text = s_error;
                    return;
                }
            }
            if (o_commission_search.TableOutsideSumTotal.Rows.Count > 0)
            {
                string s_error = string.Empty;
                CommissionFilter filter_search = new CommissionFilter();
                filter_search.Id = 0;
                filter_search.UserId = n_user_id;
                filter_search.CompanyId = Convert.ToInt32(ViewState["CompanyId"]);
                filter_search.CreditId = Convert.ToInt32(ViewState["CreditId"]);
                filter_search.CommissionTypeId = Convert.ToInt32(ViewState["CommissionTypeId"]);
                filter_search.PaymentDateFrom = Convert.ToDateTime(ViewState["paymentDateFrom"]);
                filter_search.PaymentDateTo = Convert.ToDateTime(ViewState["paymentDateTo"]);
                //filter.Valid = true;
                filter_search.CreditType = "";
                filter_search.Reprocess = input;
                DataActionCommission.FetchCommissionData(ref filter_search, ref s_error);
                if (s_error != "")
                {
                    lblError.Text = s_error;
                    return;
                }
                else
                lblMessage.Text = "Successsfully calculated.";
            }
        }

        protected void btnAutoCommissionDelete_Click(object sender, EventArgs e)
        {
            CommissionFilter o_commission_search = (CommissionFilter)Cache[s_cache_commission_search];
            if (o_commission_search.TableOutsideSumTotal.Rows.Count > 0)
            {
                string s_error = string.Empty;
                CommissionFilter filter_search = new CommissionFilter();
                filter_search.Id = 0;
                filter_search.UserId = n_user_id;
                filter_search.CompanyId = Convert.ToInt32(ViewState["CompanyId"]);
                filter_search.CreditId = Convert.ToInt32(ViewState["CreditId"]);
                filter_search.CommissionTypeId = Convert.ToInt32(ViewState["CommissionTypeId"]);
                filter_search.PaymentDateFrom = Convert.ToDateTime(ViewState["paymentDateFrom"]);
                filter_search.PaymentDateTo = Convert.ToDateTime(ViewState["paymentDateTo"]);
                //filter.Valid = true;
                filter_search.CreditType = "";
                DataActionCommission.DeleteCommissionData(ref filter_search, ref s_error);
                if (s_error != "")
                {
                    lblError.Text = s_error;
                    return;
                }
                else
                    lblMessage.Text = "Successsfully Deleted.";
            }
        }
    }
}
