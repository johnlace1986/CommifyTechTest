namespace CommifyTechTest.Domain.AggregateRoots;

public class Employee
{
    public int Id { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public DateOnly DateOfBirth { get; }

    public decimal? NetAnnualSalary { get; set; }

    public Employee(
        int id,
        string firstName,
        string lastName,
        DateOnly dateOfBirth,
        decimal? netAnnualSalary = null)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        NetAnnualSalary = netAnnualSalary;
    }

    public void CalculateNetAnnualSalary(decimal grossAnnualSalary, IEnumerable<TaxBand> taxBands)
    {
        var tax = 0M;
        var workingSalary = grossAnnualSalary;

        foreach (var taxBand in taxBands.OrderByDescending(taxBand => taxBand.LowerLimit))
        {
            if (workingSalary > taxBand.LowerLimit)
            {
                var taxable = workingSalary - taxBand.LowerLimit;
                tax += taxable * (taxBand.TaxRate / 100M);

                workingSalary = taxBand.LowerLimit;
            }
        }

        NetAnnualSalary = grossAnnualSalary - tax;
    }
}
