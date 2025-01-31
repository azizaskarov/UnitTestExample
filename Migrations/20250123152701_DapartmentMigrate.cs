using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UnitTestExample.Migrations
{
    /// <inheritdoc />
    public partial class DapartmentMigrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Departments jadvalini yaratish
            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            // Employees jadvaliga DepartmentId ustuni qo‘shish
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Departments jadvaliga unikal department nomlarini qo‘shish
            migrationBuilder.Sql(@"
        INSERT INTO [Departments] ([Name])
        SELECT DISTINCT [Department] FROM [Employees]");

            // Employees jadvalidagi DepartmentId ustunini yangilash
            migrationBuilder.Sql(@"
        UPDATE [Employees]
        SET [DepartmentId] = d.[Id]
        FROM [Departments] d
        WHERE [Employees].[Department] = d.[Name]");

            // Employees jadvalidagi DepartmentId ustuniga indeks yaratish
            migrationBuilder.CreateIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees",
                column: "DepartmentId");

            // Departments jadvalidagi Name ustuniga unikal indeks yaratish
            migrationBuilder.CreateIndex(
                name: "IX_Departments_Name",
                table: "Departments",
                column: "Name",
                unique: true);

            // Employees jadvaliga foreign key qo‘shish
            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                table: "Employees",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            // Employees jadvalidagi eski Department ustunini o‘chirish
            migrationBuilder.DropColumn(
                name: "Department",
                table: "Employees");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Departments_DepartmentId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Employees_DepartmentId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Employees");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "Employees",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

        }
    }
}
