namespace Task_8.Domain;

public abstract class Person(string firstName, string lastName)
{
    public string FirstName { get; set; } = firstName ?? throw new ArgumentNullException(nameof(firstName));
    public string LastName { get; set; } = lastName ?? throw new ArgumentNullException(nameof(lastName));
}
