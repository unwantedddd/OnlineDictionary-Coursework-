using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace OnlineDictionary.Models.ViewModels
{
    public class WordVM
    {
        public IEnumerable<Language>? Languages { get; set; } = new List<Language>();
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(500)]
        public string Description { get; set; }
        public int? LanguageId { get; set; }
        [ForeignKey("LanguageId")]
        [ValidateNever]
        public virtual Language? Language { get; set; }
    }
}
