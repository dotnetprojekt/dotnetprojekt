using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    public class UserDAL
    {
        private static string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["LocalDb"].ConnectionString;

        public static void UserAdd(User user, int userId=5)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "inv.usp_UsersAdd";
                    cmd.Parameters.Add("@p_FirstName", SqlDbType.NVarChar, 128).Value = user.FirstName;
                    cmd.Parameters.Add("@p_LastName", SqlDbType.NVarChar, 128).Value = user.LastName;
                    cmd.Parameters.Add("@p_Login", SqlDbType.NVarChar, 32).Value = user.Login;
                    cmd.Parameters.Add("@p_PasswordHash", SqlDbType.NVarChar, 128).Value = user.PasswordHash;
                    cmd.Parameters.Add("@p_Email", SqlDbType.NVarChar, 128).Value = user.Email;
                    cmd.Parameters.Add("@p_IsAdmin", SqlDbType.Bit, 2048).Value = user.IsAdmin;
                    cmd.Parameters.Add("@p_User", SqlDbType.Int).Value = userId;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public static bool UserBlock(string login)
        {
            int code = 0;
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "inv.usp_UsersBlock";
                    cmd.Parameters.Add("@p_Login", SqlDbType.NVarChar, 32).Value = login;

                    connection.Open();
                    code = cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }

            if(code == 1)
            {
                return true;
            }
            return false;
        }

        public static bool UserUnblock(string login)
        {
            int code = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "inv.usp_UsersUnblock";
                    cmd.Parameters.Add("@p_Login", SqlDbType.NVarChar, 32).Value = login;

                    connection.Open();
                    code = cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }

            if (code == 1)
            {
                return true;
            }
            return false;
        }

        public static bool UserAccept(string login)
        {
            int code = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "inv.usp_UsersAccept";
                    cmd.Parameters.Add("@p_Login", SqlDbType.NVarChar, 32).Value = login;

                    connection.Open();
                    code = cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }

            if (code == 1)
            {
                return true;
            }
            return false;
        }

        public static bool UserLogin(string login, string password)
        {
            int code = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "inv.usp_UsersLogin";
                    cmd.Parameters.Add("@p_Login", SqlDbType.NVarChar, 32).Value = login;
                    cmd.Parameters.Add("@p_PasswordHash", SqlDbType.NVarChar, 128).Value = Utils.GetStringSha256Hash(password);

                    connection.Open();
                    code = cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }

            if (code == 1)
            {
                return true;
            }
            return false;
        }


        public static bool UserLogout(string login)
        {
            int code = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "inv.usp_UsersLogout";
                    cmd.Parameters.Add("@p_Login", SqlDbType.NVarChar, 32).Value = login;

                    connection.Open();
                    code = cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }

            if (code == 1)
            {
                return true;
            }
            return false;
        }
    }
}
