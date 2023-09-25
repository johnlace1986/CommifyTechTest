using System.ComponentModel.DataAnnotations.Schema;

namespace CommifyTechTest.Persistence.Models;
internal class Employee
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int EmployeeID { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    [Column(TypeName = "DATE")]
    public DateTime BirthDate { get; set; }

    public decimal AnnualIncome { get; set; }
}
