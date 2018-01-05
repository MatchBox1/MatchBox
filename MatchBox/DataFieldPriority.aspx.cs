using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public partial class DataFieldPriority : BasePage
    {
        private int n_user_id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            n_user_id = o_user.ID;

            if (Request.QueryString["result"] != null && Request.QueryString["result"] == "updated")
            {
                lblMessage.Text = "Item successfully updated.";
            }

            if (Page.IsPostBack) { return; }

            string s_error = "";

            DataTable dt_field_priority = new DataTable();
            DataTable dt_field_sort = new DataTable();
            DataTable dt_data_field = new DataTable();

            DataAction.Select_Settings(n_user_id, ref dt_field_priority, ref dt_field_sort, ref dt_data_field, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            Bind_Form(dt_field_priority, dt_field_sort, dt_data_field);
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            divMessage.Visible = lblMessage.Text != "" || lblError.Text != "";

            if (lblError.Text != "") { lblMessage.Text = ""; }
        }

        protected void Save_Settings(object sender, EventArgs e)
        {
            // GET INSIDE PRIORITY FIELDS

            DataTable dt_inside_field = new DataTable();
            dt_inside_field.Columns.Add("FieldName", typeof(string));
            dt_inside_field.Columns.Add("Priority", typeof(int));
            dt_inside_field.Columns.Add("Checked", typeof(bool));

            foreach (RepeaterItem o_item in repInsideFieldPriority.Items)
            {
                HiddenField hidFieldName = (HiddenField)o_item.FindControl("hidFieldName");
                TextBox txtPriority = (TextBox)o_item.FindControl("txtPriority");
                CheckBox chkDisplay = (CheckBox)o_item.FindControl("chkDisplay");

                int n_priority = 0;
                int.TryParse(txtPriority.Text, out n_priority);

                if (n_priority == 0) { n_priority = 99; }

                DataRow dr_inside_field = dt_inside_field.NewRow();

                dr_inside_field["FieldName"] = hidFieldName.Value;
                dr_inside_field["Priority"] = n_priority;
                dr_inside_field["Checked"] = chkDisplay.Checked;

                dt_inside_field.Rows.Add(dr_inside_field);
            }

            DataView dv_inside_field = dt_inside_field.DefaultView;
            dv_inside_field.Sort = "Checked DESC, Priority ASC";
            dt_inside_field = dv_inside_field.ToTable();

            List<string> lst_inside_priority = new List<string>();
            List<string> lst_inside_hidden = new List<string>();

            foreach(DataRow dr in dt_inside_field.Rows)
            {
                string s_field = dr["FieldName"].ToString();
                bool b_checked = Convert.ToBoolean(dr["Checked"]);

                lst_inside_priority.Add(s_field);

                if (b_checked == false) { lst_inside_hidden.Add(s_field); }
            }

            // GET OUTSIDE PRIORITY FIELDS

            DataTable dt_outside_field = new DataTable();
            dt_outside_field.Columns.Add("FieldName", typeof(string));
            dt_outside_field.Columns.Add("Priority", typeof(int));
            dt_outside_field.Columns.Add("Checked", typeof(bool));

            foreach (RepeaterItem o_item in repOutsideFieldPriority.Items)
            {
                HiddenField hidFieldName = (HiddenField)o_item.FindControl("hidFieldName");
                TextBox txtPriority = (TextBox)o_item.FindControl("txtPriority");
                CheckBox chkDisplay = (CheckBox)o_item.FindControl("chkDisplay");

                int n_priority = 0;
                int.TryParse(txtPriority.Text, out n_priority);

                if (n_priority == 0) { n_priority = 99; }

                DataRow dr_outside_field = dt_outside_field.NewRow();

                dr_outside_field["FieldName"] = hidFieldName.Value;
                dr_outside_field["Priority"] = n_priority;
                dr_outside_field["Checked"] = chkDisplay.Checked;

                dt_outside_field.Rows.Add(dr_outside_field);
            }

            DataView dv_outside_field = dt_outside_field.DefaultView;
            dv_outside_field.Sort = "Checked DESC, Priority ASC";
            dt_outside_field = dv_outside_field.ToTable();

            List<string> lst_outside_priority = new List<string>();
            List<string> lst_outside_hidden = new List<string>();

            foreach (DataRow dr in dt_outside_field.Rows)
            {
                string s_field = dr["FieldName"].ToString();
                bool b_checked = Convert.ToBoolean(dr["Checked"]);

                lst_outside_priority.Add(s_field);

                if (b_checked == false) { lst_outside_hidden.Add(s_field); }
            }

            // GET INSIDE & OUTSIDE SORT FIELDS

            string s_inside_sort = "", s_outside_sort = "";

            if (ddlInsideSort_1.SelectedValue != "")
            {
                s_inside_sort = ddlInsideSort_1.SelectedValue + " " + ddlInsideOrder_1.SelectedValue;
            }

            if (ddlInsideSort_2.SelectedValue != "" && ddlInsideSort_2.SelectedValue != ddlInsideSort_1.SelectedValue)
            {
                if (s_inside_sort != "") { s_inside_sort += ","; }

                s_inside_sort += ddlInsideSort_2.SelectedValue + " " + ddlInsideOrder_2.SelectedValue;
            }

            if (ddlInsideSort_3.SelectedValue != "" && ddlInsideSort_3.SelectedValue != ddlInsideSort_2.SelectedValue && ddlInsideSort_3.SelectedValue != ddlInsideSort_1.SelectedValue)
            {
                if (s_inside_sort != "") { s_inside_sort += ","; }

                s_inside_sort += ddlInsideSort_3.SelectedValue + " " + ddlInsideOrder_3.SelectedValue;
            }

            if (ddlOutsideSort_1.SelectedValue != "")
            {
                s_outside_sort = ddlOutsideSort_1.SelectedValue + " " + ddlOutsideOrder_1.SelectedValue;
            }

            if (ddlOutsideSort_2.SelectedValue != "" && ddlOutsideSort_2.SelectedValue != ddlOutsideSort_1.SelectedValue)
            {
                if (s_outside_sort != "") { s_outside_sort += ","; }

                s_outside_sort += ddlOutsideSort_2.SelectedValue + " " + ddlOutsideOrder_2.SelectedValue;
            }

            if (ddlOutsideSort_3.SelectedValue != "" && ddlOutsideSort_3.SelectedValue != ddlOutsideSort_2.SelectedValue && ddlOutsideSort_3.SelectedValue != ddlOutsideSort_1.SelectedValue)
            {
                if (s_outside_sort != "") { s_outside_sort += ","; }

                s_outside_sort += ddlOutsideSort_3.SelectedValue + " " + ddlOutsideOrder_3.SelectedValue;
            }

            // UPDATE

            string s_inside_priority = String.Join(",", lst_inside_priority.ToArray());
            string s_outside_priority = String.Join(",", lst_outside_priority.ToArray());

            string s_inside_hidden = String.Join(",", lst_inside_hidden.ToArray());
            string s_outside_hidden = String.Join(",", lst_outside_hidden.ToArray());

            string s_error = "";

            DataAction.Update_Settings(n_user_id, s_inside_priority, s_outside_priority, s_inside_hidden, s_outside_hidden, s_inside_sort, s_outside_sort, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            Response.Redirect("DataFieldPriority.aspx?result=updated");
        }

        protected void Default_Settings(object sender, EventArgs e)
        {
            string s_error = "";

            DataAction.Update_Settings(n_user_id, "", "", "", "", "", "", ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            Response.Redirect("DataFieldPriority.aspx?result=updated");
        }

        private void Bind_Drop_Down(DropDownList ddlFieldName, DataTable dt_settings, string s_value)
        {
            ddlFieldName.DataValueField = "FieldName";
            ddlFieldName.DataTextField = "FieldDescription";
            ddlFieldName.DataSource = dt_settings;
            ddlFieldName.DataBind();
            ddlFieldName.SelectedValue = s_value;
        }

        private void Bind_Form(DataTable dt_field_priority, DataTable dt_field_sort, DataTable dt_data_field)
        {
            string s_inside_field_priority = dt_field_priority.Rows[0]["InsideFieldPriority"].ToString();
            string s_outside_field_priority = dt_field_priority.Rows[0]["OutsideFieldPriority"].ToString();

            string s_inside_field_hidden = dt_field_priority.Rows[0]["InsideFieldHidden"].ToString();
            string s_outside_field_hidden = dt_field_priority.Rows[0]["OutsideFieldHidden"].ToString();

            string s_inside_sort_order = dt_field_sort.Rows[0]["InsideSortOrder"].ToString().Trim();
            string s_outside_sort_order = dt_field_sort.Rows[0]["OutsideSortOrder"].ToString().Trim();

            // INSIDE FIELD PRIORITY

            DataTable dt_inside_field_priority = new DataTable();

            dt_inside_field_priority.Columns.Add("Priority");
            dt_inside_field_priority.Columns.Add("FieldName");
            dt_inside_field_priority.Columns.Add("FieldDescription");
            dt_inside_field_priority.Columns.Add("Checked");

            string[] arr_inside_field_priority = s_inside_field_priority.Split(',');
            string[] arr_inside_field_hidden = s_inside_field_hidden.Split(',');

            for (int i = 0; i < arr_inside_field_priority.Length; i++)
            {
                string s_field_name = arr_inside_field_priority[i].Trim();
                string s_field_description = "";

                DataRow dr_data_field = dt_data_field.Select(" FieldName = '" + s_field_name + "' ").FirstOrDefault();

                if (dr_data_field != null)
                {
                    s_field_description = dr_data_field["FieldDescription"].ToString();
                }
                else
                {
                    s_field_description = s_field_name;
                }

                DataRow dr_inside_field_priority = dt_inside_field_priority.NewRow();

                dr_inside_field_priority["Priority"] = i + 1;
                dr_inside_field_priority["FieldName"] = s_field_name;
                dr_inside_field_priority["FieldDescription"] = s_field_description;
                dr_inside_field_priority["Checked"] = (arr_inside_field_hidden.Contains(s_field_name) == false);

                dt_inside_field_priority.Rows.Add(dr_inside_field_priority);
            }

            // OUTSIDE FIELD PRIORITY

            DataTable dt_outside_field_priority = new DataTable();

            dt_outside_field_priority.Columns.Add("Priority");
            dt_outside_field_priority.Columns.Add("FieldName");
            dt_outside_field_priority.Columns.Add("FieldDescription");
            dt_outside_field_priority.Columns.Add("Checked");

            string[] arr_outside_field_priority = s_outside_field_priority.Split(',');
            string[] arr_outside_field_hidden = s_outside_field_hidden.Split(',');

            for (int i = 0; i < arr_outside_field_priority.Length; i++)
            {
                string s_field_name = arr_outside_field_priority[i].Trim();
                string s_field_description = "";

                DataRow dr_data_field = dt_data_field.Select(" FieldName = '" + s_field_name + "' ").FirstOrDefault();

                if (dr_data_field != null)
                {
                    s_field_description = dr_data_field["FieldDescription"].ToString();
                }
                else
                {
                    s_field_description = s_field_name;
                }

                DataRow dr_outside_field_priority = dt_outside_field_priority.NewRow();

                dr_outside_field_priority["Priority"] = i + 1;
                dr_outside_field_priority["FieldName"] = s_field_name;
                dr_outside_field_priority["FieldDescription"] = s_field_description;
                dr_outside_field_priority["Checked"] = (arr_outside_field_hidden.Contains(s_field_name) == false);

                dt_outside_field_priority.Rows.Add(dr_outside_field_priority);
            }

            // SORT ORDER TABLES

            DataTable dt_inside_field_sort = dt_inside_field_priority.Copy();
            DataTable dt_outside_field_sort = dt_outside_field_priority.Copy();

            DataRow dr_inside_query_id = dt_inside_field_sort.Select(" FieldName = 'QueryID' AND FieldDescription = 'Match' ").FirstOrDefault();
            DataRow dr_inside_unique_id = dt_inside_field_sort.Select(" FieldName = 'UniqueID' AND FieldDescription = 'Original' ").FirstOrDefault();

            if (dr_inside_query_id != null) { dt_inside_field_sort.Rows.Remove(dr_inside_query_id); }
            if (dr_inside_unique_id != null) { dt_inside_field_sort.Rows.Remove(dr_inside_unique_id); }

            DataRow dr_outside_query_id = dt_outside_field_sort.Select(" FieldName = 'QueryID' AND FieldDescription = 'Match' ").FirstOrDefault();
            DataRow dr_outside_unique_id = dt_outside_field_sort.Select(" FieldName = 'UniqueID' AND FieldDescription = 'Original' ").FirstOrDefault();

            if (dr_outside_query_id != null) { dt_outside_field_sort.Rows.Remove(dr_outside_query_id); }
            if (dr_outside_unique_id != null) { dt_outside_field_sort.Rows.Remove(dr_outside_unique_id); }

            Bind_Drop_Down(ddlInsideSort_1, dt_inside_field_sort, "");
            Bind_Drop_Down(ddlInsideSort_2, dt_inside_field_sort, "");
            Bind_Drop_Down(ddlInsideSort_3, dt_inside_field_sort, "");

            ddlInsideSort_1.Items.Insert(0, new ListItem("", ""));
            ddlInsideSort_2.Items.Insert(0, new ListItem("", ""));
            ddlInsideSort_3.Items.Insert(0, new ListItem("", ""));

            Bind_Drop_Down(ddlOutsideSort_1, dt_outside_field_sort, "");
            Bind_Drop_Down(ddlOutsideSort_2, dt_outside_field_sort, "");
            Bind_Drop_Down(ddlOutsideSort_3, dt_outside_field_sort, "");

            ddlOutsideSort_1.Items.Insert(0, new ListItem("", ""));
            ddlOutsideSort_2.Items.Insert(0, new ListItem("", ""));
            ddlOutsideSort_3.Items.Insert(0, new ListItem("", ""));

            // INSIDE SORT ORDER

            string[] arr_inside_sort_order = s_inside_sort_order.Split(',');

            for (int i = 0; i < arr_inside_sort_order.Length; i++)
            {
                string[] arr_sort = arr_inside_sort_order[i].Trim().Split(' ');     // SPLIT BY SPACE

                switch (i)
                {
                    case 0:
                        ddlInsideSort_1.SelectedValue = arr_sort[0];
                        ddlInsideOrder_1.SelectedValue = arr_sort[1];
                        break;
                    case 1:
                        ddlInsideSort_2.SelectedValue = arr_sort[0];
                        ddlInsideOrder_2.SelectedValue = arr_sort[1];
                        break;
                    case 2:
                        ddlInsideSort_3.SelectedValue = arr_sort[0];
                        ddlInsideOrder_3.SelectedValue = arr_sort[1];
                        break;
                }
            }

            // OUTSIDE SORT ORDER

            string[] arr_outside_sort_order = s_outside_sort_order.Split(',');

            for (int i = 0; i < arr_outside_sort_order.Length; i++)
            {
                string[] arr_sort = arr_outside_sort_order[i].Trim().Split(' ');    // SPLIT BY SPACE

                switch (i)
                {
                    case 0:
                        ddlOutsideSort_1.SelectedValue = arr_sort[0];
                        ddlOutsideOrder_1.SelectedValue = arr_sort[1];
                        break;
                    case 1:
                        ddlOutsideSort_2.SelectedValue = arr_sort[0];
                        ddlOutsideOrder_2.SelectedValue = arr_sort[1];
                        break;
                    case 2:
                        ddlOutsideSort_3.SelectedValue = arr_sort[0];
                        ddlOutsideOrder_3.SelectedValue = arr_sort[1];
                        break;
                }
            }

            // BIND REPEATERS

            repInsideFieldPriority.DataSource = dt_inside_field_priority;
            repInsideFieldPriority.DataBind();

            repOutsideFieldPriority.DataSource = dt_outside_field_priority;
            repOutsideFieldPriority.DataBind();
        }
    }
}
