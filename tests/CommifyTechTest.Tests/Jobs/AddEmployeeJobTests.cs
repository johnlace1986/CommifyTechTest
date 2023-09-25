using CommifyTechTest.Application.Commands;
using CommifyTechTest.Contracts;
using CommifyTechTest.Jobs;
using MediatR;
using Moq;
using NUnit.Framework;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommifyTechTest.Tests.Jobs;

public class AddEmployeeJobTests
{
    [Test]
    public async Task CommandSent()
    {
        var employee = new Employee
        {
            EmployeeID = 1,
            FirstName = "John",
            LastName = "Smith",
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow),
            GrossAnnualSalary = 12345
        };

        var jobData = new JobDataMap { ["employee"] = employee };
        var jobDetail = Mock.Of<IJobDetail>(mock => mock.JobDataMap == jobData);
        var context = Mock.Of<IJobExecutionContext>(mock => mock.JobDetail == jobDetail);

        var mediatorMock = new Mock<IMediator>();

        var sut = new AddEmployeeJob(mediatorMock.Object);
        await sut.Execute(context);

        mediatorMock.Verify(mock => mock.Send(It.Is<AddEmployeeCommand>(command =>
            command.EmployeeID == employee.EmployeeID &&
            command.FirstName == employee.FirstName &&
            command.LastName == employee.LastName &&
            command.DateOfBirth == employee.DateOfBirth &&
            command.GrossAnnualSalary == employee.GrossAnnualSalary), It.IsAny<CancellationToken>()), Times.Once);
    }
}
