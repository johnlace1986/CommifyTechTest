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

        await TriggerJob(employees);

        return Accepted();
    }

    private async Task TriggerJob(IEnumerable<IEmployeesParser.Employee> employees)
    {
        var jobKey = new JobKey(AddEmployeesJob.GenerateJobKey(), AddEmployeesJob.GroupKey);

        var job = JobBuilder
            .Create<AddEmployeesJob>()
            .WithIdentity(jobKey)
            .SetJobData(new JobDataMap
            {
                { "employees", employees }
            })
            .Build();

        await _scheduler.AddJob(job, replace: true, storeNonDurableWhileAwaitingScheduling: true);
        await _scheduler.TriggerJob(jobKey);
    }
}
