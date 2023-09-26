using CommifyTechTest.Application.CommandHandlers;
using CommifyTechTest.Application.Commands;
using CommifyTechTest.Application.Infrastructure.Repositories;
using CommifyTechTest.Domain;
using CommifyTechTest.Domain.AggregateRoots;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CommifyTechTest.Application.Tests.CommandHandlers;

public class AddEmployeeCommandHandlerTests
{
    [Test]
    public async Task EmployeeAlreadyExists()
    {
        var employee = new Employee(1, "John", "Smith", DateOnly.FromDateTime(DateTime.UtcNow));

        var repositoryMock = new Mock<IEmployeeRepository>();
        repositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(employee);

        var sut = new AddEmployeeCommandHandler(
            repositoryMock.Object,
            new Settings.TaxBandSettings
            {
                TaxBands = Enumerable.Empty<TaxBand>()
            });

        var work = async () => await sut.Handle(new AddEmployeeCommand { Id = 1 }, CancellationToken.None);

        await work.Should().ThrowAsync<InvalidOperationException>();
    }

    [Test]
    public async Task ValidCommand()
    {
        var command = new AddEmployeeCommand
        {
            Id = 1,
            FirstName = "John",
            LastName = "Smith",
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow),
            GrossAnnualSalary = 10000
        };

        var repositoryMock = new Mock<IEmployeeRepository>();
        repositoryMock.Setup(mock => mock.GetByIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(default(Employee));

        var sut = new AddEmployeeCommandHandler(
            repositoryMock.Object,
            new Settings.TaxBandSettings
            {
                TaxBands = Enumerable.Empty<TaxBand>()
            });

        await sut.Handle(command, CancellationToken.None);

        repositoryMock.Verify(mock => mock.AddEmployeeAsync(It.Is<Employee>(employee =>
            employee.Id == command.Id &&
            employee.FirstName == command.FirstName &&
            employee.LastName == command.LastName &&
            employee.DateOfBirth == command.DateOfBirth &&
            employee.NetAnnualSalary == command.GrossAnnualSalary), It.IsAny<CancellationToken>()), Times.Once);
    }
}
