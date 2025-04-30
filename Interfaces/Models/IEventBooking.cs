namespace Gadevang_Tennis_Klub.Interfaces.Models
{
    public interface IEventBooking
    {
        int ID { get; set; }
        int MemberID { get; set; }
        int EventID { get; set; }
    }
}
