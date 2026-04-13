using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TemplateChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Body",
                table: "Emails");

            migrationBuilder.AddColumn<string>(
                name: "TemplateData",
                table: "Emails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TemplateData",
                table: "Emails");

            migrationBuilder.AddColumn<string>(
                name: "Body",
                table: "Emails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
