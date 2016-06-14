using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FileTransfer
{
    public class SQL
    {
        public static void insertShareFile(int fileid, int userid, int shareduserid)
        {
            using (SqlConnection connection = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["FileDatabaseConnectionString2"].ConnectionString))
            {

                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "INSERT INTO [shareFiles] (fileID,userID,shareWith) VALUES ('" + fileid + "','" + userid + "','" + shareduserid + "');";
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
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

                rd.Close();
            }
            return fileid;
        }
    }
}