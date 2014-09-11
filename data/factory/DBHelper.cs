using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace ica.aps.data.helpers
{
	internal static class DBHelper
	{
        /// <summary>
        /// Add a Parameter to a Command
        /// </summary>
        /// <param name="cmd">SQL command </param>
        /// <param name="name">Name of the @parameter in the SQL command</param>
        /// <param name="type">Data type of the parameter</param>
        /// <param name="direction">Parameter direction</param>
        /// <param name="value">Value to assign to the parameter</param>
        /// <returns>Returns the parameter object</returns>
        public static IDbDataParameter AddCommandParameter(IDbCommand command, string name, DbType dataType, ParameterDirection direction, Object value)
        {
            IDbDataParameter param = command.CreateParameter();
            param.DbType = dataType;
            param.ParameterName = name;
            param.Value = value;
            param.Direction = direction;
            command.Parameters.Add(param);

            return param;
        }

		public static Guid RetrieveGuid(IDataReader dr, string colname)
		{
			if (dr[colname] != System.DBNull.Value)
				return (Guid) dr[colname];
			return Guid.Empty;
		}
		
		public static string RetrieveString(IDataReader dr, string colname)
		{
			if (dr[colname] != System.DBNull.Value)
				return (string) dr[colname];			
			return null;
		}
		public static bool RetrieveBool(IDataReader dr, string colname)
		{
			if (dr[colname] != System.DBNull.Value)
				return (bool) dr[colname];
			return false;
		}
		public static int RetrieveInt(IDataReader dr, string colname)
		{
			if (dr[colname] != System.DBNull.Value)
				return (int) dr[colname];
			return 0;
		}		
		public static short RetrieveShort(IDataReader dr, string colname)
		{
			if (dr[colname] != System.DBNull.Value)
				return Convert.ToInt16(dr[colname]);
			return 0;
		}
		public static decimal RetrieveDecimal(IDataReader dr, string colname)
		{
			if (dr[colname] != System.DBNull.Value)
				return (decimal) dr[colname];
			return 0.0M;
		}
		public static double RetrieveDouble(IDataReader dr, string colname)
		{			
			if (dr[colname] != System.DBNull.Value)
				return (double) dr[colname];
			return 0.0;
		}
		public static DateTime RetrieveDateTime(IDataReader dr, string colname)
		{
			if (dr[colname] != System.DBNull.Value)
				return (DateTime) dr[colname];
			return DateTime.MinValue;
		}		
		public static float RetrieveFloat(IDataReader dr, string colname)
		{
			if (dr[colname] != System.DBNull.Value)
				return (float) dr[colname];
			return 0.0F;
		}
		public static long RetrieveLong(IDataReader dr, string colname)
		{
			if (dr[colname] != System.DBNull.Value)
				return (long) dr[colname];
			return 0L;
		}
	}
}

