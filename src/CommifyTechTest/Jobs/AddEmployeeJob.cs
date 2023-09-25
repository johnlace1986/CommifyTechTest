using CommifyTechTest.Application.Commands;
using CommifyTechTest.Contracts;
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

    public async Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"Executing {nameof(AddEmployeeJob)}...");

        var employee = context
            .JobDetail
            .JobDataMap
            .Get("employee") as Employee;

        var cts = new CancellationTokenSource();
        await _mediator.Send(CreatedAddEmployeeCommand(employee), cts.Token);

        Console.WriteLine($"Executed {nameof(AddEmployeeJob)}.");
    }

    private static AddEmployeeCommand CreatedAddEmployeeCommand(Employee employee) =>
        new()
        {
            EmployeeID = employee.EmployeeID,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            DateOfBirth = employee.DateOfBirth,
            GrossAnnualSalary = employee.GrossAnnualSalary
        };
}
