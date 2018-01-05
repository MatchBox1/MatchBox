using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public partial class UserSettings : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string s_result = "", s_item = "";

            if (Request.QueryString["result"] != null) { s_result = Request.QueryString.Get("result"); }
            if (Request.QueryString["item"] != null) { s_item = Request.QueryString.Get("item"); }

            if (s_result == "updated")
            {
                string s_message = " successfully updated.";

                switch (s_item.ToLower())
                {
                    case "settings":
                        s_message = "Settings " + s_message;
                        break;
                    case "credit_supplier_group":
                        s_message = "Credit Supplier Group " + s_message;
                        break;
                    case "discount_supplier_group":
                        s_message = "Discount Supplier Group " + s_message;
                        break;
                    case "credit_supplier":
                        s_message = "Credit Supplier " + s_message;
                        break;
                    case "discount_supplier":
                        s_message = "Discount Supplier " + s_message;
                        break;
                    case "data_source":
                        s_message = "Data Source " + s_message;
                        break;
                    default:
                        s_message = "Item " + s_message;
                        break;
                }

                lblMessage.Text = s_message;
            }

            if (!Page.IsPostBack)
            {
                Bind_Bank();
                Bind_Currency();
                Bind_Credit();
                Bind_Discount();
                Bind_DataSource();
            }
        }

        protected void repBank_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView o_data_row = (DataRowView)e.Item.DataItem;

                int n_bank_id = 0;
                int.TryParse(o_data_row["ID"].ToString(), out n_bank_id);

                HtmlAnchor o_anchor = (HtmlAnchor)e.Item.FindControl("lnkBankName");
                CheckBoxList o_check_list = (CheckBoxList)e.Item.FindControl("cblBankBranch");

                o_anchor.Attributes.Add("onclick", "javascript: display_by_class(" + @"'" + o_check_list.ClientID + @"'" + ");");

                Bind_Branch(n_bank_id, ref o_check_list);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) { return; }

            DataTable dt_bank_branch = new DataTable();
            DataTable dt_currency = new DataTable();

            dt_bank_branch.Columns.Add("ID", typeof(int));
            dt_currency.Columns.Add("ID", typeof(int));

            foreach (RepeaterItem item in repBank.Items)
            {
                CheckBoxList o_check_list = (CheckBoxList)item.FindControl("cblBankBranch");

                if (o_check_list == null) { continue; }

                foreach (ListItem o_list_item in o_check_list.Items)
                {
                    if (o_list_item.Selected)
                    {
                        DataRow o_data_row = dt_bank_branch.NewRow();

                        o_data_row["ID"] = o_list_item.Value;

                        dt_bank_branch.Rows.Add(o_data_row);
                    }
                }
            }

            foreach (RepeaterItem item in repCurrency.Items)
            {
                CheckBox o_check_box = (CheckBox)item.FindControl("chkIsSelected");
                HiddenField o_hidden = (HiddenField)item.FindControl("hidIsDefault");

                if (o_check_box.Checked || Convert.ToBoolean(o_hidden.Value))
                {
                    DataRow o_data_row = dt_currency.NewRow();

                    o_data_row["ID"] = ((HiddenField)item.FindControl("hidCurrencyID")).Value;

                    dt_currency.Rows.Add(o_data_row);
                }
            }

            SqlCommand o_command = new SqlCommand("sprUserSettingsUpdate", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_user.ID;

            o_command.Parameters.AddWithValue("@TableBankBranchID", dt_bank_branch);
            o_command.Parameters["@TableBankBranchID"].SqlDbType = SqlDbType.Structured;

            o_command.Parameters.AddWithValue("@TableCurrencyID", dt_currency);
            o_command.Parameters["@TableCurrencyID"].SqlDbType = SqlDbType.Structured;

            try
            {
                o_command.Connection.Open();
                o_command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                lblError.Text = "Error on update Settings.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            Response.Redirect("UserSettings.aspx?result=updated&item=settings");
        }

        protected void btnSupplierGroup_Save(object sender, CommandEventArgs e)
        {
            if (!Page.IsValid) { return; }

            string[] arr_arguments = e.CommandArgument.ToString().Split('-');

            string s_item_name = arr_arguments[0];
            string s_user_item_id = arr_arguments[1];

            Button btn_supplier_group = (Button)sender;
            HtmlGenericControl div_supplier_group = (HtmlGenericControl)btn_supplier_group.Parent;
            TextBox txt_supplier_group = (TextBox)div_supplier_group.FindControl("txtSupplierGroup");

            int n_user_item_id = 0;
            int.TryParse(s_user_item_id, out n_user_item_id);

            long n_supplier_group_number = 0;
            long.TryParse(txt_supplier_group.Text.Trim(), out n_supplier_group_number);

            if (n_user_item_id == 0 || n_supplier_group_number == 0) { return; }

            SqlCommand o_command = new SqlCommand("sprUser" + s_item_name + "_SupplierGroupInsert", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Direction = ParameterDirection.Output;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_user.ID;

            o_command.Parameters.Add("@User" + s_item_name + "ID", SqlDbType.Int);
            o_command.Parameters["@User" + s_item_name + "ID"].Value = n_user_item_id;

            o_command.Parameters.Add("@SupplierGroupNumber", SqlDbType.BigInt);
            o_command.Parameters["@SupplierGroupNumber"].Value = n_supplier_group_number;

            try
            {
                o_command.Connection.Open();
                o_command.Parameters["@ID"].Value = o_command.ExecuteScalar();

                int n = Convert.ToInt32(o_command.Parameters["@ID"].Value);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.Text = "Error on update Supplier Group Number.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            Response.Redirect("UserSettings.aspx?result=updated&item=" + s_item_name + "_supplier_group");
        }

        protected void btnSupplier_Save(object sender, CommandEventArgs e)
        {
            if (!Page.IsValid) { return; }

            string s_item_name = e.CommandArgument.ToString();

            Button btn_supplier = (Button)sender;
            HtmlGenericControl div_supplier = (HtmlGenericControl)btn_supplier.Parent;

            DropDownList ddl_group = (DropDownList)div_supplier.FindControl("ddlGroup");
            TextBox txt_supplier = (TextBox)div_supplier.FindControl("txtSupplier");

            int n_supplier_group_id = 0;
            int.TryParse(ddl_group.SelectedValue, out n_supplier_group_id);

            long n_supplier_number = 0;
            long.TryParse(txt_supplier.Text.Trim(), out n_supplier_number);

            if (n_supplier_group_id == 0 || n_supplier_number == 0) { return; }

            SqlCommand o_command = new SqlCommand("sprUser" + s_item_name + "_SupplierInsert", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@ID", SqlDbType.Int);
            o_command.Parameters["@ID"].Direction = ParameterDirection.Output;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_user.ID;

            o_command.Parameters.Add("@SupplierGroupID", SqlDbType.Int);
            o_command.Parameters["@SupplierGroupID"].Value = n_supplier_group_id;

            o_command.Parameters.Add("@SupplierNumber", SqlDbType.BigInt);
            o_command.Parameters["@SupplierNumber"].Value = n_supplier_number;

            try
            {
                o_command.Connection.Open();
                o_command.Parameters["@ID"].Value = o_command.ExecuteScalar();

                int n = Convert.ToInt32(o_command.Parameters["@ID"].Value);
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.Text = "Error on update Supplier Number.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            Response.Redirect("UserSettings.aspx?result=updated&item=" + s_item_name + "_supplier");
        }

        protected void btnDataSource_Save(object sender, CommandEventArgs e)
        {
            if (!Page.IsValid) { return; }

            int n_id = 0;
            int.TryParse(hidDataSourceID.Value, out n_id);

            string s_name = txtDataSourceName.Text.Trim();

            string s_procedure = "";

            if (n_id == 0) { s_procedure = "sprDataSourceInsert"; } else { s_procedure = "sprDataSourceUpdate"; }

            SqlCommand o_command = new SqlCommand(s_procedure, DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            if (n_id > 0)
            {
                o_command.Parameters.Add("@ID", SqlDbType.Int);
                o_command.Parameters["@ID"].Value = n_id;
            }

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_user.ID;

            o_command.Parameters.Add("@DataSourceName", SqlDbType.NVarChar, 50);
            o_command.Parameters["@DataSourceName"].Value = s_name;

            try
            {
                o_command.Connection.Open();

                if (n_id == 0)
                {
                    n_id = (int)o_command.ExecuteScalar();
                }
                else
                {
                    o_command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.Message;
                lblError.Text = "Error on update data source.";
                return;
            }
            finally
            {
                o_command.Connection.Close();
                o_command.Dispose();
            }

            Response.Redirect("UserSettings.aspx?result=updated&item=data_source");
        }

        private void Bind_Bank()
        {
            DataTable o_data_table = new DataTable();

            string s_error = "";

            DB.Bind_Data_Table("sprBank", ref o_data_table, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            if (o_data_table.Rows.Count == 0) { return; }

            repBank.DataSource = o_data_table;
            repBank.DataBind();
        }

        private void Bind_Branch(int n_bank_id, ref CheckBoxList o_check_list)
        {
            string s_error = "";

            SqlCommand o_command = new SqlCommand("sprBankBranch_Selected", DB.Get_Connection());
            o_command.CommandType = CommandType.StoredProcedure;

            o_command.Parameters.Add("@UserID", SqlDbType.Int);
            o_command.Parameters["@UserID"].Value = o_user.ID;

            o_command.Parameters.Add("@BankID", SqlDbType.Int);
            o_command.Parameters["@BankID"].Value = n_bank_id;

            DB.Bind_Check_list(o_command, ref o_check_list, ref s_error);

            if (o_check_list.Items.Count == 0) { return; }

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }
        }

        private void Bind_Currency()
        {
            DataTable o_data_table = new DataTable();

            string s_error = "";

            DB.Bind_Data_Table("sprCurrency_Selected", ref o_data_table, ref s_error, "@UserID", o_user.ID.ToString());

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            if (o_data_table.Rows.Count == 0) { return; }

            repCurrency.DataSource = o_data_table;
            repCurrency.DataBind();
        }

        private void Bind_Credit()
        {
            DataTable o_data_table = new DataTable();

            string s_error = "";

            DB.Bind_Data_Table("sprUserCredit", ref o_data_table, ref s_error, "@UserID", o_user.ID.ToString());

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            if (o_data_table.Rows.Count == 0) { return; }

            repCredit.DataSource = o_data_table;
            repCredit.DataBind();
        }

        private void Bind_Discount()
        {
            DataTable o_data_table = new DataTable();

            string s_error = "";

            DB.Bind_Data_Table("sprUserDiscount", ref o_data_table, ref s_error, "@UserID", o_user.ID.ToString());

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            if (o_data_table.Rows.Count == 0) { return; }

            repDiscount.DataSource = o_data_table;
            repDiscount.DataBind();
        }

        private void Bind_DataSource()
        {
            DataTable o_data_table = new DataTable();

            string s_error = "";

            DB.Bind_Data_Table("sprDataSource", ref o_data_table, ref s_error, "@UserID", o_user.ID.ToString());

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            if (o_data_table.Rows.Count == 0) { return; }

            repDataSource.DataSource = o_data_table;
            repDataSource.DataBind();
        }

        private void Bind_Item_Supplier_Group(string s_item_name, int n_user_item_id, HtmlGenericControl ul_supplier_group, DropDownList ddl_group)
        {
            DataTable o_data_table = new DataTable();

            string s_error = "", s_procedure = "sprUser" + s_item_name + "_SupplierGroupSelect", s_parameter_name = "@User" + s_item_name + "ID";

            DB.Bind_Data_Table(s_procedure, ref o_data_table, ref s_error, s_parameter_name, n_user_item_id.ToString());

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            foreach (DataRow o_data_row in o_data_table.Rows)
            {
                int n_supplier_group_id = 0;
                int.TryParse(o_data_row["ID"].ToString(), out n_supplier_group_id);

                int n_supplier_count = 0;
                int.TryParse(o_data_row["SupplierCount"].ToString(), out n_supplier_count);

                string s_supplier_group_number = o_data_row["SupplierGroupNumber"].ToString();

                ListItem o_list_item = new ListItem();

                o_list_item.Value = n_supplier_group_id.ToString();
                o_list_item.Text = s_supplier_group_number;
                ddl_group.Items.Add(o_list_item);

                HtmlGenericControl li_supplier_group = new HtmlGenericControl("li");

                li_supplier_group.InnerHtml = s_supplier_group_number;
                ul_supplier_group.Controls.Add(li_supplier_group);

                if (n_supplier_count > 0)
                {
                    HtmlGenericControl ul_supplier = new HtmlGenericControl("ul");

                    Bind_Item_Supplier(s_item_name, n_supplier_group_id, ul_supplier);

                    li_supplier_group.Controls.Add(ul_supplier);
                }
            }
        }

        private void Bind_Item_Supplier(string s_item_name, int n_supplier_group_id, HtmlGenericControl ul_supplier)
        {
            DataTable o_data_table = new DataTable();

            string s_error = "";

            DB.Bind_Data_Table("sprUser" + s_item_name + "_SupplierSelect", ref o_data_table, ref s_error, "@SupplierGroupID", n_supplier_group_id.ToString());

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            foreach (DataRow o_data_row in o_data_table.Rows)
            {
                string s_supplier_number = o_data_row["SupplierNumber"].ToString();

                HtmlGenericControl li_supplier = new HtmlGenericControl("li");

                li_supplier.InnerHtml = s_supplier_number;
                ul_supplier.Controls.Add(li_supplier);
            }
        }
    }
}
