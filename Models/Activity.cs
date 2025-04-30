using Gadevang_Tennis_Klub.Interfaces.Models;
using System.ComponentModel.DataAnnotations;

namespace Gadevang_Tennis_Klub.Models
{
    public class Activity : IActivity
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Titel er påkrævet")]
        [StringLength(32, ErrorMessage = "Titel må ikke være mere end 32 karakterer lang")]
        public string Title { get; set; }

        public string Description { get; set; }
        public DateTime Start { get; set; }
        public TimeOnly End { get; set; }

        public Activity(int id, string title, string description, DateTime start, TimeOnly end)
        {
            ID = id;
            Title = title;
            Description = description;
            Start = start;
            End = end;
        }
    }
}
