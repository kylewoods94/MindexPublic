﻿using CodeChallenge.Models;
using System;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public interface IEmployeeRepository
    {
        Employee GetById(string id);
        Employee Add(Employee employee);
        Employee Remove(Employee employee);
        Task SaveAsync();
        Task<Employee> GetByIdAsync(string id);
        Task<Employee> GetByIdRecursiveDirectReportsAsync(string id);
        Task<Employee> AddAsync(Employee employee);
    }
}