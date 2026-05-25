namespace Task_8.Domain;

public class Position(string title, double baseSalary, double workingHoursPerMonth) : ISearchable
{
    public string Title { get; set; } = title;
    public double BaseSalary { get; set; } = baseSalary;
    public double WorkingHoursPerMonth { get; set; } = workingHoursPerMonth;

    public bool ContainsKeyword(string keyword)
    {
        return this.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase);
    }

    public override string ToString()
    {
        return $"{this.Title} | Очікувана заробітня плата: {this.BaseSalary} | Робочі години на місяць: {this.WorkingHoursPerMonth}";
    }
}
