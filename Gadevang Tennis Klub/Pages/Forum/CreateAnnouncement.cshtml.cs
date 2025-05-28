using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.Forum
{
    public class CreateAnnouncementModel : PageModel
    {
        private readonly IAnnouncementDB _announcementDB;
        private readonly IMemberDB _memberDB;

        [BindProperty]
        public Announcement NewAnnouncement { get; set; } = new();

        public CreateAnnouncementModel(IAnnouncementDB announcementDB, IMemberDB memberDB)
        {
            _announcementDB = announcementDB;
            _memberDB = memberDB;
        }

        public IActionResult OnGet()
        {
            var sessionUser = HttpContext.Session.GetString("User");

            if (string.IsNullOrEmpty(sessionUser))
            {
                return RedirectToPage("/User/Login");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var sessionUser = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(sessionUser))
            {
                return RedirectToPage("/User/Login");
            }

            var parts = sessionUser.Split('|');
            if (parts.Length < 1 || !int.TryParse(parts[0], out int memberId))
            {
                return RedirectToPage("/User/Login");
            }

            var announcer = await _memberDB.GetMemberByIDAsync(memberId);
            if (announcer == null)
            {
                ModelState.AddModelError(string.Empty, "Could not identify announcer.");
                return Page();
            }

            NewAnnouncement.UploadTime = DateTime.Now;
            NewAnnouncement.Announcer = announcer;

            // Automatically set Actual = true if it's a Service announcement
            if (NewAnnouncement.Type == "Service")
            {
                NewAnnouncement.Actual = true;
            }

            await _announcementDB.CreateAnnouncementAsync(NewAnnouncement);

            return RedirectToPage("/Forum/GetAllAnnouncements");
        }
    }
}
