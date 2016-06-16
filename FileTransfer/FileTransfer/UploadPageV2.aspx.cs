using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;

namespace FileTransfer
{
    public partial class UploadPageV2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void AjaxFileUploadEvent(object sender, AjaxFileUploadEventArgs e)
        {
            string username = Username.Text;
            string filename = System.IO.Path.GetFileName(e.FileName);
            string strUploadPath = "~/App_Data/" + username;

            if (!System.IO.Directory.Exists(Server.MapPath("~/App_Data/") + Username.Text))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/App_Data/") + Username.Text);
            }

            AjaxFileUpload1.SaveAs(Server.MapPath(strUploadPath) + filename);

        }
    }
}