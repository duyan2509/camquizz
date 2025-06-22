using CamQuizz.Domain.Entities;
using CamQuizz.Domain;
using Microsoft.EntityFrameworkCore;

namespace CamQuizz.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Genre> Genres { get; set; }

    public DbSet<Quizz> Quizzes { get; set; }
    public DbSet<Question> Questions { get; set; }

    public DbSet<Answer> Answers { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);
        });
        modelBuilder.Entity<Role>()
            .Property(r => r.Name)
            .HasConversion<int>();

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasMany(e => e.Quizzes)
                .WithOne(q => q.Genre)
                .HasForeignKey(q => q.GenreId)
                .OnDelete(DeleteBehavior.SetNull);
        });
        modelBuilder.Entity<Quizz>(entity =>
        {
            entity.HasMany(e=>e.Questions)
                  .WithOne(q=>q.Quizz)
                  .HasForeignKey(q=>q.QuizzId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasMany(e=>e.Answers)
                .WithOne(a=>a.Question)
                .HasForeignKey(a=>a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        // Global query filter for soft delete
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var method = typeof(DbContextExtensions)
                    .GetMethod(nameof(DbContextExtensions.AddIsDeletedFilter), System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)!
                    .MakeGenericMethod(entityType.ClrType);

                method.Invoke(null, new object[] { modelBuilder });
            }
        }


        // Seed
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = UserRole.User },
            new Role { Id = 2, Name = UserRole.Admin }
        );



    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<BaseEntity>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
    public static class DbContextExtensions
    {
        public static void AddIsDeletedFilter<TEntity>(ModelBuilder builder) where TEntity : BaseEntity
        {
            builder.Entity<TEntity>().HasQueryFilter(e => !e.IsDeleted);
        }
    }

}
