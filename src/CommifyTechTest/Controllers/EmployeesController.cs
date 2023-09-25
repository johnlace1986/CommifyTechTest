using CommifyTechTest.Contracts;
using CommifyTechTest.Jobs;
using CommifyTechTest.Services;
using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace CommifyTechTest.Controllers;

public class EmployeesController : Controller
{
    private readonly IEmployeesParser _employeesParser;
    private readonly IScheduler _scheduler;

    public EmployeesController(
        IEmployeesParser employeesParser,
        IScheduler scheduler)
    {
        _employeesParser = employeesParser;
        _scheduler = scheduler;
    }

    [HttpPost]
    [Route("[controller]")]
    public async Task<IActionResult> LoadFromBodyAsync([FromBody] IEnumerable<Employee> employees, CancellationToken cancellationToken)
    {
        await TriggerJobs(employees);

        return Accepted();
    }

    [HttpPost]
    [Route("[controller]/file")]
    public async Task<IActionResult> LoadFromFormDataAsync([FromForm(Name = "file")] IFormFile file, CancellationToken cancellationToken)
    {
        IEnumerable<Employee> employees;

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

        await TriggerJobs(employees);

        return Accepted();
    }

    private async Task TriggerJobs(IEnumerable<Employee> employees)
    {
        foreach (var employee in employees)
        {
            var jobKey = new JobKey(AddEmployeeJob.GenerateJobKey(employee.EmployeeID), AddEmployeeJob.GroupKey);

            var job = JobBuilder
                .Create<AddEmployeeJob>()
                .WithIdentity(jobKey)
                .SetJobData(new JobDataMap
                {
                    ["employee"] = employee
                })
                .Build();

            await _scheduler.AddJob(job, replace: true, storeNonDurableWhileAwaitingScheduling: true);
            await _scheduler.TriggerJob(jobKey);
        }
    }
}
