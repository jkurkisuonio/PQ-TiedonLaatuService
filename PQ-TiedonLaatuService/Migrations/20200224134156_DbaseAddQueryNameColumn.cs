using Microsoft.EntityFrameworkCore.Migrations;

namespace PQ_TiedonLaatuService.Migrations
{
    public partial class DbaseAddQueryNameColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QueryName",
                table: "AlertType",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QueryName",
                table: "AlertType");
        }
    }
}
