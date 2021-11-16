using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class schemeCodepermission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SchemeCodeSettingPermissions_SchemeCodes_SchemeCodeId",
                table: "SchemeCodeSettingPermissions");

            migrationBuilder.AlterColumn<long>(
                name: "SchemeCodeId",
                table: "SchemeCodeSettingPermissions",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

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

            migrationBuilder.AlterColumn<long>(
                name: "SchemeCodeId",
                table: "SchemeCodeSettingPermissions",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SchemeCodeSettingPermissions_SchemeCodes_SchemeCodeId",
                table: "SchemeCodeSettingPermissions",
                column: "SchemeCodeId",
                principalTable: "SchemeCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
