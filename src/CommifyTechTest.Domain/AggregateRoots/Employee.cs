namespace CommifyTechTest.Domain.AggregateRoots;

public class Employee
{
    public int Id { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public DateOnly DateOfBirth { get; }

    public decimal AnnualIncome { get; set; }

    public Employee(
        int id,
        string firstName,
        string lastName,
        DateOnly dateOfBirth,
        decimal annualIncome)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        AnnualIncome = annualIncome;
    }
}
