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
        static Stack dirs = new Stack();

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public void SaveAs(String fileName)
        { }

        protected void upload(object sender, EventArgs e)
        {

            if (!System.IO.Directory.Exists(Server.MapPath("~/App_Data/") + Username.Text))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/App_Data/") + Username.Text);
            }

            int userid = SQL.getUserID(Username.Text);
            string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
            string path = Server.MapPath("~/App_Data/") + Username.Text + "/" + fileName;

            FileUpload1.PostedFile.SaveAs(path);
            Security.EncryptFile(path, path);
            //Response.Redirect(Request.Url.AbsoluteUri);
            Label1.Visible = true;
            Label1.Text = "File successfully uploaded!";


            SQL.insertFile(fileName, FileUpload1.PostedFile.ContentLength, path, userid);




        }


        protected void retrieve(object sender, EventArgs e)
        {
            int userid = SQL.getUserID(Username.Text);

            //My files
            
            string user = Server.MapPath("~/App_Data/") + Username.Text;
            dirs.Push(user);
            DataTable dt = fillMainTable(user);
            GridView1.DataSource = dt;
            GridView1.DataBind();
            MultiView.ActiveViewIndex = 0;

            
            
          
            
            //Files shared with me

            List<int> fileids = SQL.getShareFileID(userid);
            DataTable dt2 = new DataTable();
            DataRow row;
            DataColumn column = new DataColumn("fileName");
            column.DataType = Type.GetType("System.String");
            DataColumn column2 = new DataColumn("Username");
            column2.DataType = Type.GetType("System.String");
            dt2.Columns.Add(column);
            dt2.Columns.Add(column2);
            List<String> files = SQL.getSharedDataTable(fileids);
            List<String> usernames = SQL.getShareUser(userid);
            
            for (int i = 0; i < files.Count(); i++)
            {
                row = dt2.NewRow();
                row["fileName"] = files[i];
                row["Username"] = usernames[i];
                dt2.Rows.Add(row);
            }

            
            GridView2.DataSource = dt2;
            GridView2.DataBind();

    
        }

        protected void retrieveSharedUsers(int fileid)
        {
            DataTable dt = SQL.getSharedUsers(fileid);
            sharedList.DataSource = dt;
            sharedList.DataBind();
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

        protected void DownloadSharedFile(object sender, EventArgs e)
        {
            string filePath = getsharedpath(sender);
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
            int fileid = SQL.getFileID(filepath);
            fileName.Text = filename;
            retrieveSharedUsers(fileid);
            ModalPopupExtender2.Show();
        }

        protected void fileshare(object sender, EventArgs e)
        {

            string shareduser = sharedUser.Text;
            string user = Username.Text;

            int userid = SQL.getUserID(user);
            int shareduserid = SQL.getUserID(shareduser);
            string filename = fileName.Text;
            int fileid = SQL.getFileID(filename, userid);
            SQL.insertShareFile(fileid, shareduserid, user);

        }

        protected void RemoveFile(object sender, EventArgs e)
        {
            string filePath = getsharedpath(sender);
            int fileid = SQL.getFileID(filePath);
            SQL.removeSharedFile(fileid);
        }

        protected void RemoveShare(object sender, EventArgs e)
        {
            int fileid = SQL.getFileID(fileName.Text, SQL.getUserID(Username.Text));
            SQL.removeSharedFile(fileid);
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

        protected override void Render(HtmlTextWriter writer)
        {
            string condition = "/";
            string condition2 = "..";
            foreach (GridViewRow r in GridView1.Rows)
            {
                if (r.RowType == DataControlRowType.DataRow)
                {
                    if ((r.Cells[0].Text.Substring(0, 1).Equals(condition)) || (r.Cells[0].Text.Substring(0, 2).Equals(condition2)))
                    {

                        r.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                        r.ToolTip = "Click to select row";
                        r.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink(this.GridView1, "Select$" + r.RowIndex, true);

                    }

                }
            }

            base.Render(writer);
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

        private string getsharedpath(object sender)
        {
            LinkButton lb = (LinkButton)sender;
            GridViewRow grv = (GridViewRow)lb.NamingContainer;
            string filename = grv.Cells[0].Text;
            int userid = SQL.getUserID(grv.Cells[1].Text);
            string filePath = SQL.getFilePaths(filename, userid);
            return filePath;
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string foldername;
            string path;
            DataTable dt;
            foreach (GridViewRow row in GridView1.Rows)
            {
                if (row.RowIndex == GridView1.SelectedIndex)
                {
                    if (row.Cells[0].Text.Equals(".."))
                    {
                        dirs.Pop();
                        path = dirs.Peek().ToString();
                        
                        if (path.Equals(Server.MapPath("~/App_Data/") + Username.Text))
                        {
                            dt = fillMainTable(path);
                        }
                        else
                        {
                            dt = fillTable(path);
                        }
                        
                        GridView1.DataSource = null;
                        GridView1.DataBind();
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }

                    else
                    {
                        foldername = row.Cells[0].Text.Substring(1, row.Cells[0].Text.Length - 2);
                        path = dirs.Peek().ToString() + "\\" + foldername;
                        dt = fillTable(path);
                        dirs.Push(path);
                    }

                    GridView1.DataSource = null;
                    GridView1.DataBind();
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
        }

        protected DataTable fillTable(string path)
        {
            string back = "..";
            DataTable dt = new DataTable();
            List<string> dir = new List<string>(Directory.EnumerateDirectories(path));
            string[] files = Directory.GetFiles(path);
            DataRow row;
            DataColumn column = new DataColumn("Name");
            column.DataType = Type.GetType("System.String");
            DataColumn column2 = new DataColumn("Last modified");
            column2.DataType = Type.GetType("System.String");
            dt.Columns.Add(column);
            dt.Columns.Add(column2);

            

            for (int i = 0; i < dir.Count; i++)
            {
                row = dt.NewRow();
                row["Name"] = "/" + Path.GetFileName(dir[i]) + "/";
                dt.Rows.Add(row);
            }

            for (int i = 0; i < files.Length; i++)
            {
                row = dt.NewRow();
                row["Name"] = Path.GetFileName(files[i]);
                dt.Rows.Add(row);
            }

            row = dt.NewRow();
            row["Name"] = back;
            dt.Rows.Add(row);

            return dt;
        }

        protected DataTable fillMainTable(string path)
        {
            DataTable dt = new DataTable();
            List<string> dir = new List<string>(Directory.EnumerateDirectories(path));
            string[] files = Directory.GetFiles(path);
            DataRow row;
            DataColumn column = new DataColumn("Name");
            column.DataType = Type.GetType("System.String");
            DataColumn column2 = new DataColumn("Last modified");
            column2.DataType = Type.GetType("System.String");
            dt.Columns.Add(column);
            dt.Columns.Add(column2);

            for (int i = 0; i < dir.Count; i++)
            {
                row = dt.NewRow();
                row["Name"] = "/" + Path.GetFileName(dir[i]) + "/";
                dt.Rows.Add(row);
            }

            for (int i = 0; i < files.Length; i++)
            {
                row = dt.NewRow();
                row["Name"] = Path.GetFileName(files[i]);
                dt.Rows.Add(row);
            }

            return dt;
        }

        protected void moveto(object sender, EventArgs e) 
        {
            int fileid = SQL.getFileID(Label3.Text, SQL.getUserID(Username.Text));
            string selectedfolder = dirlist.SelectedNode
            
        }

        protected void fillTree(object sender, EventArgs e) 
        {
            ModalPopupExtender3.Show();
            LinkButton lb = (LinkButton)sender;
            GridViewRow grv = (GridViewRow)lb.NamingContainer;
            string filename = grv.Cells[0].Text;
            Label3.Text += filename;
            dirlist.Nodes.Clear();
            var rootDirectoryInfo = new DirectoryInfo(Server.MapPath("~/App_Data/") + Username.Text);
            dirlist.Nodes.Add(getNodes(rootDirectoryInfo));
        }
        
        protected TreeNode getNodes(DirectoryInfo dirinfo)
        {
            var directorynode = new TreeNode(dirinfo.Name);
           
            foreach (var directory in dirinfo.GetDirectories())
            {
                directorynode.ChildNodes.Add(getNodes(directory));
            }

            return directorynode;
        }
    }
}