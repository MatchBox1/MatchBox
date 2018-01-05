using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MatchBox
{
    public partial class InsideFieldData : System.Web.UI.UserControl
    {
        public void LoadData(DataTable dt)
        {
            gvInside1.DataSource = dt;
            gvInside1.DataBind();
        }
        DataInspector objDataInspector;
        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}