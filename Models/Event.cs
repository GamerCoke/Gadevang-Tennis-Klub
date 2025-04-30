using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Models
{
    public class Event : IEvent
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public TimeOnly End { get; set; }
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
