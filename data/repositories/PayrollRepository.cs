using System;
using System.Data;
using System.Collections.Generic;

using ica.aps.container;
using ica.aps.data.helpers;
using ica.aps.data.interfaces;
using ica.aps.data.models;

namespace ica.aps.data.repositories
{
    public class PayrollRepository : IPayrollRepository
    {
        public PayrollRepository(IIocContainer container)
        {
			_container = container;
        }

        public IList<IEmployee> GetEmployees()
        {
			IDBFactory factory = _container.Create<IDBFactory>();
            using (IDbConnection conn = factory.Create())
            {
                conn.Open();
				
				IEmployeeRepository emprepos = _container.Create<IEmployeeRepository>(conn);
				IList<IEmployee> employees = emprepos.GetEmployees();
								
				return employees;
            }
        }

        public IPayroll GetPayroll(DateTime dt)
        {
			IDBFactory factory = _container.Create<IDBFactory>();
            using (IDbConnection conn = factory.Create())
            {
                conn.Open();

                int offset = (int)dt.DayOfWeek - (int)System.DayOfWeek.Monday;
                int diff = (int)System.DayOfWeek.Saturday - (int)System.DayOfWeek.Monday;

				DateTime start = dt - new TimeSpan(offset, 0, 0, 0, 0);
                DateTime end = start.AddDays(diff);

				IPayroll payroll = new Payroll {
					StartTDS = start,
					EndTDS = end,
					Employees = new List<IEmployeePayroll>()
				};
				
				IEmployeeRepository emprepos = _container.Create<IEmployeeRepository>(conn);
                IDailyGrossRepository dgr = _container.Create<IDailyGrossRepository>(conn);
				
                IList<IEmployee> employees = emprepos.GetEmployees();
				foreach (IEmployee emp in employees)
				{
                    IEmployeePayroll pr = new EmployeePayroll(emp, start);
                    //pr.Rent = emp.EffectiveRent(start);
                    pr.Grosses = dgr.GetDailyGrosses(emp, start, end);
					
					payroll.Employees.Add(pr);
				}

				return payroll;
            }
        }
		
        public void SavePayroll(IPayroll payroll)
        {
			IDBFactory factory = _container.Create<IDBFactory>();
            using (IDbConnection conn = factory.Create())
            {
                conn.Open();
				
                IDailyGrossRepository dgr = _container.Create<IDailyGrossRepository>(conn);
				foreach (IEmployeePayroll pr in payroll.Employees)
				{
                    if (pr.Dirty)
                    {
                        foreach (IDailyGross dg in pr.Grosses)
                        {
                            if (dg.Dirty)
                            {
                                if (dg.ID == null || dg.ID.Equals(Guid.Empty))
                                    dgr.InsertDailyGross(pr.Employee, dg);
                                else
                                    dgr.UpdateDailyGross(pr.Employee, dg);
                                dg.Dirty = false;
                            }
                        }
                    }
				}
            }
        }
		
        #region Implementation
        #endregion

        #region Private
        private IIocContainer _container;
        #endregion
    }
}
