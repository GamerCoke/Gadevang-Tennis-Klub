using Gadevang_Tennis_Klub.Interfaces.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace Gadevang_Tennis_Klub.Models
{
    public class Member : BasePerson, IMember
    {
        [BindProperty]
        [Required(ErrorMessage = "Sex er påkrævet")]
        [StringLength(8, ErrorMessage = "Sex må ikke være mere end 8 karakterer lang")]
        public string Sex { get; set; }

        public bool IsAdmin { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Bio er påkrævet")]
        [StringLength(1024, ErrorMessage = "Bio må ikke være mere end 1024 karakterer lang")]
        public string Bio { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Adresse er påkrævet")]
        [StringLength(256, ErrorMessage = "Adresse må ikke være mere end 256 karakterer lang")]
        public string Address { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Kodeord er påkrævet")]
        [StringLength(256, ErrorMessage = "Kodeord må ikke være mere end 256 karakterer lang")]
        public string Password { get; set; }

        public DateOnly DoB { get; set; }

        public Member() : base()
        {
            Sex = string.Empty;
            IsAdmin = false;
            Bio = string.Empty;
            Address = string.Empty;
            Password = string.Empty;
        }
        public Member(int id, string name, string phone, string email, string? image, string sex, DateOnly doB, bool isAdmin, string bio, string adress, string password)
            : base(id, name, phone, email, image)
        {
            Sex = sex;
            IsAdmin = isAdmin;
            Bio = bio;
            Address = adress;
            Password = password;
            DoB = doB;
        }
    }
}
