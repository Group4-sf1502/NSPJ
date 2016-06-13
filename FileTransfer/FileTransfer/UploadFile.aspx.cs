using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.SqlClient;

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
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                if (!System.IO.Directory.Exists(Server.MapPath("~/App_Data/") + Username.Text))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/App_Data/") + Username.Text);
            }



            string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
            string path = Server.MapPath("~/App_Data/") + Username.Text + "/" + fileName;
            FileUpload1.PostedFile.SaveAs(path);
            //Response.Redirect(Request.Url.AbsoluteUri);
            Label1.Visible = true;
            Label1.Text = "File successfully uploaded!";
      

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO [userFiles] (fileName,fileSize,filePath,userID) VALUES ('" + fileName + "','" + FileUpload1.PostedFile.ContentLength + "','" + path + "','" + 1 + "');";
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();

            }

        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                String username = "Daryl";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO [User] (Username) VALUES ('" + username + "');";
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}