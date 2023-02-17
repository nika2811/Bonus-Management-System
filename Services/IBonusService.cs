namespace BonusManagementSystem.Services;

public interface IBonusService
{
    Task GiveBonusAsync(int employeeId, double percentage);
}