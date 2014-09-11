using System;
using System.Collections.Generic;
using System.Linq;

using ica.aps.data.interfaces;

namespace ica.aps.data.models
{
    public class Employee : IEmployee
    {
        public Guid? ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public int Sequence { get; set; }
        public IList<IRent> Rents { get; set; }
        public bool Enabled { get; set; }
        public DateTime Modified { get; set; }
        public string ModifiedBy { get; set; }

        public string FullName
        {
            get 
            {
                return this.FirstName + " " + this.LastName;
            }
        }

        public IRent EffectiveRent(DateTime? dt = null)
        {
            if (this.Rents == null || this.Rents.Count < 1) 
                return null;

            if (dt == null || !dt.HasValue)            
                dt = DateTime.Now;
            
            var rent =
                from r in this.Rents
                where r.EffectiveDate <= dt
                orderby r.EffectiveDate descending
                select r;

            if (rent != null) 
                return rent.First();
            return this.Rents[0];
        }
    }
}
