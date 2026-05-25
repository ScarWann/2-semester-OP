namespace Task_8.Domain;

public class Project(string name, decimal totalBudget) : ISearchable
{
    public string Name { get; set; } = name;
    public decimal TotalBudget { get; set; } = totalBudget;

    public bool ContainsKeyword(string keyword)
    {
        return this.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase);
    }

    public override string ToString()
    {
        return $"{this.Name} | Бюджет: {this.TotalBudget}";
    }
}
