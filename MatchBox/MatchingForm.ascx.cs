using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public partial class MatchingForm : UserControl
    {
        public int MatchingID { get; set; }

        private int n_user_id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            n_user_id = ((UserModel)Session["User"]).ID;

            if (Page.IsPostBack) { return; }

            hedTitle.InnerHtml = "Matching #" + MatchingID;

            MatchingModel o_matching = new MatchingModel();

            if (MatchingID > 0)
            {
                if (Request.QueryString["result"] != null)
                {
                    string s_result = Request.QueryString.Get("result").Trim();

                    if (s_result.Contains("deleted"))
                    {
                        lblMessage.Text = "Item/s successfully deleted.";
                    }
                }

                o_matching.ID = MatchingID;
                o_matching.UserID = n_user_id;

                MatchingAction.Select(ref o_matching);

                if (o_matching.ErrorMessage != "")
                {
                    lblError.Text = o_matching.ErrorMessage;
                    return;
                }

                Bind_Form(o_matching);
            }
        }

        protected void gvMatchingSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            bool b_allow_delete = (bool)ViewState["AllowDelete"];

            if (e.Row.RowType == DataControlRowType.Header)
            {
                DataTable dt_data_field = (DataTable)ViewState["TableDataField"];

                foreach (TableCell o_cell in e.Row.Cells)
                {
                    string s_cell_text = o_cell.Text.Trim();

                    if (s_cell_text == "SelectQuery")
                    {
                        HtmlInputCheckBox chkSelectQuery = new HtmlInputCheckBox();

                        if (b_allow_delete == true)
                        {
                            chkSelectQuery.Attributes.Add("value", "0");
                            chkSelectQuery.Attributes.Add("class", "SelectQuery");
                            chkSelectQuery.Attributes.Add("onclick", "javascript: select_matching_query_all(this.checked);");
                        }
                        else
                        {
                            chkSelectQuery.Disabled = true;
                        }

                        o_cell.Controls.Add(chkSelectQuery);
                    }
                    else
                    {
                        switch (s_cell_text)
                        {
                            case "MatchingTypeName":
                                s_cell_text = "Matching Type";
                                break;
                            case "QueryNumber":
                                s_cell_text = "Query Number";
                                break;
                            case "InsideCount":
                                s_cell_text = "Inside Count";
                                break;
                            case "OutsideCount":
                                s_cell_text = "Outside Count";
                                break;
                            case "InsideAmount":
                                s_cell_text = "Inside Amount";
                                break;
                            case "OutsideAmount":
                                s_cell_text = "Outside Amount";
                                break;
                            default:
                                DataRow dr_data_field = dt_data_field.Select(" FieldName = '" + s_cell_text + "' ").FirstOrDefault();

                                if (dr_data_field != null)
                                {
                                    s_cell_text = dr_data_field["FieldDescription"].ToString();
                                }

                                break;
                        }

                        o_cell.Text = s_cell_text;
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // TableRow

                e.Row.Attributes.Add("class", "nowrap");

                // QueryNumber

                int n_query_number = 0;
                int.TryParse(e.Row.Cells[2].Text, out n_query_number);

                // SelectQuery

                TableCell tc_select_query = e.Row.Cells[0];

                HtmlInputCheckBox chkSelectQuery = new HtmlInputCheckBox();

                if (b_allow_delete == true)
                {
                    chkSelectQuery.Attributes.Add("value", n_query_number.ToString());
                    chkSelectQuery.Attributes.Add("class", "SelectQuery");
                    chkSelectQuery.Attributes.Add("onclick", "javascript: select_matching_query();");
                }
                else
                {
                    chkSelectQuery.Disabled = true;
                }

                tc_select_query.Controls.Add(chkSelectQuery);

                // InsideCount

                TableCell tc_inside_count = e.Row.Cells[3];

                int n_inside_count = 0;
                int.TryParse(tc_inside_count.Text, out n_inside_count);

                tc_inside_count.Text = String.Format("{0:n0}", n_inside_count);

                // OutsideCount

                TableCell tc_outside_count = e.Row.Cells[4];

                int n_outside_count = 0;
                int.TryParse(tc_outside_count.Text, out n_outside_count);

                tc_outside_count.Text = String.Format("{0:n0}", n_outside_count);

                // InsideAmount

                TableCell tc_inside_amount = e.Row.Cells[5];

                double n_inside_amount = 0;
                double.TryParse(tc_inside_amount.Text, out n_inside_amount);

                tc_inside_amount.Text = String.Format("{0:n2}", n_inside_amount);

                // OutsideAmount

                TableCell tc_outside_amount = e.Row.Cells[6];

                double n_outside_amount = 0;
                double.TryParse(tc_outside_amount.Text, out n_outside_amount);

                tc_outside_amount.Text = String.Format("{0:n2}", n_outside_amount);
            }
        }

        protected void btnMatchingQueryView_Click(object sender, EventArgs e)
        {
            DataTable dt_matching_inside = new DataTable();
            DataTable dt_matching_outside = new DataTable();

            string s_error = "";

            DataAction.Select_Query(n_user_id, MatchingID, hidMatchingQueryNumber.Value, ref dt_matching_inside, ref dt_matching_outside, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            string s_cache_inside_matching = String.Format("TableInside_Matching_{0}_{1}", n_user_id, MatchingID);
            string s_cache_outside_matching = String.Format("TableOutside_Matching_{0}_{1}", n_user_id, MatchingID);
            string s_cache_data_field = String.Format("TableDataField_{0}", n_user_id);

            Cache.Insert(s_cache_inside_matching, dt_matching_inside, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);
            Cache.Insert(s_cache_outside_matching, dt_matching_outside, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);

            if (Cache[s_cache_data_field] == null)
            {
                DataTable dt_data_field = new DataTable();

                DB.Bind_Data_Table("sprData_Field_Extended", ref dt_data_field, ref s_error);

                Cache.Insert(s_cache_data_field, dt_data_field, null, DateTime.Now.AddMinutes(20), Cache.NoSlidingExpiration);
            }

            Response.Redirect(String.Format("FrameForm.aspx?p=details&i={0}&q={1}", Request.QueryString.Get("i"), Request.QueryString.Get("q")));
        }

        protected void btnMatchingQueryDelete_Click(object sender, EventArgs e)
        {
            bool b_allow_delete = (bool)ViewState["AllowDelete"];

            if (b_allow_delete == false)
            {
                lblError.Text = "Can't delete query with 'Resplit' action.";
                return;
            }

            string s_error = "";

            int n_rows_affected = MatchingAction.Delete_Query(n_user_id, MatchingID, hidMatchingQueryNumber.Value, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            if (n_rows_affected <= 0)
            {
                lblError.Text = "No item/s was deleted.";
                return;
            }

            Response.Redirect(String.Format("FrameForm.aspx?result=deleted-query&i={0}&q={1}", Request.QueryString.Get("i"), Request.QueryString.Get("q")));
        }

        private void Bind_Form(MatchingModel o_matching)
        {
            ViewState["TableDataField"] = o_matching.TableDataField;

            // CAN'T DELETE MATCHING WITH 'RESPLIT' ACTION. THIS MATCHING WILL DELETED ON 'PAYMENT GROUP' RESTORATION. ( MatchingActionID # 3 : Resplit )

            bool b_allow_delete = (o_matching.MatchingActionID != 3);

            ViewState["AllowDelete"] = b_allow_delete;

            if (b_allow_delete == false)
            {
                btnMatchingQueryDelete.Enabled = false;
                btnMatchingQueryDelete.OnClientClick = "";
                btnMatchingQueryDelete.Style.Add("pointer-events", "none");

                lblMessage.Text = "Can't delete matching with 'Resplit' action. This matching will deleted on 'Payment Group' restoration.";
            }

            lblMatchingInsideCount.Text = String.Format("{0:n0}", o_matching.MatchingInsideCount);
            lblMatchingOutsideCount.Text = String.Format("{0:n0}", o_matching.MatchingOutsideCount);
            lblMatchingInsideAmount.Text = String.Format("{0:n2}", o_matching.MatchingInsideAmount);
            lblMatchingOutsideAmount.Text = String.Format("{0:n2}", o_matching.MatchingOutsideAmount);

            lblAllInsideCount.Text = String.Format("{0:n0}", o_matching.AllInsideCount);
            lblAllOutsideCount.Text = String.Format("{0:n0}", o_matching.AllOutsideCount);
            lblAllInsideAmount.Text = String.Format("{0:n2}", o_matching.AllInsideAmount);
            lblAllOutsideAmount.Text = String.Format("{0:n2}", o_matching.AllOutsideAmount);

            lblNotMatchingInsideCount.Text = String.Format("{0:n0}", o_matching.AllInsideCount - o_matching.MatchingInsideCount);
            lblNotMatchingOutsideCount.Text = String.Format("{0:n0}", o_matching.AllOutsideCount - o_matching.MatchingOutsideCount);
            lblNotMatchingInsideAmount.Text = String.Format("{0:n2}", o_matching.AllInsideAmount - o_matching.MatchingInsideAmount);
            lblNotMatchingOutsideAmount.Text = String.Format("{0:n2}", o_matching.AllOutsideAmount - o_matching.MatchingOutsideAmount);

            List<string> lst_matching_summary = new List<string>();

            for (int i = o_matching.TableMatchingSummary.Columns.Count - 1; i >= 0; i--)
            {
                string s_column = o_matching.TableMatchingSummary.Columns[i].ColumnName;

                bool b_exclude = false;

                DataTable dt_check = new DataView(o_matching.TableMatchingSummary).ToTable(true, s_column);

                switch (dt_check.Rows.Count)
                {
                    case 0:
                        b_exclude = true;
                        break;
                    case 1:
                        string s_value = dt_check.Rows[0][0].ToString().Trim();

                        b_exclude = (s_value == "");

                        break;
                }

                if (b_exclude == true)
                {
                    o_matching.TableMatchingSummary.Columns.Remove(s_column);
                }
                else
                {
                    lst_matching_summary.Insert(0, s_column);
                }
            }

            lst_matching_summary.Insert(0, "SelectQuery");

            o_matching.TableMatchingSummary.Columns.Add("SelectQuery");

            o_matching.TableMatchingSummary = new DataView(o_matching.TableMatchingSummary).ToTable(true, lst_matching_summary.ToArray());

            gvMatchingSummary.DataSource = o_matching.TableMatchingSummary;
            gvMatchingSummary.DataBind();
        }
    }
}
