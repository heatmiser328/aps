using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ica.aps.data.interfaces;

namespace ica.aps.data.models
{
    public class Payroll : IPayroll
    {
        public DateTime StartTDS {get;set;}
        public DateTime EndTDS {get;set;}
        public IList<IEmployeePayroll> Employees {get;set;}

        public decimal Gross
        {
            get 
            {
                return this.Employees.Sum(e => e.Gross);
            }
        }

        public decimal Net
        {
            get
            {
                return this.Employees.Sum(e => e.Net);
            }
        }

        public decimal Rent
        {
            get
            {
                return this.Employees.Sum(e => e.Rent);
            }
        }
    }
}
