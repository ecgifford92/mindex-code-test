using CodeChallenge.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public interface ICompensationRepository
    {
        public List<Compensation> GetByEmployeeId(string id);

        public Compensation Add(Compensation compensation);

        public Task SaveAsync();

        public Compensation GetByEmployeeIdAndEffectiveDate(string id, DateTime effectiveDate);
    }
}
