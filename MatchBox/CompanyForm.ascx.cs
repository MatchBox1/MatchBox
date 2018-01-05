using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public partial class CompanyForm : UserControl
    {
        public int CompanyID { get; set; }

        private int n_user_id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            n_user_id = ((UserModel)Session["User"]).ID;

            if (Page.IsPostBack) { return; }

            if (Request.QueryString["result"] != null)
            {
                string s_message = "";
                string s_result = Request.QueryString.Get("result").Trim();

                if (s_result.Contains("updated"))
                {
                    s_message = "updated";
                }
                else if (s_result.Contains("deleted"))
                {
                    s_message = "deleted";
                }
                else
                {
                    s_message = "???";
                }

                string[] arr_result = s_result.Split('-');

                if (arr_result.Length == 3)     // If Update Terminal Table
                {
                    int n_rows_affected = 0;
                    int.TryParse(arr_result[2], out n_rows_affected);

                    if (n_rows_affected > 0)
                    {
                        s_message = String.Format("{0} item/s successfully {1}.", n_rows_affected, s_message);
                    }
                    else
                    {
                        s_message = String.Format("No item was {0}.", s_message);
                    }
                }
                else
                {
                    s_message = String.Format("Item successfully {0}.", s_message);
                }

                if (s_result.Contains("terminal"))
                {
                    lblMessageTerminal.Text = s_message;
                }
                else
                {
                    lblMessage.Text = s_message;
                }
            }

            CompanyModel o_company = new CompanyModel();

            if (CompanyID > 0)
            {
                o_company.ID = CompanyID;
                o_company.UserID = n_user_id;

                CompanyAction.Select(ref o_company);

                if (o_company.ErrorMessage != "")
                {
                    lblError.Text = o_company.ErrorMessage;
                    return;
                }
            }

            Bind_Form(o_company);

            txtCompanyNumber.Focus();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) { return; }

            CompanyModel o_company = new CompanyModel();

            o_company.ID = CompanyID;
            o_company.UserID = n_user_id;
            o_company.CityID = Convert.ToInt32(ddlCity.SelectedValue);
            o_company.CompanyNumber = Convert.ToInt64(txtCompanyNumber.Text.Trim());

            o_company.CompanyName = txtCompanyName.Text.Trim();
            o_company.Phone = txtPhone.Text.Trim();
            o_company.Fax = txtFax.Text.Trim();
            o_company.Mail = txtMail.Text.Trim();
            o_company.Address = txtAddress.Text.Trim();

            CompanyAction.Update(ref o_company);

            if (o_company.ErrorMessage != "")
            {
                lblError.Text = o_company.ErrorMessage;
                return;
            }

            string s_guid = Request.QueryString.Get("q");
            string s_result = "updated";

            if (CompanyID == 0)
            {
                s_guid = Guid.NewGuid().ToString();
                s_result = "inserted";

                ((Dictionary<string, int>)Session["CompanyLookupTable"]).Add(s_guid, o_company.ID);
            }

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}", s_result, Request.QueryString.Get("i"), s_guid));
        }

        protected void btnSaveTerminal_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) { return; }

            TerminalModel o_terminal = new TerminalModel();

            o_terminal.ID = 0;  // ONLY ADD NEW AVAILABLE
            o_terminal.UserID = n_user_id;
            o_terminal.CompanyID = CompanyID;
            o_terminal.TerminalNumber = Convert.ToInt64(txtTerminalNumber.Text.Trim());
            o_terminal.TerminalName = txtTerminalName.Text.Trim();

            TerminalAction.Update(ref o_terminal);

            if (o_terminal.ErrorMessage != "")
            {
                lblErrorTerminal.Text = o_terminal.ErrorMessage;
                return;
            }

            if (o_terminal.ID == 0)
            {
                lblErrorTerminal.Text = "No item was updated.";
                return;
            }

            string s_interface = Request.QueryString.Get("i");
            string s_guid = Request.QueryString.Get("q");

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}&trm={3}", "updated-terminal", s_interface, s_guid, o_terminal.ID));   // trm = TerminalID
        }

        protected void btnUploadTerminal_Click(object sender, EventArgs e)
        {
            string s_error = "";

            string s_file_name = ExcelAction.Save(fuTerminalUpload, ref s_error);

            if (s_error != "")
            {
                lblErrorTerminal.Text = s_error;
                return;
            }

            int n_rows_affected = 0;

            string s_path = ExcelAction.ExcelFolder + "\\" + s_file_name;

            DataTable dt_terminal = new DataTable();

            ExcelAction.Bind_Data_Table(s_path, ref dt_terminal, ref s_error);

            if (s_error != "") { goto Finish; }

            List<string> lst_column = new List<string>();

            lst_column.Add("TerminalNumber");
            lst_column.Add("TerminalName");

            if (dt_terminal.Columns.Count != lst_column.Count)
            {
                s_error = String.Format("File must contain {0} column/s.", lst_column.Count);
                goto Finish;
            }

            foreach (DataColumn dc_terminal in dt_terminal.Columns)
            {
                if (lst_column.Contains(dc_terminal.ColumnName) == false)
                {
                    s_error = String.Format("File must contain following column/s: {0}.", String.Join(", ", lst_column.ToArray()));
                    goto Finish;
                }
            }

            dt_terminal = new DataView(dt_terminal).ToTable(true, lst_column.ToArray());

            for (int i = dt_terminal.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr_terminal = dt_terminal.Rows[i];

                long n_number = 0;
                long.TryParse(dr_terminal["TerminalNumber"].ToString(), out n_number);

                if (n_number == 0 || dt_terminal.Select(" TerminalNumber = " + n_number).Length > 1)
                {
                    dt_terminal.Rows.RemoveAt(i);
                }
                else
                {
                    string s_name = dr_terminal["TerminalName"].ToString().Trim();

                    if (s_name.Length > 20) { s_name = s_name.Substring(0, 20); }

                    dr_terminal["TerminalNumber"] = n_number;
                    dr_terminal["TerminalName"] = s_name;
                }
            }

            TerminalAction.Update_Table(n_user_id, CompanyID, dt_terminal, ref n_rows_affected, ref s_error);

            if (s_error != "") { goto Finish; }

            Finish:

            ExcelAction.Remove(s_path, ref s_error);

            if (s_error != "")
            {
                lblErrorTerminal.Text = s_error;
            }
            else
            {
                Response.Redirect(String.Format("FrameForm.aspx?result=updated-terminal-{0}&i={1}&q={2}", n_rows_affected, Request.QueryString.Get("i"), Request.QueryString.Get("q")));
            }
        }

        protected void btnDeleteTerminal_Command(object sender, CommandEventArgs e)
        {
            TerminalModel o_terminal = new TerminalModel();

            o_terminal.ID = Convert.ToInt32(e.CommandArgument);
            o_terminal.UserID = n_user_id;
            o_terminal.CompanyID = CompanyID;

            bool b_deleted = TerminalAction.Delete(ref o_terminal);

            if (o_terminal.ErrorMessage != "")
            {
                lblErrorTerminal.Text = o_terminal.ErrorMessage;
                return;
            }

            if (b_deleted == false)
            {
                lblErrorTerminal.Text = "No item was deleted.";
                return;
            }

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}", "deleted-terminal", Request.QueryString.Get("i"), Request.QueryString.Get("q")));
        }

        private void Bind_Form(CompanyModel o_company)
        {
            string s_error = "";

            DB.Bind_List_Control(ddlCity, "sprCity", ref s_error);

            ddlCity.SelectedValue = o_company.CityID.ToString();

            if (o_company.CompanyNumber > 0) { txtCompanyNumber.Text = o_company.CompanyNumber.ToString(); }

            txtCompanyName.Text = o_company.CompanyName;
            txtPhone.Text = o_company.Phone;
            txtFax.Text = o_company.Fax;
            txtMail.Text = o_company.Mail;
            txtAddress.Text = o_company.Address;

            if (o_company.ID > 0)
            {
                Bind_Table_Terminal();
            }
            else
            {
                divTerminal_Change.Visible = false;
            }
        }

        private void Bind_Table_Terminal()
        {
            DataTable o_data_table = new DataTable();

            string s_error = "";

            TerminalAction.Select(ref o_data_table, ref s_error, n_user_id, CompanyID);

            if (s_error != "")
            {
                lblErrorTerminal.Text = s_error;
                return;
            }

            if (o_data_table.Rows.Count == 0) { return; }

            repTerminal.DataSource = o_data_table;
            repTerminal.DataBind();
        }
    }
}
