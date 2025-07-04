﻿using System.ComponentModel.DataAnnotations;

namespace RestaurationAPI.Models
{
    public class RegisterUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
       // public string FirstName { get; set; }
        //public string LastName { get; set; }
        public string confirmPassword { get; set; }
        public string? Nationality { get; set; }
        public DateTime? DateOfBirth { get; set; }


        public int RoleId { get; set; } = 1;
    }
}
