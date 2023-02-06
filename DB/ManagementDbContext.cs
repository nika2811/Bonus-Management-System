using BonusManagementSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BonusManagementSystem.DB;

public class ManagementDbContext : DbContext
{
    public ManagementDbContext(DbContextOptions<ManagementDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Bonus> Bonuses { get; set; }
}