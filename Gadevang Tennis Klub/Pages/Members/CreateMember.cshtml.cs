using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.Members
{
    public class CreateMemberModel : PageModel
    {
        [BindProperty]
        public Member Member { get; set; }

        [BindProperty] 
        public IFormFile? Photo { get; set; }

        public string? Message;
        private IMemberDB _db;
        private IWebHostEnvironment _webHostEnvironment;
        public CreateMemberModel(IMemberDB db, IWebHostEnvironment webHostEnvironment)
        {
            Message = null;
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult OnGet()
        {
            string? user = HttpContext.Session.GetString("User");
            if (user == null)
                return RedirectToPage(@"/User/Login");
            else if (!bool.Parse(user.Split('|')[1]))
                return RedirectToPage(@"/Members/GetAllMembers");

            return Page();
        }
        public async Task<IActionResult> OnPostCreate()
        {
            Member.Image = ProcessUploadedFile();
            IMember member = Member;
            if (ModelState.IsValid && await _db.CreateMemberAsync(member))
                return RedirectToPage(@"/Members/GetAllMembers");
            else
            {
                Message = "Medlem blev ikke oprettet";
                return Page();
            }
        }

        private string? ProcessUploadedFile()
        {
            string? uniqueFileName = null;
            if (Photo != null)
            {
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Photo.FileName;
                using (var fileStream = new FileStream(Path.Combine(Path.Combine(_webHostEnvironment.WebRootPath, @"Images"), uniqueFileName), FileMode.Create))
                {
                    Photo.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}

