using CommifyTechTest.Contracts;
using CommifyTechTest.Controllers;
using CommifyTechTest.Services;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Quartz;

namespace CommifyTechTest.Tests.Controllers;

internal class EmployeesControllerTests
{
    [Test]
    public async Task MalformedCsvFile()
    {
        var employeesParserMock = new Mock<IEmployeesParser>();
        employeesParserMock.Setup(mock => mock.ParseAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        var sut = new EmployeesController(
            employeesParserMock.Object,
            Mock.Of<IScheduler>());

        var result = await sut.LoadFromFormDataAsync(FormFile, CancellationToken.None);

        result.Should().BeOfType<BadRequestResult>();
    }

    [Test]
    public async Task ValidCsvFile()
    {
        var employees = new[]
        {
            new Employee { EmployeeID = 1, FirstName = "John", LastName = "Smith", DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow), GrossAnnualSalary = 10000 }
        };

        var employeesParserMock = new Mock<IEmployeesParser>();
        employeesParserMock.Setup(mock => mock.ParseAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(employees);

        var schedulerMock = new Mock<IScheduler>();

        var sut = new EmployeesController(
            employeesParserMock.Object,
            schedulerMock.Object);

        var result = await sut.LoadFromFormDataAsync(FormFile, CancellationToken.None);

        using (new AssertionScope())
        {
            schedulerMock.Verify(mock => mock.AddJob(It.IsAny<IJobDetail>(), true, true, It.IsAny<CancellationToken>()), Times.Once);
            schedulerMock.Verify(mock => mock.TriggerJob(It.IsAny<JobKey>(), It.IsAny<CancellationToken>()), Times.Once);

            result.Should().BeOfType<AcceptedResult>();
        }
    }

    private static IFormFile FormFile
    {
        get
        {
            var formFileMock = new Mock<IFormFile>();
            formFileMock.Setup(mock => mock.OpenReadStream()).Returns(new MemoryStream());

            return formFileMock.Object;
        }
    }
}
