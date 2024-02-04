using H3___Secure_Login_With_GUI.DataAccess;
using H3___Secure_Login_With_GUI.Models;
using H3___Secure_Login_With_GUI.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace H3___Secure_Login_With_GUI.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty] public string Username { get; set; }

        [Required (ErrorMessage = "Password is required")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
        [BindProperty] public string Password { get; set; }

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
                ViewData["debug"] = "Model state was invalid: ";

                // Print all values from the model state and their error messages
                foreach (var modelState in ModelState.Values)
                {
					foreach (var error in modelState.Errors)
                    {
                        ViewData["debug"] += error.ErrorMessage + " ";
					}
				}

                return Page();
            }

			if (Password != ConfirmPassword)
            {
                ViewData["error"] = "Passwords do not match.";
				return Page();
			}

            ConfirmPassword = ""; // Unset confirm password to remove it from memory

            try
            {
                if (!new UsernameValidator().Validate(Username))
                {
                    RegisterMessage = "Invalid username.";
                    return Unauthorized();
                }

                if (!new PasswordValidator().Validate(Password))
                {
                    RegisterMessage = "Invalid password.";
                    return Unauthorized();
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                _logger.LogWarning("User registration failed. Possible regex timeout attack detected. Error: " + e.Message + "\n" + e.StackTrace + "\n" + e.Source);
                ViewData["error"] = "Registration could not be completed. Please check your data and try again.";
                return Page();
            }

            try 
            {
                _logger.LogInformation("User registration started.");
                await MockDatabase.Instance.CreateUser(Username, Password);
				RegisterMessage = "Registration successful!";
                _logger.LogInformation("User registration completed");

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
