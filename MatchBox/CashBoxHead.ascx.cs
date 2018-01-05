using System;
using System.Web.UI;

namespace MatchBox
{
    public partial class CashBoxHead : UserControl
    {
        public int CashBoxID { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) { return; }

            if (CashBoxID > 0)
            {
                hedTitle.InnerHtml = "Cashbox #" + CashBoxID;

                lnkHome.HRef = String.Format("FrameForm.aspx?i={0}&q={1}", Request.QueryString.Get("i"), Request.QueryString.Get("q"));
                lnkPeriod.HRef = String.Format("FrameForm.aspx?i={0}&q={1}&p=period", Request.QueryString.Get("i"), Request.QueryString.Get("q"));

                if (Request.QueryString.Get("p") == null)
                {
                    lnkHome.Attributes.CssStyle.Add("color", "red");
                }
                else if (Request.QueryString.Get("p") == "period")
                {
                    lnkPeriod.Attributes.CssStyle.Add("color", "red");
                }
            }
            else
            {
                hedTitle.InnerHtml = "New Cashbox";

                lnkHome.Visible = false;
                lnkPeriod.Visible = false;
            }
        }
    }
}