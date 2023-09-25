namespace CommifyTechTest.Services;

public interface IEmployeesParser
{
    Task<IEnumerable<Employee>> ParseAsync(Stream stream, CancellationToken cancellationToken);

    public class Employee
    {
        public int EmployeeID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public int GrossAnnualSalary { get; set; }
    }
}