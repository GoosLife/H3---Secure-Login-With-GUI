using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H3___Secure_Login_With_GUI.Pages
{
    public class ProfileModel : PageModel
    {
        [BindProperty]
        public List<string> TextList { get; set; }

        public void OnGet()
        {
            TextList = new List<string>();
        }

        public IActionResult OnPost()
        {
            var textInput = Request.Form["textInput"];
            if (!string.IsNullOrEmpty(textInput))
            {
                TextList.Add(textInput);
            }
            return Page();
        }
    }
}
