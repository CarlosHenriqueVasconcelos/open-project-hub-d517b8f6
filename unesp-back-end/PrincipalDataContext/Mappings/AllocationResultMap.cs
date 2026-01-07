using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaGestaoIA.Models;

namespace PlataformaGestaoIA.DataContext.Mappings
{
    public class AllocationResultMap : IEntityTypeConfiguration<AllocationResult>
    {
        public void Configure(EntityTypeBuilder<AllocationResult> builder)
        {
            // Tabela
            builder.ToTable("AllocationResult");

            // Chave Primária
            builder.HasKey(x => x.Id);

            // Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            // Relacionamentos
            builder.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey("id_student")
                .HasConstraintName("FK_AllocationResult_Student")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Project)
                .WithMany()
                .HasForeignKey("id_project")
                .HasConstraintName("FK_AllocationResult_Project")
                .OnDelete(DeleteBehavior.Cascade);

            // Propriedade adicional Semester
            builder.Property(ps => ps.Semester)
                .IsRequired()
                .HasColumnName("semester")
                .HasColumnType("NVARCHAR")
                .HasMaxLength(7);

            builder.HasIndex("id_project", "id_student", "Semester")
               .IsUnique()
                .HasDatabaseName("IX_AllocationResult_Unique");
        }
    }
}