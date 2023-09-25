using CommifyTechTest.Services;
using Microsoft.AspNetCore.Mvc;

namespace CommifyTechTest.Controllers;

public class EmployeesController : Controller
{
    private readonly IEmployeesParser _employeesParser;

    public EmployeesController(
        IEmployeesParser employeesParser)
    {
        _employeesParser = employeesParser;
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

        return Ok();
    }
}
