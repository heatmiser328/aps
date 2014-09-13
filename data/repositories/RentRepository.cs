using System;
using System.Data;
using System.Collections.Generic;

using ica.aps.container;
using ica.aps.data.helpers;
using ica.aps.data.interfaces;
using ica.aps.data.models;

namespace ica.aps.data.repositories
{						  
    public class RentRepository : IRentRepository
    {
        public RentRepository(IIocContainer container, IDbConnection conn = null)
        {
			_container = container;
			_conn = conn;
        }
	
        public IList<IRent> GetRents(IEmployee employee)
        {
            IList<IRent> rents = new List<IRent>();
			/*using (*/IDbConnection conn = GetConnection();//)
			{
	            using (IDbCommand cmd = conn.CreateCommand())
	            {
	                cmd.CommandText = cSelectRentsForEmployee_SQL;
	                DBHelper.AddCommandParameter(cmd, "@EmployeeID", DbType.Guid, ParameterDirection.Input, employee.ID);
						
	                using (IDataReader dr = cmd.ExecuteReader())
	                {
	                    while (dr.Read())
	                    {
	                        IRent r = CreateRent(dr);
	                        rents.Add(r);
	                    }
	                }
	            }
			}

            return rents;
        }

        #region Implementation
        private IRent CreateRent(IDataReader dr)
        {
            IRent r = new Rent();
			
			r.ID = DBHelper.RetrieveGuid(dr, "RentID");
			r.RentPct = DBHelper.RetrieveDecimal(dr, "RentPct");
			r.EffectiveDate = DBHelper.RetrieveDateTime(dr, "EffectiveTDS");
			r.ModifiedBy = DBHelper.RetrieveString(dr, "ModifiedBy");
			r.Modified = DBHelper.RetrieveDateTime(dr, "ModifiedTDS");
			
            return r;
        }

		private IDbConnection GetConnection()
		{
			if (_conn == null)
			{
				IDBFactory factory = _container.Create<IDBFactory>();
    	        _conn = factory.Create();
				_conn.Open();
			}
			return _conn;
		}
		
        #endregion

        #region SQL

        private const string cSelectRentsForEmployee_SQL =
@"SELECT *
FROM Rent
WHERE EmployeeID = @EmployeeID
ORDER BY EffectiveTDS DESC";
        #endregion
		
        #region Private
        private IIocContainer _container;
		private IDbConnection _conn;
        #endregion
    }
}
