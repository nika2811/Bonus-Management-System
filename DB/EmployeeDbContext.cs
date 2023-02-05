using BonusManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BonusManagementSystem.DB
{
    public class EmployeeDbContext:DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
