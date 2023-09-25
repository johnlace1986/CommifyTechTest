using CommifyTechTest.Domain.AggregateRoots;
using FluentAssertions;
using NUnit.Framework;

namespace CommifyTechTest.Domain.Tests.AggregateRoots;

public class EmployeeTests
{
    private readonly IEnumerable<TaxBand> _taxBands = new[]
    {
        new TaxBand{LowerLimit = 0, TaxRate = 0},
        new TaxBand{LowerLimit = 5000, TaxRate = 20},
        new TaxBand{LowerLimit = 20000, TaxRate = 40}
    };

    [TestCase(10000, 9000)]
    [TestCase(40000, 29000)]
    public void CalculateAnnualIncome(int grossAnnualSalary, int annualIncome)
    {
        var employee = new Employee(1, "John", "Smith", DateOnly.FromDateTime(DateTime.UtcNow));
        employee.CalculateAnnualIncome(grossAnnualSalary, _taxBands);

        employee.AnnualIncome.Should().Be(annualIncome);
    }
}
