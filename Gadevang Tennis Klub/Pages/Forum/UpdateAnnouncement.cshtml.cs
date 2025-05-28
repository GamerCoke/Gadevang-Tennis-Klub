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

        public UpdateAnnouncementModel(IAnnouncementDB announcementDB, IMemberDB memberDB)
        {
            _announcementDB = announcementDB;
            _memberDB = memberDB;
        }

        // Bound properties from the form
        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public string Title { get; set; } = string.Empty;

        [BindProperty]
        public string Text { get; set; } = string.Empty;

        [BindProperty]
        public bool ActualFlag { get; set; }

        public string Type { get; set; } = string.Empty;

        public bool IsAdmin { get; set; }

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

            // Fill in form-bound properties
            Id = announcement.Id;
            Title = announcement.Title;
            Text = announcement.Text;
            Type = announcement.Type;
            ActualFlag = announcement.Actual == true;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var sessionUser = HttpContext.Session.GetString("User");
            if (string.IsNullOrEmpty(sessionUser)) return RedirectToPage("/User/Login");

            var parts = sessionUser.Split('|');
            if (!int.TryParse(parts[0], out int userId)) return RedirectToPage("/User/Login");

            IsAdmin = parts.Length > 1 && bool.TryParse(parts[1], out var admin) && admin;

            var announcement = await _announcementDB.GetAnnouncementByIDAsync(Id);
            if (announcement == null) return NotFound();

            if (announcement.Announcer?.Id != userId && !IsAdmin)
                return Forbid();

            // Update editable fields
            announcement.Title = Title;
            announcement.Text = Text;

            // Preserve type
            announcement.Type = announcement.Type;

            // Only Admins can update Actual for Service announcements
            if (IsAdmin && announcement.Type == "Service")
            {
                announcement.Actual = ActualFlag;
            }

            await _announcementDB.UpdateAnnouncementAsync(announcement);
            return RedirectToPage("/Forum/GetAllAnnouncements");
        }
    }
}
