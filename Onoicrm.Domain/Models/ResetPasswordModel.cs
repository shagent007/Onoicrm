using System.ComponentModel.DataAnnotations;

namespace Onoicrm.Domain.Models;

public class ResetPasswordModel
{

    [Required]
    public string Email { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 5)]
    public string NewPassword { get; set; }

}