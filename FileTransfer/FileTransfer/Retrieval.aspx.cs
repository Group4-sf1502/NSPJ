
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

namespace FileTransfer
{
    public partial class Retrieval : System.Web.UI.Page
    {
        /*
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
            GridView2.DataSource = null;
            GridView2.DataBind();
            string user = Server.MapPath("~/App_Data/") + TextBox1.Text;
            string[] files = Directory.GetFiles(user);
            List<ListItem> list = new List<ListItem>();

            foreach (string filePath in files)
                list.Add(new ListItem(Path.GetFileName(filePath), filePath));


            GridView1.DataSource = list;
            GridView1.DataBind();
            MultiView.ActiveViewIndex = 0;

            int userid = SQL.getUserID(TextBox1.Text);

            List<int> fileids = SQL.getShareFileID(userid);

            List<String> filePaths = SQL.getFilePaths(fileids);
            
            List<ListItem> list2 = new List<ListItem>();

            foreach (string path in filePaths) 
               list2.Add(new ListItem(Path.GetFileName(path), path));

            GridView2.DataSource = list2;
            GridView2.DataBind();

        }
        protected void DownloadFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            string fileName = Path.GetFileName(filePath);
            string tempPath = Server.MapPath("~/temp/") + fileName;
            File.Copy(filePath, tempPath);
            Security.DecryptFile(tempPath, tempPath);
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(tempPath));
            Response.WriteFile(tempPath);
            Response.Flush();
            File.Delete(tempPath);
            
        }
        protected void DeleteFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            File.Delete(filePath);
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            MultiView.ActiveViewIndex = 0;


        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            MultiView.ActiveViewIndex = 1;
        }

        protected void confirm_Click(object sender, EventArgs e)
        {

        }

        protected void Share_click(object sender, EventArgs e)
        {

        }

        protected void Cancel_Click(object sender, EventArgs e)
        {
            ModalPopupExtender2.Hide();
        }

        protected void showpopup(object sender, EventArgs e)
        {
            string filename = Path.GetFileName((sender as LinkButton).CommandArgument);
            fileName.Text = filename;
            ModalPopupExtender2.Show();
        }

        protected void fileshare(object sender, EventArgs e)
        {

            string shareduser = username.Text;
            string user = TextBox1.Text;

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

            TextBox1.Text = "" + username;
            
        }

    }
    */
    }
}