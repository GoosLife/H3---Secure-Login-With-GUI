using H3___Secure_Login_With_GUI.DataAccess;
using H3___Secure_Login_With_GUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace H3___Secure_Login_With_GUI.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty] public LoginModel Register { get; set; }
        [BindProperty] public string ConfirmPassword { get; set; }
		
        private readonly ILogger<RegisterModel> _logger;
        public string RegisterMessage = "";

        public RegisterModel(ILogger<RegisterModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            RegisterMessage = "Register";
        }

        public async Task<IActionResult> OnPostRegister()
        {
            if (!ModelState.IsValid)
            {
                ViewData["error"] = "Registration could not be completed. Please check your data and try again.";
                return Page();
            }

			if (Register.Password != ConfirmPassword)
            {
                ViewData["error"] = "Passwords do not match.";
				return Page();
			}

            ConfirmPassword = ""; // Unset confirm password to remove it from memory

            try 
            {
                _logger.LogInformation("User registration started.");
                await MockDatabase.Instance.CreateUser(Register);
				RegisterMessage = "Registration successful!";

                // Set a cookie in the browser with the name "auth" and the value of an authorization string
				Response.Cookies.Append("auth", "authString");
				Response.Cookies.Append("username", Register.Username);

                // Send user to the login page
                return RedirectToPage("Index", "SuccessfulRegister");
			}
			catch (FormatException e)
            {
                _logger.LogError("User registration failed. Error: " + e.Message + "\n" + e.StackTrace + "\n" + e.Source);
                ViewData["error"] = "Registration could not be completed. Please check your data and try again.";
				return Page();
			}
        }
    }
}
