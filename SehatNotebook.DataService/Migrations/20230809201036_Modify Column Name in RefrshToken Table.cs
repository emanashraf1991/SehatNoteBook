using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SehatNotebook.DataService.Migrations
{
    /// <inheritdoc />
    public partial class ModifyColumnNameinRefrshTokenTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tokem",
                table: "RefreshTokens",
                newName: "Token");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Token",
                table: "RefreshTokens",
                newName: "Tokem");
        }
    }
}
