using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace SkillSeek.Application.DTOs.Identity;

public class RegistrationDto
{
    [Required]
    [Display(Name = "Name")]
    public string Name { get; set; }
    
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }
    
    [Required]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }
    
    [Required]
    [Display(Name = "State Address")]
    public string State { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
 
    [Required]
    [Display(Name = "Service")]
    public int ServiceId { get; set; }
    
    [Display(Name = "Profile Image")]
    public IFormFile? ProfileImage { get; set; }

    [Required]
    [Display(Name = "Resume")]
    public IFormFile Resume { get; set; }
    
    [Required]
    [Display(Name = "Certification")]
    public IFormFile Certification { get; set; }
    
    [Display(Name = "Role")]
    public string Role { get; set; }
}