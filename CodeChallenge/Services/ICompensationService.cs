using System.Threading.Tasks;
using CodeChallenge.Models;

namespace CodeChallenge.Services
{
    public interface ICompensationService
    {
        Task<Compensation> GetByEmployeeIdAsync(string employeeId);
        Task<Compensation> CreateAsync(Compensation compensation);
        Task<Compensation> UpdateAsync(Compensation compensation);
    }
}
