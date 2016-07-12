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
using System.IO.Compression;

namespace Testing
{
    public partial class uploadFile : System.Web.UI.Page
    {
        static Stack dirs = new Stack();

        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.RowStyle.Height = 30;
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
            string path = dirs.Peek().ToString() + "\\" + fileName;

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





            // Folders shared with me
            DataTable dt2 = new DataTable();
            DataRow row;
            DataColumn column = new DataColumn("Name");
            column.DataType = Type.GetType("System.String");
            DataColumn column2 = new DataColumn("Username");
            column2.DataType = Type.GetType("System.String");
            dt2.Columns.Add(column);
            dt2.Columns.Add(column2);
            List<string> folders = SQL.getSharedFolderPath(userid);
            foreach (string i in folders)
            {
                row = dt2.NewRow();
                row["Name"] = "/" + Path.GetFileName(i) + "/";
                dt2.Rows.Add(row);
            }
            

            // Files
            List<int> fileids = SQL.getShareFileID(userid);
            
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
            LinkButton lb = (LinkButton)sender;
            GridViewRow grv = (GridViewRow)lb.NamingContainer;
            string filename = grv.Cells[0].Text;
            string filePath = getpath(sender);

            if (filename.Substring(0, 1).Equals("/"))
            {
                string source = dirs.Peek().ToString() + "\\" + filename.Substring(1,filename.Length-2);
                string mainfolder = Server.MapPath("~/temp/") + filename.Substring(1, filename.Length - 2);
                string dest = mainfolder + "\\" + filename.Substring(1,filename.Length-2);
                string zip = Server.MapPath("~/temp/") + filename.Substring(1,filename.Length-2) + ".zip";

                Directory.CreateDirectory(mainfolder);
                
                
                foreach (string dir in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dest + dir.Substring(source.Length));
                }
                List<string> filepaths = new List<string>(Directory.GetFiles(source, "*.*", System.IO.SearchOption.AllDirectories));
                foreach (string file_name in filepaths)
                {
                    File.Copy(file_name, dest + file_name.Substring(source.Length));
                    Security.DecryptFile(dest+file_name.Substring(source.Length),dest+ file_name.Substring(source.Length));
                }

                ZipFile.CreateFromDirectory(mainfolder, zip);
                Response.ClearContent();
                Response.ContentType = ContentType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(zip));
                Response.TransmitFile(zip);
                Response.Flush();
                Directory.Delete(mainfolder,true);
                File.Delete(zip);
                Response.End();
            
            }
            else
            {
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
            LinkButton lb = (LinkButton)sender;
            GridViewRow grv = (GridViewRow)lb.NamingContainer;
            string filename = grv.Cells[0].Text;
            if (filename.Substring(0, 1).Equals("/"))
            {
                string foldername = dirs.Peek().ToString() + "\\" + filename.Substring(1, filename.Length - 2);
                List<string> files = new List<string>(Directory.GetFiles(foldername, "*", SearchOption.AllDirectories));
                SQL.deleteFiles(files, SQL.getUserID(Username.Text));
                foreach (string i in files)
                {
                    File.Delete(i);
                }
                Directory.Delete(foldername);
            }
            else
            {
                string filePath = getpath(sender);
                File.Delete(filePath);
                SQL.deleteFile(filePath);
            }

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
            LinkButton lb = (LinkButton)sender;
            GridViewRow grv = (GridViewRow)lb.NamingContainer;
            string name = grv.Cells[0].Text;
            if (name.Substring(0,1).Equals("/"))
            {
                fileName.Text = name;
                string path = dirs.Peek().ToString() + name;
                retrieveSharedFolders(path);
            }
            /*
            int fileid = SQL.getFileID(filepath);
            fileName.Text = filename;
            retrieveSharedUsers(fileid);
            ModalPopupExtender2.Show();
        */}

        protected void fileshare(object sender, EventArgs e)
        {

            string filename = fileName.Text;
            string shareduser = sharedUser.Text;
            string user = Username.Text;
            int userid = SQL.getUserID(user);
            int shareduserid = SQL.getUserID(shareduser);

            if (filename.Substring(0, 1).Equals("/"))
            {
                string path = dirs.Peek().ToString() + "\\" + filename.Substring(1, filename.Length - 2);
                SQL.insertShareFolder(path,userid,shareduserid);
            }

            else
            {

                
                int fileid = SQL.getFileID(filename, userid);
                SQL.insertShareFile(fileid, shareduserid, user);

            }

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

            row = dt.NewRow();
            row["Name"] = back;
            row["Last modified"] = "--";
            dt.Rows.Add(row);

            for (int i = 0; i < dir.Count; i++)
            {
                row = dt.NewRow();
                row["Name"] = "/" + Path.GetFileName(dir[i]) + "/";
                row["Last Modified"] = "--";
                dt.Rows.Add(row);
            }

            for (int i = 0; i < files.Length; i++)
            {
                row = dt.NewRow();
                row["Name"] = Path.GetFileName(files[i]);
                row["Last Modified"] = File.GetLastWriteTime(files[i]);
                dt.Rows.Add(row);
            }



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
                row["Last Modified"] = "--";
                dt.Rows.Add(row);
            }

            for (int i = 0; i < files.Length; i++)
            {
                row = dt.NewRow();
                row["Name"] = Path.GetFileName(files[i]);
                row["Last Modified"] = File.GetLastWriteTime(files[i]);
                dt.Rows.Add(row);
            }

            return dt;
        }

        protected void fillTree(object sender, EventArgs e)
        {
            Label3.Text = "";
            ModalPopupExtender3.Show();
            LinkButton lb = (LinkButton)sender;
            GridViewRow grv = (GridViewRow)lb.NamingContainer;
            string name = grv.Cells[0].Text;
            Label3.Text += name;
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
            directorynode.NavigateUrl = "javascript:void(0)";
            directorynode.Target = "_self";
            directorynode.SelectAction = TreeNodeSelectAction.Select;
            return directorynode;
        }



        protected void move_Click(object sender, EventArgs e)
        {
            if (Label3.Text.Substring(0, 1).Equals("/"))
            {

                string foldername = Label3.Text;
                string path = Server.MapPath("~/App_Data/");
                string originalpath = "";

                List<TreeNode> tree = new List<TreeNode>();
                TreeNode node = dirlist.SelectedNode;

                while (node != null)
                {
                    tree.Add(node);
                    node = node.Parent;
                }

                tree.Reverse();

                foreach (TreeNode i in tree)
                {
                    path += i.Text + "\\";
                }

                path += Label3.Text.Substring(1, Label3.Text.Length - 2) + "\\";

                originalpath += dirs.Peek().ToString() + "\\" + Label3.Text.Substring(1, Label3.Text.Length - 2) + "\\";

                Directory.Move(originalpath, path);

                List<string> filepaths = new List<string>(Directory.GetFiles(path, "*", SearchOption.AllDirectories));
                List<string> filenames = new List<string>();
                int userid = SQL.getUserID(Username.Text);
                foreach (string i in filepaths)
                {
                    filenames.Add(Path.GetFileName(i));
                }

                SQL.moveFolder(filenames, userid, filepaths);
            }


            else
            {

                int id = SQL.getFileID(Label3.Text, SQL.getUserID(Username.Text));
                string filepath = SQL.getFilePaths(Label3.Text, SQL.getUserID(Username.Text));
                string path = Server.MapPath("~/App_data/");
                List<TreeNode> tree = new List<TreeNode>();
                TreeNode node = dirlist.SelectedNode;

                while (node != null)
                {
                    tree.Add(node);
                    node = node.Parent;
                }

                tree.Reverse();

                foreach (TreeNode i in tree)
                {
                    path += i.Text + "\\";
                }
                path += Label3.Text;

                SQL.moveFile(id, path);

                File.Move(filepath, path);
            }

            ModalPopupExtender3.Hide();


        }

        protected void dirlist_SelectedNodeChanged(object sender, EventArgs e)
        {

        }

        protected void addFolder_Click(object sender, EventArgs e)
        {
            ModalPopupExtender4.Show();
            // Popup for folder name
            // Directory.createdirectory
        }

        protected void cr8folder(object sender, EventArgs e)
        {
            string folder = foldername.Text;
            string path = dirs.Peek().ToString() + "\\" + folder;
            Directory.CreateDirectory(path);
        }
    }
}