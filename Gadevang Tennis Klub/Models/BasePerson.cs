using Gadevang_Tennis_Klub.Interfaces.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Gadevang_Tennis_Klub.Models
{
    public abstract class BasePerson : IBasePerson
    {
        public int Id { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Navn er påkrævet")]
        [StringLength(256, ErrorMessage = "Navn må ikke være mere end 256 karakterer lang")]
        [RegularExpression(
           "^[a-zA-Z ]*$",
            ErrorMessage = "Ugyldigt tegn fundet i Navn, gyldige er: a-z A-Z og mellemrum"
        )]
        public string Name { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Telefonnummer er påkrævet")]
        [StringLength(16, ErrorMessage = "Telefonnummer må ikke være mere end 16 karakterer lang")]
        [RegularExpression(
            "^[+]{1}45 ([0-9]{2}) ([0-9]{2}) ([0-9]{2}) ([0-9]{2})$",
            ErrorMessage = "Telefonnummer skal være i formattet \"+45 nn nn nn nn\""
        )]
        public string Phone { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Email er påkrævet")]
        [StringLength(256, ErrorMessage = "Email må ikke være mere end 256 karakterer lang")]
        [RegularExpression(
            "^([a-zA-Z0-9_\\-\\.]+)@([a-zA-Z0-9_\\-\\.]+)\\.([a-zA-Z]{2,5})$",
            ErrorMessage = "Email er i forkert format"
        )]
        public string Email { get; set; }

        [BindProperty]
        [StringLength(64, ErrorMessage = "Image må ikke være mere end 64 karakterer lang")]
        public string? Image { get; set; }

        public BasePerson()
        {
            Id = 0;
            Name = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            Image = string.Empty;
        }
        public BasePerson(int id, string name, string phone, string email, string? image)
        {
            Id = id;
            Name = name;
            Phone = phone;
            Email = email;
            Image = image;
        }
    }
}
