using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;


namespace Testing
{
    public partial class uploadFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void SaveAs(String fileName)
        { }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (!System.IO.Directory.Exists(@"C:/Users/daryl/Source/Repos/NSPJ/FileTransfer/FileTransfer/App_Data/" + Username.Text))
            {
                System.IO.Directory.CreateDirectory(@"C:/Users/daryl/Source/Repos/NSPJ/FileTransfer/FileTransfer/App_Data/" + Username.Text);
            }

            //System.IO.DriveInfo di = new System.IO.DriveInfo("~/App_Data/" + Username.Text);
            //System.IO.DirectoryInfo dirInfo = di.RootDirectory;
            //System.IO.FileInfo[] files = dirInfo.GetFiles("*.*");
            string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
            FileUpload1.PostedFile.SaveAs((@"C:/Users/daryl/Source/Repos/NSPJ/FileTransfer/FileTransfer/App_Data/" + Username.Text + "/") + fileName);
            //  Response.Redirect(Request.Url.AbsoluteUri);
            Label1.Visible = true;
            Label1.Text = "File successfully uploaded!";


        }

    }
}