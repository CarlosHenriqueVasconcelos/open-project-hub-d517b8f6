using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace plataformagestaoiabe.Migrations
{
    /// <inheritdoc />
    public partial class CreateMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "NVARCHAR(80)", maxLength: 80, nullable: false),
                    legalname = table.Column<string>(name: "legal_name", type: "NVARCHAR(80)", maxLength: 80, nullable: false),
                    cnpj = table.Column<string>(type: "NVARCHAR(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CourseSubject",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "NVARCHAR(80)", maxLength: 80, nullable: false),
                    code = table.Column<string>(type: "VARCHAR(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseSubject", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CurrentCourse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Mode = table.Column<string>(type: "longtext", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false),
                    Period = table.Column<string>(type: "longtext", nullable: false),
                    Campus = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentCourse", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GeneralConfig",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    configheader = table.Column<string>(name: "config_header", type: "LONGTEXT", nullable: false),
                    configbody = table.Column<string>(name: "config_body", type: "LONGTEXT", nullable: false),
                    configemaildomainavaliable = table.Column<string>(name: "config_email_domain_avaliable", type: "LONGTEXT", nullable: false),
                    configconsent = table.Column<string>(name: "config_consent", type: "LONGTEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralConfig", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    description = table.Column<string>(type: "TEXT", nullable: false),
                    internalcode = table.Column<string>(name: "internal_code", type: "NVARCHAR(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false),
                    Slug = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Skill",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    tag = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: false),
                    issoftskill = table.Column<ulong>(name: "is_soft_skill", type: "BIT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StudentRegistrationScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    performancecoefficient = table.Column<float>(name: "performance_coefficient", type: "FLOAT", nullable: false),
                    scientificinitiationprogramscore = table.Column<int>(name: "scientific_initiation_program_score", type: "INT", nullable: false),
                    institutionalmonitoringprogramscore = table.Column<int>(name: "institutional_monitoring_program_score", type: "INT", nullable: false),
                    juniorenterpriseexperiencescore = table.Column<int>(name: "junior_enterprise_experience_score", type: "INT", nullable: false),
                    projectintechnologicalhotelscore = table.Column<int>(name: "project_in_technological_hotel_score", type: "INT", nullable: false),
                    volunteeringscore = table.Column<int>(name: "volunteering_score", type: "INT", nullable: false),
                    highgradedisciplinescore = table.Column<int>(name: "high_grade_discipline_score", type: "INT", nullable: false),
                    certificationcoursesscore = table.Column<int>(name: "certification_courses_score", type: "INT", nullable: false),
                    highgradecoursesscore = table.Column<int>(name: "high_grade_courses_score", type: "INT", nullable: false),
                    aiprojectsscore = table.Column<int>(name: "ai_projects_score", type: "INT", nullable: false),
                    internshipemploymentscore = table.Column<int>(name: "internship_employment_score", type: "INT", nullable: false),
                    technologycertificationscore = table.Column<int>(name: "technology_certification_score", type: "INT", nullable: false),
                    lowleveltechscore = table.Column<int>(name: "low_level_tech_score", type: "INT", nullable: false),
                    scorecoursesdescription = table.Column<string>(name: "score_courses_description", type: "NVARCHAR(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentRegistrationScores", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "NVARCHAR(80)", maxLength: 80, nullable: true),
                    email = table.Column<string>(type: "VARCHAR(160)", maxLength: 160, nullable: false),
                    passwordhash = table.Column<string>(name: "password_hash", type: "VARCHAR(255)", maxLength: 255, nullable: true),
                    Image = table.Column<string>(type: "longtext", nullable: true),
                    slug = table.Column<string>(type: "VARCHAR(80)", maxLength: 80, nullable: true),
                    Bio = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProjectSkill",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    SkillId = table.Column<int>(type: "int", nullable: false),
                    Level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSkill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectSkill_Skill_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StudentRegistrationSkill",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    idskill = table.Column<int>(name: "id_skill", type: "int", nullable: false),
                    level = table.Column<short>(type: "SMALLINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentRegistrationSkill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentRegSkill_Skill",
                        column: x => x.idskill,
                        principalTable: "Skill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CompanyRepresentative",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "NVARCHAR(80)", maxLength: 80, nullable: false),
                    internalcode = table.Column<string>(name: "internal_code", type: "VARCHAR(50)", maxLength: 50, nullable: false),
                    cpf = table.Column<string>(type: "VARCHAR(11)", maxLength: 11, nullable: false),
                    companyid = table.Column<int>(name: "company_id", type: "int", nullable: true),
                    userid = table.Column<int>(name: "user_id", type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyRepresentative", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyRepresentative_Company",
                        column: x => x.companyid,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyRepresentative_User",
                        column: x => x.userid,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Professor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "NVARCHAR(80)", maxLength: 80, nullable: false),
                    iduser = table.Column<int>(name: "id_user", type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Professor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Professor_User",
                        column: x => x.iduser,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "NVARCHAR(80)", maxLength: 80, nullable: false),
                    ra = table.Column<string>(type: "VARCHAR(10)", maxLength: 10, nullable: false),
                    cpf = table.Column<string>(type: "VARCHAR(11)", maxLength: 11, nullable: false),
                    rg = table.Column<string>(type: "VARCHAR(15)", maxLength: 15, nullable: false),
                    cellphone = table.Column<string>(type: "VARCHAR(15)", maxLength: 15, nullable: false),
                    idcurrentcourse = table.Column<int>(name: "id_current_course", type: "int", nullable: true),
                    iduser = table.Column<int>(name: "id_user", type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Student_CurrentCourse",
                        column: x => x.idcurrentcourse,
                        principalTable: "CurrentCourse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Student_User",
                        column: x => x.iduser,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User_Project",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Project", x => new { x.ProjectId, x.UserId });
                    table.ForeignKey(
                        name: "FK_Project_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    idrole = table.Column<int>(name: "id_role", type: "int", nullable: false),
                    iduser = table.Column<int>(name: "id_user", type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.idrole, x.iduser });
                    table.ForeignKey(
                        name: "FK_UserRole_RoleId",
                        column: x => x.idrole,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_UserId",
                        column: x => x.iduser,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Project_ProjectSkill",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ProjectSkillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project_ProjectSkill", x => new { x.ProjectId, x.ProjectSkillId });
                    table.ForeignKey(
                        name: "FK_ProjectReg_ProjectSkill_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Project_ProjectSkill_SkillId",
                        column: x => x.ProjectSkillId,
                        principalTable: "ProjectSkill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AllocationResult",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    idstudent = table.Column<int>(name: "id_student", type: "int", nullable: false),
                    idproject = table.Column<int>(name: "id_project", type: "int", nullable: false),
                    semester = table.Column<string>(type: "NVARCHAR(7)", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllocationResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllocationResult_Project",
                        column: x => x.idproject,
                        principalTable: "Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AllocationResult_Student",
                        column: x => x.idstudent,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StudentRegistration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    idstudent = table.Column<int>(name: "id_student", type: "int", nullable: false),
                    skillsdescription = table.Column<string>(name: "skills_description", type: "NVARCHAR(100)", nullable: false),
                    registrationdate = table.Column<DateTime>(name: "registration_date", type: "DATE", nullable: false),
                    subject = table.Column<short>(type: "SMALLINT", nullable: false),
                    choicepriority = table.Column<short>(name: "choice_priority", type: "SMALLINT", nullable: false),
                    enrolledinindustry40 = table.Column<ulong>(name: "enrolled_in_industry_4_0", type: "BIT", nullable: false),
                    doesnotmeetrequirements = table.Column<short>(name: "does_not_meet_requirements", type: "SMALLINT", nullable: false),
                    presencial = table.Column<ulong>(type: "BIT", nullable: false),
                    firstmeetingdate = table.Column<DateTime>(name: "first_meeting_date", type: "DATE", nullable: false),
                    semester = table.Column<string>(type: "VARCHAR(7)", nullable: false),
                    StudentRegistrationScoreId = table.Column<int>(type: "int", nullable: false),
                    filepath = table.Column<string>(name: "file_path", type: "VARCHAR(200)", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentRegistration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentReg_Student",
                        column: x => x.idstudent,
                        principalTable: "Student",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentRegistration_StudentRegistrationScores_StudentRegistr~",
                        column: x => x.StudentRegistrationScoreId,
                        principalTable: "StudentRegistrationScores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentRegistration_Student_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Student",
                        principalColumn: "Id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "StudentRegistration_RegistrationSkill",
                columns: table => new
                {
                    RegistrationId = table.Column<int>(type: "int", nullable: false),
                    RegistrationSkillId = table.Column<int>(type: "int", nullable: false),
                    semester = table.Column<string>(type: "VARCHAR(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentRegistration_RegistrationSkill", x => new { x.RegistrationId, x.RegistrationSkillId });
                    table.ForeignKey(
                        name: "FK_StdReg_RegSkill_RegId",
                        column: x => x.RegistrationId,
                        principalTable: "StudentRegistration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StdReg_RegSkill_SkillId",
                        column: x => x.RegistrationSkillId,
                        principalTable: "StudentRegistrationSkill",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AllocationResult_id_student",
                table: "AllocationResult",
                column: "id_student");

            migrationBuilder.CreateIndex(
                name: "IX_AllocationResult_Unique",
                table: "AllocationResult",
                columns: new[] { "id_project", "id_student", "semester" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyRepresentative_company_id",
                table: "CompanyRepresentative",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyRepresentative_InternalCode",
                table: "CompanyRepresentative",
                column: "internal_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyRepresentative_user_id",
                table: "CompanyRepresentative",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Code_Slug",
                table: "CourseSubject",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professor_id_user",
                table: "Professor",
                column: "id_user",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_ProjectSkill_ProjectSkillId",
                table: "Project_ProjectSkill",
                column: "ProjectSkillId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSkill_SkillId",
                table: "ProjectSkill",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_Skill_Tag",
                table: "Skill",
                column: "tag",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RA_Slug",
                table: "Student",
                column: "ra",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_id_current_course",
                table: "Student",
                column: "id_current_course",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_id_user",
                table: "Student",
                column: "id_user",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentRegistration_id_student",
                table: "StudentRegistration",
                column: "id_student");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRegistration_StudentId",
                table: "StudentRegistration",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRegistration_StudentRegistrationScoreId",
                table: "StudentRegistration",
                column: "StudentRegistrationScoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRegistration_RegistrationSkill_RegistrationSkillId",
                table: "StudentRegistration_RegistrationSkill",
                column: "RegistrationSkillId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentRegistrationSkill_id_skill",
                table: "StudentRegistrationSkill",
                column: "id_skill");

            migrationBuilder.CreateIndex(
                name: "IX_User_Slug",
                table: "User",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Project_UserId",
                table: "User_Project",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_id_user",
                table: "UserRole",
                column: "id_user");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllocationResult");

            migrationBuilder.DropTable(
                name: "CompanyRepresentative");

            migrationBuilder.DropTable(
                name: "CourseSubject");

            migrationBuilder.DropTable(
                name: "GeneralConfig");

            migrationBuilder.DropTable(
                name: "Professor");

            migrationBuilder.DropTable(
                name: "Project_ProjectSkill");

            migrationBuilder.DropTable(
                name: "StudentRegistration_RegistrationSkill");

            migrationBuilder.DropTable(
                name: "User_Project");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "ProjectSkill");

            migrationBuilder.DropTable(
                name: "StudentRegistration");

            migrationBuilder.DropTable(
                name: "StudentRegistrationSkill");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "StudentRegistrationScores");

            migrationBuilder.DropTable(
                name: "Skill");

            migrationBuilder.DropTable(
                name: "CurrentCourse");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
