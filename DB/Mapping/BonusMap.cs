using BonusManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BonusManagementSystem.DB.Mapping;

public class BonusMap : IEntityTypeConfiguration<Bonus>
{
    public void Configure(EntityTypeBuilder<Bonus> builder)
    {
        builder.HasOne(t => t.Employee).WithMany().HasForeignKey(t => t.EmployeeId);
    }
}