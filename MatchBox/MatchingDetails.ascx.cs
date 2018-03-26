using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public partial class MatchingDetails : UserControl
    {
        public int MatchingID { get; set; }

        private int n_user_id = 0;

        private string s_cache_inside_matching = "";
        private string s_cache_outside_matching = "";
        private string s_cache_data_field = "";

        private const int PAGE_SIZE = 50;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (MatchingID <= 0) { return; }

            n_user_id = ((UserModel)Session["User"]).ID;

            s_cache_inside_matching = String.Format("TableInside_Matching_{0}_{1}", n_user_id, MatchingID);
            s_cache_outside_matching = String.Format("TableOutside_Matching_{0}_{1}", n_user_id, MatchingID);
            s_cache_data_field = String.Format("TableDataField_{0}", n_user_id);

            if (Page.IsPostBack) { return; }

            Bind_Form();
        }

        protected void gvMatchingInside_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                DataTable dt_data_field = (DataTable)Cache[s_cache_data_field];

                bool b_checked = (hidSelectInside.Value == "" && hidSelectOutside.Value == "");
                bool b_disabled = (hidSelectInside.Value != "" || hidSelectOutside.Value != "");

                DataAction.Bind_Grid_Data_Header(e.Row, dt_data_field, "Inside", b_checked, b_disabled);
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                List<string> lst_inside_field_priority = (List<string>)ViewState["ListInsideFieldPriority"];

                string s_query_id_href = String.Format("FrameForm.aspx?p=details&pn_in={0}&pn_out={1}&i={2}&q={3}", ddlMatchingInsidePage.SelectedValue, ddlMatchingOutsidePage.SelectedValue, Request.QueryString.Get("i"), Request.QueryString.Get("q"));

                DataAction.Bind_Grid_Data_Row(e.Row, lst_inside_field_priority, hidSelectInside.Value, hidSelectOutside.Value, "Inside", "matching", s_query_id_href);
            }
        }

        protected void gvMatchingOutside_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                DataTable dt_data_field = (DataTable)Cache[s_cache_data_field];

                bool b_checked = (hidSelectInside.Value == "" && hidSelectOutside.Value == "");
                bool b_disabled = (hidSelectInside.Value != "" || hidSelectOutside.Value != "");

                DataAction.Bind_Grid_Data_Header(e.Row, dt_data_field, "Outside", b_checked, b_disabled);
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                List<string> lst_outside_field_priority = (List<string>)ViewState["ListOutsideFieldPriority"];

                string s_query_id_href = String.Format("FrameForm.aspx?p=details&pn_in={0}&pn_out={1}&i={2}&q={3}", ddlMatchingInsidePage.SelectedValue, ddlMatchingOutsidePage.SelectedValue, Request.QueryString.Get("i"), Request.QueryString.Get("q"));

                DataAction.Bind_Grid_Data_Row(e.Row, lst_outside_field_priority, hidSelectInside.Value, hidSelectOutside.Value, "Outside", "matching", s_query_id_href);
            }
        }

        protected void Page_Changed(object sender, EventArgs e)
        {
            hidSelectInside.Value = "";
            hidSelectOutside.Value = "";

            DataTable dt_matching_inside = ((DataTable)Cache[s_cache_inside_matching]).Copy();
            DataTable dt_matching_outside = ((DataTable)Cache[s_cache_outside_matching]).Copy();

            Bind_Table(dt_matching_inside, dt_matching_outside, Convert.ToInt32(ddlMatchingInsidePage.SelectedValue), Convert.ToInt32(ddlMatchingOutsidePage.SelectedValue));
        }

        protected void btnRecalculate_Click(object sender, EventArgs e)
        {
            DataTable dt_matching_inside = ((DataTable)Cache[s_cache_inside_matching]).Copy();
            DataTable dt_matching_outside = ((DataTable)Cache[s_cache_outside_matching]).Copy();

            int n_inside_rows = 0, n_outside_rows = 0;
            double n_inside_amount = 0, n_outside_amount = 0;

            string s_error = "";

            //DataAction.Recalculate_Matching(dt_matching_inside, dt_matching_outside, hidSelectInside.Value, hidSelectOutside.Value, ref n_inside_rows, ref n_outside_rows, ref n_inside_amount, ref n_outside_amount, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            lblMatchingInsideRows.Text = String.Format("{0:n0}", n_inside_rows);
            lblMatchingInsideAmount.Text = String.Format("{0:n2}", Math.Round(n_inside_amount, 2));

            lblMatchingOutsideRows.Text = String.Format("{0:n0}", n_outside_rows);
            lblMatchingOutsideAmount.Text = String.Format("{0:n2}", Math.Round(n_outside_amount, 2));

            ddlMatchingInsidePage.Enabled = false;
            ddlMatchingOutsidePage.Enabled = false;

            tdRecalculate.Visible = false;
            tdSaveChanges.Visible = true;

            lnkCancelChanges.HRef = String.Format("FrameForm.aspx?p=details&pn_in={0}&pn_out={1}&i={2}&q={3}", Convert.ToInt32(ddlMatchingInsidePage.SelectedValue), Convert.ToInt32(ddlMatchingOutsidePage.SelectedValue), Request.QueryString.Get("i"), Request.QueryString.Get("q"));

            Bind_Table(dt_matching_inside, dt_matching_outside, Convert.ToInt32(ddlMatchingInsidePage.SelectedValue), Convert.ToInt32(ddlMatchingOutsidePage.SelectedValue));
        }

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            string s_error = "";
            string s_query_id_array = Common.Get_Distinct_Values(hidSelectInside.Value, hidSelectOutside.Value);

            DataAction.Delete_Match(n_user_id, MatchingID, s_query_id_array, ref s_error,"");

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            s_query_id_array = Common.Convert_To_SQL_Array(s_query_id_array, "string");

            DataTable dt_matching_inside = ((DataTable)Cache[s_cache_inside_matching]).Copy();
            DataTable dt_matching_outside = ((DataTable)Cache[s_cache_outside_matching]).Copy();

            Cache.Remove(s_cache_inside_matching);
            Cache.Remove(s_cache_outside_matching);

            DataRow[] dr_matching_inside = dt_matching_inside.Select(" NOT QueryID IN (" + s_query_id_array + ") ");
            DataRow[] dr_matching_outside = dt_matching_outside.Select(" NOT QueryID IN (" + s_query_id_array + ") ");

            if (dr_matching_inside.Length == 0 || dr_matching_outside.Length == 0)
            {
                Response.Redirect(String.Format("FrameForm.aspx?i={0}&q={1}", Request.QueryString.Get("i"), Request.QueryString.Get("q")));     // REDIRECT TO 'Matching Queries'.
                Response.End();
            }

            dt_matching_inside = dr_matching_inside.CopyToDataTable();
            dt_matching_outside = dr_matching_outside.CopyToDataTable();

            Cache.Insert(s_cache_inside_matching, dt_matching_inside, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);
            Cache.Insert(s_cache_outside_matching, dt_matching_outside, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);

            string s_url = String.Format("FrameForm.aspx?p=details&pn_in={0}&pn_out={1}&i={2}&q={3}", Convert.ToInt32(ddlMatchingInsidePage.SelectedValue), Convert.ToInt32(ddlMatchingOutsidePage.SelectedValue), Request.QueryString.Get("i"), Request.QueryString.Get("q"));

            Response.Redirect(s_url);
        }

        private void Bind_Form()
        {
            DataTable dt_matching_inside = ((DataTable)Cache[s_cache_inside_matching]).Copy();
            DataTable dt_matching_outside = ((DataTable)Cache[s_cache_outside_matching]).Copy();

            int n_inside_pages = Common.Get_Pages_Count(dt_matching_inside.Rows.Count, PAGE_SIZE);
            int n_outside_pages = Common.Get_Pages_Count(dt_matching_outside.Rows.Count, PAGE_SIZE);

            for (int i = 1; i <= n_inside_pages; i++)
            {
                ddlMatchingInsidePage.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            for (int i = 1; i <= n_outside_pages; i++)
            {
                ddlMatchingOutsidePage.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            string s_query_id = "";

            if (Request["qid"] != null) { s_query_id = Request["qid"]; }

            int n_page_number_inside = 1, n_page_number_outside = 1;

            int.TryParse(Request.QueryString["pn_in"], out n_page_number_inside);
            int.TryParse(Request.QueryString["pn_out"], out n_page_number_outside);

            if (n_page_number_inside < 1) { n_page_number_inside = 1; }
            if (n_page_number_outside < 1) { n_page_number_outside = 1; }

            double n_inside_amount = dt_matching_inside.AsEnumerable().Sum(dr => dr.Field<double>("DutyPaymentAmount"));
            double n_outside_amount = dt_matching_outside.AsEnumerable().Sum(dr => dr.Field<double>("DutyPaymentAmount"));

            hedTitle.InnerHtml = "Matching #" + MatchingID;

            lnkHome.HRef = String.Format("FrameForm.aspx?i={0}&q={1}", Request.QueryString.Get("i"), Request.QueryString.Get("q"));

            if (s_query_id != "")
            {
                tdRecalculate.Visible = false;
                lnkReturn.HRef = String.Format("FrameForm.aspx?p=details&pn_in={0}&pn_out={1}&i={2}&q={3}", n_page_number_inside, n_page_number_outside, Request.QueryString.Get("i"), Request.QueryString.Get("q"));
            }
            else
            {
                lnkReturn.Style.Add("color", "gray");
            }

            ddlMatchingInsidePage.SelectedValue = n_page_number_inside.ToString();
            ddlMatchingOutsidePage.SelectedValue = n_page_number_outside.ToString();

            lblMatchingInsideRows.Text = String.Format("{0:n0}", dt_matching_inside.Rows.Count);
            lblMatchingInsideAmount.Text = String.Format("{0:n2}", Math.Round(n_inside_amount, 2));

            lblMatchingOutsideRows.Text = String.Format("{0:n0}", dt_matching_outside.Rows.Count);
            lblMatchingOutsideAmount.Text = String.Format("{0:n2}", Math.Round(n_outside_amount, 2));

            divMatchingInsideControl.Visible = (s_query_id == "");
            divMatchingOutsideControl.Visible = (s_query_id == "");

            divMatchingInsideMessage.Visible = (s_query_id != "");
            divMatchingOutsideMessage.Visible = (s_query_id != "");

            if (s_query_id != "")
            {
                DataRow[] dr_matching_inside = dt_matching_inside.Select(" QueryID = '" + s_query_id + "' ");
                DataRow[] dr_matching_outside = dt_matching_outside.Select(" QueryID = '" + s_query_id + "' ");

                if (dr_matching_inside.Length > 0)
                {
                    dt_matching_inside = dr_matching_inside.CopyToDataTable();
                }
                else
                {
                    dt_matching_inside.Rows.Clear();
                }

                if (dr_matching_outside.Length > 0)
                {
                    dt_matching_outside = dr_matching_outside.CopyToDataTable();
                }
                else
                {
                    dt_matching_outside.Rows.Clear();
                }

                string s_message = "Click 'Return' to view all rows.";

                divMatchingInsideMessage.InnerHtml = s_message;
                divMatchingOutsideMessage.InnerHtml = s_message;
            }
            else
            {
                divMatchingInsideMessage.InnerHtml = "";
                divMatchingOutsideMessage.InnerHtml = "";
            }

            Bind_Table(dt_matching_inside, dt_matching_outside, n_page_number_inside, n_page_number_outside);
        }

        private void Bind_Table(DataTable dt_matching_inside, DataTable dt_matching_outside, int n_page_number_inside, int n_page_number_outside)
        {
            // KEEP FIELD PRIORITY INTO LIST

            if (ViewState["ListInsideFieldPriority"] == null)
            {
                List<string> lst_inside_field_priority = new List<string>();

                for (int i = 0; i < dt_matching_inside.Columns.Count; i++)
                {
                    lst_inside_field_priority.Add(dt_matching_inside.Columns[i].ColumnName);
                }

                ViewState["ListInsideFieldPriority"] = lst_inside_field_priority;
            }

            if (ViewState["ListOutsideFieldPriority"] == null)
            {
                List<string> lst_outside_field_priority = new List<string>();

                for (int i = 0; i < dt_matching_outside.Columns.Count; i++)
                {
                    lst_outside_field_priority.Add(dt_matching_outside.Columns[i].ColumnName);
                }

                ViewState["ListOutsideFieldPriority"] = lst_outside_field_priority;
            }

            // PAGE SIZE

            if (dt_matching_inside.Rows.Count > PAGE_SIZE)
            {
                int n_inside_pages = Common.Get_Pages_Count(dt_matching_inside.Rows.Count, PAGE_SIZE);

                if (n_page_number_inside > n_inside_pages)
                {
                    n_page_number_inside = n_inside_pages;

                    ddlMatchingInsidePage.SelectedValue = n_page_number_inside.ToString();
                }

                dt_matching_inside = dt_matching_inside.AsEnumerable().Skip(PAGE_SIZE * (n_page_number_inside - 1)).Take(PAGE_SIZE).CopyToDataTable();
            }

            if (dt_matching_outside.Rows.Count > PAGE_SIZE)
            {
                int n_outside_pages = Common.Get_Pages_Count(dt_matching_outside.Rows.Count, PAGE_SIZE);

                if (n_page_number_outside > n_outside_pages)
                {
                    n_page_number_outside = n_outside_pages;

                    ddlMatchingOutsidePage.SelectedValue = n_page_number_outside.ToString();
                }

                dt_matching_outside = dt_matching_outside.AsEnumerable().Skip(PAGE_SIZE * (n_page_number_outside - 1)).Take(PAGE_SIZE).CopyToDataTable();
            }

            // BIND REPEATERS

            gvMatchingInside.DataSource = dt_matching_inside;
            gvMatchingInside.DataBind();

            gvMatchingOutside.DataSource = dt_matching_outside;
            gvMatchingOutside.DataBind();
        }
    }
}
