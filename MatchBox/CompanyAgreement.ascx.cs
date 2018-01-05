using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace MatchBox
{
    public partial class CompanyAgreement : UserControl
    {
        public int CompanyID { get; set; }

        private int n_user_id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CompanyID <= 0) { return; }

            n_user_id = ((UserModel)Session["User"]).ID;

            CompanyModel o_company = new CompanyModel();

            o_company.ID = CompanyID;
            o_company.UserID = n_user_id;

            CompanyAction.Select(ref o_company);

            if (o_company.ErrorMessage != "")
            {
                lblError.Text = o_company.ErrorMessage;
                return;
            }

            lblCompanyNumber.Text = o_company.CompanyNumber.ToString();
            lblCompanyName.Text = o_company.CompanyName;

            string s_error = "";

            DataTable dt_cashbox_period = new DataTable();  // Not in use here. Used in AgreementItem Bind_Form.
            DataTable dt_agreement_period = new DataTable();
            DataTable dt_agreement_payment_card = new DataTable();
            DataTable dt_agreement_item = new DataTable();
            DataTable dt_agreement_item_terminal = new DataTable();

            AgreementItemAction.Select(ref dt_cashbox_period, ref dt_agreement_period, ref dt_agreement_payment_card, ref dt_agreement_item, ref dt_agreement_item_terminal, ref s_error, n_user_id, CompanyID);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            ViewState["TableAgreementPeriod"] = dt_agreement_period;
            ViewState["TableAgreementPaymentCard"] = dt_agreement_payment_card;
            ViewState["TableAgreementItem"] = dt_agreement_item;
            ViewState["TableAgreementItemTerminal"] = dt_agreement_item_terminal;

            DataTable dt_agreement = new DataView(dt_agreement_period).ToTable(true, new string[] { "CreditID", "CreditName" });

            repAgreement.DataSource = dt_agreement;
            repAgreement.DataBind();
        }

        protected void repAgreement_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView o_data_row = (DataRowView)e.Item.DataItem;

                int n_credit_id = 0;
                int.TryParse(o_data_row["CreditID"].ToString(), out n_credit_id);

                DataTable dt_agreement_period = (DataTable)ViewState["TableAgreementPeriod"];

                DataRow[] dr_agreement_period = dt_agreement_period.Select("CreditID = " + n_credit_id);

                if (dr_agreement_period.Length > 0)
                {
                    Repeater repAgreementPeriod = (Repeater)e.Item.FindControl("repAgreementPeriod");

                    repAgreementPeriod.DataSource = dr_agreement_period.CopyToDataTable();
                    repAgreementPeriod.DataBind();
                }
            }
        }

        protected void repAgreementPeriod_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView o_data_row = (DataRowView)e.Item.DataItem;

                int n_agreement_period_id = 0;
                int.TryParse(o_data_row["ID"].ToString(), out n_agreement_period_id);

                DataTable dt_agreement_item = (DataTable)ViewState["TableAgreementItem"];

                DataRow[] dr_agreement_item = dt_agreement_item.Select("AgreementPeriodID = " + n_agreement_period_id);

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

                DataRow[] dr_agreement_item_terminal = dt_agreement_item_terminal.Select("AgreementItemID = " + n_agreement_item_id);

                if (dr_agreement_item_terminal.Length > 0)
                {
                    Repeater repAgreementItemTerminal = (Repeater)e.Item.FindControl("repAgreementItemTerminal");

                    repAgreementItemTerminal.DataSource = dr_agreement_item_terminal.CopyToDataTable();
                    repAgreementItemTerminal.DataBind();
                }
            }
        }
    }
}
