using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public partial class AgreementPeriod : UserControl
    {
        public int AgreementID { get; set; }

        private int n_user_id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (AgreementID <= 0) { return; }

            n_user_id = ((UserModel)Session["User"]).ID;

            if (Page.IsPostBack) { return; }

            if (Request.QueryString["result"] != null)
            {
                string s_message = "";
                string s_result = Request.QueryString.Get("result");

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

                if (s_result.Contains("updated-supplier-terminal-"))     // If Update SupplierTerminal Table
                {
                    string[] arr_result = s_result.Split('-');

                    int n_rows_affected = 0;
                    int.TryParse(arr_result[3], out n_rows_affected);

                    s_message = String.Format("{0} item/s successfully {1}.", n_rows_affected, s_message);
                }
                else
                {
                    s_message = String.Format("Item successfully {0}.", s_message);
                }

                lblMessage.Text = s_message;

                if (Request.QueryString["period"] != null)
                {
                    // SHOW Agreement Period FORM ( period = AgreementPeriodID )

                    CommandEventArgs o_args = new CommandEventArgs("Edit_Period", Request.QueryString.Get("period"));

                    Edit_Period(null, o_args);
                }
            }

            AgreementModel o_agreement = new AgreementModel();

            o_agreement.ID = AgreementID;
            o_agreement.UserID = n_user_id;

            AgreementAction.Select(ref o_agreement);

            if (o_agreement.ErrorMessage != "")
            {
                lblError.Text = o_agreement.ErrorMessage;
                return;
            }

            Bind_Form(o_agreement);
        }

        protected void repAgreementPeriod_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataTable dt_supplier_terminal = (DataTable)ViewState["TableSupplierTerminal"];

                if (dt_supplier_terminal == null || dt_supplier_terminal.Rows.Count == 0) { return; }

                DataRowView o_data_row = (DataRowView)e.Item.DataItem;

                int n_agreement_period_id = 0;
                int.TryParse(o_data_row["ID"].ToString(), out n_agreement_period_id);

                DataRow[] dr_supplier_terminal = dt_supplier_terminal.Select("AgreementPeriodID = " + n_agreement_period_id);

                if (dr_supplier_terminal.Length > 0)
                {
                    Repeater repSupplierTerminal = (Repeater)e.Item.FindControl("repSupplierTerminal");

                    repSupplierTerminal.DataSource = dr_supplier_terminal.CopyToDataTable();
                    repSupplierTerminal.DataBind();
                }
            }
        }

        protected void repSupplierTerminalForm_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataTable dt_terminal = (DataTable)ViewState["TableTerminal"];

                if (dt_terminal == null || dt_terminal.Rows.Count == 0) { return; }

                // =================================

                DropDownList ddlTerminal = (DropDownList)e.Item.FindControl("ddlTerminal");

                ddlTerminal.DataSource = dt_terminal;
                ddlTerminal.DataValueField = "ID";
                ddlTerminal.DataTextField = "Terminal";
                ddlTerminal.DataBind();
                ddlTerminal.Items.Insert(0, new ListItem("", "0"));

                // =================================

                CheckBoxList cblTerminal = (CheckBoxList)e.Item.FindControl("cblTerminal");

                cblTerminal.DataSource = dt_terminal;
                cblTerminal.DataValueField = "ID";
                cblTerminal.DataTextField = "Terminal";
                cblTerminal.DataBind();
            }
        }

        protected void Edit_Period(object sender, CommandEventArgs e)
        {
            if (AgreementID <= 0) { return; }

            int n_agreement_period_id = 0;
            int.TryParse(e.CommandArgument.ToString(), out n_agreement_period_id);

            Bind_Form_Period(n_agreement_period_id);

            divSupplierTerminalForm.Visible = false;
            divSupplierTerminalUpload.Visible = false;

            divPeriodForm.Visible = true;

            txtDateStart.Focus();
        }

        protected void Add_Supplier_Terminal(object sender, CommandEventArgs e)
        {
            if (AgreementID <= 0) { return; }

            int n_agreement_period_id = 0;
            int.TryParse(e.CommandArgument.ToString(), out n_agreement_period_id);

            Bind_Form_Supplier_Terminal(n_agreement_period_id);

            divPeriodForm.Visible = false;
            divSupplierTerminalUpload.Visible = false;

            divSupplierTerminalForm.Visible = true;
        }

        protected void Upload_Supplier_Terminal(object sender, CommandEventArgs e)
        {
            if (AgreementID <= 0) { return; }

            int n_agreement_period_id = 0;
            int.TryParse(e.CommandArgument.ToString(), out n_agreement_period_id);

            if (n_agreement_period_id <= 0) { return; }

            ViewState["AgreementPeriodID"] = n_agreement_period_id;

            AgreementPeriodModel o_agreement_period = new AgreementPeriodModel();

            o_agreement_period.ID = n_agreement_period_id;
            o_agreement_period.UserID = n_user_id;
            o_agreement_period.AgreementID = AgreementID;

            AgreementPeriodAction.Select(ref o_agreement_period);

            if (o_agreement_period.ErrorMessage != "")
            {
                lblError.Text = o_agreement_period.ErrorMessage;
                return;
            }

            lblDate_Upload.Text = String.Format("[ {0:dd/MM/yyyy} - {1:dd/MM/yyyy} ]", o_agreement_period.DateStart, o_agreement_period.DateEnd);

            divPeriodForm.Visible = false;
            divSupplierTerminalForm.Visible = false;

            divSupplierTerminalUpload.Visible = true;
        }

        protected void Close_Form(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("FrameForm.aspx?i={0}&q={1}&p={2}", Request.QueryString.Get("i"), Request.QueryString.Get("q"), "period"));
        }

        protected void cvDateRange_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool b_valid = true;

            string s_date_start = txtDateStart.Text.Trim();
            string s_date_end = txtDateEnd.Text.Trim();

            DateTime? d_date_start = null;
            DateTime? d_date_end = null;

            if (s_date_start == "")
            {
                b_valid = false;
                cvDateRange.ErrorMessage = "Enter 'Start Date'.";
            }
            else
            {
                d_date_start = Common.Convert_To_Date(s_date_start);

                if (d_date_start == null)
                {
                    b_valid = false;
                    cvDateRange.ErrorMessage = "'Start Date' not valid.";
                }
            }

            if (b_valid == false)
            {
                txtDateStart.Focus();
                goto Finish;
            }

            if (s_date_end != "")
            {
                d_date_end = Common.Convert_To_Date(s_date_end);

                if (d_date_end == null)
                {
                    b_valid = false;
                    cvDateRange.ErrorMessage = "'End Date' not valid.";
                }
                else if (d_date_start > d_date_end)
                {
                    b_valid = false;
                    cvDateRange.ErrorMessage = "'Date Start' can't be greater than 'Date End'.";
                }

                if (b_valid == false)
                {
                    txtDateEnd.Focus();
                    goto Finish;
                }
            }

            AgreementPeriodModel o_agreement_period = new AgreementPeriodModel();

            o_agreement_period.ID = (int)ViewState["AgreementPeriodID"];
            o_agreement_period.UserID = n_user_id;
            o_agreement_period.AgreementID = AgreementID;
            o_agreement_period.DateStart = d_date_start;

            if (d_date_end != null) { o_agreement_period.DateEnd = d_date_end; }

            string s_result = AgreementPeriodAction.Check(ref o_agreement_period);

            if (o_agreement_period.ErrorMessage != "")
            {
                b_valid = false;
                cvDateRange.ErrorMessage = o_agreement_period.ErrorMessage;
                goto Finish;
            }

            if (s_result != "OK")
            {
                b_valid = false;
                cvDateRange.ErrorMessage = s_result;
                goto Finish;
            }

            Finish:

            args.IsValid = b_valid;
        }

        protected void btnSavePeriod_Click(object sender, EventArgs e)
        {
            if (AgreementID <= 0) { return; }

            if (!Page.IsValid) { return; }

            AgreementPeriodModel o_agreement_period = new AgreementPeriodModel();

            o_agreement_period.ID = (int)ViewState["AgreementPeriodID"];
            o_agreement_period.UserID = n_user_id;
            o_agreement_period.AgreementID = AgreementID;

            o_agreement_period.DateStart = Common.Convert_To_Date(txtDateStart.Text);

            DateTime? d_date_end = Common.Convert_To_Date(txtDateEnd.Text);

            if (d_date_end != null) { o_agreement_period.DateEnd = d_date_end; }

            AgreementPeriodAction.Update(ref o_agreement_period);

            if (o_agreement_period.ErrorMessage != "")
            {
                lblError.Text = o_agreement_period.ErrorMessage;
                return;
            }

            string s_interface = Request.QueryString.Get("i");
            string s_guid = Request.QueryString.Get("q");

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}&p={3}&period={4}", "updated-period", s_interface, s_guid, "period", o_agreement_period.ID));
        }

        protected void Update_Supplier_Terminal(object sender, EventArgs e)
        {
            if (AgreementID <= 0) { return; }

            if (!Page.IsValid) { return; }

            int n_agreement_period_id = (int)ViewState["AgreementPeriodID"], n_items_selected = 0, n_items_inserted = 0;

            bool b_error = false;

            foreach (RepeaterItem o_item in repSupplierTerminalForm.Items)
            {
                if (o_item.ItemType == ListItemType.Item || o_item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBoxList cblTerminal = (CheckBoxList)o_item.FindControl("cblTerminal");

                    var chkTerminalArray = cblTerminal.Items.Cast<ListItem>().Where(i => i.Selected == true);

                    if (chkTerminalArray.Count<ListItem>() == 0) { continue; }

                    n_items_selected++;

                    HiddenField hidSupplierID = (HiddenField)o_item.FindControl("hidSupplierID");
                    Label lblSupplierTerminalMessage = (Label)o_item.FindControl("lblSupplierTerminalMessage");
                    HtmlTableCell tdMessage = (HtmlTableCell)o_item.FindControl("tdMessage");

                    int n_supplier_id = Convert.ToInt32(hidSupplierID.Value);

                    string s_error = "";

                    foreach (ListItem chkTerminal in chkTerminalArray)
                    {
                        int n_terminal_id = Convert.ToInt32(chkTerminal.Value);

                        SupplierTerminalModel o_supplier_terminal = new SupplierTerminalModel();

                        o_supplier_terminal.UserID = n_user_id;
                        o_supplier_terminal.AgreementPeriodID = n_agreement_period_id;
                        o_supplier_terminal.SupplierID = n_supplier_id;
                        o_supplier_terminal.TerminalID = n_terminal_id;

                        SupplierTerminalAction.Update(ref o_supplier_terminal);

                        Label lblResult = new Label();

                        string s_result = "", s_css = "";

                        if (o_supplier_terminal.ErrorMessage != "") { s_error = o_supplier_terminal.ErrorMessage; }

                        if (s_error == "" && o_supplier_terminal.ID == 0) { s_error = "No item was updated."; }

                        if (s_error == "")
                        {
                            n_items_inserted++;
                            s_result = "Item successfully inserted";
                            s_css = "block message";
                        }
                        else
                        {
                            b_error = true;
                            s_result = s_error;
                            s_css = "block error";
                        }

                        lblResult.Text = String.Format("{0} &mdash; {1}", chkTerminal.Text, s_result);
                        lblResult.CssClass = s_css;

                        tdMessage.Controls.Add(lblResult);
                    }

                    //DropDownList ddlTerminal = (DropDownList)o_item.FindControl("ddlTerminal");

                    //int n_terminal_id = Convert.ToInt32(ddlTerminal.SelectedValue);

                    //if (n_terminal_id > 0)
                    //{
                    //n_items_selected++;

                    //HiddenField hidSupplierID = (HiddenField)o_item.FindControl("hidSupplierID");
                    //Label lblSupplierTerminalMessage = (Label)o_item.FindControl("lblSupplierTerminalMessage");

                    //int n_supplier_id = Convert.ToInt32(hidSupplierID.Value);

                    //SupplierTerminalModel o_supplier_terminal = new SupplierTerminalModel();

                    //o_supplier_terminal.UserID = n_user_id;
                    //o_supplier_terminal.AgreementPeriodID = n_agreement_period_id;
                    //o_supplier_terminal.SupplierID = n_supplier_id;
                    //o_supplier_terminal.TerminalID = n_terminal_id;

                    //SupplierTerminalAction.Update(ref o_supplier_terminal);

                    //string s_error = "";

                    //if (o_supplier_terminal.ErrorMessage != "") { s_error = o_supplier_terminal.ErrorMessage; }

                    //if (s_error == "" && o_supplier_terminal.ID == 0) { s_error = "No item was updated."; }

                    //if (s_error == "")
                    //{
                    //    n_items_inserted++;

                    //    lblSupplierTerminalMessage.Text = "Item successfully inserted";
                    //    lblSupplierTerminalMessage.CssClass = "message";
                    //}
                    //else
                    //{
                    //    b_error = true;

                    //    lblSupplierTerminalMessage.Text = s_error;
                    //    lblSupplierTerminalMessage.CssClass = "error";
                    //}
                    //}
                }
            }

            if (b_error == true) { return; }

            if (n_items_selected == 0)
            {
                lblError.Text = "No Terminal was selected.";
                return;
            }

            string s_interface = Request.QueryString.Get("i");
            string s_guid = Request.QueryString.Get("q");

            Response.Redirect(String.Format("FrameForm.aspx?result=updated-supplier-terminal-{0}&i={1}&q={2}&p={3}", n_items_inserted, s_interface, s_guid, "period"));
        }

        protected void btnSaveSupplierTerminalUpload_Click(object sender, EventArgs e)
        {
            string s_error = "";

            string s_file_name = ExcelAction.Save(fuSupplierTerminalUpload, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            int n_items_inserted = 0;

            string s_path = ExcelAction.ExcelFolder + "\\" + s_file_name;

            // GET dt_supplier_terminal Uploaded Table

            DataTable dt_supplier_terminal = new DataTable();

            ExcelAction.Bind_Data_Table(s_path, ref dt_supplier_terminal, ref s_error);

            if (s_error != "") { goto Finish; }

            // REMOVE Empty Rows

            dt_supplier_terminal = dt_supplier_terminal.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is DBNull || string.IsNullOrWhiteSpace(field as string))).CopyToDataTable();

            // REMOVE Duplicate Rows

            dt_supplier_terminal = new DataView(dt_supplier_terminal).ToTable(true, new string[] { "SupplierNumber", "TerminalNumber" });

            // GET dt_supplier Table

            DataTable dt_supplier = new DataTable();

            SupplierAction.Select(ref dt_supplier, ref s_error, n_user_id, AgreementID, "Number");

            if (s_error != "") { goto Finish; }

            // GET dt_terminal Table

            DataTable dt_terminal = new DataTable();

            TerminalAction.Select(ref dt_terminal, ref s_error, n_user_id, (int)ViewState["CompanyID"]);

            if (s_error != "") { goto Finish; }

            // CHECK & UPDATE EACH ROW FROM dt_supplier_terminal Table

            int n_agreement_period_id = (int)ViewState["AgreementPeriodID"];

            foreach (DataRow dr in dt_supplier_terminal.Rows)
            {
                string s_supplier_number = dr["SupplierNumber"].ToString();
                string s_terminal_number = dr["TerminalNumber"].ToString();

                DataRow dr_supplier = dt_supplier.Select(" SupplierNumber = " + s_supplier_number).FirstOrDefault();
                DataRow dr_terminal = dt_terminal.Select(" TerminalNumber = " + s_terminal_number).FirstOrDefault();

                if (dr_supplier != null && dr_terminal != null)
                {
                    int n_supplier_id = Convert.ToInt32(dr_supplier["ID"]);
                    int n_terminal_id = Convert.ToInt32(dr_terminal["ID"]);

                    SupplierTerminalModel o_supplier_terminal = new SupplierTerminalModel();

                    o_supplier_terminal.UserID = n_user_id;
                    o_supplier_terminal.AgreementPeriodID = n_agreement_period_id;
                    o_supplier_terminal.SupplierID = n_supplier_id;
                    o_supplier_terminal.TerminalID = n_terminal_id;

                    SupplierTerminalAction.Update(ref o_supplier_terminal);

                    if (o_supplier_terminal.ErrorMessage == "") { n_items_inserted++; }
                }
            }

        Finish:

            ExcelAction.Remove(s_path, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
            }
            else
            {
                Response.Redirect(String.Format("FrameForm.aspx?result=updated-supplier-terminal-{0}&i={1}&q={2}&p={3}", n_items_inserted, Request.QueryString.Get("i"), Request.QueryString.Get("q"), "period"));
            }
        }

        protected void btnDeletePeriod_Command(object sender, CommandEventArgs e)
        {
            AgreementPeriodModel o_agreement_period = new AgreementPeriodModel();

            o_agreement_period.ID = Convert.ToInt32(e.CommandArgument);
            o_agreement_period.UserID = n_user_id;
            o_agreement_period.AgreementID = AgreementID;

            bool b_deleted = AgreementPeriodAction.Delete(ref o_agreement_period);

            if (o_agreement_period.ErrorMessage != "")
            {
                lblError.Text = o_agreement_period.ErrorMessage;
                return;
            }

            if (b_deleted == false)
            {
                lblError.Text = "No item was deleted.";
                return;
            }

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}&p={3}", "deleted-period", Request.QueryString.Get("i"), Request.QueryString.Get("q"), "period"));
        }

        protected void btnDeleteSupplierTerminal_Command(object sender, CommandEventArgs e)
        {
            string s_error = "";

            int n_id = Convert.ToInt32(e.CommandArgument);

            bool b_deleted = SupplierTerminalAction.Delete(n_id, n_user_id, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            if (b_deleted == false)
            {
                lblError.Text = "No item was deleted.";
                return;
            }

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}&p={3}", "deleted-supplier-terminal", Request.QueryString.Get("i"), Request.QueryString.Get("q"), "period"));
        }

        private void Bind_Form(AgreementModel o_agreement)
        {
            string s_error = "";

            ViewState["CompanyID"] = o_agreement.CompanyID;

            lblCompanyName.Text = o_agreement.CompanyName;
            lblCreditName.Text = o_agreement.CreditName;

            DataTable dt_supplier_terminal = new DataTable();

            SupplierTerminalAction.Select(ref dt_supplier_terminal, ref s_error, n_user_id, AgreementID);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            ViewState["TableSupplierTerminal"] = dt_supplier_terminal;

            DataTable dt_agreement_period = new DataTable();

            AgreementPeriodAction.Select(ref dt_agreement_period, ref s_error, n_user_id, AgreementID);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            repAgreementPeriod.DataSource = dt_agreement_period;
            repAgreementPeriod.DataBind();
        }

        private void Bind_Form_Period(int n_agreement_period_id)
        {
            ViewState["AgreementPeriodID"] = n_agreement_period_id;

            if (n_agreement_period_id == 0) { return; }

            AgreementPeriodModel o_agreement_period = new AgreementPeriodModel();

            o_agreement_period.ID = n_agreement_period_id;
            o_agreement_period.UserID = n_user_id;
            o_agreement_period.AgreementID = AgreementID;

            AgreementPeriodAction.Select(ref o_agreement_period);

            if (o_agreement_period.ErrorMessage != "")
            {
                lblError.Text = o_agreement_period.ErrorMessage;
                return;
            }

            txtDateStart.Text = String.Format("{0:dd/MM/yyyy}", o_agreement_period.DateStart);

            if (o_agreement_period.DateEnd != null)
            {
                txtDateEnd.Text = String.Format("{0:dd/MM/yyyy}", o_agreement_period.DateEnd);
            }
        }

        private void Bind_Form_Supplier_Terminal(int n_agreement_period_id)
        {
            ViewState["AgreementPeriodID"] = n_agreement_period_id;

            string s_error = "";

            DataTable dt_supplier = new DataTable();

            SupplierAction.Select(ref dt_supplier, ref s_error, n_user_id, AgreementID, "Number");

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            DataTable dt_terminal = new DataTable();

            TerminalAction.Select(ref dt_terminal, ref s_error, n_user_id, (int)ViewState["CompanyID"]);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            ViewState["TableTerminal"] = dt_terminal;

            repSupplierTerminalForm.DataSource = dt_supplier;
            repSupplierTerminalForm.DataBind();

            AgreementPeriodModel o_agreement_period = new AgreementPeriodModel();

            o_agreement_period.ID = n_agreement_period_id;
            o_agreement_period.UserID = n_user_id;
            o_agreement_period.AgreementID = AgreementID;

            AgreementPeriodAction.Select(ref o_agreement_period);

            if (o_agreement_period.ErrorMessage != "")
            {
                lblError.Text = o_agreement_period.ErrorMessage;
                return;
            }

            lblDate_Form.Text = String.Format("{0:dd/MM/yyyy} - {1:dd/MM/yyyy}", o_agreement_period.DateStart, o_agreement_period.DateEnd);
        }
    }
}
