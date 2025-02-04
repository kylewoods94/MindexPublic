using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Data;
using CodeChallenge.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly EmployeeContext _context;
        private readonly ILogger<ICompensationRepository> _logger;

        public CompensationRepository(ILogger<ICompensationRepository> logger, EmployeeContext context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Compensation> AddAsync(Compensation compensation)
        {
            await _context.Compensations.AddAsync(compensation);
            return compensation;
        }

        public async Task<Compensation> GetByEmployeeIdAsync(string employeeId)
        {
            return await _context.Compensations.SingleOrDefaultAsync(c => c.EmployeeId == employeeId);
        }

        public Task SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Update(Compensation compensation)
        {
            _context.Entry(compensation).State = EntityState.Modified;
        }
    }
}
