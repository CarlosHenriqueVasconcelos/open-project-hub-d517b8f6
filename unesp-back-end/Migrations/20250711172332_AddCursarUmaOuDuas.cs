using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace plataformagestaoiabe.Migrations
{
    /// <inheritdoc />
    public partial class AddCursarUmaOuDuas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentRegistration_StudentRegistrationScores_StudentRegistr~",
                table: "StudentRegistration");

            migrationBuilder.AlterColumn<int>(
                name: "id_skill",
                table: "StudentRegistrationSkill",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "skills_description",
                table: "StudentRegistration",
                type: "NVARCHAR(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(100)");

            migrationBuilder.AlterColumn<int>(
                name: "id_student",
                table: "StudentRegistration",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "file_path",
                table: "StudentRegistration",
                type: "VARCHAR(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(200)");

            migrationBuilder.AlterColumn<int>(
                name: "StudentRegistrationScoreId",
                table: "StudentRegistration",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<short>(
                name: "cursar_uma_ou_duas",
                table: "StudentRegistration",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AlterColumn<string>(
                name: "rg",
                table: "Student",
                type: "VARCHAR(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(15)",
                oldMaxLength: 15);

            migrationBuilder.AlterColumn<string>(
                name: "cpf",
                table: "Student",
                type: "VARCHAR(11)",
                maxLength: 11,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(11)",
                oldMaxLength: 11);

            migrationBuilder.AlterColumn<string>(
                name: "cellphone",
                table: "Student",
                type: "VARCHAR(15)",
                maxLength: 15,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(15)",
                oldMaxLength: 15);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentRegistration_StudentRegistrationScores_StudentRegistr~",
                table: "StudentRegistration",
                column: "StudentRegistrationScoreId",
                principalTable: "StudentRegistrationScores",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentRegistration_StudentRegistrationScores_StudentRegistr~",
                table: "StudentRegistration");

            migrationBuilder.DropColumn(
                name: "cursar_uma_ou_duas",
                table: "StudentRegistration");

            migrationBuilder.AlterColumn<int>(
                name: "id_skill",
                table: "StudentRegistrationSkill",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "skills_description",
                table: "StudentRegistration",
                type: "NVARCHAR(100)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id_student",
                table: "StudentRegistration",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "file_path",
                table: "StudentRegistration",
                type: "VARCHAR(200)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StudentRegistrationScoreId",
                table: "StudentRegistration",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "rg",
                table: "Student",
                type: "VARCHAR(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "cpf",
                table: "Student",
                type: "VARCHAR(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(11)",
                oldMaxLength: 11,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "cellphone",
                table: "Student",
                type: "VARCHAR(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "VARCHAR(15)",
                oldMaxLength: 15,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentRegistration_StudentRegistrationScores_StudentRegistr~",
                table: "StudentRegistration",
                column: "StudentRegistrationScoreId",
                principalTable: "StudentRegistrationScores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
