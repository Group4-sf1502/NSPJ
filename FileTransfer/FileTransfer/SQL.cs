using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FileTransfer
{
    public class SQL
    {
        public static List<String> getFilePaths(List<int> id)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {
                List<String> filePaths = new List<String>();
                int size = id.Count();

                if (size > 0)
                {

                    String command = "SELECT filePath FROM [UserFiles] WHERE fileID IN ";
                    command += "(";
                    for (int i = 0; i < size - 1; i++)
                    {
                        command += id[i] + ",";
                    }

                    command += id[size - 1] + ");";

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

                else
                {
                    return filePaths;
                }
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



        public static void insertShareFile(int fileid, int shareduserid)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO [sharedFiles] (fileID,shareduser) VALUES ('" + fileid + "','" + shareduserid + "');";
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }
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
    }
}