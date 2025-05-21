using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Gadevang_Tennis_Klub.Pages.Forum
{
    public class UpdateAnnouncementModel : PageModel
    {
        private readonly IAnnouncementDB _announcementDB;
        private readonly IMemberDB _memberDB;

        [BindProperty]
        public Announcement AnnouncementToUpdate { get; set; } = new();

        public bool IsAdmin { get; set; }

        public UpdateAnnouncementModel(IAnnouncementDB announcementDB, IMemberDB memberDB)
        {
            _announcementDB = announcementDB;
            _memberDB = memberDB;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var sessionUser = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(sessionUser)) return RedirectToPage("/User/Login");

            var parts = sessionUser.Split('|');
            if (!int.TryParse(parts[0], out int userId)) return RedirectToPage("/User/Login");

            IsAdmin = parts.Length > 1 && bool.TryParse(parts[1], out var admin) && admin;

            var announcement = await _announcementDB.GetAnnouncementByIDAsync(id);
            if (announcement == null) return NotFound();

            if (announcement.Announcer?.Id != userId && !IsAdmin)
                return Forbid();

            AnnouncementToUpdate = (Announcement)announcement;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var sessionUser = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(sessionUser)) return RedirectToPage("/User/Login");

            var parts = sessionUser.Split('|');
            if (!int.TryParse(parts[0], out int userId)) return RedirectToPage("/User/Login");

            IsAdmin = parts.Length > 1 && bool.TryParse(parts[1], out var admin) && admin;

            var original = await _announcementDB.GetAnnouncementByIDAsync(AnnouncementToUpdate.Id);
            if (original == null) return NotFound();

            if (original.Announcer?.Id != userId && !IsAdmin) return Forbid();

            // Enforce permission restrictions
            AnnouncementToUpdate.Announcer = original.Announcer;
            AnnouncementToUpdate.UploadTime = original.UploadTime;
            if (!IsAdmin)
                AnnouncementToUpdate.Actual = original.Actual;

            await _announcementDB.UpdateAnnouncementAsync(AnnouncementToUpdate);
            return RedirectToPage("/Forum/GetAllAnnouncements");
        }
    }
}
