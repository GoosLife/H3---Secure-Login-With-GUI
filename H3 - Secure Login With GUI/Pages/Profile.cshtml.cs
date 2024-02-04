using H3___Secure_Login_With_GUI.Authentication;
using H3___Secure_Login_With_GUI.DataAccess;
using H3___Secure_Login_With_GUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H3___Secure_Login_With_GUI.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly ILogger<ProfileModel> _logger;

        [BindProperty]
        public List<string> TextList { get; set; }

        public ProfileModel(ILogger<ProfileModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            if (Request.Cookies["auth"] == null || JwtAuthenticator.ValidateToken(Request.Cookies["auth"]!) == false)
            {
                Response.StatusCode = 401;
                Response.Redirect("/Login");
                return;
            }
            else
            {
                TextList = new List<string>();

                string jwtToken = Request.Cookies["auth"]!;
                string username = JwtAuthenticator.GetClaim(jwtToken!, Models.User.Claims.Username);

                User? user = MockDatabase.Instance.GetUser(username, jwtToken);

                if (user == null)
                    throw new Exception("Unable to load memories");

                if (user.Memories != null)
                    TextList = user.Memories;
            }
        }


        public void OnPostAddMemory(string memory)
        {
            _logger.LogInformation(memory + " - from logger");
            Console.WriteLine(memory + " - from Console.WriteLine()");

            string? jwtToken = Request.Cookies["auth"];

            if (jwtToken == null)
            {
                throw new UnauthorizedAccessException();
            }
            else
            {
                string username = JwtAuthenticator.GetClaim(jwtToken, Models.User.Claims.Username);
                MockDatabase.Instance.AddMemory(username, memory, jwtToken);
            }

        }
    }
}
