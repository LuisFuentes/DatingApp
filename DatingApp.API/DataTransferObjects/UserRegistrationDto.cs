﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.DataTransferObjects
{
    public class UserRegistrationDto
    {
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage ="")]
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "Password must be between four to eight characters.")]
        public string Password { get; set; }
    }
}
