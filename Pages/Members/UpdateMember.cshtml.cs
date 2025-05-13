using Gadevang_Tennis_Klub.Interfaces.Models;
using Gadevang_Tennis_Klub.Interfaces.Services;
using Gadevang_Tennis_Klub.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Gadevang_Tennis_Klub.Pages.Members
{
    public class UpdateMemberModel : PageModel
    {
        public bool IsAdmin;
        [BindProperty]
        public Member Member { get; set; }

        [BindProperty]
        public IFormFile? Photo { get; set; }

        public string? Message;
        public IMemberDB _memberdb;
        private IWebHostEnvironment _webHostEnvironment;
        public UpdateMemberModel(IMemberDB memberdb, IWebHostEnvironment webHostEnvironment)
        {
            Message = null;
            _memberdb = memberdb;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> OnGet(int memberID)
        {
            IsAdmin = false;
            string? user = HttpContext.Session.GetString("User");
            if (user == null)
                return RedirectToPage(@"/User/Login");
            else if (!(bool.Parse(user.Split('|')[1]) || memberID == int.Parse(user.Split('|')[0])))
                return RedirectToPage(@"/Members/GetAllMembers");

            if (user == null)
                return RedirectToPage(@"/User/Login");
            else if (user != null)
                IsAdmin = bool.Parse(user.Split('|')[1]);

            if (!(IsAdmin || memberID == int.Parse(user.Split('|')[0])))
                return RedirectToPage(@"/Members/GetAllMembers");

            try
            {
                Member = (Member)await _memberdb.GetMemberByIDAsync(memberID);
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
            }
            return RedirectToPage(@"/Index");
        }

        public async Task<IActionResult> OnPost(int memberID)
        {
            try
            {
                Member.Image = ProcessUploadedFile();
                if (ModelState.IsValid && await _memberdb.UpdateMemberAsync(Member))
                    return RedirectToPage(@"/Members/GetAllMembers");
                else
                {
                    Message = "Medlem blev ikke oprettet";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                Member = (Member)await _memberdb.GetMemberByIDAsync(memberID);
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
