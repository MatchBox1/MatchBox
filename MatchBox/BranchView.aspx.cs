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
    public partial class BranchView : BasePage
    {
        public string s_lnk_open_id = "";

        private int n_user_id = 0;

        private bool b_inserted = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            n_user_id = o_user.ID;

            if (ScriptManager.GetCurrent(Page).IsInAsyncPostBack) { return; }

            if (Request.QueryString["result"] != null)
            {
                string s_message = "";
                string s_result = Request.QueryString.Get("result").Trim();

                if (s_result.Contains("inserted") == true)
                {
                    string[] arr_result = s_result.Split('-');

                    if (arr_result.Length == 2)     // If Update Branch Table
                    {
                        int n_rows_affected = 0;
                        int.TryParse(arr_result[1], out n_rows_affected);

                        if (n_rows_affected > 0)
                        {
                            s_message = String.Format("{0} item/s successfully inserted.", n_rows_affected);
                        }
                        else
                        {
                            s_message = "No item was inserted.";
                        }
                    }
                    else
                    {
                        b_inserted = true;
                        s_message = "Item successfully inserted.";
                    }
                }

                lblMessage.Text = s_message;
            }

            Session["BranchLookupTable"] = new Dictionary<string, int>();

            string s_guid = Guid.NewGuid().ToString();
            string s_container_id = "secForm";
            string s_frame_id = fraForm.ClientID;
            string s_frame_url = "FrameForm.aspx?i=" + BranchModel.InterfaceName + "&q=" + s_guid;

            ((Dictionary<string, int>)Session["BranchLookupTable"]).Add(s_guid, 0);

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

        protected void ddlCompanySearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            int n_company_id = Convert.ToInt32(ddlCompanySearch.SelectedValue);

            while (ddlNetworkSearch.Items.Count > 1)
            {
                ddlNetworkSearch.Items.Remove(ddlNetworkSearch.Items[1]);
            }

            string s_error = "";

            if (n_company_id > 0)
            {
                List<SqlParameter> o_network_parameters = new List<SqlParameter>();

                o_network_parameters.Add(new SqlParameter("@UserID", n_user_id));
                o_network_parameters.Add(new SqlParameter("@CompanyID", n_company_id));

                DB.Bind_List_Control(ddlNetworkSearch, "sprNetwork", ref s_error, null, o_network_parameters);
            }
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
                string s_frame_url = "FrameForm.aspx?i=" + BranchModel.InterfaceName + "&q=" + s_guid;

                ((Dictionary<string, int>)Session["BranchLookupTable"]).Add(s_guid, n_id);

                o_lnk_open.Attributes["onclick"] += String.Format("display_frame_form('{0}', '{1}', '{2}');", s_container_id, s_frame_id, s_frame_url);

                if (b_inserted == true && e.Item.ItemIndex == 0) { s_lnk_open_id = o_lnk_open.ClientID; }
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

        protected void btnUploadBranch_Click(object sender, EventArgs e)
        {
            string s_error = "";

            string s_file_name = ExcelAction.Save(fuBranchUpload, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            int n_rows_affected = 0;

            string s_path = ExcelAction.ExcelFolder + "\\" + s_file_name;

            DataTable dt_branch = new DataTable();

            ExcelAction.Bind_Data_Table(s_path, ref dt_branch, ref s_error);

            if (s_error != "") { goto Finish; }

            List<string> lst_column = new List<string>();

            lst_column.Add("CompanyNumber");
            lst_column.Add("NetworkNumber");
            lst_column.Add("BranchNumber");
            lst_column.Add("BranchName");
            lst_column.Add("Phone");
            lst_column.Add("Fax");
            lst_column.Add("Mail");
            lst_column.Add("Address");
            lst_column.Add("CityID");

            if (dt_branch.Columns.Count != lst_column.Count)
            {
                s_error = String.Format("File must contain {0} column/s.", lst_column.Count);
                goto Finish;
            }

            foreach (DataColumn dc_branch in dt_branch.Columns)
            {
                if (lst_column.Contains(dc_branch.ColumnName) == false)
                {
                    s_error = String.Format("File must contain following column/s: {0}.", String.Join(", ", lst_column.ToArray()));
                    goto Finish;
                }
            }

            dt_branch = new DataView(dt_branch).ToTable(true, lst_column.ToArray());

            for (int i = dt_branch.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr_branch = dt_branch.Rows[i];

                long n_company_number = 0;
                long.TryParse(dr_branch["CompanyNumber"].ToString(), out n_company_number);

                string s_network_number = dr_branch["NetworkNumber"].ToString().Trim();
                string s_branch_number = dr_branch["BranchNumber"].ToString().Trim();

                if (s_network_number.Length > 15)
                {
                    s_error = String.Format("Maximum length of 'NetworkNumber' is 15 ({0}).", s_network_number);
                    goto Finish;
                }

                if (s_branch_number.Length > 15)
                {
                    s_error = String.Format("Maximum length of 'BranchNumber' is 15 ({0}).", s_branch_number);
                    goto Finish;
                }

                string s_query = String.Format(" CompanyNumber = {0} AND NetworkNumber = '{1}' AND BranchNumber = '{2}' ", n_company_number, s_network_number, s_branch_number);

                if (n_company_number == 0 || s_network_number == "" || s_branch_number == "" || dt_branch.Select(s_query).Length > 1)
                {
                    dt_branch.Rows.RemoveAt(i);
                }
                else
                {
                    string s_branch_name = dr_branch["BranchName"].ToString().Trim();
                    string s_phone = dr_branch["Phone"].ToString().Trim();
                    string s_fax = dr_branch["Fax"].ToString().Trim();
                    string s_mail = dr_branch["Mail"].ToString().Trim();
                    string s_address = dr_branch["Address"].ToString().Trim();

                    int n_city_id = 0;
                    int.TryParse(dr_branch["CityID"].ToString(), out n_city_id);

                    if (s_branch_name.Length > 50) { s_branch_name = s_branch_name.Substring(0, 50); }
                    if (s_phone.Length > 15) { s_phone = s_phone.Substring(0, 15); }
                    if (s_fax.Length > 15) { s_fax = s_fax.Substring(0, 15); }
                    if (s_mail.Length > 50) { s_mail = s_mail.Substring(0, 50); }
                    if (s_address.Length > 50) { s_address = s_address.Substring(0, 50); }

                    dr_branch["CompanyNumber"] = n_company_number;
                    dr_branch["NetworkNumber"] = s_network_number;
                    dr_branch["BranchNumber"] = s_branch_number;
                    dr_branch["BranchName"] = s_branch_name;
                    dr_branch["Phone"] = s_phone;
                    dr_branch["Fax"] = s_fax;
                    dr_branch["Mail"] = s_mail;
                    dr_branch["Address"] = s_address;
                    dr_branch["CityID"] = n_city_id;
                }
            }

            BranchAction.Update_Table(n_user_id, dt_branch, ref n_rows_affected, ref s_error);

            if (s_error != "") { goto Finish; }

            if (n_rows_affected < 0) { n_rows_affected = 0; }

            Finish:

            ExcelAction.Remove(s_path, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
            }
            else
            {
                Response.Redirect(String.Format("BranchView.aspx?result=inserted-{0}", n_rows_affected));
            }
        }

        protected void btnDownloadExcel_Click(object sender, EventArgs e)
        {
            DataTable o_data_table = new DataTable();
            Dictionary<string, string> o_query_dictionary = Get_Query_Dictionary();

            o_query_dictionary.Add("NoPaging", true.ToString());

            string s_error = "";
            int n_pages_count = 0;

            BranchAction.Select(ref o_data_table, ref n_pages_count, ref s_error, o_query_dictionary);

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

                o_excel.Caption = BranchModel.InterfaceName + " [" + DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "]";
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

            ddlCompanySearch.SelectedIndex = 0;
            ddlNetworkSearch.SelectedIndex = 0;
            ddlCitySearch.SelectedIndex = 0;

            txtBranchNameSearch.Text = "";
            txtPhoneSearch.Text = "";
            txtMailSearch.Text = "";
            txtAddressSearch.Text = "";
        }

        private void Bind_Search()
        {
            string s_error = "";

            List<SqlParameter> o_company_parameters = new List<SqlParameter>();
            o_company_parameters.Add(new SqlParameter("@UserID", n_user_id));

            DB.Bind_List_Control(ddlCompanySearch, "sprCompany", ref s_error, null, o_company_parameters);
            DB.Bind_List_Control(ddlCitySearch, "sprCity", ref s_error);
        }

        private void Bind_Table()
        {
            DataTable o_data_table = new DataTable();

            string s_error = "";
            int n_pages_count = 0;

            BranchAction.Select(ref o_data_table, ref n_pages_count, ref s_error, Get_Query_Dictionary());

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

            int n_company_id = Convert.ToInt32(ddlCompanySearch.SelectedValue);
            int n_network_id = Convert.ToInt32(ddlNetworkSearch.SelectedValue);
            int n_city_id = Convert.ToInt32(ddlCitySearch.SelectedValue);

            string s_branch_name = txtBranchNameSearch.Text.Trim();
            string s_phone = txtPhoneSearch.Text.Trim();
            string s_mail = txtMailSearch.Text.Trim();
            string s_address = txtAddressSearch.Text.Trim();

            string s_order_by = hidOrderBy.Value;
            string s_page_size = ddlPageSize.SelectedValue;
            string s_page = txtCurrentPage.Text.Trim();

            o_query_dictionary.Add("UserID", n_user_id.ToString());

            if (n_company_id > 0) { o_query_dictionary.Add("CompanyID", n_company_id.ToString()); }
            if (n_network_id > 0) { o_query_dictionary.Add("NetworkID", n_network_id.ToString()); }
            if (n_city_id > 0) { o_query_dictionary.Add("CityID", n_city_id.ToString()); }

            if (s_branch_name != "") { o_query_dictionary.Add("BranchName", s_branch_name); }
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

            DB.Delete(n_id, "sprBranchDelete", ref s_error, n_user_id);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            lblMessage.Text = "Item successfully deleted.";
        }
    }
}
