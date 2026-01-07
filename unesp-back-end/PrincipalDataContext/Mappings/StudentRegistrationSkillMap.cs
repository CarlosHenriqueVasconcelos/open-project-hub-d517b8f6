using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.Models;

namespace PlataformaGestaoIA.DataContext.Mappings
{
    public class StudentRegistrationSkillMap : IEntityTypeConfiguration<StudentRegistrationSkill>
    {
        public void Configure(EntityTypeBuilder<StudentRegistrationSkill> builder)
        {
            // Tabela
            builder.ToTable("StudentRegistrationSkill");

            // Chave Primária
            builder.HasKey(x => x.Id);

            // Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Level)
                .IsRequired()
                .HasColumnName("level")
                .HasColumnType("SMALLINT");

            builder.HasOne(x => x.Skill)
               .WithMany()
               .HasForeignKey("id_skill")
               .HasConstraintName("FK_StudentRegSkill_Skill")
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
