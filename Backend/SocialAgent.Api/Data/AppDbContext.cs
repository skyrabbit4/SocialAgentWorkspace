using Microsoft.EntityFrameworkCore;
using SocialAgent.Api.Models;
namespace SocialAgent.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<SocialPost> SocialPosts { get; set; }
}