using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class schemeCodepermissions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchemeCodeSettingPermissions_SchemeCodes_SchemeCodeId",
                table: "SchemeCodeSettingPermissions");

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
            migrationBuilder.DropForeignKey(
                name: "FK_SchemeCodeSettingPermissions_SchemeCodes_SchemeCodeId",
                table: "SchemeCodeSettingPermissions");

            migrationBuilder.AlterColumn<long>(
                name: "SchemeCodeId",
                table: "SchemeCodeSettingPermissions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AddForeignKey(
                name: "FK_SchemeCodeSettingPermissions_SchemeCodes_SchemeCodeId",
                table: "SchemeCodeSettingPermissions",
                column: "SchemeCodeId",
                principalTable: "SchemeCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
