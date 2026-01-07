using PlataformaGestaoIA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PlataformaGestaoIA.DataContext.Mappings
{
    public class CourseSubjectMap : IEntityTypeConfiguration<CourseSubject>
    {
        public void Configure(EntityTypeBuilder<CourseSubject> builder)
        {
            // Tabela
            builder.ToTable("CourseSubject");

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
                .HasMaxLength(80);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasColumnName("code")
                .HasColumnType("VARCHAR")
                .HasMaxLength(10);

            // Índices
            builder
                .HasIndex(x => x.Code, "IX_Code_Slug")
                .IsUnique();
        }
    }
}