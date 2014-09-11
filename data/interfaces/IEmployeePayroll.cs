using System;
using System.Collections.Generic;

namespace ica.aps.data.interfaces
{
    public interface IEmployeePayroll
    {
		IEmployee Employee {get;set;}        
		IList<IDailyGross> Grosses {get;set;}

        decimal Gross { get; }
        decimal Net { get; }
        decimal Rent { get; }

        bool Dirty { get; }
    }
}
