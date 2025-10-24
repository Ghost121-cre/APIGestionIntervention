using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionIntervention.Migrations
{
    /// <inheritdoc />
    public partial class editUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "Utilisateurs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Utilisateurs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pays",
                table: "Utilisateurs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ville",
                table: "Utilisateurs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "Pays",
                table: "Utilisateurs");

            migrationBuilder.DropColumn(
                name: "Ville",
                table: "Utilisateurs");
        }
    }
}
