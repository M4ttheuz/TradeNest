using System.ComponentModel.DataAnnotations;

public class UserReview
{
    public int Id { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    public string Comment { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int TargetUserId { get; set; }
    public User TargetUser { get; set; } = null!;

    public int AuthorId { get; set; }
    public User Author { get; set; } = null!;
}
