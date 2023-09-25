﻿using CommifyTechTest.Application.Commands;
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
        var employee = await _repository.GetByIdAsync(command.EmployeeID, cancellationToken);

        if (employee is not null)
        {
            //do something
            return;
        }

        employee = new Domain.AggregateRoots.Employee(command.EmployeeID, command.FirstName, command.LastName, command.DateOfBirth);
        employee.CalculateAnnualIncome(command.GrossAnnualSalary, _taxBands);

        await _repository.AddEmployeeAsync(employee, cancellationToken);
    }
}
