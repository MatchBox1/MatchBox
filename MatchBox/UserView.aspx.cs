using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;

namespace MatchBox
{
    public partial class UserView : BasePageAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["result"] != null && Request.QueryString.Get("result") == "inserted") { lblMessage.Text = "Item successfully inserted."; }

            Session["UserLookupTable"] = new Dictionary<string, int>();

            string s_guid = Guid.NewGuid().ToString();
            string s_container_id = "secForm";
            string s_frame_id = fraForm.ClientID;
            string s_frame_url = "FrameForm.aspx?i=" + UserModel.InterfaceName + "&q=" + s_guid;

            ((Dictionary<string, int>)Session["UserLookupTable"]).Add(s_guid, 0);

            lnkAddNew.Attributes.Add("onclick", String.Format("javascript: display_frame_form('{0}', '{1}', '{2}');", s_container_id, s_frame_id, s_frame_url));

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

                HtmlAnchor o_lnk_mail = (HtmlAnchor)e.Item.FindControl("lnkMail");
                HtmlAnchor o_lnk_open = (HtmlAnchor)e.Item.FindControl("lnkOpen");

                string s_mail = o_data_row["Mail"].ToString();

                if (s_mail != "")
                {
                    o_lnk_mail.HRef = "mailto: " + s_mail;
                    o_lnk_mail.Visible = true;
                }

                int n_id = 0;
                int.TryParse(o_data_row["ID"].ToString(), out n_id);

                string s_guid = Guid.NewGuid().ToString();
                string s_container_id = ((HtmlTableRow)e.Item.FindControl("trForm")).ClientID;
                string s_frame_id = ((HtmlIframe)e.Item.FindControl("fraForm")).ClientID;
                string s_frame_url = "FrameForm.aspx?i=" + UserModel.InterfaceName + "&q=" + s_guid;

                ((Dictionary<string, int>)Session["UserLookupTable"]).Add(s_guid, n_id);

                o_lnk_open.Attributes["onclick"] += String.Format("display_frame_form('{0}', '{1}', '{2}');", s_container_id, s_frame_id, s_frame_url);
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

            UserAction.Select(ref o_data_table, ref n_pages_count, ref s_error, o_query_dictionary);

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

                o_excel.Caption = UserModel.InterfaceName + " [" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "]";
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

            ddlStatusSearch.SelectedIndex = 0;
            ddlCitySearch.SelectedIndex = 0;

            txtFullNameSearch.Text = "";
            txtUserNameSearch.Text = "";
            txtPasswordSearch.Text = "";
            txtPhoneSearch.Text = "";
            txtMailSearch.Text = "";
            txtAddressSearch.Text = "";
        }

        private void Bind_Search()
        {
            string s_error = "";

            DB.Bind_List_Control(ddlStatusSearch, "sprStatus", ref s_error);
            DB.Bind_List_Control(ddlCitySearch, "sprCity", ref s_error);
        }

        private void Bind_Table()
        {
            DataTable o_data_table = new DataTable();

            string s_error = "";
            int n_pages_count = 0;

            UserAction.Select(ref o_data_table, ref n_pages_count, ref s_error, Get_Query_Dictionary());

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

            int n_status_id = Convert.ToInt32(ddlStatusSearch.SelectedValue);
            int n_city_id = Convert.ToInt32(ddlCitySearch.SelectedValue);

            string s_full_name = txtFullNameSearch.Text.Trim();
            string s_user_name = txtUserNameSearch.Text.Trim();
            string s_password = txtPasswordSearch.Text.Trim();
            string s_phone = txtPhoneSearch.Text.Trim();
            string s_mail = txtMailSearch.Text.Trim();
            string s_address = txtAddressSearch.Text.Trim();

            string s_order_by = hidOrderBy.Value;
            string s_page_size = ddlPageSize.SelectedValue;
            string s_page = txtCurrentPage.Text.Trim();

            if (n_status_id > 0) { o_query_dictionary.Add("StatusID", n_status_id.ToString()); }
            if (n_city_id > 0) { o_query_dictionary.Add("CityID", n_city_id.ToString()); }

            if (s_full_name != "") { o_query_dictionary.Add("FullName", s_full_name); }
            if (s_user_name != "") { o_query_dictionary.Add("UserName", s_user_name); }
            if (s_password != "") { o_query_dictionary.Add("Password", s_password); }
            if (s_phone != "") { o_query_dictionary.Add("Phone", s_phone); }
            if (s_mail != "") { o_query_dictionary.Add("Mail", s_mail); }
            if (s_address != "") { o_query_dictionary.Add("Address", s_address); }

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

            DB.Delete(n_id, "sprUserDelete", ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            lblMessage.Text = "Item successfully deleted.";
        }
    }
}
