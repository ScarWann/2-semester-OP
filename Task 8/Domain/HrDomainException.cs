namespace Task_8.Domain;

public class HrDomainException : Exception
{
    public HrDomainException() : base() { }

    public HrDomainException(string message) : base(message) { }

    public HrDomainException(string? message, Exception? innerException) : base(message, innerException) { }
}

