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

        //private string s_cache_matching_search = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            n_user_id = o_user.ID;

            //s_cache_matching_search = String.Format("Matching_Search_{0}", n_user_id);

            if (Page.IsPostBack) { return; }

            Bind_Form();
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            //divMessage.Visible = lblMessage.Text != "" || lblError.Text != "";

            //if (lblError.Text != "") { lblMessage.Text = ""; }
        }


        private void Bind_Form()
        {
            string s_error = "";

            List<SqlParameter> o_company_parameters = new List<SqlParameter>();
            o_company_parameters.Add(new SqlParameter("@UserID", n_user_id));

            DB.Bind_List_Control(ddlCompany, "sprCompany", ref s_error, null, o_company_parameters);
            DB.Bind_List_Control(ddlCreditCompany, "sprCredit", ref s_error);
            DB.Bind_List_Control(ddlCommissionType, "sprCommissionTypes", ref s_error);
        }

        protected void btnSaveCommission_Click(object sender, EventArgs e)
        {
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

            CommissionFilter filter = new CommissionFilter();
            filter.Id = 0;
            filter.UserId = n_user_id;
            filter.CompanyId = Convert.ToInt32(ddlCompany.SelectedValue);
            filter.CreditId = Convert.ToInt32(ddlCreditCompany.SelectedValue);
            filter.CommissionTypeId = Convert.ToInt32(ddlCommissionType.SelectedValue);
            filter.PaymentDateFrom = paymentDateFrom;
            filter.PaymentDateTo = paymentDateTo;
            filter.Valid = true;

            int n_rows_affected = DataActionCommission.SaveCommissionData(filter, ref s_error);
            if(s_error!="")
            {
                lblError.Text = s_error;
                return;
            }
            if(n_rows_affected>0)
            {
                lblMessage.Text = "Commission saved Successfully.";
            }

        }

    }
}
