using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public partial class UserForm : UserControl
    {
        public int UserID { get; set; }

        private UserModel o_user_authorized = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            o_user_authorized = (UserModel)Session["User"];

            if ((o_user_authorized.IsAdmin == false) && (o_user_authorized.IsUser == false || o_user_authorized.ID != UserID))
            {
                Response.Redirect("Default.aspx");
            }

            if (Page.IsPostBack) { return; }

            if (UserID > 0) { hedTitle.InnerHtml = "User #" + UserID; } else { hedTitle.InnerHtml = "New User"; }

            if (Request.QueryString["result"] != null && Request.QueryString.Get("result") == "updated") { lblMessage.Text = "Item successfully updated."; }

            string s_error = "";

            DB.Bind_List_Control(ddlStatus, "sprStatus", ref s_error);
            DB.Bind_List_Control(ddlCity, "sprCity", ref s_error);

            UserModel o_user_selected = new UserModel();

            if (UserID > 0)
            {
                o_user_selected.ID = UserID;

                UserAction.Select(ref o_user_selected);

                if (o_user_selected.ErrorMessage != "")
                {
                    lblError.Text = o_user_selected.ErrorMessage;
                    return;
                }
            }

            Bind_Form(o_user_selected);

            if (o_user_authorized.ID == 0) { ddlStatus.Focus(); } else { txtFullName.Focus(); }
        }

        private void Reload_TableUserService(ref UserModel o_user_selected)
        {
            foreach (DataRow o_row in o_user_selected.TableUserService.Rows)
            {
                o_row.Delete();
            }

            foreach (RepeaterItem o_item in repService.Items)
            {
                if (o_item.ItemType == ListItemType.Item || o_item.ItemType == ListItemType.AlternatingItem)
                {
                    CheckBox chk_is_selected = (CheckBox)o_item.FindControl("chkIsSelected");

                    if (chk_is_selected.Checked == true)
                    {
                        HiddenField hid_service_id = (HiddenField)o_item.FindControl("hidServiceID");
                        HiddenField hid_dependency_id = (HiddenField)o_item.FindControl("hidDependencyID");
                        Label lbl_service_name = (Label)o_item.FindControl("lblServiceName");

                        DataRow o_data_row = o_user_selected.TableUserService.NewRow();

                        o_data_row["ServiceID"] = Convert.ToInt32(hid_service_id.Value);
                        o_data_row["DependencyID"] = Convert.ToInt32(hid_dependency_id.Value);
                        o_data_row["ServiceName"] = lbl_service_name.Text;
                        o_data_row["IsSelected"] = true;

                        o_user_selected.TableUserService.Rows.Add(o_data_row);
                    }
                }
            }

            // Check If Dependency Services Selected

            foreach (DataRow o_data_row in o_user_selected.TableUserService.Select("DependencyID > 0"))
            {
                int n_service_id = Convert.ToInt32(o_data_row["ServiceID"]);
                int n_dependency_id = Convert.ToInt32(o_data_row["DependencyID"]);

                if (o_user_selected.TableUserService.Select("ServiceID = " + n_dependency_id).Length == 0)
                {
                    if (o_user_selected.ErrorMessage != "") { o_user_selected.ErrorMessage += "<br />"; }

                    o_user_selected.ErrorMessage += String.Format("If option <b>#{0}</b> selected, then option <b>#{1}</b> must be selected to.", n_service_id, n_dependency_id);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) { return; }

            UserModel o_user_selected = new UserModel();

            o_user_selected.ID = UserID;
            o_user_selected.CityID = Convert.ToInt32(ddlCity.SelectedValue);
            o_user_selected.StatusID = Convert.ToInt32(ddlStatus.SelectedValue);

            o_user_selected.FullName = txtFullName.Text.Trim();
            o_user_selected.UserName = txtUserName.Text.Trim();
            o_user_selected.Password = txtPassword.Text.Trim();
            o_user_selected.Mail = txtMail.Text.Trim();
            o_user_selected.Phone = txtPhone.Text.Trim();
            o_user_selected.Mobile = txtMobile.Text.Trim();
            o_user_selected.Address = txtAddress.Text.Trim();

            // Update User Services

            if (o_user_authorized.IsAdmin == true)
            {
                Reload_TableUserService(ref o_user_selected);
            }

            if (o_user_selected.ErrorMessage != "")
            {
                lblServiceError.Visible = true;
                lblServiceError.Text = o_user_selected.ErrorMessage;
                return;
            }

            UserAction.Update(ref o_user_selected, o_user_authorized.IsAdmin);

            if (o_user_selected.ErrorMessage != "")
            {
                lblError.Text = o_user_selected.ErrorMessage;
                return;
            }

            string s_guid = Request.QueryString.Get("q");
            string s_result = "updated";

            if (UserID == 0)
            {
                s_guid = Guid.NewGuid().ToString();
                s_result = "inserted";

                ((Dictionary<string, int>)Session["UserLookupTable"]).Add(s_guid, o_user_selected.ID);
            }

            if (o_user_authorized.IsAdmin == true)
            {
                // UserForm IN IFRAME
                Response.Redirect(String.Format("FrameForm.aspx?result={0}&i={1}&q={2}", s_result, Request.QueryString.Get("i"), s_guid));
            }
            else if (o_user_authorized.IsUser == true)
            {
                // UserForm NOT IN IFRAME

                o_user_selected.IsUser = true;

                Session["User"] = o_user_selected;

                Response.Redirect(String.Format("UserDetails.aspx?result={0}", s_result));
            }
        }

        private void Bind_Form(UserModel o_user_selected)
        {
            divStatus_Change.Visible = o_user_authorized.IsAdmin;

            if (UserID == 0) { ddlStatus.SelectedValue = ""; } else { ddlStatus.SelectedValue = o_user_selected.StatusID.ToString(); }

            ddlCity.SelectedValue = o_user_selected.CityID.ToString();

            txtFullName.Text = o_user_selected.FullName;
            txtUserName.Text = o_user_selected.UserName;
            txtPassword.Text = o_user_selected.Password;
            txtMail.Text = o_user_selected.Mail;
            txtPhone.Text = o_user_selected.Phone;
            txtMobile.Text = o_user_selected.Mobile;
            txtAddress.Text = o_user_selected.Address;

            if (o_user_authorized.IsAdmin == true)
            {
                divService_Change.Visible = true;

                repService.DataSource = o_user_selected.TableUserService;
                repService.DataBind();
            }
            else
            {
                divService_View.Visible = true;

                DataRow[] o_data_rows_selected = o_user_selected.TableUserService.Select("IsSelected = True");
                DataRow[] o_data_rows_available = o_user_selected.TableUserService.Select("IsSelected = False");

                if (o_data_rows_selected.Length > 0)
                {
                    divService_View_Selected.Visible = true;

                    repServiceSelected.DataSource = new DataView(o_data_rows_selected.CopyToDataTable()).ToTable(false, new string[] { "ServiceName" });
                    repServiceSelected.DataBind();
                }

                if (o_data_rows_available.Length > 0)
                {
                    divService_View_Available.Visible = true;

                    repServiceAvailable.DataSource = new DataView(o_data_rows_available.CopyToDataTable()).ToTable(false, new string[] { "ServiceName" });
                    repServiceAvailable.DataBind();
                }
            }
        }
    }
}
