namespace Gadevang_Tennis_Klub.Interfaces.Models
{
    public interface IEvent
    {
        int ID { get; set; }
        string Title { get; set; }
        string Description { get; set; }
        DateTime Start { get; set; }
        TimeOnly End { get; set; }
        string Location { get; set; }
        int? Capacity { get; set; }
    }
}
