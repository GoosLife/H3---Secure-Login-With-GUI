using System.Text.RegularExpressions;

namespace H3___Secure_Login_With_GUI.Security
{
    public class RegexValidator
    {
        private Regex Regex;

        protected RegexValidator() { }

        public RegexValidator(Regex regex) 
        {
            Regex = regex;
        }

        public bool Validate(string input)
        {
            try
            {
                Match match = Regex.Match(input);
                return match.Success;
            }
            catch(RegexMatchTimeoutException e)
            {
                throw new RegexMatchTimeoutException("Took too long to validate input", e);
            }
        }
    }
}
