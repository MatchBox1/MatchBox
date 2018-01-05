using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;

namespace MatchBox
{
    public partial class MatchingView : BasePage
    {
        public string s_lnk_open_id = "";

        private int n_user_id = 0;

        private bool b_inserted = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            n_user_id = o_user.ID;

            if (Request.QueryString["result"] != null && Request.QueryString.Get("result") == "inserted")
            {
                b_inserted = true;
                lblMessage.Text = "Item successfully inserted.";
            }

            Session["MatchingLookupTable"] = new Dictionary<string, int>();

            string s_guid = Guid.NewGuid().ToString();
            string s_container_id = "secForm";
            string s_frame_id = fraForm.ClientID;
            string s_frame_url = "FrameForm.aspx?i=" + MatchingModel.InterfaceName + "&q=" + s_guid;

            ((Dictionary<string, int>)Session["MatchingLookupTable"]).Add(s_guid, 0);

            if (Page.IsPostBack) { return; }

            if (Request.Cookies["PageSize"] != null) { ddlPageSize.SelectedValue = Request.Cookies["PageSize"].Value; }

            Bind_Search();

            Bind_Table();
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            divMessage.Visible = lblMessage.Text != "" || lblError.Text != "";

            if (lblError.Text != "") { lblMessage.Text = ""; }
        }

        protected void repTable_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView o_data_row = (DataRowView)e.Item.DataItem;

                int n_id = 0;
                int.TryParse(o_data_row["ID"].ToString(), out n_id);

                int n_matching_action_id = 0;
                int.TryParse(o_data_row["MatchingActionID"].ToString(), out n_matching_action_id);

                // btnDelete

                LinkButton btn_delete = (LinkButton)e.Item.FindControl("btnDelete");

                if (n_matching_action_id == 3)
                {
                    // CAN'T DELETE MATCHING WITH 'RESPLIT' ACTION. THIS MATCHING WILL DELETED ON 'PAYMENT GROUP' RESTORATION. ( MatchingActionID # 3 : Resplit )
                    btn_delete.CommandArgument = "0";
                    btn_delete.Visible = false;
                }

                // lnkOpen

                HtmlAnchor lnk_open = (HtmlAnchor)e.Item.FindControl("lnkOpen");

                string s_guid = Guid.NewGuid().ToString();
                string s_container_id = ((HtmlTableRow)e.Item.FindControl("trForm")).ClientID;
                string s_frame_id = ((HtmlIframe)e.Item.FindControl("fraForm")).ClientID;
                string s_frame_url = "FrameForm.aspx?i=" + MatchingModel.InterfaceName + "&q=" + s_guid;

                ((Dictionary<string, int>)Session["MatchingLookupTable"]).Add(s_guid, n_id);

                lnk_open.Attributes["onclick"] += String.Format("display_frame_form('{0}', '{1}', '{2}');", s_container_id, s_frame_id, s_frame_url);

                if (b_inserted == true && e.Item.ItemIndex == 0) { s_lnk_open_id = lnk_open.ClientID; }
            }
        }

        protected void txtCurrentPage_TextChanged(object sender, EventArgs e)
        {
            On_Command(null, new CommandEventArgs("GoToPage", null));
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Response.Cookies["PageSize"].Value = ddlPageSize.SelectedValue;
            Response.Cookies["PageSize"].Expires = DateTime.Now.AddMonths(3);

            On_Command(null, new CommandEventArgs("PageSize", null));
        }

        protected void On_Command(object sender, CommandEventArgs e)
        {
            b_inserted = false;
            s_lnk_open_id = "";

            lblMessage.Text = "";
            lblError.Text = "";

            secSearch.Style.Remove("display");

            switch (e.CommandName)
            {
                case "Reload":
                    break;
                case "Reset":
                    secSearch.Style.Add("display", "block");
                    Reset();
                    break;
                case "Search":
                    hidOrderBy.Value = "";
                    txtCurrentPage.Text = "1";
                    secSearch.Style.Add("display", "block");
                    break;
                case "Sort":
                    hidOrderBy.Value = e.CommandArgument.ToString();
                    txtCurrentPage.Text = "1";
                    break;
                case "Previous":
                    Change_Page(-1);
                    break;
                case "Next":
                    Change_Page(1);
                    break;
                case "GoToPage":
                    Change_Page(0);
                    break;
                case "PageSize":
                    txtCurrentPage.Text = "1";
                    break;
                case "Delete":
                    Delete(e.CommandArgument.ToString());
                    break;
            }

            Bind_Table();

            if (e.CommandName == "Delete" && repTable.DataSource == null) { secSearch.Style.Add("display", "block"); }
        }

        protected void btnDownloadExcel_Click(object sender, EventArgs e)
        {
            DataTable o_data_table = new DataTable();
            Dictionary<string, string> o_query_dictionary = Get_Query_Dictionary();

            o_query_dictionary.Add("NoPaging", true.ToString());

            string s_error = "";
            int n_pages_count = 0;

            MatchingAction.Select(ref o_data_table, ref n_pages_count, ref s_error, o_query_dictionary);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            if (n_pages_count == 0) { return; }

            try
            {
                Excel.Application o_excel = new Excel.Application();
                Excel.Workbook o_workbook = o_excel.Workbooks.Add();
                Excel.Worksheet o_worksheet = o_excel.ActiveSheet;

                for (int i = 0; i < o_data_table.Columns.Count; i++)
                {
                    o_worksheet.Cells[1, (i + 1)] = o_data_table.Columns[i].ColumnName;
                }

                Excel.Range o_range = o_worksheet.get_Range((Excel.Range)o_worksheet.Cells[1, 1], (Excel.Range)o_worksheet.Cells[1, o_data_table.Columns.Count]);

                o_range.Font.Bold = true;

                for (int i = 0; i < o_data_table.Rows.Count; i++)
                {
                    // to do: format datetime values before printing
                    for (int j = 0; j < o_data_table.Columns.Count; j++)
                    {
                        o_worksheet.Cells[(i + 2), (j + 1)] = o_data_table.Rows[i][j];
                    }
                }

                o_excel.Caption = MatchingModel.InterfaceName + " [" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "]";
                o_excel.Visible = true;
            }
            catch (Exception ex)
            {
                s_error = ex.Message;
            }

            if (s_error != "")
            {
                lblError.Text = "Error on download excel.";
                return;
            }
        }

        private void Reset()
        {
            hidOrderBy.Value = "";
            txtCurrentPage.Text = "1";

            ddlStrategySearch.SelectedIndex = 0;
        }

        private void Bind_Search()
        {
            string s_error = "";

            List<SqlParameter> o_strategy_parameters = new List<SqlParameter>();
            o_strategy_parameters.Add(new SqlParameter("@UserID", n_user_id));

            DB.Bind_List_Control(ddlStrategySearch, "sprStrategy", ref s_error, null, o_strategy_parameters);
        }

        private void Bind_Table()
        {
            DataTable o_data_table = new DataTable();

            string s_error = "";
            int n_pages_count = 0;

            MatchingAction.Select(ref o_data_table, ref n_pages_count, ref s_error, Get_Query_Dictionary());

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            pPaging.Visible = (n_pages_count > 0);

            if (o_data_table.Rows.Count > 0)
            {
                repTable.DataSource = o_data_table;

                lblPagesCount.Text = n_pages_count.ToString();

                btnPreviousPage.Enabled = (txtCurrentPage.Text != "1");
                btnNextPage.Enabled = (txtCurrentPage.Text != lblPagesCount.Text);
            }
            else
            {
                repTable.DataSource = null;
                lblMessage.Text = "No records were found.";
            }

            repTable.DataBind();
        }

        private Dictionary<string, string> Get_Query_Dictionary()
        {
            Dictionary<string, string> o_query_dictionary = new Dictionary<string, string>();

            int n_strategy_id = Convert.ToInt32(ddlStrategySearch.SelectedValue);

            string s_order_by = hidOrderBy.Value;
            string s_page_size = ddlPageSize.SelectedValue;
            string s_page = txtCurrentPage.Text.Trim();

            o_query_dictionary.Add("UserID", n_user_id.ToString());

            if (n_strategy_id > 0) { o_query_dictionary.Add("StrategyID", n_strategy_id.ToString()); }

            if (s_order_by != "") { o_query_dictionary.Add("OrderBy", s_order_by); }
            if (s_page_size != "") { o_query_dictionary.Add("PageSize", s_page_size); }
            if (s_page != "") { o_query_dictionary.Add("PageNumber", s_page); }

            if (o_query_dictionary.Count == 0) { o_query_dictionary = null; }

            return o_query_dictionary;
        }

        private void Change_Page(int n_diff)
        {
            int n_page = 0;
            int.TryParse(txtCurrentPage.Text, out n_page);

            int n_pages_count = 0;
            int.TryParse(lblPagesCount.Text, out n_pages_count);

            n_page = n_page + n_diff;

            if (n_page < 1) { n_page = 1; }
            if (n_page > n_pages_count) { n_page = n_pages_count; }

            txtCurrentPage.Text = n_page.ToString();
        }

        private void Delete(string s_argument = "")
        {
            int n_id = 0;
            int.TryParse(s_argument, out n_id);

            if (n_id == 0) { return; }

            string s_error = "";

            DB.Delete(n_id, "sprMatchingDelete", ref s_error, n_user_id);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            lblMessage.Text = "Item successfully deleted.";
        }
    }
}
