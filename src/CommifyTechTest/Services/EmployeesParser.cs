namespace CommifyTechTest.Services;

public class EmployeesParser : IEmployeesParser
{
    public async Task<IEnumerable<IEmployeesParser.Employee>> ParseAsync(Stream stream, CancellationToken cancellationToken)
    {
        using var streamReader = new StreamReader(stream);

        //header row
        await streamReader.ReadLineAsync();

        var employees = new List<IEmployeesParser.Employee>();

        while (streamReader.EndOfStream is false)
        {
            var line = await streamReader.ReadLineAsync();
            var parts = line.Split(',');

            employees.Add(new IEmployeesParser.Employee
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
