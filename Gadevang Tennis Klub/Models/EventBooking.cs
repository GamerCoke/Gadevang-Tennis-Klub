using Gadevang_Tennis_Klub.Interfaces.Models;

namespace Gadevang_Tennis_Klub.Models
{
    public class EventBooking : IEventBooking
    {
        public int ID { get; set; }
        public int MemberID { get; set; }
        public int EventID { get; set; }

        public EventBooking(int memberId, int eventId)
        {
            MemberID = memberId;
            EventID = eventId;
        }

        public EventBooking(int id, int memberId, int eventId)
        {
            ID = id;
            MemberID = memberId;
            EventID = eventId;
        }
    }
}
