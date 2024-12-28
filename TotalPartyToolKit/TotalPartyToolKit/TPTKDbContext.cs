using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

public class TPTKDbContext : DbContext {
  public TPTKDbContext(DbContextOptions<TPTKDbContext> options) : base(options) { }

  public DbSet<User> Users { get; set; }
}

public class User {
  public int Id { get; set; }
  public string Username { get; set; }
  public string PasswordHash { get; set; }
  public string Email { get; set; }
  public DateTime CreatedAt { get; set; }
}