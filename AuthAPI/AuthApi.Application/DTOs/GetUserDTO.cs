using System.ComponentModel.DataAnnotations;

namespace AuthApi.Application.DTOs
{
    public record GetUserDTO
    (
        int ID,
        [Required] string Name,
        [Required] string Telephone,
        [Required] string Address,
        [Required, EmailAddress] string Email,
        
        [Required] string Role
    );
}
