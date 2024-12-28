using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase {
  private readonly TPTKDbContext _context;

  public AuthController(TPTKDbContext context) {
    _context = context;
  }

  [HttpPost("login")]
  public async Task<IActionResult> Login([FromBody] LoginRequest request) {
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

    if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.PasswordHash)) {
      return Unauthorized(new { message = "Invalid username or password" });
    }

    return Ok(new { message = "Login successful" });
  }
}

public class LoginRequest {
  public string Username { get; set; }
  public string Password { get; set; }
}
