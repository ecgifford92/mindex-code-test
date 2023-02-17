namespace CodeChallenge.Models
{
    public class ReportingStructure
    {
        public ReportingStructure(Employee employee, int numberOfReports)
        {
            Employee = employee;
            NumberOfReports = numberOfReports;
        }

        /// <summary>
        /// The Employee referenced in this structure
        /// </summary>
        public Employee Employee { get; }


        /// <summary>
        /// The total number of reports in the tree
        /// </summary>
        public int NumberOfReports { get; }
    }
}
