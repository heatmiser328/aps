using System;
using System.Data;
using System.Collections.Generic;

using ica.Payroll.models.interfaces;
using ica.Payroll.models;
using ica.aps.data.repositories;

namespace ica.aps.data.repositories
{
    public static class EmployeeRepository
    {
        public static IList<IEmployee> GetEmployees(IDbConnection conn)
        {
            IList<IEmployee> employees = new List<IEmployee>();
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

			foreach (IEmployee emp in employees)
			{
				emp.Rents = RentRepository.GetRents(conn, emp);
			}

            return employees;
        }


        #region Implementation
        private static IEmployee CreateEmployee(IDataReader dr)
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

        #endregion

        #region SQL

        private const string cSelectEmployees_SQL =
@"SELECT *
FROM Employee
ORDER BY Sequence";

        #endregion
    }
}
