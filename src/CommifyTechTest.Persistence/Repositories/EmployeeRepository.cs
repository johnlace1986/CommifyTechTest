using CommifyTechTest.Application.Infrastructure.Repositories;
using CommifyTechTest.Domain.AggregateRoots;
using CommifyTechTest.Persistence.Mappers;
using Microsoft.EntityFrameworkCore;

namespace CommifyTechTest.Persistence.Repositories;

internal class EmployeeRepository : IEmployeeRepository
{
    private readonly PersistenceContext _context;

    public EmployeeRepository()
    {
        _context = new PersistenceContext();
    }

    public async Task<Employee> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var employee = await _context
            .Employees
            .SingleOrDefaultAsync(employee => employee.EmployeeID == id, cancellationToken);

        return EmployeeMapper.Map(employee);
    }

    public async Task AddEmployeeAsync(Employee employee, CancellationToken cancellationToken)
    {
        await _context.Employees.AddAsync(EmployeeMapper.Map(employee), cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
