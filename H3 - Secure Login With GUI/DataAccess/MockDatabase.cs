using H3___Secure_Login_With_GUI.Authentication;
using H3___Secure_Login_With_GUI.Models;
using H3___Secure_Login_With_GUI.Security;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace H3___Secure_Login_With_GUI.DataAccess
{
	public class MockDatabase : IDbAccess
	{
		private static readonly Lazy<MockDatabase> mockDatabase = new Lazy<MockDatabase>(() => new MockDatabase());

		public static MockDatabase Instance { get { return mockDatabase.Value; } }

		private MockDatabase()
		{
			Users = new List<User>();
			PasswordMap = new Dictionary<string, string>();
		}

		private List<User> Users;
		private Dictionary<string, string> PasswordMap;


		public Task CreateUser(string username, string password)
		{
			try
			{
				password = EncryptionService.Hash(password);

				User user = new User(new Guid().ToString(), username, new List<string>());
				Users.Add(user);

				try
				{
					PasswordMap.Add(user.ID, password);
				}
				catch (Exception e)
				{
					// Remove user because something went wrong
					Users.Remove(user);
					throw new Exception("Failed to store users password: " + e.Message);
				}

				return Task.CompletedTask;
			}
			catch (FormatException e)
			{
				throw new FormatException(e.Message);
			}
		}

		public async Task<User?> LogInUser(string username, string password)
		{
			if (Users.Any(u => u.Username == username))
			{
				User user = Users.First(u => u.Username == username);

				string hashedPassword = PasswordMap[user.ID];

				if (EncryptionService.Verify(hashedPassword, password))
				{
					return await Task.FromResult<User?>(user);
				}
			}
			return await Task.FromResult<User?>(null);
		}

		public User? GetUser(string username, string token)
		{
			if (IsLoggedIn(token) && JwtAuthenticator.GetClaim(token, Models.User.Claims.Username) == username)
			{
				return Users.FirstOrDefault(u => u.Username == username);
			}
			else
			{
				return null;
			}
        }

		public void AddMemory(string username, string memory, string token)
		{
			if (IsLoggedIn(token))
			{
				User user = Users.First(u => u.Username == username);
				user.Memories.Add(memory);
				return;
			}
			else
			{
				throw new UnauthorizedAccessException("User is not logged in.");
			}
		}

		private bool IsLoggedIn(string token)
		{
			if (token != null)
			{
				return true;
			}
			return false;
		}
	}
}
