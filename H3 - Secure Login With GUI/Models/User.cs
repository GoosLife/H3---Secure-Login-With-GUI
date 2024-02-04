using System.ComponentModel.DataAnnotations;

namespace H3___Secure_Login_With_GUI.Models
{
	public class User
	{
		/// <summary>
		/// Represents the claims that can be stored in a JWT token for a user.
		/// This is used to avoid magic strings in the code.
		/// </summary>
		public static class Claims
		{
            public const string Username = "username";
            public const string ID = "guid";
        }

		public readonly string ID;

		[Required (ErrorMessage = "Username is required")]
		[MinLength(3, ErrorMessage = "Username must be at least 3 characters")]
		[MaxLength(20, ErrorMessage = "Username must be at most 20 characters")]
		public string Username { get; set; }
		public List<string> Memories { get; private set; }

		public User(string id, string username, List<string> memories)
		{
            ID = id;
            Username = username;
            Memories = memories;
        }
	}
}
