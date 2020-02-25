using Microsoft.EntityFrameworkCore.Migrations;

namespace PQ_TiedonLaatuService.Migrations
{
    public partial class ChangeColumnNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrimusAlert_AlertReceiver_AlertReceiverId",
                table: "PrimusAlert");

            migrationBuilder.DropForeignKey(
                name: "FK_PrimusAlert_AlertType_AlertTypeId",
                table: "PrimusAlert");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrimusAlert",
                table: "PrimusAlert");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AlertType",
                table: "AlertType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AlertReceiver",
                table: "AlertReceiver");

            migrationBuilder.DropColumn(
                name: "PrimusAlertId",
                table: "PrimusAlert");

            migrationBuilder.DropColumn(
                name: "AlertTypeId",
                table: "AlertType");

            migrationBuilder.DropColumn(
                name: "AlertReceiverId",
                table: "AlertReceiver");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PrimusAlert",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AlertType",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AlertReceiver",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrimusAlert",
                table: "PrimusAlert",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlertType",
                table: "AlertType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlertReceiver",
                table: "AlertReceiver",
                column: "Id");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_PrimusAlert",
                table: "PrimusAlert");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AlertType",
                table: "AlertType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AlertReceiver",
                table: "AlertReceiver");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PrimusAlert");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AlertType");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AlertReceiver");

            migrationBuilder.AddColumn<int>(
                name: "PrimusAlertId",
                table: "PrimusAlert",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "AlertTypeId",
                table: "AlertType",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "AlertReceiverId",
                table: "AlertReceiver",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrimusAlert",
                table: "PrimusAlert",
                column: "PrimusAlertId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlertType",
                table: "AlertType",
                column: "AlertTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AlertReceiver",
                table: "AlertReceiver",
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
    }
}
