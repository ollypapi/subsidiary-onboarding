using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class schemeCodeupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "SchemeCodeId",
                table: "SchemeCodeSettingPermissions",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SchemeCodes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    CountryCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchemeCodes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SchemeCodeSettingPermissions_SchemeCodeId",
                table: "SchemeCodeSettingPermissions",
                column: "SchemeCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchemeCodeSettingPermissions_SchemeCodes_SchemeCodeId",
                table: "SchemeCodeSettingPermissions",
                column: "SchemeCodeId",
                principalTable: "SchemeCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchemeCodeSettingPermissions_SchemeCodes_SchemeCodeId",
                table: "SchemeCodeSettingPermissions");

            migrationBuilder.DropTable(
                name: "SchemeCodes");

            migrationBuilder.DropIndex(
                name: "IX_SchemeCodeSettingPermissions_SchemeCodeId",
                table: "SchemeCodeSettingPermissions");

            migrationBuilder.DropColumn(
                name: "SchemeCodeId",
                table: "SchemeCodeSettingPermissions");
        }
    }
}
