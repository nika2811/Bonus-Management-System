using System.ComponentModel.DataAnnotations;

namespace BonusManagementSystem.Models;

public class BonusModel
{
    [Required]
    public string EmployeeName { get; set; }

    [Required]
    [Range(0.5, 3.0)]
    public double Percentage { get; set;}

    [Required]
    public DateTime BonusDate { get; set; }
}