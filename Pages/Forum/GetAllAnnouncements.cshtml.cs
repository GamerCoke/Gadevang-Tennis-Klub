using Microsoft.AspNetCore.Mvc.RazorPages;
using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Services.SQL;
using Microsoft.AspNetCore.Mvc;

public class GetAllAnnouncementsModel : PageModel
{
    private readonly IAnnouncementDB _announcementDB;
    public int CurrentUserId { get; set; }
    public bool IsAdmin { get; set; }

    public List<IAnnouncement> ServiceAnnouncements { get; set; } = new();
    public List<IAnnouncement> PartnerAnnouncements { get; set; } = new();
    public List<IAnnouncement> GeneralAnnouncements { get; set; } = new();

    public GetAllAnnouncementsModel()
    {
        _announcementDB = new AnnouncementDB_SQL();
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var session = HttpContext.Session.GetString("User");

        if (!string.IsNullOrEmpty(session))
        {
            var parts = session.Split('|');
            if (parts.Length >= 2)
            {
                int.TryParse(parts[0], out int id);
                bool.TryParse(parts[1], out bool isAdmin);

                CurrentUserId = id;
                IsAdmin = isAdmin;
            }
        }

        var all = await _announcementDB.GetAllAnnouncementsAsync();
        ServiceAnnouncements = all.Where(a => a.Type == "Service").ToList();
        PartnerAnnouncements = all.Where(a => a.Type == "Partner").ToList();
        GeneralAnnouncements = all.Where(a => a.Type == "General").ToList();

        return Page();
    }
    public async Task<IActionResult> OnPostToggleActualAsync(int id, bool actual)
    {
        var session = HttpContext.Session.GetString("User");

        if (string.IsNullOrEmpty(session) || !bool.TryParse(session.Split('|')[1], out bool isAdmin) || !isAdmin)
            return Unauthorized();

        var all = await _announcementDB.GetAllAnnouncementsAsync();
        var announcement = all.FirstOrDefault(a => a.Id == id);

        if (announcement == null)
            return NotFound();

        announcement.Actual = actual;
        await _announcementDB.UpdateAnnouncementAsync(announcement);

        return RedirectToPage(); // Refresh
    }
    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var session = HttpContext.Session.GetString("User");
        if (string.IsNullOrEmpty(session))
            return RedirectToPage("/User/Login");

        var parts = session.Split('|');
        if (!int.TryParse(parts[0], out int userId))
            return RedirectToPage("/User/Login");

        bool isAdmin = parts.Length > 1 && bool.TryParse(parts[1], out var admin) && admin;

        var announcement = (await _announcementDB.GetAllAnnouncementsAsync())
            .FirstOrDefault(a => a.Id == id);

        if (announcement == null)
            return NotFound();

        // Check permission: Only announcer or admin can delete
        if (announcement.Announcer?.Id != userId && !isAdmin)
            return Forbid();

        bool deleted = await _announcementDB.DeleteAnnouncementAsync(id);

        if (!deleted)
            ModelState.AddModelError(string.Empty, "Sletning mislykkedes.");

        return RedirectToPage(); // Refresh the list
    }


}
