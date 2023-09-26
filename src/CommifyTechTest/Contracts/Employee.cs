namespace CommifyTechTest.Contracts;

public class Employee
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public int GrossAnnualSalary { get; set; }
}
