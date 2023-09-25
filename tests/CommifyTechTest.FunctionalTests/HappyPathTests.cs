using EmployeeContract = CommifyTechTest.Contracts.Employee;
using CommifyTechTest.Persistence;
using FluentAssertions.Execution;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;
using EmployeeDataModel = CommifyTechTest.Persistence.Models.Employee;

namespace CommifyTechTest.FunctionalTests;

public class HappyPathTests
{
    private static WebApplicationFactory<Program> Application => new();

    private HttpClient Client { get; set; }

    private readonly EmployeeContract _employee = new() 
    { 
        EmployeeID = int.MaxValue, 
        FirstName = "John", 
        LastName = "Smith", 
        DateOfBirth = new DateOnly(1986, 10, 1), 
        GrossAnnualSalary = 10000 
    };

    private readonly decimal _expectedAnnualIncome = 9000;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        Client = Application.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Test]
    public async Task LoadEmployees()
    {
        using var stream = await GenerateFile(_employee);

        using var request = new HttpRequestMessage(HttpMethod.Post, "employees");
        using var content = new MultipartFormDataContent
        {
            { new StreamContent(stream), "file", "employees.csv" }
        };

        request.Content = content;

        var response = await Client.SendAsync(request);

        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Accepted);

            await Task.Delay(TimeSpan.FromSeconds(10));

            var context = new PersistenceContext();
            var addedEmployee = await context.Employees.SingleOrDefaultAsync(employee => employee.EmployeeID == _employee.EmployeeID);

            addedEmployee.Should().NotBeNull()
                .And.Subject.As<EmployeeDataModel>().Should().BeEquivalentTo(new EmployeeDataModel
                {
                    EmployeeID = _employee.EmployeeID,
                    FirstName = _employee.FirstName,
                    LastName = _employee.LastName,
                    BirthDate = _employee.DateOfBirth.ToDateTime(TimeOnly.MinValue),
                    AnnualIncome = _expectedAnnualIncome
                });
        }
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        var context = new PersistenceContext();

        var addedEmployee = await context.Employees.SingleOrDefaultAsync(employee => employee.EmployeeID == _employee.EmployeeID);

        if (addedEmployee is not null)
        {
            context.Employees.Remove(addedEmployee);
            await context.SaveChangesAsync();
        }
    }

    private static async Task<Stream> GenerateFile(params EmployeeContract[] employees)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);

        await writer.WriteLineAsync("EmployeeID,FirstName,LastName,DateOfBirth,GrossAnnualSalary");

        foreach (var employee in employees)
        {
            await writer.WriteLineAsync($"{employee.EmployeeID},{employee.FirstName},{employee.LastName},{employee.DateOfBirth},{employee.GrossAnnualSalary}");
        }

        await writer.FlushAsync();
        stream.Position = 0;

        return stream;
    }
}