using Microsoft.AspNetCore.Identity;

namespace SkillSeek.Domain.Entities.Identity;

public class User : IdentityUser<Guid>
{
    public string Name { get; set; }

    public string? Address { get; set; }

    public string? State { get; set; }

    public string? ImageURL { get; set; }
}