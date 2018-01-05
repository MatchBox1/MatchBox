using System;
using System.Web.UI;

namespace MatchBox
{
    public partial class StrategyHead : UserControl
    {
        public int StrategyID { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) { return; }

            if (StrategyID > 0)
            {
                hedTitle.InnerHtml = "Strategy #" + StrategyID;

                lnkHome.HRef = String.Format("FrameForm.aspx?i={0}&q={1}", Request.QueryString.Get("i"), Request.QueryString.Get("q"));
                lnkDataFile.HRef = String.Format("FrameForm.aspx?i={0}&q={1}&p=data", Request.QueryString.Get("i"), Request.QueryString.Get("q"));

                if (Request.QueryString.Get("p") == null)
                {
                    lnkHome.Attributes.CssStyle.Add("color", "red");
                }
                else if (Request.QueryString.Get("p") == "data")
                {
                    lnkDataFile.Attributes.CssStyle.Add("color", "red");
                }
            }
            else
            {
                hedTitle.InnerHtml = "New Strategy";

                lnkHome.Visible = false;
                lnkDataFile.Visible = false;
            }
        }
    }
}
