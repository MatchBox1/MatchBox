using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public partial class StrategyForm : UserControl
    {
        public int StrategyID { get; set; }

        private int n_user_id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            n_user_id = ((UserModel)Session["User"]).ID;

            if (Page.IsPostBack) { return; }

            if (Request.QueryString["result"] != null && Request.QueryString.Get("result") == "updated") { lblMessage.Text = "Item successfully updated."; }

            StrategyModel o_strategy = new StrategyModel();

            o_strategy.UserID = n_user_id;

            if (StrategyID > 0)
            {
                o_strategy.ID = StrategyID;

                StrategyAction.Select(ref o_strategy);
            }
            else
            {
                StrategyAction.Select_Template(ref o_strategy);
            }

            if (o_strategy.ErrorMessage != "")
            {
                lblError.Text = o_strategy.ErrorMessage;
                return;
            }

            Bind_Form(o_strategy);

            txtStrategyName.Focus();
        }

        protected void repTemplate_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Header && e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) { return; }

            DataTable dt_template_field = (DataTable)ViewState["TableTemplateField"];
            DataTable dt_data_field = (DataTable)ViewState["TableDataField"];

            if (e.Item.ItemType == ListItemType.Header)
            {
                foreach (DataRow dr_data_field in dt_data_field.Rows)
                {
                    string s_field_name = dr_data_field["FieldName"].ToString();
                    string s_field_description = dr_data_field["FieldDescription"].ToString();

                    Label o_label = (Label)e.Item.FindControl("lbl" + s_field_name);

                    if (o_label != null) { o_label.Text = s_field_description; }
                }
            }

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView o_data_row = (DataRowView)e.Item.DataItem;

                int n_id = Convert.ToInt32(o_data_row["ID"]);

                bool b_selected = Convert.ToInt16(o_data_row["IsSelected"]) > 0;

                CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("chkIsSelected");

                chkIsSelected.Checked = b_selected;

                foreach (DataRow dr_template_field in dt_template_field.Select(" TemplateID = " + n_id))
                {
                    CheckBox o_checkbox = (CheckBox)e.Item.FindControl("chk" + dr_template_field["FieldFromDB"]);

                    o_checkbox.Checked = true;
                }

                CheckBox chkCreditBrand = (CheckBox)e.Item.FindControl("chkCreditBrand");
                CheckBox chkCardBrand = (CheckBox)e.Item.FindControl("chkCardBrand");
                CheckBox chkTransactionCurrency = (CheckBox)e.Item.FindControl("chkTransactionCurrency");
                CheckBox chkCardNumber = (CheckBox)e.Item.FindControl("chkCardNumber");
                CheckBox chkPaymentsCount = (CheckBox)e.Item.FindControl("chkPaymentsCount");
                CheckBox chkDutyPaymentNumber = (CheckBox)e.Item.FindControl("chkDutyPaymentNumber");
                CheckBox chkDutyPaymentAmount = (CheckBox)e.Item.FindControl("chkDutyPaymentAmount");
                CheckBox chkPaymentDate = (CheckBox)e.Item.FindControl("chkPaymentDate");

                chkCreditBrand.Checked = true;
                chkCardBrand.Checked = true;
                chkTransactionCurrency.Checked = true;
                chkCardNumber.Checked = true;
                chkPaymentsCount.Checked = true;
                chkDutyPaymentNumber.Checked = true;
                chkDutyPaymentAmount.Checked = true;
                chkPaymentDate.Checked = true;

                string s_data_source = o_data_row["DataSource"].ToString();

                if (s_data_source == "Inside")
                {
                    CheckBox chkTerminalNumber = (CheckBox)e.Item.FindControl("chkTerminalNumber");
                    CheckBox chkSupplierNumber = (CheckBox)e.Item.FindControl("chkSupplierNumber");

                    chkTerminalNumber.Checked = true;
                    chkSupplierNumber.Checked = true;
                }
            }
        }

        protected void cvTemplate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool b_checked_inside = false, b_checked_outside = false;

            lblError.Text = "";

            foreach (RepeaterItem o_item in repTemplate.Items)
            {
                if (o_item.ItemType == ListItemType.Item || o_item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox chkIsSelected = (CheckBox)o_item.FindControl("chkIsSelected");

                    if (chkIsSelected.Checked == true)
                    {
                        Label lblDataSource = (Label)o_item.FindControl("lblDataSource");

                        if (lblDataSource.Text == "Inside")
                        {
                            b_checked_inside = true;
                        }
                        else
                        {
                            b_checked_outside = true;
                        }
                    }

                    if (b_checked_inside == true && b_checked_outside == true) { break; }
                }
            }

            bool b_valid = (b_checked_inside == true && b_checked_outside == true);

            if (b_valid == false)
            {
                lblError.Text = "Must select minimum one row from inside templates and one row from outside templates.";
            }

            args.IsValid = b_valid;
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) { return; }

            double n_strategy_percent = 0;
            double.TryParse(txtQueryPercent.Text.Trim(), out n_strategy_percent);

            if (n_strategy_percent <= 0 || n_strategy_percent > 100) { n_strategy_percent = 100; }

            n_strategy_percent = Math.Round(n_strategy_percent, 2);

            StrategyModel o_strategy = new StrategyModel();

            o_strategy.ID = StrategyID;
            o_strategy.UserID = n_user_id;
            o_strategy.QueryPercent = n_strategy_percent;
            o_strategy.StrategyName = txtStrategyName.Text.Trim();

            List<string> lst_field_inside = new List<string>();
            List<string> lst_field_outside = new List<string>();

            // REFORMAT TableStrategyTemplate
            o_strategy.TableStrategyTemplate = new DataTable();
            o_strategy.TableStrategyTemplate.Columns.Add("ID");

            int n_priority_count = 0, n_tolerance_count = 0;

            foreach (RepeaterItem o_item in repTemplate.Items)
            {
                if (o_item.ItemType == ListItemType.Item || o_item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox chkIsSelected = (CheckBox)o_item.FindControl("chkIsSelected");

                    if (chkIsSelected.Checked == false) { continue; }

                    HiddenField hidTemplateID = (HiddenField)o_item.FindControl("hidTemplateID");
                    Label lblDataSource = (Label)o_item.FindControl("lblDataSource");

                    int n_id = Convert.ToInt32(hidTemplateID.Value);
                    string s_source = lblDataSource.Text;

                    // Strategy Template

                    DataRow dr_strategy_template = o_strategy.TableStrategyTemplate.NewRow();

                    dr_strategy_template["ID"] = n_id;

                    o_strategy.TableStrategyTemplate.Rows.Add(dr_strategy_template);

                    // Strategy Field

                    List<string> lst_strategy_field = StrategyModel.List_Strategy_Field();

                    foreach (string s_srategy_field in lst_strategy_field)
                    {
                        // Continue if s_srategy_field already exists

                        if (o_strategy.TableStrategyField.Select(" StrategyField = '" + s_srategy_field + "' ").Length > 0) { continue; }

                        CheckBox o_checkbox_template = (CheckBox)o_item.FindControl("chk" + s_srategy_field);
                        CheckBox o_checkbox_strategy = (CheckBox)tblField.FindControl("chk" + s_srategy_field);

                        bool b_include = (o_checkbox_template != null && o_checkbox_template.Checked == true && (tblField.Visible == false || o_checkbox_strategy.Checked == true));

                        if (b_include == true)
                        {
                            if (s_source == "Inside")
                            {
                                if (lst_field_inside.Contains(s_srategy_field) == false) { lst_field_inside.Add(s_srategy_field); }
                            }
                            else
                            {
                                if (lst_field_outside.Contains(s_srategy_field) == false) { lst_field_outside.Add(s_srategy_field); }
                            }

                            if (lst_field_inside.Contains(s_srategy_field) == true && lst_field_outside.Contains(s_srategy_field) == true)
                            {
                                DataRow dr_strategy_field = o_strategy.TableStrategyField.NewRow();

                                TextBox txt_priority = (TextBox)tblField.FindControl("txtPriority" + s_srategy_field);
                                TextBox txt_tolerance_1 = (TextBox)tblField.FindControl("txtTolerance" + s_srategy_field + "_1");
                                TextBox txt_tolerance_2 = (TextBox)tblField.FindControl("txtTolerance" + s_srategy_field + "_2");
                                TextBox txt_tolerance_3 = (TextBox)tblField.FindControl("txtTolerance" + s_srategy_field + "_3");

                                int n_priority = 0;
                                float n_tolerance_1 = 0, n_tolerance_2 = 0, n_tolerance_3 = 0;

                                if (txt_priority != null) { int.TryParse(txt_priority.Text, out n_priority); }
                                if (txt_tolerance_1 != null) { float.TryParse(txt_tolerance_1.Text, out n_tolerance_1); }
                                if (txt_tolerance_2 != null) { float.TryParse(txt_tolerance_2.Text, out n_tolerance_2); }
                                if (txt_tolerance_3 != null) { float.TryParse(txt_tolerance_3.Text, out n_tolerance_3); }

                                if (s_srategy_field != "TransactionGrossAmount" && s_srategy_field != "DutyPaymentAmount")
                                {
                                    n_tolerance_1 = (int)n_tolerance_1;
                                    n_tolerance_2 = (int)n_tolerance_2;
                                    n_tolerance_3 = (int)n_tolerance_3;
                                }

                                // n_priority_count

                                if (n_priority > 0)
                                {
                                    n_priority_count++;

                                    if (n_priority_count > StrategyModel.MAX_PRIORITY)
                                    {
                                        lblError.Text = String.Format("Can't insert more then {0} fields in Priority.", StrategyModel.MAX_PRIORITY);
                                        return;
                                    }

                                    if (o_strategy.TableStrategyField.Select(" Priority = " + n_priority).Length > 0)
                                    {
                                        lblError.Text = "Can't insert dublicated values in Priority.";
                                        return;
                                    }
                                }

                                List<float> lst_tolerance = new List<float>();

                                if (n_tolerance_1 > 0) { lst_tolerance.Add(n_tolerance_1); }
                                if (n_tolerance_2 > 0) { lst_tolerance.Add(n_tolerance_2); }
                                if (n_tolerance_3 > 0) { lst_tolerance.Add(n_tolerance_3); }

                                n_tolerance_count += lst_tolerance.Count;

                                if (n_tolerance_count > StrategyModel.MAX_TOLERANCE)
                                {
                                    lblError.Text = String.Format("Can't insert more then {0} fields in Tolerance.", StrategyModel.MAX_TOLERANCE);
                                    return;
                                }

                                lst_tolerance.Sort();

                                if (lst_tolerance.Count > 0) { n_tolerance_1 = lst_tolerance[0]; }
                                if (lst_tolerance.Count > 1) { n_tolerance_2 = lst_tolerance[1]; }
                                if (lst_tolerance.Count > 2) { n_tolerance_3 = lst_tolerance[2]; }

                                if (n_tolerance_2 > 0)
                                {
                                    if (n_tolerance_1 == 0)
                                    {
                                        n_tolerance_1 = n_tolerance_2;
                                        n_tolerance_2 = 0;
                                    }
                                    else if (n_tolerance_1 == n_tolerance_2)
                                    {
                                        n_tolerance_2 = 0;
                                    }
                                }

                                if (n_tolerance_3 > 0)
                                {
                                    if (n_tolerance_1 == 0)
                                    {
                                        n_tolerance_1 = n_tolerance_3;
                                        n_tolerance_3 = 0;
                                    }
                                    else if (n_tolerance_1 == n_tolerance_3)
                                    {
                                        n_tolerance_3 = 0;
                                    }
                                }

                                if (n_tolerance_3 > 0)
                                {
                                    if (n_tolerance_2 == 0)
                                    {
                                        n_tolerance_2 = n_tolerance_3;
                                        n_tolerance_3 = 0;
                                    }
                                    else if (n_tolerance_2 == n_tolerance_3)
                                    {
                                        n_tolerance_3 = 0;
                                    }
                                }

                                dr_strategy_field["Priority"] = n_priority;
                                dr_strategy_field["Tolerance_1"] = n_tolerance_1;
                                dr_strategy_field["Tolerance_2"] = n_tolerance_2;
                                dr_strategy_field["Tolerance_3"] = n_tolerance_3;
                                dr_strategy_field["StrategyField"] = s_srategy_field;

                                o_strategy.TableStrategyField.Rows.Add(dr_strategy_field);
                            }
                        }
                    }
                }
            }

            StrategyAction.Update(ref o_strategy);

            if (o_strategy.ErrorMessage != "")
            {
                lblError.Text = o_strategy.ErrorMessage;
                return;
            }

            string s_guid = Request.QueryString.Get("q");
            string s_result = "updated";

            if (StrategyID == 0)
            {
                s_guid = Guid.NewGuid().ToString();
                s_result = "inserted";

                ((Dictionary<string, int>)Session["StrategyLookupTable"]).Add(s_guid, o_strategy.ID);
            }

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}", s_result, Request.QueryString.Get("i"), s_guid));
        }

        private void Bind_Form(StrategyModel o_strategy)
        {
            ViewState["TableTemplateField"] = o_strategy.TableTemplateField;
            ViewState["TableDataField"] = o_strategy.TableDataField;

            repTemplate.DataSource = o_strategy.TableStrategyTemplate;
            repTemplate.DataBind();

            if (o_strategy.ID == 0) { return; }

            if (o_strategy.StatusID == 1 || o_strategy.StatusID == 2)
            {
                btnSaveTop.Enabled = false;
                btnSaveBottom.Enabled = false;
            }

            txtStrategyName.Text = o_strategy.StrategyName;
            txtQueryPercent.Text = o_strategy.QueryPercent.ToString();

            lblStatusTop.Text = o_strategy.StatusName;
            lblStatusBottom.Text = o_strategy.StatusName;

            divField.Visible = true;

            lblMaxPriority.Text = StrategyModel.MAX_PRIORITY.ToString();
            lblMaxTolerance.Text = StrategyModel.MAX_TOLERANCE.ToString();

            Dictionary<string, string> dic_data_field = new Dictionary<string, string>();

            DataTable dt_strategy_field_tolerance = new DataTable();

            foreach (DataRow dr_data_field in o_strategy.TableDataField.Rows)
            {
                string s_field_name = dr_data_field["FieldName"].ToString();
                string s_field_description = dr_data_field["FieldDescription"].ToString();

                dic_data_field.Add(s_field_name, s_field_description);

                Label o_label = (Label)tblField.FindControl("lbl" + s_field_name);

                if (o_label == null) { continue; }

                o_label.Text = s_field_description;

                CheckBox chk_selected = (CheckBox)tblField.FindControl("chk" + s_field_name);
                TextBox txt_priority = (TextBox)tblField.FindControl("txtPriority" + s_field_name);
                TextBox txt_tolerance_1 = (TextBox)tblField.FindControl("txtTolerance" + s_field_name + "_1");
                TextBox txt_tolerance_2 = (TextBox)tblField.FindControl("txtTolerance" + s_field_name + "_2");
                TextBox txt_tolerance_3 = (TextBox)tblField.FindControl("txtTolerance" + s_field_name + "_3");

                DataRow dr_strategy_field = o_strategy.TableStrategyField.Select(" StrategyField = '" + s_field_name + "' ").FirstOrDefault();

                if (dr_strategy_field != null)
                {
                    chk_selected.Checked = true;

                    int n_priority = 0;
                    int.TryParse(dr_strategy_field["Priority"].ToString(), out n_priority);

                    if (n_priority > 0 && txt_priority != null) { txt_priority.Text = n_priority.ToString(); }

                    float n_tolerance_1 = 0;
                    float.TryParse(dr_strategy_field["Tolerance_1"].ToString(), out n_tolerance_1);

                    if (n_tolerance_1 > 0 && txt_tolerance_1 != null) { txt_tolerance_1.Text = n_tolerance_1.ToString(); }

                    float n_tolerance_2 = 0;
                    float.TryParse(dr_strategy_field["Tolerance_2"].ToString(), out n_tolerance_2);

                    if (n_tolerance_2 > 0 && txt_tolerance_2 != null) { txt_tolerance_2.Text = n_tolerance_2.ToString(); }

                    float n_tolerance_3 = 0;
                    float.TryParse(dr_strategy_field["Tolerance_3"].ToString(), out n_tolerance_3);

                    if (n_tolerance_3 > 0 && txt_tolerance_3 != null) { txt_tolerance_3.Text = n_tolerance_3.ToString(); }
                }
                else
                {
                    if (txt_priority != null) { txt_priority.Enabled = false; }
                    if (txt_tolerance_1 != null) { txt_tolerance_1.Enabled = false; }
                    if (txt_tolerance_2 != null) { txt_tolerance_2.Enabled = false; }
                    if (txt_tolerance_3 != null) { txt_tolerance_3.Enabled = false; }
                }
            }

            // IF CardNumber EXISTS THEN Select_Query FUNCTION CREATE ADITIONAL QUERY FOR CardNumber WITHOUT CardPrefix IN LOWER PRIORITY.

            bool b_add_card_prefix = (o_strategy.TableStrategyField.Select(" StrategyField = 'CardNumber' ").FirstOrDefault() != null);

            // GET dt_query

            DataTable dt_query = new DataTable();

            StrategyAction.Select_Query(o_strategy.TableStrategyField, b_add_card_prefix, true, ref dt_query);

            int n_query_count = dt_query.Select(" QueryNumber <> '-' ").Length;

            if (n_query_count > 0)
            {
                dt_query.Columns["QueryNumber"].ColumnName = "#";

                for (int i = 1; i < dt_query.Columns.Count; i++)
                {
                    dt_query.Columns[i].ColumnName = dic_data_field[dt_query.Columns[i].ColumnName];
                }

                divQuery.Visible = true;

                gvQuery.DataSource = dt_query;
                gvQuery.DataBind();

                double n_last_query_double = (double)n_query_count / (double)100 * o_strategy.QueryPercent;

                int n_last_query = (int)Math.Round(n_last_query_double, 0);

                if (n_last_query > dt_query.Rows.Count) { n_last_query = dt_query.Rows.Count; }

                lblQueryInfo.Text = String.Format("Total queries: {0}.<br />Last query number: {1} ({2}).", n_query_count, n_last_query, n_last_query_double);
            }
        }
    }
}
