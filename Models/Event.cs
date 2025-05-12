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

        [Required(ErrorMessage = "Beskrivelse er påkrævet")]
        [StringLength(1024, ErrorMessage = "Beskrivelse må ikke være mere end 1024 karakterer lang")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Starttidspunkt er påkrævet")]
        [CustomValidation(typeof(Event), nameof(ValidateStart))]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "Sluttidspunkt er påkrævet")]
        [CustomValidation(typeof(Event), nameof(ValidateEnd))]
        public TimeOnly End { get; set; }

        [Required(ErrorMessage = "Lokation er påkrævet")]
        [StringLength(64, ErrorMessage = "Lokation må ikke være mere end 64 karakterer lang")]
        public string Location { get; set; }

        public int? Capacity { get; set; }


        public Event()
        {
            
        }
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

        public static ValidationResult? ValidateStart(DateTime start, ValidationContext context) // Must be static to access it at runtime
        {
            DateTime minDate = DateTime.Today;
            DateTime maxDate = DateTime.Today.AddYears(5);

            return (start >= minDate && start <= maxDate) ? ValidationResult.Success : new ValidationResult($"Starttidspunktet skal være mellem {minDate.ToShortDateString()} og {maxDate.ToShortDateString()}");
        }

        public static ValidationResult? ValidateEnd(TimeOnly end, ValidationContext context) // Must be static to access it at runtime
        {
            return (end >= TimeOnly.FromDateTime(((Event)context.ObjectInstance).Start)) ? ValidationResult.Success : new ValidationResult("Sluttidspunkt skal enten være det samme som starttidspunktet, eller senere");
        }
    }
}

