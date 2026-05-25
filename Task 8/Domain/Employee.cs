namespace Task_8.Domain;

public class Employee(string firstName, string lastName, string accountNumber, int experienceYears, Position position) : Person(firstName, lastName), ISearchable
{
    public string SalaryAccountNumber { get; set; } = accountNumber;
    public int ExperienceYears { get; set; } = experienceYears;
    public Position CurrentPosition { get; set; } = position;
    public List<Project> Projects { get; } = [];
    public Department? Unit { get; set; }

    public decimal GetTotalProjectsCost()
    {
        return this.Projects.Sum(p => p.TotalBudget);
    }

    public bool ContainsKeyword(string keyword)
    {
        return this.FirstName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
               this.LastName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
               this.SalaryAccountNumber.Contains(keyword, StringComparison.OrdinalIgnoreCase);
    }

    public override string ToString()
    {
        return $"{this.FirstName} {this.LastName} | Рахунок: {this.SalaryAccountNumber} | Підрозділ: {this.Unit?.Name ?? "Немає"} | Посада: {this.CurrentPosition?.Title ?? "Немає"} | Стаж: {this.ExperienceYears} р.";
    }
}
