using System.ComponentModel.DataAnnotations;

namespace H3___Secure_Login_With_GUI.Models
{
	public class LoginModel
	{
		[Required(ErrorMessage = "Invalid username.")]
		[StringLength(20), MinLength(3)]
		public User User { get; set; }

		[Required(ErrorMessage = "Invalid password.")]
		[MinLength(8, ErrorMessage = "Invalid password.")]
		public string Password { get; set; }

		public LoginModel() { }

		public LoginModel(User user, string password)
		{
			User = user;
			Password = password;
		}
	}
}
