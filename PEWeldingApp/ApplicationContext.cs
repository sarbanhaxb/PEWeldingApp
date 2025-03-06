using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PEWeldingApp.Models;


namespace PEWeldingApp
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Organization> Organizations { get; set; } = null!;
        public DbSet<Emission> Emissions { get; set; } = null!;
        public DbSet<CalcVariant> CalcVariants { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source=database.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrganizationConf());
            modelBuilder.ApplyConfiguration(new CalcVariantConf());
        }

        public class OrganizationConf : IEntityTypeConfiguration<Organization>
        {
            public void Configure(EntityTypeBuilder<Organization> builder)
            {
                builder.HasKey(o => o.Id);
                builder.Property(p => p.Title).HasField("title");
                builder.Property(p => p.Code).HasField("code");
            }
        }

        public class CalcVariantConf : IEntityTypeConfiguration<CalcVariant>
        {
            public void Configure(EntityTypeBuilder<CalcVariant> builder)
            {
                builder.HasKey(o => o.Id);
                builder.Property(o => o.SeamLength).HasField("seamLength");
                builder.Property(o => o.SeamWidth).HasField("seamWidth");
                builder.Property(o => o.SeamThick).HasField("seamThick");
                builder.Property(o => o.Density).HasField("density");
                builder.Property(o => o.Place).HasField("place");
                builder.Property(o => o.Title).HasField("title");
                builder.Property(o => o.Type).HasField("type");
                builder.Property(o => o.WorkVol).HasField("workVol");
                builder.Property(o => o.SeamNum).HasField("seamNum");
                builder.Property(o => o.Number).HasField("number");
                builder
                    .HasOne(o => o.Organization)
                    .WithMany(o => o.CalcVariants)
                    .HasForeignKey(o => o.OrganizationId);
            }
        }

    }
}