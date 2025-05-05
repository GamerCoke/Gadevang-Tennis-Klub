using Gadevang_Tennis_Klub.Interfaces.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Gadevang_Tennis_Klub.Models
{
    public class CourtBooking : ICourtBooking
    {
        [BindProperty]
        [Required(ErrorMessage = "Dato er påkrævet")]
        public DateOnly Date { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Bane ID er påkrævet")]
        public int Court_ID { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Tid er påkrævet")]
        public int TimeSlot { get; set; }

        public int ID { get; set; }

        [BindProperty]
        public IReadOnlyList<IMember>? Participants { get; set; }

        [BindProperty]
        public int? Team_ID { get; set; }

        [BindProperty]
        public int? Member_ID { get; set; }

        [BindProperty]
        public int? Event_ID { get; set; }

        public CourtBooking()
        {
        }

        public CourtBooking(int id, int courtID, DateOnly date, int timeSlot, int? teamID, int? memberID, int? eventID)
        {
            ID = id;
            Court_ID = courtID;
            Date = date;
            TimeSlot = timeSlot;
            if ((teamID != null) && (memberID == null) && (eventID == null))
            {
                Team_ID = teamID;
            }
            else if ((teamID == null) && (memberID != null) && (eventID == null))
            {
                Member_ID = memberID;
            }
            else if ((teamID != null) && (memberID == null) && (eventID != null))
            {
                Event_ID = eventID;
            }
            else
                throw new Exception();
        }

        
    }
}
