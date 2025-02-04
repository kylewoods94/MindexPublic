using System.Threading.Tasks;
using CodeChallenge.Models;

namespace CodeChallenge.Repositories
{
    public interface ICompensationRepository
    {
        Task<Compensation> GetByEmployeeIdAsync(string id);
        Task<Compensation> AddAsync(Compensation compensation);
        Task SaveAsync();
        void Update(Compensation compensation);
    }
}
