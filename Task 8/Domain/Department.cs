namespace Task_8.Domain;

public class Department(string name) : ISearchable
{
    public string Name { get; set; } = name;

    public List<Employee> Employees { get; } = [];

    public bool ContainsKeyword(string keyword)
    {
        return this.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase);
    }

    public override string ToString()
    {
        return $"{this.Name}";
    }
}

