using BonusManagementSystem.DB;
using BonusManagementSystem.Models;

namespace BonusManagementSystem.Services;

public class BonusService:IBonusService
{
    private readonly ManagementDbContext _context;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly int _maxLevels;

    public BonusService(IEmployeeRepository employeeRepository, ManagementDbContext context)
    {
        _employeeRepository = employeeRepository;
        _context = context;
        _maxLevels = 3;
    }

    public async Task GiveBonusAsync(int employeeId, double percentage)
    {
        var employee = await _employeeRepository.GetEmployeeByIdAsync(employeeId);
        if (employee == null)
            throw new Exception("Employee not found");

        var bonusAmount = employee.Salary * (decimal)(percentage / 100);
        
        var bonus = new Bonus
        {
            EmployeeId = employee.Id,
            Percentage = percentage,
            Amount = bonusAmount,
            BonusDate = DateTime.Now
        };
        _context.Bonuses.Add(bonus);
        await _context.SaveChangesAsync();

        var currentLevel = 1;
        var recommender = await _employeeRepository.GetEmployeeByIdAsync(employee.RecommenderId);
        while (currentLevel <= _maxLevels)
        {
            var recommenderBonusAmount = bonusAmount * (decimal)0.5;
            var recommenderBonus = new Bonus
            {
                EmployeeId = recommender.Id,
                Percentage = percentage * 0.5,
                Amount = recommenderBonusAmount,
                BonusDate = DateTime.Now
            };
            _context.Bonuses.Add(recommenderBonus);
            await _context.SaveChangesAsync();
            

            currentLevel++;
            recommender = await _employeeRepository.GetEmployeeByIdAsync(recommender.RecommenderId);
        }

        await _employeeRepository.UpdateEmployeeAsync(employee);
        await _context.SaveChangesAsync();
    }
}