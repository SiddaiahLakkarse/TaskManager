using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Domain;
using TaskStatus = TaskManager.Api.Domain.TaskStatus;

namespace TaskManager.Api.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
        modelBuilder.Entity<User>().Property(x => x.Email).HasConversion(v => v.ToLowerInvariant(), v => v);
        modelBuilder.Entity<TaskItem>().HasOne(x => x.User).WithMany(x => x.Tasks).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
        var demoId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        modelBuilder.Entity<User>().HasData(new User { Id = demoId, Name = "Demo User", Email = "demo@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Demo1234!"), CreatedAt = DateTime.UtcNow });
        modelBuilder.Entity<TaskItem>().HasData(new TaskItem { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), UserId = demoId, Title = "Explore TaskManager", Description = "Update this sample task.", Status = TaskStatus.InProgress, Priority = TaskPriority.High, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow });
    }
}