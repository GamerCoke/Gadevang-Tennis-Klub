using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace Gadevang_Tennis_Klub.Pages.CourtBookings
{
    public class AddParticipantModel : PageModel
    {
        public int BookingId { get; set; }
        public ICourtBookingDB CourtBookingDB { get; set; }
        private IMemberDB MemberDB { get; set; }
        public AddParticipantModel(ICourtBookingDB courtBookingDB, IMemberDB memberDB)
        {
            CourtBookingDB = courtBookingDB;
            MemberDB = memberDB;
        }
        public async Task<IActionResult> OnGet(int bookingId)
        {
            string? user = HttpContext.Session.GetString("User");
            BookingId = bookingId;
            int bookerId = (int)(await CourtBookingDB.GetCourtBookingByIDAsync(bookingId)).Member_ID;
            if (user == null || bookerId != int.Parse(HttpContext.Session.GetString("User").Split('|')[0]))
                return RedirectToPage(@"/User/Login");
            else
                return Page();
        }
        public async Task<IActionResult> OnPost(int bookingId, int memberId)
        {
            await CourtBookingDB.AddPartisipantAsync(bookingId, memberId);
            BookingId = bookingId;
            return Page();
        }
        public async Task<IEnumerable<IMember>> GetViableParticipants()
        {
            List<IMember> participants = (await CourtBookingDB.GetCourtBookingByIDAsync(BookingId)).Participants.ToList();
            IEnumerable<IMember> members = (await MemberDB.GetAllMembersAsync()).Where(m=>m.Id != int.Parse(HttpContext.Session.GetString("User").Split('|')[0])); ;
            
            List<IMember> result = new();
            foreach (IMember member in members)
                if (!participants.ConvertAll(m=>m.Id).Contains(member.Id))
                    result.Add(member);

            return result;
        }
    }
}
