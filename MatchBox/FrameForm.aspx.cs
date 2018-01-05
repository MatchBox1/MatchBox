using System;
using System.Collections.Generic;
using System.Web.UI;

namespace MatchBox
{
    public partial class FrameForm : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["i"] == null || Request.QueryString["q"] == null) { return; }

            if (Request.QueryString["result"] != null) { hidResult.Value = Request.QueryString.Get("result"); }

            string s_interface = Request.QueryString.Get("i");
            string s_query = Request.QueryString.Get("q");

            Dictionary<string, int> o_lookup_dictionary = null;
            UserControl o_user_control = new UserControl();

            if (s_interface == UserModel.InterfaceName)
            {
                o_lookup_dictionary = (Dictionary<string, int>)Session["UserLookupTable"];

                if (o_lookup_dictionary.ContainsKey(s_query))
                {
                    o_user_control = (UserForm)LoadControl("UserForm.ascx");
                    ((UserForm)o_user_control).UserID = o_lookup_dictionary[s_query];
                }
            }
            else if (s_interface == CompanyModel.InterfaceName)
            {
                o_lookup_dictionary = (Dictionary<string, int>)Session["CompanyLookupTable"];

                if (o_lookup_dictionary.ContainsKey(s_query))
                {
                    UserControl o_user_control_head = (CompanyHead)LoadControl("CompanyHead.ascx");
                    ((CompanyHead)o_user_control_head).CompanyID = o_lookup_dictionary[s_query];

                    frmFrameForm.Controls.Add(o_user_control_head);

                    if (Request.QueryString.Get("p") == null)
                    {
                        o_user_control = (CompanyForm)LoadControl("CompanyForm.ascx");
                        ((CompanyForm)o_user_control).CompanyID = o_lookup_dictionary[s_query];
                    }
                    else if (Request.QueryString.Get("p") == "agreement")
                    {
                        o_user_control = (CompanyAgreement)LoadControl("CompanyAgreement.ascx");
                        ((CompanyAgreement)o_user_control).CompanyID = o_lookup_dictionary[s_query];
                    }
                    else if (Request.QueryString.Get("p") == "cashbox")
                    {
                        o_user_control = (CompanyCashBox)LoadControl("CompanyCashBox.ascx");
                        ((CompanyCashBox)o_user_control).CompanyID = o_lookup_dictionary[s_query];
                    }
                }
            }
            else if (s_interface == NetworkModel.InterfaceName)
            {
                o_lookup_dictionary = (Dictionary<string, int>)Session["NetworkLookupTable"];

                if (o_lookup_dictionary.ContainsKey(s_query))
                {
                    o_user_control = (NetworkForm)LoadControl("NetworkForm.ascx");
                    ((NetworkForm)o_user_control).NetworkID = o_lookup_dictionary[s_query];
                }
            }
            else if (s_interface == BranchModel.InterfaceName)
            {
                o_lookup_dictionary = (Dictionary<string, int>)Session["BranchLookupTable"];

                if (o_lookup_dictionary.ContainsKey(s_query))
                {
                    o_user_control = (BranchForm)LoadControl("BranchForm.ascx");
                    ((BranchForm)o_user_control).BranchID = o_lookup_dictionary[s_query];
                }
            }
            else if (s_interface == CashBoxModel.InterfaceName)
            {
                o_lookup_dictionary = (Dictionary<string, int>)Session["CashBoxLookupTable"];

                if (o_lookup_dictionary.ContainsKey(s_query))
                {
                    UserControl o_user_control_head = (CashBoxHead)LoadControl("CashBoxHead.ascx");
                    ((CashBoxHead)o_user_control_head).CashBoxID = o_lookup_dictionary[s_query];

                    frmFrameForm.Controls.Add(o_user_control_head);

                    if (Request.QueryString.Get("p") == null)
                    {
                        o_user_control = (CashBoxForm)LoadControl("CashBoxForm.ascx");
                        ((CashBoxForm)o_user_control).CashBoxID = o_lookup_dictionary[s_query];
                    }
                    else if (Request.QueryString.Get("p") == "period")
                    {
                        o_user_control = (CashBoxPeriod)LoadControl("CashBoxPeriod.ascx");
                        ((CashBoxPeriod)o_user_control).CashBoxID = o_lookup_dictionary[s_query];
                    }
                }
            }
            else if (s_interface == AgreementModel.InterfaceName)
            {
                o_lookup_dictionary = (Dictionary<string, int>)Session["AgreementLookupTable"];

                if (o_lookup_dictionary.ContainsKey(s_query))
                {
                    UserControl o_user_control_head = (AgreementHead)LoadControl("AgreementHead.ascx");
                    ((AgreementHead)o_user_control_head).AgreementID = o_lookup_dictionary[s_query];

                    frmFrameForm.Controls.Add(o_user_control_head);

                    if (Request.QueryString.Get("p") == null)
                    {
                        o_user_control = (AgreementForm)LoadControl("AgreementForm.ascx");
                        ((AgreementForm)o_user_control).AgreementID = o_lookup_dictionary[s_query];
                    }
                    else if (Request.QueryString.Get("p") == "period")
                    {
                        o_user_control = (AgreementPeriod)LoadControl("AgreementPeriod.ascx");
                        ((AgreementPeriod)o_user_control).AgreementID = o_lookup_dictionary[s_query];
                    }
                    else if (Request.QueryString.Get("p") == "payment")
                    {
                        o_user_control = (AgreementPayment)LoadControl("AgreementPayment.ascx");
                        ((AgreementPayment)o_user_control).AgreementID = o_lookup_dictionary[s_query];
                    }
                    else if (Request.QueryString.Get("p") == "item")
                    {
                        o_user_control = (AgreementItem)LoadControl("AgreementItem.ascx");
                        ((AgreementItem)o_user_control).AgreementID = o_lookup_dictionary[s_query];
                    }
                    else if (Request.QueryString.Get("p") == "discount")
                    {
                        o_user_control = (AgreementDiscount)LoadControl("AgreementDiscount.ascx");
                        ((AgreementDiscount)o_user_control).AgreementID = o_lookup_dictionary[s_query];
                    }
                    else if (Request.QueryString.Get("p") == "club")
                    {
                        o_user_control = (AgreementClub)LoadControl("AgreementClub.ascx");
                        ((AgreementClub)o_user_control).AgreementID = o_lookup_dictionary[s_query];
                    }
                }
            }
            else if (s_interface == TemplateModel.InterfaceName)
            {
                o_lookup_dictionary = (Dictionary<string, int>)Session["TemplateLookupTable"];

                if (o_lookup_dictionary.ContainsKey(s_query))
                {
                    o_user_control = (TemplateForm)LoadControl("TemplateForm.ascx");
                    ((TemplateForm)o_user_control).TemplateID = o_lookup_dictionary[s_query];
                }
            }
            else if (s_interface == DataFileModel.InterfaceName)
            {
                o_lookup_dictionary = (Dictionary<string, int>)Session["DataFileLookupTable"];

                if (o_lookup_dictionary.ContainsKey(s_query))
                {
                    o_user_control = (DataFileForm)LoadControl("DataFileForm.ascx");
                    ((DataFileForm)o_user_control).DataFileID = o_lookup_dictionary[s_query];
                }
            }
            else if (s_interface == StrategyModel.InterfaceName)
            {
                o_lookup_dictionary = (Dictionary<string, int>)Session["StrategyLookupTable"];

                if (o_lookup_dictionary.ContainsKey(s_query))
                {
                    UserControl o_user_control_head = (StrategyHead)LoadControl("StrategyHead.ascx");
                    ((StrategyHead)o_user_control_head).StrategyID = o_lookup_dictionary[s_query];

                    frmFrameForm.Controls.Add(o_user_control_head);

                    if (Request.QueryString.Get("p") == null)
                    {
                        o_user_control = (StrategyForm)LoadControl("StrategyForm.ascx");
                        ((StrategyForm)o_user_control).StrategyID = o_lookup_dictionary[s_query];
                    }
                    else if (Request.QueryString.Get("p") == "data")
                    {
                        o_user_control = (StrategyData)LoadControl("StrategyData.ascx");
                        ((StrategyData)o_user_control).StrategyID = o_lookup_dictionary[s_query];
                    }
                }
            }
            else if (s_interface == MatchingModel.InterfaceName)
            {
                o_lookup_dictionary = (Dictionary<string, int>)Session["MatchingLookupTable"];

                if (o_lookup_dictionary.ContainsKey(s_query))
                {
                    if (Request.QueryString.Get("p") == null)
                    {
                        o_user_control = (MatchingForm)LoadControl("MatchingForm.ascx");
                        ((MatchingForm)o_user_control).MatchingID = o_lookup_dictionary[s_query];
                    }
                    else if (Request.QueryString.Get("p") == "details")
                    {
                        o_user_control = (MatchingDetails)LoadControl("MatchingDetails.ascx");
                        ((MatchingDetails)o_user_control).MatchingID = o_lookup_dictionary[s_query];
                    }
                }
            }
            else
            {
                return;
            }

            frmFrameForm.Controls.Add(o_user_control);
        }
    }
}
