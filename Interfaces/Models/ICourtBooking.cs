namespace Gadevang_Tennis_Klub.Interfaces.Models
{
    public interface ICourtBooking
    {
        public DateOnly Date { get; set; }
        public int Court_ID { get; set; }
        public TimeOnly Time { get; set; }
        public int ID { get; set; }
        public IReadOnlyList<IMember>? Participants { get; set; }
        public int? Team_ID { get; set; }
        public int? Member_ID { get; set; }
        public int? Event_ID { get; set; }

        
    }
}
