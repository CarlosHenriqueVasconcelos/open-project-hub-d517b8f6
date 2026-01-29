using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using PlataformaGestaoIA.DataContext;

#nullable disable

namespace plataformagestaoiabe.Migrations
{
    [DbContext(typeof(PrincipalDataContext))]
    [Migration("20260115140000_AddConfirmationPhase2Deadline")]
    public partial class AddConfirmationPhase2Deadline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "config_confirmation_deadline_phase2",
                table: "GeneralConfig",
                type: "DATETIME",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "config_confirmation_deadline_phase2",
                table: "GeneralConfig");
        }
    }
}
