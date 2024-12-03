using System.ComponentModel.DataAnnotations;

namespace AuthApi.Application.DTOs
{
    public record AppUserDTO : GetUserDTO
    {
        public AppUserDTO(int ID, [Required] string Name, [Required] string Telephone, [Required] string Address, [EmailAddress, Required] string Email, [Required] string Role)
            : base(ID, Name, Telephone, Address, Email, Role)
        {
        }

        [Required]
        public string? Password { get; }
    }
}
