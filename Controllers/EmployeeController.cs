using BonusManagementSystem.Models;
using BonusManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BonusManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly BonusService _bonusService;
    private readonly IEmployeeRepository _employeeRepository;


    public EmployeeController(IEmployeeRepository employeeRepository, BonusService bonusService)
    {
        _employeeRepository = employeeRepository;
        _bonusService = bonusService;
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
        var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);

        existingEmployee.Name = employee.Name;
        existingEmployee.LastName = employee.LastName;
        existingEmployee.PersonalNumber = employee.PersonalNumber;
        existingEmployee.Salary = employee.Salary;
        existingEmployee.Recommender = employee.Recommender;
        existingEmployee.DateOfCommencementOfWork = employee.DateOfCommencementOfWork;

        var updatedEmployee = await _employeeRepository.UpdateEmployeeAsync(existingEmployee);
        return Ok(updatedEmployee);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Employee>> DeleteEmployee(int id)
    {
        var employee = await _employeeRepository.DeleteEmployeeAsync(id);
        return Ok(employee);
    }


    [HttpPost("give-bonus")]
    public async Task<IActionResult> GiveBonus(int employeeId, [FromBody] Bonus model)
    {
        try
        {
            await _bonusService.GiveBonusAsync(employeeId, model.Percentage);
            return Ok("Bonus successfully issued");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /* ------------------------------ */

    // [HttpPost("give-bonus")]
    // public async Task<IActionResult> GiveBonus([FromBody] Bonus bonus)
    // {
    //     // Validate the incoming bonus model
    //     if (!ModelState.IsValid)
    //         return BadRequest(ModelState);
    //
    //     // Retrieve the employee
    //     var employee = await _employeeRepository.GetEmployeeByNameAsync(bonus.EmployeeName);
    //
    //     // Calculate the bonus amount
    //     var bonusAmount = bonus.Percentage * (double)employee.Salary;
    //     employee.Salary += (decimal)bonusAmount;
    //     employee.BonusDate = bonus.BonusDate;
    //
    //     // Update the employee
    //     await _employeeRepository.UpdateEmployeeAsync(employee);
    //
    //     // Check if the employee has a recommender
    //     var recommender = await _employeeRepository.GetEmployeeByIdAsync(employee.RecommenderId);
    //
    //     // Give bonus to the recommender
    //     var level = 1;
    //     while (level <= 3)
    //     {
    //         var recommenderBonusAmount = bonusAmount * 0.5;
    //         recommender.Salary += (decimal)recommenderBonusAmount;
    //         recommender.BonusDate = bonus.BonusDate;
    //
    //         await _employeeRepository.UpdateEmployeeAsync(recommender);
    //
    //         recommender = await _employeeRepository.GetEmployeeByIdAsync(recommender.RecommenderId);
    //         level++;
    //     }
    //
    //     return Ok();
    // }

    [HttpGet("statistics/bonus-count")]
    public async Task<IActionResult> GetBonusCount([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        if (!startDate.HasValue || !endDate.HasValue)
            return BadRequest("Start date and end date are required to calculate bonus count.");

        var bonusCount = await _employeeRepository.GetBonusCountAsync(startDate.Value, endDate.Value);
        return Ok(bonusCount);
    }

    [HttpGet("statistics/top10employees")]
    public async Task<IActionResult> GetTop10EmployeesWithMostBonuses()
    {
        try
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();
            var employeesWithBonuses = employees
                .Where(e => e.Bonuses.Count > 0)
                .OrderByDescending(e => e.Bonuses.Sum(b => b.Amount))
                .Take(10);

            return Ok(employeesWithBonuses);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet("statistics/top-10-referred-employees")]
    public async Task<IActionResult> GetTop10ReferredEmployees()
    {
        try
        {
            var employees = await _employeeRepository.GetAllEmployeesAsync();
            var employeesWithReferrals = employees
                .Where(e => e.ReferredEmployees.Count > 0)
                .OrderByDescending(e => e.ReferredEmployees.Sum(r => r.Bonuses.Sum(b => b.Amount)))
                .Take(10);

            return Ok(employeesWithReferrals);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}