using BonusManagementSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BonusManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees()
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult<Employee>> AddEmployee(Employee employee)
        {
            var newEmployee = await _employeeRepository.AddEmployeeAsync(employee);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = newEmployee.Id }, newEmployee);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, Employee employee)
        {
            if (id != employee.Id)
                return BadRequest();
            var updatedEmployee = await _employeeRepository.UpdateEmployeeAsync(employee);
            return Ok(updatedEmployee);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            var employee = await _employeeRepository.DeleteEmployeeAsync(id);
            return Ok(employee);
        }

        [HttpPut("{id}/bonus")]
        public async Task<ActionResult<Employee>> GiveBonus(int id, double bonusPercentage)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
            if (bonusPercentage < 50 || bonusPercentage > 300)
                return BadRequest("Bonus percentage must be between 50% and 300%");

            var bonusAmount = employee.Remuneration * (decimal)(bonusPercentage / 100);
            employee.Remuneration += bonusAmount;

            // Code to calculate recommender bonus...

            var updatedEmployee = await _employeeRepository.UpdateEmployeeAsync(employee);
            return Ok(updatedEmployee);
        }
        
        [HttpPost("give-bonus")]
        public async Task<IActionResult> GiveBonus([FromBody] BonusModel bonusModel)
        {
            try
            {
                Employee employee = await _employeeRepository.GetEmployeeByNameAsync(bonusModel.EmployeeName);
                if (employee == null)
                    return NotFound($"Employee with name {bonusModel.EmployeeName} not found.");

                decimal bonusAmount = employee.Remuneration * (decimal)(bonusModel.Percentage / 100);
                employee.Remuneration += bonusAmount;
                await _employeeRepository.UpdateEmployeeAsync(employee);

                // Give bonus to the recommender
                
                Employee recommender = await _employeeRepository.GetEmployeeByNameAsync(employee.Recommender.Name);
                
                decimal recommenderBonusAmount = bonusAmount / 2;
                recommender.Remuneration += recommenderBonusAmount;
                await _employeeRepository.UpdateEmployeeAsync(recommender);
                
                

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("statistics/bonus-count")]
        public async Task<IActionResult> GetBonusCount([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                return BadRequest("Start date and end date are required to calculate bonus count.");
            }

            var bonusCount = await _employeeRepository.GetBonusCountAsync(startDate.Value, endDate.Value);
            return Ok(bonusCount);
        }
    }
}
