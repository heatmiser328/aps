using System;
using System.Collections.Generic;

using ica.aps.data.interfaces;

namespace ica.aps.data.models
{
    public class DailyGross : IDailyGross
    {
        public Guid? ID { get; set; }        
        public DateTime GrossDate { get; set; }
        public decimal GrossPay { get; set; }
        public DateTime Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool Dirty { get; set; }
    }
}
