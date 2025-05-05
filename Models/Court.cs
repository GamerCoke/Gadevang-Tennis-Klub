using Gadevang_Tennis_Klub.Interfaces.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Gadevang_Tennis_Klub.Models
{
    public class Court : ICourt
    {
        [BindProperty]
        [Required(ErrorMessage = "Type er påkrævet")]
        [StringLength(32, ErrorMessage = "Type må ikke være mere end 32 karakterer lang")]
        public string Type { get; set; }
        public int? ID { get; set; }
        [BindProperty]
        [Required(ErrorMessage = "Navn er påkrævet")]
        [StringLength(32, ErrorMessage = "Navn må ikke være mere end 32 karakterer lang")]
        public string? Name { get; set; }

        public Court()
        { 
        }

        public Court(int? id, string type, string? name)
        {
            ID = id;
            Type = type;
            Name = name;
        }
    }
}
