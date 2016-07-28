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
using nClam;

namespace Testing
{
    public partial class uploadFile : System.Web.UI.Page
    {
        static Stack dirs = new Stack();
        static Stack shareddir = new Stack();

        protected void Page_Load(object sender, EventArgs e)
        {

            GridView1.RowStyle.Height = 30;
            GridView2.RowStyle.Height = 30;
            if (!IsPostBack)
            {
                dirs.Clear();
                shareddir.Clear();
            }

        }

        protected void upload(object sender, EventArgs e)
        {
            try
            {
                var clam = new ClamClient("localhost", 3310);
                string username = Username.Text;
                int remainingstorage = SQL.getStorageSize(username) - SQL.getUsedSpace(username);
                HttpFileCollection uploadedFiles = Request.Files;
                for (int i = 0; i < uploadedFiles.Count; i++)
                {
                    HttpPostedFile userPostedFile = uploadedFiles[i];

                    if (userPostedFile.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(userPostedFile.FileName);
                        string path = dirs.Peek().ToString() + "\\" + fileName;
                        string dest = Server.MapPath("~/temp/") + fileName;
                        userPostedFile.SaveAs(dest);
                        var scanResult = clam.ScanFileOnServer(dest);
                        if (scanResult.Result == ClamScanResults.Clean)
                        {
                            if (userPostedFile.ContentLength < remainingstorage)
                            {
                                string IV = Security.EncryptFile(username, dest, path);
                                SQL.insertFile(fileName, userPostedFile.ContentLength, path, username, IV);
                                int space = (int)new FileInfo(path).Length;
                                SQL.addUsedspace(space, username);
                                remainingstorage -= space;
                            }
                            else
                            {
                                Response.Write("File is too big!");
                            }

                        }
                        else
                        {
                            File.Delete(path);
                            Response.Write(fileName + " is malicious!");
                        }
                        File.Delete(dest);
                    }
                }

                //Refresh table after upload
                DataTable dt;
                if (dirs.Count == 1)
                {
                    dt = fillMainTable(dirs.Peek().ToString());
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
                else
                {
                    dt = fillTable(dirs.Peek().ToString());
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }


            }
            catch (InvalidOperationException exc)
            {
                Response.Write("An error has occured");
            }

        }


        protected void retrieve(object sender, EventArgs e)
        {
            try
            {
                dirs.Clear();
                //int userid = SQL.getUserID(Username.Text);
                string username = Username.Text;

                //My files

                string user = Server.MapPath("~/App_Data/") + username;
                dirs.Push(user);
                DataTable dt = fillMainTable(user);
                GridView1.DataSource = dt;
                GridView1.DataBind();
                MultiView.ActiveViewIndex = 0;

                DirectoryInfo di = new DirectoryInfo(user);
                List<FileInfo> filelist = new List<FileInfo>(di.GetFiles());


                // Folders shared with me
                DataTable dt2 = new DataTable();
                DataRow row;
                DataColumn column = new DataColumn("Name");
                column.DataType = Type.GetType("System.String");
                DataColumn column2 = new DataColumn("Username");
                column2.DataType = Type.GetType("System.String");
                dt2.Columns.Add(column);
                dt2.Columns.Add(column2);
                List<string> folders = SQL.getSharedFolderPath(username);
                List<string> Usernames = SQL.getSharedFolderUser(username);
                for (int i = 0; i < folders.Count(); i++)
                {
                    row = dt2.NewRow();
                    row["Name"] = "/" + Path.GetFileName(folders[i]) + "/";
                    row["Username"] = Usernames[i];
                    dt2.Rows.Add(row);
                }


                // Files
                List<int> fileids = SQL.getShareFileID(username);

                List<String> files = SQL.getSharedDataTable(fileids);
                List<String> usernames = SQL.getShareUser(username);

                for (int i = 0; i < files.Count(); i++)
                {
                    row = dt2.NewRow();
                    row["Name"] = files[i];
                    row["Username"] = usernames[i];
                    dt2.Rows.Add(row);
                }


                GridView2.DataSource = dt2;
                GridView2.DataBind();
            }
            catch (FileNotFoundException exc)
            {
                Response.Write("File not found!");
            }
            catch (DirectoryNotFoundException exc2)
            {
                Response.Write("Folder not found");
            }

        }

        protected void DownloadFile(object sender, EventArgs e)
        {
            try
            {
                string filename = getname(sender);
                string filePath = getpath(sender);
                string username = Username.Text;
                //int userid = SQL.getUserID(Username.Text);

                //Test if folder
                if (filename.Substring(0, 1).Equals("/"))
                {
                    string source = dirs.Peek().ToString() + "\\" + filename.Substring(1, filename.Length - 2);
                    string mainfolder = Server.MapPath("~/temp/") + filename.Substring(1, filename.Length - 2);
                    string dest = mainfolder + "\\" + filename.Substring(1, filename.Length - 2);
                    string zip = Server.MapPath("~/temp/") + filename.Substring(1, filename.Length - 2) + ".zip";


                    Directory.CreateDirectory(mainfolder);
                    Directory.CreateDirectory(dest);

                    //Replicate folder & subfolders
                    foreach (string dir in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
                    {
                        Directory.CreateDirectory(dest + dir.Substring(source.Length));
                    }

                    string IV;

                    List<string> filepaths = new List<string>(Directory.GetFiles(source, "*.*", SearchOption.AllDirectories));
                    //Replicate files in folder & subfolders
                    foreach (string file_name in filepaths)
                    {
                        IV = SQL.getIV(file_name);
                        Security.DecryptFile(username, file_name, dest + file_name.Substring(source.Length), IV);
                    }
                    //Create zip file
                    ZipFile.CreateFromDirectory(mainfolder, zip);
                    Response.ClearContent();
                    Response.ContentType = ContentType;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(zip));
                    Response.TransmitFile(zip);
                    Response.Flush();
                    Directory.Delete(mainfolder, true);
                    File.Delete(zip);
                    Response.End();

                }
                //Download sequence for files
                else
                {
                    string IV = SQL.getIV(filePath);
                    string tempPath = Server.MapPath("~/temp/") + filename;
                    Security.DecryptFile(username, filePath, tempPath, IV);
                    Response.ClearContent();
                    Response.ContentType = ContentType;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(tempPath));
                    Response.TransmitFile(tempPath);
                    Response.Flush();
                    File.Delete(tempPath);
                    Response.End();
                }
            }
            catch (FileNotFoundException exc)
            {
                Response.Write("File not found");
            }
            catch (InvalidOperationException exc2)
            {
                Response.Write("An error has occured");
            }
            catch (DirectoryNotFoundException exc3)
            {
                Response.Write("Folder not found");
            }

        }

        protected void DownloadSharedFile(object sender, EventArgs e)
        {

            string filename = getname(sender);
            string mainfolder;
            string source;
            string dest;
            string zip;
            //Test if folder
            if (filename.Substring(0, 1).Equals("/"))
            {
                //Overview of shared folders + files
                if (shareddir.Count == 0)
                {
                    string user = getshareduser(sender);
                    //int userid = SQL.getUserID(user);
                    source = SQL.getSharedFolderPath(filename, user);
                    mainfolder = Server.MapPath("~/temp/") + filename.Substring(1, filename.Length - 2);
                    dest = mainfolder + "\\" + filename.Substring(1, filename.Length - 2);
                    zip = Server.MapPath("~/temp/") + filename.Substring(1, filename.Length - 2) + ".zip";
                    Directory.CreateDirectory(mainfolder);
                    Directory.CreateDirectory(dest);


                    foreach (string dir in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
                    {
                        Directory.CreateDirectory(dest + dir.Substring(source.Length));
                    }

                    List<string> filepaths = new List<string>(Directory.GetFiles(source, "*.*", System.IO.SearchOption.AllDirectories));
                    foreach (string file_path in filepaths)
                    {
                        string IV = SQL.getIV(file_path);
                        Security.DecryptFile(user, file_path, dest + file_path.Substring(source.Length), IV);
                    }
                }
                //Specific shared folders
                else
                {
                    string user = getshareduser(sender);
                    //int userid = SQL.getUserID(user);
                    source = shareddir.Peek().ToString() + "\\" + filename.Substring(1, filename.Length - 2);
                    mainfolder = Server.MapPath("~/temp/") + filename.Substring(1, filename.Length - 2);
                    dest = mainfolder + "\\" + filename.Substring(1, filename.Length - 2);
                    zip = Server.MapPath("~/temp/") + filename.Substring(1, filename.Length - 2) + ".zip";

                    Directory.CreateDirectory(mainfolder);
                    Directory.CreateDirectory(dest);


                    foreach (string dir in Directory.GetDirectories(source, "*", SearchOption.AllDirectories))
                    {
                        Directory.CreateDirectory(dest + dir.Substring(source.Length));
                    }

                    List<string> filepaths = new List<string>(Directory.GetFiles(source, "*.*", System.IO.SearchOption.AllDirectories));
                    foreach (string file_path in filepaths)
                    {
                        string IV = SQL.getIV(file_path);
                        Security.DecryptFile(user, file_path, dest + file_path.Substring(source.Length), IV);
                    }
                }

                //Create zip
                ZipFile.CreateFromDirectory(mainfolder, zip);
                Response.ClearContent();
                Response.ContentType = ContentType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(zip));
                Response.TransmitFile(zip);
                Response.Flush();
                Directory.Delete(mainfolder, true);
                File.Delete(zip);
                Response.End();

            }
            //Shared files
            else
            {
                string fileName = getname(sender);
                string filePath = getsharedpath(sender);
                string user = getshareduser(sender);
                //int userid = SQL.getUserID(user);
                string IV = SQL.getIV(filePath);
                string tempPath = Server.MapPath("~/temp/") + filename;
                Security.DecryptFile(user, filePath, tempPath, IV);
                Response.ClearContent();
                Response.ContentType = ContentType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(tempPath));
                Response.TransmitFile(tempPath);
                Response.Flush();
                File.Delete(tempPath);
                Response.End();

            }

        }
        protected void DeleteFile(object sender, EventArgs e)
        {
            try
            {
                string username = Username.Text;
                string filename = getname(sender);
                if (filename.Substring(0, 1).Equals("/"))
                {
                    string foldername = dirs.Peek().ToString() + "\\" + filename.Substring(1, filename.Length - 2);
                    List<string> files = new List<string>(Directory.GetFiles(foldername, "*", SearchOption.AllDirectories));
                    SQL.deleteFiles(files);
                    foreach (string i in files)
                    {
                        SQL.removeUsedspace((int) new FileInfo(i).Length, username);
                        File.Delete(i);
                    }
                    Directory.Delete(foldername);
                }
                else
                {
                    string filePath = getpath(sender);
                    SQL.removeUsedspace((int)new FileInfo(filePath).Length, username);
                    File.Delete(filePath);
                    SQL.deleteFile(filePath);
                }
                //Refresh gridview
                DataTable dt = new DataTable();
                if (dirs.Count == 1)
                {
                    dt = fillMainTable(dirs.Peek().ToString());
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
                else
                {
                    dt = fillTable(dirs.Peek().ToString());
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
            }
            catch (FileNotFoundException exc)
            {
                Response.Write("File not found");
            }

            catch (DirectoryNotFoundException exc2)
            {
                Response.Write("Folder not found");
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

        //Popup for sharing files
        protected void showpopup(object sender, EventArgs e)
        {
            DataTable dt;
            string name = getname(sender);
            if (name.Substring(0, 1).Equals("/"))
            {
                fileName.Text = name;
                string path = dirs.Peek().ToString() + "\\" + name.Substring(1, name.Length - 2);
                dt = SQL.retrieveSharedFolders(path);
                sharedList.DataSource = dt;
                sharedList.DataBind();

            }
            else
            {
                string filepath = getpath(sender);
                int fileid = SQL.getFileID(filepath);
                fileName.Text = name;
                dt = SQL.getSharedUsers(fileid);
                sharedList.DataSource = dt;
                sharedList.DataBind();
            }
            ModalPopupExtender2.Show();
        }

        protected void fileshare(object sender, EventArgs e)
        {

            string filename = fileName.Text;
            string shareduser = sharedUser.Text;
            string user = Username.Text;
            //int userid = SQL.getUserID(user);
            //int shareduserid = SQL.getUserID(shareduser);

            if (filename.Substring(0, 1).Equals("/"))
            {
                string path = dirs.Peek().ToString() + "\\" + filename.Substring(1, filename.Length - 2);
                SQL.insertShareFolder(path, user, shareduser, filename);
            }

            else
            {
                int fileid = SQL.getFileID(filename, user);
                SQL.insertShareFile(fileid, shareduser, user);
            }

        }
        //Remove shared files
        protected void RemoveFile(object sender, EventArgs e)
        {
            string filename = getname(sender);
            string user = Username.Text;
            if (filename.Substring(0, 1).Equals("/"))
            {
                string shareduser = getshareduser(sender);
                string source = SQL.getSharedFolderPath(filename, shareduser);
                SQL.deleteSharedFolder(source, user);
            }
            else
            {
                string filePath = getsharedpath(sender);
                int fileid = SQL.getFileID(filePath);
                SQL.removeSharedFile(fileid);
            }

            //Refresh table
            DataTable dt;
            if (shareddir.Count == 0)
            {
                dt = fillMainSharedTable(user);
            }
            else
            {
                dt = fillSharedTable(shareddir.Peek().ToString(), getshareduser(sender));
            }

            GridView2.DataSource = dt;
            GridView2.DataBind();


        }

        protected void RemoveShare(object sender, EventArgs e)
        {
            DataTable dt;
            if (fileName.Text.Substring(0, 1).Equals("/"))
            {
                string sharedwith = getname(sender);
                //int shareduser = SQL.getUserID(sharedwith);
                string folderpath = dirs.Peek().ToString() + "\\" + fileName.Text.Substring(1, fileName.Text.Length - 2);
                SQL.deleteSharedFolder(folderpath, sharedwith);
                dt = SQL.retrieveSharedFolders(folderpath);
                sharedList.DataSource = dt;
                sharedList.DataBind();
            }
            else
            {
                int fileid = SQL.getFileID(fileName.Text, Username.Text);
                SQL.removeSharedFile(fileid);
                dt = SQL.getSharedUsers(fileid);
                sharedList.DataSource = dt;
                sharedList.DataBind();
            }
        }

        //Apply CSS style
        protected void rowdatabind(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.CssClass = "header";
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.CssClass = "normal";
                if (e.Row.Cells[1].Text.Substring(0,1).Equals("/"))
                {
                    Image hello = e.Row.FindControl("image") as Image;
                    hello.ImageUrl = "images/folder.png";
                    hello.BorderStyle = BorderStyle.None;

                }

                else if (e.Row.Cells[1].Text.Substring(0,2).Equals(".."))
                {
                    Image hello = e.Row.FindControl("image") as Image;
                }

                else
                {
                    Image hello = e.Row.FindControl("image") as Image;
                    hello.ImageUrl = "images/document.png";
                    hello.BorderStyle = BorderStyle.None;
                }
            }
        }

        //Add css when converting ASP.NET to HTML for client side
        protected override void Render(HtmlTextWriter writer)
        {
            string condition = "/";
            string condition2 = "..";
            foreach (GridViewRow r in GridView1.Rows)
            {
                if (r.RowType == DataControlRowType.DataRow)
                {
                    if ((r.Cells[1].Text.Substring(0, 1).Equals(condition)) || (r.Cells[1].Text.Substring(0, 2).Equals(condition2)))
                    {
                        r.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                        r.ToolTip = "Click to select row";
                        r.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink(this.GridView1, "Select$" + r.RowIndex, true);
                    }

                }
            }

            foreach (GridViewRow r in GridView2.Rows)
            {
                if (r.RowType == DataControlRowType.DataRow)
                {
                    if ((r.Cells[1].Text.Substring(0, 1).Equals(condition)) || (r.Cells[1].Text.Substring(0, 2).Equals(condition2)))
                    {
                        r.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                        r.ToolTip = "Click to select row";
                        r.Attributes["onclick"] = this.Page.ClientScript.GetPostBackClientHyperlink(this.GridView2, "Select$" + r.RowIndex, true);
                    }

                }
            }

            base.Render(writer);
        }

        private string getname(object sender)
        {
            LinkButton lb = (LinkButton)sender;
            GridViewRow grv = (GridViewRow)lb.NamingContainer;
            string filename = grv.Cells[1].Text;
            return filename;
        }

        private string getshareduser(object sender)
        {
            LinkButton lb = (LinkButton)sender;
            GridViewRow grv = (GridViewRow)lb.NamingContainer;
            string user = grv.Cells[2].Text;
            return user;
        }


        private string getpath(object sender)
        {
            LinkButton lb = (LinkButton)sender;
            GridViewRow grv = (GridViewRow)lb.NamingContainer;
            string filename = grv.Cells[1].Text;
            string user = Username.Text;
            string filePath = SQL.getFilePaths(filename, user);
            return filePath;
        }

        private string getsharedpath(object sender)
        {
            LinkButton lb = (LinkButton)sender;
            GridViewRow grv = (GridViewRow)lb.NamingContainer;
            string filename = grv.Cells[1].Text;
            string username = grv.Cells[2].Text;
            string filePath = SQL.getFilePaths(filename, username);
            return filePath;
        }

        //Clicking on rows in gv1
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string foldername;
            string path;
            DataTable dt;
            foreach (GridViewRow row in GridView1.Rows)
            {
                if (row.RowIndex == GridView1.SelectedIndex)
                {
                    if (row.Cells[1].Text.Equals(".."))
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
                        foldername = row.Cells[1].Text.Substring(1, row.Cells[1].Text.Length - 2);
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

        //Clicking on rows in gv2
        protected void GridView2_SelectedIndexChanged(object sender, EventArgs e)
        {

            string foldername;
            string path;
            DataTable dt;
            foreach (GridViewRow row in GridView2.Rows)
            {
                if (row.RowIndex == GridView2.SelectedIndex)
                {
                    if (row.Cells[1].Text.Equals(".."))
                    {
                        shareddir.Pop();
                        if (shareddir.Count == 0)
                        {
                            string username = Username.Text;
                            dt = fillMainSharedTable(username);
                        }

                        else
                        {
                            path = shareddir.Peek().ToString();
                            dt = fillSharedTable(path, row.Cells[2].Text);
                        }

                    }

                    else
                    {
                        if (shareddir.Count == 0)
                        {
                            string username = row.Cells[2].Text;
                            foldername = row.Cells[1].Text;
                            path = SQL.getSharedFolderPath(foldername, username);
                            shareddir.Push(path);
                            dt = fillSharedTable(path, row.Cells[2].Text);

                        }

                        else
                        {
                            path = shareddir.Peek().ToString() + "\\" + row.Cells[1].Text.ToString().Substring(1, row.Cells[1].Text.ToString().Length - 2);
                            shareddir.Push(path);
                            dt = fillSharedTable(path, row.Cells[2].Text);

                        }
                    }

                    GridView2.DataSource = null;
                    GridView2.DataBind();

                    GridView2.DataSource = dt;
                    GridView2.DataBind();
                }
            }

        }

        protected DataTable fillMainSharedTable(string username)
        {
            DataTable dt2 = new DataTable();
            DataRow row;
            DataColumn column = new DataColumn("Name");
            column.DataType = Type.GetType("System.String");
            DataColumn column2 = new DataColumn("Username");
            column2.DataType = Type.GetType("System.String");
            dt2.Columns.Add(column);
            dt2.Columns.Add(column2);
            List<string> folders = SQL.getSharedFolderPath(username);
            List<string> Usernames = SQL.getSharedFolderUser(username);
            for (int i = 0; i < folders.Count(); i++)
            {
                row = dt2.NewRow();
                row["Name"] = "/" + Path.GetFileName(folders[i]) + "/";
                row["Username"] = Usernames[i];
                dt2.Rows.Add(row);
            }


            // Files
            List<int> fileids = SQL.getShareFileID(username);

            List<String> files = SQL.getSharedDataTable(fileids);
            List<String> usernames = SQL.getShareUser(username);

            for (int i = 0; i < files.Count(); i++)
            {
                row = dt2.NewRow();
                row["Name"] = files[i];
                row["Username"] = usernames[i];
                dt2.Rows.Add(row);
            }


            return dt2;
        }

        protected DataTable fillSharedTable(string path, string username)
        {
            string back = "..";
            DataTable dt = new DataTable();
            List<string> dir = new List<string>(Directory.EnumerateDirectories(path));
            string[] files = Directory.GetFiles(path);
            DataRow row;
            DataColumn column = new DataColumn("Name");
            column.DataType = Type.GetType("System.String");
            DataColumn column2 = new DataColumn("Username");
            column2.DataType = Type.GetType("System.String");
            dt.Columns.Add(column);
            dt.Columns.Add(column2);

            row = dt.NewRow();
            row["Name"] = back;
            dt.Rows.Add(row);

            for (int i = 0; i < dir.Count; i++)
            {
                row = dt.NewRow();
                row["Name"] = "/" + Path.GetFileName(dir[i]) + "/";
                row["Username"] = username;
                dt.Rows.Add(row);
            }

            for (int i = 0; i < files.Length; i++)
            {
                row = dt.NewRow();
                row["Name"] = Path.GetFileName(files[i]);
                row["Username"] = username;
                dt.Rows.Add(row);
            }

            return dt;
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
            DataColumn column3 = new DataColumn("Size");
            column3.DataType = Type.GetType("System.String");
            DataColumn column2 = new DataColumn("Last modified");
            column2.DataType = Type.GetType("System.String");
            dt.Columns.Add(column);
            dt.Columns.Add(column2);
            dt.Columns.Add(column3);

            row = dt.NewRow();
            row["Name"] = back;
            row["Size"] = "--";
            row["Last modified"] = "--";
            dt.Rows.Add(row);

            for (int i = 0; i < dir.Count; i++)
            {
                row = dt.NewRow();
                row["Name"] = "/" + Path.GetFileName(dir[i]) + "/";
                row["Size"] = "--";
                row["Last Modified"] = "--";
                dt.Rows.Add(row);
            }

            for (int i = 0; i < files.Length; i++)
            {
                string length = size(new FileInfo(files[i])); row = dt.NewRow();
                row["Name"] = Path.GetFileName(files[i]);
                row["Size"] = length;
                row["Last Modified"] = File.GetLastWriteTime(files[i]);
                dt.Rows.Add(row);
            }


            updatespace();
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
            DataColumn column3 = new DataColumn("Size");
            column3.DataType = Type.GetType("System.String");
            DataColumn column2 = new DataColumn("Last modified");
            column2.DataType = Type.GetType("System.String");
            dt.Columns.Add(column);
            dt.Columns.Add(column2);
            dt.Columns.Add(column3);

            for (int i = 0; i < dir.Count; i++)
            {
                row = dt.NewRow();
                row["Name"] = "/" + Path.GetFileName(dir[i]) + "/";
                row["Last Modified"] = "--";
                dt.Rows.Add(row);
            }

            for (int i = 0; i < files.Length; i++)
            {
                string length = size(new FileInfo(files[i]));
                row = dt.NewRow();
                row["Name"] = Path.GetFileName(files[i]);
                row["Size"] = length;
                row["Last Modified"] = File.GetLastWriteTime(files[i]);
                dt.Rows.Add(row);
            }
            updatespace();
            return dt;
        }

        private string size(FileInfo fInf)
        {
            string sLen = fInf.Length.ToString();
            if (fInf.Length >= (1 << 30))
                sLen = string.Format("{0}GB", fInf.Length >> 30);
            else
            if (fInf.Length >= (1 << 20))
                sLen = string.Format("{0}MB", fInf.Length >> 20);
            else
            if (fInf.Length >= (1 << 10))
                sLen = string.Format("{0}KB", fInf.Length >> 10);
            return sLen;
        }

        protected void fillTree(object sender, EventArgs e)
        {
            Label3.Text = "";
            ModalPopupExtender3.Show();
            LinkButton lb = (LinkButton)sender;
            GridViewRow grv = (GridViewRow)lb.NamingContainer;
            string name = grv.Cells[1].Text;
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
                //int userid = SQL.getUserID(Username.Text);
                string username = Username.Text;
                foreach (string i in filepaths)
                {
                    filenames.Add(Path.GetFileName(i));
                }

                SQL.moveFolder(filenames, username, filepaths);

            }


            else
            {

                int id = SQL.getFileID(Label3.Text, Username.Text);
                string filepath = SQL.getFilePaths(Label3.Text, Username.Text);
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
            DataTable dt;
            if (dirs.Count == 1)
            {
                dt = fillMainTable(dirs.Peek().ToString());
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            else
            {
                dt = fillTable(dirs.Peek().ToString());
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }

        }

        protected void addFolder_Click(object sender, EventArgs e)
        {
            ModalPopupExtender4.Show();
        }

        protected void cr8folder(object sender, EventArgs e)
        {
            string folder = foldername.Text;
            string path = dirs.Peek().ToString() + "\\" + folder;
            if (Directory.Exists(path))
            {
                path += " -Copy";
                Directory.CreateDirectory(path);
            }
            else
            {
                Directory.CreateDirectory(path);
            }
            DataTable dt;
            if (dirs.Count == 1)
            {
                dt = fillMainTable(dirs.Peek().ToString());
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }
            else
            {
                dt = fillTable(dirs.Peek().ToString());
                GridView1.DataSource = dt;
                GridView1.DataBind();
            }


            /*
             * using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "UPDATE [User] SET keys = '" + encryptedkey + "' WHERE userID = 11;";
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
             * using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = "SELECT keys FROM [User] WHERE userID = 11";
                    connection.Open();
                    SqlDataReader rd = cmd.ExecuteReader();
                    rd.Read();
                    encryptedkey = rd.GetString(0);
                    string decryptedkey = Security.DecryptKey(encryptedkey);
                    Username.Text += decryptedkey;
                }
             * 
             * 
             * iZ8duv2/Zq1jqPGbFwbv4NZ/QLLljQCIhQ8h1ADbDVI=
        NPNxRsrC/DXyAji7fXAEAyR9O5JP6AxjdCQHOTWrq3AzXTshbZ2CwCcyJ3MF2WGHTU1jcYpQAtSRnDaAi0CzPuYs+1KhVUY2e3yBOQ7Rq4co1Jq6PwfsFpXoWC4P3I5MV+5bo8cOFUqnZbwdbkMUeJehyzSVk/6a1BtJfya5f/M=


    LMnSA/3T3ONADqTxWHk53ZwvvA0tROwCPAnTyq4zJxY=
    NrwTNW1bzPT5ZqeUkniw1y38Z4sdmNSklQV3tknhnqLxg1a+547iNtxpNPweDGDEl5x1PV19PRdGLDWPMxUVfVWBnotyVZBkqb1zhZnojD77Pu2ym4mWdGO0w+Hg7G1UjAM//f3RkKBAAU2KRVqZE4GYTvnz0i/yrm81FsRQk8w=


    qFUMGMVgTJQDYwQ+MlmfavoI5JsjPcmGuWdEHO7kJhY=
    GvGLWL+TG0DY8DlZdybqT/gl1nllbtMOdZ56tlz0spIo5WWF9PmLby1PtQ1Uw7PyqsbrjpS7A2/w6zeVyGqLTJKLS8IPycR0ykc7dBPcAutC45ylbQbVaT5vQqiswfyIulvkKLCT9hV1UxFT0Z44NAF+H2j+XQ2Jupvw4i+qmsM=
             * 
             * 
             * 
             * 
             * 
             * 
             * 
             * 
             * 
             * 


            /*
    protected void Button8_Click(object sender, EventArgs e)
    {
    TextBox1.Text += "Helo";
    var clam = new ClamClient("localhost", 3310);
    var scanResult = clam.ScanFileOnServer(@"C:\Users\daryl\Desktop\z.com");

    switch (scanResult.Result)
    {
    case ClamScanResults.Clean:
    Response.Write("The file is clean!");
    break;
    case ClamScanResults.VirusDetected:
    Response.Write("Virus Found!");
    break;
    case ClamScanResults.Error:
    Response.Write("Error occurred");
    break;
    }
    }*/
            /*
    private string changedirection(SortDirection e)
    {
       string newdirection = "";
       switch (e)
       {
           case SortDirection.Ascending: newdirection = "DESC";break;
           case SortDirection.Descending: newdirection = "ASC";break;
       }
       return newdirection;
    }

    protected void sortview(object sender, GridViewSortEventArgs e)
    {
       Response.Write("Sort expression: " + e.SortExpression);
       Response.Write("Sort direction: " + e.SortDirection);

       DataTable dt = GridView1.DataSource as DataTable;

       if (dt != null)
       {
           DataView dv = new DataView(dt);
           dv.Sort = e.SortExpression + " " + "DESC";

           GridView1.DataSource = dv;
           GridView1.DataBind();
       }
    } 
    */

        }
        private void updatespace()
        {
            string username = Username.Text;
            double space = (double)SQL.getUsedSpace(username) / (double)SQL.getStorageSize(username) * 100.0;
            Label5.Text = "" + Math.Round(space, 2) + "%";
        }
    }
}