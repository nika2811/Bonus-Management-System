using BonusManagementSystem.Repository;
using BonusManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace BonusManagementSystem.Controllers.Statistics;

public class EmployStatisticsController : Controller
{
    private readonly IEmployeeRepository _employeeRepository;


    public EmployStatisticsController(IEmployeeRepository employeeRepository, BonusService bonusService)
    {
        _employeeRepository = employeeRepository;
    }

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