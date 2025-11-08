using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionIntervention.Migrations
{
    /// <inheritdoc />
    public partial class InterventionImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateUpload",
                table: "InterventionImages",
                newName: "DateCreation");

            migrationBuilder.AddColumn<string>(
                name: "AltText",
                table: "InterventionImages",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AltText",
                table: "InterventionImages");

            migrationBuilder.RenameColumn(
                name: "DateCreation",
                table: "InterventionImages",
                newName: "DateUpload");
        }
    }
}
