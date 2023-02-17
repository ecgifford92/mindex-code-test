using CodeChallenge.Data;
using CodeChallenge.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public class CompensationRepository : ICompensationRepository

    {
        private readonly CompensationContext _compensationContext;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(CompensationContext compensationContext, ILogger<ICompensationRepository> logger)
        {
            _compensationContext = compensationContext;
            _logger = logger;
        }

        public Compensation Add(Compensation compensation)
        {
            compensation.Id = Guid.NewGuid().ToString();

            _compensationContext.Compensations.Add(compensation);
            return compensation;
        }
        public Task SaveAsync()
        {
            return _compensationContext.SaveChangesAsync();
        }
        /// <summary>
        /// Retrieve a list of compensations
        /// 
        /// Regarding the assignment, the introduction of an "effective date" implies the one-to-many relationship
        /// since salary fortunately increases exist
        /// </summary>
        /// <param name="id">The id of the employee to get the compensations for</param>
        /// <returns></returns>
        public List<Compensation> GetByEmployeeId(string id)
        {
            return _compensationContext.Compensations
                .Include(b => b.Employee)
                .Where(e => e.Employee.EmployeeId == id)
                .ToList();
        }
    }
}
