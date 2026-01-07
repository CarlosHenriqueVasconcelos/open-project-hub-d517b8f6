using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlataformaGestaoIA.Models;

namespace PlataformaGestaoIA.DataContext.Mappings;
public class ProjectMap : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        // Tabela
        builder.ToTable("Project");

        // Chave Primária
        builder.HasKey(x => x.Id);

        // Identity
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Description)
            .HasColumnName("description")
            .HasColumnType("TEXT");

        builder.Property(x => x.InternalCode)
            .IsRequired()
            .HasColumnName("internal_code")
            .HasColumnType("NVARCHAR")
            .HasMaxLength(20);

        builder
          .HasMany(x => x.Users)
          .WithMany(x => x.Projects)
          .UsingEntity<Dictionary<string, object>>(
              "User_Project",
              registration => registration
                  .HasOne<User>()
                  .WithMany()
                  .HasForeignKey("UserId")
                  .HasConstraintName("FK_Project_UserId")
                  .OnDelete(DeleteBehavior.Cascade),
              regskill => regskill
                  .HasOne<Project>()
                  .WithMany()
                  .HasForeignKey("ProjectId")
                  .HasConstraintName("FK_User_ProjectId")
                  .OnDelete(DeleteBehavior.Cascade));

        builder
                .HasMany(x => x.ProjectSkill)
                .WithMany(x => x.Project)
                .UsingEntity<Dictionary<string, object>>(
                    "Project_ProjectSkill",
                    ProjectSkill => ProjectSkill
                        .HasOne<ProjectSkill>()
                        .WithMany()
                        .HasForeignKey("ProjectSkillId")
                        .HasConstraintName("FK_Project_ProjectSkill_SkillId")
                        .OnDelete(DeleteBehavior.Cascade),
                    Project => Project
                        .HasOne<Project>()
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .HasConstraintName("FK_ProjectReg_ProjectSkill_ProjectId")
                        .OnDelete(DeleteBehavior.Cascade));
    }
}