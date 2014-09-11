using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
//using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Data.SqlServerCe;


namespace ica.aps.data.factory
{
    /// <summary>
    /// Database Factory
    /// </summary>
    public class DBFactory : IDBFactory
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="connectionString"></param>
        public DBFactory(string provider, string connectionString)
        {
            _provider = provider;
            _connectionString = connectionString;
        }

        #region IDBFactory
        /// <summary>
        /// Is Sql Server Provider
        /// </summary>
        public bool IsSqlServerProvider
        {
            get { return IsProvider(cSQLProviderName); }
        }
        /// <summary>
        /// Is Sql Server CE Provider
        /// </summary>
        public bool IsSqlServerCeProvider
        {
            get { return IsProvider(cSQLCEProviderName); }
        }
        /// <summary>
        /// Is Oracle Provider
        /// </summary>
        public bool IsOracleProvider
        {
            get { return IsProvider(cOracleProviderName); }
        }

        /// <summary>
        /// Create a database connection
        /// </summary>
        /// <returns>IDbConnection reference to a specific provider connection object</returns>
        public IDbConnection Create()
        {            
            IDbConnection connection = null;
            switch (_provider)
            {
                case "System.Data.SqlServerCe":
                    connection = new SqlCeConnection(_connectionString);
                    break;
                case "System.Data.SqlClient":
                    connection = new SqlConnection(_connectionString);
                    break;
                //case "System.Data.OracleClient":
                //    connection = new OracleConnection(_connectionString);
                //    break;
                default:
                    throw new Exception("Unknown Database Provider");
            }

            return connection;
        }

        #endregion

        #region Implementation
        private bool IsProvider(string providerName)
        {
            return IsProvider(_provider, providerName);
        }
        private bool IsProvider(string provider, string providerName)
        {
            return (string.Compare(provider, providerName, true) == 0);
        }
        #endregion

        #region Private Properties
        private const string cSQLProviderName = "System.Data.SqlClient";
        private const string cSQLCEProviderName = "System.Data.SqlServerCe";
        private const string cOracleProviderName = "System.Data.OracleClient";        

        string _provider;
        string _connectionString;
        #endregion
    }
}
