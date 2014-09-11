using System;
using System.Collections.Generic;

namespace ica.aps.data.interfaces
{
    public interface IEmployeeRepository
    {
        IList<IEmployee> GetEmployees();
    }
}
