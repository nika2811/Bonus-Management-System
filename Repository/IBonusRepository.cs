namespace BonusManagementSystem.Services;

public interface IBonusRepository
{
    Task GiveBonusAsync(int employeeId, double percentage);
}