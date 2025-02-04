using CodeChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeChallenge.Services
{
    public interface IEmployeeService
    {
        Employee GetById(string id);
        Employee Create(Employee employee);
        Employee Replace(Employee originalEmployee, Employee newEmployee);
        Task<int> GetNumberOfDirectReportsAsync(Employee employee);
        Task<Employee> GetByIdAsync(string id, bool useRecursiveDirectReports);
        Task<Employee> CreateAsync(Employee employee);
        Task<Employee> ReplaceAsync(Employee originalEmployee, Employee newEmployee);
        
    }
}
