using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class clsTaskDataAccess
    {

        public static DataTable GetAllTasks(int UserID)
        {
            DataTable dt = new DataTable(); 

            SqlConnection connection = new SqlConnection(clsSettings.ConnetionString);
            string query = @"select TaskID, Title, Description, StartDate, EndDate, IsActive ,Tasks.UserID from Tasks  Where Tasks.UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch 
            {

            }
            finally
            {
                connection.Close();
            }
            return dt;
        }

        public static int AddNewTask( string Title, string Description, DateTime StartDate,
            DateTime EndDate, bool IsActive , int UserID)
        {
            int TaskID = -1;

            SqlConnection connection = new SqlConnection(clsSettings.ConnetionString);
            string query = @"insert into Tasks ( Title, Description, StartDate, EndDate, IsActive ,UserID)
                values (@Title, @Description, @StartDate, @EndDate, @IsActive ,@UserID);
                Select Scope_identity();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Title", Title);
            command.Parameters.AddWithValue("@Description", Description);
            command.Parameters.AddWithValue("@StartDate", StartDate);
            command.Parameters.AddWithValue("@EndDate", EndDate);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int insertID))
                {
                    TaskID = insertID;
                }
            }
            catch
            {
                //Write The Exception
            }
            finally
            {
                connection.Close() ;
            }
            return TaskID;  
        }

        public static bool UpdateTask(int TaskID, string Title, string Description,  DateTime StartDate,
            DateTime EndDate, bool IsActive, int UserID)
        {
            int rowEffected;

            SqlConnection connection = new SqlConnection (clsSettings.ConnetionString);
            string query = @"update Tasks
                set Title = @Title, Description = @Description,StartDate = @StartDate,
                EndDate = @EndDate,IsActive = @IsActive, UserID = @UserID
                where TaskID = @TaskID";

            SqlCommand command = new SqlCommand (query, connection);

            command.Parameters.AddWithValue("@Title", Title);
            command.Parameters.AddWithValue("@Description", Description);
            command.Parameters.AddWithValue("@StartDate", StartDate);
            command.Parameters.AddWithValue("@EndDate", EndDate);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@UserID", UserID);
            command.Parameters.AddWithValue("@TaskID", TaskID);

            try
            {
                connection.Open();

                rowEffected = command.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return (rowEffected > 0);

        }

        public static bool DeleteTask(int TaskID)
        {
            int rowEffected;
            SqlConnection connection = new SqlConnection(clsSettings.ConnetionString);
            string query = "Delete From Tasks Where TaskID = @TaskID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TaskID", TaskID);

            try
            {
                connection.Open();

                rowEffected = command.ExecuteNonQuery();
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
            return (rowEffected > 0);
        }

       
        public static bool FindByTaskID(int TaskID, ref string Title, ref string Description,
            ref DateTime StartDate, ref DateTime EndDate, ref bool IsActive,ref int UserID)
        {
            bool IsFound;

            SqlConnection connection = new SqlConnection(clsSettings.ConnetionString);
            string query = "select * from Tasks WHere TaskID = @TaskID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TaskID", TaskID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    IsFound = true;
                    
                    Title = (string)reader["Title"];
                    Description = (string)reader["Description"];
                    StartDate = (DateTime)reader["StartDate"];
                    EndDate = (DateTime)reader["EndDate"];
                    IsActive = (bool)reader["IsActive"];
                    UserID = (int)reader["UserID"];

                    
                }
                else
                    IsFound = false;
                reader.Close();

            }
            catch
            {
                IsFound = false;
            }
            finally
            {
                connection.Close();
            }
            return IsFound;
        }


    }
}
