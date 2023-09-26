using CommifyTechTest.Contracts;
using CommifyTechTest.Services;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using System.Text;

namespace CommifyTechTest.Tests.Services;

public class EmployeesParserTests
{
    [Test]
    public async Task ValidData()
    {
        var expectedEmployee = new Employee
        {
            Id = 1,
            FirstName = "John",
            LastName = "Smith",
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow),
            GrossAnnualSalary = 12345
        };

        var builder = new StringBuilder();
        builder.AppendLine("EmployeeID,FirstName,LastName,DateOfBirth,GrossAnnualSalary");
        builder.AppendLine($"{expectedEmployee.Id},{expectedEmployee.FirstName},{expectedEmployee.LastName},{expectedEmployee.DateOfBirth},{expectedEmployee.GrossAnnualSalary}");

        using var stream = new MemoryStream(Encoding.Default.GetBytes(builder.ToString()));

        var sut = new EmployeesParser();

        var result = await sut.ParseAsync(stream, CancellationToken.None);

        using (new AssertionScope())
        {
            var parsedEmployee = result.Should().ContainSingle().Subject;
            parsedEmployee.Id.Should().Be(expectedEmployee.Id);
            parsedEmployee.FirstName.Should().Be(expectedEmployee.FirstName);
            parsedEmployee.LastName.Should().Be(expectedEmployee.LastName);
            parsedEmployee.DateOfBirth.Should().Be(expectedEmployee.DateOfBirth);
            parsedEmployee.GrossAnnualSalary.Should().Be(expectedEmployee.GrossAnnualSalary);
        }
    }
}