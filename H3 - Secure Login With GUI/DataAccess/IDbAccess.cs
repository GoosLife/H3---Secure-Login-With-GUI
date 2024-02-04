using H3___Secure_Login_With_GUI.Models;

namespace H3___Secure_Login_With_GUI.DataAccess
{
	public interface IDbAccess
	{
		public Task CreateUser(string username, string password);
		public Task<User?> LogInUser(string username, string password);
		public void AddMemory(string username, string memory, string token);
	}
}
