using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H3___Secure_Login_With_GUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public string Message { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Message = "Regular get";
        }

        public IActionResult OnPostSetAuthCookie()
        {
			Message = "Auth cookie get";

            // Set a cookie in the browser with the name "auth" and the value of an authorization string
            Response.Cookies.Append("auth", "myAuthString");

            return Page();
        }
    }
}
