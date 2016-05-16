using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using FakturyMVC.Models.DALmodels;

namespace FakturyMVC.Controllers
{
    static class UserDAL
    {
        private static string _connectionString;

        static UserDAL()
        {
            string machineName = System.Environment.MachineName.ToUpper();
            _connectionString = ConfigurationManager.ConnectionStrings[machineName].ConnectionString;
        }

        public static void UserAdd(User user, int userId = 5)
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
                    cmd.Parameters.Add("@p_IsAdmin", SqlDbType.Bit).Value = user.IsAdmin;
                    cmd.Parameters.Add("@p_User", SqlDbType.Int).Value = userId;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public static List<User> UserSearch
        (
            string firstName = null,
            string lastName = null,
            string login = null,
            string email = null,
            bool? isAdmin = null,
            bool? isLogged = null,
            UserStatus? status = null,
            int pageNumber = 1,
            int rowsPerPage = Int32.MaxValue
        )
        {
            List<User> usersFound = new List<User>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "inv.usp_UsersSearch";
                    cmd.Parameters.Add("@p_FirstName", SqlDbType.NVarChar, 128).Value = firstName ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@p_LastName", SqlDbType.NVarChar, 128).Value = lastName ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@p_Login", SqlDbType.NVarChar, 32).Value = login ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@p_Email", SqlDbType.NVarChar, 128).Value = email ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@p_IsAdmin", SqlDbType.Bit).Value = isAdmin.HasValue ? isAdmin.Value : (object)DBNull.Value;
                    cmd.Parameters.Add("@p_IsLogged", SqlDbType.Bit).Value = isLogged.HasValue ? isLogged.Value : (object)DBNull.Value;
                    cmd.Parameters.Add("@p_Status", SqlDbType.TinyInt).Value = status.HasValue ? status.Value : (object)DBNull.Value;
                    cmd.Parameters.Add("@p_pageNumber", SqlDbType.Int).Value = pageNumber;
                    cmd.Parameters.Add("@p_rowsPerPage", SqlDbType.Int).Value = rowsPerPage;

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string uEmail = String.Empty;
                            int uId = reader.GetInt32(reader.GetOrdinal("Usr_Id"));
                            string uFirstName = reader.GetString(reader.GetOrdinal("Usr_FirstName"));
                            string uLastName = reader.GetString(reader.GetOrdinal("Usr_LastName"));
                            string uLogin = reader.GetString(reader.GetOrdinal("Usr_Login"));
                            if (reader[reader.GetOrdinal("Usr_Email")] != DBNull.Value)
                            {
                                uEmail = reader.GetString(reader.GetOrdinal("Usr_Email"));
                            }
                            bool uIsAdmin = reader.GetBoolean(reader.GetOrdinal("Usr_IsAdmin"));
                            UserStatus uStatus = (UserStatus)reader.GetInt16(reader.GetOrdinal("Usr_Status"));
                            bool uIsLogged = reader.GetBoolean(reader.GetOrdinal("Usr_IsLogged"));

                            usersFound.Add(new User(uFirstName, uLastName, uLogin, null, uEmail, uStatus, uIsAdmin, uIsLogged, uId));
                        }
                    }
                    connection.Close();
                }
            }

            return usersFound;
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

            if (code == 1)
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
