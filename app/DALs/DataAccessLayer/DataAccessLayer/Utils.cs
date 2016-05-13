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
    }
}
