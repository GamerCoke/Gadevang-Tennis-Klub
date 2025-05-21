using Microsoft.AspNetCore.Mvc.RazorPages;
using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Services.SQL;

public class GetAllAnnouncementsModel : PageModel
{
    private readonly IAnnouncementDB _announcementDB;

    public List<IAnnouncement> ServiceAnnouncements { get; set; } = new();
    public List<IAnnouncement> PartnerAnnouncements { get; set; } = new();
    public List<IAnnouncement> GeneralAnnouncements { get; set; } = new();

    public GetAllAnnouncementsModel()
    {
        _announcementDB = new AnnouncementDB_SQL();
    }

    public async Task OnGetAsync()
    {
        var all = await _announcementDB.GetAllAnnouncementsAsync();

        ServiceAnnouncements = all.Where(a => a.Type == "Service").ToList();
        PartnerAnnouncements = all.Where(a => a.Type == "Partner").ToList();
        GeneralAnnouncements = all.Where(a => a.Type == "General").ToList();
    }
}
