using BonusManagementSystem.Models;
using BonusManagementSystem.Repository;
using BonusManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BonusManagementSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IBonusRepository _bonusService;
    private readonly IEmployeeRepository _employeeRepository;


    public EmployeeController(IEmployeeRepository employeeRepository, IBonusRepository bonusService)
    {
        _employeeRepository = employeeRepository;
        _bonusService = bonusService;
    }

    [HttpGet("All-Employees")]
    public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployees()
    {
        var employees = await _employeeRepository.GetAllEmployeesAsync();
        return Ok(employees);
    }

    [HttpGet("Employee:{id}")]
    public async Task<ActionResult<Employee>> GetEmployeeById(int id)
    {
        var employee = await _employeeRepository.GetEmployeeByIdAsync(id);
        return Ok(employee);
    }


    [HttpGet("Employee-{name}")]
    public async Task<ActionResult<Employee>> GetEmployeeByName(string name)
    {
        var employee = await _employeeRepository.GetEmployeeByNameAsync(name);
        return Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> AddEmployee(Employee employee)
    {
        var newEmployee = await _employeeRepository.AddEmployeeAsync(employee);
        return CreatedAtAction(nameof(GetEmployeeById), new { id = newEmployee.Id }, newEmployee);
    }


    [HttpPut("UpdateEmployee-{id}")]
    public async Task<ActionResult<Employee>> UpdateEmployee(int id, Employee employee)
    {
        if (id != employee.Id)
            return BadRequest();
        var existingEmployee = await _employeeRepository.GetEmployeeByIdAsync(id);

        existingEmployee.Name = employee.Name;
        existingEmployee.LastName = employee.LastName;
        existingEmployee.PersonalNumber = employee.PersonalNumber;
        existingEmployee.Salary = employee.Salary;
        existingEmployee.EmployedAt = employee.EmployedAt;

        var updatedEmployee = await _employeeRepository.UpdateEmployeeAsync(existingEmployee);
        return Ok(updatedEmployee);
    }

    [HttpDelete("DeleteEmployee-{id}")]
    public async Task<ActionResult<Employee>> DeleteEmployee(int id)
    {
        var employee = await _employeeRepository.DeleteEmployeeAsync(id);
        return Ok(employee);
    }


    [HttpPost("give-bonus")]
    public async Task<IActionResult> GiveBonus(int employeeId, double percentage)
    {
        await _bonusService.GiveBonusAsync(employeeId, percentage);
        return Ok("Bonus successfully issued");
    }
}