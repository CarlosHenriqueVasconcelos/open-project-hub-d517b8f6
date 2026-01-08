using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.Models;

namespace PlataformaGestaoIA.DataContext.Mappings
{
    public class StudentRegistrationScoresMap : IEntityTypeConfiguration<StudentRegistrationScore>
    {
        public void Configure(EntityTypeBuilder<StudentRegistrationScore> builder)
        {
            // Tabela
            builder.ToTable("StudentRegistrationScores");

            // Chave Primária
            builder.HasKey(x => x.Id);

            // Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.PerformanceCoefficient)
                .IsRequired()
                .HasColumnName("performance_coefficient")
                .HasColumnType("FLOAT");

            builder.Property(x => x.ScientificInitiationProgramScore)
                .HasColumnName("scientific_initiation_program_score")
                .HasColumnType("INT");

            builder.Property(x => x.InstitutionalMonitoringProgramScore)
                .HasColumnName("institutional_monitoring_program_score")
                .HasColumnType("INT");

            builder.Property(x => x.JuniorEnterpriseExperienceScore)
                .HasColumnName("junior_enterprise_experience_score")
                .HasColumnType("INT");

            builder.Property(x => x.ProjectInTechnologicalHotelScore)
                .HasColumnName("project_in_technological_hotel_score")
                .HasColumnType("INT");

            builder.Property(x => x.VolunteeringScore)
                .HasColumnName("volunteering_score")
                .HasColumnType("FLOAT");

            builder.Property(x => x.HighGradeDisciplineScore)
                .HasColumnName("high_grade_discipline_score")
                .HasColumnType("INT");

            builder.Property(x => x.CertificationCoursesScore)
                .HasColumnName("certification_courses_score")
                .HasColumnType("INT");

            builder.Property(x => x.HighGradeCoursesScore)
                .HasColumnName("high_grade_courses_score")
                .HasColumnType("INT");

            builder.Property(x => x.AIProjectsScore)
                .HasColumnName("ai_projects_score")
                .HasColumnType("INT");

            builder.Property(x => x.InternshipEmploymentScore)
                .HasColumnName("internship_employment_score")
                .HasColumnType("INT");

            builder.Property(x => x.TechnologyCertificationScore)
                .HasColumnName("technology_certification_score")
                .HasColumnType("INT");

            builder.Property(x => x.LowLevelTechScore)
                .HasColumnName("low_level_tech_score")
                .HasColumnType("INT");

            builder.Property(x => x.ScoreCoursesDescription)
                .HasColumnName("score_courses_description")
                .HasColumnType("NVARCHAR(100)");
        }
    }
}
