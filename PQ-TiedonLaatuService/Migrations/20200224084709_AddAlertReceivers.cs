using Microsoft.EntityFrameworkCore.Migrations;

namespace PQ_TiedonLaatuService.Migrations
{
    public partial class AddAlertReceivers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrimusAlert_AlertType_AlertTypeId",
                table: "PrimusAlert");

            migrationBuilder.RenameColumn(
                name: "PrimusAlertID",
                table: "PrimusAlert",
                newName: "PrimusAlertId");

            migrationBuilder.AlterColumn<int>(
                name: "AlertTypeId",
                table: "PrimusAlert",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AlertReceiverId",
                table: "PrimusAlert",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AlertReceiver",
                columns: table => new
                {
                    AlertReceiverId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: true),
                    CardNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertReceiver", x => x.AlertReceiverId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrimusAlert_AlertReceiverId",
                table: "PrimusAlert",
                column: "AlertReceiverId");

            migrationBuilder.AddForeignKey(
                name: "FK_PrimusAlert_AlertReceiver_AlertReceiverId",
                table: "PrimusAlert",
                column: "AlertReceiverId",
                principalTable: "AlertReceiver",
                principalColumn: "AlertReceiverId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrimusAlert_AlertType_AlertTypeId",
                table: "PrimusAlert",
                column: "AlertTypeId",
                principalTable: "AlertType",
                principalColumn: "AlertTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrimusAlert_AlertReceiver_AlertReceiverId",
                table: "PrimusAlert");

            migrationBuilder.DropForeignKey(
                name: "FK_PrimusAlert_AlertType_AlertTypeId",
                table: "PrimusAlert");

            migrationBuilder.DropTable(
                name: "AlertReceiver");

            migrationBuilder.DropIndex(
                name: "IX_PrimusAlert_AlertReceiverId",
                table: "PrimusAlert");

            migrationBuilder.DropColumn(
                name: "AlertReceiverId",
                table: "PrimusAlert");

            migrationBuilder.RenameColumn(
                name: "PrimusAlertId",
                table: "PrimusAlert",
                newName: "PrimusAlertID");

            migrationBuilder.AlterColumn<int>(
                name: "AlertTypeId",
                table: "PrimusAlert",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_PrimusAlert_AlertType_AlertTypeId",
                table: "PrimusAlert",
                column: "AlertTypeId",
                principalTable: "AlertType",
                principalColumn: "AlertTypeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
