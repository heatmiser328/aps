using System;
using System.Data;
using System.Collections.Generic;

using ica.Payroll.models.interfaces;
using ica.Payroll.models;
using ica.aps.data.repositories;

namespace ica.aps.data.repositories
{						  
    public static class RentRepository
    {
        public static IList<IRent> GetRents(IDbConnection conn, IEmployee employee)
        {
            IList<IRent> rents = new List<IRent>();
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

            return rents;
        }

        #region Implementation
        private static IRent CreateRent(IDataReader dr)
        {
            IRent r = new Rent();
			
			r.ID = DBHelper.RetrieveGuid(dr, "RentID");
			r.RentPct = DBHelper.RetrieveDecimal(dr, "RentPct");
			r.EffectiveDate = DBHelper.RetrieveDateTime(dr, "EffectiveTDS");
			r.ModifiedBy = DBHelper.RetrieveString(dr, "ModifiedBy");
			r.Modified = DBHelper.RetrieveDateTime(dr, "ModifiedTDS");
			
            return r;
        }

        #endregion

        #region SQL

        private const string cSelectRentsForEmployee_SQL =
@"SELECT *
FROM Rent
WHERE EmployeeID = @EmployeeID
ORDER BY EffectiveTDS DESC";

        #endregion
    }
}
