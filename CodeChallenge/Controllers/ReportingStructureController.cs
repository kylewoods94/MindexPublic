using System.Threading.Tasks;
using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportingStructureController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger _logger;
        public ReportingStructureController(ILogger<ReportingStructureController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }

        [HttpGet("{id}", Name = "getReportingStructureById")]
        public async Task<IActionResult> GetReportingStructureById(string id)
        {
            _logger.LogDebug($"Received reporting structure get request for employee: '{id}'");
            var employee = await _employeeService.GetByIdAsync(id, false);
            if (employee == null) return NotFound();

            var numberOfReports = await _employeeService.GetNumberOfDirectReportsAsync(employee);
            var reportingStructure = new ReportingStructure
            {
                Employee = $"{employee.FirstName} {employee.LastName}",
                NumberOfReports = numberOfReports
            };

            return Ok(reportingStructure);
        }
    }
}
