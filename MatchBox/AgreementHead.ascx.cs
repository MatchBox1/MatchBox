using System;
using System.Web.UI;

namespace MatchBox
{
    public partial class AgreementHead : UserControl
    {
        public int AgreementID { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) { return; }

            if (AgreementID > 0)
            {
                hedTitle.InnerHtml = "Agreement #" + AgreementID;

                lnkHome.HRef = String.Format("FrameForm.aspx?i={0}&q={1}", Request.QueryString.Get("i"), Request.QueryString.Get("q"));
                lnkPeriod.HRef = String.Format("FrameForm.aspx?i={0}&q={1}&p=period", Request.QueryString.Get("i"), Request.QueryString.Get("q"));
                lnkPayment.HRef = String.Format("FrameForm.aspx?i={0}&q={1}&p=payment", Request.QueryString.Get("i"), Request.QueryString.Get("q"));
                lnkItem.HRef = String.Format("FrameForm.aspx?i={0}&q={1}&p=item", Request.QueryString.Get("i"), Request.QueryString.Get("q"));
                lnkDiscount.HRef = String.Format("FrameForm.aspx?i={0}&q={1}&p=discount", Request.QueryString.Get("i"), Request.QueryString.Get("q"));
                lnkClub.HRef = String.Format("FrameForm.aspx?i={0}&q={1}&p=club", Request.QueryString.Get("i"), Request.QueryString.Get("q"));
                
                if (Request.QueryString.Get("p") == null)
                {
                    lnkHome.Attributes.CssStyle.Add("color", "red");
                }
                else if (Request.QueryString.Get("p") == "period")
                {
                    lnkPeriod.Attributes.CssStyle.Add("color", "red");
                }
                else if (Request.QueryString.Get("p") == "payment")
                {
                    lnkPayment.Attributes.CssStyle.Add("color", "red");
                }
                else if (Request.QueryString.Get("p") == "item")
                {
                    lnkItem.Attributes.CssStyle.Add("color", "red");
                }
                else if (Request.QueryString.Get("p") == "discount")
                {
                    lnkDiscount.Attributes.CssStyle.Add("color", "red");
                }
                else if (Request.QueryString.Get("p") == "club")
                {
                    lnkClub.Attributes.CssStyle.Add("color", "red");
                }
            }
            else
            {
                hedTitle.InnerHtml = "New Agreement";

                lnkHome.Visible = false;
                lnkPeriod.Visible = false;
                lnkPayment.Visible = false;
                lnkItem.Visible = false;
                lnkDiscount.Visible = false;
                lnkClub.Visible = false;
            }
        }
    }
}
