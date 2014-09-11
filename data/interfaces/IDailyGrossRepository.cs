using System;
using System.Collections.Generic;

namespace ica.aps.data.interfaces
{						  
    public interface IDailyGrossRepository
    {
        IList<IDailyGross> GetDailyGrosses(IEmployee employee, DateTime start, DateTime end);
        void InsertDailyGross(IEmployee employee, IDailyGross dg);
        void UpdateDailyGross(IEmployee employee, IDailyGross dg);
    }
}
