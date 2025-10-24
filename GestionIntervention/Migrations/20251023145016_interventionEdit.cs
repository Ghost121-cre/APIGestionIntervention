using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionIntervention.Migrations
{
    /// <inheritdoc />
    public partial class interventionEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateDebut",
                table: "Interventions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateFin",
                table: "Interventions",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateDebut",
                table: "Interventions");

            migrationBuilder.DropColumn(
                name: "DateFin",
                table: "Interventions");
        }
    }
}
