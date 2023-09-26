using CommifyTechTest.Application.Commands;
using CommifyTechTest.Application.Infrastructure.Repositories;
using CommifyTechTest.Application.Settings;
using CommifyTechTest.Domain;
using MediatR;

namespace CommifyTechTest.Application.CommandHandlers;

internal class AddEmployeeCommandHandler : IRequestHandler<AddEmployeeCommand>
{
    private readonly IEmployeeRepository _repository;
    private readonly IEnumerable<TaxBand> _taxBands;

    public AddEmployeeCommandHandler(
        IEmployeeRepository repository,
        TaxBandSettings taxBandSettings)
    {
        _repository = repository;
        _taxBands = taxBandSettings.TaxBands;
    }

    public async Task Handle(AddEmployeeCommand command, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Executing {nameof(AddEmployeeCommandHandler)}...");

        var employee = await _repository.GetByIdAsync(command.Id, cancellationToken);

        if (employee is not null)
        {
            throw new InvalidOperationException($"Employee {command.Id} already exists.");
        }

        employee = new Domain.AggregateRoots.Employee(command.Id, command.FirstName, command.LastName, command.DateOfBirth);
        employee.CalculateNetAnnualSalary(command.GrossAnnualSalary, _taxBands);

        await _repository.AddEmployeeAsync(employee, cancellationToken);

        Console.WriteLine($"Executed {nameof(AddEmployeeCommandHandler)}.");
    }
}
