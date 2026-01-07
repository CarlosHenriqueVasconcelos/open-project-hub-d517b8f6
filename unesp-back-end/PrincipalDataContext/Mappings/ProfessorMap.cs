using PlataformaGestaoIA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PlataformaGestaoIA.DataContext.Mappings
{
    public class ProfessorMap : IEntityTypeConfiguration<Professor>
    {
        public void Configure(EntityTypeBuilder<Professor> builder)
        {
            // Tabela
            builder.ToTable("Professor");

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

            builder.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<Professor>("id_user")
                .HasConstraintName("FK_Professor_User")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}