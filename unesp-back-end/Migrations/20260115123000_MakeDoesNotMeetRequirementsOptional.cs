using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using PlataformaGestaoIA.DataContext;

#nullable disable

namespace plataformagestaoiabe.Migrations
{
    [DbContext(typeof(PrincipalDataContext))]
    [Migration("20260115123000_MakeDoesNotMeetRequirementsOptional")]
    public partial class MakeDoesNotMeetRequirementsOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "does_not_meet_requirements",
                table: "StudentRegistration",
                type: "SMALLINT",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "SMALLINT");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<short>(
                name: "does_not_meet_requirements",
                table: "StudentRegistration",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0,
                oldClrType: typeof(short),
                oldType: "SMALLINT",
                oldNullable: true);
        }
    }
}
