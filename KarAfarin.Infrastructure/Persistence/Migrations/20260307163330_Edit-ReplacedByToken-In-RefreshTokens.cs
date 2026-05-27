using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarAfarin.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditReplacedByTokenInRefreshTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastToken",
                table: "RefreshTokens",
                newName: "ReplacedByToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReplacedByToken",
                table: "RefreshTokens",
                newName: "LastToken");
        }
    }
}
