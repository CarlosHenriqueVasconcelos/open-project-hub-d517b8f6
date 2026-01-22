using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaGestaoIA.Models;

namespace PlataformaGestaoIA.DataContext.Mappings
{
    public class StudentRegistrationRankingMap : IEntityTypeConfiguration<StudentRegistrationRanking>
    {
        public void Configure(EntityTypeBuilder<StudentRegistrationRanking> builder)
        {
            builder.ToTable("StudentRegistrationRanking");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Semester)
                .IsRequired()
                .HasColumnName("semester")
                .HasColumnType("VARCHAR(7)");

            builder.Property(x => x.SubjectValue)
                .IsRequired()
                .HasColumnName("subject_value")
                .HasColumnType("SMALLINT");

            builder.Property(x => x.TotalScore)
                .IsRequired()
                .HasColumnName("total_score")
                .HasColumnType("FLOAT");

            builder.Property(x => x.PerformanceCoefficient)
                .IsRequired()
                .HasColumnName("performance_coefficient")
                .HasColumnType("FLOAT");

            builder.Property(x => x.RankPosition)
                .IsRequired()
                .HasColumnName("rank_position")
                .HasColumnType("INT");

            builder.Property(x => x.Classification)
                .IsRequired()
                .HasColumnName("classification")
                .HasColumnType("VARCHAR(20)");

            builder.Property(x => x.Status)
                .IsRequired()
                .HasColumnName("status")
                .HasColumnType("VARCHAR(20)");

            builder.Property(x => x.ConfirmBy)
                .HasColumnName("confirm_by")
                .HasColumnType("DATETIME");

            builder.Property(x => x.StatusUpdatedAt)
                .HasColumnName("status_updated_at")
                .HasColumnType("DATETIME");

            builder.Property(x => x.RegistrationDate)
                .HasColumnName("registration_date")
                .HasColumnType("DATETIME");

            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("DATETIME");

            builder.Property(x => x.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("DATETIME");

            builder.HasOne(x => x.StudentRegistration)
                .WithMany()
                .HasForeignKey(x => x.StudentRegistrationId)
                .HasConstraintName("FK_Ranking_StudentRegistration")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .HasConstraintName("FK_Ranking_Student")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
