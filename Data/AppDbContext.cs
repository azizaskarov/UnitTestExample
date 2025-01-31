using Microsoft.EntityFrameworkCore;
using UnitTestExample.Entities;

namespace UnitTestExample.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<TelegramUser> TelegramUsers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Department> (entity =>
        {
            entity.HasIndex(d => d.Name).IsUnique();

        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }

}
