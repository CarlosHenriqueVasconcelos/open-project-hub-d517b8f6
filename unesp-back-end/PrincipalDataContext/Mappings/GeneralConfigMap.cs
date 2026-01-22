using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaGestaoIA.Models;

namespace PlataformaGestaoIA.DataContext.Mappings
{
    public class GeneralConfigMap : IEntityTypeConfiguration<GeneralConfig>
    {
        public void Configure(EntityTypeBuilder<GeneralConfig> builder)
        {
            builder.ToTable("GeneralConfig");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ConfigHeader)
                .IsRequired()
                .HasColumnName("config_header")
                .HasColumnType("LONGTEXT");

            builder.Property(x => x.ConfigBody)
                .IsRequired()
                .HasColumnName("config_body")
                .HasColumnType("LONGTEXT");

            builder.Property(x => x.ConfigEmailDomainAvaliable)
                .IsRequired()
                .HasColumnName("config_email_domain_avaliable")
                .HasColumnType("LONGTEXT");

            builder.Property(x => x.ConfigConsent)
                .IsRequired()
                .HasColumnName("config_consent")
                .HasColumnType("LONGTEXT");

            builder.Property(x => x.Stage)
                .HasColumnName("config_stage")
                .HasColumnType("VARCHAR(20)");

            builder.Property(x => x.ConfirmationDeadline)
                .HasColumnName("config_confirmation_deadline")
                .HasColumnType("DATETIME");
        }
    }
}
