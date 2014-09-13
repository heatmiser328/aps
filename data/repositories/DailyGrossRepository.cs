using System;
using System.Data;
using System.Collections.Generic;

using ica.aps.container;
using ica.aps.data.helpers;
using ica.aps.data.interfaces;
using ica.aps.data.models;

namespace ica.aps.data.repositories
{						  
    public class DailyGrossRepository : IDailyGrossRepository
    {
        public DailyGrossRepository(IIocContainer container, IDbConnection conn = null)
        {
			_container = container;
			_conn = conn;
        }
	
        public IList<IDailyGross> GetDailyGrosses(IEmployee employee, DateTime start, DateTime end)
        {
            IList<IDailyGross> grosses = new List<IDailyGross>();
			/*using (*/IDbConnection conn = GetConnection();//)
			{
	            using (IDbCommand cmd = conn.CreateCommand())
	            {
					IRent r = employee.EffectiveRent(start);
					
	                cmd.CommandText = cSelectDailyGrossesForEmployee_SQL;
	                DBHelper.AddCommandParameter(cmd, "@RentID", DbType.Guid, ParameterDirection.Input, r.ID);
	                DBHelper.AddCommandParameter(cmd, "@StartTDS", DbType.DateTime, ParameterDirection.Input, start);
	                DBHelper.AddCommandParameter(cmd, "@EndTDS", DbType.DateTime, ParameterDirection.Input, end);
						
	                using (IDataReader dr = cmd.ExecuteReader())
	                {
	                    while (dr.Read())
	                    {
	                        IDailyGross dg = CreateDailyGross(dr);
	                        grosses.Add(dg);
	                    }
	                }
	            }
			}

            return grosses;
        }

        public void InsertDailyGross(IEmployee employee, IDailyGross dg)
        {
			/*using (*/IDbConnection conn = GetConnection();//)
			{
	            using (IDbCommand cmd = conn.CreateCommand())
	            {
					dg.ID = Guid.NewGuid();
					IRent r = employee.EffectiveRent(dg.GrossDate);
					
	                cmd.CommandText = cInsertDailyGrossForEmployee_SQL;
	                DBHelper.AddCommandParameter(cmd, "@DailyGrossID", DbType.Guid, ParameterDirection.Input, dg.ID);
	                DBHelper.AddCommandParameter(cmd, "@RentID", DbType.Guid, ParameterDirection.Input, r.ID);
	                DBHelper.AddCommandParameter(cmd, "@Gross", DbType.Decimal, ParameterDirection.Input, dg.GrossPay);
	                DBHelper.AddCommandParameter(cmd, "@GrossTDS", DbType.DateTime, ParameterDirection.Input, dg.GrossDate);
	                DBHelper.AddCommandParameter(cmd, "@ModifiedBy", DbType.String, ParameterDirection.Input, dg.ModifiedBy);
	                DBHelper.AddCommandParameter(cmd, "@ModifiedTDS", DbType.DateTime, ParameterDirection.Input, dg.Modified);
						
					cmd.ExecuteNonQuery();
	            }
			}
        }
	
        public void UpdateDailyGross(IEmployee employee, IDailyGross dg)
        {
			/*using (*/IDbConnection conn = GetConnection();//)
			{
	            using (IDbCommand cmd = conn.CreateCommand())
	            {
					IRent r = employee.EffectiveRent(dg.GrossDate);
					
	                cmd.CommandText = cUpdateDailyGrossForEmployee_SQL;
	                DBHelper.AddCommandParameter(cmd, "@DailyGrossID", DbType.Guid, ParameterDirection.Input, dg.ID);
	                DBHelper.AddCommandParameter(cmd, "@RentID", DbType.Guid, ParameterDirection.Input, r.ID);
	                DBHelper.AddCommandParameter(cmd, "@Gross", DbType.Decimal, ParameterDirection.Input, dg.GrossPay);
	                DBHelper.AddCommandParameter(cmd, "@GrossTDS", DbType.DateTime, ParameterDirection.Input, dg.GrossDate);
	                DBHelper.AddCommandParameter(cmd, "@ModifiedBy", DbType.String, ParameterDirection.Input, dg.ModifiedBy);
	                DBHelper.AddCommandParameter(cmd, "@ModifiedTDS", DbType.DateTime, ParameterDirection.Input, dg.Modified);
						
					cmd.ExecuteNonQuery();
	            }
			}
        }
		
        #region Implementation
        private static IDailyGross CreateDailyGross(IDataReader dr)
        {
            IDailyGross dg = new DailyGross();
			
			dg.ID = DBHelper.RetrieveGuid(dr, "DailyGrossID");
			dg.GrossPay = DBHelper.RetrieveDecimal(dr, "Gross");
			dg.GrossDate = DBHelper.RetrieveDateTime(dr, "GrossTDS");
			dg.ModifiedBy = DBHelper.RetrieveString(dr, "ModifiedBy");
			dg.Modified = DBHelper.RetrieveDateTime(dr, "ModifiedTDS");
            dg.Dirty = false;
			
            return dg;
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

        private const string cSelectDailyGrossesForEmployee_SQL =
@"SELECT *
FROM DailyGross
WHERE RentID = @RentID AND GrossTDS between @StartTDS and @EndTDS
ORDER BY GrossTDS";

        private const string cInsertDailyGrossForEmployee_SQL = 
@"INSERT INTO [DailyGross] 
	([DailyGrossID], [RentID], [Gross], [GrossTDS], [ModifiedBy], [ModifiedTDS])
VALUES
	(@DailyGrossID, @RentID, @Gross, @GrossTDS, @ModifiedBy, @ModifiedTDS)";

        private const string cUpdateDailyGrossForEmployee_SQL = 
@"UPDATE [DailyGross] SET 
	[RentID] = @RentID, 
	[Gross] = @Gross, 
	[GrossTDS] = @GrossTDS, 
	[ModifiedBy] = @ModifiedBy, 
	[ModifiedTDS] = @ModifiedTDS
WHERE
	[DailyGrossID] = @DailyGrossID";
	
        #endregion
		
        #region Private
        private IIocContainer _container;
		private IDbConnection _conn;
        #endregion
    }
}
