using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KoperasiTentera.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class useBiometric : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFaceIdEnabled",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "IsFingerprintEnabled",
                table: "Users",
                newName: "BiometricEnabled");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BiometricEnabled",
                table: "Users",
                newName: "IsFingerprintEnabled");

            migrationBuilder.AddColumn<bool>(
                name: "IsFaceIdEnabled",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
