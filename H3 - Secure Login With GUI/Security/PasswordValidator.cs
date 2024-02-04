using System.Text.RegularExpressions;

namespace H3___Secure_Login_With_GUI.Security
{
    public class PasswordValidator : RegexValidator
    {
        public PasswordValidator() : base(CreateRegexPattern()) {}

        private static Regex CreateRegexPattern()
        {
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            RegexOptions options = RegexOptions.None;
            TimeSpan timeout = new TimeSpan(0, 0, 0, 0, 2000);
            return new Regex(pattern, options, timeout);
        }
    }
}
