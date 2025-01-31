using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UnitTestExample.Entities;
using UnitTestExample.Services;

namespace UnitTestExample.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IEmployeeService employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        this.employeeService = employeeService;
    }

    [HttpGet("{id}")]
    public async  Task<IActionResult> GetById(int id)
    {
        var employee = await employeeService.GetByIdEmployeeAsync(id);
        if (employee is not null)
            return Ok(employee);
        else
            return NotFound($"Employee not found with id: {id}");
    }

    [HttpPost]
    public  Task<int> AddSalaryAndBonus(int salary, int bonus)
    {
        return employeeService.AddSalaryAndBonusAsync(salary, bonus);
    }
}
