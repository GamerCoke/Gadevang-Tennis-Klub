using Gadevang_Tennis_Klub.Interfaces.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Gadevang_Tennis_Klub.Models
{
    public class Announcement : IAnnouncement
    {
        public int Id { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Titel er påkrævet")]
        [StringLength(32, ErrorMessage = "Titel må ikke være mere end 8 karakterer lang")]
        public string Title { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Text er påkrævet")]
        [StringLength(1024, ErrorMessage = "Text må ikke være mere end 1024 karakterer lang")]
        public string Text { get; set; }

        public DateTime UploadTime { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Type er påkrævet")]
        [StringLength(16, ErrorMessage = "Type må ikke være mere end 16 karakterer lang")]
        public string Type { get; set; }
        public IMember Announcer { get; set; }

        public Announcement(IMember announcer)
        {
            Announcer = announcer;
            Id = 0;
            Title = string.Empty;
            Text = string.Empty;
            UploadTime = DateTime.Now;
            Type = string.Empty;
        }
        public Announcement(int id, string title, string text, string uploadTime, string type, IMember announcer)
        {
            Announcer = announcer;
            Id = id;
            Title = title;
            Text = text;
            UploadTime = DateTime.Parse(uploadTime);
            Type = type;
        }
    }
}
