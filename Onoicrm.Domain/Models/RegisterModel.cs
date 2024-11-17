using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Onoicrm.Domain.Entities;

namespace Onoicrm.Domain.Models;

public class RegisterModel
{
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    public UserProfile UserProfile { get; set; }

    public ICollection<IdentityRole> Roles { get; set; } = new List<IdentityRole>();
    public ICollection<Clinic> Clinics { get; set; } = new List<Clinic>();

}