using MediatR;

namespace CommifyTechTest.Application.Commands;

public class AddEmployeeCommand : IRequest
{
    public int EmployeeID { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public int GrossAnnualSalary { get; set; }
}
