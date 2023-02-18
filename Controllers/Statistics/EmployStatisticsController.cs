using BonusManagementSystem.DB;
using BonusManagementSystem.Models;
using BonusManagementSystem.Repository;
using BonusManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BonusManagementSystem.Controllers.Statistics;

public class EmployStatisticsController : Controller
{
    private readonly IBonusRepository _bonusService;
    private readonly ManagementDbContext _context;
    private readonly IEmployeeRepository _employeeRepository;

    public EmployStatisticsController(IEmployeeRepository employeeRepository, IBonusRepository bonusService,
        ManagementDbContext context)
    {
        _employeeRepository = employeeRepository;
        _bonusService = bonusService;
        _context = context;
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
    public async Task<List<Employee>> Top10EmployeesWithMostBonuses()
    {
        var employeeBonuses = _context.Bonuses
            .GroupBy(b => b.EmployeeId)
            .Select(g => new { EmployeeId = g.Key, TotalBonuses = g.Sum(b => b.Amount) })
            .OrderByDescending(e => e.TotalBonuses)
            .Take(10)
            .ToList();

        var topEmployees = new List<Employee>();

        foreach (var employeeBonus in employeeBonuses)
        {
            var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeBonus.EmployeeId);
            topEmployees.Add(employee);
        }

        return topEmployees.ToList();
    }


    [HttpGet("statistics/top-10-referred-employees")]
    public List<Employee> GetTopReferralBonuses()
    {
        var employeesWithReferralBonuses = _context.Bonuses
            .Include(b => b.Employee)
            .Where(b => b.Employee.RecommenderId != null)
            .GroupBy(b => b.Employee.RecommenderId)
            .Select(g => new
            {
                RecommenderId = g.Key.Value,
                TotalReferralBonus = g.Sum(b => b.Amount)
            })
            .OrderByDescending(x => x.TotalReferralBonus)
            .Take(10)
            .ToList();

        var employeeIds = employeesWithReferralBonuses.Select(e => e.RecommenderId);

        var topEmployees = _context.Employees
            .Where(e => employeeIds.Contains(e.Id))
            .ToList();

        return topEmployees;
    }
}