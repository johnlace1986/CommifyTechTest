using CommifyTechTest.Application.Commands;
using CommifyTechTest.Application.Infrastructure.Repositories;
using MediatR;

namespace CommifyTechTest.Application.CommandHandlers;

internal class AddEmployeeCommandHandler : IRequestHandler<AddEmployeeCommand>
{
    private readonly IEmployeeRepository _repository;

    public AddEmployeeCommandHandler(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(AddEmployeeCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var employee = await _repository.GetByIdAsync(command.EmployeeID, cancellationToken);

            if (employee is not null)
            {
                //do something
                return;
            }

            employee = new Domain.AggregateRoots.Employee(command.EmployeeID, command.FirstName, command.LastName, command.DateOfBirth, command.GrossAnnualSalary);

            await _repository.AddEmployeeAsync(employee, cancellationToken);

        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
