using PlataformaGestaoIA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PlataformaGestaoIA.DataContext.Mappings
{
    public class StudentMap : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            // Tabela
            builder.ToTable("Student");

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

            builder.Property(x => x.RA)
                .IsRequired()
                .HasColumnName("ra")
                .HasColumnType("VARCHAR")
                .HasMaxLength(10);

            builder.Property(x => x.CPF)
                .HasColumnName("cpf")
                .HasColumnType("VARCHAR")
                .HasMaxLength(11);

            builder.Property(x => x.RG)
                .HasColumnName("rg")
                .HasColumnType("VARCHAR")
                .HasMaxLength(15);

            builder.Property(x => x.Cellphone)
                .HasColumnName("cellphone")
                .HasColumnType("VARCHAR")
                .HasMaxLength(15);

            // Relacionamento com CurrentCourse (1 para 1)
            builder.HasOne(x => x.CurrentCourse)
                .WithOne()
                .HasForeignKey<Student>("id_current_course")
                .HasConstraintName("FK_Student_CurrentCourse")
                .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento com User (1 para 1)
            builder.HasOne(x => x.User)
                .WithOne()
                .HasForeignKey<Student>("id_user")
                .HasConstraintName("FK_Student_User")
                .OnDelete(DeleteBehavior.Cascade);

            // Índices
            builder
                .HasIndex(x => x.RA, "IX_RA_Slug")
                .IsUnique();
        }
    }
}