using BonusManagementSystem.DB;
using BonusManagementSystem.Models;

namespace BonusManagementSystem.Services;

public class BonusService:IBonusService
{
    private readonly ManagementDbContext _context;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly int _maxLevels;

    public BonusService(IEmployeeRepository employeeRepository, ManagementDbContext context, int maxLevels)
    {
        _employeeRepository = employeeRepository;
        _context = context;
        _maxLevels = maxLevels;
    }

    public async Task GiveBonusAsync(int employeeId, double percentage)
    {
        var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
        if (employee == null)
            throw new Exception("Employee not found");

        var bonusAmount = employee.Salary * (decimal)(percentage / 100);
        employee.Bonuses.Add(new Bonus
        {
            Percentage = percentage,
            Amount = bonusAmount,
            BonusDate = DateTime.Now
        });

        var currentLevel = 1;
        var recommender = await _employeeRepository.GetEmployeeByIdAsync(employee.RecommenderId);
        while (currentLevel <= _maxLevels)
        {
            var recommenderBonusAmount = bonusAmount * (decimal)0.5;
            recommender.Bonuses.Add(new Bonus
            {
                Percentage = percentage * 0.5,
                Amount = recommenderBonusAmount,
                BonusDate = DateTime.Now
            });

            currentLevel++;
            recommender = await _employeeRepository.GetEmployeeByIdAsync(recommender.RecommenderId);
        }

        await _employeeRepository.UpdateEmployeeAsync(employee);
        await _context.SaveChangesAsync();
    }
}