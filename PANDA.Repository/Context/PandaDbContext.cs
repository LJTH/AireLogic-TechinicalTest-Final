using Microsoft.EntityFrameworkCore;
using PANDA.Repository.Model;

namespace PANDA.Repository.Context
{
    public class PandaDbContext : DbContext
    {
        public PandaDbContext(DbContextOptions<PandaDbContext> options):base(options) 
        {
            
        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Clinician> Clinicians { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureBaseProperties(modelBuilder);

            modelBuilder.Entity<Department>(builder => { 
                builder.Property(entity => entity.Code).IsRequired(); 
                builder.Property(entity => entity.Name).IsRequired();
            });

            modelBuilder.Entity<Clinician>(builder =>
            {
                builder.Property(entity => entity.GmcCode).IsRequired();
                builder.Property(entity => entity.Surname).IsRequired();
                builder.Property(entity => entity.Forename).IsRequired();
            });
        }

        private void ConfigureBaseProperties(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(EntityModelBase).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                       .HasKey(nameof(EntityModelBase.Id));

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(EntityModelBase.CreatedDateTime))
                        .IsRequired()
                        .HasDefaultValueSql("GETUTCDATE()");

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(EntityModelBase.UpdatedDateTime))
                        .IsRequired()
                        .HasDefaultValueSql("GETUTCDATE()");
                }
            }
        }

    }
}
