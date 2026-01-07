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
                .IsRequired()
                .HasColumnName("scientific_initiation_program_score")
                .HasColumnType("INT");

            builder.Property(x => x.InstitutionalMonitoringProgramScore)
                .IsRequired()
                .HasColumnName("institutional_monitoring_program_score")
                .HasColumnType("INT");

            builder.Property(x => x.JuniorEnterpriseExperienceScore)
                .IsRequired()
                .HasColumnName("junior_enterprise_experience_score")
                .HasColumnType("INT");

            builder.Property(x => x.ProjectInTechnologicalHotelScore)
                .IsRequired()
                .HasColumnName("project_in_technological_hotel_score")
                .HasColumnType("INT");

            builder.Property(x => x.VolunteeringScore)
                .IsRequired()
                .HasColumnName("volunteering_score")
                .HasColumnType("INT");

            builder.Property(x => x.HighGradeDisciplineScore)
                .IsRequired()
                .HasColumnName("high_grade_discipline_score")
                .HasColumnType("INT");

            builder.Property(x => x.CertificationCoursesScore)
                .IsRequired()
                .HasColumnName("certification_courses_score")
                .HasColumnType("INT");

            builder.Property(x => x.HighGradeCoursesScore)
                .IsRequired()
                .HasColumnName("high_grade_courses_score")
                .HasColumnType("INT");

            builder.Property(x => x.AIProjectsScore)
                .IsRequired()
                .HasColumnName("ai_projects_score")
                .HasColumnType("INT");

            builder.Property(x => x.InternshipEmploymentScore)
                .IsRequired()
                .HasColumnName("internship_employment_score")
                .HasColumnType("INT");

            builder.Property(x => x.TechnologyCertificationScore)
                .IsRequired()
                .HasColumnName("technology_certification_score")
                .HasColumnType("INT");

            builder.Property(x => x.LowLevelTechScore)
                .IsRequired()
                .HasColumnName("low_level_tech_score")
                .HasColumnType("INT");

            builder.Property(x => x.ScoreCoursesDescription)
                .HasColumnName("score_courses_description")
                .HasColumnType("NVARCHAR(100)");
        }
    }
}
