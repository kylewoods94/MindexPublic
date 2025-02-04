using System.Threading.Tasks;
using CodeChallenge.Models;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;

        public CompensationService(ICompensationRepository compensationRepository)
        {
            _compensationRepository = compensationRepository;
        }

        public async Task<Compensation> GetByEmployeeIdAsync(string employeeId)
        {
            return await _compensationRepository.GetByEmployeeIdAsync(employeeId);
        }

        public async Task<Compensation> CreateAsync(Compensation compensation)
        {
            await _compensationRepository.AddAsync(compensation);
            await _compensationRepository.SaveAsync();
            return compensation;
        }

        public async Task<Compensation> UpdateAsync(Compensation compensation)
        {
            var existingCompensation = await _compensationRepository.GetByEmployeeIdAsync(compensation.EmployeeId);
            existingCompensation.Salary = compensation.Salary;
            existingCompensation.EffectiveDate = compensation.EffectiveDate;
            _compensationRepository.Update(existingCompensation);
            await _compensationRepository.SaveAsync();
            return compensation;
        }
    }
}
