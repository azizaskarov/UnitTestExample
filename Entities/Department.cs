namespace UnitTestExample.Entities;

public class Department
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public IEnumerable<Employee>? Employees { get; set; }
}
