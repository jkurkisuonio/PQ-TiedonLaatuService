using Microsoft.EntityFrameworkCore.Migrations;

namespace PQ_TiedonLaatuService.Migrations
{
    public partial class Again : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrimusAlert_AlertReceiver_AlertReceiverId",
                table: "PrimusAlert");

            migrationBuilder.DropForeignKey(
                name: "FK_PrimusAlert_AlertType_AlertTypeId",
                table: "PrimusAlert");

            migrationBuilder.AlterColumn<int>(
                name: "AlertTypeId",
                table: "PrimusAlert",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AlertReceiverId",
                table: "PrimusAlert",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PrimusAlert_AlertReceiver_AlertReceiverId",
                table: "PrimusAlert",
                column: "AlertReceiverId",
                principalTable: "AlertReceiver",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrimusAlert_AlertType_AlertTypeId",
                table: "PrimusAlert",
                column: "AlertTypeId",
                principalTable: "AlertType",
                principalColumn: "Id",
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

            migrationBuilder.AlterColumn<int>(
                name: "AlertTypeId",
                table: "PrimusAlert",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "AlertReceiverId",
                table: "PrimusAlert",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_PrimusAlert_AlertReceiver_AlertReceiverId",
                table: "PrimusAlert",
                column: "AlertReceiverId",
                principalTable: "AlertReceiver",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrimusAlert_AlertType_AlertTypeId",
                table: "PrimusAlert",
                column: "AlertTypeId",
                principalTable: "AlertType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
