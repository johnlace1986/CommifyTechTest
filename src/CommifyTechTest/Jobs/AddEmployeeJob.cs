using CommifyTechTest.Application.Commands;
using CommifyTechTest.Services;
using MediatR;
using Quartz;

namespace CommifyTechTest.Jobs;

public class AddEmployeeJob : IJob
{
    public const string GroupKey = "JobGroups";

    private readonly IMediator _mediator;

    public AddEmployeeJob(IMediator mediator)
    {
        _mediator = mediator;
    }

    public static string GenerateJobKey(int employeeId) => $"{nameof(AddEmployeeJob)}-{employeeId}-{Guid.NewGuid()}";

    public Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"Executing {nameof(AddEmployeeJob)}...");

        var employee = context
            .JobDetail
            .JobDataMap
            .Get("employee") as IEmployeesParser.Employee;

        var cts = new CancellationTokenSource();
        return _mediator.Send(CreatedAddEmployeeCommand(employee), cts.Token);
    }

    private static AddEmployeeCommand CreatedAddEmployeeCommand(IEmployeesParser.Employee employee) =>
        new()
        {
            EmployeeID = employee.EmployeeID,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            DateOfBirth = employee.DateOfBirth,
            GrossAnnualSalary = employee.GrossAnnualSalary
        };
}
