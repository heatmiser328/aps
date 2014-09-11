using System;
using System.Data;

namespace ica.aps.data.interfaces
{
    public interface IDBFactory
    {
        bool IsSqlServerProvider {get;}
        bool IsSqlServerCeProvider {get;}
        bool IsOracleProvider { get; }

        IDbConnection Create();        
    }
}
