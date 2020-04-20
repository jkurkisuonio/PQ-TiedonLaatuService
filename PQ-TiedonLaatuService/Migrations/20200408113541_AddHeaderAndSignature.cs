using Microsoft.EntityFrameworkCore.Migrations;

namespace PQ_TiedonLaatuService.Migrations
{
    public partial class AddHeaderAndSignature : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AlertMsgHeader",
                table: "AlertType",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AlertMsgSignature",
                table: "AlertType",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AlertMsgHeader",
                table: "AlertType");

            migrationBuilder.DropColumn(
                name: "AlertMsgSignature",
                table: "AlertType");
        }
    }
}
