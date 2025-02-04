using System.Threading.Tasks;
using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompensationController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ICompensationService _compensationService;
        private readonly ILogger _logger;

        public CompensationController(IEmployeeService employeeService, ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _employeeService = employeeService;
            _logger = logger;
            _compensationService = compensationService;
        }

        [HttpGet("{employeeId}", Name = "getByEmployeeId")]
        public async Task<IActionResult> GetByEmployeeId(string employeeId)
        {
            _logger.LogDebug($"Received compensation get request for '{employeeId}'");

            var compensation = await _compensationService.GetByEmployeeIdAsync(employeeId);

            if (compensation == null) return NotFound();
            return Ok(compensation);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request for '{compensation.EmployeeId}'");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var employee = await _employeeService.GetByIdAsync(compensation.EmployeeId, false);
            if (employee == null) return BadRequest("Invalid employee ID.");

            var existingCompensation = await _compensationService.GetByEmployeeIdAsync(compensation.EmployeeId);
            if (existingCompensation != null) return BadRequest("Compensation already exists for this employee.");

            var createdCompensation = await _compensationService.CreateAsync(compensation);
            return CreatedAtRoute("getByEmployeeId", new { employeeId = createdCompensation.EmployeeId }, createdCompensation);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation update request for '{compensation.EmployeeId}'");
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (compensation.EmployeeId != id) return BadRequest("Employee ID does not match between route and body.");

            var existingEmployee = await _employeeService.GetByIdAsync(id, false);
            if (existingEmployee == null) return BadRequest("Invalid employee ID.");

            var existingCompensation = await _compensationService.GetByEmployeeIdAsync(compensation.EmployeeId);
            if (existingCompensation == null) return NotFound();

            await _compensationService.UpdateAsync(compensation);
            return Ok(compensation);
        }
    }
}
