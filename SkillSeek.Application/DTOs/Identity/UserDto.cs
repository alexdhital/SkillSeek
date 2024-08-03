namespace SkillSeek.Application.DTOs.Identity;

public class UserDto
{
    public Guid Id { get; set; }
    
    public string FullName { get; set; }    

    public string PhoneNumber { get; set; }    

    public string? ImageURL { get; set; }

    public string? State { get; set; }

    public string? Address { get; set; }
}