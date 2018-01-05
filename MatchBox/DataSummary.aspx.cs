using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public partial class DataSummary : BasePage
    {
        private int n_user_id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            n_user_id = o_user.ID;

            if (Page.IsPostBack) { return; }

            Bind_Search();

            if (ddlCompanySearch.SelectedIndex > 0)
            {
                Bind_Table();
            }
            else
            {
                secSearch.Style["display"] = "block";
            }
        }

        protected void repInside_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView o_data_row = (DataRowView)e.Item.DataItem;

                int n_template_id = 0;
                int.TryParse(o_data_row["TemplateID"].ToString(), out n_template_id);

                int n_data_source_id = 0;
                int.TryParse(o_data_row["DataSourceID"].ToString(), out n_data_source_id);

                int n_year = 0;
                int.TryParse(o_data_row["TransactionDateYear"].ToString(), out n_year);

                int n_month = 0;
                int.TryParse(o_data_row["TransactionDateMonth"].ToString(), out n_month);

                string s_query = String.Format(" TemplateID = {0} AND DataSourceID = {1} AND TransactionDateYear = {2} AND TransactionDateMonth = {3} ", n_template_id, n_data_source_id, n_year, n_month);

                DataTable dt_inside_summary_splitted = (DataTable)ViewState["TableInsideSummarySplitted"];

                DataRow dr_inside_summary_splitted = dt_inside_summary_splitted.Select(s_query).FirstOrDefault();

                if (dr_inside_summary_splitted != null)
                {
                    int n_rows_splitted = 0;
                    int.TryParse(dr_inside_summary_splitted["RowsCount"].ToString(), out n_rows_splitted);

                    if (n_rows_splitted > 0)
                    {
                        Label lblRowsCountSplitted = (Label)e.Item.FindControl("lblRowsCountSplitted");
                        lblRowsCountSplitted.Text = String.Format("(+{0})", n_rows_splitted);
                    }
                }
            }
        }

        protected void On_Command(object sender, CommandEventArgs e)
        {
            lblError.Text = "";

            secSearch.Style.Remove("display");

            switch (e.CommandName)
            {
                case "Reset":
                    secSearch.Style.Add("display", "block");
                    Reset();
                    break;
                case "Search":
                    secSearch.Style.Add("display", "block");
                    break;
            }

            Bind_Table();
        }

        private void Reset()
        {
            ddlCompanySearch.SelectedIndex = 0;

            ddlYearFrom.SelectedIndex = 0;
            ddlMonthFrom.SelectedIndex = 0;
            ddlYearTo.SelectedIndex = 0;
            ddlMonthTo.SelectedIndex = 0;
        }

        private void Bind_Search()
        {
            string s_error = "";

            List<SqlParameter> o_company_parameters = new List<SqlParameter>();
            o_company_parameters.Add(new SqlParameter("@UserID", n_user_id));

            DB.Bind_List_Control(ddlCompanySearch, "sprCompany", ref s_error, null, o_company_parameters);

            int n_year = DateTime.Now.Year;

            for (int i = n_year; i >= n_year - 15; i--)
            {
                ddlYearFrom.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlYearTo.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            for (int i = 1; i <= 12; i++)
            {
                ddlMonthFrom.Items.Add(new ListItem(i.ToString(), i.ToString()));
                ddlMonthTo.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }

        private void Bind_Table()
        {
            string s_error = "";

            DataTable dt_inside_summary = new DataTable();
            DataTable dt_inside_summary_splitted = new DataTable();
            DataTable dt_outside_summary = new DataTable();
            DataTable dt_total_summary = new DataTable();

            DataFileAction.Select_Summary(ref dt_inside_summary, ref dt_inside_summary_splitted, ref dt_outside_summary, ref dt_total_summary, ref s_error, Get_Query_Dictionary());

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            ViewState["TableInsideSummarySplitted"] = dt_inside_summary_splitted;

            repInside.DataSource = dt_inside_summary;
            repInside.DataBind();

            repOutside.DataSource = dt_outside_summary;
            repOutside.DataBind();

            DataRow dr_total_summary = dt_total_summary.Rows[0];

            lblInsideRows.Text = String.Format("{0:n0}", Convert.ToInt32(dr_total_summary["InsideRows"]));
            lblInsideAmount.Text = String.Format("{0:n2}", Convert.ToInt32(dr_total_summary["InsideAmount"]));
            lblOutsideRows.Text = String.Format("{0:n0}", Convert.ToInt32(dr_total_summary["OutsideRows"]));
            lblOutsideAmount.Text = String.Format("{0:n2}", Convert.ToInt32(dr_total_summary["OutsideAmount"]));
        }

        private Dictionary<string, string> Get_Query_Dictionary()
        {
            Dictionary<string, string> o_query_dictionary = new Dictionary<string, string>();

            int n_company_id = Convert.ToInt32(ddlCompanySearch.SelectedValue);

            int n_year_from = Convert.ToInt32(ddlYearFrom.SelectedValue);
            int n_month_from = Convert.ToInt32(ddlMonthFrom.SelectedValue);
            int n_year_to = Convert.ToInt32(ddlYearTo.SelectedValue);
            int n_month_to = Convert.ToInt32(ddlMonthTo.SelectedValue);

            if (n_year_from == 0)
            {
                n_month_from = 0;
                ddlMonthFrom.SelectedIndex = 0;
            }
            else if (n_month_from == 0)
            {
                n_month_from = 1;
                ddlMonthFrom.SelectedValue = "1";
            }

            if (n_year_to == 0)
            {
                n_month_to = 0;
                ddlMonthTo.SelectedIndex = 0;
            }
            else if (n_month_to == 0)
            {
                n_month_to = 12;
                ddlMonthTo.SelectedValue = "12";
            }

            o_query_dictionary.Add("UserID", n_user_id.ToString());

            if (n_company_id > 0) { o_query_dictionary.Add("CompanyID", n_company_id.ToString()); }

            if (n_year_from > 0) { o_query_dictionary.Add("YearFrom", n_year_from.ToString()); }
            if (n_month_from > 0) { o_query_dictionary.Add("MonthFrom", n_month_from.ToString()); }
            if (n_year_to > 0) { o_query_dictionary.Add("YearTo", n_year_to.ToString()); }
            if (n_month_to > 0) { o_query_dictionary.Add("MonthTo", n_month_to.ToString()); }

            if (o_query_dictionary.Count == 0) { o_query_dictionary = null; }

            return o_query_dictionary;
        }
    }
}
