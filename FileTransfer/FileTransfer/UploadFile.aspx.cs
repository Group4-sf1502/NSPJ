using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Collections;
using AjaxControlToolkit;
using System.Data.SqlClient;
using FileTransfer;
using System.Data;

namespace Testing
{
    public partial class uploadFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void SaveAs(String fileName)
        { }

        protected void upload(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                if (!System.IO.Directory.Exists(Server.MapPath("~/App_Data/") + Username.Text))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("~/App_Data/") + Username.Text);
                }

                int userid = SQL.getUserID(Username.Text);
                string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                string path = Server.MapPath("~/App_Data/") + Username.Text + "/" + fileName;
                
                FileUpload1.PostedFile.SaveAs(path);
                Security.EncryptFile(path,path);
                //Response.Redirect(Request.Url.AbsoluteUri);
                Label1.Visible = true;
                Label1.Text = "File successfully uploaded!";


                
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO [userFiles] (fileName,fileSize,filePath,userID) VALUES ('" + fileName + "','" + FileUpload1.PostedFile.ContentLength + "','" + path + "','" + userid + "');";
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                
            }

        }


        protected void retrieve(object sender, EventArgs e)
        {
            //My files

            int userid = SQL.getUserID(Username.Text);
            DataTable dt = SQL.getDataTable(userid);
            GridView1.DataSource = dt;
            GridView1.DataBind();
           

            //Files shared with me
    
            List<int> fileids = SQL.getShareFileID(userid);
            List<String> filePaths = SQL.getFilePaths(fileids);
            List<ListItem> list2 = new List<ListItem>();

            foreach (string path in filePaths)
                list2.Add(new ListItem(Path.GetFileName(path), path));

            GridView2.DataSource = list2;
            GridView2.DataBind();
            
            MultiView.ActiveViewIndex = 0;
        }

        protected void DownloadFile(object sender, EventArgs e)
        {
            string filePath = getpath(sender);
            string filename = Path.GetFileName(filePath);
            string tempPath = Server.MapPath("~/temp/") + filename;
            File.Copy(filePath, tempPath);
            Security.DecryptFile(tempPath, tempPath);
            Response.ClearContent();
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(tempPath));
            Response.TransmitFile(tempPath);
            Response.Flush();
            File.Delete(tempPath);
            Response.End();
           
            

        }
        protected void DeleteFile(object sender, EventArgs e)
        {
            string filePath = getpath(sender);
            File.Delete(filePath);
            SQL.deleteFile(filePath);
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        protected void ViewMyFiles(object sender, EventArgs e)
        {
            MultiView.ActiveViewIndex = 0;


        }

        protected void ViewSharedFiles(object sender, EventArgs e)
        {
            MultiView.ActiveViewIndex = 1;
        }


        protected void Cancel_Click(object sender, EventArgs e)
        {
            ModalPopupExtender2.Hide();
        }

        protected void showpopup(object sender, EventArgs e)
        {
            string filepath = getpath(sender);
            string filename = Path.GetFileName(filepath);
            fileName.Text = filename;
            ModalPopupExtender2.Show();
        }

        protected void fileshare(object sender, EventArgs e)
        {

            string shareduser = sharedUser.Text;
            string user = Username.Text;

            int userid = SQL.getUserID(user);
            int shareduserid = SQL.getUserID(shareduser);
            String filename = fileName.Text;  
            int fileid = SQL.getFileID(fileName.Text, userid);
            SQL.insertShareFile(fileid, shareduserid);
            
        }

        protected void getinfo(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            int fileid = SQL.getFileID(filePath);
            int userid = SQL.getUserID(fileid);
            String username = SQL.getUsername(userid);

            Username.Text = "" + username;

        }

        protected void rowdatabind(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.CssClass = "header";
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.CssClass = "normal";
            }
        }

        private string getpath(object sender)
        {
            LinkButton lb = (LinkButton)sender;
            GridViewRow grv = (GridViewRow)lb.NamingContainer;
            string filename = grv.Cells[0].Text;
            int userid = SQL.getUserID(Username.Text);
            string filePath = SQL.getFilePaths(filename, userid);
            return filePath;
        }
       
    }
}