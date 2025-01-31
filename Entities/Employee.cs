using System.ComponentModel.DataAnnotations;

namespace UnitTestExample.Entities;

public class Employee
{
    public int Id { get; set; }

    [MaxLength(255)]
    public required string Name { get; set; }

    public int DepartmentId { get; set; }
    public Department? Department { get; set; }
}
