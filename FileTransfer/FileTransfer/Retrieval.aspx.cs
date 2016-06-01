using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Collections;

namespace FileTransfer
{
    public partial class Retrieval : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string user = @"C:/Users/daryl/Source/Repos/NSPJ/FileTransfer/FileTransfer/App_Data/" + TextBox1.Text;
            string[] files = Directory.GetFiles(user);
            ArrayList names = new ArrayList();
            for ( int i = 0; i < files.Length; i++)
                names.Add(files[i]);
            ListBox1.DataSource = names;
            ListBox1.DataBind();
        }
    }
}