namespace H3___Secure_Login_With_GUI.Models
{
	public class User
	{
		public readonly string ID;
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
