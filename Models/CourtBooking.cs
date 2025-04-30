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
        public TimeOnly Time { get; set; }

        public int ID { get; set; }

        [BindProperty]
        public IReadOnlyList<IMember> Participants { get; set; }

        [BindProperty]
        public int? Team_ID { get; set; }

        [BindProperty]
        public int? Member_ID { get; set; }

        [BindProperty]
        public int? Event_ID { get; set; }

        public CourtBooking()
        {
            
        }
    }
}
