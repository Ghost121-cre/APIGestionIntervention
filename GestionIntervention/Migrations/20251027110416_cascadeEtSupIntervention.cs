using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionIntervention.Migrations
{
    /// <inheritdoc />
    public partial class cascadeEtSupIntervention : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Produits_ProduitId",
                table: "Incidents");

            migrationBuilder.DropForeignKey(
                name: "FK_StatutInterventions_Utilisateurs_UtilisateurId",
                table: "StatutInterventions");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Produits_ProduitId",
                table: "Incidents",
                column: "ProduitId",
                principalTable: "Produits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StatutInterventions_Utilisateurs_UtilisateurId",
                table: "StatutInterventions",
                column: "UtilisateurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidents_Produits_ProduitId",
                table: "Incidents");

            migrationBuilder.DropForeignKey(
                name: "FK_StatutInterventions_Utilisateurs_UtilisateurId",
                table: "StatutInterventions");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidents_Produits_ProduitId",
                table: "Incidents",
                column: "ProduitId",
                principalTable: "Produits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StatutInterventions_Utilisateurs_UtilisateurId",
                table: "StatutInterventions",
                column: "UtilisateurId",
                principalTable: "Utilisateurs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
