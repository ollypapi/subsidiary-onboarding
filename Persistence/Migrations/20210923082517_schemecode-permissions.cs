using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class schemecodepermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SchemeCodeRestriction",
                table: "Settings");

            migrationBuilder.CreateTable(
                name: "SettingAccess",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Permission = table.Column<string>(nullable: false),
                    IsPermitted = table.Column<bool>(nullable: false),
                    SettingId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SettingAccess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SettingAccess_Settings_SettingId",
                        column: x => x.SettingId,
                        principalTable: "Settings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SettingAccess_SettingId",
                table: "SettingAccess",
                column: "SettingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SettingAccess");

            migrationBuilder.AddColumn<string>(
                name: "SchemeCodeRestriction",
                table: "Settings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
