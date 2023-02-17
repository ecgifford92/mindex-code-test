using System;

namespace CodeChallenge.Models
{
    public class Compensation
    { 
        public String Id { get; set; }

        /// <summary>
        /// The employee referenced for compensation
        /// </summary>
        public Employee Employee { get; set; }

        /// <summary>
        ///  The salary for referenced employee
        /// </summary>
        public decimal Salary { get; set; }

        /// <summary>
        /// The effective data for the salary for the referenced employee
        /// </summary>
        public DateTime EffectiveDate { get; set; }
    }
}
