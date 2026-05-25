namespace Task_8.Domain;

public class HrEngine
{
    public List<Employee> AllEmployees { get; } = [];
    public List<Department> AllDepartments { get; } = [];
    public List<Project> AllProjects { get; } = [];
    public List<Position> AllPositions { get; } = [];

    public void AddEmployee(Employee employee)
    {
        if (employee == null) throw new ArgumentNullException(nameof(employee));
        this.AllEmployees.Add(employee);
    }

    public void DeleteEmployee(Employee employee)
    {
        if (!this.AllEmployees.Contains(employee))
            throw new HrDomainException("Працівника не знайдено в системі.");

        if (employee.Unit != null)
        {
            employee.Unit.Employees.Remove(employee);
        }
        this.AllEmployees.Remove(employee);
    }

    public List<Employee> GetAllEmployeesSorted(int sortType)
    {
        return sortType switch
        {
            1 => this.AllEmployees.OrderBy(e => e.FirstName).ToList(),
            2 => this.AllEmployees.OrderBy(e => e.LastName).ToList(),
            3 => this.AllEmployees.OrderBy(e => e.CurrentPosition?.BaseSalary ?? 0).ToList(),
            _ => this.AllEmployees
        };
    }

    public void AddDepartment(Department department)
    {
        this.AllDepartments.Add(department);
    }

    public void AssignEmployeeToDepartment(Employee emp, Department dept)
    {
        if (emp == null || dept == null) throw new HrDomainException("Некоректні дані призначення.");
        if (emp.Unit != null) emp.Unit.Employees.Remove(emp);

        emp.Unit = dept;
        dept.Employees.Add(emp);
    }

    public List<Employee> GetDepartmentEmployeesSorted(Department dept, int sortType)
    {
        return sortType switch
        {
            1 => dept.Employees.OrderBy(e => e.CurrentPosition?.Title).ToList(),
            2 => dept.Employees.OrderByDescending(e => e.GetTotalProjectsCost()).ToList(),
            _ => dept.Employees
        };
    }

    public IEnumerable<Position> GetTop5AttractivePositions()
    {
        return this.AllPositions
            .Where(p => p.WorkingHoursPerMonth > 0)
            .OrderByDescending(p => p.BaseSalary / p.WorkingHoursPerMonth)
            .Take(5);
    }

    public Employee? GetMostProfitableEmployeeAtPosition(Position position)
    {
        var positionEmployees = this.AllEmployees.Where(e => e.CurrentPosition == position).ToList();
        if (!positionEmployees.Any()) return null;

        return positionEmployees
            .OrderByDescending(e => e.ExperienceYears == 0 ? (double)e.GetTotalProjectsCost() : (double)e.GetTotalProjectsCost() / e.ExperienceYears)
            .FirstOrDefault();
    }

    public List<Employee> SearchWorkersByKeyword(string keyword) =>
        this.AllEmployees.Where(e => e.ContainsKeyword(keyword)).ToList();

    public List<Project> SearchProjectsByKeyword(string keyword) =>
        this.AllProjects.Where(p => p.ContainsKeyword(keyword)).ToList();

    public List<ISearchable> GlobalSearch(string keyword)
    {
        var results = new List<ISearchable>();
        results.AddRange(this.AllEmployees.Where(e => e.ContainsKeyword(keyword)));
        results.AddRange(this.AllProjects.Where(p => p.ContainsKeyword(keyword)));
        results.AddRange(this.AllPositions.Where(p => p.ContainsKeyword(keyword)));
        results.AddRange(this.AllDepartments.Where(d => d.ContainsKeyword(keyword)));
        return results;
    }

    public List<Employee> AdvancedEmployeeSearch(
    string? firstName = null,
    string? lastName = null,
    string? accountNumber = null,
    string? positionTitle = null,
    int? minExperience = null)
    {
        var query = this.AllEmployees.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(firstName))
            query = query.Where(e => e.FirstName.Contains(firstName, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(lastName))
            query = query.Where(e => e.LastName.Contains(lastName, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(accountNumber))
            query = query.Where(e => e.SalaryAccountNumber.Contains(accountNumber, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(positionTitle))
            query = query.Where(e => e.CurrentPosition != null &&
                                    e.CurrentPosition.Title.Contains(positionTitle, StringComparison.OrdinalIgnoreCase));

        if (minExperience.HasValue)
            query = query.Where(e => e.ExperienceYears >= minExperience.Value);

        return query.ToList();
    }
}
