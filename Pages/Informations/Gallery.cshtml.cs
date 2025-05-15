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
                { "../Images/ImageTemp.png" },
                { "../Images/GadevangBillede1.png" },
                { "../Images/ImageTemp.png" },
                { "../Images/GadevangBillede2.png" },
                { "../Images/ImageTemp.png" },
                { "../Images/GadevangBillede3.png" },
                { "../Images/ImageTemp.png" },
                { "../Images/GadevangBillede4.png" },
                { "../Images/ImageTemp.png" },
            };
        }
    }
}
