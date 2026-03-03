namespace SocialAgent.Api.Models; // This MUST match the 'using' in AppDbContext

public class SocialPost
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string Platform { get; set; } = "X";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsPublished { get; set; }
    public string? ExternalId { get; set; }
}