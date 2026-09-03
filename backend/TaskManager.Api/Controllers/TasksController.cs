using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Contracts;
using TaskManager.Api.Domain;
using TaskManager.Api.Infrastructure.Persistence;
using TaskStatus = TaskManager.Api.Domain.TaskStatus;

namespace TaskManager.Api.Controllers;
[ApiController, Authorize, Route("api/tasks")]
public sealed class TasksController(AppDbContext db) : ControllerBase
{
    private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    private static TaskResponse Map(TaskItem x) => new(x.Id, x.Title, x.Description, x.Status, x.Priority, x.DueDate, x.CreatedAt, x.UpdatedAt, x.UserId);
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskResponse>>> Get([FromQuery] TaskStatus? status, [FromQuery] TaskPriority? priority, [FromQuery] string? search)
    {
        var query = db.Tasks.AsNoTracking().Where(x => x.UserId == UserId);
        if (status.HasValue) query = query.Where(x => x.Status == status);
        if (priority.HasValue) query = query.Where(x => x.Priority == priority);
        if (!string.IsNullOrWhiteSpace(search)) query = query.Where(x => x.Title.Contains(search));
        return Ok(await query.OrderBy(x => x.DueDate).Select(x => Map(x)).ToListAsync());
    }
    [HttpGet("{id:guid}")] public async Task<ActionResult<TaskResponse>> Get(Guid id) { var x = await db.Tasks.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id && x.UserId == UserId); return x is null ? NotFound() : Ok(Map(x)); }
    [HttpPost] public async Task<ActionResult<TaskResponse>> Create(TaskRequest request) { var x = new TaskItem { Title = request.Title.Trim(), Description = request.Description, Status = request.Status, Priority = request.Priority, DueDate = request.DueDate, UserId = UserId }; db.Tasks.Add(x); await db.SaveChangesAsync(); return CreatedAtAction(nameof(Get), new { id = x.Id }, Map(x)); }
    [HttpPut("{id:guid}")] public async Task<ActionResult<TaskResponse>> Update(Guid id, TaskRequest request) { var x = await db.Tasks.SingleOrDefaultAsync(x => x.Id == id && x.UserId == UserId); if (x is null) return NotFound(); x.Title = request.Title.Trim(); x.Description = request.Description; x.Status = request.Status; x.Priority = request.Priority; x.DueDate = request.DueDate; x.UpdatedAt = DateTime.UtcNow; await db.SaveChangesAsync(); return Ok(Map(x)); }
    [HttpPatch("{id:guid}/complete")] public async Task<IActionResult> Complete(Guid id) { var x = await db.Tasks.SingleOrDefaultAsync(x => x.Id == id && x.UserId == UserId); if (x is null) return NotFound(); x.Status = TaskStatus.Completed; x.UpdatedAt = DateTime.UtcNow; await db.SaveChangesAsync(); return NoContent(); }
    [HttpDelete("{id:guid}")] public async Task<IActionResult> Delete(Guid id) { var x = await db.Tasks.SingleOrDefaultAsync(x => x.Id == id && x.UserId == UserId); if (x is null) return NotFound(); db.Tasks.Remove(x); await db.SaveChangesAsync(); return NoContent(); }
}