using H3___Secure_Login_With_GUI.DataAccess;
using H3___Secure_Login_With_GUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata.Ecma335;

namespace H3___Secure_Login_With_GUI.Pages
{
	public class LoginPageModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public string LoginMessage = "Log in:";
		public string LoginError = "";
		public string LoginMessageSuccess = "";

		[BindProperty]
		public LoginModel Login { get; set; }

		// Login does not get a value here, because the value must be obtained when logging in.
		// If the login fails, the value is null, and the model state is invalid.
		// This is the intended behavior.
		public LoginPageModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{
			LoginMessage = "Log ind";
		}

		public void OnGetSuccessfulRegister()
		{
			LoginMessageSuccess = "Succesfully registered! You can now log in.";
		}

		public async void OnPostLogin()
		{
			if (!ModelState.IsValid)
			{
				LoginError = "Invalid login.";
				return;
			}

			// Check if the user exists in the database
			var user = await MockDatabase.Instance.LogInUser(Login);

			if (user == null)
			{
				LoginError = "Invalid login.";
				Response.StatusCode = 401;
			}
			else
			{
				// Set a cookie in the browser with the name "auth" and the value of an authorization string
				Response.Cookies.Append("auth", "User successfully logged in");
				Response.Cookies.Append("username", user.Username);

				// Set successful login message for testing
				LoginMessage = "Succesfully logged in!";
				
				// Send user to the index page
				Response.Redirect("Index");
			}
		}
	}
}
