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

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public async Task<Employee> CreateAsync(Employee employee)
        {
            if (employee != null)
            {
                await _employeeRepository.AddAsync(employee);
                await _employeeRepository.SaveAsync();
            }

            return employee;
        }

        public async Task<Employee> GetByIdAsync(string id, bool useRecursiveDirectReports)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (useRecursiveDirectReports)
                {
                    return await _employeeRepository.GetByIdRecursiveDirectReportsAsync(id);
                }
                return await _employeeRepository.GetByIdAsync(id);
            }

            return null;
        }

        public async Task<Employee> ReplaceAsync(Employee originalEmployee, Employee newEmployee)
        {
            if (originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    await _employeeRepository.SaveAsync();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                }
                await _employeeRepository.SaveAsync();
            }

            return newEmployee;
        }

        public async Task<int> GetNumberOfDirectReportsAsync(Employee employee)
        {
            if (employee.DirectReports == null ||
                !employee.DirectReports.Any())
            {
                return 0;
            }

            int count = employee.DirectReports.Count;
            foreach (var directReport in employee.DirectReports)
            {
                count += await GetNumberOfDirectReportsAsync(directReport);
            }

            return count;
        }

        public Employee GetById(string id)
        {
            return GetByIdAsync(id, false).GetAwaiter().GetResult();
        }

        public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            return ReplaceAsync(originalEmployee, newEmployee).GetAwaiter().GetResult();
        }

        public Employee Create(Employee employee)
        {
            return CreateAsync(employee).GetAwaiter().GetResult();
        }
    }
}
