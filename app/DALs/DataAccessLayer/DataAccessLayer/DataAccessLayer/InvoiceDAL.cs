using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace DataAccessLayer
{
    static class InvoiceDAL
    {
        private static string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["LocalDb"].ConnectionString;

        public static string GetInvoiceNumber()
        {
            string nextInvoiceNumber = String.Empty;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "inv.usp_InvoiceNumberNext";
                    cmd.Parameters.Add("@p_InvoiceNumber", SqlDbType.NVarChar, 16).Direction = ParameterDirection.Output;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();

                    nextInvoiceNumber = cmd.Parameters["@p_InvoiceNumber"].Value.ToString();
                }
            }

            return nextInvoiceNumber;
        }


        public static void InvoiceAdd(Invoice invoice, long vendorVatin, long buyerVatin)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "inv.usp_InvoiceAdd";
                    cmd.Parameters.Add("@p_InvoiceNumber", SqlDbType.NVarChar, 16).Value = invoice.Number;
                    cmd.Parameters.Add(Utils.GetDecimalParam("@p_VendorVatin", 24, 0, vendorVatin));
                    cmd.Parameters.Add(Utils.GetDecimalParam("@p_BuyerVatin", 24, 0, buyerVatin));
                    cmd.Parameters.Add("@p_Title", SqlDbType.NVarChar, 2048).Value = invoice.Title;
                    cmd.Parameters.Add("@p_Goods", SqlDbType.NVarChar, -1).Value = Goods.Serialize(invoice.GoodsList);
                    cmd.Parameters.Add(Utils.GetDecimalParam("@p_OverallNetValue", 9, 2, invoice.OverallNet));
                    cmd.Parameters.Add(Utils.GetDecimalParam("@p_OverallGrossValue", 9, 2, invoice.OverallGross));
                    cmd.Parameters.Add(Utils.GetDecimalParam("@p_Discount", 3, 2, invoice.Discount));
                    cmd.Parameters.Add(Utils.GetDecimalParam("@p_OverallCost", 9, 2, invoice.OverallGross));

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public static List<InvoiceGenerals> InvoiceSearch
        (
            string number = null,
            DateTime? dateStart = null,
            DateTime? dateEnd = null,
            string title = null,
            float costMin = -1,
            float costMax = -1,
            Partner vendor = null, 
            Partner buyer = null,
            int pageNumber = 1,
            int rowsPerPage = Int32.MaxValue
        )
        {
            List<InvoiceGenerals> invoicesFound = new List<InvoiceGenerals>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "inv.usp_InvoicesSearch";
                    cmd.Parameters.Add("@p_Number", SqlDbType.NVarChar,16).Value = number ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@p_DateStart", SqlDbType.NVarChar,16).Value = dateStart.HasValue ? dateStart.Value.ToShortDateString() : (object)DBNull.Value;
                    cmd.Parameters.Add("@p_DateEnd", SqlDbType.NVarChar,16).Value = dateEnd.HasValue ? dateEnd.Value.ToShortDateString() : (object)DBNull.Value;
                    cmd.Parameters.Add("@p_VendorFirstName", SqlDbType.NVarChar,128).Value = vendor.FirstName ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@p_VendorLastName", SqlDbType.NVarChar,128).Value = vendor.LastName ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@p_VendorCompany", SqlDbType.NVarChar,256).Value = vendor.CompanyName ?? (object)DBNull.Value;
                    cmd.Parameters.Add(Utils.GetDecimalParam("@p_VendorVatin",24,0,vendor.Vatin != -1 ? vendor.Vatin : (object)DBNull.Value));
                    cmd.Parameters.Add("@p_BuyerFirstName", SqlDbType.NVarChar,128).Value = buyer.FirstName ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@p_BuyerLastName", SqlDbType.NVarChar,128).Value = buyer.LastName ?? (object)DBNull.Value;
                    cmd.Parameters.Add("@p_BuyerCompany", SqlDbType.NVarChar,256).Value = buyer.CompanyName ?? (object)DBNull.Value;
                    cmd.Parameters.Add(Utils.GetDecimalParam("@p_BuyerVatin",24,0,buyer.Vatin != -1 ? buyer.Vatin : (object)DBNull.Value));
                    cmd.Parameters.Add("@p_Title", SqlDbType.NVarChar,2048).Value = title ?? (object)DBNull.Value;
                    cmd.Parameters.Add(Utils.GetDecimalParam("@p_CostMin",9,2,costMin > 0 ? costMin : (object)DBNull.Value));
                    cmd.Parameters.Add(Utils.GetDecimalParam("@p_CostMax",9,2,costMax > 0 ? costMax : (object)DBNull.Value));
                    cmd.Parameters.Add("@p_pageNumber", SqlDbType.Int).Value = pageNumber;
                    cmd.Parameters.Add("@p_rowsPerPage", SqlDbType.Int).Value = rowsPerPage;

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            int iId = reader.GetInt32(reader.GetOrdinal("v_Inv_Id"));
                            string iNumber = reader.GetString(reader.GetOrdinal("v_Inv_DateOfIssue"));
                            DateTime iDateOfIssue = reader.GetDateTime(reader.GetOrdinal("v_Inv_DateOfIssue"));       
                            string iVendor = reader.GetString(reader.GetOrdinal("v_Inv_Vendor"));
                            string iBuyer = reader.GetString(reader.GetOrdinal("v_Inv_Buyer"));
                            string iTitle = reader.GetString(reader.GetOrdinal("v_Inv_Title"));
                            float iOverallCost = (float)reader.GetDecimal(reader.GetOrdinal("v_Inv_OverallCost"));
                            InvoiceState iState = (InvoiceState)reader.GetInt32(reader.GetOrdinal("v_Inv_Status"));

                            invoicesFound.Add(new InvoiceGenerals(iNumber, iDateOfIssue, iVendor, iBuyer, iTitle, iOverallCost, iState, iId));
                        }
                    }
                    connection.Close();
                }
            }

            return invoicesFound;
        }


        public static InvoiceDetails InvoiceGetDetails(int invoiceId)
        {
            InvoiceDetails detailedInvoice = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "inv.usp_InvoicesDetailsGet";
                    cmd.Parameters.Add("@p_Inv_Id", SqlDbType.Int).Value = invoiceId;

                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {        
                            int iId = reader.GetInt32(reader.GetOrdinal("Inv_Id"));
                            string iNumber = reader.GetString(reader.GetOrdinal("Inv_Number"));
                            DateTime iDateOfIssue = reader.GetDateTime(reader.GetOrdinal("Inv_DateOfIssue"));
                            int iVendorId = reader.GetInt32(reader.GetOrdinal("Inv_VendorId"));
                            int iBuyerId = reader.GetInt32(reader.GetOrdinal("Inv_BuyerId"));
                            string iTitle = reader.GetString(reader.GetOrdinal("Inv_Title"));
                            Goods iGoods = Goods.Deserialize(reader.GetString(reader.GetOrdinal("Inv_Goods")));
                            float iOverallNet = (float)reader.GetDecimal(reader.GetOrdinal("Inv_OverallNetValue"));
                            float iOverallGross = (float)reader.GetDecimal(reader.GetOrdinal("Inv_OverallGrossValue"));
                            float iDiscount = (float)reader.GetDecimal(reader.GetOrdinal("Inv_Discount"));
                            float iOverallCost = (float)reader.GetDecimal(reader.GetOrdinal("Inv_OverallCost"));
                            InvoiceState iState = (InvoiceState)reader.GetInt32(reader.GetOrdinal("Inv_Status"));

                            Invoice inv = new Invoice()
                            {
                                Id = iId,
                                Number = iNumber,
                                DateOfIssue = iDateOfIssue,
                                Title = iTitle,
                                GoodsList = iGoods,
                                OverallNet = iOverallNet,
                                OverallGross = iOverallGross,
                                Discount = iDiscount,
                                OverallCost = iOverallCost,
                                Status = iState
                            };

                            Partner vendor = PartnerDAL.PartnerGetById(iVendorId);
                            Partner buyer = PartnerDAL.PartnerGetById(iBuyerId);

                            detailedInvoice = new InvoiceDetails(inv, vendor, buyer);
                        }
                    }
                    connection.Close();
                }
            }

            return detailedInvoice;
        }


        public static void InvoiceSetPaid(int invoiceId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "inv.usp_InvoicesSetPaid";
                    cmd.Parameters.Add("@p_Inv_Id", SqlDbType.Int).Value = invoiceId;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }
    }
}
