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
        private Dictionary<string, List<string>> UserMemoryMap;


        public Task CreateUser(LoginModel user)
        {
            try
            {
                user.Password = EncryptionService.Encrypt(user.Password);

                user.User = new User(new Guid().ToString(), user.User.Username, new List<string>());
                Users.Add(user);
                return Task.CompletedTask;
            }
            catch (FormatException e)
            {
                throw new FormatException(e.Message);
            }
        }

        public async Task<User?> LogInUser(LoginModel login)
        {
            if (Users.Any(u => u.User.Username == login.User.Username))
            {
                var user = Users.First(u => u.User.Username == login.User.Username);

                var memories = await GetMemories(user.User.ID);

                if (EncryptionService.Verify(user.Password, login.Password))
                {
                    return await Task.FromResult<User?>(new User(user.User.ID, user.User.Username, new List<string>()));
                }
            }
            return await Task.FromResult<User?>(null);
        }

        public Task<List<string>> GetMemories(string userId)
        {
            // Find all memories from this user ID
            return Task.FromResult(UserMemoryMap[userId]);
        }

        public async Task<User?> GetUser(string username)
        {
            User? user = Users.FirstOrDefault(u => u.User.Username == username)?.User;

            if (user == null)
                return await Task.FromResult<User?>(null);

            return await Task.FromResult<User?>(new User(user.ID, user.Username, await GetMemories(user.ID)));
        }

        public void UpdateMemories(User user)
        {
            // Replace the memories for this user in the dictionary
            UserMemoryMap[user.ID] = user.Memories;
        }
    }
}
