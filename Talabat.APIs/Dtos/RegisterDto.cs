﻿using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[\\W_]).{8,}$",
            ErrorMessage = "Password must be at least 6 characters long and contain only alphanumeric characters.")]
        public string Password { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
    }
}