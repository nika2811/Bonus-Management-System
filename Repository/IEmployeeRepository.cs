using BonusManagementSystem.Models;

namespace BonusManagementSystem.Repository;

public interface IEmployeeRepository
{
    Task<Employee> AddEmployeeAsync(Employee employee);
    Task<Employee> GetEmployeeByIdAsync(int? id);
    Task<Employee> UpdateEmployeeAsync(Employee employee);
    Task<bool> DeleteEmployeeAsync(int id);
    Task<IEnumerable<Employee>> GetAllEmployeesAsync();
    Task<int> GetBonusCountAsync(DateTime startDate, DateTime endDate);
    Task<Employee> GetEmployeeByNameAsync(string name);
}