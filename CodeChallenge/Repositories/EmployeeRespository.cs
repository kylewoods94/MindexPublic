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

        public async Task<Employee> AddAsync(Employee employee)
        {
            employee.EmployeeId = Guid.NewGuid().ToString();
            await _employeeContext.Employees.AddAsync(employee);
            return employee;
        }

        public async Task<Employee> GetByIdAsync(string id)
        {
            // Include up to 4 levels of direct reports
            // the app can technically support infinite amount of direct reports, but for the purpose of this exercise, we'll limit it to 4
            // will turn to recursive loading if needed to scale to infinite levels
            return await _employeeContext.Employees
                .Include(e => e.DirectReports)
                .ThenInclude(e => e.DirectReports)
                .ThenInclude(e => e.DirectReports)
                .ThenInclude(e => e.DirectReports)
                .SingleOrDefaultAsync(e => e.EmployeeId == id);
        }

        public async Task<Employee> GetByIdRecursiveDirectReportsAsync(string id)
        {
            var employee = await _employeeContext.Employees
                .Include(e => e.DirectReports)
                .SingleOrDefaultAsync(e => e.EmployeeId == id);

            if (employee != null)
            {
                await LoadDirectReportsAsync(employee);
            }

            return employee;
        }

        private async Task LoadDirectReportsAsync(Employee employee)
        {
            if (employee.DirectReports != null && employee.DirectReports.Any())
            {
                foreach (var directReport in employee.DirectReports)
                {
                    var report = await _employeeContext.Employees
                        .Include(e => e.DirectReports)
                        .SingleOrDefaultAsync(e => e.EmployeeId == directReport.EmployeeId);

                    if (report != null)
                    {
                        directReport.FirstName = report.FirstName;
                        directReport.LastName = report.LastName;
                        directReport.Position = report.Position;
                        directReport.Department = report.Department;
                        directReport.DirectReports = report.DirectReports;

                        await LoadDirectReportsAsync(directReport);
                    }
                }
            }
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }

        public Employee Remove(Employee employee)
        {
            return _employeeContext.Remove(employee).Entity;
        }

        public Employee GetById(string id)
        {
            return GetByIdAsync(id).GetAwaiter().GetResult();
        }

        public Employee Add(Employee employee)
        {
            return AddAsync(employee).GetAwaiter().GetResult();
        }
    }
}
