using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class schemeCodeupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AlterColumn<long>(
                name: "SchemeCodeId",
                table: "SchemeCodeSettingPermissions",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SchemeCodeSettingPermissions_SchemeCodes_SchemeCodeId",
                table: "SchemeCodeSettingPermissions",
                column: "SchemeCodeId",
                principalTable: "SchemeCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AlterColumn<long>(
                name: "SchemeCodeId",
                table: "SchemeCodeSettingPermissions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddColumn<long>(
                name: "SettingId",
                table: "SchemeCodeSettingPermissions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_SchemeCodeSettingPermissions_SettingId",
                table: "SchemeCodeSettingPermissions",
                column: "SettingId");

            migrationBuilder.AddForeignKey(
                name: "FK_SchemeCodeSettingPermissions_SchemeCodes_SchemeCodeId",
                table: "SchemeCodeSettingPermissions",
                column: "SchemeCodeId",
                principalTable: "SchemeCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SchemeCodeSettingPermissions_Settings_SettingId",
                table: "SchemeCodeSettingPermissions",
                column: "SettingId",
                principalTable: "Settings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
