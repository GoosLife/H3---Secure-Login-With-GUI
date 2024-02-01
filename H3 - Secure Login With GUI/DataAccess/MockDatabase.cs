using H3___Secure_Login_With_GUI.Models;
using H3___Secure_Login_With_GUI.Security;
using Microsoft.AspNetCore.Identity;

namespace H3___Secure_Login_With_GUI.DataAccess
{
	public class MockDatabase : IDbAccess
	{
		private static readonly Lazy<MockDatabase> mockDatabase = new Lazy<MockDatabase>(() => new MockDatabase());
		
		public static MockDatabase Instance { get { return mockDatabase.Value; } }
		
		private MockDatabase()
		{
			Users = new List<LoginModel>();
		}

		private List<LoginModel> Users;
		
		
		public Task CreateUser(LoginModel user)
		{
			try
			{
				user.Password = EncryptionService.Encrypt(user.Password);
				Users.Add(user);
				return Task.CompletedTask;
			}
			catch (FormatException e)
			{
				throw new FormatException(e.Message);
			}
		}

		public Task<User?> LogInUser(LoginModel login)
		{
			if (Users.Any(u => u.Username == login.Username))
			{
				var user = Users.First(u => u.Username == login.Username);
				if (EncryptionService.Verify(user.Password, login.Password))
				{
					return Task.FromResult<User?>(new User { Username = user.Username });
				}
			}
			return Task.FromResult<User?>(null);
		}

		public Task<User?> GetUser(string username)
		{
			string? foundUsername = Users.FirstOrDefault(u => u.Username == username)?.Username;

			if (foundUsername == null)
				return Task.FromResult<User?>(null);

			return Task.FromResult<User?>(new User { Username = foundUsername });
		}
	}
}
