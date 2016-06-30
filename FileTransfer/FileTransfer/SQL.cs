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

        public static DataTable getDataTable(int userid)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                DataTable dt = new DataTable();
                string command = "SELECT fileName,uploadTime FROM UserFiles WHERE userid = '" + userid + "';";
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
                string command = "SELECT [User].Username FROM [User] INNER JOIN sharedFiles ON [User].userID = sharedFiles.shareduser WHERE sharedFiles.fileID = '" + fileid + "';";
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

        public static List<String> getShareUser(int shareduser)
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


        public static string getFilePaths(string fileName, int userid)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                string filePath = "";
                string command = "SELECT filePath from [UserFiles] WHERE fileName = '" + fileName + "' AND userID = '" + userid + "';";
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


        public static List<int> getShareFileID(int userid)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                List<int> fileid = new List<int>();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT fileID FROM [sharedFiles] WHERE shareduser = '" + userid + "';";
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



        public static void insertShareFile(int fileid, int shareduserid,string username)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO [sharedFiles] (fileID,shareduser,Username) VALUES ('" + fileid + "','" + shareduserid + "','" + username + "');";
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static int getFileID(String fileName, int userid)
        {
            int fileid = 0;
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT fileID FROM [UserFiles] WHERE fileName = '" + fileName + "' AND userid = '" + userid + "';";
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
    }
}
