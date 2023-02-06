﻿using System.ComponentModel.DataAnnotations;

namespace BonusManagementSystem.Models;

public class Bonus
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    [Required] public string EmployeeName { get; set; }
    public Employee Employee { get; set; }
    public decimal Amount { get; set; }
    [Range(0.5, 3.0)] public double Percentage { get; set; }
    [Required] public DateTime BonusDate { get; set; }
}