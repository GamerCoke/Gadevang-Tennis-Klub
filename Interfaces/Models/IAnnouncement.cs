using System.ComponentModel.DataAnnotations;

namespace Gadevang_Tennis_Klub.Interfaces.Models
{
    public interface IAnnouncement
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(32, ErrorMessage = "Title must be 32 characters or fewer.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Text is required.")]
        [StringLength(1024, ErrorMessage = "Text must be 1024 characters or fewer.")]
        public string Text { get; set; }
        public DateTime UploadTime { get; set; }
        public string Type { get; set; }
        public IMember Announcer { get; set; }
        public bool? Actual { get; set; }
    }
}
