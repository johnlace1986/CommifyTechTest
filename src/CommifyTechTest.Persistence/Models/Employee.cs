using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace CommifyTechTest.Persistence.Models;
internal class Employee
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int EmployeeID { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    [Column(TypeName = "DATE")]
    public DateTime BirthDate { get; set; }

    [NotNull]
    public decimal? AnnualIncome { get; set; }
}
