using System.ComponentModel.DataAnnotations;
using TaskManager.Api.Domain;
using TaskStatus = TaskManager.Api.Domain.TaskStatus;

namespace TaskManager.Api.Contracts;

public sealed record RegisterRequest([Required, MaxLength(100)] string Name, [Required, EmailAddress] string Email, [Required, MinLength(8)] string Password);
public sealed record LoginRequest([Required, EmailAddress] string Email, [Required] string Password);
public sealed record AuthResponse(string Token, Guid UserId, string Name, string Email);
public sealed record TaskRequest([Required, MaxLength(200)] string Title, [MaxLength(4000)] string? Description, TaskStatus Status, TaskPriority Priority, DateTime? DueDate);
public sealed record TaskResponse(Guid Id, string Title, string? Description, TaskStatus Status, TaskPriority Priority, DateTime? DueDate, DateTime CreatedAt, DateTime UpdatedAt, Guid UserId);
