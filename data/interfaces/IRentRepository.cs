using System;
using System.Collections.Generic;

namespace ica.aps.data.interfaces
{						  
    public interface IRentRepository
    {
        IList<IRent> GetRents(IEmployee employee);
    }
}
