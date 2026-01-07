using System;
using System.Collections.Generic;
using System.Text;

namespace TaskFlow.Application.DTOs.Auth
{
    public class RegisterRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
    }
}
