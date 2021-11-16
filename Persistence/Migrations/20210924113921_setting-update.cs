using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class settingupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SchemeCodeSettingPermissions_SettingId",
                table: "SchemeCodeSettingPermissions",
                column: "SettingId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchemeCodeSettingPermissions_Settings_SettingId",
                table: "SchemeCodeSettingPermissions",
                column: "SettingId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchemeCodeSettingPermissions_Settings_SettingId",
                table: "SchemeCodeSettingPermissions");

            migrationBuilder.DropIndex(
                name: "IX_SchemeCodeSettingPermissions_SettingId",
                table: "SchemeCodeSettingPermissions");
        }
    }
}
