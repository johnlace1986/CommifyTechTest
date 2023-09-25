namespace CommifyTechTest.Domain.AggregateRoots;

public class Employee
{
    public int Id { get; }

    public string FirstName { get; }

    public string LastName { get; }

    public DateOnly DateOfBirth { get; }

    public decimal? AnnualIncome { get; set; }

    public Employee(
        int id,
        string firstName,
        string lastName,
        DateOnly dateOfBirth,
        decimal? annualIncome = null)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        AnnualIncome = annualIncome;
    }

    public void CalculateAnnualIncome(decimal grossAnnualIncome, IEnumerable<TaxBand> taxBands)
    {
        var tax = 0M;
        var workingIncome = grossAnnualIncome;

        foreach (var taxBand in taxBands.OrderByDescending(taxBand => taxBand.LowerLimit))
        {
            if (workingIncome > taxBand.LowerLimit)
            {
                var taxable = workingIncome - taxBand.LowerLimit;
                tax += taxable * (taxBand.TaxRate / 100M);

                workingIncome = taxBand.LowerLimit;
            }
        }

        AnnualIncome = grossAnnualIncome - tax;
    }
}
