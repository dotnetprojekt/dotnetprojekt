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

    public sealed class PartnerDAL
    {
        private static volatile PartnerDAL instance;
        private static object syncRoot = new Object();
        private string _connectionString = ConfigurationManager.ConnectionStrings[System.Environment.MachineName.ToUpper()].ConnectionString;

        private PartnerDAL()
        {

        }

        public static PartnerDAL Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new PartnerDAL();
                        }
                    }
                }
                return instance;
            }
        }

        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        public void PartnerAdd(Partner partner, int userId = 5)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "inv.usp_PartnersAdd";
                    cmd.Parameters.Add("@p_FirstName", SqlDbType.NVarChar, 128).Value = partner.FirstName == null ? (object)DBNull.Value : partner.FirstName;
                    cmd.Parameters.Add("@p_LastName", SqlDbType.NVarChar, 128).Value = partner.LastName == null ? (object)DBNull.Value : partner.LastName;
                    cmd.Parameters.Add("@p_CompanyName", SqlDbType.NVarChar, 256).Value = partner.CompanyName == null ? (object)DBNull.Value : partner.CompanyName;
                    cmd.Parameters.Add(Utils.GetDecimalParam("@p_Vatin", 24, 0, partner.Vatin));
                    cmd.Parameters.Add("@p_Address", SqlDbType.NVarChar, 2048).Value = partner.Address;
                    cmd.Parameters.Add("@p_User", SqlDbType.Int).Value = userId;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public ResultSet<Partner> PartnerList(int pageNumber = 1, int rowsPerPage = Int32.MaxValue-1)
        {
            ResultSet<Partner> allPartners = new ResultSet<Partner>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "inv.usp_PartnersList";
                    cmd.Parameters.Add("@p_pageNumber", SqlDbType.Int).Value = pageNumber;
                    cmd.Parameters.Add("@p_rowsPerPage", SqlDbType.Int).Value = rowsPerPage+1;

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string rFirstName = String.Empty;
                            string rLastName = String.Empty;
                            string rCompanyName = String.Empty;

                            int rId = reader.GetInt32(reader.GetOrdinal("Part_Id"));
                            if (reader[reader.GetOrdinal("Part_FirstName")] != DBNull.Value)
                            {
                                rFirstName = reader.GetString(reader.GetOrdinal("Part_FirstName"));
                            }
                            if (reader[reader.GetOrdinal("Part_LastName")] != DBNull.Value)
                            {
                                rLastName = reader.GetString(reader.GetOrdinal("Part_LastName"));
                            }
                            if (reader[reader.GetOrdinal("Part_CompanyName")] != DBNull.Value)
                            {
                                rCompanyName = reader.GetString(reader.GetOrdinal("Part_CompanyName"));
                            }
                            long rVatin = (long)reader.GetDecimal(reader.GetOrdinal("Part_Vatin"));
                            string rAddress = reader.GetString(reader.GetOrdinal("Part_Address"));

                            allPartners.Add(new Partner(rFirstName, rLastName, rCompanyName, rVatin, rAddress));
                        }
                    }
                    connection.Close();
                }
            }

            if (allPartners.Count == (rowsPerPage + 1))
            {
                allPartners.IsLastPage = true;
                allPartners.RemoveAt(allPartners.Count - 1);
            }
            return allPartners;
        }

        public ResultSet<Partner> PartnerSearch
        (
            string firstName = null,
            string lastName = null,
            string companyName = null,
            long vatin = -1,
            int pageNumber = 1,
            int rowsPerPage = (Int32.MaxValue-1)
        )
        {
            ResultSet<Partner> partnersFound = new ResultSet<Partner>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "inv.usp_PartnerSearch";
                    cmd.Parameters.Add("@p_FirstName", SqlDbType.NVarChar, 128).Value = firstName ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@p_LastName", SqlDbType.NVarChar, 128).Value = lastName ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@p_CompanyName", SqlDbType.NVarChar, 256).Value = companyName ?? (object)DBNull.Value;
                    cmd.Parameters.Add(Utils.GetDecimalParam("@p_Vatin", 24, 0, vatin < 0 ? (object)DBNull.Value : vatin));
                    cmd.Parameters.Add("@p_pageNumber", SqlDbType.Int).Value = pageNumber;
                    cmd.Parameters.Add("@p_rowsPerPage", SqlDbType.Int).Value = rowsPerPage+1;

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string rFirstName = String.Empty;
                            string rLastName = String.Empty;
                            string rCompanyName = String.Empty;

                            int rId = reader.GetInt32(reader.GetOrdinal("Part_Id"));
                            if (reader[reader.GetOrdinal("Part_FirstName")] != DBNull.Value)
                            {
                                rFirstName = reader.GetString(reader.GetOrdinal("Part_FirstName"));
                            }
                            if (reader[reader.GetOrdinal("Part_LastName")] != DBNull.Value)
                            {
                                rLastName = reader.GetString(reader.GetOrdinal("Part_LastName"));
                            }
                            if (reader[reader.GetOrdinal("Part_CompanyName")] != DBNull.Value)
                            {
                                rCompanyName = reader.GetString(reader.GetOrdinal("Part_CompanyName"));
                            }
                            long rVatin = (long)reader.GetDecimal(reader.GetOrdinal("Part_Vatin"));
                            string rAddress = reader.GetString(reader.GetOrdinal("Part_Address"));

                            partnersFound.Add(new Partner(rFirstName, rLastName, rCompanyName, rVatin, rAddress));
                        }
                    }
                    connection.Close();
                }
            }

            if (partnersFound.Count != (rowsPerPage + 1))
            {
                partnersFound.IsLastPage = true;
            }
            else
            {
                partnersFound.RemoveAt(partnersFound.Count - 1);
            }
            return partnersFound;
        }

        public Partner PartnerGetById(int partnerId)
        {
            Partner partner = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "inv.usp_PartnerGetById";
                    cmd.Parameters.Add("@p_PartnerId", SqlDbType.Int).Value = partnerId;

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string rFirstName = String.Empty;
                            string rLastName = String.Empty;
                            string rCompanyName = String.Empty;

                            int rId = reader.GetInt32(reader.GetOrdinal("Part_Id"));
                            if (reader[reader.GetOrdinal("Part_FirstName")] != DBNull.Value)
                            {
                                rFirstName = reader.GetString(reader.GetOrdinal("Part_FirstName"));
                            }
                            if (reader[reader.GetOrdinal("Part_LastName")] != DBNull.Value)
                            {
                                rLastName = reader.GetString(reader.GetOrdinal("Part_LastName"));
                            }
                            if (reader[reader.GetOrdinal("Part_CompanyName")] != DBNull.Value)
                            {
                                rCompanyName = reader.GetString(reader.GetOrdinal("Part_CompanyName"));
                            }
                            long rVatin = (long)reader.GetDecimal(reader.GetOrdinal("Part_Vatin"));
                            string rAddress = reader.GetString(reader.GetOrdinal("Part_Address"));

                            partner = new Partner(rFirstName, rLastName, rCompanyName, rVatin, rAddress);
                        }
                    }
                    connection.Close();
                }
            }

            return partner;
        }
    }
}