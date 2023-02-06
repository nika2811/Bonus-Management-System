using BonusManagementSystem.Models;

namespace BonusManagementSystem.Services;

public class CalculateBonus
{
    private readonly EmployeeRepository _employeeRepository;

    public CalculateBonus(EmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    private void CalculateRecommenderBonus(string employeeName, double bonusAmount, DateTime bonusDate, int level)
    {
        if (level > 3)
        {
            return;
        }

        var employee = _employeeRepository.GetEmployeeByNameAsync(employeeName);
        if (employee == null || string.IsNullOrEmpty(employee.Recommender))
        {
            return;
        }

        var recommenderBonus = bonusAmount * 0.5;
        var recommender = _employeeRepository.GetEmployeeByNameAsync(employee.Recommender);
        if (recommender == null)
        {
            return;
        }

        recommender.Remuneration += recommenderBonus;
        _employeeRepository.UpdateEmployeeAsync(recommender);

        CalculateRecommenderBonus(recommender.Name, recommenderBonus, bonusDate, level + 1);
    }
}