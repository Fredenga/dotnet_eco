﻿using System.ComponentModel.DataAnnotations;

namespace AuthApi.Application.DTOs
{
    public record AppUserDTO
    (
        int ID,
        [Required] string Name,
        [Required] string Telephone,
        [Required] string Address,
        [Required, EmailAddress] string Email,
        [Required] string Password,
        [Required] string Role
    );
}
