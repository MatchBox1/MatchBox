using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public partial class TemplateForm : UserControl
    {
        public int TemplateID { get; set; }

        private bool b_importable = false;

        private UserModel o_user_authorized = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            o_user_authorized = (UserModel)Session["User"];

            b_importable = (bool)Session["TemplateIsImportable"];

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

                s_message = String.Format("Item successfully {0}.", s_message);

                lblMessage.Text = s_message;
            }

            TemplateModel o_template = new TemplateModel();

            if (TemplateID > 0)
            {
                hedTitle.InnerHtml = "Template #" + TemplateID;

                o_template.ID = TemplateID;
                o_template.UserID = o_user_authorized.ID;
                o_template.IsImportable = b_importable;

                TemplateAction.Select(ref o_template);

                if (o_template.ErrorMessage != "")
                {
                    lblError.Text = o_template.ErrorMessage;
                    return;
                }
            }
            else
            {
                hedTitle.InnerHtml = "New Template";
            }

            Bind_Form(o_template);

            if (o_template.ErrorMessage != "")
            {
                lblError.Text = o_template.ErrorMessage;
                return;
            }
        }

        protected void Check_Template_For(object sender, EventArgs e)
        {
            repTemplateField.DataSource = null;
            repTemplateField.DataBind();

            divTemplateField.Visible = false;

            if (TemplateID > 0) { divTemplateMessage.InnerHtml = "Template For Changed"; }

            ddlDataSource.Enabled = false;
            ddlCredit.Enabled = false;
            ddlDiscount.Enabled = false;

            string s_radio_button_id = ((RadioButton)sender).ID;

            switch (s_radio_button_id)
            {
                case "rdoDataSource":
                    ddlDataSource.Enabled = true;
                    ddlCredit.SelectedIndex = 0;
                    ddlDiscount.SelectedIndex = 0;
                    break;
                case "rdoCredit":
                    ddlCredit.Enabled = true;
                    ddlDataSource.SelectedIndex = 0;
                    ddlDiscount.SelectedIndex = 0;
                    break;
                case "rdoDiscount":
                    ddlDiscount.Enabled = true;
                    ddlDataSource.SelectedIndex = 0;
                    ddlCredit.SelectedIndex = 0;
                    break;
            }

            string s_error = "";
            string s_table = s_radio_button_id == "rdoDataSource" ? "tblData_Inside" : "tblData_Outside";

            DataTable dt_data_field = new DataTable();

            DB.Bind_Data_Table("sprData_Field", ref dt_data_field, ref s_error, "@TableName", s_table);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            ViewState["TableDataField"] = dt_data_field;
        }

        protected void cvTemplateFor_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool b_valid = rdoDataSource.Checked || rdoCredit.Checked || rdoDiscount.Checked;

            if (b_valid == false)
            {
                cvTemplateFor.ErrorMessage = "Select 'Template For'.";
                goto Finish;
            }

            if (rdoDataSource.Checked == true)
            {
                int n_data_source = 0;
                int.TryParse(ddlDataSource.SelectedValue, out n_data_source);

                if (n_data_source <= 0)
                {
                    b_valid = false;
                    cvTemplateFor.ErrorMessage = "Select 'Inside Data Source'.";
                    ddlDataSource.Enabled = true;
                    ddlDataSource.Focus();
                    goto Finish;
                }
            }

            if (rdoCredit.Checked == true)
            {
                int n_credit = 0;
                int.TryParse(ddlCredit.SelectedValue, out n_credit);

                if (n_credit <= 0)
                {
                    b_valid = false;
                    cvTemplateFor.ErrorMessage = "Select 'Credit'.";
                    ddlCredit.Enabled = true;
                    ddlCredit.Focus();
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

        protected void repTemplateField_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView o_data_row = (DataRowView)e.Item.DataItem;

                DataTable dt_data_field = (DataTable)ViewState["TableDataField"];

                DropDownList ddl_field_db = (DropDownList)e.Item.FindControl("ddlFieldFromDB");

                foreach (DataRow dr_data_field in dt_data_field.Rows)
                {
                    ListItem o_list_item = new ListItem();

                    o_list_item.Text = dr_data_field["FieldDescription"].ToString();
                    o_list_item.Value = dr_data_field["FieldName"].ToString();
                    o_list_item.Attributes.Add("data-type", dr_data_field["FieldType"].ToString());

                    ddl_field_db.Items.Add(o_list_item);
                }

                ddl_field_db.Items.Insert(0, new ListItem("", ""));

                string s_field_db = o_data_row["FieldFromDB"].ToString();

                if (s_field_db != "")
                {
                    ddl_field_db.SelectedValue = s_field_db;

                    string s_type = ddl_field_db.SelectedItem.Attributes["data-type"];

                    switch (s_type)
                    {
                        case "date":
                            HtmlGenericControl div_field_format_date = (HtmlGenericControl)e.Item.FindControl("divFieldFormatDate");

                            div_field_format_date.Attributes["class"] = "";

                            string s_format = o_data_row["FieldFormat"].ToString();

                            if (s_format != "")
                            {
                                string[] arr_format = s_format.Split(';');

                                DropDownList ddl_format_date = (DropDownList)e.Item.FindControl("ddlFormatDate");

                                if (arr_format[0].Trim() == "date")
                                {
                                    DropDownList ddl_format_date_delimiter = (DropDownList)e.Item.FindControl("ddlFormatDateDelimiter");
                                    HtmlGenericControl span_format_date_delimiter = (HtmlGenericControl)e.Item.FindControl("spanFormatDateDelimiter");

                                    span_format_date_delimiter.Attributes["class"] = "";

                                    ddl_format_date.SelectedValue = arr_format[1].Trim();
                                    ddl_format_date_delimiter.SelectedValue = arr_format[2].Trim();
                                }
                                else if (arr_format[0].Trim() == "number")
                                {
                                    DropDownList ddl_format_date_part = (DropDownList)e.Item.FindControl("ddlFormatDatePart");
                                    TextBox txt_format_date_year = (TextBox)e.Item.FindControl("txtFormatDateYear");
                                    TextBox txt_format_date_month = (TextBox)e.Item.FindControl("txtFormatDateMonth");
                                    TextBox txt_format_date_day = (TextBox)e.Item.FindControl("txtFormatDateDay");
                                    HtmlGenericControl span_format_date_from_number = (HtmlGenericControl)e.Item.FindControl("spanFormatDateFromNumber");

                                    span_format_date_from_number.Attributes["class"] = "";

                                    ddl_format_date.SelectedValue = arr_format[0].Trim();
                                    ddl_format_date_part.SelectedValue = arr_format[1].Trim();

                                    string[] s_base_date = arr_format[2].Trim().Split('-');

                                    txt_format_date_year.Text = s_base_date[0];
                                    txt_format_date_month.Text = s_base_date[1];
                                    txt_format_date_day.Text = s_base_date[2];
                                }
                            }

                            break;
                        case "bit":
                            HtmlGenericControl div_field_format_bit = (HtmlGenericControl)e.Item.FindControl("divFieldFormatBit");
                            TextBox txt_format_bit = (TextBox)e.Item.FindControl("txtFormatBit");

                            div_field_format_bit.Attributes["class"] = "";
                            txt_format_bit.Text = o_data_row["FieldFormat"].ToString();

                            break;
                    }
                }
            }
        }

        protected void btnUploadExcel_Click(object sender, EventArgs e)
        {
            bool b_valid = rdoDataSource.Checked || rdoCredit.Checked || rdoDiscount.Checked;

            if (b_valid == false)
            {
                lblError.Text = "Select 'Template For'.";
                return;
            }

            if (!Page.IsValid) { return; }

            int n_rows = Convert.ToInt32(txtHeaderRowsCount.Text.Trim());

            if (n_rows <= 0)
            {
                lblError.Text = "Enter header rows count.";
                return;
            }

            string s_error = "";

            string s_file_name = ExcelAction.Save(fuUploadExcel, ref s_error);

            if (s_error != "")
            {
                rfvUploadExcel.IsValid = false;
                rfvUploadExcel.ErrorMessage = s_error;
                return;
            }

            string s_path = ExcelAction.ExcelFolder + "\\" + s_file_name;

            DataTable dt_excel = new DataTable();

            ExcelAction.Bind_Data_Table(s_path, ref dt_excel, ref s_error, "", "NO", n_rows);       //Bind_Excel_Table(s_path, ref dt_excel, ref s_error, n_rows);

            if (s_error != "")
            {
                lblError.Text = s_error;
                goto Finish;
            }

            // CHECK dt_excel.Columns.Count. EXCEL TABLE CAN CONTAINS ONLY [ExcelAction.MaxFieldsCount] FIELDS !!!

            if (dt_excel.Columns.Count > ExcelAction.MaxColumnsCount)
            {
                s_error = String.Format("Excel file contains {0} columns. Maximum columns count supported by system is {1}.", dt_excel.Columns.Count, ExcelAction.MaxColumnsCount);
                goto Finish;
            }

            DataTable dt_db = new DataTable();

            dt_db.Columns.Add("FieldFromExcel", typeof(string));
            dt_db.Columns.Add("FieldFromDB");
            dt_db.Columns.Add("FieldExclude");
            dt_db.Columns.Add("FieldFormat");

            for (int i_col = 0; i_col < dt_excel.Columns.Count; i_col++)
            {
                string s_field_excel = "";

                for (int i_row = 0; i_row < dt_excel.Rows.Count; i_row++)
                {
                    s_field_excel += " " + dt_excel.Rows[i_row][i_col].ToString().Trim();
                }

                s_field_excel = ExcelAction.Valid_Field_Name(s_field_excel);

                if (s_field_excel != "")
                {
                    DataRow dr_db = dt_db.NewRow();

                    dr_db["FieldFromExcel"] = s_field_excel;
                    dr_db["FieldFromDB"] = "";
                    dr_db["FieldExclude"] = "";
                    dr_db["FieldFormat"] = "";

                    dt_db.Rows.Add(dr_db);
                }
            }

            divTemplateField.Visible = true;

            lblTemplateFieldCount.Text = dt_db.Rows.Count.ToString();

            repTemplateField.DataSource = dt_db;
            repTemplateField.DataBind();

            Finish:

            ExcelAction.Remove(s_path, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) { return; }

            TemplateModel o_template = new TemplateModel();

            o_template.ID = (b_importable == true) ? 0 : TemplateID;

            o_template.UserID = o_user_authorized.ID;
            o_template.DataSourceID = Convert.ToInt32(ddlDataSource.SelectedValue);
            o_template.CreditID = Convert.ToInt32(ddlCredit.SelectedValue);
            o_template.DiscountID = Convert.ToInt32(ddlDiscount.SelectedValue);

            int n_rows = 0;
            int.TryParse(txtHeaderRowsCount.Text.Trim(), out n_rows);

            o_template.HeaderRowsCount = n_rows;

            DataTable dt_data_field = (DataTable)ViewState["TableDataField"];

            o_template.TableTemplateField.Rows.Clear();

            foreach (RepeaterItem o_item in repTemplateField.Items)
            {
                HiddenField hid_field_excel = (HiddenField)o_item.FindControl("hidFieldFromExcel");
                DropDownList ddl_field_db = (DropDownList)o_item.FindControl("ddlFieldFromDB");
                TextBox txt_field_exclude = (TextBox)o_item.FindControl("txtFieldExclude");

                string s_field_from_excel = hid_field_excel.Value.Trim();
                string s_field_from_db = ddl_field_db.SelectedValue;
                string s_field_exclude = txt_field_exclude.Text.Trim();

                if (o_template.TableTemplateField.Select(" FieldFromExcel = '" + s_field_from_excel + "' ").Length > 0)
                {
                    lblError.Text = "Duplicate values in Excel Fields (" + s_field_from_excel + ").";
                    return;
                }

                if (s_field_from_db != "" && o_template.TableTemplateField.Select(" FieldFromDB = '" + s_field_from_db + "' ").Length > 0)
                {
                    lblError.Text = "Duplicate values in DB Fields (" + s_field_from_db + ").";
                    return;
                }

                string s_field_format = "", s_field_type = "";

                if (s_field_from_db != "")
                {
                    s_field_type = dt_data_field.Select(" FieldName = '" + s_field_from_db + "' ").First()["FieldType"].ToString();
                }

                switch (s_field_type)
                {
                    case "date":
                        DropDownList ddl_format_date = (DropDownList)o_item.FindControl("ddlFormatDate");

                        if (ddl_format_date.SelectedValue != "")
                        {
                            string s_format = ddl_format_date.SelectedValue;

                            if (s_format == "number")
                            {
                                DropDownList ddl_format_date_part = (DropDownList)o_item.FindControl("ddlFormatDatePart");
                                TextBox txt_format_date_year = (TextBox)o_item.FindControl("txtFormatDateYear");
                                TextBox txt_format_date_month = (TextBox)o_item.FindControl("txtFormatDateMonth");
                                TextBox txt_format_date_day = (TextBox)o_item.FindControl("txtFormatDateDay");

                                string s_part = ddl_format_date_part.SelectedValue;

                                int n_year = 0, n_month = 0, n_day = 0;

                                int.TryParse(txt_format_date_year.Text, out n_year);
                                int.TryParse(txt_format_date_month.Text, out n_month);
                                int.TryParse(txt_format_date_day.Text, out n_day);

                                if (s_part != "" && n_year > 0 && n_month > 0 && n_day > 0)
                                {
                                    string s_year = n_year.ToString(), s_month = n_month.ToString(), s_day = n_day.ToString();

                                    if (s_month.Length == 1) { s_month = "0" + s_month; }
                                    if (s_day.Length == 1) { s_day = "0" + s_day; }

                                    if (s_year.Length == 4 && s_month.Length == 2 && s_day.Length == 2)
                                    {
                                        s_field_format = String.Format("number ; {0} ; {1}-{2}-{3}", s_part, s_year, s_month, s_day);
                                    }
                                }
                            }
                            else
                            {
                                DropDownList ddl_format_date_delimiter = (DropDownList)o_item.FindControl("ddlFormatDateDelimiter");

                                string s_delimiter = ddl_format_date_delimiter.SelectedValue;

                                if (s_format.Contains("["))
                                {
                                    if (s_delimiter == "none")
                                    {
                                        s_delimiter = "";
                                    }
                                    else
                                    {
                                        s_format = s_format.Replace("[", "").Replace("]", "");
                                    }
                                }

                                if (s_delimiter != "")
                                {
                                    s_field_format = String.Format("date ; {0} ; {1}", s_format, s_delimiter);
                                }
                            }
                        }

                        break;
                    case "bit":
                        TextBox txt_format_bit = (TextBox)o_item.FindControl("txtFormatBit");

                        string[] arr_field_format = txt_format_bit.Text.Trim().Split(',');

                        for (int i = 0; i < arr_field_format.Length; i++)
                        {
                            string s_value = arr_field_format[i].Trim();

                            if (s_value != "")
                            {
                                if (s_field_format != "") { s_field_format += ","; }

                                s_field_format += s_value;
                            }
                        }

                        break;
                }

                DataRow o_data_row = o_template.TableTemplateField.NewRow();

                o_data_row["FieldFromExcel"] = s_field_from_excel;
                o_data_row["FieldFromDB"] = s_field_from_db;
                o_data_row["FieldExclude"] = s_field_exclude;
                o_data_row["FieldFormat"] = s_field_format.Trim();

                o_template.TableTemplateField.Rows.Add(o_data_row);
            }

            if (o_template.TableTemplateField.Rows.Count > 0)
            {
                TemplateAction.Validate(ref o_template);

                if (o_template.ErrorMessage != "")
                {
                    lblError.Text = o_template.ErrorMessage;
                    return;
                }
            }

            int[] arr_user_service_id = (int[])ViewState["arr_user_service_id"];

            TemplateAction.Validate_Completed(ref o_template, dt_data_field, arr_user_service_id);

            if (o_template.ErrorMessage != "")
            {
                lblError.Text = o_template.ErrorMessage;
                return;
            }

            TemplateAction.Update(ref o_template);

            if (o_template.ErrorMessage != "")
            {
                lblError.Text = o_template.ErrorMessage;
                return;
            }

            string s_guid = Request.QueryString.Get("q");
            string s_result = "updated";

            if (TemplateID == 0 || b_importable == true)
            {
                s_guid = Guid.NewGuid().ToString();
                s_result = "inserted";

                ((Dictionary<string, int>)Session["TemplateLookupTable"]).Add(s_guid, o_template.ID);
            }

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}", s_result, Request.QueryString.Get("i"), s_guid));
        }

        private void Bind_Form(TemplateModel o_template)
        {
            if (o_template.IsImportable == true)
            {
                btnSaveTop.Text = "Import";
                btnSaveBottom.Text = "Import";
            }

            // ViewState["arr_user_service_id"] / ViewState["TableDataField"]

            string s_error = "";

            List<SqlParameter> o_data_source_parameters = new List<SqlParameter>();
            o_data_source_parameters.Add(new SqlParameter("@UserID", o_user_authorized.ID));

            DB.Bind_List_Control(ddlDataSource, "sprDataSource", ref s_error, null, o_data_source_parameters);

            DB.Bind_List_Control(ddlCredit, "sprCredit", ref s_error);
            DB.Bind_List_Control(ddlDiscount, "sprDiscount", ref s_error);

            //if (o_user_authorized.IsAdmin)
            //{
            //    DB.Bind_List_Control(ddlCredit, "sprCredit", ref s_error);
            //    DB.Bind_List_Control(ddlDiscount, "sprDiscount", ref s_error);
            //}
            //else if (o_user_authorized.IsUser)
            //{
            //    List<SqlParameter> o_credit_parameters = new List<SqlParameter>();
            //    o_credit_parameters.Add(new SqlParameter("@UserID", o_user_authorized.ID));

            //    List<SqlParameter> o_discount_parameters = new List<SqlParameter>();
            //    o_discount_parameters.Add(new SqlParameter("@UserID", o_user_authorized.ID));

            //    DB.Bind_List_Control(ddlCredit, "sprUserCredit", ref s_error, null, o_credit_parameters);
            //    DB.Bind_List_Control(ddlDiscount, "sprUserDiscount", ref s_error, null, o_discount_parameters);
            //}

            if (s_error != "")
            {
                o_template.ErrorMessage = s_error;
                return;
            }

            int[] arr_user_service_id;

            if (o_user_authorized.IsAdmin)
            {
                DataTable dt_service = new DataTable();

                DB.Bind_Data_Table("sprService", ref dt_service, ref s_error);

                if (s_error != "")
                {
                    o_template.ErrorMessage = "Error on select services.";
                    return;
                }

                arr_user_service_id = (
                   from r in dt_service.AsEnumerable()
                   select r.Field<int>("ID")
                   ).ToArray();
            }
            else
            {
                arr_user_service_id = (
                   from r in o_user_authorized.TableUserService.AsEnumerable()
                   where r.Field<int>("IsSelected") == 1
                   select r.Field<int>("ServiceID")
                   ).ToArray();
            }

            ViewState["arr_user_service_id"] = arr_user_service_id;

            if (o_template.ID == 0) { return; }

            DataTable dt_data_field = new DataTable();

            string s_table = o_template.DataSourceID > 0 ? "tblData_Inside" : "tblData_Outside";

            DB.Bind_Data_Table("sprData_Field", ref dt_data_field, ref s_error, "@TableName", s_table);

            if (s_error != "")
            {
                o_template.ErrorMessage = s_error;
                return;
            }

            ViewState["TableDataField"] = dt_data_field;

            if (o_template.IsCompleted)
            {
                lblStatusTop.Text = "Completed";
                lblStatusTop.ForeColor = Color.Green;
            }
            else
            {
                lblStatusTop.Text = "Not Completed";
                lblStatusTop.ForeColor = Color.Red;
            }

            lblStatusBottom.Text = lblStatusTop.Text;
            lblStatusBottom.ForeColor = lblStatusTop.ForeColor;

            if (o_template.DataSourceID > 0)
            {
                rdoDataSource.Checked = true;
                ddlDataSource.Enabled = true;
                ddlDataSource.SelectedValue = o_template.DataSourceID.ToString();
            }
            else if (o_template.CreditID > 0)
            {
                rdoCredit.Checked = true;
                ddlCredit.Enabled = true;
                ddlCredit.SelectedValue = o_template.CreditID.ToString();
            }
            else if (o_template.DiscountID > 0)
            {
                rdoDiscount.Checked = true;
                ddlDiscount.Enabled = true;
                ddlDiscount.SelectedValue = o_template.DiscountID.ToString();
            }

            txtHeaderRowsCount.Text = o_template.HeaderRowsCount.ToString();

            divTemplateMessage.InnerHtml = "";

            if (arr_user_service_id.Length > 0)
            {
                foreach (DataRow o_data_row in o_template.TableTemplateService.Select(" ServiceID IN ( " + String.Join(",", arr_user_service_id) + " ) "))
                {
                    divTemplateMessage.InnerHtml += o_data_row["CompletedMessage"];
                }
            }

            if (o_template.TableTemplateField.Rows.Count > 0)
            {
                divTemplateField.Visible = true;

                lblTemplateFieldCount.Text = o_template.TableTemplateField.Rows.Count.ToString();

                repTemplateField.DataSource = o_template.TableTemplateField;
                repTemplateField.DataBind();
            }
            
            if (o_template.TableTemplateCurrency.Rows.Count > 0)
            {
                divTemplateCurrency.Visible = true;

                repTemplateCurrency.DataSource = o_template.TableTemplateCurrency;
                repTemplateCurrency.DataBind();
            }

            if (o_template.TableTemplateCredit.Rows.Count > 0)
            {
                divTemplateCredit.Visible = true;

                repTemplateCredit.DataSource = o_template.TableTemplateCredit;
                repTemplateCredit.DataBind();
            }

            if (o_template.TableTemplateCard.Rows.Count > 0)
            {
                divTemplateCard.Visible = true;

                repTemplateCard.DataSource = o_template.TableTemplateCard;
                repTemplateCard.DataBind();
            }

            if (o_template.TableTemplateOperationType.Rows.Count > 0)
            {
                divOperationType.Visible = true;

                repOperationType.DataSource = o_template.TableTemplateOperationType;
                repOperationType.DataBind();
            }
        }

        //private void Bind_Excel_Table(string s_excel_file_path, ref DataTable o_data_table, ref string s_error, int n_rows = 0)
        //{
        //    Microsoft.Office.Interop.Excel.Application x_app = new Microsoft.Office.Interop.Excel.Application();
        //    Microsoft.Office.Interop.Excel.Workbook x_workbook = x_app.Workbooks.Open(s_excel_file_path);
        //    Microsoft.Office.Interop.Excel._Worksheet x_worksheet = x_workbook.Sheets[1];
        //    Microsoft.Office.Interop.Excel.Range x_range = x_worksheet.UsedRange;

        //    int n_row_count = x_range.Rows.Count;
        //    int n_col_count = x_range.Columns.Count;

        //    for (int i = 1; i <= n_col_count; i++)
        //    {
        //        o_data_table.Columns.Add("F" + i, typeof(string));
        //    }

        //    if (n_rows > 0) { n_row_count = n_rows; }

        //    for (int i_row = 1; i_row <= n_row_count; i_row++)
        //    {
        //        DataRow o_data_row = o_data_table.NewRow();

        //        for (int i_col = 1; i_col <= n_col_count; i_col++)
        //        {
        //            string s_value = "";

        //            if (x_range.Cells[i_row, i_col] != null && x_range.Cells[i_row, i_col].Value2 != null)
        //            {
        //                s_value = x_range.Cells[i_row, i_col].Value2.ToString();
        //            }

        //            o_data_row[i_col - 1] = s_value.Trim();
        //        }

        //        o_data_table.Rows.Add(o_data_row);
        //    }

        //    x_workbook.Close();

        //    x_range = null;
        //    x_worksheet = null;
        //    x_workbook = null;
        //    x_app = null;
        //}
    }
}
