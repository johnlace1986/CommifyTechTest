using CommifyTechTest.Contracts;

namespace CommifyTechTest.Services;

public class EmployeesParser : IEmployeesParser
{
    public async Task<IEnumerable<Employee>> ParseAsync(Stream stream, CancellationToken cancellationToken)
    {
        using var streamReader = new StreamReader(stream);

        //header row
        await streamReader.ReadLineAsync();

        var employees = new List<Employee>();

        while (streamReader.EndOfStream is false)
        {
            var line = await streamReader.ReadLineAsync();
            var parts = line.Split(',');

            employees.Add(new Employee
            {
                EmployeeID = int.Parse(parts[0]),
                FirstName = parts[1],
                LastName = parts[2],
                DateOfBirth = DateOnly.Parse(parts[3]),
                GrossAnnualSalary = int.Parse(parts[4])
            });
        }

        return employees;
    }
}
