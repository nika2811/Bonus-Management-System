namespace BonusManagementSystem.Models;

public class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public int PersonalNumber { get; set; }
    public decimal Salary { get; set; }
    public int? RecommenderId { get; set; }

    public DateTime EmployedAt { get; set; }
    // public DateTime CreatedAt { get; set; }
    // public List<Bonus> Bonuses { get; set; }
    // public IList<Employee> ReferredEmployees { get; set; }
}