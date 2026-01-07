using PlataformaGestaoIA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PlataformaGestaoIA.DataContext.Mappings
{
	public class CompanyMap : IEntityTypeConfiguration<Company>
	{
		public void Configure(EntityTypeBuilder<Company> builder)
		{
			// Tabela
			builder.ToTable("Company");

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

			// Propriedades
			builder.Property(x => x.LegalName)
				.IsRequired()
				.HasColumnName("legal_name")
				.HasColumnType("NVARCHAR")
				.HasMaxLength(80);

			builder.Property(x => x.CNPJ)
				.IsRequired()
				.HasColumnName("cnpj")
				.HasColumnType("NVARCHAR")
				.HasMaxLength(15);
		}
	}
}