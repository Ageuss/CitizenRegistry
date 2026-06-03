using CitizenRegistry.API.Domain.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CitizenRegistry.API.Infrastructure.API.Database.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Citizen> Citizens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Citizen>(entity =>
            {
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(c => c.Cpf)
                    .IsRequired()
                    .HasMaxLength(11);

                entity.HasIndex(c => c.Cpf)
                    .IsUnique();
            });

            base.OnModelCreating(modelBuilder);

        }
    }

}
