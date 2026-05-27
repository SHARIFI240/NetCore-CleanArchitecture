using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarAfarin.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditLastLoginDateInUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LasLoginDate",
                table: "Users",
                newName: "LastLoginDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastLoginDate",
                table: "Users",
                newName: "LasLoginDate");
        }
    }
}
