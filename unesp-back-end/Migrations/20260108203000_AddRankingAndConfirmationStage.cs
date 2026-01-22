using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MySql.EntityFrameworkCore.Metadata;
using PlataformaGestaoIA.DataContext;

#nullable disable

namespace plataformagestaoiabe.Migrations
{
    [DbContext(typeof(PrincipalDataContext))]
    [Migration("20260108203000_AddRankingAndConfirmationStage")]
    public partial class AddRankingAndConfirmationStage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "config_confirmation_deadline",
                table: "GeneralConfig",
                type: "DATETIME",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "config_stage",
                table: "GeneralConfig",
                type: "VARCHAR(20)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StudentRegistrationRanking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    StudentRegistrationId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    semester = table.Column<string>(type: "VARCHAR(7)", nullable: false),
                    subjectvalue = table.Column<short>(name: "subject_value", type: "SMALLINT", nullable: false),
                    totalscore = table.Column<float>(name: "total_score", type: "FLOAT", nullable: false),
                    performancecoefficient = table.Column<float>(name: "performance_coefficient", type: "FLOAT", nullable: false),
                    rankposition = table.Column<int>(name: "rank_position", type: "INT", nullable: false),
                    classification = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    status = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    confirmby = table.Column<DateTime>(name: "confirm_by", type: "DATETIME", nullable: true),
                    statusupdatedat = table.Column<DateTime>(name: "status_updated_at", type: "DATETIME", nullable: true),
                    registrationdate = table.Column<DateTime>(name: "registration_date", type: "DATETIME", nullable: true),
                    createdat = table.Column<DateTime>(name: "created_at", type: "DATETIME", nullable: true),
                    updatedat = table.Column<DateTime>(name: "updated_at", type: "DATETIME", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentRegistrationRanking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ranking_Student",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ranking_StudentRegistration",
                        column: x => x.StudentRegistrationId,
                        principalTable: "StudentRegistration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRegistrationRanking_StudentId",
                table: "StudentRegistrationRanking",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRegistrationRanking_StudentRegistrationId",
                table: "StudentRegistrationRanking",
                column: "StudentRegistrationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentRegistrationRanking");

            migrationBuilder.DropColumn(
                name: "config_confirmation_deadline",
                table: "GeneralConfig");

            migrationBuilder.DropColumn(
                name: "config_stage",
                table: "GeneralConfig");
        }
    }
}
