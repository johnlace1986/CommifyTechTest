using CommifyTechTest.Contracts;

namespace CommifyTechTest.Services;

public interface IEmployeesParser
{
    Task<IEnumerable<Employee>> ParseAsync(Stream stream, CancellationToken cancellationToken);
}