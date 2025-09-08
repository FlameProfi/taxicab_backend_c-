using course.Models;
using Microsoft.EntityFrameworkCore;

namespace course.Models
{
    public class TaxiDbContext : DbContext
    {
        public TaxiDbContext(DbContextOptions<TaxiDbContext> options) : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<TaxiCall> TaxiCalls { get; set; }
        public DbSet<DriverApplication> DriverApplications { get; set; }
        public DbSet<User> Users { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>(entity =>
            {
                entity.HasIndex(e => e.Make);
                entity.HasIndex(e => e.Model);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Department);
            });

            modelBuilder.Entity<TaxiCall>(entity =>
            {
                entity.HasIndex(e => e.CallTime);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.ClientName);

                entity.Property(e => e.Status)
                    .HasMaxLength(20);
            });

            modelBuilder.Entity<DriverApplication>(entity =>
            {
                entity.HasIndex(e => e.Phone);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.CreatedAt);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Role).HasConversion<string>();
            });

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Car || e.Entity is Employee ||
                           e.Entity is TaxiCall || e.Entity is DriverApplication || e.Entity is User) 
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.Entity is Car car)
                    car.UpdatedAt = DateTime.UtcNow;
                else if (entry.Entity is Employee employee)
                    employee.UpdatedAt = DateTime.UtcNow;
                else if (entry.Entity is TaxiCall taxiCall)
                    taxiCall.UpdatedAt = DateTime.UtcNow;
                else if (entry.Entity is DriverApplication application)
                    application.UpdatedAt = DateTime.UtcNow;
                else if (entry.Entity is User user) 
                    user.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}