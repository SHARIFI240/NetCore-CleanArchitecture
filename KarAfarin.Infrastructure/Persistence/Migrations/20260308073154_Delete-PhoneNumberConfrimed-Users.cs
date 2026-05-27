using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarAfarin.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DeletePhoneNumberConfrimedUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhoneNumberConfrimed",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PhoneNumberConfrimed",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
