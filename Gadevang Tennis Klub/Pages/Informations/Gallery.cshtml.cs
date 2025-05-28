using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.Informations
{
    public class GalleryModel : PageModel
    {
        public List<string> GalleryImages { get; set; }

        public void OnGet()
        {
            GalleryImages = new List<string>()
            {
                { "../Images/458498773_10233418641630953_2962686801713532471_n.png" },
                { "../Images/GadevangBillede1.png" },
                { "../Images/458613537_10233418642070964_4565044780875557319_n.png" },
                { "../Images/GadevangBillede2.png" },
                { "../Images/458708467_10233418640790932_7845244199252550932_n.png" },
                { "../Images/GadevangBillede3.png" },
                { "../Images/458735634_10233418641310945_4579495833375290972_n.png" },
                { "../Images/GadevangBillede4.png" },
                { "../Images/459002070_10233432201689946_6284068253120155534_n.png" },
                { "../Images/463054478_10233804884366780_3919973871656113720_n.png" },
                { "../Images/492543230_10231202541620034_1346593014046891145_n.png" },
                { "../Images/493516899_29730271586558097_7419098826387616638_n.png" },
            };
        }
    }
}
