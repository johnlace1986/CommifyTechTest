using CommifyTechTest.Application.Commands;
using CommifyTechTest.Services;
using MediatR;
using Quartz;

namespace CommifyTechTest.Jobs;

public class AddEmployeesJob : IJob
{
    public const string GroupKey = "JobGroups";

    private readonly IMediator _mediator;

    public AddEmployeesJob(IMediator mediator)
    {
        _mediator = mediator;
    }

    public static string GenerateJobKey() => $"{nameof(AddEmployeesJob)}-{Guid.NewGuid()}";

    public Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"Executing {nameof(AddEmployeesJob)}...");

        var employees = context
            .JobDetail
            .JobDataMap
            .Get("employees") as IEnumerable<IEmployeesParser.Employee>;

        return Task.WhenAll(employees.Select(employee =>
        {
            var cts = new CancellationTokenSource();
            return _mediator.Send(CreatedAddEmployeeCommand(employee), cts.Token);
        }));
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
