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
                String condition = "";
                int size = id.Count();
                for (int i = 0; i < size - 1; i++)
                {
                    condition += "" + id[i] + ",";
                }
                size -= 1;
                condition += id[size];

                List<String> filePaths = new List<String>();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "SELECT filePath FROM [UserFiles] WHERE fileID IN ('" + 3 + "');";
                cmd.Connection = connection;
                connection.Open();
                SqlDataReader rd = cmd.ExecuteReader();

                if (rd.HasRows)
                {
                    int count = 0;
                    while (rd.Read());
                    filePaths.Add(rd[count].ToString());
                    count++; 
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
    }
}