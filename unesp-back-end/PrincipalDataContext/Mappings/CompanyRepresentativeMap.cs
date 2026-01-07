using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaGestaoIA.Models;

namespace PlataformaGestaoIA.DataContext.Mappings
{
    public class CompanyRepresentativeMap : IEntityTypeConfiguration<CompanyRepresentative>
    {
        public void Configure(EntityTypeBuilder<CompanyRepresentative> builder)
        {
            // Tabela
            builder.ToTable("CompanyRepresentative");

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

            builder.Property(x => x.InternalCode)
                .IsRequired()
                .HasColumnName("internal_code")
                .HasColumnType("VARCHAR")
                .HasMaxLength(50);

            builder.Property(x => x.CPF)
                .IsRequired()
                .HasColumnName("cpf")
                .HasColumnType("VARCHAR")
                .HasMaxLength(11);

            builder.HasOne(x => x.Company)
                .WithMany()
                .HasForeignKey("company_id")
                .HasConstraintName("FK_CompanyRepresentative_Company")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey("user_id")
                .HasConstraintName("FK_CompanyRepresentative_User")
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasIndex(x => x.InternalCode, "IX_CompanyRepresentative_InternalCode")
                .IsUnique();
        }
    }
}