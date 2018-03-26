using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public partial class StrategyData : UserControl
    {
        public int StrategyID { get; set; }

        private int n_user_id = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (StrategyID <= 0) { return; }

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

                s_message = String.Format("Item successfully {0}.", s_message);

                lblMessage.Text = s_message;
            }

            Bind_Form();
        }

        protected void repInsideSum_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DataTable dt_inside_total = (DataTable)ViewState["TableInsideTotal"];

                if (dt_inside_total.Rows.Count > 0)
                {
                    DataRow dr_inside_sum_total = dt_inside_total.Rows[0];

                    int n_transaction_count_total = 0;
                    int.TryParse(dr_inside_sum_total["TransactionCountTotal"].ToString(), out n_transaction_count_total);

                    double n_amount_sum_total = 0;
                    double.TryParse(dr_inside_sum_total["AmountSumTotal"].ToString(), out n_amount_sum_total);

                    Label lblTransactionCountTotal = (Label)e.Item.FindControl("lblTransactionCountTotal");
                    Label lblAmountSumTotal = (Label)e.Item.FindControl("lblAmountSumTotal");

                    lblTransactionCountTotal.Text = String.Format("{0:n0}", n_transaction_count_total);
                    lblAmountSumTotal.Text = String.Format("{0:n2}", n_amount_sum_total);
                }
            }
        }

        protected void repOutsideSum_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer)
            {
                DataTable dt_outside_total = (DataTable)ViewState["TableOutsideTotal"];

                if (dt_outside_total.Rows.Count > 0)
                {
                    DataRow dr_outside_sum_total = dt_outside_total.Rows[0];

                    int n_transaction_count_total = 0;
                    int.TryParse(dr_outside_sum_total["TransactionCountTotal"].ToString(), out n_transaction_count_total);

                    double n_amount_sum_total = 0;
                    double.TryParse(dr_outside_sum_total["AmountSumTotal"].ToString(), out n_amount_sum_total);

                    Label lblTransactionCountTotal = (Label)e.Item.FindControl("lblTransactionCountTotal");
                    Label lblAmountSumTotal = (Label)e.Item.FindControl("lblAmountSumTotal");

                    lblTransactionCountTotal.Text = String.Format("{0:n0}", n_transaction_count_total);
                    lblAmountSumTotal.Text = String.Format("{0:n2}", n_amount_sum_total);
                }
            }
        }

        protected void DataFile_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int n_status_id = Convert.ToInt32(hidStatusID.Value);

            if ((n_status_id == 1 || n_status_id == 2) && (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem))
            {
                LinkButton btnDelete = (LinkButton)e.Item.FindControl("btnDelete");

                btnDelete.Enabled = false;
                btnDelete.OnClientClick = "";
                btnDelete.ForeColor = System.Drawing.Color.Gray;
            }
        }

        protected void Strategy_StopExecution(object sender, EventArgs e)
        {
            DataTable dt_strategy = new DataTable();
            string s_error = "";
            StrategyAction.Select_StopExecution(StrategyID, n_user_id, dt_strategy,  ref s_error);
            btnPending.Visible = true;
            btnstopexecution.Visible = false;
            lblMessage.Text = "Execution has been stopped successfully.";
        }

        protected void Strategy_Pending(object sender, EventArgs e)
        {
            string s_error = "";

            DataTable dt_strategy = new DataTable();

            StrategyAction.Select_Single(StrategyID, n_user_id,"fromDataFile", ref dt_strategy, ref s_error);

            if (s_error != "") { goto Error; }

            if (dt_strategy.Rows.Count == 0)
            {
                s_error = "Strategy not found.";
                goto Error;
            }

            DataRow dr_strategy = dt_strategy.Rows[0];

            int n_status_id = Convert.ToInt32(dr_strategy["StatusID"]);

            switch (n_status_id)
            {
                case 0:
                    string s_last_execution_date = dr_strategy["LastExecutionDate"].ToString().Trim();

                    if (s_last_execution_date != "")
                    {
                        DateTime d_last_execution_date = Convert.ToDateTime(s_last_execution_date);

                        TimeSpan t_after_execution = DateTime.Now - d_last_execution_date;

                        if (t_after_execution.Minutes < 1)
                        {
                            s_error = String.Format("Strategy already executed. Next execution will be available after {0}.", d_last_execution_date.AddMinutes(1));
                            goto Error;
                        }
                    }

                    n_status_id = 1;

                    break;
                case 1:
                    n_status_id = 0;
                    break;
                case 2:
                    Response.Redirect(String.Format("FrameForm.aspx?i={0}&q={1}&p={2}", Request.QueryString.Get("i"), Request.QueryString.Get("q"), "data"));
                    return;
                default:
                    s_error = "Status not valid.";
                    goto Error;
            }

            hidStatusID.Value = n_status_id.ToString();

            int n_rows_affected = StrategyAction.Update_Status(n_user_id, StrategyID, n_status_id, ref s_error);

            if (s_error != "") { goto Error; }

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}&p={3}", "updated", Request.QueryString.Get("i"), Request.QueryString.Get("q"), "data"));

            return;

        Error:

            lblError.Text = s_error;
        }

        protected void DataFile_Validate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = (cblDataFile.SelectedIndex >= 0);
        }

        protected void DataFile_Add(object sender, EventArgs e)
        {
            if (Page.IsValid == false) { return; }

            string s_data_file_array = "", s_error = "";

            foreach (ListItem chkDataFile in cblDataFile.Items.Cast<ListItem>().Where(i => i.Selected == true))
            {
                if (s_data_file_array != "") { s_data_file_array += ","; }

                s_data_file_array += chkDataFile.Value;
            }

            int n_rows_affected = StrategyAction.Update_Data(n_user_id, StrategyID, Common.Convert_To_Table(s_data_file_array, "int"), ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}&p={3}", "updated", Request.QueryString.Get("i"), Request.QueryString.Get("q"), "data"));
        }

        protected void DataFile_Delete(object sender, CommandEventArgs e)
        {
            string s_error = "";

            int n_data_file_id = Convert.ToInt32(e.CommandArgument);

            int n_rows_affected = StrategyAction.Delete_Data(n_user_id, StrategyID, n_data_file_id, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}&p={3}", "deleted", Request.QueryString.Get("i"), Request.QueryString.Get("q"), "data"));
        }

        private void Bind_Form()
        {
            string s_error = "";

            DataTable dt_strategy = new DataTable();
            DataTable dt_data_file = new DataTable();
            DataTable dt_inside = new DataTable();
            DataTable dt_outside = new DataTable();
            DataTable dt_inside_total = new DataTable();
            DataTable dt_outside_total = new DataTable();
            DataTable dt_inside_data_file = new DataTable();
            DataTable dt_outside_data_file = new DataTable();

            StrategyAction.Select_Data(n_user_id, StrategyID, ref dt_strategy, ref dt_data_file, ref dt_inside, ref dt_outside, ref dt_inside_total, ref dt_outside_total, ref dt_inside_data_file, ref dt_outside_data_file, ref s_error);

            if (s_error != "")
            {
                lblError.Text = s_error;
                return;
            }

            //ViewState["TableStrategy"] = dt_strategy;

            ViewState["TableInsideTotal"] = dt_inside_total;
            ViewState["TableOutsideTotal"] = dt_outside_total;

            DataRow dr_strategy = dt_strategy.Rows[0];

            lblStrategyName.Text = dr_strategy["StrategyName"].ToString();
            lblStatusName.Text = dr_strategy["StatusName"].ToString();
            lblQueryPercent.Text = dr_strategy["QueryPercent"].ToString();

            int n_status_id = Convert.ToInt32(dr_strategy["StatusID"]);

            hidStatusID.Value = n_status_id.ToString();

            if (n_status_id == 1 || n_status_id == 2)
            {
                btnSave.Visible = false;
                tdDataFile.Visible = false;

                if (n_status_id == 1)
                {
                    //btnPending.Text = "Cancel Execution";
                    btnPending.Visible = false;
                    btnstopexecution.Visible = true;
                }
            }
            else
            {
                cblDataFile.DataValueField = "ID";
                cblDataFile.DataTextField = "DataFileName";
                cblDataFile.DataSource = dt_data_file;
                cblDataFile.DataBind();
            }

            if (n_status_id == 2 || dt_inside.Rows.Count == 0 || dt_outside.Rows.Count == 0)
            {
                tdPending.Visible = false;
                tdstopexecution.Visible = true;
            }

            repInsideSum.DataSource = dt_inside;
            repInsideSum.DataBind();

            repOutsideSum.DataSource = dt_outside;
            repOutsideSum.DataBind();

            repInsideDataFile.DataSource = dt_inside_data_file;
            repInsideDataFile.DataBind();

            repOutsideDataFile.DataSource = dt_outside_data_file;
            repOutsideDataFile.DataBind();
        }
    }
}
