using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace DataAccessLayer
{
    public static class Utils
    {
        public static SqlParameter GetDecimalParam(string name, byte precision, byte scale, object value)
        {
            SqlParameter decimalParam = new SqlParameter();

            decimalParam.DbType = DbType.Decimal;
            decimalParam.ParameterName = name;
            decimalParam.Precision = precision;
            decimalParam.Scale = scale;
            decimalParam.Value = value;

            return decimalParam;
        }

        public static string GetStringSha256Hash(string password)
        {
            if (String.IsNullOrEmpty(password))
            {
                return String.Empty;
            }

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] passBytes = System.Text.Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha.ComputeHash(passBytes);
                return BitConverter.ToString(hashBytes).Replace("-", String.Empty);
            }
        }
    }
}
