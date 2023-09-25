using CommifyTechTest.Application.Commands;
using CommifyTechTest.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CommifyTechTest.Controllers;

public class EmployeesController : Controller
{
    private readonly IEmployeesParser _employeesParser;
    private readonly IMediator _mediator;

    public EmployeesController(
        IEmployeesParser employeesParser,
        IMediator mediator)
    {
        _employeesParser = employeesParser;
        _mediator = mediator;
    }

    [HttpPost]
    [Route("[controller]/file")]
    public async Task<IActionResult> AddEmployeesAsync([FromForm(Name = "file")] IFormFile file, CancellationToken cancellationToken)
    {
        IEnumerable<IEmployeesParser.Employee> employees;

        try
        {
            using var stream = file.OpenReadStream();
            employees = await _employeesParser.ParseAsync(stream, cancellationToken);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return BadRequest();
        }

        await Task.WhenAll(employees.Select(employee =>
        {
            return _mediator.Send(CreatedAddEmployeeCommand(employee), cancellationToken);
        }));

        return Ok();
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
