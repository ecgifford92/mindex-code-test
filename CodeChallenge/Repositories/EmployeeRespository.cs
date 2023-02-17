using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public class EmployeeRespository : IEmployeeRepository
    {
        private readonly EmployeeContext _employeeContext;
        private readonly ILogger<IEmployeeRepository> _logger;

        public EmployeeRespository(ILogger<IEmployeeRepository> logger, EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
            _logger = logger;
        }

        public Employee Add(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            _employeeContext.Employees.Add(employee);
            return employee;
        }

        public Employee GetById(string id)
        {
            return _employeeContext.Employees
                .Include(d => d.DirectReports)
                .SingleOrDefault(e => e.EmployeeId == id);
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }

        /// <summary>
        /// Get the complete reporting structure for a given employee id
        /// </summary>
        /// <param name="id">The employee's ID</param>
        /// <returns>The Reporting Structure</returns>
        public ReportingStructure GetReportingStructure(string id)
        {
            var employee = GetById(id);

            var allReports = GetAllDirectReports(id);

            return new ReportingStructure(employee, allReports.Count);
        }


        /// <summary>
        /// Recursive function to get all direct reports for an employee and add them to a flattened list for n depth
        /// </summary>
        /// <param name="id">The employee ID you want to retrieve data for</param>
        /// <returns>The total flattened list of direct reports for the specified employee ID</returns>
        private List<Employee> GetAllDirectReports(string id)
        {
            var employee = GetById(id);

            var directReports = employee.DirectReports.ToList();

            var allReports = directReports;

            // cache the list of reports to add because modifying lists inside a loop isn't allowed
            List<Employee> reportsToAdd = null;

            foreach(var e in directReports)
            {
                // recursively get each subordinate and their employees
                reportsToAdd = GetAllDirectReports(e.EmployeeId);
            }

            // only add if we have anything to add
            if(reportsToAdd != null)
            {
                allReports.AddRange(reportsToAdd);
            }

            return allReports;
        }
    }
}
