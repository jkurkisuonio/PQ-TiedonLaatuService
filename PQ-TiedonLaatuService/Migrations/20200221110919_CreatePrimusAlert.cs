using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PQ_TiedonLaatuService.Migrations
{
    public partial class CreatePrimusAlert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AlertType",
                columns: table => new
                {
                    AlertTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardNumber = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    QueryString = table.Column<string>(nullable: true),
                    AlertMsgText = table.Column<string>(nullable: true),
                    AlertMsgSubject = table.Column<string>(nullable: true),
                    IsInUse = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlertType", x => x.AlertTypeId);
                });

            migrationBuilder.CreateTable(
                name: "PrimusAlert",
                columns: table => new
                {
                    PrimusAlertID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CardNumber = table.Column<string>(nullable: true),
                    SentDate = table.Column<DateTime>(nullable: false),                    
                    AlertTypeId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimusAlert", x => x.PrimusAlertID);
                    table.ForeignKey(
                        name: "FK_PrimusAlert_AlertType_AlertTypeId",
                        column: x => x.AlertTypeId,
                        principalTable: "AlertType",
                        principalColumn: "AlertTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrimusAlert_AlertTypeId",
                table: "PrimusAlert",
                column: "AlertTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrimusAlert");

            migrationBuilder.DropTable(
                name: "AlertType");
        }
    }
}
