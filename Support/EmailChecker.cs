using System.Net.Mail;
using System.Text.RegularExpressions;

namespace CathayDomain
{
    public static class EmailChecker
    {
        // Regular expression for validating an email
        private static readonly Regex EmailRegex = new Regex(
            @"^[^@\s]+@[^@\s]+\.[^@\s]+$", 
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            // Ensure email is within the valid length range
            if (email.Length > 320)
            {
                return false;
            }

            // Check format using regular expression
            if (!EmailRegex.IsMatch(email))
            {
                return false;
            }

            try
            {
                // Validate using MailAddress to handle additional parsing
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}