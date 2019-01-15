using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Test
{

    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write(MyIdHelper.myId.GetGuid());
            Response.Write("<br/>");
            Response.Write(MyIdHelper.myId.GetGuid(2));
            Response.Write("<br/>");
            Response.Write(MyIdHelper.myId.GetGuidToN());
            Response.Write("<br/>");
            Response.Write(MyIdHelper.myId.GetGuidToN(2));
            Response.Write("<br/>");
            Response.Write(MyIdHelper.myId.GetObjectId());
            Response.Write("<br/>");
            Response.Write(MyIdHelper.myId.GetObjectId(2));
            Response.Write("<br/>");
            Response.Write(MyIdHelper.myId.GetSnowflakeId());
            Response.Write("<br/>");
            Response.Write(MyIdHelper.myId.GetSnowflakeId(2));
            Response.Write("<br/>");
            Response.Write(MyIdHelper.myId.GetBase16Id());
            Response.Write("<br/>");
            Response.Write(MyIdHelper.myId.GetBase16Id(2));
            Response.Write("<br/>");
            Response.Write(MyIdHelper.myId.GetBase20Id());
            Response.Write("<br/>");
            Response.Write(MyIdHelper.myId.GetBase20Id(2));
            Response.Write("<br/>");
            Response.Write(MyIdHelper.myId.GetBase25Id());
            Response.Write("<br/>");
            Response.Write(MyIdHelper.myId.GetBase25Id(2));
        }
    }
}