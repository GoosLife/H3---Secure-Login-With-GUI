using System.Text.RegularExpressions;

namespace H3___Secure_Login_With_GUI.Security
{
    public class UsernameValidator : RegexValidator
    {
        public UsernameValidator() : base(CreateRegexPattern()) { }
        private static Regex CreateRegexPattern()
        {
            string pattern = @"^[a-zA-Z0-9]*$";
            RegexOptions options = RegexOptions.None;
            TimeSpan timeout = new TimeSpan(0, 0, 0, 0, 2000);
            return new Regex(pattern, options, timeout);
        }
    }
}
