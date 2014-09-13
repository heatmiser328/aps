using System;
using System.Data;
using System.Collections.Generic;

using ica.aps.container;
using ica.aps.data.helpers;
using ica.aps.data.interfaces;
using ica.aps.data.models;

namespace ica.aps.data.repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public EmployeeRepository(IIocContainer container, IDbConnection conn = null)
        {
			_container = container;
			_conn = conn;
        }
	
        public IList<IEmployee> GetEmployees()
        {
            IList<IEmployee> employees = new List<IEmployee>();
			/*using (*/IDbConnection conn = GetConnection();//)
			{
	            using (IDbCommand cmd = conn.CreateCommand())
	            {
	                cmd.CommandText = cSelectEmployees_SQL;
	                using (IDataReader dr = cmd.ExecuteReader())
	                {
	                    while (dr.Read())
	                    {
	                        IEmployee emp = CreateEmployee(dr);
	                        employees.Add(emp);
	                    }
	                }
	            }

				IRentRepository rrepo = _container.Create<IRentRepository>(conn);
				foreach (IEmployee emp in employees)
				{
                    emp.Rents = rrepo.GetRents(emp);
				}
			}

            return employees;
        }


        #region Implementation
        private IEmployee CreateEmployee(IDataReader dr)
        {
            IEmployee emp = new Employee();
			
			emp.ID = DBHelper.RetrieveGuid(dr, "EmployeeID");
			emp.FirstName = DBHelper.RetrieveString(dr, "FirstName");
            emp.LastName = DBHelper.RetrieveString(dr, "LastName");
            emp.Title = DBHelper.RetrieveString(dr, "Title");
			emp.Sequence = DBHelper.RetrieveShort(dr, "Sequence");
			emp.Enabled = DBHelper.RetrieveBool(dr, "Enabled");
			emp.ModifiedBy = DBHelper.RetrieveString(dr, "ModifiedBy");
			emp.Modified = DBHelper.RetrieveDateTime(dr, "ModifiedTDS");
			
            return emp;
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

        private const string cSelectEmployees_SQL =
@"SELECT *
FROM Employee
ORDER BY Sequence";
        #endregion
		
        #region Private
        private IIocContainer _container;
		private IDbConnection _conn;
        #endregion
    }
}
