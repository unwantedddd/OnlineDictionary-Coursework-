using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineDictionary.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public required string Name { get; set; }
        [MaybeNull]
        public string? Phone { get; set; }
    }
}
