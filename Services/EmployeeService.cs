using Microsoft.EntityFrameworkCore;
using UnitTestExample.Data;
using UnitTestExample.Entities;
using UnitTestExample.Interfaces.ServiceLifeTimes;

namespace UnitTestExample.Services;

public class EmployeeService(AppDbContext dbContext) : IEmployeeService, IScopedService
{

    public  Task<Employee?> GetByIdEmployeeAsync(int id) => Task.FromResult(dbContext.Employees.Include(e => e.Department).FirstOrDefault(e => e.Id == id));
    public  Task<int> AddSalaryAndBonusAsync(int salary,int bonus) => Task.FromResult(bonus+salary);
}


public interface IEmployeeService
{
     Task<Employee?> GetByIdEmployeeAsync(int id);
     Task<int> AddSalaryAndBonusAsync(int salary, int bonus);
}
