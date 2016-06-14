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
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string user = Server.MapPath("~/App_Data/") + TextBox1.Text;
            string[] files = Directory.GetFiles(user);
            List<ListItem> list = new List<ListItem>();
            foreach (string filePath in files)
                list.Add(new ListItem(Path.GetFileName(filePath), filePath));
            GridView1.DataSource = list;
            GridView1.DataBind();
            MultiView.ActiveViewIndex = 0;


        }
        protected void DownloadFile(object sender, EventArgs e)
        {
            string filePath = (sender as LinkButton).CommandArgument;
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(filePath);
            Response.End();
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
            SQL.insertShareFile(fileid, userid, shareduserid);



        }

    }
}