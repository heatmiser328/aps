using System;
using System.Collections.Generic;

namespace ica.aps.data.interfaces
{
    public interface IPayrollRepository
    {
        IList<IEmployee> GetEmployees();
        IPayroll GetPayroll(DateTime dt);
        void SavePayroll(IPayroll payroll);
    }
}
