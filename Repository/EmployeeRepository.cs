using BonusManagementSystem.DB;
using BonusManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BonusManagementSystem.Repository;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ManagementDbContext _context;

    public EmployeeRepository(ManagementDbContext context)
    {
        _context = context;
    }

    public async Task<Employee> AddEmployeeAsync(Employee employee)
    {
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task<Employee> GetEmployeeByIdAsync(int? id)
    {
        return (await _context.Employees.FirstOrDefaultAsync(e => e.Id == id))!;
    }

    public async Task<Employee> GetEmployeeByNameAsync(string name)
    {
        return (await _context.Employees.FirstOrDefaultAsync(e => e.Name == name))!;
    }

    public async Task<Employee> UpdateEmployeeAsync(Employee employee)
    {
        _context.Employees.Update(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task<bool> DeleteEmployeeAsync(int id)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        if (employee == null)
            return false;

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
    {
        return await _context.Employees.ToListAsync();
    }

    public async Task<int> GetBonusCountAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Bonuses
            .Where(e => e.CreatedAt >= startDate && e.CreatedAt <= endDate)
            .CountAsync();
    }
}