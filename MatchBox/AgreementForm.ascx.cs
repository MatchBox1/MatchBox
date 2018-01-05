using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public partial class AgreementForm : UserControl
    {
        public int AgreementID { get; set; }

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

                if (arr_result.Length == 3)     // If Update Supplier Table
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

                if (s_result.Contains("supplier"))
                {
                    lblMessageSupplier.Text = s_message;
                }
                else
                {
                    lblMessage.Text = s_message;
                }
            }

            AgreementModel o_agreement = new AgreementModel();

            if (AgreementID > 0)
            {
                o_agreement.ID = AgreementID;
                o_agreement.UserID = n_user_id;

                AgreementAction.Select(ref o_agreement);

                if (o_agreement.ErrorMessage != "")
                {
                    lblError.Text = o_agreement.ErrorMessage;
                    return;
                }
            }

            Bind_Form(o_agreement);

            ddlCompany.Focus();
        }

        protected void repSupplierGroup_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            DataTable dt_supplier = (DataTable)ViewState["TableSupplier"];

            if (dt_supplier.Rows.Count == 0) { return; }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView o_data_row = (DataRowView)e.Item.DataItem;

                int n_supplier_group_id = 0;
                int.TryParse(o_data_row["ID"].ToString(), out n_supplier_group_id);

                DataRow[] dr_supplier = dt_supplier.Select("SupplierGroupID = " + n_supplier_group_id);

                if (dr_supplier.Length > 0)
                {
                    Repeater repSupplier = (Repeater)e.Item.FindControl("repSupplier");

                    repSupplier.DataSource = dr_supplier.CopyToDataTable();
                    repSupplier.DataBind();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) { return; }

            AgreementModel o_agreement = new AgreementModel();

            o_agreement.ID = AgreementID;
            o_agreement.UserID = n_user_id;
            o_agreement.CompanyID = Convert.ToInt32(ddlCompany.SelectedValue);
            o_agreement.CreditID = Convert.ToInt32(ddlCredit.SelectedValue);

            AgreementAction.Update(ref o_agreement);

            if (o_agreement.ErrorMessage != "")
            {
                lblError.Text = o_agreement.ErrorMessage;
                return;
            }

            string s_guid = Request.QueryString.Get("q");
            string s_result = "updated";

            if (AgreementID == 0)
            {
                s_guid = Guid.NewGuid().ToString();
                s_result = "inserted";

                ((Dictionary<string, int>)Session["AgreementLookupTable"]).Add(s_guid, o_agreement.ID);
            }

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}", s_result, Request.QueryString.Get("i"), s_guid));
        }

        protected void btnSaveSupplierGroup_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) { return; }

            SupplierGroupModel o_supplier_group = new SupplierGroupModel();

            o_supplier_group.ID = 0;  // ONLY ADD NEW AVAILABLE
            o_supplier_group.UserID = n_user_id;
            o_supplier_group.AgreementID = AgreementID;
            o_supplier_group.SupplierGroupNumber = Convert.ToInt64(txtSupplierGroupNumber.Text.Trim());
            o_supplier_group.SupplierGroupName = txtSupplierGroupName.Text.Trim();

            SupplierGroupAction.Update(ref o_supplier_group);

            if (o_supplier_group.ErrorMessage != "")
            {
                lblErrorSupplier.Text = o_supplier_group.ErrorMessage;
                return;
            }

            if (o_supplier_group.ID == 0)
            {
                lblErrorSupplier.Text = "No item was updated.";
                return;
            }

            string s_interface = Request.QueryString.Get("i");
            string s_guid = Request.QueryString.Get("q");

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}", "updated-supplier", s_interface, s_guid));
        }

        protected void btnSaveSupplier_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) { return; }

            SupplierModel o_supplier = new SupplierModel();

            o_supplier.ID = 0;  // ONLY ADD NEW AVAILABLE
            o_supplier.UserID = n_user_id;
            o_supplier.AgreementID = AgreementID;
            o_supplier.SupplierGroupID = Convert.ToInt32(ddlSupplierGroup.SelectedValue);
            o_supplier.SupplierNumber = Convert.ToInt64(txtSupplierNumber.Text.Trim());
            o_supplier.SupplierName = txtSupplierName.Text.Trim();

            SupplierAction.Update(ref o_supplier);

            if (o_supplier.ErrorMessage != "")
            {
                lblErrorSupplier.Text = o_supplier.ErrorMessage;
                return;
            }

            if (o_supplier.ID == 0)
            {
                lblErrorSupplier.Text = "No item was updated.";
                return;
            }

            string s_interface = Request.QueryString.Get("i");
            string s_guid = Request.QueryString.Get("q");

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}", "updated-supplier", s_interface, s_guid));
        }

        protected void btnUploadSupplier_Click(object sender, EventArgs e)
        {
            string s_error = "";

            string s_file_name = ExcelAction.Save(fuSupplierUpload, ref s_error);

            if (s_error != "")
            {
                lblErrorSupplier.Text = s_error;
                return;
            }

            int n_rows_affected = 0;

            string s_path = ExcelAction.ExcelFolder + "\\" + s_file_name;

            DataTable dt_group_and_supplier = new DataTable();

            ExcelAction.Bind_Data_Table(s_path, ref dt_group_and_supplier, ref s_error);

            if (s_error != "") { goto Finish; }

            List<string> lst_column = new List<string>();

            lst_column.Add("SupplierGroupNumber");
            lst_column.Add("SupplierGroupName");
            lst_column.Add("SupplierNumber");
            lst_column.Add("SupplierName");

            if (dt_group_and_supplier.Columns.Count != lst_column.Count)
            {
                s_error = String.Format("File must contain {0} column/s.", lst_column.Count);
                goto Finish;
            }

            foreach (DataColumn dr_group_and_supplier in dt_group_and_supplier.Columns)
            {
                if (lst_column.Contains(dr_group_and_supplier.ColumnName) == false)
                {
                    s_error = String.Format("File must contain following column/s: {0}.", String.Join(", ", lst_column.ToArray()));
                    goto Finish;
                }
            }

            dt_group_and_supplier = new DataView(dt_group_and_supplier).ToTable(true, lst_column.ToArray());

            for (int i = dt_group_and_supplier.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr_group_and_supplier = dt_group_and_supplier.Rows[i];

                long n_group_number = 0, n_supplier_number = 0;

                long.TryParse(dr_group_and_supplier["SupplierGroupNumber"].ToString(), out n_group_number);
                long.TryParse(dr_group_and_supplier["SupplierNumber"].ToString(), out n_supplier_number);

                if (n_group_number == 0 || n_supplier_number == 0 || dt_group_and_supplier.Select(" SupplierNumber = " + n_supplier_number).Length > 1)
                {
                    dt_group_and_supplier.Rows.RemoveAt(i);
                }
                else
                {
                    string s_group_name = dr_group_and_supplier["SupplierGroupName"].ToString().Trim();
                    string s_supplier_name = dr_group_and_supplier["SupplierName"].ToString().Trim();

                    if (s_group_name.Length > 20) { s_group_name = s_group_name.Substring(0, 20); }
                    if (s_supplier_name.Length > 20) { s_supplier_name = s_supplier_name.Substring(0, 20); }

                    dr_group_and_supplier["SupplierGroupNumber"] = n_group_number;
                    dr_group_and_supplier["SupplierGroupName"] = s_group_name;
                    dr_group_and_supplier["SupplierNumber"] = n_supplier_number;
                    dr_group_and_supplier["SupplierName"] = s_supplier_name;
                }
            }

            // UPDATE SUPPLIER GROUP

            List<string> lst_group_column = new List<string>();

            lst_group_column.Add("SupplierGroupNumber");
            lst_group_column.Add("SupplierGroupName");

            DataTable dt_group = new DataView(dt_group_and_supplier).ToTable(true, lst_group_column.ToArray());

            int n_group_rows_affected = 0;

            SupplierGroupAction.Update_Table(n_user_id, AgreementID, dt_group, ref n_group_rows_affected, ref s_error);

            if (s_error != "") { goto Finish; }

            if (n_group_rows_affected < 0) { n_group_rows_affected = 0; }

            // UPDATE SUPPLIER

            List<string> lst_supplier_column = new List<string>();

            lst_supplier_column.Add("SupplierGroupNumber");
            lst_supplier_column.Add("SupplierNumber");
            lst_supplier_column.Add("SupplierName");

            DataTable dt_supplier = new DataView(dt_group_and_supplier).ToTable(true, lst_supplier_column.ToArray());

            int n_supplier_rows_affected = 0;

            SupplierAction.Update_Table(n_user_id, AgreementID, dt_supplier, ref n_supplier_rows_affected, ref s_error);

            if (s_error != "") { goto Finish; }

            if (n_supplier_rows_affected < 0) { n_supplier_rows_affected = 0; }

            // ===

            n_rows_affected = n_group_rows_affected + n_supplier_rows_affected;

            if (n_rows_affected < 0) { n_rows_affected = 0; }

        Finish:

            ExcelAction.Remove(s_path, ref s_error);

            if (s_error != "")
            {
                lblErrorSupplier.Text = s_error;
            }
            else
            {
                Response.Redirect(String.Format("FrameForm.aspx?result=updated-supplier-{0}&i={1}&q={2}", n_rows_affected, Request.QueryString.Get("i"), Request.QueryString.Get("q")));
            }
        }

        protected void btnDeleteSupplierGroup_Command(object sender, CommandEventArgs e)
        {
            string s_error = "";

            SupplierGroupModel o_supplier_group = new SupplierGroupModel();

            o_supplier_group.ID = Convert.ToInt32(e.CommandArgument);
            o_supplier_group.UserID = n_user_id;
            o_supplier_group.AgreementID = AgreementID;

            bool b_deleted = SupplierGroupAction.Delete(o_supplier_group, ref s_error);

            if (s_error != "")
            {
                lblErrorSupplier.Text = s_error;
                return;
            }

            if (b_deleted == false)
            {
                lblErrorSupplier.Text = "No item was deleted.";
                return;
            }

            string s_interface = Request.QueryString.Get("i");
            string s_guid = Request.QueryString.Get("q");

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}", "deleted-supplier", s_interface, s_guid));
        }

        protected void btnDeleteSupplier_Command(object sender, CommandEventArgs e)
        {
            string s_error = "";

            SupplierModel o_supplier = new SupplierModel();

            o_supplier.ID = Convert.ToInt32(e.CommandArgument);
            o_supplier.UserID = n_user_id;
            o_supplier.AgreementID = AgreementID;

            bool b_deleted = SupplierAction.Delete(o_supplier, ref s_error);

            if (s_error != "")
            {
                lblErrorSupplier.Text = s_error;
                return;
            }

            if (b_deleted == false)
            {
                lblErrorSupplier.Text = "No item was deleted.";
                return;
            }

            string s_interface = Request.QueryString.Get("i");
            string s_guid = Request.QueryString.Get("q");

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}", "deleted-supplier", s_interface, s_guid));
        }

        private void Bind_Form(AgreementModel o_agreement)
        {
            string s_error = "";

            List<SqlParameter> o_company_parameters = new List<SqlParameter>();
            o_company_parameters.Add(new SqlParameter("@UserID", n_user_id));

            DB.Bind_List_Control(ddlCompany, "sprCompany", ref s_error, null, o_company_parameters);
            DB.Bind_List_Control(ddlCredit, "sprCredit", ref s_error);

            if (o_agreement.ID == 0) { return; }

            divSupplier_Change.Visible = true;

            ddlCompany.SelectedValue = o_agreement.CompanyID.ToString();
            ddlCredit.SelectedValue = o_agreement.CreditID.ToString();

            DataTable dt_supplier = new DataTable();

            SupplierAction.Select(ref dt_supplier, ref s_error, n_user_id, o_agreement.ID);

            if (s_error != "")
            {
                lblErrorSupplier.Text = s_error;
                return;
            }

            ViewState["TableSupplier"] = dt_supplier;

            DataTable dt_supplier_group = new DataTable();

            SupplierGroupAction.Select(ref dt_supplier_group, ref s_error, n_user_id, o_agreement.ID);

            if (s_error != "")
            {
                lblErrorSupplier.Text = s_error;
                return;
            }

            repSupplierGroup.DataSource = dt_supplier_group;
            repSupplierGroup.DataBind();

            ddlSupplierGroup.DataSource = dt_supplier_group;
            ddlSupplierGroup.DataValueField = "ID";
            ddlSupplierGroup.DataTextField = "SupplierGroup";
            ddlSupplierGroup.DataBind();
            ddlSupplierGroup.Items.Insert(0, new ListItem("Supplier Group", "0"));
        }
    }
}
