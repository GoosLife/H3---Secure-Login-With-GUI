using H3___Secure_Login_With_GUI.Models;

namespace H3___Secure_Login_With_GUI.DataAccess
{
	public interface IDbAccess
	{
		public Task CreateUser(LoginModel user);
		public Task<User?> LogInUser(LoginModel login);
		public Task<User?> GetUser(string username);
	}
}
