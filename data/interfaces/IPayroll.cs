using System;
using System.Collections.Generic;

namespace ica.aps.data.interfaces
{
    public interface IPayroll
    {
        DateTime StartTDS { get; set; }
        DateTime EndTDS { get; set; }

        IList<IEmployeePayroll> Employees {get;set;}

        decimal Gross { get; }
        decimal Net { get; }
        decimal Rent { get; }
    }
}
