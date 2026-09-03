using System.ComponentModel.DataAnnotations;

namespace TaskManager.Api.Domain;

public enum TaskStatus { ToDo, InProgress, Completed }
public enum TaskPriority { Low, Medium, High }

public sealed class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [MaxLength(100)] public required string Name { get; set; }
    [MaxLength(256)] public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<TaskItem> Tasks { get; set; } = [];
}

public sealed class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [MaxLength(200)] public required string Title { get; set; }
    [MaxLength(4000)] public string? Description { get; set; }
    public TaskStatus Status { get; set; } = TaskStatus.ToDo;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}