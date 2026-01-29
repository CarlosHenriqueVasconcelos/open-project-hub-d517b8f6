using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.Models;

namespace PlataformaGestaoIA.DataContext.Mappings
{
    public class StudentRegistrationMap : IEntityTypeConfiguration<StudentRegistration>
    {
        public void Configure(EntityTypeBuilder<StudentRegistration> builder)
        {
            // Tabela
            builder.ToTable("StudentRegistration");

            // Chave Primária
            builder.HasKey(x => x.Id);

            // Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();


            builder.Property(x => x.SkillsDescription)
                .HasColumnName("skills_description")
                .HasColumnType("NVARCHAR(100)");

            builder.Property(x => x.RegistrationDate)
                .IsRequired()
                .HasColumnName("registration_date")
                .HasColumnType("DATE");

            builder.Property(x => x.FilePath)
                .HasColumnName("file_path")
                .HasColumnType("VARCHAR(200)");

            builder.Property(x => x.Subject)
                .IsRequired()
                .HasColumnName("subject")
                .HasColumnType("SMALLINT");

            builder.Property(x => x.ChoicePriority)
                .IsRequired()
                .HasColumnName("choice_priority")
                .HasColumnType("SMALLINT");

            builder.Property(x => x.EnrolledInIndustry4_0)
                .HasColumnName("enrolled_in_industry_4_0")
                .HasColumnType("BIT");

            builder.Property(x => x.Presencial)
                .HasColumnName("presencial")
                .HasColumnType("BIT");

            builder.Property(x => x.DoesNotMeetRequirements)
                .HasColumnName("does_not_meet_requirements")
                .HasColumnType("SMALLINT");

            builder.Property(x => x.CursarUmaOuDuas)
                .IsRequired()                            // ou remova se for opcional
                .HasColumnName("cursar_uma_ou_duas")
                .HasColumnType("SMALLINT");

            builder.Property(x => x.FirstMeetingDate)
                .HasColumnName("first_meeting_date")
                .HasColumnType("DATE");

            builder.Property(x => x.Semester)
                .IsRequired()
                .HasColumnName("semester")
                .HasColumnType("VARCHAR(7)");

            // Relacionamentos
            builder.HasOne(x => x.Student)
               .WithMany()
               .HasForeignKey("id_student")
               .HasConstraintName("FK_StudentReg_Student")
               .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasMany(x => x.StudentSkills)
                .WithMany(x => x.StudentRegistration)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentRegistration_RegistrationSkill",
                    registration => registration
                        .HasOne<StudentRegistrationSkill>()
                        .WithMany()
                        .HasForeignKey("RegistrationSkillId")
                        .HasConstraintName("FK_StdReg_RegSkill_SkillId")
                        .OnDelete(DeleteBehavior.Cascade),
                    regskill => regskill
                        .HasOne<StudentRegistration>()
                        .WithMany()
                        .HasForeignKey("RegistrationId")
                        .HasConstraintName("FK_StdReg_RegSkill_RegId")
                        .OnDelete(DeleteBehavior.Cascade))
            .Property<string>("Semester")
                .HasColumnName("semester")
                .HasColumnType("VARCHAR(7)");
        }
    }
}
