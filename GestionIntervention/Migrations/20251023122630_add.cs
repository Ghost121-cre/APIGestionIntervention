using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionIntervention.Migrations
{
    /// <inheritdoc />
    public partial class add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateUpload",
                table: "IncidentImages",
                newName: "DateCreation");

            migrationBuilder.AddColumn<string>(
                name: "AltText",
                table: "IncidentImages",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AltText",
                table: "IncidentImages");

            migrationBuilder.RenameColumn(
                name: "DateCreation",
                table: "IncidentImages",
                newName: "DateUpload");
        }
    }
}
