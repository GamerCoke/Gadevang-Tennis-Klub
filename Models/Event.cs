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
        public int? Capacity { get; set; }

        public Event(string title, string description, DateTime start, TimeOnly end, string location, int? capacity)
        {
            Title = title;
            Description = description;
            Start = start;
            End = end;
            Location = location;
            Capacity = capacity;
        }

        public Event(int id, string title, string description, DateTime start, TimeOnly end, string location, int? capacity)
        {
            ID = id;
            Title = title;
            Description = description;
            Start = start;
            End = end;
            Location = location;
            Capacity = capacity;
        }

        public override string ToString()
        {
            return $"ID: {ID}, Title: {Title}, Start: {Start}, End: {End}, Location: {Location}, Capacity: {(Capacity == null ? "null" : Capacity)}";
            
            // Commented out description while testing, because too long.
            //return $"ID: {ID}, Title: {Title}, Description: {Description}, Start: {Start}, End: {End}, Location: {Location}, Capacity: {(Capacity == null ? 0 : Capacity)}";
        }
    }
}
