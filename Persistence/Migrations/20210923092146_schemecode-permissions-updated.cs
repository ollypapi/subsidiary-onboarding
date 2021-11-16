using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class schemecodepermissionsupdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SettingAccess_Settings_SettingId",
                table: "SettingAccess");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SettingAccess",
                table: "SettingAccess");

            migrationBuilder.DropIndex(
                name: "IX_SettingAccess_SettingId",
                table: "SettingAccess");

            migrationBuilder.RenameTable(
                name: "SettingAccess",
                newName: "SchemeCodeSettingPermissions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SchemeCodeSettingPermissions",
                table: "SchemeCodeSettingPermissions",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SchemeCodeSettingPermissions",
                table: "SchemeCodeSettingPermissions");

            migrationBuilder.RenameTable(
                name: "SchemeCodeSettingPermissions",
                newName: "SettingAccess");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SettingAccess",
                table: "SettingAccess",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SettingAccess_SettingId",
                table: "SettingAccess",
                column: "SettingId");

            migrationBuilder.AddForeignKey(
                name: "FK_SettingAccess_Settings_SettingId",
                table: "SettingAccess",
                column: "SettingId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
