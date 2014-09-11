using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ica.aps.data.interfaces;

namespace ica.aps.data.models
{
    public class EmployeePayroll : IEmployeePayroll
    {
        public EmployeePayroll(IEmployee emp, DateTime start) 
        {
            this.Employee = emp;
            _start = start;
        }

        public IEmployee Employee { get; set; }
        public IList<IDailyGross> Grosses { get; set; }

        public decimal Gross
        {
            get 
            {
                return this.Grosses.Sum(g => g.GrossPay);                
            }
        }

        public decimal Net
        {
            get
            {
                return this.Gross * (1.0M - this.Employee.EffectiveRent(_start).RentPct);
            }
        }

        public decimal Rent
        {
            get
            {
                return this.Gross * this.Employee.EffectiveRent(_start).RentPct;
            }
        }

        public bool Dirty
        {
            get
            {
                return this.Grosses.Any(g => g.Dirty);
            }
        }

        private DateTime _start;
    }
}
