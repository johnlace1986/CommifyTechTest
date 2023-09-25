using CommifyTechTest.Domain.AggregateRoots;

namespace CommifyTechTest.Application.Infrastructure.Repositories;

public interface IEmployeeRepository
{
    Task<Employee> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken);
}
