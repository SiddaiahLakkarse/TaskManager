using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Contracts;
using TaskManager.Api.Infrastructure.Auth;
using TaskManager.Api.Infrastructure.Persistence;

namespace TaskManager.Api.Controllers;
[ApiController, Route("api/auth")]
public sealed class AuthController(AppDbContext db, ITokenService tokens) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        if (await db.Users.AnyAsync(x => x.Email == email)) return Conflict(new { message = "Email is already registered." });
        var user = new Domain.User { Name = request.Name.Trim(), Email = email, PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password) };
        db.Users.Add(user); await db.SaveChangesAsync();
        return Ok(new AuthResponse(tokens.Create(user), user.Id, user.Name, user.Email));
    }
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        var user = await db.Users.SingleOrDefaultAsync(x => x.Email == request.Email.Trim().ToLowerInvariant());
        if (user is null) return Unauthorized(new { message = "Invalid email or password." });
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) return Unauthorized(new { message = "Invalid email or password." });
        return Ok(new AuthResponse(tokens.Create(user), user.Id, user.Name, user.Email));
    }
}