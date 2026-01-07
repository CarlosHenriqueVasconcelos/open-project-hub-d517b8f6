using PlataformaGestaoIA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PlataformaGestaoIA.DataContext.Mappings
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class SkillMap : IEntityTypeConfiguration<Skill>
    {
        public void Configure(EntityTypeBuilder<Skill> builder)
        {
            // Tabela
            builder.ToTable("Skill");

            // Chave Primária
            builder.HasKey(x => x.Id);

            // Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            // Propriedades
            builder.Property(x => x.Name)
                .IsRequired()
                .HasColumnName("name")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Tag)
                .IsRequired()
                .HasColumnName("tag")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(100)
                .IsRequired();

            builder
                .HasIndex(x => x.Tag, "IX_Skill_Tag")
                .IsUnique();

            builder.Property(x => x.IsSoftSkill)
                .IsRequired()
                .HasColumnName("is_soft_skill")
                .HasColumnType("BIT");
        }
    }
}