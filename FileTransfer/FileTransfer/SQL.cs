using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FileTransfer
{
    public class SQL
    {


        public static List<String> getSharedDataTable(List<int> fileid)
        {
            List<String> filenames = new List<String>();
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {

                int size = fileid.Count();
                SqlDataAdapter sda = new SqlDataAdapter();
                if (size > 0)
                {

                    string command = "SELECT fileName FROM [UserFiles] WHERE fileID IN ";
                    command += "(";
                    for (int i = 0; i < size - 1; i++)
                    {
                        command += fileid[i] + ",";
                    }

                    command += fileid[size - 1] + ");";

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = command;
                    cmd.Connection = connection;
                    connection.Open();
                    SqlDataReader rd = cmd.ExecuteReader();

                    if (rd.HasRows)
                    {
                        while (rd.Read())
                        {
                            filenames.Add(rd.GetString(0));
                        }
                    }
                    connection.Close();
                    rd.Close();

                }

                return filenames;
            }
        }

        public static DataTable getSharedUsers(int fileid)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                string command = "SELECT [User].Username FROM [User] INNER JOIN sharedFiles ON [User].Username = sharedFiles.shareduser WHERE sharedFiles.fileID = '" + fileid + "';";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = command;
                cmd.Connection = connection;
                connection.Open();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(dt);

                connection.Close();
                return dt;
            }
        }


        /*
        public static List<String> getFilePaths(List<int> fileid)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                List<String> filePaths = new List<String>();
                int size = fileid.Count();

                if (size > 0)
                {

                    String command = "SELECT filePath FROM [UserFiles] WHERE fileID IN ";
                    command += "(";
                    for (int i = 0; i < size - 1; i++)
                    {
                        command += fileid[i] + ",";
                    }

                    command += fileid[size - 1] + ");";

                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = command;
                    cmd.Connection = connection;
                    connection.Open();
                    SqlDataReader rd = cmd.ExecuteReader();

                    if (rd.HasRows)
                    {

                        while (rd.Read())
                        {
                            filePaths.Add(rd.GetString(0));
                        }

                    }
                    connection.Close();
                    rd.Close();
                }

                return filePaths;
            }
        }
        

        public static List<String> getFilePaths(int userid)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                List<String> filePaths = new List<String>();
                string command = "SELECT filePath FROM [UserFiles] WHERE userID = '" + userid + "';";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = command;
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader rd = cmd.ExecuteReader();

                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        filePaths.Add(rd.GetString(0));
                    }
                }
                connection.Close();
                rd.Close();
                return filePaths;
            }

        }

    */
        public static int getFileID(String filePath)
        {
            int fileid = 0;
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT fileID FROM [UserFiles] WHERE filePath = '" + filePath + "';";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    rd.Read();
                    fileid = rd.GetInt32(0);
                }
                connection.Close();
                rd.Close();
            }
            return fileid;
        }
        /*
        public static int getUserID(int fileid)
        {
            int userid = 0;
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT userID FROM [UserFiles] WHERE fileID = '" + fileid + "';";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    rd.Read();
                    userid = rd.GetInt32(0);
                }
                connection.Close();
                rd.Close();
            }
            return userid;
        }
        */

        public static void deleteFile(string filePath)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                int fileid = getFileID(filePath);
                cmd.CommandText = "DELETE FROM [sharedFiles] WHERE fileID = '" + fileid + "';";
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DELETE FROM [UserFiles] WHERE fileID = '" + fileid + "';";
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void removeSharedFile(int fileid)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                string command = "DELETE FROM [sharedFiles] WHERE fileID = '" + fileid + "';";
                cmd.CommandText = command;
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void moveFile(int fileid, string path)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UPDATE [UserFiles] SET filePath = '" + path + "' WHERE fileID = '" + fileid + "';";
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public static DataTable retrieveSharedFolders(string path)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT Username FROM [User] INNER JOIN sharedFolder ON sharedFolder.sharedWith = [User].Username WHERE folderPath = '" + path + "';";
                cmd.Connection = connection;
                connection.Open();
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                connection.Close();
                return dt;
            }
        }

        public static string getIV(string filePath)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                string IV = "";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT IV FROM [UserFiles] WHERE filePath = '" + filePath + "';";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    rd.Read();
                    IV = rd.GetString(0);
                }
                connection.Close();
                rd.Close();
                return IV;
            }
        }

        /*
         * 
         * 
         * 
         * 
         *    
         *    
         *    
         *          CHANGE ALL 'userID' FIELD TO 'Username'!
         * 
         *      
         * 
         * 
         * 
         * 
         * 
         */

        public static List<int> getShareFileID(string username)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                List<int> fileid = new List<int>();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT fileID FROM [sharedFiles] WHERE shareduser = '" + username + "';";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader rd = cmd.ExecuteReader();

                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        fileid.Add(rd.GetInt32(0));
                    }
                }
                rd.Close();
                connection.Close();
                return fileid;
            }
        }
        public static void insertShareFile(int fileid, string sharedusername, string username)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO [sharedFiles] (fileID,shareduser,Username) VALUES ('" + fileid + "','" + sharedusername + "','" + username + "');";
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void insertShareFolder(string path, string username, string sharedusername, string name)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandText = "INSERT INTO [sharedFolder] VALUES ('" + path + "', '" + sharedusername + "','" + username + "','" + name + "');";
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static List<String> getShareUser(string shareduser)
        {

            List<String> usernames = new List<String>();

            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                string command = "SELECT Username from [sharedFiles] WHERE shareduser = '" + shareduser + "';";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = command;
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader rd = cmd.ExecuteReader();

                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        usernames.Add(rd.GetString(0));
                    }
                }
                connection.Close();
                rd.Close();
            }
            return usernames;
        }


        public static string getFilePaths(string fileName, string username)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                string filePath = "";
                string command = "SELECT filePath from [UserFiles] WHERE fileName = '" + fileName + "' AND Username = '" + username + "';";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = command;
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader rd = cmd.ExecuteReader();

                if (rd.HasRows)
                {
                    rd.Read();
                    filePath = rd.GetString(0);
                }
                connection.Close();
                rd.Close();
                return filePath;
            }
        }

        public static int getFileID(String fileName, string username)
        {
            int fileid = 0;
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT fileID FROM [UserFiles] WHERE fileName = '" + fileName + "' AND Username = '" + username + "';";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    rd.Read();
                    fileid = rd.GetInt32(0);
                }
                connection.Close();
                rd.Close();
            }
            return fileid;
        }

        public static void deleteFiles(List<string> filePath)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                connection.Open();
                string path;
                int fileid;
                int count = filePath.Count;
                for (int i = 0; i < count; i++)
                {
                    path = filePath[i];
                    fileid = SQL.getFileID(path);
                    cmd.CommandText = "DELETE FROM [sharedFiles] WHERE fileID = '" + fileid + "';";
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = "DELETE FROM [UserFiles] WHERE fileID = '" + fileid + "';";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /*
        public static int getUserID(String username)
        {
            int userid = 0;
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT userID FROM [User] WHERE username = '" + username + "';";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    rd.Read();
                    userid = rd.GetInt32(0);
                }
                connection.Close();
                rd.Close();
            }
            return userid;
        }
        */
        /*
        public static String getUsername(int userid)
        {
            string username = "";
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT username FROM [User] WHERE userID = '" + userid + "';";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    rd.Read();
                    username = rd.GetString(0);
                }
                connection.Close();
                rd.Close();
            }
            return username;
        }
        */
        public static void insertFile(string fileName, int fileSize, string path, string username,string IV)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO [userFiles] (fileName,fileSize,filePath,Username,IV) VALUES ('" + fileName + "','" + fileSize + "','" + path + "','" + username + "','" + IV + "');";
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public static void moveFolder(List<string> filename, string username, List<string> filepath)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                string command;
                string path, name;
                int count = filename.Count;
                cmd.Connection = connection;
                connection.Open();
                for (int i = 0; i < count; i++)
                {
                    path = filepath[i];
                    name = filename[i];
                    command = "UPDATE [UserFiles] SET filePath = '" + path + "' WHERE fileName = '" + name + "' AND Username = '" + username + "';";
                    cmd.CommandText = command;
                    cmd.ExecuteNonQuery();
                }

                connection.Close();

            }
        }

        public static void deleteSharedFolder(string path, string sharedwith)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "DELETE FROM [sharedFolder] WHERE folderPath = '" + path + "' AND sharedWith = '" + sharedwith + "';";
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static List<string> getSharedFolderPath(string sharewith)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                List<string> folders = new List<string>();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT folderPath,User FROM [sharedFolder] WHERE sharedWith = '" + sharewith + "';";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        folders.Add(rd.GetString(0));
                    }
                }
                connection.Close();
                rd.Close();
                return folders;
            }
        }

        public static string getSharedFolderPath(string foldername,string username)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                string path = "";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT folderPath FROM [sharedFolder] WHERE folderName = '" + foldername + "' AND [User] = '" + username + "';";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    rd.Read();
                    path = rd.GetString(0);
                }
                connection.Close();
                rd.Close();
                return path;
            }
        }

        public static List<string> getSharedFolderUser(string sharedwith)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                List<string> Users = new List<string>();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT Username FROM [User] INNER JOIN sharedFolder ON sharedFolder.[User] = [User].Username WHERE sharedWith = '" + sharedwith + "';";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    while (rd.Read())
                    {
                        Users.Add(rd.GetString(0));
                    }
                }
                connection.Close();
                rd.Close();
                return Users;
            }
        }

        public static string getKey(string username)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                string key = "";
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT keys FROM [User] WHERE Username = '" + username + "';";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    rd.Read();
                    key = rd.GetString(0);
                }
                connection.Close();
                rd.Close();
                return key;
            }
        }

        public static void addUsedspace(int space, string username)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UPDATE [User] SET StorageUsed = StorageUsed + '" + space + "' WHERE Username = '" + username + "';";
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                cmd.Dispose();
            }
        }

        public static void removeUsedspace(int space,string username)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "UPDATE [User] SET StorageUsed = StorageUsed - '" + space + "' WHERE Username = '" + username + "';";
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
                cmd.Dispose();
            }
        }

        public static int getStorageSize(string username)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                int x = 0;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT StorageSize FROM [User] WHERE Username = '" + username + "';";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    rd.Read();
                    x = rd.GetInt32(0);
                }
                connection.Close();
                return x;
            }
        }

        public static int getUsedSpace(string username)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                int x = 0;
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT StorageUsed FROM [User] WHERE Username = '" + username + "';";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader rd = cmd.ExecuteReader();
                if (rd.HasRows)
                {
                    rd.Read();
                    x = rd.GetInt32(0);
                }
                connection.Close();
                return x;
            }
        }
    }
}

