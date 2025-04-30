using Gadevang_Tennis_Klub.Interfaces.Models;
using System.ComponentModel.DataAnnotations;

namespace Gadevang_Tennis_Klub.Models
{
    public class Event : IEvent
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Titel er påkrævet")]
        [StringLength(64, ErrorMessage = "Titel må ikke være mere end 64 karakterer lang")]
        public string Title { get; set; }

        public string Description { get; set; }
        public DateTime Start { get; set; }
        public TimeOnly End { get; set; }

        [Required(ErrorMessage = "Lokation er påkrævet")]
        [StringLength(64, ErrorMessage = "Lokation må ikke være mere end 64 karakterer lang")]
        public string Location { get; set; }

        public Event(int id, string title, string description, DateTime start, TimeOnly end, string location)
        {
            ID = id;
            Title = title;
            Description = description;
            Start = start;
            End = end;
            Location = location;
        }
    }
}
