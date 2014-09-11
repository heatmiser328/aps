using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ica.aps.data.interfaces
{
    public interface IEmployee
    {
        Guid? ID { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Title { get; set; }        
        string FullName { get; }
        int Sequence { get; set; }
        IList<IRent> Rents { get; set; }        
        bool Enabled { get; set; }
        DateTime Modified { get; set; }
        string ModifiedBy { get; set; }

        IRent EffectiveRent(DateTime? dt = null);
    }
}
