using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrudOpeartion.Migrations
{
    public partial class newMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategotyId",
                table: "Games");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategotyId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
