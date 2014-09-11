using System;

using ica.aps.data.interfaces;

namespace ica.aps.data.models
{
    public class Rent : IRent
    {
        public Guid? ID { get; set; }
        public decimal RentPct { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime Modified { get; set; }
        public string ModifiedBy { get; set; }
    }
}
