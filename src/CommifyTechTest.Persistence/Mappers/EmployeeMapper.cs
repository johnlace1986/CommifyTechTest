using DomainEmployee = CommifyTechTest.Domain.AggregateRoots.Employee;
using PersistenceEmployee = CommifyTechTest.Persistence.Models.Employee;

namespace CommifyTechTest.Persistence.Mappers;

internal static class EmployeeMapper
{
    public static DomainEmployee Map(PersistenceEmployee employee)
    {
        if (employee is null)
            return null;

        return new DomainEmployee(
            id: employee.EmployeeID,
            firstName: employee.FirstName,
            lastName: employee.LastName,
            dateOfBirth: DateOnly.FromDateTime(employee.BirthDate),
            annualIncome: employee.AnnualIncome);
    }

    public static PersistenceEmployee Map(DomainEmployee employee)
    {
        if (employee is null)
            return null;

        return new PersistenceEmployee
        {
            EmployeeID = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            BirthDate = employee.DateOfBirth.ToDateTime(TimeOnly.MinValue),
            AnnualIncome = employee.AnnualIncome
        };
    }
}
