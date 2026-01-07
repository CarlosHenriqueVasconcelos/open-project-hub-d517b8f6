using PlataformaGestaoIA.Models;
using MySql.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PlataformaGestaoIA.DataContext.Mappings;
using PlataformaGestaoIA.Models;

namespace PlataformaGestaoIA.DataContext
{
    public class PrincipalDataContext : DbContext
    {
        public PrincipalDataContext(DbContextOptions<PrincipalDataContext> options)
            : base(options)
        {

        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<CourseSubject> CourseSubjects { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<StudentRegistration> StudentRegistrations { get; set; }
        public DbSet<CurrentCourse> CurrentCourse { get; set; }
        public DbSet<CompanyRepresentative> CompanyRepresentatives { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<AllocationResult> AllocationResults { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<GeneralConfig> GeneralConfigs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompanyMap());
            modelBuilder.ApplyConfiguration(new CourseSubjectMap());
            modelBuilder.ApplyConfiguration(new ProfessorMap());
            modelBuilder.ApplyConfiguration(new SkillMap());
            modelBuilder.ApplyConfiguration(new StudentMap());
            modelBuilder.ApplyConfiguration(new UserMap());
            modelBuilder.ApplyConfiguration(new StudentRegistrationMap());
            modelBuilder.ApplyConfiguration(new StudentRegistrationSkillMap());
            modelBuilder.ApplyConfiguration(new StudentRegistrationScoresMap());
            modelBuilder.ApplyConfiguration(new CompanyRepresentativeMap());
            modelBuilder.ApplyConfiguration(new ProjectMap());
            modelBuilder.ApplyConfiguration(new AllocationResultMap());
            modelBuilder.ApplyConfiguration(new GeneralConfigMap());
        }
    }
}