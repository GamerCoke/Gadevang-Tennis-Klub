using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Models
{
    public class Activity : IActivity
    {
        public int ID { get; set; }
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
