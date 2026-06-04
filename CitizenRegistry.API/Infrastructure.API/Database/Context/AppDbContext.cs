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

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                }
            }
  
            return base.SaveChangesAsync(cancellationToken);
        }
    }

}
