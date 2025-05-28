using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private IEventDB _eventDB;
        private IAnnouncementDB _announcementDB;

        public IEvent? Event { get; set; }
        public List<IAnnouncement> ActualServiceAnnouncements { get; set; }


        public IndexModel(ILogger<IndexModel> logger, IEventDB eventDB, IAnnouncementDB announcementDB)
        {
            _logger = logger;
            _eventDB = eventDB;
            _announcementDB = announcementDB;
        }

        public async Task OnGetAsync()
        {
            try
            {
                List<IEvent> events = await _eventDB.GetAllEventsAsync();
                Event = events.Where(e => e.Start > DateTime.Now).OrderBy(e => e.Start).FirstOrDefault();

                ActualServiceAnnouncements = (await _announcementDB.GetAnnouncementsByTypeAsync("Service")).Where(a => a.Actual == true).ToList();
            }
            catch(Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
        }
    }
}
