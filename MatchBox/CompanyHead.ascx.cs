using System;
using System.Web.UI;

namespace MatchBox
{
    public partial class CompanyHead : UserControl
    {
        public int CompanyID { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) { return; }

            if (CompanyID > 0)
            {
                hedTitle.InnerHtml = "Company #" + CompanyID;

                lnkHome.HRef = String.Format("FrameForm.aspx?i={0}&q={1}", Request.QueryString.Get("i"), Request.QueryString.Get("q"));
                lnkAgreement.HRef = String.Format("FrameForm.aspx?i={0}&q={1}&p=agreement", Request.QueryString.Get("i"), Request.QueryString.Get("q"));
                lnkCashbox.HRef = String.Format("FrameForm.aspx?i={0}&q={1}&p=cashbox", Request.QueryString.Get("i"), Request.QueryString.Get("q"));

                if (Request.QueryString.Get("p") == null)
                {
                    lnkHome.Attributes.CssStyle.Add("color", "red");
                }
                else if (Request.QueryString.Get("p") == "agreement")
                {
                    lnkAgreement.Attributes.CssStyle.Add("color", "red");
                }
                else if (Request.QueryString.Get("p") == "cashbox")
                {
                    lnkCashbox.Attributes.CssStyle.Add("color", "red");
                }
            }
            else
            {
                hedTitle.InnerHtml = "New Company";

                lnkHome.Visible = false;
                lnkAgreement.Visible = false;
                lnkCashbox.Visible = false;
            }
        }
    }
}