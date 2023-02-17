using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;
        private readonly ICompensationRepository _compensationRepository;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository, ICompensationRepository compensationRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _compensationRepository = compensationRepository;
        }

        public Compensation CreateCompensationForEmployee(string employeeId, Compensation compensation)
        {
            var employee = GetById(employeeId);

            // controller should throw not found if we return nothing as it should have already handled validation, but the service should be verifying entity existence
            if (employee == null)
            {
                return null;
            }

            compensation.Employee = employee;
            _compensationRepository.Add(compensation);
            _compensationRepository.SaveAsync().Wait();

            return compensation;
        }

        public Employee Create(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }

            return employee;
        }

        public Employee GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetById(id);
            }

            return null;
        }

        public List<Compensation> GetCompensationsForEmployee(string employeeId)
        {
            if(!String.IsNullOrEmpty(employeeId))
            {
                // verify the employee exists first, if it doesn't make sure we "not found" the consumer
                var employee = GetById(employeeId);
                if(employee == null)
                {
                    return null;
                }

                return _compensationRepository.GetByEmployeeId(employeeId);
            }

            return null;
        }

        public ReportingStructure GetReportingStructure(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                return _employeeRepository.GetReportingStructure(id);
            }

            return null;
        }

        public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if(originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }
    }
}
